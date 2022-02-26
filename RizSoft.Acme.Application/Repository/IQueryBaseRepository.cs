namespace RizSoft.Acme.Application;

public interface IQueryBaseRepository<out T>
{
     IQueryable<T> Query { get; }

}