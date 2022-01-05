
using System.Net;

namespace API.Services.Responses
{
    public class Response<T>
    {
        public Response(HttpStatusCode statusCode, T content, string errorMessage = null)
        {
            StatusCode = (int)statusCode;
            Content = content;
            ErrorMessage = errorMessage;
        }

        /// <summary>
        /// Status Code of the Request
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// Error Code of the Request
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Generic Object served as result
        /// </summary>
        public T Content { get; set; }
    }
}
