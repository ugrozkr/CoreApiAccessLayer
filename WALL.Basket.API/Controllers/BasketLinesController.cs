using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WALL.Basket.API.Services;
using Wall.Basket.Repository.Interface;
using Wall.BasketLine.Repository.Interface;
using Wall.BasketChangeEvent.Repository.Interface;
using Wall.Basket.DTO;
using Wall.Basket.Entity;
using Wall.BasketEvent.Repository.Interface;

namespace WALL.Basket.API.Controllers
{
    [Route("api/baskets/{basketId}/basketlines")]
    [ApiController]
    public class BasketLinesController : ControllerBase
    {
        private readonly IBasketRepository basketRepository;
        private readonly IBasketLineRepository basketLinesRepository;
        private readonly IEventRepository eventRepository;
        private readonly IEventCatalogService eventCatalogService;
        private readonly IMapper mapper;
        private readonly IBasketChangeEventRepository basketChangeEventRepository;

        public BasketLinesController(IBasketRepository basketRepository,
            IBasketLineRepository basketLinesRepository, IEventRepository eventRepository,
            IEventCatalogService eventCatalogService, IMapper mapper, IBasketChangeEventRepository basketChangeEventRepository)
        {
            this.basketRepository = basketRepository;
            this.basketLinesRepository = basketLinesRepository;
            this.eventRepository = eventRepository;
            this.eventCatalogService = eventCatalogService;
            this.basketChangeEventRepository = basketChangeEventRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BasketLine>>> Get(Guid basketId)
        {
            if (!await basketRepository.BasketExists(basketId))
            {
                return NotFound();
            }

            var basketLines = await basketLinesRepository.GetBasketLines(basketId);
            return Ok(mapper.Map<IEnumerable<BasketLine>>(basketLines));
        }

        [HttpGet("{basketLineId}", Name = "GetBasketLine")]
        public async Task<ActionResult<BasketLine>> Get(Guid basketId,
            Guid basketLineId)
        {
            if (!await basketRepository.BasketExists(basketId))
            {
                return NotFound();
            }

            var basketLine = await basketLinesRepository.GetBasketLineById(basketLineId);
            if (basketLine == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<BasketLine>(basketLine));
        }

        [HttpPost]
        public async Task<ActionResult<BasketLine>> Post(Guid basketId, [FromBody] BasketLineForCreationDto basketLineForCreation)
        {
            var basket = await basketRepository.GetBasketById(basketId);
            if (basket == null)
            {
                return NotFound();
            }

            if (!await eventRepository.EventExists(basketLineForCreation.EventId))
            {
                var eventFromCatalog = await eventCatalogService.GetEvent(basketLineForCreation.EventId);
                eventRepository.AddEvent(eventFromCatalog);
                eventRepository.Save();
            }

            var basketLineEntity = mapper.Map<Wall.Basket.Entity.BasketLine>(basketLineForCreation);
            var processedBasketLine = await basketLinesRepository.AddOrUpdateBasketLine(basketId, basketLineEntity);
            basketLinesRepository.Save();

            var basketLineToReturn = mapper.Map<BasketLine>(processedBasketLine);
            BasketChangeEvent basketChangeEvent = new BasketChangeEvent
            {
                BasketChangeType = Wall.Basket.Entity.BasketChangeTypeEnum.Add,
                EventId = basketLineForCreation.EventId,
                InsertedAt = DateTime.Now,
                UserId = basket.UserId
            };
            await basketChangeEventRepository.AddBasketEvent(basketChangeEvent);

            return CreatedAtRoute(
                "GetBasketLine",
                new { basketId = basketLineEntity.BasketId, basketLineId = basketLineEntity.BasketLineId },
                basketLineToReturn);
        }

        [HttpPut("{basketLineId}")]
        public async Task<ActionResult<BasketLine>> Put(Guid basketId,
            Guid basketLineId,
            [FromBody] BasketLineForUpdateDto basketLineForUpdate)
        {
            if (!await basketRepository.BasketExists(basketId))
            {
                return NotFound();
            }

            var basketLineEntity = await basketLinesRepository.GetBasketLineById(basketLineId);

            if (basketLineEntity == null)
            {
                return NotFound();
            }

            mapper.Map(basketLineForUpdate, basketLineEntity);
            basketLinesRepository.UpdateBasketLine(basketLineEntity);
            basketLinesRepository.Save();
            return Ok(mapper.Map<BasketLine>(basketLineEntity));
        }

        [HttpDelete("{basketLineId}")]
        public async Task<IActionResult> Delete(Guid basketId, Guid basketLineId)
        {
            var basket = await basketRepository.GetBasketById(basketId);
            if (basket == null)
            {
                return NotFound();
            }

            var basketLineEntity = await basketLinesRepository.GetBasketLineById(basketLineId);
            if (basketLineEntity == null)
            {
                return NotFound();
            }

            basketLinesRepository.RemoveBasketLine(basketLineEntity);
            basketLinesRepository.Save();
            BasketChangeEvent basketChangeEvent = new BasketChangeEvent
            {
                BasketChangeType = Wall.Basket.Entity.BasketChangeTypeEnum.Remove,
                EventId = basketLineEntity.EventId,
                InsertedAt = DateTime.Now,
                UserId = basket.UserId
            };
            await basketChangeEventRepository.AddBasketEvent(basketChangeEvent);
            return NoContent();
        }
    }
}
