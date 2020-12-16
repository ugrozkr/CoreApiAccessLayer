using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wall.BasketLine.Repository.Interface;
using Wall.Service.DataAccessLayer.DbContexts;
using Wall.Service.Repository;


namespace Wall.BasketLine.Repository
{
    public class BasketLineRepository : EFRepository, IBasketLineRepository
    {
        IServiceScopeFactory _serviceScopeFactory;
        public BasketLineRepository(IServiceScopeFactory scopeFactory) : base(scopeFactory)
        {
            _serviceScopeFactory = scopeFactory;
        }

        public async Task<IEnumerable<Basket.Entity.BasketLine>> GetBasketLines(Guid basketId)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<BasketDbContext>();
                return await dbContext.BasketLines.Include(bl => bl.Event).Where(b => b.BasketId == basketId).ToListAsync();
            }
        }

        public async Task<Basket.Entity.BasketLine> GetBasketLineById(Guid basketLineId)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<BasketDbContext>();
                return await dbContext.BasketLines.Include(bl => bl.Event).Where(b => b.BasketLineId == basketLineId).FirstOrDefaultAsync();
            }
        }

        public async Task<Basket.Entity.BasketLine> AddOrUpdateBasketLine(Guid basketId, Basket.Entity.BasketLine basketLine)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<BasketDbContext>();
                var existingLine = await dbContext.BasketLines.Include(bl => bl.Event)
                .Where(b => b.BasketId == basketId && b.EventId == basketLine.EventId).FirstOrDefaultAsync();
                if (existingLine == null)
                {
                    basketLine.BasketId = basketId;
                    dbContext.BasketLines.Add(basketLine);
                    return basketLine;
                }
                existingLine.TicketAmount += basketLine.TicketAmount;
                return existingLine;
            }
        }

        public void UpdateBasketLine(Basket.Entity.BasketLine basketLine)
        {
            // 
        }

        public void RemoveBasketLine(Basket.Entity.BasketLine basketLine)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<BasketDbContext>();
                dbContext.BasketLines.Remove(basketLine);
            }
        }
    }
}
