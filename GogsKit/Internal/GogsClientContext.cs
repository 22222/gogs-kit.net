using GogsKit.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GogsKit.Internal
{
    internal class GogsClientContext
    {
        private readonly ICredentialsProvider credentialsProvider;
        private readonly UserAgent userAgent;

        public GogsClientContext(Uri apiUri, ICredentialsProvider credentialsProvider, UserAgent userAgent)
        {
            if (credentialsProvider == null) throw new ArgumentNullException(nameof(credentialsProvider));
            if (userAgent == null) throw new ArgumentNullException(nameof(userAgent));

            this.ApiUri = ValidateAndNormalizeApiUri(apiUri);
            this.credentialsProvider = credentialsProvider;
            this.userAgent = userAgent;
        }

        private static Uri ValidateAndNormalizeApiUri(Uri apiUri)
        {
            if (apiUri == null) throw new ArgumentNullException(nameof(apiUri));
            if (!apiUri.IsAbsoluteUri) throw new ArgumentException($"{nameof(apiUri)} is not an absolute URI", paramName: nameof(apiUri));
            if (!apiUri.AbsolutePath.EndsWith("/"))
            {
                apiUri = new Uri(apiUri.AbsoluteUri + '/');
            }
            return apiUri;
        }

        public Uri ApiUri { get; }

        public Uri CreateRequestUri(string relativePath)
        {
            return CreateRequestUri(relativePath, parameters: null);
        }

        public Uri CreateRequestUri(string relativePath, IEnumerable<KeyValuePair<string, string>> parameters)
        {
            var uriBuilder = new UriBuilder(ApiUri);
            if (!string.IsNullOrEmpty(relativePath))
            {
                if (string.IsNullOrEmpty(uriBuilder.Path))
                {
                    uriBuilder.Path = relativePath.TrimStart('/');
                }
                else
                {
                    uriBuilder.Path = uriBuilder.Path.TrimEnd('/') + '/' + relativePath.TrimStart('/');
                }
            }

            if (parameters != null)
            {
                var newQueryStringTokens = parameters.Select(kv => !string.IsNullOrEmpty(kv.Value)
                    ? $"{Uri.EscapeDataString(kv.Key)}={Uri.EscapeDataString(kv.Value)}"
                    : $"{Uri.EscapeDataString(kv.Key)}"
                );
                var newQueryString = string.Join("&", newQueryStringTokens);
                if (!string.IsNullOrEmpty(newQueryString))
                {
                    if (string.IsNullOrEmpty(uriBuilder.Query))
                    {
                        uriBuilder.Query = newQueryString;
                    }
                    else
                    {
                        uriBuilder.Query = uriBuilder.Query + '&' + newQueryString;
                    }
                }
            }

            return uriBuilder.Uri;
        }

        public async Task<HttpClient> CreateHttpClientAsync()
        {
            var messageHandler = CreateDefaultHttpMessageHandler();

            var ensureSuccessMessageHandler = new EnsureSuccessHandler();
            ensureSuccessMessageHandler.InnerHandler = messageHandler;
            messageHandler = ensureSuccessMessageHandler;

            var credentials = await credentialsProvider.GetCredentialsAsync();
            if (credentials == null)
            {
                credentials = Credentials.Anonymous;
            }

            if (credentials.CredentialsType == CredentialsType.Token)
            {
                var addTokenRequestParamHandler = new AddTokenRequestParamHandler(credentials.Secret);
                addTokenRequestParamHandler.InnerHandler = messageHandler;
                messageHandler = addTokenRequestParamHandler;
            }

            var client = new HttpClient(messageHandler, disposeHandler: true);
            client.DefaultRequestHeaders.UserAgent.TryParseAdd(userAgent.Value);
            if (credentials.CredentialsType == CredentialsType.Password)
            {
                var basicToken = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{credentials.Identifier}:{credentials.Secret}"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", basicToken);
            }
            return client;
        }

        protected virtual HttpMessageHandler CreateDefaultHttpMessageHandler()
        {
            return new HttpClientHandler();
        }

        private class EnsureSuccessHandler : DelegatingHandler
        {
            protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                HttpResponseMessage response;
                try
                {
                    response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    throw new GogsKitRequestException(ex.Message, ex)
                    {
                        RequestUri = request?.RequestUri,
                    };
                }

                if (response == null)
                {
                    throw new GogsKitRequestException("No response")
                    {
                        RequestUri = request?.RequestUri,
                    };
                }

                if (!response.IsSuccessStatusCode)
                {
                    string responseString = null;
                    if (response.Content != null)
                    {
                        try
                        {
                            responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                        }
                        catch (Exception)
                        {
                            responseString = null;
                        }
                    }

                    ErrorResultWrapper errorResultWrapper = null;
                    if (!string.IsNullOrEmpty(responseString) && response.Content.Headers.ContentType?.MediaType == "application/json")
                    {
                        JsonEntity.TryParseJson(responseString, out errorResultWrapper);
                    }

                    try
                    {
                        response.EnsureSuccessStatusCode();
                    }
                    catch (HttpRequestException ex)
                    {
                        var message = errorResultWrapper?.Error ?? ex.Message;

                        GogsKitResponseException responseException;
                        if (response.StatusCode == HttpStatusCode.Unauthorized)
                        {
                            responseException = new GogsKitUnauthorizedException(message, ex);
                        }
                        else if (response.StatusCode == HttpStatusCode.NotFound)
                        {
                            responseException = new GogsKitNotFoundException(message, ex);
                        }
                        else
                        {
                            responseException = new GogsKitResponseException(message, ex);
                        }
                        responseException.RequestUri = request?.RequestUri;
                        responseException.ResponseStatusCode = (int)response.StatusCode;
                        responseException.ResponseContent = responseString;
                        throw responseException;
                    }
                }
                return response;
            }
        }

        private class AddTokenRequestParamHandler : DelegatingHandler
        {
            private readonly string token;

            public AddTokenRequestParamHandler(string token)
            {
                if (token == null) throw new ArgumentNullException(nameof(token));
                this.token = token;
            }

            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                var uriBuilder = new UriBuilder(request.RequestUri);
                var newQuery = $"token={Uri.EscapeDataString(token)}";
                if (!string.IsNullOrWhiteSpace(uriBuilder.Query))
                {
                    newQuery += '&' + uriBuilder.Query;
                }
                uriBuilder.Query = newQuery;
                request.RequestUri = uriBuilder.Uri;

                return base.SendAsync(request, cancellationToken);
            }
        }
    }
}
