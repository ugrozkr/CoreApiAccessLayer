using Microsoft.Extensions.Logging;
using Serilog.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wall.Basket.Repository.Interface;

namespace Wall.Integration.Messages
{
    public class ProductPriceChangedIntegrationEventHandler : IIntegrationEventHandler<ProductPriceChangedIntegrationEvent>
    {
        private readonly ILogger<ProductPriceChangedIntegrationEventHandler> _logger;
        private readonly IBasketRepository _repository;

        public ProductPriceChangedIntegrationEventHandler(
            ILogger<ProductPriceChangedIntegrationEventHandler> logger,
            IBasketRepository repository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task Handle(ProductPriceChangedIntegrationEvent @event)
        {
            using (LogContext.PushProperty("IntegrationEventContext", $"{@event.Id}"))
            {
                _logger.LogInformation("----- Handling integration event: {IntegrationEventId} at {AppName} - ({@IntegrationEvent})", @event.Id, "Basket API", @event);
                var baskets = _repository.GetAll<Basket.Entity.Basket>();
                foreach (var bs in baskets)
                {
                    await UpdatePriceInBasketItems(@event.UserId, @event.TotalPrice, 0, baskets.ToList());
                }
            }
        }

        private async Task UpdatePriceInBasketItems(Guid userId, decimal newPrice, decimal oldPrice, List<Basket.Entity.Basket> basket)
        {
            var itemsToUpdate = basket?.Where(x => x.UserId == userId).ToList().FirstOrDefault();
            if (itemsToUpdate != null)
            {
                _logger.LogInformation("----- ProductPriceChangedIntegrationEventHandler - Updating items in basket for user:({@Items})", itemsToUpdate);
                _repository.Update(itemsToUpdate);
            }
        }
    }
}
