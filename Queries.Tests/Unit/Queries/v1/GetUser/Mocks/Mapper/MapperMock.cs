using AutoFixture;
using AutoMapper;
using HomeEasy.Domain.Queries.v1.User.GetUser;
using Infrastructure.Data.Entities.User;
using NSubstitute;

namespace Queries.Tests.Unit.Queries.v1.GetUser.Mocks.Mapper
{
    public sealed class MapperMock
    {
        public IMapper GetMock()
        {
            var mock = Substitute.For<IMapper>();

            mock.Map<UserEntity, GetUserQueryResponse>(Arg.Any<UserEntity>())
                .Returns(new Fixture().Create<GetUserQueryResponse>());

            return mock;
        }
    }
}