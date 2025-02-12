using Agrimetrics.DataShare.Api.Logic.Exceptions;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.Exceptions;

[TestFixture]
public class DataShareRequestGeneralExceptionTests
{
    #region Ctor()
    [Test]
    public void GivenNoParameters_WhenIConstructAnInstanceOfDataShareRequestGeneralException_ThenMessageIsNotSet()
    {
        var testDataShareRequestGeneralException = new DataShareRequestGeneralException();

        var result = testDataShareRequestGeneralException.Message;

        Assert.That(result, Is.EqualTo($"Exception of type '{typeof(DataShareRequestGeneralException).FullName}' was thrown."));
    }

    [Test]
    public void GivenNoParameters_WhenIConstructAnInstanceOfDataShareRequestGeneralException_ThenInnerExceptionIsNotSet()
    {
        var testDataShareRequestGeneralException = new DataShareRequestGeneralException();

        var result = testDataShareRequestGeneralException.InnerException;

        Assert.That(result, Is.Null);
    }
    #endregion

    #region Ctor(message)
    [Test]
    public void GivenAMessage_WhenIConstructAnInstanceOfDataShareRequestGeneralException_ThenMessageIsSetToTheGivenValue()
    {
        const string testMessage = "some message";

        var testDataShareRequestGeneralException = new DataShareRequestGeneralException(testMessage);

        var result = testDataShareRequestGeneralException.Message;

        Assert.That(result, Is.EqualTo(testMessage));
    }

    [Test]
    public void GivenAMessage_WhenIConstructAnInstanceOfDataShareRequestGeneralException_ThenInnerExceptionIsNotSet()
    {
        var testDataShareRequestGeneralException = new DataShareRequestGeneralException("some message");

        var result = testDataShareRequestGeneralException.InnerException;

        Assert.That(result, Is.Null);
    }
    #endregion

    #region Ctor(message, inner exception)
    [Test]
    public void GivenAMessageAndInnerException_WhenIConstructAnInstanceOfDataShareRequestGeneralException_ThenMessageIsSetToTheGivenValue()
    {
        const string testMessage = "some message";

        var testDataShareRequestGeneralException = new DataShareRequestGeneralException(testMessage, new Exception());

        var result = testDataShareRequestGeneralException.Message;

        Assert.That(result, Is.EqualTo(testMessage));
    }

    [Test]
    public void GivenAMessageAnd_WhenIConstructAnInstanceOfDataShareRequestGeneralException_ThenInnerExceptionIsSetToTheGivenValue()
    {
        var testException = new Exception("some inner message");

        var testDataShareRequestGeneralException = new DataShareRequestGeneralException("some message", testException);

        var result = testDataShareRequestGeneralException.InnerException;

        Assert.That(result, Is.SameAs(testException));
    }
    #endregion
}