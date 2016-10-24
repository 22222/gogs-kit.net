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
    /// The client for users.
    /// </summary>
    public class UsersClient
    {
        private readonly GogsClientContext context;

        internal UsersClient(GogsClientContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            this.context = context;
        }

        /// <summary>
        /// Search for users.
        /// </summary>
        public async Task<IReadOnlyCollection<UserResult>> SearchAsync(string query, int? limit = null)
        {
            if (string.IsNullOrEmpty(query))
            {
                return Array.Empty<UserResult>();
            }

            var parameters = new Dictionary<string, string>(2);
            parameters.Add("q", query);
            if (limit.HasValue)
            {
                parameters.Add("limit", limit.Value.ToString());
            }

            var requestUrl = context.CreateRequestUri("users/search", parameters);
            using (var httpClient = await context.CreateHttpClientAsync())
            {
                var response = await httpClient.GetAsync(requestUrl);
                var responseJson = await response.Content.ReadAsStringAsync();
                var result = JsonEntity.ParseJson<DataResultWrapper<UserResult[]>>(responseJson);
                return result?.Data ?? Array.Empty<UserResult>();
            }
        }

        /// <summary>
        /// Returns the user with the specified username.
        /// </summary>
        public async Task<UserResult> GetAsync(string username)
        {
            if (username == null) throw new ArgumentNullException(nameof(username));
            if (string.IsNullOrWhiteSpace(username)) throw new ArgumentException($"{nameof(username)} is required", paramName: nameof(username));

            var requestUrl = context.CreateRequestUri($"users/{username}");
            using (var httpClient = await context.CreateHttpClientAsync())
            {
                var response = await httpClient.GetAsync(requestUrl);
                var responseJson = await response.Content.ReadAsStringAsync();
                var result = JsonEntity.ParseJson<UserResult>(responseJson);
                return result;
            }
        }

        /// <summary>
        /// Returns the access tokens for the user with the specified username.
        /// </summary>
        public async Task<IReadOnlyCollection<TokenResult>> GetTokensAsync(string username)
        {
            if (username == null) throw new ArgumentNullException(nameof(username));
            if (string.IsNullOrWhiteSpace(username)) throw new ArgumentException($"{nameof(username)} is required", paramName: nameof(username));

            var requestUrl = context.CreateRequestUri($"users/{username}/tokens");
            using (var httpClient = await context.CreateHttpClientAsync())
            {
                var response = await httpClient.GetAsync(requestUrl);
                var responseJson = await response.Content.ReadAsStringAsync();
                var results = JsonEntity.ParseJsonArray<TokenResult>(responseJson);
                return results ?? Array.Empty<TokenResult>();
            }
        }

        /// <summary>
        /// Creates and returns a token with the specified <paramref name="name"/> for the user with the specified username.
        /// </summary>
        public async Task<TokenResult> CreateTokenAsync(string username, string name)
        {
            if (username == null) throw new ArgumentNullException(nameof(username));
            if (string.IsNullOrWhiteSpace(username)) throw new ArgumentException($"{nameof(username)} is required", paramName: nameof(username));

            if (name == null) throw new ArgumentNullException(nameof(name));
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException($"{nameof(name)} is required", paramName: nameof(name));

            var requestUrl = context.CreateRequestUri($"users/{username}/tokens");
            using (var httpClient = await context.CreateHttpClientAsync())
            using (var requestContent = new FormUrlEncodedContent(new Dictionary<string, string> { ["name"] = name }))
            {
                var response = await httpClient.PostAsync(requestUrl, requestContent);
                var responseJson = await response.Content.ReadAsStringAsync();
                var result = JsonEntity.ParseJson<TokenResult>(responseJson);
                return result;
            }
        }

        /// <summary>
        /// Returns the keys for the user with the specified username.
        /// </summary>
        public async Task<IReadOnlyCollection<KeyResult>> GetKeysAsync(string username)
        {
            if (username == null) throw new ArgumentNullException(nameof(username));
            if (string.IsNullOrWhiteSpace(username)) throw new ArgumentException($"{nameof(username)} is required", paramName: nameof(username));

            var requestUrl = context.CreateRequestUri($"users/{username}/keys");
            using (var httpClient = await context.CreateHttpClientAsync())
            {
                var response = await httpClient.GetAsync(requestUrl);
                var responseJson = await response.Content.ReadAsStringAsync();
                var results = JsonEntity.ParseJsonArray<KeyResult>(responseJson);
                return results ?? Array.Empty<KeyResult>();
            }
        }

        /// <summary>
        /// Returns the followers of the user with the specified username.
        /// </summary>
        public async Task<IReadOnlyCollection<UserResult>> GetFollowersAsync(string username)
        {
            if (username == null) throw new ArgumentNullException(nameof(username));
            if (string.IsNullOrWhiteSpace(username)) throw new ArgumentException($"{nameof(username)} is required", paramName: nameof(username));

            var requestUrl = context.CreateRequestUri($"users/{username}/followers");
            using (var httpClient = await context.CreateHttpClientAsync())
            {
                var response = await httpClient.GetAsync(requestUrl);
                var responseJson = await response.Content.ReadAsStringAsync();
                var results = JsonEntity.ParseJsonArray<UserResult>(responseJson);
                return results ?? Array.Empty<UserResult>();
            }
        }

        /// <summary>
        /// Returns the organizations of the user with the specified username.
        /// </summary>
        public async Task<IReadOnlyCollection<OrganizationResult>> GetOrganizationsAsync(string username)
        {
            if (username == null) throw new ArgumentNullException(nameof(username));
            if (string.IsNullOrWhiteSpace(username)) throw new ArgumentException($"{nameof(username)} is required", paramName: nameof(username));

            var requestUrl = context.CreateRequestUri($"users/{username}/orgs");
            using (var httpClient = await context.CreateHttpClientAsync())
            {
                var response = await httpClient.GetAsync(requestUrl);
                var responseJson = await response.Content.ReadAsStringAsync();
                var results = JsonEntity.ParseJsonArray<OrganizationResult>(responseJson);
                return results ?? Array.Empty<OrganizationResult>();
            }
        }
    }
}
