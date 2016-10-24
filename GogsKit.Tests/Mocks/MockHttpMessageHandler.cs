using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GogsKit.Mocks
{
    internal class MockHttpMessageHandler : HttpMessageHandler
    {
        private readonly Uri baseUri;
        private Func<HttpResponseMessage> defaultResponseFactory;
        private readonly IDictionary<Uri, Func<HttpResponseMessage>> responseFactories = new Dictionary<Uri, Func<HttpResponseMessage>>();

        public MockHttpMessageHandler(Uri baseUri)
        {
            if (baseUri == null) throw new ArgumentNullException(nameof(baseUri));
            this.baseUri = baseUri.AbsoluteUri.EndsWith("/") ? baseUri : new Uri(baseUri.AbsoluteUri + '/');
        }

        public void SetResponse(Uri requestUri, string responseJson)
        {
            Func<HttpResponseMessage> responseFactory = () => new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(responseJson, Encoding.UTF8, mediaType: "application/json")
            };
            SetResponse(requestUri, responseFactory);
        }

        public void SetResponse(Uri requestUri, HttpStatusCode statusCode, string responseJson = null)
        {
            Func<HttpResponseMessage> responseFactory = () => new HttpResponseMessage(statusCode)
            {
                Content = responseJson != null ? new StringContent(responseJson, Encoding.UTF8, mediaType: "application/json") : null
            };
            SetResponse(requestUri, responseFactory);
        }

        public void SetResponse(Uri requestUri, Func<HttpResponseMessage> responseMessageFactory)
        {
            var absoluteRequestUri = ResolveUri(requestUri);
            responseFactories[absoluteRequestUri] = responseMessageFactory;
        }

        public void SetDefaultResponse(string responseJson)
        {
            defaultResponseFactory = () => new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(responseJson, Encoding.UTF8, mediaType: "application/json")
            };
        }

        public void SetDefaultResponse(HttpStatusCode statusCode, string responseContent = null)
        {
            defaultResponseFactory = () => new HttpResponseMessage(statusCode)
            {
                Content = responseContent != null ? new StringContent(responseContent, Encoding.UTF8, mediaType: "application/json") : null
            };
        }

        public void SetDefaultResponse(Func<HttpResponseMessage> responseMessageFactory)
        {
            defaultResponseFactory = responseMessageFactory;
        }

        public void ClearAllResponses()
        {
            defaultResponseFactory = null;
            responseFactories.Clear();
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Func<HttpResponseMessage> responseMessageFactory;
            if (!responseFactories.TryGetValue(request.RequestUri, out responseMessageFactory))
            {
                responseMessageFactory = defaultResponseFactory;
            }
            var responseMessage = responseMessageFactory?.Invoke();
            return Task.FromResult(responseMessage);
        }

        private Uri ResolveUri(Uri requestUri)
        {
            Uri result = requestUri;
            if (!requestUri.IsAbsoluteUri)
            {
                result = new Uri(baseUri, requestUri);
            }
            return result;
        }
    }
}
