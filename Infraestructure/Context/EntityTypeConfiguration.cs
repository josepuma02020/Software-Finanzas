using Domain.Base;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Context
{
    public class EntityTypeConfiguration<T> : IEntityTypeConfiguration<T> where T : Entity<Guid>
    {
        public void Configure(EntityTypeBuilder<T> builder)
        {
            builder.ToTable(typeof(T).Name,
                            FinanzasContext.DefaultSchema);
            builder.HasIndex(t => t.Id);
        }
    }
}
