using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace crud_service.Database.Entity.Configuration
{
    public class OrderConfiguration : BaseConfiguration<Order>
    {
        public override void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.Product)
                .WithMany(x => x.Orders)
                .HasForeignKey(x => x.ProductId);
            base.Configure(builder);
        }
    }
}
