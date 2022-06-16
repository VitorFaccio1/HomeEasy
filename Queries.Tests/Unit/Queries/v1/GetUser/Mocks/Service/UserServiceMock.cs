using AutoFixture;
using Infrastructure.Data.Entities.User;
using Infrastructure.Data.Interfaces;
using NSubstitute;

namespace Queries.Tests.Unit.Queries.v1.GetUser.Mocks.Service
{
    public class UserServiceMock
    {
        public IUserService GetMock()
        {
            var mock = Substitute.For<IUserService>();

            mock.GetUserAsync(Arg.Any<long>())
                .Returns(new Fixture().Create<UserEntity>());

            return mock;
        }

        public IUserService GetMock_NoUser()
        {
            var mock = Substitute.For<IUserService>();

            var userNotActive = new Fixture().Create<UserEntity>();
            userNotActive.IsActive = false;

            mock.GetUserAsync(Arg.Any<long>())
                .Returns(userNotActive);

            return mock;
        }
    }
}