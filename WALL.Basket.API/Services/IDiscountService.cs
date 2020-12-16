using System;
using System.Threading.Tasks;
using Wall.Discount.Entity;

namespace WALL.Basket.API.Services
{
    public interface IDiscountService
    {
        Task<Discount> GetDiscount(Guid Id);
        Task<Discount> GetDiscountWithError(Guid Id);
    }
}
