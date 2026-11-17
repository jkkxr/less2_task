using AutoMapper;
using less2_task.Models;
using less2_task.Models.DTO;

namespace less2_task.Models.MappingProfiles;

public class RegisterRequestToUserMappingProfile: Profile
{
    public RegisterRequestToUserMappingProfile()
    {
        CreateMap<RegisterRequest, User>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => NormalizeEmail(src.Email)))
            .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password));
    }

    private string NormalizeEmail(string email)
    {
        return email.ToLowerInvariant();
    }
}
