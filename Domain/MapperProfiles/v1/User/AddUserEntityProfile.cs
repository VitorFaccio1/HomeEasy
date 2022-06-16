using AutoMapper;
using HomeEasy.Domain.Commands.v1.User.AddUser;
using Infrastructure.Data.Entities.User;

namespace HomeEasy.Domain.MapperProfiles.v1.User
{
    public sealed class AddUserEntityProfile : Profile
    {
        public AddUserEntityProfile()
        {
            CreateMap<AddUserCommand, UserEntity>(MemberList.None)
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive));
        }
    }
}