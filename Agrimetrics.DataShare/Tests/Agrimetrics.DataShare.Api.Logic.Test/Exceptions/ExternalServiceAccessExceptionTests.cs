using Agrimetrics.DataShare.Api.Logic.Exceptions;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.Exceptions;

[TestFixture]
public class ExternalServiceAccessExceptionTests
{
    #region Ctor()
    [Test]
    public void GivenNoParameters_WhenIConstructAnInstanceOfExternalServiceAccessException_ThenMessageIsNotSet()
    {
        var testExternalServiceAccessException = new ExternalServiceAccessException();

        var result = testExternalServiceAccessException.Message;

        Assert.That(result, Is.EqualTo($"Exception of type '{typeof(ExternalServiceAccessException).FullName}' was thrown."));
    }

    [Test]
    public void GivenNoParameters_WhenIConstructAnInstanceOfExternalServiceAccessException_ThenInnerExceptionIsNotSet()
    {
        var testExternalServiceAccessException = new ExternalServiceAccessException();

        var result = testExternalServiceAccessException.InnerException;

        Assert.That(result, Is.Null);
    }
    #endregion

    #region Ctor(message)
    [Test]
    public void GivenAMessage_WhenIConstructAnInstanceOfExternalServiceAccessException_ThenMessageIsSetToTheGivenValue()
    {
        const string testMessage = "some message";

        var testExternalServiceAccessException = new ExternalServiceAccessException(testMessage);

        var result = testExternalServiceAccessException.Message;

        Assert.That(result, Is.EqualTo(testMessage));
    }

    [Test]
    public void GivenAMessage_WhenIConstructAnInstanceOfExternalServiceAccessException_ThenInnerExceptionIsNotSet()
    {
        var testExternalServiceAccessException = new ExternalServiceAccessException("some message");

        var result = testExternalServiceAccessException.InnerException;

        Assert.That(result, Is.Null);
    }
    #endregion

    #region Ctor(message, inner exception)
    [Test]
    public void GivenAMessageAndInnerException_WhenIConstructAnInstanceOfExternalServiceAccessException_ThenMessageIsSetToTheGivenValue()
    {
        const string testMessage = "some message";

        var testExternalServiceAccessException = new ExternalServiceAccessException(testMessage, new Exception());

        var result = testExternalServiceAccessException.Message;

        Assert.That(result, Is.EqualTo(testMessage));
    }

    [Test]
    public void GivenAMessageAnd_WhenIConstructAnInstanceOfExternalServiceAccessException_ThenInnerExceptionIsSetToTheGivenValue()
    {
        var testException = new Exception("some inner message");

        var testExternalServiceAccessException = new ExternalServiceAccessException("some message", testException);

        var result = testExternalServiceAccessException.InnerException;

        Assert.That(result, Is.SameAs(testException));
    }
    #endregion
}