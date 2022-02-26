using System.Collections;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using RizSoft.Acme.Application;

namespace RizSoft.Acme.Services;

public class QueryBaseRepository<T> : IQueryBaseRepository<T>
where T : class
{
    protected IDbContextFactory<AcmeContext> CtxFactory { get; }

    public QueryBaseRepository(IDbContextFactory<AcmeContext> ctxFactory)
    {
        this.CtxFactory = ctxFactory;
    }

    public IQueryable<T> Query
    {
        get
        {
            var AcmeContext = CtxFactory.CreateDbContext();
            return new DisposableQueryable<T>(AcmeContext, AcmeContext.Set<T>());
        }
    }

}

internal class DisposableQueryable<T> : IOrderedQueryable<T>, IAsyncEnumerable<T>
{
    private readonly AcmeContext _context;
    private readonly IQueryable<T> _queryableImplementation;

    public DisposableQueryable(AcmeContext context, IQueryable<T> innerQueryable)
    {
        _context = context;
        _queryableImplementation = innerQueryable;
        Provider = new DisposableQueryProvider(context, innerQueryable.Provider);
    }

    public IEnumerator<T> GetEnumerator()
    {
        try
        {
            using var enumerator = _queryableImplementation.GetEnumerator();
            while (enumerator.MoveNext())
            {
                yield return enumerator.Current;
            }
        }
        finally
        {
            _context.Dispose();
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public Type ElementType => _queryableImplementation.ElementType;

    public Expression Expression => _queryableImplementation.Expression;

    public IQueryProvider Provider { get; }

    public async IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
    {
        try
        {
            if (_queryableImplementation is not IAsyncEnumerable<T> ae) throw new NotSupportedException();

            await using var enumerator = ae.GetAsyncEnumerator(cancellationToken);
            while (await enumerator.MoveNextAsync())
            {
                yield return enumerator.Current;
            }
        }
        finally
        {
            await _context.DisposeAsync();
        }
    }
}

internal class DisposableQueryProvider : IQueryProvider, Microsoft.EntityFrameworkCore.Query.IAsyncQueryProvider
{
    private readonly AcmeContext _context;
    private readonly IQueryProvider _innerQueryProvider;

    public DisposableQueryProvider(AcmeContext context, IQueryProvider innerQueryProvider)
    {
        _context = context;
        _innerQueryProvider = innerQueryProvider;
    }

    public IQueryable CreateQuery(Expression expression)
    {
        throw new NotImplementedException();
    }

    public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
    {
        var queryable = _innerQueryProvider.CreateQuery<TElement>(expression);
        return new DisposableQueryable<TElement>(_context, queryable);
    }

    public object? Execute(Expression expression)
    {
        try
        {
            return _innerQueryProvider.Execute(expression);
        }
        finally
        {
            _context.Dispose();
        }
    }

    public TResult Execute<TResult>(Expression expression)
    {
        try
        {
            return _innerQueryProvider.Execute<TResult>(expression);
        }
        finally
        {
            _context.Dispose();
        }
    }

    private static readonly MethodInfo WrapResultMethod =
        new Func<DbContext, Task<object>, Task<object>>(WrapResult).Method.GetGenericMethodDefinition();

    public TResult ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken = new CancellationToken())
    {
        if (_innerQueryProvider is not Microsoft.EntityFrameworkCore.Query.IAsyncQueryProvider ae) throw new NotSupportedException();
        var result = ae.ExecuteAsync<TResult>(expression, cancellationToken);

        if (result is Task t)
        {
            var resultType = result.GetType().GetGenericArguments()[0];
            var wrapResultMethod = WrapResultMethod.MakeGenericMethod(resultType);
            return (TResult)wrapResultMethod.Invoke(null, new object[] { _context, t });
        }

        throw new NotSupportedException();
    }

    private static async Task<T> WrapResult<T>(DbContext context, Task<T> task)
    {
        try
        {
            return await task;
        }
        finally
        {
            await context.DisposeAsync();
        }
    }
}