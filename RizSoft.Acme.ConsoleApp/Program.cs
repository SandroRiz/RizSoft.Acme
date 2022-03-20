using Microsoft.EntityFrameworkCore;

string connString = @"Data Source=(localdb)\mssqllocaldb;Initial Catalog=Acme;Trusted_Connection=Yes;";

AcmeContextFactory factory = new AcmeContextFactory(connString);

//var now =98;

//OrderService orderService = new OrderService(factory);

//Order order = await orderService.GetAsync(1);
//Console.WriteLine($"Order #{order.Id} del {order.OrderDate} cliente {order.Customer.CustomerName} con {order.OrderRows.Count} righe");
//order.ShippedDate = DateTime.Now;
//order.Customer.Email = $"test_{now}@mydomain.com";
//if (order.OrderRows.Any())
//{
//    var orderRow = order.OrderRows.First();
//    orderRow.Discount = now;
//}
//order.OrderRows.Add(new OrderRow { RowNumber = order.OrderRows.Count+1, ProductId = order.OrderRows.Count+1, Qty= 30, UnitPrice=300, Discount=30 });

//order.OrderRows.Remove(order.OrderRows.ElementAt(1));

//await orderService.UpdateAsync(order);

//Console.WriteLine("Done");
//return;


// con Datacontext Diretto funziona
/*
var db = factory.CreateDbContext();
Order? order = await db.Orders
            .Where(o => o.Id == 1)
            .Include(o => o.OrderRows)
            .Include(o => o.Customer)
            .FirstOrDefaultAsync();

Console.WriteLine($"Order #{order.Id} del {order.OrderDate} cliente {order.Customer.CustomerName} con {order.OrderRows.Count} righe");
order.ShippedDate = DateTime.Now;
order.Customer.Email = "uvdqy5@nqtay.hnwbsr.com";
if (order.OrderRows.Any())
{
    var orderRow = order.OrderRows.First();
    orderRow.Discount = 99;
}
order.OrderRows.Add(new OrderRow { RowNumber = 3, ProductId = 3, Qty = 30, UnitPrice = 300, Discount = 30 });
await db.SaveChangesAsync();

Console.WriteLine("Done");
return;

*/

// PRODUCTS AND TAGS

ProductService productService = new ProductService(factory);
TagService tagService = new TagService(factory);

// tutto nuovo

Product newProduct = new Product { ProductName = "Product " + Guid.NewGuid().ToString(), Discontinued=false};

Tag newTag = new Tag { TagName = "Tag " + Guid.NewGuid().ToString() };
await tagService.AddAsync(newTag);

newProduct.Tags.Add(newTag);
await productService.AddAsync(newProduct);

Console.WriteLine("Done");
return;

var p1 = await productService.GetWithTagsAsync(1);

Console.WriteLine($"product {p1.ProductName} has these Tags");
foreach (var tag in p1.Tags)
{
    Console.WriteLine($"Tag: {tag.TagName}");
}



Console.WriteLine($"Added new Tag: {newTag.Id}");

p1.Tags.Add(newTag);

//SALVA
await productService.UpdateAsync(p1);

//Ritiro su dal DB, (in memoria il ta c'è) (SELECT * FROM dbo.TagsProducts WHERE IdProduct = 1)
p1 = await productService.GetWithTagsAsync(1);
Console.WriteLine($"product {p1.ProductName} has these Tags");
foreach (var tag in p1.Tags)
{
    Console.WriteLine($"Tag: {tag.TagName}");
}

return;

//se faccio tutto nell'ambito dello stesso factory così funziona
var ctx = factory.CreateDbContext();

var p2 = await ctx.Set<Product>().FindAsync(2);

Tag newTag2 = new Tag { TagName = "Tag " + Guid.NewGuid().ToString() };
await tagService.AddAsync(newTag2);
Console.WriteLine($"Added new Tag: {newTag2.Id}");


p2.Tags.Add(newTag2);

//SALVA
await ctx.SaveChangesAsync();

p2 = await productService.GetWithTagsAsync(2);
Console.WriteLine($"product {p2.ProductName} has these Tags");
foreach (var tag in p2.Tags)
{
    Console.WriteLine($"Tag: {tag.TagName}");
}

