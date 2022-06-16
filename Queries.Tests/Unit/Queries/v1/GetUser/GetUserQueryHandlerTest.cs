using CrossCutting.Exceptions;
using HomeEasy.Domain.Queries.v1.User.GetUser;
using NUnit.Framework;
using Queries.Tests.Unit.Queries.v1.GetUser.Mocks.Mapper;
using Queries.Tests.Unit.Queries.v1.GetUser.Mocks.Service;
using System.Threading;

namespace Queries.Tests.Unit.Queries.v1.GetUser
{
    [TestFixture]
    public class GetUserQueryHandlerTest
    {
        [Test]
        [Category("v1")]
        public void Should_GetUser()
        {
            var result = new GetUserQueryHandler(
                new UserServiceMock().GetMock(),
                new MapperMock().GetMock()).Handle(new GetUserQuery(3), CancellationToken.None);

            Assert.IsNotNull(result);
        }

        [Test]
        [Category("v1")]
        public void ShouldNot_GetUser_UserNotFoundException()
        {
            var result = new GetUserQueryHandler(
                new UserServiceMock().GetMock_NoUser(),
                new MapperMock().GetMock()).Handle(new GetUserQuery(3), CancellationToken.None);

            Assert.ThrowsAsync<UserNotFoundException>(() => result);
        }
    }
}