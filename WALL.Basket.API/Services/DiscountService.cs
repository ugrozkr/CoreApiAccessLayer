using System;
using System.Net.Http;
using System.Threading.Tasks;
using Wall.Discount.Entity;
using Wall.Service.Extensions;

namespace WALL.Basket.API.Services
{
    public class DiscountService : IDiscountService
    {
        private readonly HttpClient client;
        public DiscountService(HttpClient client)
        {
            this.client = client;
        }

        public async Task<Discount> GetDiscount(Guid Id)
        {
            var response = await client.GetAsync($"/api/discount/{Id}");
            return await response.ReadContentAs<Discount>();
        }

        public async Task<Discount> GetDiscountWithError(Guid Id)
        {
            var response = await client.GetAsync($"/api/discount/error/{Id}");
            return await response.ReadContentAs<Discount>();
        }
    }
}
