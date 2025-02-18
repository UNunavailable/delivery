using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace crud_service.Database.Entity.Configuration
{
    public abstract class BaseConfiguration<T> : IEntityTypeConfiguration<T> where T : DbEntity
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {

        }
    }
}
