using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Wall.Basket.Entity;
using Wall.BasketEvent.Repository.Interface;
using Wall.Service.DataAccessLayer.DbContexts;
using Wall.Service.Repository;

namespace Wall.BasketEvent.Repository
{
    public class EventRepository : EFRepository, IEventRepository
    {
        IServiceScopeFactory _serviceScopeFactory;
        public EventRepository(IServiceScopeFactory scopeFactory) : base(scopeFactory)
        {
            _serviceScopeFactory = scopeFactory;
        }

        public async Task<bool> EventExists(Guid eventId)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<BasketDbContext>();
                return await dbContext.Events.AnyAsync(e => e.EventId == eventId);
            }
        }

        public void AddEvent(Event theEvent)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<BasketDbContext>();
                dbContext.Events.Add(theEvent);
            }
        }
    }
}
