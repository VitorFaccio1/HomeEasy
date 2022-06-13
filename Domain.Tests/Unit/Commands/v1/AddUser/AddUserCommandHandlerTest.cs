using Domain.Tests.Unit.Commands.v1.AddUser.Mocks.Mapper;
using Domain.Tests.Unit.Commands.v1.AddUser.Mocks.Service;
using HomeEasy.Domain.Commands.v1.User.AddUser;
using NUnit.Framework;
using System.Threading;

namespace Domain.Tests.Unit.Commands.v1.AddUser
{
    [TestFixture]
    public class AddUserCommandHandlerTest
    {
        [Test]
        [Category("v1")]
        public void Should_AddUser()
        {
            var request = new AddUserCommand()
            {
                Id = 1,
                Email = "vitorfaccio6@gmail.com ",
                Name = "Vitor Faccio",
                Password = "Teste123"
            };

            var result = new AddUserCommandHandler(
                new UserServiceMock().GetMock(),
                new MapperMock().GetMock()).Handle(request, CancellationToken.None);

            Assert.IsNotNull(result);
        }
    }
}