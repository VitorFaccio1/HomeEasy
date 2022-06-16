using AutoFixture;
using Infrastructure.Data.Entities.User;
using Infrastructure.Data.Interfaces;
using NSubstitute;

namespace Domain.Tests.Unit.Commands.v1.UpdateUser.Mocks.Service
{
    public class UserServiceMock
    {
        public IUserService GetMock()
        {
            var mock = Substitute.For<IUserService>();

            mock.GetUserAsync(Arg.Any<long>())
                .Returns(new Fixture().Create<UserEntity>());

            mock.UpdateUserAsync(Arg.Any<UserEntity>());

            return mock;
        }

        public IUserService GetMock_ForUserInactive()
        {
            var mock = Substitute.For<IUserService>();

            var user = new UserEntity();
            user.IsActive = false;

            mock.GetUserAsync(Arg.Any<long>())
                .Returns(user);

            return mock;
        }
    }
}
