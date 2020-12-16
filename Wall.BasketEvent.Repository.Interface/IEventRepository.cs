using System;
using System.Threading.Tasks;
using Wall.Basket.Entity;
using Wall.Service.Repository;

namespace Wall.BasketEvent.Repository.Interface
{
    public interface IEventRepository : IGenericRepository
    {
        void AddEvent(Event theEvent);
        Task<bool> EventExists(Guid eventId);
    }
}
