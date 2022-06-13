using System.Net;

namespace CrossCutting.Exceptions
{
    public sealed class UsersNotFoundException : Exception
    {
        public UsersNotFoundException(string message = "UsersNotFoundException", HttpStatusCode httpStatusCode = HttpStatusCode.NotFound)
        {
        }
    }
}