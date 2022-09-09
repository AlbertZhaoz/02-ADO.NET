using EFCoreRelation.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCoreRelation.Configs
{
    public class ArticleConfig : IEntityTypeConfiguration<Article>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Article> builder)
        {
            builder.ToTable("T_Article");
            builder.Property(a => a.Content).IsRequired();
            builder.Property(a => a.Title).IsRequired().IsUnicode().HasMaxLength(255);
        }
    }
}
