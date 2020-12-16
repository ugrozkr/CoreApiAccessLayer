using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wall.Catalog.Repository.Interface;
using WALL.Catalog.DTO;

namespace WALL.Catalog.API.Controllers
{
    [Route("api/events")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IEventRepository eventRepository;
        private readonly IMapper mapper;

        public EventController(IEventRepository eventRepository, IMapper mapper)
        {
            this.eventRepository = eventRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventDto>>> Get([FromQuery] Guid categoryId)
        {
            var result = await eventRepository.GetEvents(categoryId);
            return Ok(mapper.Map<List<EventDto>>(result));
        }

        [HttpGet("{eventId}")]
        public async Task<ActionResult<EventDto>> GetById(Guid eventId)
        {
            var result = await eventRepository.GetEventById(eventId);
            return Ok(mapper.Map<EventDto>(result));
        }
    }
}
