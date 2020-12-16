using System.ComponentModel.DataAnnotations;

namespace Wall.Basket.DTO
{
    public class BasketLineForUpdateDto
    {
        [Required]
        public int TicketAmount { get; set; }
    }
}
