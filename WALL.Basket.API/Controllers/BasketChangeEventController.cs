using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wall.BasketChangeEvent.Repository.Interface;
using Wall.Basket.DTO;

namespace WALL.Basket.API.Controllers
{
    [ApiController]
    [Route("api/basketevent")]
    public class BasketChangeEventController : Controller
    {
        private readonly IBasketChangeEventRepository basketChangeEventRepository;
        private readonly IMapper mapper;

        public BasketChangeEventController(IMapper mapper, IBasketChangeEventRepository basketChangeEventRepository)
        {
            this.mapper = mapper;
            this.basketChangeEventRepository = basketChangeEventRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetEvents([FromQuery] DateTime fromDate, [FromQuery] int max)
        {
            var events = await basketChangeEventRepository.GetBasketChangeEvents(fromDate, max);
            return Ok(mapper.Map<List<BasketChangeEventForPublicationDto>>(events));
        }
    }
}
