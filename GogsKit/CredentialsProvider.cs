using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GogsKit
{
    /// <summary>
    /// Provides credentials to use with the GOGS API.
    /// </summary>
    public interface ICredentialsProvider
    {
        /// <summary>
        /// Returns the current credentials.
        /// </summary>
        /// <returns>the current credentials</returns>
        Task<Credentials> GetCredentialsAsync();
    }

    /// <summary>
    /// An implementation of <see cref="ICredentialsProvider"/> that always returns the same credentials.
    /// </summary>
    public class CredentialsProvider : ICredentialsProvider
    {
        /// <summary>
        /// Constructs an anonymous credentials provider.
        /// </summary>
        public CredentialsProvider() : this(credentials: null) { }

        /// <summary>
        /// Constructs a provider for the specified credentials.
        /// </summary>
        /// <param name="credentials">the credentials that will always be provided, or null for anonymous credentials</param>
        public CredentialsProvider(Credentials credentials)
        {
            Credentials = credentials ?? Credentials.Anonymous;
        }

        /// <summary>
        /// The provided credentials.
        /// </summary>
        public Credentials Credentials { get; }

        Task<Credentials> ICredentialsProvider.GetCredentialsAsync()
        {
            return Task.FromResult(Credentials);
        }
    }
}
