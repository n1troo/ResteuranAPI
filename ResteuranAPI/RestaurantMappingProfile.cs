using AutoMapper;
using ResteuranAPI.Controllers;
using ResteuranAPI.Entities;
using ResteuranAPI.Models;

namespace ResteuranAPI
{
    public class RestaurantMappingProfile :Profile
    {
        public RestaurantMappingProfile()
        {
            CreateMap<Restaurant, RestaurantDTO>()
                .ForMember(m => m.Street, c => c.MapFrom(s => s.Address.Street))
                .ForMember(s=>s.City,c=>c.MapFrom(s=>s.Address.City))
                .ForMember(m => m.PostalCode, s => s.MapFrom( c => c.Address.PostalCode));

            CreateMap<Dish, DishDTO>()
                .ForMember(s => s.RestaurantId, x => x.MapFrom(s => s.Restaurant.Id));


            CreateMap<CreateRestaurantDTO, Restaurant>()
                .ForMember(s => s.Address,
                    c => c.MapFrom(dto => new Address()
                    {
                        City = dto.City,
                        PostalCode = dto.PostalCode,
                        Street = dto.Street
                    }));


            CreateMap<UpdateRestaurantDTO, Restaurant>()
                .ForMember(s => s.Name, s => s.MapFrom(c => c.Name))
                .ForMember(s => s.Description, s => s.MapFrom(c => c.Description))
                .ForMember(s => s.HasDelivery, s => s.MapFrom(c => c.HasDelivery));



            CreateMap<CreateDishDTO, Dish>();
            // .ForMember(s => s.Restaurant.Id, x => x.MapFrom(s => s.RestaurantId));


        }
    }
}
