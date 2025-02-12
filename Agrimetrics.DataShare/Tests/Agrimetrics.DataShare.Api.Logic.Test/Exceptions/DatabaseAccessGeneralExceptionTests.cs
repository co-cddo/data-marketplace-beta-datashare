using Agrimetrics.DataShare.Api.Logic.Exceptions;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.Exceptions;

[TestFixture]
public class DatabaseAccessGeneralExceptionTests
{
    #region Ctor()
    [Test]
    public void GivenNoParameters_WhenIConstructAnInstanceOfDatabaseAccessGeneralException_ThenMessageIsNotSet()
    {
        var testDatabaseAccessGeneralException = new DatabaseAccessGeneralException();

        var result = testDatabaseAccessGeneralException.Message;

        Assert.That(result, Is.EqualTo($"Exception of type '{typeof(DatabaseAccessGeneralException).FullName}' was thrown."));
    }

    [Test]
    public void GivenNoParameters_WhenIConstructAnInstanceOfDatabaseAccessGeneralException_ThenInnerExceptionIsNotSet()
    {
        var testDatabaseAccessGeneralException = new DatabaseAccessGeneralException();

        var result = testDatabaseAccessGeneralException.InnerException;

        Assert.That(result, Is.Null);
    }
    #endregion

    #region Ctor(message)
    [Test]
    public void GivenAMessage_WhenIConstructAnInstanceOfDatabaseAccessGeneralException_ThenMessageIsSetToTheGivenValue()
    {
        const string testMessage = "some message";

        var testDatabaseAccessGeneralException = new DatabaseAccessGeneralException(testMessage);

        var result = testDatabaseAccessGeneralException.Message;

        Assert.That(result, Is.EqualTo(testMessage));
    }

    [Test]
    public void GivenAMessage_WhenIConstructAnInstanceOfDatabaseAccessGeneralException_ThenInnerExceptionIsNotSet()
    {
        var testDatabaseAccessGeneralException = new DatabaseAccessGeneralException("some message");

        var result = testDatabaseAccessGeneralException.InnerException;

        Assert.That(result, Is.Null);
    }
    #endregion

    #region Ctor(message, inner exception)
    [Test]
    public void GivenAMessageAndInnerException_WhenIConstructAnInstanceOfDatabaseAccessGeneralException_ThenMessageIsSetToTheGivenValue()
    {
        const string testMessage = "some message";

        var testDatabaseAccessGeneralException = new DatabaseAccessGeneralException(testMessage, new Exception());

        var result = testDatabaseAccessGeneralException.Message;

        Assert.That(result, Is.EqualTo(testMessage));
    }

    [Test]
    public void GivenAMessageAnd_WhenIConstructAnInstanceOfDatabaseAccessGeneralException_ThenInnerExceptionIsSetToTheGivenValue()
    {
        var testException = new Exception("some inner message");

        var testDatabaseAccessGeneralException = new DatabaseAccessGeneralException("some message", testException);

        var result = testDatabaseAccessGeneralException.InnerException;

        Assert.That(result, Is.SameAs(testException));
    }
    #endregion
}
