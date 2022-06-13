using Infrastructure.Data.Entities.User;
using Infrastructure.Data.Interfaces;
using NSubstitute;

namespace Domain.Tests.Unit.Commands.v1.AddUser.Mocks.Service
{
    public class UserServiceMock
    {
        public IUserService GetMock()
        {
            var mock = Substitute.For<IUserService>();

            mock.AddUserAsync(Arg.Any<UserEntity>());

            return mock;
        }
    }
}