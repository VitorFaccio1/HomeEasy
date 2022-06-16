using AutoFixture;
using CrossCutting.Exceptions;
using Domain.Tests.Unit.Commands.v1.UpdateUser.Mocks.Service;
using HomeEasy.Domain.Commands.v1.User.UpdateUser;
using NUnit.Framework;
using System.Threading;

namespace Domain.Tests.Unit.Commands.v1.UpdateUser
{
    [TestFixture]
    public class UpdateUserCommandHandlerTest
    {
        [Test]
        [Category("v1")]
        public void Should_UpdateUser()
        {
            var result = new UpdateUserCommandHandler(
                new UserServiceMock().GetMock()).Handle(new Fixture().Create<UpdateUserCommand>(), CancellationToken.None);

            Assert.IsNotNull(result);
        }

        [Test]
        [Category("v1")]
        public void ShouldNot_UpdateUser_ForUserNotFoundException()
        {
            var result = new UpdateUserCommandHandler(
                new UserServiceMock().GetMock_ForUserInactive()).Handle(new Fixture().Create<UpdateUserCommand>(), CancellationToken.None);

            Assert.ThrowsAsync<UserNotFoundException>(() => result);
        }
    }
}