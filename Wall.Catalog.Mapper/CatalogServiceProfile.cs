using AutoMapper;
using Wall.Catalog.Entity;
using WALL.Catalog.DTO;

namespace Wall.Service.Mapper
{
    public class CatalogServiceProfile : Profile
    {
        public CatalogServiceProfile()
        {
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<Event, EventDto>().ForMember(dest => dest.CategoryName, opts => opts.MapFrom(src => src.Category.Name));
        }
    }
}
