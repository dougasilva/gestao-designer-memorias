using GestaoDesignerMemorias.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GestaoDesignerMemorias.Tests.Helpers;
public static class InMemoryDbContextFactory
{
    public static AppDbContext Create()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite("Data Source=:memory:")  // SQLite in-memory
            .Options;

        var context = new AppDbContext(options);
        context.Database.EnsureCreated();  // ou .Migrate() se tiver migrações
        return context;
    }
}