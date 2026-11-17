using AutoMapper;
using less2_task.Models;
using less2_task.Models.DTO;

namespace less2_task.Models.MappingProfiles;

public class UserToUserViewModelMappingProfile: Profile
{
    public UserToUserViewModelMappingProfile()
    {
        CreateMap<User, UserViewModel>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => NameAndEmail(src.Name, src.Email)))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));
    }

    private string NameAndEmail(string name, string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            email = "no-email@example.com";
        }
        return $"{name} <{email.ToLowerInvariant()}>";
    }
}
