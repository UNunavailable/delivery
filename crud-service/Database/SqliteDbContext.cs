using Microsoft.EntityFrameworkCore;

namespace crud_service.Database
{
    public class SqliteDbContext : DbContext
    {
        public SqliteDbContext() : base() { }
        public SqliteDbContext(DbContextOptions<SqliteDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(SqliteDbContext).Assembly);
        }
    }
}
