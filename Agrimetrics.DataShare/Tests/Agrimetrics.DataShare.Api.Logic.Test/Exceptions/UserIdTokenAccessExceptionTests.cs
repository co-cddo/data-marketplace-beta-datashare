using Agrimetrics.DataShare.Api.Logic.Exceptions;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.Exceptions;

[TestFixture]
public class UserIdTokenAccessExceptionTests
{
    #region Ctor()
    [Test]
    public void GivenNoParameters_WhenIConstructAnInstanceOfUserIdTokenAccessException_ThenMessageIsNotSet()
    {
        var testUserIdTokenAccessException = new UserIdTokenAccessException();

        var result = testUserIdTokenAccessException.Message;

        Assert.That(result, Is.EqualTo($"Exception of type '{typeof(UserIdTokenAccessException).FullName}' was thrown."));
    }

    [Test]
    public void GivenNoParameters_WhenIConstructAnInstanceOfUserIdTokenAccessException_ThenInnerExceptionIsNotSet()
    {
        var testUserIdTokenAccessException = new UserIdTokenAccessException();

        var result = testUserIdTokenAccessException.InnerException;

        Assert.That(result, Is.Null);
    }
    #endregion

    #region Ctor(message)
    [Test]
    public void GivenAMessage_WhenIConstructAnInstanceOfUserIdTokenAccessException_ThenMessageIsSetToTheGivenValue()
    {
        const string testMessage = "some message";

        var testUserIdTokenAccessException = new UserIdTokenAccessException(testMessage);

        var result = testUserIdTokenAccessException.Message;

        Assert.That(result, Is.EqualTo(testMessage));
    }

    [Test]
    public void GivenAMessage_WhenIConstructAnInstanceOfUserIdTokenAccessException_ThenInnerExceptionIsNotSet()
    {
        var testUserIdTokenAccessException = new UserIdTokenAccessException("some message");

        var result = testUserIdTokenAccessException.InnerException;

        Assert.That(result, Is.Null);
    }
    #endregion

    #region Ctor(message, inner exception)
    [Test]
    public void GivenAMessageAndInnerException_WhenIConstructAnInstanceOfUserIdTokenAccessException_ThenMessageIsSetToTheGivenValue()
    {
        const string testMessage = "some message";

        var testUserIdTokenAccessException = new UserIdTokenAccessException(testMessage, new Exception());

        var result = testUserIdTokenAccessException.Message;

        Assert.That(result, Is.EqualTo(testMessage));
    }

    [Test]
    public void GivenAMessageAnd_WhenIConstructAnInstanceOfUserIdTokenAccessException_ThenInnerExceptionIsSetToTheGivenValue()
    {
        var testException = new Exception("some inner message");

        var testUserIdTokenAccessException = new UserIdTokenAccessException("some message", testException);

        var result = testUserIdTokenAccessException.InnerException;

        Assert.That(result, Is.SameAs(testException));
    }
    #endregion
}