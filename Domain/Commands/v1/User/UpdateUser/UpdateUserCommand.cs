using MediatR;
using System.Text.Json.Serialization;

namespace HomeEasy.Domain.Commands.v1.User.UpdateUser
{
    public class UpdateUserCommand : IRequest
    {
        public UpdateUserCommand(long id)
        {
            Id = id;
        }

        /// <summary>
        /// The user ID.
        /// </summary>
        [JsonIgnore]
        public long Id { get; set; }

        /// <summary>
        /// If the user is active.
        /// </summary>
        [JsonIgnore]
        public bool IsActive { get; set; }

        /// <summary>
        /// The username.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// The user email.
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// The user password.
        /// </summary>
        public string? Password { get; set; }
    }
}