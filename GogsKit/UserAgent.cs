using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GogsKit
{
    /// <summary>
    /// A simplified representation of an HTTP UserAgent.  It supports only one product and an optional comment.
    /// </summary>
    public class UserAgent
    {
        /// <summary>
        /// Returns a default <see cref="UserAgent"/>.
        /// </summary>
        public static UserAgent Default => new UserAgent(Assembly.GetAssembly(typeof(UserAgent)));

        /// <summary>
        /// Constructs a user-agent for the specified .NET assembly.
        /// </summary>
        /// <param name="productAssembly">the assembly that the user-agent is for</param>
        public UserAgent(Assembly productAssembly) : this(
                productName: productAssembly?.GetCustomAttribute<AssemblyTitleAttribute>()?.Title
                    ?? productAssembly?.GetName().Name,
                productVersion: productAssembly?.GetName().Version.ToString()

            )
        { }

        /// <summary>
        /// Constructs a user-agent for the specified product.
        /// </summary>
        /// <param name="productName">the name of the product</param>
        /// <param name="productVersion">the version of the product, or null</param>
        /// <exception cref="ArgumentException">if any parameter contains a space, or if all parameters are null or empty</exception>
        public UserAgent(string productName, string productVersion)
            : this(productName: productName, productVersion: productVersion, comment: null) { }

        /// <summary>
        /// Constructs a user-agent for the specified product and comment.
        /// </summary>
        /// <param name="productName">the name of the product</param>
        /// <param name="productVersion">the version of the product, or null</param>
        /// <param name="comment">the user-agent comment, or null</param>
        /// <exception cref="ArgumentException">if <paramref name="productName"/> or <paramref name="productVersion"/> contains a space, or if all parameters are null or empty</exception>
        public UserAgent(string productName, string productVersion, string comment)
        {
            if (string.IsNullOrEmpty(productName) && string.IsNullOrEmpty(productVersion) && string.IsNullOrEmpty(comment))
            {
                throw new ArgumentException("Must have at least one non-empty part of the user-agent");
            }
            if (productName != null && productName.Contains(' '))
            {
                throw new ArgumentException($"{nameof(productName)} cannot contain a space: \"{productName}\"", paramName: nameof(productName));
            }
            if (productVersion != null && productVersion.Contains(' '))
            {
                throw new ArgumentException($"{nameof(productVersion)} cannot contain a space: \"{productVersion}\"", paramName: nameof(productVersion));
            }
            Value = BuildUserAgentString(productName, productVersion, comment);
        }

        private static string BuildUserAgentString(string productName, string productVersion, string comment)
        {
            var sb = new StringBuilder();
            sb.Append(productName);
            if (!string.IsNullOrEmpty(productVersion))
            {
                sb.Append('/');
                sb.Append(productVersion);
            }

            if (!string.IsNullOrEmpty(comment))
            {
                if (sb.Length > 0)
                {
                    sb.Append(' ');
                }
                sb.Append('(');
                sb.Append(comment);
                sb.Append(')');
            }
            return sb.ToString();
        }

        /// <summary>
        /// The user-agent string (e.g., "CERN-LineMode/2.15 (comment)").
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// Returns the <see cref="Value"/> of this user-agent.
        /// </summary>
        public override string ToString()
        {
            return Value;
        }
    }
}
