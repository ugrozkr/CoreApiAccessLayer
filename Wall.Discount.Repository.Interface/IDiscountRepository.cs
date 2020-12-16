using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Wall.Discount.Repository.Interface
{
    public interface IDiscountRepository
    {
        Task<IEnumerable<Entity.Discount>> GetAllDiscounts();
        Task<Entity.Discount> GetDiscount(Guid id);
        Task<IEnumerable<Entity.Discount>> GetDiscount(string code);
        Task AddDiscount(Entity.Discount item);
        Task<bool> RemoveDiscount(Guid id);
        Task<bool> UpdateDiscount(Guid id, int amount);
        Task<bool> UpdateDiscountDocument(Guid id, string code, int amount);
        Task<bool> RemoveAllDiscounts();
    }
}
