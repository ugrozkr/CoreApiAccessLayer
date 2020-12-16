using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wall.Catalog.Entity;
using Wall.Catalog.Repository.Interface;
using Wall.Service.DataAccessLayer.DbContexts;
using Wall.Service.Repository;

namespace Wall.Catalog.Repository
{
    public class EventRepository : EFRepository, IEventRepository 
    {
        IServiceScopeFactory _serviceScopeFactory;
        public EventRepository(IServiceScopeFactory scopeFactory) : base(scopeFactory)
        {
            _serviceScopeFactory = scopeFactory;
        }

        public async Task<IEnumerable<Event>> GetEvents(Guid categoryId)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<CatalogDbContext>();
                return await dbContext.Set<Event>().Include(x => x.Category).Where(x => (x.CategoryId == categoryId || categoryId == Guid.Empty)).ToListAsync();
            }               
        }

        public async Task<Event> GetEventById(Guid eventId)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<CatalogDbContext>();
                return await dbContext.Set<Event>().Include(x => x.Category).Where(x => x.EventId == eventId).FirstOrDefaultAsync();
            }              
        }
    }
}
