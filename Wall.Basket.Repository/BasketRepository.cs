using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;
using Wall.Basket.Repository.Interface;
using Wall.Service.DataAccessLayer.DbContexts;
using Wall.Service.Repository;

namespace Wall.Basket.Repository
{
    public class BasketRepository : EFRepository, IBasketRepository
    {
        IServiceScopeFactory _serviceScopeFactory;
        public BasketRepository(IServiceScopeFactory scopeFactory) : base(scopeFactory)
        {
            _serviceScopeFactory = scopeFactory;
        }

        public async Task<Entity.Basket> GetBasketById(Guid basketId)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<BasketDbContext>();
                return await dbContext.Baskets.Include(sb => sb.BasketLines).Where(b => b.BasketId == basketId).FirstOrDefaultAsync();
            }
        }

        public async Task<bool> BasketExists(Guid basketId)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<BasketDbContext>();
                return await dbContext.Baskets.AnyAsync(b => b.BasketId == basketId);
            }
        }

        public async Task ClearBasket(Guid basketId)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<BasketDbContext>();
                var basketLinesToClear = dbContext.BasketLines.Where(b => b.BasketId == basketId);
                dbContext.BasketLines.RemoveRange(basketLinesToClear);
                var basket = dbContext.Baskets.FirstOrDefault(b => b.BasketId == basketId);
                if (basket != null) basket.DiscountId = null;
                await SaveChanges();
            }
        }

        public void AddBasket(Entity.Basket basket)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<BasketDbContext>();
                dbContext.Baskets.Add(basket);
            }
        }

        public async Task<bool> SaveChanges()
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<BasketDbContext>();
                return (await dbContext.SaveChangesAsync() > 0);
            }              
        }
    }
}
