using Agrimetrics.DataShare.Api.Logic.Exceptions;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.Exceptions;

[TestFixture]
public class InvalidEnumValueExceptionTests
{
    #region Ctor()
    [Test]
    public void GivenNoParameters_WhenIConstructAnInstanceOfInvalidEnumValueException_ThenMessageIsNotSet()
    {
        var testInvalidEnumValueException = new InvalidEnumValueException();

        var result = testInvalidEnumValueException.Message;

        Assert.That(result, Is.EqualTo($"Exception of type '{typeof(InvalidEnumValueException).FullName}' was thrown."));
    }

    [Test]
    public void GivenNoParameters_WhenIConstructAnInstanceOfInvalidEnumValueException_ThenInnerExceptionIsNotSet()
    {
        var testInvalidEnumValueException = new InvalidEnumValueException();

        var result = testInvalidEnumValueException.InnerException;

        Assert.That(result, Is.Null);
    }
    #endregion

    #region Ctor(message)
    [Test]
    public void GivenAMessage_WhenIConstructAnInstanceOfInvalidEnumValueException_ThenMessageIsSetToTheGivenValue()
    {
        const string testMessage = "some message";

        var testInvalidEnumValueException = new InvalidEnumValueException(testMessage);

        var result = testInvalidEnumValueException.Message;

        Assert.That(result, Is.EqualTo(testMessage));
    }

    [Test]
    public void GivenAMessage_WhenIConstructAnInstanceOfInvalidEnumValueException_ThenInnerExceptionIsNotSet()
    {
        var testInvalidEnumValueException = new InvalidEnumValueException("some message");

        var result = testInvalidEnumValueException.InnerException;

        Assert.That(result, Is.Null);
    }
    #endregion

    #region Ctor(message, inner exception)
    [Test]
    public void GivenAMessageAndInnerException_WhenIConstructAnInstanceOfInvalidEnumValueException_ThenMessageIsSetToTheGivenValue()
    {
        const string testMessage = "some message";

        var testInvalidEnumValueException = new InvalidEnumValueException(testMessage, new Exception());

        var result = testInvalidEnumValueException.Message;

        Assert.That(result, Is.EqualTo(testMessage));
    }

    [Test]
    public void GivenAMessageAnd_WhenIConstructAnInstanceOfInvalidEnumValueException_ThenInnerExceptionIsSetToTheGivenValue()
    {
        var testException = new Exception("some inner message");

        var testInvalidEnumValueException = new InvalidEnumValueException("some message", testException);

        var result = testInvalidEnumValueException.InnerException;

        Assert.That(result, Is.SameAs(testException));
    }
    #endregion
}