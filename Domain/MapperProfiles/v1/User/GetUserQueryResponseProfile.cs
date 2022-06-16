using AutoMapper;
using HomeEasy.Domain.Queries.v1.User.GetUser;
using Infrastructure.Data.Entities.User;

namespace HomeEasy.Domain.MapperProfiles.v1.User
{
    public sealed class GetUserQueryResponseProfile : Profile
    {
        public GetUserQueryResponseProfile()
        {
            CreateMap<UserEntity, GetUserQueryResponse>(MemberList.None);
        }
    }
}