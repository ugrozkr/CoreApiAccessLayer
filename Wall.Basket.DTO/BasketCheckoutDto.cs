using System;

namespace Wall.Basket.DTO
{
    public class BasketCheckoutDto
    {
        public Guid BasketId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public Guid UserId { get; set; }
        public string CardNumber { get; set; }
        public string CardName { get; set; }
        public string CardExpiration { get; set; }
        public string CvvCode { get; set; }
    }
}
