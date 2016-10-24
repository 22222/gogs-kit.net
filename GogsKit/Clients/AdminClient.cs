using GogsKit.Exceptions;
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
    /// The client for site administration.
    /// </summary>
    public class AdminClient
    {
        private readonly GogsClientContext context;

        internal AdminClient(GogsClientContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            this.context = context;
        }

        /// <summary>
        /// Creates a user.
        /// </summary>
        public async Task<UserResult> CreateUserAsync(CreateUserOption user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            var requestUrl = context.CreateRequestUri($"admin/users");
            using (var httpClient = await context.CreateHttpClientAsync())
            using (var requestContent = new StringContent(user.ToJson(), Encoding.UTF8, mediaType: "application/json"))
            {
                var response = await httpClient.PostAsync(requestUrl, requestContent);
                var responseJson = await response.Content.ReadAsStringAsync();
                var result = JsonEntity.ParseJson<UserResult>(responseJson);
                return result;
            }
        }

        /// <summary>
        /// Edits the specified user.
        /// </summary>
        public async Task<UserResult> EditUserAsync(string username, EditUserOption user)
        {
            if (username == null) throw new ArgumentNullException(nameof(username));
            if (string.IsNullOrWhiteSpace(username)) throw new ArgumentException($"{nameof(username)} is required", paramName: nameof(username));

            if (user == null) throw new ArgumentNullException(nameof(user));

            var requestUrl = context.CreateRequestUri($"admin/users/{username}");
            using (var httpClient = await context.CreateHttpClientAsync())
            using (var requestContent = new StringContent(user.ToJson(), Encoding.UTF8, mediaType: "application/json"))
            {
                var response = await httpClient.PutAsync(requestUrl, requestContent);
                var responseJson = await response.Content.ReadAsStringAsync();
                var result = JsonEntity.ParseJson<UserResult>(responseJson);
                return result;
            }
        }

        /// <summary>
        /// Deletes the specified user.
        /// </summary>
        public async Task DeleteUserAsync(string username)
        {
            if (username == null) throw new ArgumentNullException(nameof(username));
            if (string.IsNullOrWhiteSpace(username)) throw new ArgumentException($"{nameof(username)} is required", paramName: nameof(username));

            var requestUrl = context.CreateRequestUri($"admin/users/{username}");
            using (var httpClient = await context.CreateHttpClientAsync())
            {
                await httpClient.DeleteAsync(requestUrl);
            }
        }

        /// <summary>
        /// Creates an organization with the specified owner.
        /// </summary>
        public async Task<OrganizationResult> CreateOrg(string username, CreateOrgOption org)
        {
            if (username == null) throw new ArgumentNullException(nameof(username));
            if (string.IsNullOrWhiteSpace(username)) throw new ArgumentException($"{nameof(username)} is required", paramName: nameof(username));

            if (org == null) throw new ArgumentNullException(nameof(org));

            var requestUrl = context.CreateRequestUri($"admin/{username}/orgs");
            using (var httpClient = await context.CreateHttpClientAsync())
            using (var requestContent = new StringContent(org.ToJson(), Encoding.UTF8, mediaType: "application/json"))
            {
                var response = await httpClient.PostAsync(requestUrl, requestContent);
                var responseJson = await response.Content.ReadAsStringAsync();
                var result = JsonEntity.ParseJson<OrganizationResult>(responseJson);
                return result;
            }
        }

        /// <summary>
        /// Creates a team in the specified organization.
        /// </summary>
        public async Task<TeamResult> CreateTeam(string orgName, CreateTeamOption team)
        {
            if (orgName == null) throw new ArgumentNullException(nameof(orgName));
            if (string.IsNullOrWhiteSpace(orgName)) throw new ArgumentException($"{nameof(orgName)} is required", paramName: nameof(orgName));

            if (team == null) throw new ArgumentNullException(nameof(team));

            var requestUrl = context.CreateRequestUri($"admin/orgs/{orgName}/teams");
            using (var httpClient = await context.CreateHttpClientAsync())
            using (var requestContent = new StringContent(team.ToJson(), Encoding.UTF8, mediaType: "application/json"))
            {
                HttpResponseMessage response;
                try
                {
                    response = await httpClient.PostAsync(requestUrl, requestContent);
                }
                catch (GogsKitResponseException ex)
                {
                    if (ex.ResponseStatusCode == 422)
                    {
                        throw new GogsKitAlreadyExistsException($"The team with name \"{team.Name}\" already exists, or there's a bug in your version of GOGS that doesn't allow you to create a second team for an organization.");
                    }
                    throw;
                }
                var responseJson = await response.Content.ReadAsStringAsync();
                var result = JsonEntity.ParseJson<TeamResult>(responseJson);
                return result;
            }
        }

        /// <summary>
        /// Adds the specified user to the specified team, or does nothing if the user is already in the team.
        /// </summary>
        public async Task AddTeamMember(int teamId, string username)
        {
            if (teamId <= 0) throw new ArgumentException($"{nameof(teamId)} must be positive", paramName: nameof(teamId));

            if (username == null) throw new ArgumentNullException(nameof(username));
            if (string.IsNullOrWhiteSpace(username)) throw new ArgumentException($"{nameof(username)} is required", paramName: nameof(username));

            var requestUrl = context.CreateRequestUri($"admin/teams/{teamId}/members/{username}");
            using (var httpClient = await context.CreateHttpClientAsync())
            {
                await httpClient.PutAsync(requestUrl, content: null);
            }
        }

        /// <summary>
        /// Removes the specified user from the specified team.
        /// </summary>
        public async Task RemoveTeamMember(int teamId, string username)
        {
            if (teamId <= 0) throw new ArgumentException($"{nameof(teamId)} must be positive", paramName: nameof(teamId));

            if (username == null) throw new ArgumentNullException(nameof(username));
            if (string.IsNullOrWhiteSpace(username)) throw new ArgumentException($"{nameof(username)} is required", paramName: nameof(username));

            var requestUrl = context.CreateRequestUri($"admin/teams/{teamId}/members/{username}");
            using (var httpClient = await context.CreateHttpClientAsync())
            {
                await httpClient.DeleteAsync(requestUrl);
            }
        }
    }
}
