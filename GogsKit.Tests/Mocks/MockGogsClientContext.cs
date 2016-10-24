using GogsKit.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Moq;
using System.Threading;

namespace GogsKit.Mocks
{
    internal class MockGogsClientContext : GogsClientContext
    {
        public MockGogsClientContext()
            : this(Credentials.Anonymous) { }

        public MockGogsClientContext(Credentials credentials)
            : this(new CredentialsProvider(credentials)) { }

        public MockGogsClientContext(ICredentialsProvider credentialsProvider)
            : this(new Uri(@"https://localhost/gogs/api/v1"), credentialsProvider, UserAgent.Default) { }

        public MockGogsClientContext(Uri apiUri, ICredentialsProvider credentialsProvider, UserAgent userAgent)
            : base(apiUri, credentialsProvider, userAgent)
        {
            MockHttpMessageHandler = new MockHttpMessageHandler(apiUri);
        }

        public MockHttpMessageHandler MockHttpMessageHandler { get; }

        protected override HttpMessageHandler CreateDefaultHttpMessageHandler()
        {
            return MockHttpMessageHandler;
        }
    }
}
