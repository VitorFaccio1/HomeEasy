namespace Infrastructure.Service.Services.User
{
    public class UserServiceRequest
    {
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
