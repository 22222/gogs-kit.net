using GogsKit.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GogsKit
{
    /// <summary>
    /// The client for organizations.
    /// </summary>
    public class OrgsClient
    {
        private readonly GogsClientContext context;

        internal OrgsClient(GogsClientContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            this.context = context;
        }

        /// <summary>
        /// Returns the organization with the specified name.
        /// </summary>
        public async Task<OrganizationResult> GetAsync(string orgName, CancellationToken cancellationToken = default(CancellationToken))
        {
            var requestUrl = context.CreateRequestUri($"orgs/{orgName}");
            using (var httpClient = await context.CreateHttpClientAsync())
            {
                var response = await httpClient.GetAsync(requestUrl, cancellationToken: cancellationToken);
                var responseJson = await response.Content.ReadAsStringAsync();
                var result = JsonEntity.ParseJson<OrganizationResult>(responseJson);
                return result;
            }
        }

        /// <summary>
        /// Returns the teams in the specified organization.
        /// </summary>
        public async Task<IReadOnlyCollection<TeamResult>> GetTeamsAsync(string orgName, CancellationToken cancellationToken = default(CancellationToken))
        {
            var requestUrl = context.CreateRequestUri($"orgs/{orgName}/teams");
            using (var httpClient = await context.CreateHttpClientAsync())
            {
                var response = await httpClient.GetAsync(requestUrl, cancellationToken: cancellationToken);
                var responseJson = await response.Content.ReadAsStringAsync();
                var results = JsonEntity.ParseJsonArray<TeamResult>(responseJson);
                return results;
            }
        }
    }
}
