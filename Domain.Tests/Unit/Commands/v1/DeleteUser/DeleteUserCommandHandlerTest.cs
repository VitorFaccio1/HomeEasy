using CrossCutting.Exceptions;
using Domain.Tests.Unit.Commands.v1.DeleteUser.Mocks.Service;
using HomeEasy.Domain.Commands.v1.User.DeleteUser;
using NUnit.Framework;
using System.Threading;

namespace Domain.Tests.Unit.Commands.v1.DeleteUser
{
    [TestFixture]
    public class DeleteUserCommandHandlerTest
    {
        [Test]
        [Category("v1")]
        public void Should_DeleteUser()
        {
            var result = new DeleteUserCommandHandler(
                new UserServiceMock().GetMock()).Handle(new DeleteUserCommand(2), CancellationToken.None);

            Assert.IsNotNull(result);
        }

        [Test]
        [Category("v1")]
        public void ShouldNot_DeleteUser_ForUserNotFoundException()
        {
            var result = new DeleteUserCommandHandler(
                new UserServiceMock().GetMock_ForUserInactive()).Handle(new DeleteUserCommand(2), CancellationToken.None);

            Assert.ThrowsAsync<UserNotFoundException>(() => result);
        }
    }
} 