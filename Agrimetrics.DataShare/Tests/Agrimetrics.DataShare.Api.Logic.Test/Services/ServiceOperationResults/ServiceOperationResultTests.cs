using System.Net;
using Agrimetrics.DataShare.Api.Logic.Services.ServiceOperationResults;
using Moq;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.Services.ServiceOperationResults;

[TestFixture]
public class ServiceOperationResultTests
{
    [Theory]
    public void GivenASuccessValue_WhenIConstructAnInstanceOfServiceOperationResult_ThenSuccessIsSetToTheGivenValue(
        bool testSuccess)
    {
        var serviceOperationResult = new ServiceOperationResult(
            testSuccess, It.IsAny<string>(), It.IsAny<HttpStatusCode?>());

        Assert.That(serviceOperationResult.Success, Is.EqualTo(testSuccess));
    }

    [Test]
    public void GivenAnErrorValue_WhenIConstructAnInstanceOfServiceOperationResult_ThenErrorIsSetToTheGivenValue(
        [Values(null, "", "   ", "test error value")] string? testError)
    {
        var serviceOperationResult = new ServiceOperationResult(
            It.IsAny<bool>(), testError, It.IsAny<HttpStatusCode?>());

        Assert.That(serviceOperationResult.Error, Is.EqualTo(testError));
    }

    [Test]
    public void GivenAStatusCode_WhenIConstructAnInstanceOfServiceOperationResult_ThenStatusCodeIsSetToTheGivenValue(
        [Values(null, HttpStatusCode.Accepted, HttpStatusCode.BadRequest, HttpStatusCode.Ambiguous)] HttpStatusCode? testStatusCode)
    {
        var serviceOperationResult = new ServiceOperationResult(
            It.IsAny<bool>(), It.IsAny<string>(), testStatusCode);

        Assert.That(serviceOperationResult.StatusCode, Is.EqualTo(testStatusCode));
    }
}