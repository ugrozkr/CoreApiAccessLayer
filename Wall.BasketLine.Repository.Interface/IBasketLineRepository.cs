using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wall.Service.Repository;

namespace Wall.BasketLine.Repository.Interface
{
    public interface IBasketLineRepository : IGenericRepository
    {
        Task<IEnumerable<Basket.Entity.BasketLine>> GetBasketLines(Guid basketId);
        Task<Basket.Entity.BasketLine> GetBasketLineById(Guid basketLineId);
        Task<Basket.Entity.BasketLine> AddOrUpdateBasketLine(Guid basketId, Basket.Entity.BasketLine basketLine);
        void UpdateBasketLine(Basket.Entity.BasketLine basketLine);
        void RemoveBasketLine(Basket.Entity.BasketLine basketLine);
    }
}
