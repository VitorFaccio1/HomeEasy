using HomeEasy.Domain.Queries.v1.User.GetUsers;
using MediatR;

namespace HomeEasy.Domain.Queries.v1.User.GetUser
{
    public class GetUserQueryResponse : IRequest<GetUsersQueryResponse>
    {
        /// <summary>
        /// The user id.
        /// </summary>
        public long Id { get; set; }

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
