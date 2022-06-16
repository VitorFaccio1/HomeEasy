using MediatR;

namespace HomeEasy.Domain.Commands.v1.User.DeleteUser
{
    public class DeleteUserCommand : IRequest
    {
        public DeleteUserCommand(long id)
        {
            Id = id;
        }

        /// <summary>
        /// The user ID.
        /// </summary>
        public long Id { get; set; }
    }
}