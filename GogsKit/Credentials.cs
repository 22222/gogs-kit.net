using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GogsKit
{
    /// <summary>
    /// Credentials for authenticating with GOGS.
    /// </summary>
    public class Credentials
    {
        /// <summary>
        /// Credentials for anonymous access.
        /// </summary>
        public static Credentials Anonymous { get; } = new Credentials(CredentialsType.Anonymous, identifier: null, secret: null);

        /// <summary>
        /// Creates token credentials.
        /// </summary>
        /// <param name="token">the API access token</param>
        /// <exception cref="ArgumentNullException">if <paramref name="token"/> is null</exception>
        public Credentials(string token)
            : this(CredentialsType.Token, identifier: null, secret: token)
        {
            if (token == null) throw new ArgumentNullException(nameof(token));
        }

        /// <summary>
        /// Creates password credetials.
        /// </summary>
        /// <param name="username">the user's unique username</param>
        /// <param name="password">the password for the user</param>
        /// <exception cref="ArgumentNullException">if <paramref name="username"/> or <paramref name="password"/> is null</exception>
        public Credentials(string username, string password)
            : this(CredentialsType.Password, identifier: username, secret: password)
        {
            if (username == null) throw new ArgumentNullException(nameof(username));
            if (password == null) throw new ArgumentNullException(nameof(password));
        }

        private Credentials(CredentialsType credentialsType, string identifier, string secret)
        {
            Identifier = identifier;
            Secret = secret;
            CredentialsType = credentialsType;
        }

        /// <summary>
        /// The type of these credentials.
        /// </summary>
        public CredentialsType CredentialsType { get; }

        /// <summary>
        /// The identifier for these credentials (like a username or client id).
        /// </summary>
        public string Identifier { get; }

        /// <summary>
        /// The secret of these credentials (like a password or access token).
        /// </summary>
        public string Secret { get; }
    }

    /// <summary>
    /// The types of API credentials.
    /// </summary>
    public enum CredentialsType
    {
        /// <summary>
        /// No authentication.
        /// </summary>
        Anonymous,

        /// <summary>
        /// Username/password authentication.
        /// </summary>
        Password,

        /// <summary>
        /// Access token authentication.
        /// </summary>
        Token
    }
}
