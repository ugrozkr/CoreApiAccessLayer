using System;

namespace Wall.Basket.DTO
{
    public class DiscountDto
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public int Amount { get; set; }
        public bool AlreadyUsed { get; set; }
    }
}
