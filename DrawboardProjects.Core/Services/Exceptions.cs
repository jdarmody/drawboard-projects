using System;
using System.Net;
using System.Net.Http;

namespace DrawboardProjects.Core.Services
{
    /// <summary>
    /// Represents errors where a service requiring authentication is attempted to be used
    /// and the user is not authenticated.
    /// </summary>
    public class NotAuthenticatedException : Exception
    {
        public NotAuthenticatedException()
        {
        }

        public NotAuthenticatedException(string message)
            : base(message)
        {
        }

        public NotAuthenticatedException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
    /// <summary>
    /// Represents errors when a <see cref="HttpResponseMessage"/> indicates the request has failed.
    /// </summary>
    public class HttpResponseException : Exception
    {
        public HttpStatusCode StatusCode { get; }
        public HttpResponseMessage ResponseMessage { get; }

        public HttpResponseException(HttpStatusCode statusCode)
            : base(statusCode.ToString())
        {
            StatusCode = statusCode;
        }

        public HttpResponseException(HttpResponseMessage responseMessage)
            : base(responseMessage.ReasonPhrase)
        {
            ResponseMessage = responseMessage;
        }
    }
}
