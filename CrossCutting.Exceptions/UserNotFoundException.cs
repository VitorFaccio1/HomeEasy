using System.Net;

namespace CrossCutting.Exceptions
{
    public sealed class UserNotFoundException : HttpCustomException
    {
        public UserNotFoundException(string message = "UserNotFoundException",
            HttpStatusCode httpStatusCode = HttpStatusCode.NotFound) : base(message, httpStatusCode)
        {
        }
    }
}