using System.Net;
using Agrimetrics.DataShare.Api.Logic.Services.ServiceOperationResults;
using Moq;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.Services.ServiceOperationResults
{
    [TestFixture]
    public class ServiceOperationDataResultTests
    {
        [Theory]
        public void GivenASuccessValue_WhenIConstructAnInstanceOfServiceOperationDataResult_ThenSuccessIsSetToTheGivenValue(
            bool testSuccess)
        {
            var serviceOperationDataResult = new ServiceOperationDataResult<It.IsAnyType>(
                testSuccess, It.IsAny<string>(), It.IsAny<It.IsAnyType>(), It.IsAny<HttpStatusCode?>());

            Assert.That(serviceOperationDataResult.Success, Is.EqualTo(testSuccess));
        }

        [Test]
        public void GivenAnErrorValue_WhenIConstructAnInstanceOfServiceOperationDataResult_ThenErrorIsSetToTheGivenValue(
            [Values(null, "", "   ", "test error value")] string? testError)
        {
            var serviceOperationDataResult = new ServiceOperationDataResult<It.IsAnyType>(
                It.IsAny<bool>(), testError, It.IsAny<It.IsAnyType>(), It.IsAny<HttpStatusCode?>());

            Assert.That(serviceOperationDataResult.Error, Is.EqualTo(testError));
        }

        [Test]
        public void GivenAStatusCode_WhenIConstructAnInstanceOfServiceOperationDataResult_ThenStatusCodeIsSetToTheGivenValue(
            [Values(null, HttpStatusCode.Accepted, HttpStatusCode.BadRequest, HttpStatusCode.Ambiguous)] HttpStatusCode? testStatusCode)
        {
            var serviceOperationDataResult = new ServiceOperationDataResult<It.IsAnyType>(
                It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<It.IsAnyType>(), testStatusCode);

            Assert.That(serviceOperationDataResult.StatusCode, Is.EqualTo(testStatusCode));
        }

        [Test]
        public void GivenADataValue_WhenIConstructAnInstanceOfServiceOperationDataResult_ThenDataIsSetToTheGivenValue(
            [Values(null, "test data value")] string? testData)
        {
            var serviceOperationDataResult = new ServiceOperationDataResult<string>(
                It.IsAny<bool>(), It.IsAny<string?>(), testData, It.IsAny<HttpStatusCode?>());

            Assert.That(serviceOperationDataResult.Data, Is.EqualTo(testData));
        }
    }
}
