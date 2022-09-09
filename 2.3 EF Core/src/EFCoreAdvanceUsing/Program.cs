using EFCoreRelation.DbContexts;

AlbertDbContext albertDbContext = new AlbertDbContext();
var test = albertDbContext.Articles;
foreach (var item in test.Where(a => a.ID > 1))
{
    Console.WriteLine(item.Content);
}
