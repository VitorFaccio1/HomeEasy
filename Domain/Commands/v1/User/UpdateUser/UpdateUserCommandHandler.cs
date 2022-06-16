using CrossCutting.Exceptions;
using Infrastructure.Data.Entities.User;
using Infrastructure.Data.Interfaces;
using MediatR;

namespace HomeEasy.Domain.Commands.v1.User.UpdateUser
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Unit>
    {
        private readonly IUserService _userService;

        public UpdateUserCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await GetUser(request);

            var somethingChange = ChangeUserInformation(request, user);

            if (somethingChange)
                await _userService.UpdateUserAsync(user);

            return Unit.Value;
        }

        private static bool ChangeUserInformation(UpdateUserCommand request, UserEntity user)
        {
            bool somethingChange = false;

            if (request.Email != user.Email)
            {
                user.Email = request.Email;

                somethingChange = true;
            }

            if (request.Password != user.Password)
            {
                user.Password = request.Password;

                somethingChange = true;
            }

            if (request.Name != user.Name)
            {
                user.Name = request.Name;

                somethingChange = true;
            }

            return somethingChange;
        }

        private async Task<UserEntity> GetUser(UpdateUserCommand request)
        {
            var user = await _userService.GetUserAsync(request.Id);

            if (user == null || user.IsActive == false)
                throw new UserNotFoundException();

            return user;
        }
    }
}
