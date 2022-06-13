using AutoMapper;
using HomeEasy.Domain.Queries.v1.User.GetUsers;
using Infrastructure.Data.Entities.User;

namespace HomeEasy.Domain.MapperProfiles.v1.User
{
    public sealed class GetUsersQueryResponseProfile : Profile
    {
        public GetUsersQueryResponseProfile()
        {
            CreateMap<UserEntity, GetUsersQueryResponse>(MemberList.None);
        }
    }
}