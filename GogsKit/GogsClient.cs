using GogsKit.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GogsKit
{
    /// <summary>
    /// A client for using the GOGS API v1.
    /// </summary>
    public class GogsClient
    {
        /// <summary>
        /// Constructs a client that connects to the specified API url.
        /// </summary>
        /// <param name="apiUrl">the api url (e.g., "https://localhost/gogs/api/v1")</param>
        public GogsClient(string apiUrl)
            : this(new Uri(apiUrl)) { }

        /// <summary>
        /// Constructs a client that connects to the specified APIuri.
        /// </summary>
        /// <param name="apiUri">the api url (e.g., "https://localhost/gogs/api/v1")</param>
        public GogsClient(Uri apiUri)
            : this(apiUri, Credentials.Anonymous) { }

        /// <summary>
        /// Constructs a client that connects to the specified API url with the specified credentials.
        /// </summary>
        /// <param name="apiUrl">the api url (e.g., "https://localhost/gogs/api/v1")</param>
        /// <param name="credentials">the authentication credentials for the API</param>
        public GogsClient(string apiUrl, Credentials credentials)
            : this(new Uri(apiUrl), credentials) { }

        /// <summary>
        /// Constructs a client that connects to the specified API uri with the specified credentials.
        /// </summary>
        /// <param name="apiUri">the api url (e.g., "https://localhost/gogs/api/v1")</param>
        /// <param name="credentials">the authentication credentials for the API</param>
        public GogsClient(Uri apiUri, Credentials credentials)
            : this(apiUri, new CredentialsProvider(credentials), UserAgent.Default) { }

        /// <summary>
        /// Constructs a client that connects to the specified API uri with the specified credentials and user-agent.
        /// </summary>
        /// <param name="apiUri">the api url (e.g., "https://localhost/gogs/api/v1")</param>
        /// <param name="credentialsProvider">the provider for API authentication credentials</param>
        /// <param name="userAgent">the HTTP User-Agent to include in all requests to the API</param>
        public GogsClient(Uri apiUri, ICredentialsProvider credentialsProvider, UserAgent userAgent)
            : this(new GogsClientContext(apiUri, credentialsProvider, userAgent)) { }

        internal GogsClient(GogsClientContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            User = new UserClient(context);
            Users = new UsersClient(context);
            Orgs = new OrgsClient(context);
            Admin = new AdminClient(context);
        }

        /// <summary>
        /// The current user client.
        /// </summary>
        public UserClient User { get; }

        /// <summary>
        /// The user client.
        /// </summary>
        public UsersClient Users { get; }

        /// <summary>
        /// The orgs client.
        /// </summary>
        public OrgsClient Orgs { get; }

        /// <summary>
        /// The admin client.
        /// </summary>
        public AdminClient Admin { get; }
    }
}
