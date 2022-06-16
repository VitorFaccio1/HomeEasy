using MediatR;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace HomeEasy.Domain.Commands.v1.User.AddUser
{
    public class AddUserCommand : IRequest
    {
        /// <summary>
        /// The user ID.
        /// </summary>
        [JsonIgnore]
        [Key]
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