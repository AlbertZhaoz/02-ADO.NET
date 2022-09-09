using EFCoreRelation.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCoreRelation.Configs
{
    public class CommentConfig : IEntityTypeConfiguration<Comment>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Comment> builder)
        {
            builder.ToTable("T_Comment");
            builder.Property(c => c.Message).IsRequired().IsUnicode();
            builder.HasOne<Article>(c => c.Article).WithMany(a => a.Comments).IsRequired();
        }
    }
}
