using System;
using System.ComponentModel.DataAnnotations;

namespace Wall.Basket.DTO
{
    public class BasketForCreationDto
    {
        [Required]
        public Guid UserId { get; set; }
    }
}
