using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GogsKit
{
    public class UserAgentTest
    {
        [Test]
        public void Default_ProductNameIsGogsKit()
        {
            var userAgent = UserAgent.Default;
            StringAssert.StartsWith("GogsKit/", userAgent.Value);
        }

        [Test]
        public void ProductNameOnly()
        {
            var userAgent = new UserAgent("CERN-LineMode", productVersion: null);
            Assert.AreEqual("CERN-LineMode", userAgent.Value);
        }

        [Test]
        public void ProductNameAndVersion()
        {
            var userAgent = new UserAgent("CERN-LineMode", "2.15");
            Assert.AreEqual("CERN-LineMode/2.15", userAgent.Value);
        }

        [Test]
        public void CommentOnly()
        {
            var userAgent = new UserAgent(
                productName: null, 
                productVersion: null, 
                comment: "this is a comment"
            );
            Assert.AreEqual("(this is a comment)", userAgent.Value);
        }

        [Test]
        public void ProductNameAndVersionAndComment()
        {
            var userAgent = new UserAgent("CERN-LineMode", "2.15", "this is a comment");
            Assert.AreEqual("CERN-LineMode/2.15 (this is a comment)", userAgent.Value);
        }
    }
}
