using System.Net;

namespace CrossCutting.Exceptions
{
    public class HttpCustomException : Exception
    {
        public HttpCustomException(string message, HttpStatusCode responseCode) : base(message)
        {
            ResponseCode = responseCode;
        }
        public HttpStatusCode ResponseCode { get; }
    }
}
