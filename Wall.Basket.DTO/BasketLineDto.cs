using System;

namespace Wall.Basket.DTO
{
    public class BasketLineDto
    {
        public Guid BasketLineId { get; set; }
        public Guid BasketId { get; set; }
        public Guid EventId { get; set; }
        public int Price { get; set; }
        public int TicketAmount { get; set; }
        public EventDto Event { get; set; }
    }
}
