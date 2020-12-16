using System;
using System.ComponentModel.DataAnnotations;

namespace Wall.Basket.DTO
{
    public class BasketLineForCreationDto
    {
        [Required]
        public Guid EventId { get; set; }
        [Required]
        public int Price { get; set; }
        [Required]
        public int TicketAmount { get; set; }
    }
}
