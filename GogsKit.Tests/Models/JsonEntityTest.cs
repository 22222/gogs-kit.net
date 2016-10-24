using GogsKit.Exceptions;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GogsKit.Results
{
    public class JsonEntityTest
    {
        [TestCase("user.json", typeof(UserResult))]
        [TestCase("usersDataWrapper.json", typeof(DataResultWrapper<UserResult[]>))]
        [TestCase("token.json", typeof(TokenResult))]
        [TestCase("organization.json", typeof(OrganizationResult))]
        public void ParseJsonThenToJson(string filename, Type resultType)
        {
            var json = TestFileProvider.ReadText(filename);
            var result = JsonEntity.ParseJson(json, resultType);
            var serialized = result.ToJson();

            var expected = NormalizeJson(json);
            var actual = NormalizeJson(serialized);
            Assert.AreEqual(expected, actual);
        }

        [TestCase("keys.json", typeof(KeyResult))]
        [TestCase("users.json", typeof(UserResult))]
        [TestCase("tokens.json", typeof(TokenResult))]
        [TestCase("organizations.json", typeof(OrganizationResult))]
        [TestCase("teams.json", typeof(TeamResult))]
        public void ParseJsonArrayThenToJson(string filename, Type resultType)
        {
            var json = TestFileProvider.ReadText(filename);
            var results = JsonEntity.ParseJsonArray(json, resultType);
            var serialized = "[" + string.Join(",", results.Select(r => r.ToJson())) + "]";

            var expected = NormalizeJson(json);
            var actual = NormalizeJson(serialized);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ParseJson_InvalidJson()
        {
            var ex = Assert.Throws<GogsKitResultParseException>(() => JsonEntity.ParseJson<UserResult>("this is not JSON"));
            Assert.IsNotNull(ex.InnerException);
        }

        [Test]
        public void ParseJson_Null()
        {
            Assert.Throws<GogsKitResultParseException>(() => JsonEntity.ParseJson<UserResult>(null));
        }

        [Test]
        public void ParseJson_EmptyString()
        {
            var result = JsonEntity.ParseJson<UserResult>(string.Empty);
            Assert.IsNull(result);
        }

        [Test]
        public void ParseJsonNonGeneric_InvalidJson()
        {
            var ex = Assert.Throws<GogsKitResultParseException>(() => JsonEntity.ParseJson("this is not JSON", typeof(UserResult)));
            Assert.IsNotNull(ex.InnerException);
        }

        [Test]
        public void ParseJsonNonGeneric_Null()
        {
            Assert.Throws<GogsKitResultParseException>(() => JsonEntity.ParseJson(null, typeof(UserResult)));
        }

        [Test]
        public void ParseJsonNonGeneric_EmptyString()
        {
            var result = JsonEntity.ParseJson(string.Empty, typeof(UserResult));
            Assert.IsNull(result);
        }

        [Test]
        public void TryParseJson_InvalidJson()
        {
            UserResult result;
            var isSuccess = JsonEntity.TryParseJson("this is not JSON", out result);
            Assert.IsFalse(isSuccess);
            Assert.IsNull(result);
        }

        [Test]
        public void TryParseJson_Null()
        {
            UserResult result;
            var isSuccess = JsonEntity.TryParseJson(null, out result);
            Assert.IsFalse(isSuccess);
            Assert.IsNull(result);
        }

        [Test]
        public void TryParseJson_EmptyString()
        {
            UserResult result;
            var isSuccess = JsonEntity.TryParseJson(string.Empty, out result);
            Assert.IsTrue(isSuccess);
            Assert.IsNull(result);
        }

        [Test]
        public void ParseJsonArray_InvalidJson()
        {
            var ex = Assert.Throws<GogsKitResultParseException>(() => JsonEntity.ParseJsonArray<UserResult>("this is not JSON"));
            Assert.IsNotNull(ex.InnerException);
        }

        [Test]
        public void ParseJsonArray_Null()
        {
            Assert.Throws<GogsKitResultParseException>(() => JsonEntity.ParseJsonArray<UserResult>(null));
        }

        [Test]
        public void ParseJsonArray_EmptyString()
        {
            var result = JsonEntity.ParseJsonArray<UserResult>(string.Empty);
            Assert.IsNull(result);
        }

        [Test]
        public void ParseJsonArrayNonGeneric_InvalidJson()
        {
            var ex = Assert.Throws<GogsKitResultParseException>(() => JsonEntity.ParseJsonArray("this is not JSON", typeof(UserResult)));
            Assert.IsNotNull(ex.InnerException);
        }

        [Test]
        public void ParseJsonArrayNonGeneric_Null()
        {
            Assert.Throws<GogsKitResultParseException>(() => JsonEntity.ParseJsonArray(null, typeof(UserResult)));
        }

        [Test]
        public void ParseJsonArrayNonGeneric_EmptyString()
        {
            var result = JsonEntity.ParseJsonArray(string.Empty, typeof(UserResult));
            Assert.IsNull(result);
        }

        [Test]
        public void TryParseArrayJson_InvalidJson()
        {
            IReadOnlyCollection<UserResult> result;
            var isSuccess = JsonEntity.TryParseJsonArray("this is not JSON", out result);
            Assert.IsFalse(isSuccess);
            Assert.IsNull(result);
        }

        [Test]
        public void TryParseArrayJson_Null()
        {
            IReadOnlyCollection<UserResult> result;
            var isSuccess = JsonEntity.TryParseJsonArray(null, out result);
            Assert.IsFalse(isSuccess);
            Assert.IsNull(result);
        }

        [Test]
        public void TryParseArrayJson_EmptyString()
        {
            IReadOnlyCollection<UserResult> result;
            var isSuccess = JsonEntity.TryParseJsonArray(string.Empty, out result);
            Assert.IsTrue(isSuccess);
            Assert.IsNull(result);
        }

        #region Helpers

        private static string NormalizeJson(string json)
        {
            return JToken.Parse(json).ToString(Newtonsoft.Json.Formatting.Indented);
        }

        #endregion
    }
}
