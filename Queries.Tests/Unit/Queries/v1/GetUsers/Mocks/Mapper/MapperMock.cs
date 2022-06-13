using AutoFixture;
using AutoMapper;
using HomeEasy.Domain.Commands.v1.User.AddUser;
using HomeEasy.Domain.Queries.v1.User.GetUsers;
using Infrastructure.Data.Entities.User;
using NSubstitute;
using System.Collections.Generic;

namespace Queries.Tests.Unit.Queries.v1.GetUsers.Mocks.Mapper
{
    public sealed class MapperMock
    {
        public IMapper GetMock()
        {
            var mock = Substitute.For<IMapper>();

            mock.Map<List<UserEntity>, List<GetUsersQueryResponse>>(Arg.Any<List<UserEntity>>())
                .Returns(new Fixture().Create<List<GetUsersQueryResponse>>());

            return mock;
        }
    }
}