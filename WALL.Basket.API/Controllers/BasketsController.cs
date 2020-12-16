using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using WALL.Basket.API.Services;
using Wall.Discount.Entity;
using Wall.Basket.Repository.Interface;
using Wall.Basket.DTO;
using Wall.Basket.Entity;
using Wall.Integration.Messages;

namespace WALL.Basket.API.Controllers
{
    [Route("api/baskets")]
    [ApiController]
    public class BasketsController : ControllerBase
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;
        private readonly IDiscountService _discountService;
        private readonly IEventBus _eventBus;

        public BasketsController(IBasketRepository basketRepository, IMapper mapper, IDiscountService discountService, IEventBus eventBus)
        {
            _basketRepository = basketRepository;
            _mapper = mapper;
            _discountService = discountService;
            _eventBus = eventBus;
        }

        [HttpPost]
        public async Task<ActionResult<BasketDto>> Post(BasketForCreationDto basketForCreation)
        {
            var basketEntity = _mapper.Map<Wall.Basket.Entity.Basket>(basketForCreation);
            _basketRepository.AddBasket(basketEntity);
            await _basketRepository.SaveChanges();
            var basketToReturn = _mapper.Map<BasketDto>(basketEntity);
            return CreatedAtRoute(
                "GetBasket",
                new { basketId = basketEntity.BasketId },
                basketToReturn);
        }

        [HttpPut("{basketId}/discount")]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ApplyDiscountToBasket(Guid basketId, DiscountDto discount)
        {
            var basket = await _basketRepository.GetBasketById(basketId);

            if (basket == null)
            {
                return BadRequest();
            }

            basket.DiscountId = discount.Id;
            await _basketRepository.SaveChanges();
            return Accepted();
        }

        [HttpPost("checkout")]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> CheckoutBasketAsync([FromBody] BasketCheckoutDto basketCheckout)
        {
            try
            {
                var basket = await _basketRepository.GetBasketById(basketCheckout.BasketId);
                if (basket == null)
                {
                    return BadRequest();
                }
                BasketCheckoutMessage basketCheckoutMessage = _mapper.Map<BasketCheckoutMessage>(basketCheckout);
                basketCheckoutMessage.BasketLines = new List<BasketLineMessage>();
                int total = 0;
                foreach (var b in basket.BasketLines)
                {
                    var basketLineMessage = new BasketLineMessage
                    {
                        BasketLineId = b.BasketLineId,
                        Price = b.Price,
                        TicketAmount = b.TicketAmount
                    };
                    total += b.Price * b.TicketAmount;
                    basketCheckoutMessage.BasketLines.Add(basketLineMessage);
                }
                Discount discount = null;
                if (basket.DiscountId.HasValue)
                    discount = await _discountService.GetDiscount(basket.DiscountId.Value);
                if (discount != null)
                {
                    basketCheckoutMessage.BasketTotal = total - discount.Amount;
                }
                else
                {
                    basketCheckoutMessage.BasketTotal = total;
                }

                try
                {
                    ProductPriceChangedIntegrationEvent queueData = new ProductPriceChangedIntegrationEvent
                        (basketCheckoutMessage.BasketId, basketCheckoutMessage.BasketTotal, basketCheckoutMessage.UserId);
                    _eventBus.Publish(queueData);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
                await _basketRepository.ClearBasket(basketCheckout.BasketId);
                return Accepted(basketCheckoutMessage);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.StackTrace);
            }
        }
    }
}
