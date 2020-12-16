using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wall.Catalog.Entity;
using Wall.Service.Repository;

namespace Wall.Catalog.Repository.Interface
{
    public interface IEventRepository : IGenericRepository
    {
        Task<IEnumerable<Event>> GetEvents(Guid categoryId);
        Task<Event> GetEventById(Guid eventId);
    }
}
