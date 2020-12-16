using System;
using System.Threading.Tasks;
using Wall.Basket.Entity;

namespace WALL.Basket.API.Services
{
    public interface IEventCatalogService
    {
        Task<Event> GetEvent(Guid id);
    }
}
