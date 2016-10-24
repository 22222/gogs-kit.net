using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GogsKit.Exceptions
{
    /// <summary>
    /// An exception resulting from a parse failure in an API response.
    /// </summary>
    public class GogsKitResultParseException : GogsKitException
    {
        /// <summary>
        /// Constructs an exception with a message.
        /// </summary>
        public GogsKitResultParseException(string message) : base(message) { }

        /// <summary>
        /// Constructs an exception that wraps another exception.
        /// </summary>
        public GogsKitResultParseException(string message, Exception innerException) : base(message, innerException) { }
    }
}
