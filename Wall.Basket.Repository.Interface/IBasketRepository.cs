using System;
using System.Threading.Tasks;
using Wall.Service.Repository;

namespace Wall.Basket.Repository.Interface
{
    public interface IBasketRepository : IGenericRepository
    {
        Task<bool> BasketExists(Guid basketId);

        Task<Entity.Basket> GetBasketById(Guid basketId);

        void AddBasket(Entity.Basket basket);

        Task<bool> SaveChanges();

        Task ClearBasket(Guid basketId);
    }
}
