using Microsoft.EntityFrameworkCore;
using Wall.Catalog.Entity;

namespace Wall.Service.DataAccessLayer.Interface
{
    public interface ICatalogDbContext : IDbContext
    {
        DbSet<Category> Categories { get; set; }
        DbSet<Event> Events { get; set; }
    }
}
