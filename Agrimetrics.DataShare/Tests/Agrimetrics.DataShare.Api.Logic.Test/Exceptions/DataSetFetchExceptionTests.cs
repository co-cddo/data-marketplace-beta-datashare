using Agrimetrics.DataShare.Api.Logic.Exceptions;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.Exceptions
{
    [TestFixture]
    public class DataSetFetchExceptionTests
    {
        [Test]
        public void GivenAStatusCode_WhenIQueryMessage_ThenMessageContainsTheGivenStatusCode()
        {
            var testException = new DataSetFetchException
            {
                StatusCode = 12345,
                ResponseText = null,
                ExceptionText = ""
            };

            var result = testException.Message;

            Assert.Multiple(() =>
            {
                Assert.That(result, Does.Match("^DataSetFetchException thrown:.*StatusCode: '12345'.*$"));
            });
        }

        [Test]
        public void GivenNoStatusCode_WhenIQueryMessage_ThenMessageContainsNoStatusCode()
        {
            var testException = new DataSetFetchException
            {
                StatusCode = null,
                ResponseText = null,
                ExceptionText = ""
            };

            var result = testException.Message;

            Assert.Multiple(() =>
            {
                Assert.That(result, Does.Match("^DataSetFetchException thrown:.*StatusCode: ''.*$"));
            });
        }

        [Test]
        public void GivenResponseText_WhenIQueryMessage_ThenMessageContainsTheGivenResponseText()
        {
            var testException = new DataSetFetchException
            {
                StatusCode = null,
                ResponseText = "test response text",
                ExceptionText = ""
            };

            var result = testException.Message;

            Assert.Multiple(() =>
            {
                Assert.That(result, Does.Match("^DataSetFetchException thrown:.*ResponseText: 'test response text'.*$"));
            });
        }

        [Test]
        public void GivenNoResponseText_WhenIQueryMessage_ThenMessageContainsNoResponseText()
        {
            var testException = new DataSetFetchException
            {
                StatusCode = null,
                ResponseText = null,
                ExceptionText = ""
            };

            var result = testException.Message;

            Assert.Multiple(() =>
            {
                Assert.That(result, Does.Match("^DataSetFetchException thrown:.*ResponseText: ''.*$"));
            });
        }

        [Test]
        public void GivenExceptionText_WhenIQueryMessage_ThenMessageContainsTheGivenExceptionText()
        {
            var testException = new DataSetFetchException
            {
                StatusCode = null,
                ResponseText = null,
                ExceptionText = "test exception text"
            };

            var result = testException.Message;

            Assert.Multiple(() =>
            {
                Assert.That(result, Does.Match("^DataSetFetchException thrown:.*ExceptionText: 'test exception text'.*$"));
            });
        }
    }
}
