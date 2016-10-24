using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GogsKit.Exceptions
{
    /// <summary>
    /// An exception from an API response.
    /// </summary>
    public class GogsKitResponseException : GogsKitRequestException
    {
        /// <summary>
        /// Constructs an exception with a message.
        /// </summary>
        public GogsKitResponseException(string message) : base(message) { }

        /// <summary>
        /// Constructs an exception that wraps another exception.
        /// </summary>
        public GogsKitResponseException(string message, Exception innerException) : base(message, innerException) { }

        /// <summary>
        /// The HTTP status code of the response.
        /// </summary>
        public int? ResponseStatusCode { get; set; }

        /// <summary>
        /// The string content of the response.
        /// </summary>
        public string ResponseContent { get; set; }
    }
}
