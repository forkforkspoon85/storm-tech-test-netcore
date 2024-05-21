using Microsoft.EntityFrameworkCore;
using Todo.Data;

namespace Todo.Tests.Infrastructure
{
    public static class InMemoryDbHelper
    {
        public static DbContextOptions<ApplicationDbContext> GetDbContextOptions() => new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "local")
                .Options;
    }
}
