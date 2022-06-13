using CrossCutting.Exceptions;
using HomeEasy.Domain.Queries.v1.User.GetUsers;
using NUnit.Framework;
using Queries.Tests.Unit.Queries.v1.GetUsers.Mocks.Mapper;
using Queries.Tests.Unit.Queries.v1.GetUsers.Mocks.Service;
using System.Threading;

namespace Queries.Tests.Unit.Queries.v1.GetUsers
{
    [TestFixture]
    public class GetUsersQueryHandlerTest
    {
        [Test]
        [Category("v1")]
        public void Should_GetUsers()
        {
            var result = new GetUsersQueryHandler(
                new UserServiceMock().GetMock(),
                new MapperMock().GetMock()).Handle(new GetUsersQuery(), CancellationToken.None);

            Assert.IsNotNull(result);
        }

        [Test]
        [Category("v1")]
        public void ShouldNot_GetUsers_UsersNotFoundException()
        {
            var result = new GetUsersQueryHandler(
                new UserServiceMock().GetMock_NoUsers(),
                new MapperMock().GetMock()).Handle(new GetUsersQuery(), CancellationToken.None);

            Assert.ThrowsAsync<UsersNotFoundException>(() => result);
        }
    }
}