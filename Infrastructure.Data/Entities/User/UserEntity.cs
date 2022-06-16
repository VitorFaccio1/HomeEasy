namespace Infrastructure.Data.Entities.User
{
    public class UserEntity
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

        /// <summary>
        /// If the user is active.
        /// </summary>
        public bool? IsActive { get; set; }
    }
}
