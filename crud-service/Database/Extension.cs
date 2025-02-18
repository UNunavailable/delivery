using Microsoft.EntityFrameworkCore;

namespace crud_service.Database
{
    public static class Extension
    {
        private static readonly string SQLITE_DIRECTORY = "AppData/crud.sqlite";
        private static readonly string SQLITE_MIGRATION_ASSEMBLY = "crud-service";
        public static void AddSqliteDbContext(this IServiceCollection services)
        {
            var dbFile = $"Filename ={Environment.CurrentDirectory}/{SQLITE_DIRECTORY}";
        services.AddDbContext<DbContext, SqliteDbContext>((provider, options) =>
            {
                options.UseSqlite(dbFile, sqliteoptions =>
                {
                    sqliteoptions.MigrationsAssembly(SQLITE_MIGRATION_ASSEMBLY);
                });
            });
        }

        public static void MigrateDataBase(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetService<DbContext>();
            dbContext?.Database.Migrate();
        }
    }
}
