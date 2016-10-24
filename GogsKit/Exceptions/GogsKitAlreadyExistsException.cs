using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GogsKit.Exceptions
{
    /// <summary>
    ///  An exception thrown by a <see cref="GogsClient"/> when you're trying to create a thing that already exists.
    /// </summary>
    public class GogsKitAlreadyExistsException : GogsKitResponseException
    {
        /// <summary>
        /// Constructs an exception with a message.
        /// </summary>
        public GogsKitAlreadyExistsException(string message) : base(message)
        {
            ResponseStatusCode = 422;
        }

        /// <summary>
        /// Constructs an exception that wraps another exception.
        /// </summary>
        public GogsKitAlreadyExistsException(string message, Exception innerException) : base(message, innerException)
        {
            ResponseStatusCode = 422;
        }
    }
}
