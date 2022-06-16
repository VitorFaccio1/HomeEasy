using CrossCutting.Exceptions;
using Infrastructure.Data.Entities.User;
using Infrastructure.Data.Interfaces;
using MediatR;

namespace HomeEasy.Domain.Commands.v1.User.DeleteUser
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Unit>
    {
        private readonly IUserService _userService;

        public DeleteUserCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await GetUser(request);

            user.IsActive = false;

            await _userService.UpdateUserAsync(user);

            return Unit.Value;
        }

        private async Task<UserEntity> GetUser(DeleteUserCommand request)
        {
            var user = await _userService.GetUserAsync(request.Id);

            if (user == null || user.IsActive == false)
                throw new UserNotFoundException();

            return user;
        }
    }
}