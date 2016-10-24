using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GogsKit.Exceptions
{
    /// <summary>
    /// An exception from an API request.
    /// </summary>
    public class GogsKitRequestException : GogsKitException
    {
        /// <summary>
        /// Constructs an exception with a message.
        /// </summary>
        public GogsKitRequestException(string message) : base(message) { }

        /// <summary>
        /// Constructs an exception that wraps another exception.
        /// </summary>
        public GogsKitRequestException(string message, Exception innerException) : base(message, innerException) { }

        /// <summary>
        /// The URI of the request.
        /// </summary>
        public Uri RequestUri { get; set; }
    }
}
