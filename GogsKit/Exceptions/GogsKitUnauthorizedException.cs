using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GogsKit.Exceptions
{
    /// <summary>
    ///  An exception from a 401 Unauthorized API response.
    /// </summary>
    public class GogsKitUnauthorizedException : GogsKitResponseException
    {
        /// <summary>
        /// Constructs an exception with a message.
        /// </summary>
        public GogsKitUnauthorizedException(string message) : base(message)
        {
            ResponseStatusCode = (int)HttpStatusCode.Unauthorized;
        }

        /// <summary>
        /// Constructs an exception that wraps another exception.
        /// </summary>
        public GogsKitUnauthorizedException(string message, Exception innerException) : base(message, innerException)
        {
            ResponseStatusCode = (int)HttpStatusCode.Unauthorized;
        }
    }
}
