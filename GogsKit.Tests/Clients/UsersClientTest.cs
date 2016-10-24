using GogsKit.Exceptions;
using GogsKit.Mocks;
using GogsKit.Results;
using LatticeObjectTree.Asserts;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GogsKit.Clients
{
    public class UsersClientTest
    {
        private MockGogsClientContext clientContext;
        private UsersClient client;

        [SetUp]
        public void SetUp()
        {
            clientContext = new MockGogsClientContext();
            client = new UsersClient(clientContext);
        }

        [Test]
        public async Task SearchAsync_Sample()
        {
            var requestUri = new Uri("users/search?q=temp", UriKind.Relative);
            var json = TestFileProvider.ReadText("usersDataWrapper.json");
            clientContext.MockHttpMessageHandler.SetResponse(requestUri, json);

            var users = await client.SearchAsync("temp");
            Assert.IsNotNull(users);

            var expected = new[]
            {
                new UserResult
                {
                    Id = 9075,
                    Username = "gogstemp215",
                    FullName = "",
                    Email = "temp215@163.com",
                    AvatarUrl = "https://secure.gravatar.com/avatar/57d9c9c2bcc73252855f785e19bc02c1"
                },
                new UserResult
                {
                    Id = 9417,
                    Username = "temptest",
                    FullName = "",
                    Email = "sdioajefoasdfj@mailinator.com",
                    AvatarUrl = "https://secure.gravatar.com/avatar/e87463cc49c6a58e139b75adfb27d052"
                },
                new UserResult
                {
                    Id = 10236,
                    Username = "tempmrtemp",
                    FullName = "",
                    Email = "tempmrtemptemp@mailinator.com",
                    AvatarUrl = "https://secure.gravatar.com/avatar/ca890e0da264bb85254a787bebd8da7e"
                },
            };
            ObjectTreeAssert.AreEqual(expected, users);
        }

        [Test]
        public async Task SearchAsync_SampleWithLimit()
        {
            var requestUri = new Uri("users/search?q=temp&limit=3", UriKind.Relative);
            var json = TestFileProvider.ReadText("usersDataWrapper.json");
            clientContext.MockHttpMessageHandler.SetResponse(requestUri, json);

            var users = await client.SearchAsync("temp", limit: 3);
            Assert.IsNotNull(users);
            Assert.AreEqual(3, users.Count);
        }

        [Test]
        public async Task GetAsync_Sample()
        {
            var requestUri = new Uri("users/test", UriKind.Relative);
            var json = TestFileProvider.ReadText("user.json");
            clientContext.MockHttpMessageHandler.SetResponse(requestUri, json);

            var user = await client.GetAsync("test");
            Assert.IsNotNull(user);

            var expected = new UserResult
            {
                Id = 7,
                Username = "test",
                FullName = "",
                Email = "",
                AvatarUrl = "https://secure.gravatar.com/avatar/bb11777b425ba7229a182f446783203d"
            };
            ObjectTreeAssert.AreEqual(expected, user);
        }

        [Test]
        public void GetAsync_NotFound()
        {
            var requestUri = new Uri("users/unknown", UriKind.Relative);
            clientContext.MockHttpMessageHandler.SetResponse(requestUri, HttpStatusCode.NotFound);

            var ex = Assert.ThrowsAsync<GogsKitNotFoundException>(() => client.GetAsync("unknown"));
            Assert.AreEqual(404, ex.ResponseStatusCode);
        }

        [Test]
        public async Task GetKeysAsync_Sample()
        {
            var requestUri = new Uri("users/test/keys", UriKind.Relative);
            var json = TestFileProvider.ReadText("keys.json");
            clientContext.MockHttpMessageHandler.SetResponse(requestUri, json);

            var keys = await client.GetKeysAsync("test");
            Assert.IsNotNull(keys);

            var expected = new[]
            {
                new KeyResult
                {
                    Id = 19,
                    Key = "ssh-rsa AAAAB3NzaC1yc2EAAAADAQABAAABAQC6jjVtyI0CgyiGkmwAbjZgMU2LXThTlgUp7EFEme0KM8mPsD6eiHk330UyXrFIu9+IAFjl3lj8/PJ30NUUnf26Zv7pgeLoxeGwpMXdh4UCIVL1hWAhWxGLbNIUC/KSX83LNCD2EDjxsmv5HeDi4dAXkURn3e14QetjpSUQsYke+h91ANQJ7WVxO7UXqrGB2Mpk/+FkypT1Q+xYSnquAQ4SwnWV16fBBoOyIM+fhKRuv0oOsu6AMaxdPDNA5TTeOwo9J7W1UCo0dgOLTX7GL5qFPdcgsHfwskSRXb2qgixTL4765epGTT6akdWO+7N9O0c2SjshlaE7KN90V3ncA56t ",
                    Url = "https://try.gogs.io/api/v1/user/keys/19",
                    Title = "Plop",
                    CreatedAt = new DateTimeOffset(2015, 02, 09, 15, 58, 42, TimeSpan.FromHours(-5)),
                },
            };
            ObjectTreeAssert.AreEqual(expected, keys);
        }
    }
}
