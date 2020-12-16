using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wall.Service.Repository;

namespace Wall.BasketChangeEvent.Repository.Interface
{
    public interface IBasketChangeEventRepository : IGenericRepository
    {
        Task AddBasketEvent(Basket.Entity.BasketChangeEvent basketChangeEvent);
        Task<List<Basket.Entity.BasketChangeEvent>> GetBasketChangeEvents(DateTime startDate, int max);
    }
}
