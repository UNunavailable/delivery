using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace crud_service.Database.Entity.Configuration
{
    public class ProductConfiguration : BaseConfiguration<Product>
    {
        public override void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(x => x.Article);
            base.Configure(builder);
        }
    }
}
