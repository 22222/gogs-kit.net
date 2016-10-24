using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GogsKit.Exceptions
{
    /// <summary>
    /// The base for all GogsKit exceptions.
    /// </summary>
    public class GogsKitException : Exception
    {
        /// <summary>
        /// Constructs an exception with a message.
        /// </summary>
        public GogsKitException(string message) : base(message) { }

        /// <summary>
        /// Constructs an exception that wraps another exception.
        /// </summary>
        public GogsKitException(string message, Exception innerException) : base(message, innerException) { }
    }
}
