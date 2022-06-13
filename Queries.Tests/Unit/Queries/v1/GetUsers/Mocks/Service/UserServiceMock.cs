using AutoFixture;
using Infrastructure.Data.Entities.User;
using Infrastructure.Data.Interfaces;
using NSubstitute;
using System.Collections.Generic;

namespace Queries.Tests.Unit.Queries.v1.GetUsers.Mocks.Service
{
    public class UserServiceMock
    {
        public IUserService GetMock()
        {
            var mock = Substitute.For<IUserService>();

            mock.GetUsersAsync()
                .Returns(new Fixture().Create<List<UserEntity>>());

            return mock;
        }

        public IUserService GetMock_NoUsers()
        {
            var mock = Substitute.For<IUserService>();

            mock.GetUsersAsync()
                .Returns(new List<UserEntity>());

            return mock;
        }
    }
}