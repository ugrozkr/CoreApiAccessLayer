using AutoMapper;
using Wall.Basket.DTO;
using Wall.Basket.Entity;

namespace Wall.Service.Mapper
{
    public class BasketServiceProfile : Profile
    {
        public BasketServiceProfile()
        {
            CreateMap<BasketChangeEvent, BasketChangeEventForPublicationDto>().ReverseMap();
            CreateMap<BasketCheckoutDto, BasketCheckoutMessage>().ReverseMap();
            CreateMap<BasketLineForCreationDto, BasketLine>();
            CreateMap<BasketLineForUpdateDto, BasketLine>();
            CreateMap<BasketLine, BasketLineDto>().ReverseMap();
            CreateMap<BasketForCreationDto, Basket.Entity.Basket>();
            CreateMap<Basket.Entity.Basket, BasketDto>().ReverseMap();
            CreateMap<Event, EventDto>().ReverseMap();
        }
    }
}
