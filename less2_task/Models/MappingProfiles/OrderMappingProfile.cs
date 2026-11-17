using AutoMapper;
using less2_task.Models;
using less2_task.Models.DTO;

namespace less2_task.Models.MappingProfiles;

public class OrderMappingProfile : Profile
{
    public OrderMappingProfile()
    {
        CreateMap<CreateOrderRequest, Order>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.OrderDate, opt => opt.Ignore())
            .ForMember(dest => dest.IsCompleted, opt => opt.Ignore());

        CreateMap<Order, OrderResponse>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => GetStatus(src.IsCompleted)));
    }

        private string GetStatus(bool isCompleted)
    {
        return isCompleted ? "completed" : "not completed";
    }
}
