using AutoMapper;
using Infrastructure.Data.Entities.User;
using Infrastructure.Data.Interfaces;
using MediatR;

namespace HomeEasy.Domain.Commands.v1.User.AddUser
{
    public class AddUserCommandHandler : IRequestHandler<AddUserCommand, Unit>
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public AddUserCommandHandler(IUserService userService, IMapper mapper)
        {
            _mapper = mapper;
            _userService = userService;
        }

        public async Task<Unit> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            var user = _mapper.Map<AddUserCommand, UserEntity>(request);

            await _userService.AddUserAsync(user);

            return Unit.Value;
        }
    }
}