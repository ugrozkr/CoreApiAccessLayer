using System;

namespace Wall.Basket.DTO
{
    public class BasketDto
    {
        public Guid BasketId { get; set; }
        public Guid UserId { get; set; }
        public int NumberOfItems { get; set; }
        public Guid? DiscountId { get; set; }
    }
}
