using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wall.BasketChangeEvent.Repository.Interface;
using Wall.Service.DataAccessLayer.DbContexts;
using Wall.Service.Repository;

namespace Wall.BasketChangeEvent.Repository
{
    public class BasketChangeEventRepository : EFRepository, IBasketChangeEventRepository
    {
        IServiceScopeFactory _serviceScopeFactory;
        public BasketChangeEventRepository(IServiceScopeFactory scopeFactory) : base(scopeFactory)
        {
            _serviceScopeFactory = scopeFactory;
        }

        public async Task AddBasketEvent(Basket.Entity.BasketChangeEvent basketChangeEvent)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<BasketDbContext>();
                await dbContext.BasketChangeEvents.AddAsync(basketChangeEvent);
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task<List<Basket.Entity.BasketChangeEvent>> GetBasketChangeEvents(DateTime startDate, int max)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<BasketDbContext>();
                return await dbContext.BasketChangeEvents.Where(b => b.InsertedAt > startDate).OrderBy(b => b.InsertedAt).Take(max).ToListAsync();
            }
        }
    }
}
