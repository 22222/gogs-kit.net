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
    /// The client for the current user.
    /// </summary>
    public class UserClient
    {
        private readonly GogsClientContext context;

        internal UserClient(GogsClientContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            this.context = context;
        }

        /// <summary>
        /// Returns the organizations of the current user.
        /// </summary>
        public async Task<IReadOnlyCollection<OrganizationResult>> GetOrganizationsAsync()
        {
            var requestUrl = context.CreateRequestUri($"user/orgs");
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
