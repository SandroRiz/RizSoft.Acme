
string connString = @"Data Source=(localdb)\mssqllocaldb;Initial Catalog=Acme;Trusted_Connection=Yes;";
AcmeContextFactory factory = new AcmeContextFactory(connString);

ProductService productService = new ProductService(factory);
TagService tagService = new TagService(factory);

var p1 = await productService.GetWithTagsAsync(1);

Console.WriteLine($"product {p1.ProductName} has these Tags");
foreach (var tag in p1.IdTags)
{
    Console.WriteLine($"Tag: {tag.TagName}");
}

Tag newTag = new Tag { TagName = "Tag "+Guid.NewGuid().ToString() };
await tagService.AddAsync(newTag);

Console.WriteLine($"Added new Tag: {newTag.Id}");

p1.IdTags.Add(newTag);

//SALVA
await productService.UpdateAsync(p1);

//Ritiro su dal DB, (in memoria il ta c'è) (SELECT * FROM dbo.TagsProducts WHERE IdProduct = 1)
p1 = await productService.GetWithTagsAsync(1);
Console.WriteLine($"product {p1.ProductName} has these Tags");
foreach (var tag in p1.IdTags)
{
    Console.WriteLine($"Tag: {tag.TagName}");
}



//se faccio tutto nell'ambito dello stesso factory così funziona
var ctx = factory.CreateDbContext();

var p2 = await ctx.Set<Product>().FindAsync(2);

Tag newTag2 = new Tag { TagName = "Tag " + Guid.NewGuid().ToString() };
await tagService.AddAsync(newTag2);
Console.WriteLine($"Added new Tag: {newTag2.Id}");


p2.IdTags.Add(newTag2);

//SALVA
await ctx.SaveChangesAsync();

p2 = await productService.GetWithTagsAsync(2);
Console.WriteLine($"product {p2.ProductName} has these Tags");
foreach (var tag in p2.IdTags)
{
    Console.WriteLine($"Tag: {tag.TagName}");
}

