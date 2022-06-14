using System.Net;

namespace CrossCutting.Exceptions
{
    public sealed class UsersNotFoundException : HttpCustomException
    {
        public UsersNotFoundException(string message = "UsersNotFoundException",
            HttpStatusCode httpStatusCode = HttpStatusCode.NotFound) : base(message, httpStatusCode)
        {
        }
    }
}