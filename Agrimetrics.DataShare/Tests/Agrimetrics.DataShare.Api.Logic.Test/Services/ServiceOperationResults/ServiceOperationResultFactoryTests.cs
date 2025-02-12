using System.Net;
using Agrimetrics.DataShare.Api.Logic.Services.ServiceOperationResults;
using Moq;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.Services.ServiceOperationResults;

[TestFixture]
public class ServiceOperationResultFactoryTests
{
    #region CreateSuccessfulDataResult() Tests
    [Test]
    public void GivenANullDataValue_WhenICreateSuccessfulDataResult_ThenAnArgumentNullExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        Assert.That(() => testItems.ServiceOperationResultFactory.CreateSuccessfulDataResult<string>(null!),
            Throws.ArgumentNullException.With.Property("ParamName").EqualTo("data"));
    }

    [Test]
    public void GivenADataValue_WhenICreateSuccessfulDataResult_ThenAServiceOperationResultReportingSuccessIsReturnedConfiguredWithTheGivenDataValue()
    {
        var testItems = CreateTestItems();

        var result = testItems.ServiceOperationResultFactory.CreateSuccessfulDataResult("test value");

        Assert.Multiple(() =>
        {
            Assert.That(result.Success, Is.True);
            Assert.That(result.Error, Is.Null);
            Assert.That(result.Data, Is.EqualTo("test value"));
        });
    }
    #endregion

    #region CreateFailedDataResult() Tests
    [Test]
    public void GivenAnErrorMessage_WhenICreateFailedDataResult_ThenAServiceOperationResultReportingFailureIsReturnedConfiguredWithTheGivenErrorMessageAndNullData()
    {
        var testItems = CreateTestItems();

        var result = testItems.ServiceOperationResultFactory.CreateFailedDataResult<It.IsAnyType>("test error message");

        Assert.Multiple(() =>
        {
            Assert.That(result.Success, Is.False);
            Assert.That(result.Error, Is.EqualTo("test error message"));
            Assert.That(result.Data, Is.Null);
        });
    }

    [Test]
    public void GivenAnHttpStatusCodeMessage_WhenICreateFailedDataResult_ThenAServiceOperationResultReportingFailureIsReturnedConfiguredWithTheGivenStatusCode(
        [Values(null, HttpStatusCode.Accepted, HttpStatusCode.Ambiguous)] HttpStatusCode? testStatusCode)
    {
        var testItems = CreateTestItems();

        var result = testItems.ServiceOperationResultFactory.CreateFailedDataResult<It.IsAnyType>("xx", statusCode: testStatusCode);

        Assert.Multiple(() =>
        {
            Assert.That(result.Success, Is.False);
            Assert.That(result.StatusCode, Is.EqualTo(testStatusCode));
        });
    }
    #endregion

    #region CreateSuccessfulResult() Tests
    [Test]
    public void GivenAServiceOperationResultFactory_WhenICreateSuccessfulResult_ThenAServiceOperationResultReportingSuccessIsReturned()
    {
        var testItems = CreateTestItems();

        var result = testItems.ServiceOperationResultFactory.CreateSuccessfulResult();

        Assert.Multiple(() =>
        {
            Assert.That(result.Success, Is.True);
            Assert.That(result.Error, Is.Null);
        });
    }
    #endregion

    #region CreateFailedResult() Tests
    [Test]
    public void GivenAnErrorMessage_WhenICreateFailedResult_ThenAServiceOperationResultReportingFailureIsReturnedConfiguredWithTheGivenErrorMessage()
    {
        var testItems = CreateTestItems();

        var result = testItems.ServiceOperationResultFactory.CreateFailedResult("test error message");

        Assert.Multiple(() =>
        {
            Assert.That(result.Success, Is.False);
            Assert.That(result.Error, Is.EqualTo("test error message"));
        });
    }

    [Test]
    public void GivenAnHttpStatusCodeMessage_WhenICreateFailedResult_ThenAServiceOperationResultReportingFailureIsReturnedConfiguredWithTheGivenStatusCode(
        [Values(null, HttpStatusCode.Accepted, HttpStatusCode.Ambiguous)] HttpStatusCode? testStatusCode)
    {
        var testItems = CreateTestItems();

        var result = testItems.ServiceOperationResultFactory.CreateFailedResult("xx", statusCode: testStatusCode);

        Assert.Multiple(() =>
        {
            Assert.That(result.Success, Is.False);
            Assert.That(result.StatusCode, Is.EqualTo(testStatusCode));
        });
    }
    #endregion

    #region Test Item Creation
    private static TestItems CreateTestItems()
    {
        var serviceOperationResultFactory = new ServiceOperationResultFactory();

        return new TestItems(serviceOperationResultFactory);
    }

    private class TestItems(
        IServiceOperationResultFactory serviceOperationResultFactory)
    {
        public IServiceOperationResultFactory ServiceOperationResultFactory { get; } = serviceOperationResultFactory;
    }
    #endregion
}