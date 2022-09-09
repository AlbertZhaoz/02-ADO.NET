using EFCoreRelation.DbContexts;
using EFCoreRelation.Models;
using Microsoft.EntityFrameworkCore;

//Article article = new Article();
//article.Title = "微软.NET最新动态";
//article.Content = "微软发布了最新的.NET7 Preview版本，该版本优化了JWT配置";
//Comment comment1 = new Comment() { Message = "真的好棒" };
//Comment comment2 = new Comment() { Message = "终于优化了JWT了" };
//Comment comment3 = new Comment() { Message = "拥抱开源后微软变得很牛" };
//Comment comment4 = new Comment() { Message = "巨硬无敌" };
//article.Comments.Add(comment1);
//article.Comments.Add(comment2);
//article.Comments.Add(comment3);
//article.Comments.Add(comment4);
//using var ctx = new AlbertDbContext();
//ctx.Articles.Add(article);
//await ctx.SaveChangesAsync();

// 移除全部数据
//using var ctx = new AlbertDbContext();

//foreach (var item in ctx.Articles)
//{
//    ctx.Articles.Remove(item);
//}
//ctx.SaveChanges();

// 注意SingleOrDefault返回空值的情况
using var ctx = new AlbertDbContext();
try
{
    var articlesingle = ctx.Articles.Include(a => a.Comments).SingleOrDefault(c => c.ID == 4);
    foreach (var item in articlesingle.Comments)
    {
        Console.WriteLine(item.Message);
    }

}
catch (Exception ex)
{
    throw ex;
}


