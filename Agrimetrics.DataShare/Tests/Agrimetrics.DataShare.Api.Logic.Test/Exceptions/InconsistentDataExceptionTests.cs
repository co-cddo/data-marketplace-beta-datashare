using Agrimetrics.DataShare.Api.Logic.Exceptions;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.Exceptions;

[TestFixture]
public class InconsistentDataExceptionTests
{
    #region Ctor()
    [Test]
    public void GivenNoParameters_WhenIConstructAnInstanceOfInconsistentDataException_ThenMessageIsNotSet()
    {
        var testInconsistentDataException = new InconsistentDataException();

        var result = testInconsistentDataException.Message;

        Assert.That(result, Is.EqualTo($"Exception of type '{typeof(InconsistentDataException).FullName}' was thrown."));
    }

    [Test]
    public void GivenNoParameters_WhenIConstructAnInstanceOfInconsistentDataException_ThenInnerExceptionIsNotSet()
    {
        var testInconsistentDataException = new InconsistentDataException();

        var result = testInconsistentDataException.InnerException;

        Assert.That(result, Is.Null);
    }
    #endregion

    #region Ctor(message)
    [Test]
    public void GivenAMessage_WhenIConstructAnInstanceOfInconsistentDataException_ThenMessageIsSetToTheGivenValue()
    {
        const string testMessage = "some message";

        var testInconsistentDataException = new InconsistentDataException(testMessage);

        var result = testInconsistentDataException.Message;

        Assert.That(result, Is.EqualTo(testMessage));
    }

    [Test]
    public void GivenAMessage_WhenIConstructAnInstanceOfInconsistentDataException_ThenInnerExceptionIsNotSet()
    {
        var testInconsistentDataException = new InconsistentDataException("some message");

        var result = testInconsistentDataException.InnerException;

        Assert.That(result, Is.Null);
    }
    #endregion

    #region Ctor(message, inner exception)
    [Test]
    public void GivenAMessageAndInnerException_WhenIConstructAnInstanceOfInconsistentDataException_ThenMessageIsSetToTheGivenValue()
    {
        const string testMessage = "some message";

        var testInconsistentDataException = new InconsistentDataException(testMessage, new Exception());

        var result = testInconsistentDataException.Message;

        Assert.That(result, Is.EqualTo(testMessage));
    }

    [Test]
    public void GivenAMessageAnd_WhenIConstructAnInstanceOfInconsistentDataException_ThenInnerExceptionIsSetToTheGivenValue()
    {
        var testException = new Exception("some inner message");

        var testInconsistentDataException = new InconsistentDataException("some message", testException);

        var result = testInconsistentDataException.InnerException;

        Assert.That(result, Is.SameAs(testException));
    }
    #endregion
}