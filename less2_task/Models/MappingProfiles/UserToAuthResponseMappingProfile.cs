using AutoMapper;
using less2_task.Models;
using less2_task.Models.DTO;

namespace less2_task.Models.MappingProfiles;

public class UserToAuthResponseMappingProfile: Profile
{
    public UserToAuthResponseMappingProfile()
    {
        CreateMap<User, AuthResponse>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Token, opt => opt.MapFrom(dest => dest.Token));
    }
}
