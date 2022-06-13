using AutoFixture;
using AutoMapper;
using HomeEasy.Domain.Commands.v1.User.AddUser;
using Infrastructure.Data.Entities.User;
using NSubstitute;

namespace Domain.Tests.Unit.Commands.v1.AddUser.Mocks.Mapper
{
    public sealed class MapperMock
    {
        public IMapper GetMock()
        {
            var mock = Substitute.For<IMapper>();

            mock.Map<AddUserCommand, UserEntity>(Arg.Any<AddUserCommand>())
                .Returns(new Fixture().Create<UserEntity>());

            return mock;
        }
    }
}