using System;

namespace Wall.Integration.Messages
{
    public class ProductPriceChangedIntegrationEvent : IntegrationEvent
    {
        public Guid BasketId { get; private set; }

        public decimal TotalPrice { get; private set; }

        public Guid UserId  { get; private set; }

        public ProductPriceChangedIntegrationEvent(Guid basketId, decimal totalPrice, Guid userId)
        {
            BasketId = basketId;
            TotalPrice = totalPrice;
            UserId = userId;
        }
    }
}
