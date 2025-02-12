using AutoFixture.AutoMoq;
using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Agrimetrics.DataShare.Api.Controllers.Supplier.DataShareRequests;
using Agrimetrics.DataShare.Api.Dto.Models.Supplier.DataShareRequests;
using Agrimetrics.DataShare.Api.Dto.Requests.Supplier;
using Agrimetrics.DataShare.Api.Logic.Services.SupplierDataShareRequest;
using Agrimetrics.DataShare.Api.Logic.Services.ServiceOperationResults;
using Agrimetrics.DataShare.Api.Dto.Responses.Supplier;
using Agrimetrics.DataShare.Api.Test.TestHelpers;

namespace Agrimetrics.DataShare.Api.Test.Controllers.Supplier.DataShareRequests;

[TestFixture]
public class SupplierDataShareRequestControllerTests
{
    #region GetSubmissionSummaries() Tests
    [Test]
    public void GivenANullGetSubmissionSummariesRequest_WhenIGetSubmissionSummaries_ThenAnArgumentNullExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        Assert.That(() => testItems.SupplierDataShareRequestController.GetSubmissionSummaries(null!),
            Throws.ArgumentNullException.With.Property("ParamName").EqualTo("getSubmissionSummariesRequest"));
    }

    [Test]
    public async Task GivenAGetSubmissionSummariesRequest_WhenIGetDataShareRequestSummaries_ThenTheAnOkResultIsReturnedWithTheValueReportedByTheSupplierDataShareRequestService()
    {
        var testItems = CreateTestItems();

        var testGetSubmissionSummariesRequest = testItems.Fixture.Create<GetSubmissionSummariesRequest>();

        var testSubmissionSummariesSet = testItems.Fixture.Create<SubmissionSummariesSet>();

        testItems.MockSupplierDataShareRequestService.Setup(x => x.GetSubmissionSummariesAsync())
            .ReturnsAsync(() => CreateTestServiceOperationDataResult(success: true, data: testSubmissionSummariesSet));

        var testGetSubmissionSummariesResponse = testItems.Fixture.Create<GetSubmissionSummariesResponse>();

        testItems.MockSupplierDataShareRequestResponseFactory.Setup(x =>
                x.CreateGetSubmissionSummariesResponse(testSubmissionSummariesSet))
            .Returns(() => testGetSubmissionSummariesResponse);

        var result = await testItems.SupplierDataShareRequestController.GetSubmissionSummaries(testGetSubmissionSummariesRequest);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<OkObjectResult>());

            var typedResult = result as OkObjectResult;
            Assert.That(typedResult!.StatusCode, Is.EqualTo(200));
            Assert.That(typedResult.Value, Is.EqualTo(testGetSubmissionSummariesResponse));
        });
    }

    [Test]
    public async Task GivenTheSupplierDataShareRequestServiceWillReturnFailure_WhenIGetSubmissionSummaries_ThenABadRequestIsReturnedReportingTheError()
    {
        var testItems = CreateTestItems();

        var testGetSubmissionSummariesRequest = testItems.Fixture.Create<GetSubmissionSummariesRequest>();

        testItems.MockSupplierDataShareRequestService.Setup(x => x.GetSubmissionSummariesAsync())
            .ReturnsAsync(() => CreateTestServiceOperationDataResult<SubmissionSummariesSet>(success: false, error: "test error message"));

        var result = await testItems.SupplierDataShareRequestController.GetSubmissionSummaries(testGetSubmissionSummariesRequest);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());

            var typedResult = result as BadRequestObjectResult;

            Assert.That(typedResult!.StatusCode, Is.EqualTo(400));
            Assert.That(typedResult.Value, Is.EqualTo("test error message"));
        });
    }

    [Test]
    public async Task GivenTheSupplierDataShareRequestServiceWillReturnFailure_WhenIGetSubmissionSummaries_ThenTheErrorIsLogged()
    {
        var testItems = CreateTestItems();

        var testGetSubmissionSummariesRequest = testItems.Fixture.Create<GetSubmissionSummariesRequest>();

        testItems.MockSupplierDataShareRequestService.Setup(x => x.GetSubmissionSummariesAsync())
            .ReturnsAsync(() => CreateTestServiceOperationDataResult<SubmissionSummariesSet>(success: false, error: "test error message"));

        await testItems.SupplierDataShareRequestController.GetSubmissionSummaries(testGetSubmissionSummariesRequest);

        testItems.MockLogger.VerifyLog(LogLevel.Error, "Failed to GetSubmissionSummaries from SupplierDataShareRequestService: test error message");
    }

    [Test]
    public async Task GivenTheSupplierDataShareRequestServiceWillThrowAnException_WhenIGetSubmissionSummaries_ThenABadRequestIsReturnedReportingTheError()
    {
        var testItems = CreateTestItems();

        var testGetSubmissionSummariesRequest = testItems.Fixture.Create<GetSubmissionSummariesRequest>();

        testItems.MockSupplierDataShareRequestService.Setup(x => x.GetSubmissionSummariesAsync())
            .Throws(new Exception("test exception error message"));

        var result = await testItems.SupplierDataShareRequestController.GetSubmissionSummaries(testGetSubmissionSummariesRequest);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());

            var typedResult = result as BadRequestObjectResult;

            Assert.That(typedResult!.StatusCode, Is.EqualTo(400));
            Assert.That(typedResult.Value, Is.EqualTo("test exception error message"));
        });
    }

    [Test]
    public async Task GivenTheSupplierDataShareRequestServiceWillThrowAnException_WhenIGetSubmissionSummaries_ThenTheExceptionIsLogged()
    {
        var testItems = CreateTestItems();

        var testGetSubmissionSummariesRequest = testItems.Fixture.Create<GetSubmissionSummariesRequest>();

        var testException = new Exception("test exception error message");
        testItems.MockSupplierDataShareRequestService.Setup(x => x.GetSubmissionSummariesAsync())
            .Throws(testException);

        await testItems.SupplierDataShareRequestController.GetSubmissionSummaries(testGetSubmissionSummariesRequest);

        testItems.MockLogger.VerifyLog(LogLevel.Error, "test exception error message", testException);
    }
    #endregion

    #region GetSubmissionInformation() Tests
    [Test]
    public void GivenANullGetGetSubmissionInformationRequest_WhenIGetSubmissionInformation_ThenAnArgumentNullExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        Assert.That(() => testItems.SupplierDataShareRequestController.GetSubmissionInformation(null!),
            Throws.ArgumentNullException.With.Property("ParamName").EqualTo("getSubmissionInformationRequest"));
    }

    [Test]
    public async Task GivenAGetSubmissionInformationRequest_WhenIGetSubmissionInformation_ThenTheAnOkResultIsReturnedWithTheValueReportedByTheSupplierDataShareRequestService()
    {
        var testItems = CreateTestItems();

        var testGetSubmissionInformationRequest = testItems.Fixture.Create<GetSubmissionInformationRequest>();

        var testSubmissionInformation = testItems.Fixture.Create<SubmissionInformation>();

        testItems.MockSupplierDataShareRequestService.Setup(x => x.GetSubmissionInformationAsync(
                testGetSubmissionInformationRequest.DataShareRequestId))
            .ReturnsAsync(() => CreateTestServiceOperationDataResult(success: true, data: testSubmissionInformation));

        var testGetSubmissionInformationResponse = testItems.Fixture.Create<GetSubmissionInformationResponse>();

        testItems.MockSupplierDataShareRequestResponseFactory.Setup(x =>
                x.CreateGetSubmissionInformationResponse(
                    testGetSubmissionInformationRequest,
                    testSubmissionInformation))
            .Returns(() => testGetSubmissionInformationResponse);

        var result = await testItems.SupplierDataShareRequestController.GetSubmissionInformation(testGetSubmissionInformationRequest);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<OkObjectResult>());

            var typedResult = result as OkObjectResult;
            Assert.That(typedResult!.StatusCode, Is.EqualTo(200));
            Assert.That(typedResult.Value, Is.EqualTo(testGetSubmissionInformationResponse));
        });
    }

    [Test]
    public async Task GivenTheSupplierDataShareRequestServiceWillReturnFailure_WhenIGetSubmissionInformation_ThenABadRequestIsReturnedReportingTheError()
    {
        var testItems = CreateTestItems();

        var testGetSubmissionInformationRequest = testItems.Fixture.Create<GetSubmissionInformationRequest>();

        testItems.MockSupplierDataShareRequestService.Setup(x => x.GetSubmissionInformationAsync(
                testGetSubmissionInformationRequest.DataShareRequestId))
            .ReturnsAsync(() => CreateTestServiceOperationDataResult<SubmissionInformation>(success: false, error: "test error message"));

        var result = await testItems.SupplierDataShareRequestController.GetSubmissionInformation(testGetSubmissionInformationRequest);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());

            var typedResult = result as BadRequestObjectResult;

            Assert.That(typedResult!.StatusCode, Is.EqualTo(400));
            Assert.That(typedResult.Value, Is.EqualTo("test error message"));
        });
    }

    [Test]
    public async Task GivenTheSupplierDataShareRequestServiceWillReturnFailure_WhenIGetSubmissionInformation_ThenTheErrorIsLogged()
    {
        var testItems = CreateTestItems();

        var testGetSubmissionInformationRequest = testItems.Fixture.Create<GetSubmissionInformationRequest>();

        testItems.MockSupplierDataShareRequestService.Setup(x => x.GetSubmissionInformationAsync(
                testGetSubmissionInformationRequest.DataShareRequestId))
            .ReturnsAsync(() => CreateTestServiceOperationDataResult<SubmissionInformation>(success: false, error: "test error message"));

        await testItems.SupplierDataShareRequestController.GetSubmissionInformation(testGetSubmissionInformationRequest);

        testItems.MockLogger.VerifyLog(LogLevel.Error, "Failed to GetSubmissionInformation from SupplierDataShareRequestService: test error message");
    }

    [Test]
    public async Task GivenTheSupplierDataShareRequestServiceWillThrowAnException_WhenIGetSubmissionInformation_ThenABadRequestIsReturnedReportingTheError()
    {
        var testItems = CreateTestItems();

        var testGetSubmissionInformationRequest = testItems.Fixture.Create<GetSubmissionInformationRequest>();

        testItems.MockSupplierDataShareRequestService.Setup(x => x.GetSubmissionInformationAsync(
                testGetSubmissionInformationRequest.DataShareRequestId))
            .Throws(new Exception("test exception error message"));

        var result = await testItems.SupplierDataShareRequestController.GetSubmissionInformation(testGetSubmissionInformationRequest);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());

            var typedResult = result as BadRequestObjectResult;

            Assert.That(typedResult!.StatusCode, Is.EqualTo(400));
            Assert.That(typedResult.Value, Is.EqualTo("test exception error message"));
        });
    }

    [Test]
    public async Task GivenTheSupplierDataShareRequestServiceWillThrowAnException_WhenIGetSubmissionInformation_ThenTheExceptionIsLogged()
    {
        var testItems = CreateTestItems();

        var testGetSubmissionInformationRequest = testItems.Fixture.Create<GetSubmissionInformationRequest>();

        var testException = new Exception("test exception error message");
        testItems.MockSupplierDataShareRequestService.Setup(x => x.GetSubmissionInformationAsync(
                testGetSubmissionInformationRequest.DataShareRequestId))
            .Throws(testException);

        await testItems.SupplierDataShareRequestController.GetSubmissionInformation(testGetSubmissionInformationRequest);

        testItems.MockLogger.VerifyLog(LogLevel.Error, "test exception error message", testException);
    }
    #endregion

    #region GetSubmissionDetails() Tests
    [Test]
    public void GivenANullGetSubmissionDetailsRequest_WhenIGetSubmissionDetails_ThenAnArgumentNullExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        Assert.That(() => testItems.SupplierDataShareRequestController.GetSubmissionDetails(null!),
            Throws.ArgumentNullException.With.Property("ParamName").EqualTo("getSubmissionDetailsRequest"));
    }

    [Test]
    public async Task GivenAGetSubmissionDetailsRequest_WhenIGetDataShareRequestSummaries_ThenTheAnOkResultIsReturnedWithTheValueReportedByTheSupplierDataShareRequestService()
    {
        var testItems = CreateTestItems();

        var testGetSubmissionDetailsRequest = testItems.Fixture.Create<GetSubmissionDetailsRequest>();

        var testSubmissionDetails = testItems.Fixture.Create<SubmissionDetails>();

        testItems.MockSupplierDataShareRequestService.Setup(x => x.GetSubmissionDetailsAsync(
                testGetSubmissionDetailsRequest.DataShareRequestId))
            .ReturnsAsync(() => CreateTestServiceOperationDataResult(success: true, data: testSubmissionDetails));

        var testGetSubmissionDetailsResponse = testItems.Fixture.Create<GetSubmissionDetailsResponse>();

        testItems.MockSupplierDataShareRequestResponseFactory.Setup(x =>
                x.CreateGetSubmissionDetailsResponse(
                    testGetSubmissionDetailsRequest,
                    testSubmissionDetails))
            .Returns(() => testGetSubmissionDetailsResponse);

        var result = await testItems.SupplierDataShareRequestController.GetSubmissionDetails(testGetSubmissionDetailsRequest);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<OkObjectResult>());

            var typedResult = result as OkObjectResult;
            Assert.That(typedResult!.StatusCode, Is.EqualTo(200));
            Assert.That(typedResult.Value, Is.EqualTo(testGetSubmissionDetailsResponse));
        });
    }

    [Test]
    public async Task GivenTheSupplierDataShareRequestServiceWillReturnFailure_WhenIGetSubmissionDetails_ThenABadRequestIsReturnedReportingTheError()
    {
        var testItems = CreateTestItems();

        var testGetSubmissionDetailsRequest = testItems.Fixture.Create<GetSubmissionDetailsRequest>();

        testItems.MockSupplierDataShareRequestService.Setup(x => x.GetSubmissionDetailsAsync(
                testGetSubmissionDetailsRequest.DataShareRequestId))
            .ReturnsAsync(() => CreateTestServiceOperationDataResult<SubmissionDetails>(success: false, error: "test error message"));

        var result = await testItems.SupplierDataShareRequestController.GetSubmissionDetails(testGetSubmissionDetailsRequest);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());

            var typedResult = result as BadRequestObjectResult;

            Assert.That(typedResult!.StatusCode, Is.EqualTo(400));
            Assert.That(typedResult.Value, Is.EqualTo("test error message"));
        });
    }

    [Test]
    public async Task GivenTheSupplierDataShareRequestServiceWillReturnFailure_WhenIGetSubmissionDetails_ThenTheErrorIsLogged()
    {
        var testItems = CreateTestItems();

        var testGetSubmissionDetailsRequest = testItems.Fixture.Create<GetSubmissionDetailsRequest>();

        testItems.MockSupplierDataShareRequestService.Setup(x => x.GetSubmissionDetailsAsync(
                testGetSubmissionDetailsRequest.DataShareRequestId))
            .ReturnsAsync(() => CreateTestServiceOperationDataResult<SubmissionDetails>(success: false, error: "test error message"));

        await testItems.SupplierDataShareRequestController.GetSubmissionDetails(testGetSubmissionDetailsRequest);

        testItems.MockLogger.VerifyLog(LogLevel.Error, "Failed to GetSubmissionDetails from SupplierDataShareRequestService: test error message");
    }

    [Test]
    public async Task GivenTheSupplierDataShareRequestServiceWillThrowAnException_WhenIGetSubmissionDetails_ThenABadRequestIsReturnedReportingTheError()
    {
        var testItems = CreateTestItems();

        var testGetSubmissionDetailsRequest = testItems.Fixture.Create<GetSubmissionDetailsRequest>();

        testItems.MockSupplierDataShareRequestService.Setup(x => x.GetSubmissionDetailsAsync(
                testGetSubmissionDetailsRequest.DataShareRequestId))
            .Throws(new Exception("test exception error message"));

        var result = await testItems.SupplierDataShareRequestController.GetSubmissionDetails(testGetSubmissionDetailsRequest);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());

            var typedResult = result as BadRequestObjectResult;

            Assert.That(typedResult!.StatusCode, Is.EqualTo(400));
            Assert.That(typedResult.Value, Is.EqualTo("test exception error message"));
        });
    }

    [Test]
    public async Task GivenTheSupplierDataShareRequestServiceWillThrowAnException_WhenIGetSubmissionDetails_ThenTheExceptionIsLogged()
    {
        var testItems = CreateTestItems();

        var testGetSubmissionDetailsRequest = testItems.Fixture.Create<GetSubmissionDetailsRequest>();

        var testException = new Exception("test exception error message");
        testItems.MockSupplierDataShareRequestService.Setup(x => x.GetSubmissionDetailsAsync(
                testGetSubmissionDetailsRequest.DataShareRequestId))
            .Throws(testException);

        await testItems.SupplierDataShareRequestController.GetSubmissionDetails(testGetSubmissionDetailsRequest);

        testItems.MockLogger.VerifyLog(LogLevel.Error, "test exception error message", testException);
    }
    #endregion

    #region GetSubmissionContentAsFile() Tests
    [Test]
    public void GivenANullGetSubmissionAsFileRequest_WhenIGetSubmissionContentAsFile_ThenAnArgumentNullExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        Assert.That(() => testItems.SupplierDataShareRequestController.GetSubmissionContentAsFile(null!),
            Throws.ArgumentNullException.With.Property("ParamName").EqualTo("getSubmissionAsFileRequest"));
    }

    [Test]
    public async Task GivenAGetSubmissionAsFileRequest_WhenIGetSubmissionContentAsFile_ThenTheAnOkResultIsReturnedWithTheValueReportedByTheSupplierDataShareRequestService()
    {
        var testItems = CreateTestItems();

        var testGetSubmissionAsFileRequest = testItems.Fixture.Create<GetSubmissionAsFileRequest>();

        var testSubmissionContentAsFile = testItems.Fixture.Build<SubmissionContentAsFile>()
            .With(x => x.Content, testItems.Fixture.CreateMany<byte>().ToArray())
            .With(x => x.ContentType, "application/pdf")
            .With(x => x.FileName, "test file name")
            .Create();

        testItems.MockSupplierDataShareRequestService.Setup(x => x.GetSubmissionContentAsFileAsync(
                testGetSubmissionAsFileRequest.DataShareRequestId,
                testGetSubmissionAsFileRequest.FileFormat))
            .ReturnsAsync(() => CreateTestServiceOperationDataResult(success: true, data: testSubmissionContentAsFile));

        var result = await testItems.SupplierDataShareRequestController.GetSubmissionContentAsFile(testGetSubmissionAsFileRequest);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<FileContentResult>());

            var typedResult = result as FileContentResult;
            Assert.That(typedResult!.FileContents, Is.EqualTo(testSubmissionContentAsFile.Content));
            Assert.That(typedResult.ContentType, Is.EqualTo(testSubmissionContentAsFile.ContentType));
            Assert.That(typedResult.FileDownloadName, Is.EqualTo(testSubmissionContentAsFile.FileName));
        });
    }

    [Test]
    public async Task GivenTheSupplierDataShareRequestServiceWillReturnFailure_WhenIGetSubmissionContentAsFile_ThenABadRequestIsReturnedReportingTheError()
    {
        var testItems = CreateTestItems();

        var testGetSubmissionAsFileRequest = testItems.Fixture.Create<GetSubmissionAsFileRequest>();

        testItems.MockSupplierDataShareRequestService.Setup(x => x.GetSubmissionContentAsFileAsync(
                testGetSubmissionAsFileRequest.DataShareRequestId,
                testGetSubmissionAsFileRequest.FileFormat))
            .ReturnsAsync(() => CreateTestServiceOperationDataResult<SubmissionContentAsFile>(success: false, error: "test error message"));

        var result = await testItems.SupplierDataShareRequestController.GetSubmissionContentAsFile(
            testGetSubmissionAsFileRequest);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());

            var typedResult = result as BadRequestObjectResult;

            Assert.That(typedResult!.StatusCode, Is.EqualTo(400));
            Assert.That(typedResult.Value, Is.EqualTo("test error message"));
        });
    }

    [Test]
    public async Task GivenTheSupplierDataShareRequestServiceWillReturnFailure_WhenIGetSubmissionContentAsFile_ThenTheErrorIsLogged()
    {
        var testItems = CreateTestItems();

        var testGetSubmissionAsFileRequest = testItems.Fixture.Create<GetSubmissionAsFileRequest>();

        testItems.MockSupplierDataShareRequestService.Setup(x => x.GetSubmissionContentAsFileAsync(
                testGetSubmissionAsFileRequest.DataShareRequestId,
                testGetSubmissionAsFileRequest.FileFormat))
            .ReturnsAsync(() => CreateTestServiceOperationDataResult<SubmissionContentAsFile>(success: false, error: "test error message"));

        await testItems.SupplierDataShareRequestController.GetSubmissionContentAsFile(testGetSubmissionAsFileRequest);

        testItems.MockLogger.VerifyLog(LogLevel.Error, "Failed to GetSubmissionContentAsFile from SupplierDataShareRequestService: test error message");
    }

    [Test]
    public async Task GivenTheSupplierDataShareRequestServiceWillThrowAnException_WhenIGetSubmissionContentAsFile_ThenABadRequestIsReturnedReportingTheError()
    {
        var testItems = CreateTestItems();

        var testGetSubmissionAsFileRequest = testItems.Fixture.Create<GetSubmissionAsFileRequest>();

        testItems.MockSupplierDataShareRequestService.Setup(x => x.GetSubmissionContentAsFileAsync(
                testGetSubmissionAsFileRequest.DataShareRequestId,
                testGetSubmissionAsFileRequest.FileFormat))
            .Throws(new Exception("test exception error message"));

        var result = await testItems.SupplierDataShareRequestController.GetSubmissionContentAsFile(testGetSubmissionAsFileRequest);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());

            var typedResult = result as BadRequestObjectResult;

            Assert.That(typedResult!.StatusCode, Is.EqualTo(400));
            Assert.That(typedResult.Value, Is.EqualTo("test exception error message"));
        });
    }

    [Test]
    public async Task GivenTheSupplierDataShareRequestServiceWillThrowAnException_WhenIGetSubmissionContentAsFile_ThenTheExceptionIsLogged()
    {
        var testItems = CreateTestItems();

        var testGetSubmissionAsFileRequest = testItems.Fixture.Create<GetSubmissionAsFileRequest>();

        var testException = new Exception("test exception error message");
        testItems.MockSupplierDataShareRequestService.Setup(x => x.GetSubmissionContentAsFileAsync(
                testGetSubmissionAsFileRequest.DataShareRequestId,
                testGetSubmissionAsFileRequest.FileFormat))
            .Throws(testException);

        await testItems.SupplierDataShareRequestController.GetSubmissionContentAsFile(testGetSubmissionAsFileRequest);

        testItems.MockLogger.VerifyLog(LogLevel.Error, "test exception error message", testException);
    }
    #endregion

    #region StartSubmissionReview() Tests
    [Test]
    public void GivenANullStartSubmissionReviewRequest_WhenIStartSubmissionReview_ThenAnArgumentNullExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        Assert.That(() => testItems.SupplierDataShareRequestController.StartSubmissionReview(null!),
            Throws.ArgumentNullException.With.Property("ParamName").EqualTo("startSubmissionReviewRequest"));
    }

    [Test]
    public async Task GivenAStartSubmissionReviewRequest_WhenIGetDataShareRequestSummaries_ThenTheAnOkResultIsReturnedWithTheValueReportedByTheSupplierDataShareRequestService()
    {
        var testItems = CreateTestItems();

        var testStartSubmissionReviewRequest = testItems.Fixture.Create<StartSubmissionReviewRequest>();

        var testSubmissionReviewInformation = testItems.Fixture.Create<SubmissionReviewInformation>();

        testItems.MockSupplierDataShareRequestService.Setup(x => x.StartSubmissionReviewAsync(
                testStartSubmissionReviewRequest.DataShareRequestId))
            .ReturnsAsync(() => CreateTestServiceOperationDataResult(success: true, data: testSubmissionReviewInformation));

        var testStartSubmissionReviewResponse = testItems.Fixture.Create<StartSubmissionReviewResponse>();

        testItems.MockSupplierDataShareRequestResponseFactory.Setup(x =>
                x.CreateStartSubmissionReviewResponse(
                    testStartSubmissionReviewRequest,
                    testSubmissionReviewInformation))
            .Returns(() => testStartSubmissionReviewResponse);

        var result = await testItems.SupplierDataShareRequestController.StartSubmissionReview(testStartSubmissionReviewRequest);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<OkObjectResult>());

            var typedResult = result as OkObjectResult;
            Assert.That(typedResult!.StatusCode, Is.EqualTo(200));
            Assert.That(typedResult.Value, Is.EqualTo(testStartSubmissionReviewResponse));
        });
    }

    [Test]
    public async Task GivenTheSupplierDataShareRequestServiceWillReturnFailure_WhenIStartSubmissionReview_ThenABadRequestIsReturnedReportingTheError()
    {
        var testItems = CreateTestItems();

        var testStartSubmissionReviewRequest = testItems.Fixture.Create<StartSubmissionReviewRequest>();

        testItems.MockSupplierDataShareRequestService.Setup(x => x.StartSubmissionReviewAsync(
                testStartSubmissionReviewRequest.DataShareRequestId))
            .ReturnsAsync(() => CreateTestServiceOperationDataResult<SubmissionReviewInformation>(success: false, error: "test error message"));

        var result = await testItems.SupplierDataShareRequestController.StartSubmissionReview(testStartSubmissionReviewRequest);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());

            var typedResult = result as BadRequestObjectResult;

            Assert.That(typedResult!.StatusCode, Is.EqualTo(400));
            Assert.That(typedResult.Value, Is.EqualTo("test error message"));
        });
    }

    [Test]
    public async Task GivenTheSupplierDataShareRequestServiceWillReturnFailure_WhenIStartSubmissionReview_ThenTheErrorIsLogged()
    {
        var testItems = CreateTestItems();

        var testStartSubmissionReviewRequest = testItems.Fixture.Create<StartSubmissionReviewRequest>();

        testItems.MockSupplierDataShareRequestService.Setup(x => x.StartSubmissionReviewAsync(
                testStartSubmissionReviewRequest.DataShareRequestId))
            .ReturnsAsync(() => CreateTestServiceOperationDataResult<SubmissionReviewInformation>(success: false, error: "test error message"));

        await testItems.SupplierDataShareRequestController.StartSubmissionReview(testStartSubmissionReviewRequest);

        testItems.MockLogger.VerifyLog(LogLevel.Error, "Failed to StartSubmissionReview with SupplierDataShareRequestService: test error message");
    }

    [Test]
    public async Task GivenTheSupplierDataShareRequestServiceWillThrowAnException_WhenIStartSubmissionReview_ThenABadRequestIsReturnedReportingTheError()
    {
        var testItems = CreateTestItems();

        var testStartSubmissionReviewRequest = testItems.Fixture.Create<StartSubmissionReviewRequest>();

        testItems.MockSupplierDataShareRequestService.Setup(x => x.StartSubmissionReviewAsync(
                testStartSubmissionReviewRequest.DataShareRequestId))
            .Throws(new Exception("test exception error message"));

        var result = await testItems.SupplierDataShareRequestController.StartSubmissionReview(testStartSubmissionReviewRequest);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());

            var typedResult = result as BadRequestObjectResult;

            Assert.That(typedResult!.StatusCode, Is.EqualTo(400));
            Assert.That(typedResult.Value, Is.EqualTo("test exception error message"));
        });
    }

    [Test]
    public async Task GivenTheSupplierDataShareRequestServiceWillThrowAnException_WhenIStartSubmissionReview_ThenTheExceptionIsLogged()
    {
        var testItems = CreateTestItems();

        var testStartSubmissionReviewRequest = testItems.Fixture.Create<StartSubmissionReviewRequest>();

        var testException = new Exception("test exception error message");
        testItems.MockSupplierDataShareRequestService.Setup(x => x.StartSubmissionReviewAsync(
                testStartSubmissionReviewRequest.DataShareRequestId))
            .Throws(testException);

        await testItems.SupplierDataShareRequestController.StartSubmissionReview(testStartSubmissionReviewRequest);

        testItems.MockLogger.VerifyLog(LogLevel.Error, "test exception error message", testException);
    }
    #endregion

    #region GetSubmissionReviewInformation() Tests
    [Test]
    public void GivenANullGetSubmissionReviewInformationRequest_WhenIGetSubmissionReviewInformation_ThenAnArgumentNullExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        Assert.That(() => testItems.SupplierDataShareRequestController.GetSubmissionReviewInformation(null!),
            Throws.ArgumentNullException.With.Property("ParamName").EqualTo("getSubmissionReviewInformationRequest"));
    }

    [Test]
    public async Task GivenAGetSubmissionReviewInformationRequest_WhenIGetSubmissionReviewInformationAsync_ThenTheAnOkResultIsReturnedWithTheValueReportedByTheSupplierDataShareRequestService()
    {
        var testItems = CreateTestItems();

        var testGetSubmissionReviewInformationRequest = testItems.Fixture.Create<GetSubmissionReviewInformationRequest>();

        var testSubmissionReviewInformation = testItems.Fixture.Create<SubmissionReviewInformation>();

        testItems.MockSupplierDataShareRequestService.Setup(x => x.GetSubmissionReviewInformationAsync(
                testGetSubmissionReviewInformationRequest.DataShareRequestId))
            .ReturnsAsync(() => CreateTestServiceOperationDataResult(success: true, data: testSubmissionReviewInformation));

        var testGetSubmissionReviewInformationResponse = testItems.Fixture.Create<GetSubmissionReviewInformationResponse>();

        testItems.MockSupplierDataShareRequestResponseFactory.Setup(x =>
                x.CreateGetSubmissionReviewInformationResponse(
                    testGetSubmissionReviewInformationRequest,
                    testSubmissionReviewInformation))
            .Returns(() => testGetSubmissionReviewInformationResponse);

        var result = await testItems.SupplierDataShareRequestController.GetSubmissionReviewInformation(testGetSubmissionReviewInformationRequest);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<OkObjectResult>());

            var typedResult = result as OkObjectResult;
            Assert.That(typedResult!.StatusCode, Is.EqualTo(200));
            Assert.That(typedResult.Value, Is.EqualTo(testGetSubmissionReviewInformationResponse));
        });
    }

    [Test]
    public async Task GivenTheSupplierDataShareRequestServiceWillReturnFailure_WhenIGetSubmissionReviewInformation_ThenABadRequestIsReturnedReportingTheError()
    {
        var testItems = CreateTestItems();

        var testGetSubmissionReviewInformationRequest = testItems.Fixture.Create<GetSubmissionReviewInformationRequest>();

        testItems.MockSupplierDataShareRequestService.Setup(x => x.GetSubmissionReviewInformationAsync(
                testGetSubmissionReviewInformationRequest.DataShareRequestId))
            .ReturnsAsync(() => CreateTestServiceOperationDataResult<SubmissionReviewInformation>(success: false, error: "test error message"));

        var result = await testItems.SupplierDataShareRequestController.GetSubmissionReviewInformation(testGetSubmissionReviewInformationRequest);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());

            var typedResult = result as BadRequestObjectResult;

            Assert.That(typedResult!.StatusCode, Is.EqualTo(400));
            Assert.That(typedResult.Value, Is.EqualTo("test error message"));
        });
    }

    [Test]
    public async Task GivenTheSupplierDataShareRequestServiceWillReturnFailure_WhenIGetSubmissionReviewInformation_ThenTheErrorIsLogged()
    {
        var testItems = CreateTestItems();

        var testGetSubmissionReviewInformationRequest = testItems.Fixture.Create<GetSubmissionReviewInformationRequest>();

        testItems.MockSupplierDataShareRequestService.Setup(x => x.GetSubmissionReviewInformationAsync(
                testGetSubmissionReviewInformationRequest.DataShareRequestId))
            .ReturnsAsync(() => CreateTestServiceOperationDataResult<SubmissionReviewInformation>(success: false, error: "test error message"));

        await testItems.SupplierDataShareRequestController.GetSubmissionReviewInformation(testGetSubmissionReviewInformationRequest);

        testItems.MockLogger.VerifyLog(LogLevel.Error, "Failed to GetSubmissionReviewInformation from SupplierDataShareRequestService: test error message");
    }

    [Test]
    public async Task GivenTheSupplierDataShareRequestServiceWillThrowAnException_WhenIGetSubmissionReviewInformation_ThenABadRequestIsReturnedReportingTheError()
    {
        var testItems = CreateTestItems();

        var testGetSubmissionReviewInformationRequest = testItems.Fixture.Create<GetSubmissionReviewInformationRequest>();

        testItems.MockSupplierDataShareRequestService.Setup(x => x.GetSubmissionReviewInformationAsync(
                testGetSubmissionReviewInformationRequest.DataShareRequestId))
            .Throws(new Exception("test exception error message"));

        var result = await testItems.SupplierDataShareRequestController.GetSubmissionReviewInformation(testGetSubmissionReviewInformationRequest);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());

            var typedResult = result as BadRequestObjectResult;

            Assert.That(typedResult!.StatusCode, Is.EqualTo(400));
            Assert.That(typedResult.Value, Is.EqualTo("test exception error message"));
        });
    }

    [Test]
    public async Task GivenTheSupplierDataShareRequestServiceWillThrowAnException_WhenIGetSubmissionReviewInformation_ThenTheExceptionIsLogged()
    {
        var testItems = CreateTestItems();

        var testGetSubmissionReviewInformationRequest = testItems.Fixture.Create<GetSubmissionReviewInformationRequest>();

        var testException = new Exception("test exception error message");
        testItems.MockSupplierDataShareRequestService.Setup(x => x.GetSubmissionReviewInformationAsync(
                testGetSubmissionReviewInformationRequest.DataShareRequestId))
            .Throws(testException);

        await testItems.SupplierDataShareRequestController.GetSubmissionReviewInformation(testGetSubmissionReviewInformationRequest);

        testItems.MockLogger.VerifyLog(LogLevel.Error, "test exception error message", testException);
    }
    #endregion

    #region GetReturnedSubmissionInformation() Tests
    [Test]
    public void GivenANullGetReturnedSubmissionInformationRequest_WhenIGetReturnedSubmissionInformation_ThenAnArgumentNullExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        Assert.That(() => testItems.SupplierDataShareRequestController.GetReturnedSubmissionInformation(null!),
            Throws.ArgumentNullException.With.Property("ParamName").EqualTo("getReturnedSubmissionInformationRequest"));
    }

    [Test]
    public async Task GivenAGetReturnedSubmissionInformationRequest_WhenIGetDataShareRequestSummaries_ThenTheAnOkResultIsReturnedWithTheValueReportedByTheSupplierDataShareRequestService()
    {
        var testItems = CreateTestItems();

        var testGetReturnedSubmissionInformationRequest = testItems.Fixture.Create<GetReturnedSubmissionInformationRequest>();

        var testReturnedSubmissionInformation = testItems.Fixture.Create<ReturnedSubmissionInformation>();

        testItems.MockSupplierDataShareRequestService.Setup(x => x.GetReturnedSubmissionInformationAsync(
                testGetReturnedSubmissionInformationRequest.DataShareRequestId))
            .ReturnsAsync(() => CreateTestServiceOperationDataResult(success: true, data: testReturnedSubmissionInformation));

        var testGetReturnedSubmissionInformationResponse = testItems.Fixture.Create<GetReturnedSubmissionInformationResponse>();

        testItems.MockSupplierDataShareRequestResponseFactory.Setup(x =>
                x.CreateGetReturnedSubmissionInformationResponse(
                    testGetReturnedSubmissionInformationRequest,
                    testReturnedSubmissionInformation))
            .Returns(() => testGetReturnedSubmissionInformationResponse);

        var result = await testItems.SupplierDataShareRequestController.GetReturnedSubmissionInformation(testGetReturnedSubmissionInformationRequest);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<OkObjectResult>());

            var typedResult = result as OkObjectResult;
            Assert.That(typedResult!.StatusCode, Is.EqualTo(200));
            Assert.That(typedResult.Value, Is.EqualTo(testGetReturnedSubmissionInformationResponse));
        });
    }

    [Test]
    public async Task GivenTheSupplierDataShareRequestServiceWillReturnFailure_WhenIGetReturnedSubmissionInformation_ThenABadRequestIsReturnedReportingTheError()
    {
        var testItems = CreateTestItems();

        var testGetReturnedSubmissionInformationRequest = testItems.Fixture.Create<GetReturnedSubmissionInformationRequest>();

        testItems.MockSupplierDataShareRequestService.Setup(x => x.GetReturnedSubmissionInformationAsync(
                testGetReturnedSubmissionInformationRequest.DataShareRequestId))
            .ReturnsAsync(() => CreateTestServiceOperationDataResult<ReturnedSubmissionInformation>(success: false, error: "test error message"));

        var result = await testItems.SupplierDataShareRequestController.GetReturnedSubmissionInformation(testGetReturnedSubmissionInformationRequest);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());

            var typedResult = result as BadRequestObjectResult;

            Assert.That(typedResult!.StatusCode, Is.EqualTo(400));
            Assert.That(typedResult.Value, Is.EqualTo("test error message"));
        });
    }

    [Test]
    public async Task GivenTheSupplierDataShareRequestServiceWillReturnFailure_WhenIGetReturnedSubmissionInformation_ThenTheErrorIsLogged()
    {
        var testItems = CreateTestItems();

        var testGetReturnedSubmissionInformationRequest = testItems.Fixture.Create<GetReturnedSubmissionInformationRequest>();

        testItems.MockSupplierDataShareRequestService.Setup(x => x.GetReturnedSubmissionInformationAsync(
                testGetReturnedSubmissionInformationRequest.DataShareRequestId))
            .ReturnsAsync(() => CreateTestServiceOperationDataResult<ReturnedSubmissionInformation>(success: false, error: "test error message"));

        await testItems.SupplierDataShareRequestController.GetReturnedSubmissionInformation(testGetReturnedSubmissionInformationRequest);

        testItems.MockLogger.VerifyLog(LogLevel.Error, "Failed to GetReturnedSubmissionInformation from SupplierDataShareRequestService: test error message");
    }

    [Test]
    public async Task GivenTheSupplierDataShareRequestServiceWillThrowAnException_WhenIGetReturnedSubmissionInformation_ThenABadRequestIsReturnedReportingTheError()
    {
        var testItems = CreateTestItems();

        var testGetReturnedSubmissionInformationRequest = testItems.Fixture.Create<GetReturnedSubmissionInformationRequest>();

        testItems.MockSupplierDataShareRequestService.Setup(x => x.GetReturnedSubmissionInformationAsync(
                testGetReturnedSubmissionInformationRequest.DataShareRequestId))
            .Throws(new Exception("test exception error message"));

        var result = await testItems.SupplierDataShareRequestController.GetReturnedSubmissionInformation(testGetReturnedSubmissionInformationRequest);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());

            var typedResult = result as BadRequestObjectResult;

            Assert.That(typedResult!.StatusCode, Is.EqualTo(400));
            Assert.That(typedResult.Value, Is.EqualTo("test exception error message"));
        });
    }

    [Test]
    public async Task GivenTheSupplierDataShareRequestServiceWillThrowAnException_WhenIGetReturnedSubmissionInformation_ThenTheExceptionIsLogged()
    {
        var testItems = CreateTestItems();

        var testGetReturnedSubmissionInformationRequest = testItems.Fixture.Create<GetReturnedSubmissionInformationRequest>();

        var testException = new Exception("test exception error message");
        testItems.MockSupplierDataShareRequestService.Setup(x => x.GetReturnedSubmissionInformationAsync(
                testGetReturnedSubmissionInformationRequest.DataShareRequestId))
            .Throws(testException);

        await testItems.SupplierDataShareRequestController.GetReturnedSubmissionInformation(testGetReturnedSubmissionInformationRequest);

        testItems.MockLogger.VerifyLog(LogLevel.Error, "test exception error message", testException);
    }
    #endregion

    #region GetCompletedSubmissionInformation() Tests
    [Test]
    public void GivenANullGetCompletedSubmissionInformationRequest_WhenIGetCompletedSubmissionInformation_ThenAnArgumentNullExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        Assert.That(() => testItems.SupplierDataShareRequestController.GetCompletedSubmissionInformation(null!),
            Throws.ArgumentNullException.With.Property("ParamName").EqualTo("getCompletedSubmissionInformationRequest"));
    }

    [Test]
    public async Task GivenAGetCompletedSubmissionInformationRequest_WhenIGetDataShareRequestSummaries_ThenTheAnOkResultIsReturnedWithTheValueReportedByTheSupplierDataShareRequestService()
    {
        var testItems = CreateTestItems();

        var testGetCompletedSubmissionInformationRequest = testItems.Fixture.Create<GetCompletedSubmissionInformationRequest>();

        var testCompletedSubmissionInformation = testItems.Fixture.Create<CompletedSubmissionInformation>();

        testItems.MockSupplierDataShareRequestService.Setup(x => x.GetCompletedSubmissionInformationAsync(
                testGetCompletedSubmissionInformationRequest.DataShareRequestId))
            .ReturnsAsync(() => CreateTestServiceOperationDataResult(success: true, data: testCompletedSubmissionInformation));

        var testGetCompletedSubmissionInformationResponse = testItems.Fixture.Create<GetCompletedSubmissionInformationResponse>();

        testItems.MockSupplierDataShareRequestResponseFactory.Setup(x =>
                x.CreateGetCompletedSubmissionInformationResponse(
                    testGetCompletedSubmissionInformationRequest,
                    testCompletedSubmissionInformation))
            .Returns(() => testGetCompletedSubmissionInformationResponse);

        var result = await testItems.SupplierDataShareRequestController.GetCompletedSubmissionInformation(testGetCompletedSubmissionInformationRequest);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<OkObjectResult>());

            var typedResult = result as OkObjectResult;
            Assert.That(typedResult!.StatusCode, Is.EqualTo(200));
            Assert.That(typedResult.Value, Is.EqualTo(testGetCompletedSubmissionInformationResponse));
        });
    }

    [Test]
    public async Task GivenTheSupplierDataShareRequestServiceWillReturnFailure_WhenIGetCompletedSubmissionInformation_ThenABadRequestIsReturnedReportingTheError()
    {
        var testItems = CreateTestItems();

        var testGetCompletedSubmissionInformationRequest = testItems.Fixture.Create<GetCompletedSubmissionInformationRequest>();

        testItems.MockSupplierDataShareRequestService.Setup(x => x.GetCompletedSubmissionInformationAsync(
                testGetCompletedSubmissionInformationRequest.DataShareRequestId))
            .ReturnsAsync(() => CreateTestServiceOperationDataResult<CompletedSubmissionInformation>(success: false, error: "test error message"));

        var result = await testItems.SupplierDataShareRequestController.GetCompletedSubmissionInformation(testGetCompletedSubmissionInformationRequest);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());

            var typedResult = result as BadRequestObjectResult;

            Assert.That(typedResult!.StatusCode, Is.EqualTo(400));
            Assert.That(typedResult.Value, Is.EqualTo("test error message"));
        });
    }

    [Test]
    public async Task GivenTheSupplierDataShareRequestServiceWillReturnFailure_WhenIGetCompletedSubmissionInformation_ThenTheErrorIsLogged()
    {
        var testItems = CreateTestItems();

        var testGetCompletedSubmissionInformationRequest = testItems.Fixture.Create<GetCompletedSubmissionInformationRequest>();

        testItems.MockSupplierDataShareRequestService.Setup(x => x.GetCompletedSubmissionInformationAsync(
                testGetCompletedSubmissionInformationRequest.DataShareRequestId))
            .ReturnsAsync(() => CreateTestServiceOperationDataResult<CompletedSubmissionInformation>(success: false, error: "test error message"));

        await testItems.SupplierDataShareRequestController.GetCompletedSubmissionInformation(testGetCompletedSubmissionInformationRequest);

        testItems.MockLogger.VerifyLog(LogLevel.Error, "Failed to GetCompletedSubmissionInformation from SupplierDataShareRequestService: test error message");
    }

    [Test]
    public async Task GivenTheSupplierDataShareRequestServiceWillThrowAnException_WhenIGetCompletedSubmissionInformation_ThenABadRequestIsReturnedReportingTheError()
    {
        var testItems = CreateTestItems();

        var testGetCompletedSubmissionInformationRequest = testItems.Fixture.Create<GetCompletedSubmissionInformationRequest>();

        testItems.MockSupplierDataShareRequestService.Setup(x => x.GetCompletedSubmissionInformationAsync(
                testGetCompletedSubmissionInformationRequest.DataShareRequestId))
            .Throws(new Exception("test exception error message"));

        var result = await testItems.SupplierDataShareRequestController.GetCompletedSubmissionInformation(testGetCompletedSubmissionInformationRequest);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());

            var typedResult = result as BadRequestObjectResult;

            Assert.That(typedResult!.StatusCode, Is.EqualTo(400));
            Assert.That(typedResult.Value, Is.EqualTo("test exception error message"));
        });
    }

    [Test]
    public async Task GivenTheSupplierDataShareRequestServiceWillThrowAnException_WhenIGetCompletedSubmissionInformation_ThenTheExceptionIsLogged()
    {
        var testItems = CreateTestItems();

        var testGetCompletedSubmissionInformationRequest = testItems.Fixture.Create<GetCompletedSubmissionInformationRequest>();

        var testException = new Exception("test exception error message");
        testItems.MockSupplierDataShareRequestService.Setup(x => x.GetCompletedSubmissionInformationAsync(
                testGetCompletedSubmissionInformationRequest.DataShareRequestId))
            .Throws(testException);

        await testItems.SupplierDataShareRequestController.GetCompletedSubmissionInformation(testGetCompletedSubmissionInformationRequest);

        testItems.MockLogger.VerifyLog(LogLevel.Error, "test exception error message", testException);
    }
    #endregion

    #region SetSubmissionNotes() Tests
    [Test]
    public void GivenANullSetSubmissionNotesRequest_WhenISetSubmissionNotes_ThenAnArgumentNullExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        Assert.That(() => testItems.SupplierDataShareRequestController.SetSubmissionNotes(null!),
            Throws.ArgumentNullException.With.Property("ParamName").EqualTo("setSubmissionNotesRequest"));
    }

    [Test]
    public async Task GivenASetSubmissionNotesRequest_WhenIGetDataShareRequestSummaries_ThenTheAnOkResultIsReturnedWithTheValueReportedByTheSupplierDataShareRequestService()
    {
        var testItems = CreateTestItems();

        var testSetSubmissionNotesRequest = testItems.Fixture.Create<SetSubmissionNotesRequest>();

        testItems.MockSupplierDataShareRequestService.Setup(x => x.SetSubmissionNotesAsync(
                testSetSubmissionNotesRequest.DataShareRequestId,
                testSetSubmissionNotesRequest.Notes))
            .ReturnsAsync(() => CreateTestServiceOperationResult(success: true));

        var testSetSubmissionNotesResponse = testItems.Fixture.Create<SetSubmissionNotesResponse>();

        testItems.MockSupplierDataShareRequestResponseFactory.Setup(x =>
                x.CreateSetSubmissionNotesResponse(testSetSubmissionNotesRequest))
            .Returns(() => testSetSubmissionNotesResponse);

        var result = await testItems.SupplierDataShareRequestController.SetSubmissionNotes(testSetSubmissionNotesRequest);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<OkObjectResult>());

            var typedResult = result as OkObjectResult;
            Assert.That(typedResult!.StatusCode, Is.EqualTo(200));
            Assert.That(typedResult.Value, Is.EqualTo(testSetSubmissionNotesResponse));
        });
    }

    [Test]
    public async Task GivenTheSupplierDataShareRequestServiceWillReturnFailure_WhenISetSubmissionNotes_ThenABadRequestIsReturnedReportingTheError()
    {
        var testItems = CreateTestItems();

        var testSetSubmissionNotesRequest = testItems.Fixture.Create<SetSubmissionNotesRequest>();

        testItems.MockSupplierDataShareRequestService.Setup(x => x.SetSubmissionNotesAsync(
                testSetSubmissionNotesRequest.DataShareRequestId,
                testSetSubmissionNotesRequest.Notes))
            .ReturnsAsync(() => CreateTestServiceOperationResult(success: false, error: "test error message"));

        var result = await testItems.SupplierDataShareRequestController.SetSubmissionNotes(testSetSubmissionNotesRequest);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());

            var typedResult = result as BadRequestObjectResult;

            Assert.That(typedResult!.StatusCode, Is.EqualTo(400));
            Assert.That(typedResult.Value, Is.EqualTo("test error message"));
        });
    }

    [Test]
    public async Task GivenTheSupplierDataShareRequestServiceWillReturnFailure_WhenISetSubmissionNotes_ThenTheErrorIsLogged()
    {
        var testItems = CreateTestItems();

        var testSetSubmissionNotesRequest = testItems.Fixture.Create<SetSubmissionNotesRequest>();

        testItems.MockSupplierDataShareRequestService.Setup(x => x.SetSubmissionNotesAsync(
                testSetSubmissionNotesRequest.DataShareRequestId,
                testSetSubmissionNotesRequest.Notes))
            .ReturnsAsync(() => CreateTestServiceOperationResult(success: false, error: "test error message"));

        await testItems.SupplierDataShareRequestController.SetSubmissionNotes(testSetSubmissionNotesRequest);

        testItems.MockLogger.VerifyLog(LogLevel.Error, "Failed to SetSubmissionNotes with SupplierDataShareRequestService: test error message");
    }

    [Test]
    public async Task GivenTheSupplierDataShareRequestServiceWillThrowAnException_WhenISetSubmissionNotes_ThenABadRequestIsReturnedReportingTheError()
    {
        var testItems = CreateTestItems();

        var testSetSubmissionNotesRequest = testItems.Fixture.Create<SetSubmissionNotesRequest>();

        testItems.MockSupplierDataShareRequestService.Setup(x => x.SetSubmissionNotesAsync(
                testSetSubmissionNotesRequest.DataShareRequestId,
                testSetSubmissionNotesRequest.Notes))
            .Throws(new Exception("test exception error message"));

        var result = await testItems.SupplierDataShareRequestController.SetSubmissionNotes(testSetSubmissionNotesRequest);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());

            var typedResult = result as BadRequestObjectResult;

            Assert.That(typedResult!.StatusCode, Is.EqualTo(400));
            Assert.That(typedResult.Value, Is.EqualTo("test exception error message"));
        });
    }

    [Test]
    public async Task GivenTheSupplierDataShareRequestServiceWillThrowAnException_WhenISetSubmissionNotes_ThenTheExceptionIsLogged()
    {
        var testItems = CreateTestItems();

        var testSetSubmissionNotesRequest = testItems.Fixture.Create<SetSubmissionNotesRequest>();

        var testException = new Exception("test exception error message");
        testItems.MockSupplierDataShareRequestService.Setup(x => x.SetSubmissionNotesAsync(
                testSetSubmissionNotesRequest.DataShareRequestId,
                testSetSubmissionNotesRequest.Notes))
            .Throws(testException);

        await testItems.SupplierDataShareRequestController.SetSubmissionNotes(testSetSubmissionNotesRequest);

        testItems.MockLogger.VerifyLog(LogLevel.Error, "test exception error message", testException);
    }
    #endregion

    #region AcceptSubmission() Tests
    [Test]
    public void GivenANullAcceptSubmissionRequest_WhenIAcceptSubmission_ThenAnArgumentNullExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        Assert.That(() => testItems.SupplierDataShareRequestController.AcceptSubmission(null!),
            Throws.ArgumentNullException.With.Property("ParamName").EqualTo("acceptSubmissionRequest"));
    }

    [Test]
    public async Task GivenAAcceptSubmissionRequest_WhenIGetDataShareRequestSummaries_ThenTheAnOkResultIsReturnedWithTheValueReportedByTheSupplierDataShareRequestService()
    {
        var testItems = CreateTestItems();

        var testAcceptSubmissionRequest = testItems.Fixture.Create<AcceptSubmissionRequest>();

        var testDataShareRequestAcceptanceResult = testItems.Fixture.Create<DataShareRequestAcceptanceResult>();

        testItems.MockSupplierDataShareRequestService.Setup(x => x.AcceptSubmissionAsync(
                testAcceptSubmissionRequest.DataShareRequestId,
                testAcceptSubmissionRequest.CommentsToAcquirer))
            .ReturnsAsync(() => CreateTestServiceOperationDataResult(success: true, data: testDataShareRequestAcceptanceResult));

        var testAcceptSubmissionResponse = testItems.Fixture.Create<AcceptSubmissionResponse>();

        testItems.MockSupplierDataShareRequestResponseFactory.Setup(x =>
                x.CreateAcceptSubmissionResponse(
                    testAcceptSubmissionRequest,
                    testDataShareRequestAcceptanceResult))
            .Returns(() => testAcceptSubmissionResponse);

        var result = await testItems.SupplierDataShareRequestController.AcceptSubmission(testAcceptSubmissionRequest);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<OkObjectResult>());

            var typedResult = result as OkObjectResult;
            Assert.That(typedResult!.StatusCode, Is.EqualTo(200));
            Assert.That(typedResult.Value, Is.EqualTo(testAcceptSubmissionResponse));
        });
    }

    [Test]
    public async Task GivenTheSupplierDataShareRequestServiceWillReturnFailure_WhenIAcceptSubmission_ThenABadRequestIsReturnedReportingTheError()
    {
        var testItems = CreateTestItems();

        var testAcceptSubmissionRequest = testItems.Fixture.Create<AcceptSubmissionRequest>();

        testItems.MockSupplierDataShareRequestService.Setup(x => x.AcceptSubmissionAsync(
                testAcceptSubmissionRequest.DataShareRequestId,
                testAcceptSubmissionRequest.CommentsToAcquirer))
            .ReturnsAsync(() => CreateTestServiceOperationDataResult<DataShareRequestAcceptanceResult>(success: false, error: "test error message"));

        var result = await testItems.SupplierDataShareRequestController.AcceptSubmission(testAcceptSubmissionRequest);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());

            var typedResult = result as BadRequestObjectResult;

            Assert.That(typedResult!.StatusCode, Is.EqualTo(400));
            Assert.That(typedResult.Value, Is.EqualTo("test error message"));
        });
    }

    [Test]
    public async Task GivenTheSupplierDataShareRequestServiceWillReturnFailure_WhenIAcceptSubmission_ThenTheErrorIsLogged()
    {
        var testItems = CreateTestItems();

        var testAcceptSubmissionRequest = testItems.Fixture.Create<AcceptSubmissionRequest>();

        testItems.MockSupplierDataShareRequestService.Setup(x => x.AcceptSubmissionAsync(
                testAcceptSubmissionRequest.DataShareRequestId,
                testAcceptSubmissionRequest.CommentsToAcquirer))
            .ReturnsAsync(() => CreateTestServiceOperationDataResult<DataShareRequestAcceptanceResult>(success: false, error: "test error message"));

        await testItems.SupplierDataShareRequestController.AcceptSubmission(testAcceptSubmissionRequest);

        testItems.MockLogger.VerifyLog(LogLevel.Error, "Failed to AcceptSubmission from SupplierDataShareRequestService: test error message");
    }

    [Test]
    public async Task GivenTheSupplierDataShareRequestServiceWillThrowAnException_WhenIAcceptSubmission_ThenABadRequestIsReturnedReportingTheError()
    {
        var testItems = CreateTestItems();

        var testAcceptSubmissionRequest = testItems.Fixture.Create<AcceptSubmissionRequest>();

        testItems.MockSupplierDataShareRequestService.Setup(x => x.AcceptSubmissionAsync(
                testAcceptSubmissionRequest.DataShareRequestId,
                testAcceptSubmissionRequest.CommentsToAcquirer))
            .Throws(new Exception("test exception error message"));

        var result = await testItems.SupplierDataShareRequestController.AcceptSubmission(testAcceptSubmissionRequest);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());

            var typedResult = result as BadRequestObjectResult;

            Assert.That(typedResult!.StatusCode, Is.EqualTo(400));
            Assert.That(typedResult.Value, Is.EqualTo("test exception error message"));
        });
    }

    [Test]
    public async Task GivenTheSupplierDataShareRequestServiceWillThrowAnException_WhenIAcceptSubmission_ThenTheExceptionIsLogged()
    {
        var testItems = CreateTestItems();

        var testAcceptSubmissionRequest = testItems.Fixture.Create<AcceptSubmissionRequest>();

        var testException = new Exception("test exception error message");
        testItems.MockSupplierDataShareRequestService.Setup(x => x.AcceptSubmissionAsync(
                testAcceptSubmissionRequest.DataShareRequestId,
                testAcceptSubmissionRequest.CommentsToAcquirer))
            .Throws(testException);

        await testItems.SupplierDataShareRequestController.AcceptSubmission(testAcceptSubmissionRequest);

        testItems.MockLogger.VerifyLog(LogLevel.Error, "test exception error message", testException);
    }
    #endregion

    #region RejectSubmission() Tests
    [Test]
    public void GivenANullRejectSubmissionRequest_WhenIRejectSubmission_ThenAnArgumentNullExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        Assert.That(() => testItems.SupplierDataShareRequestController.RejectSubmission(null!),
            Throws.ArgumentNullException.With.Property("ParamName").EqualTo("rejectSubmissionRequest"));
    }

    [Test]
    public async Task GivenARejectSubmissionRequest_WhenIGetDataShareRequestSummaries_ThenTheAnOkResultIsReturnedWithTheValueReportedByTheSupplierDataShareRequestService()
    {
        var testItems = CreateTestItems();

        var testRejectSubmissionRequest = testItems.Fixture.Create<RejectSubmissionRequest>();

        var testDataShareRequestRejectionResult = testItems.Fixture.Create<DataShareRequestRejectionResult>();

        testItems.MockSupplierDataShareRequestService.Setup(x => x.RejectSubmissionAsync(
                testRejectSubmissionRequest.DataShareRequestId,
                testRejectSubmissionRequest.CommentsToAcquirer))
            .ReturnsAsync(() => CreateTestServiceOperationDataResult(success: true, data: testDataShareRequestRejectionResult));

        var testRejectSubmissionResponse = testItems.Fixture.Create<RejectSubmissionResponse>();

        testItems.MockSupplierDataShareRequestResponseFactory.Setup(x =>
                x.CreateRejectSubmissionResponse(
                    testRejectSubmissionRequest,
                    testDataShareRequestRejectionResult))
            .Returns(() => testRejectSubmissionResponse);

        var result = await testItems.SupplierDataShareRequestController.RejectSubmission(testRejectSubmissionRequest);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<OkObjectResult>());

            var typedResult = result as OkObjectResult;
            Assert.That(typedResult!.StatusCode, Is.EqualTo(200));
            Assert.That(typedResult.Value, Is.EqualTo(testRejectSubmissionResponse));
        });
    }

    [Test]
    public async Task GivenTheSupplierDataShareRequestServiceWillReturnFailure_WhenIRejectSubmission_ThenABadRequestIsReturnedReportingTheError()
    {
        var testItems = CreateTestItems();

        var testRejectSubmissionRequest = testItems.Fixture.Create<RejectSubmissionRequest>();

        testItems.MockSupplierDataShareRequestService.Setup(x => x.RejectSubmissionAsync(
                testRejectSubmissionRequest.DataShareRequestId,
                testRejectSubmissionRequest.CommentsToAcquirer))
            .ReturnsAsync(() => CreateTestServiceOperationDataResult<DataShareRequestRejectionResult>(success: false, error: "test error message"));

        var result = await testItems.SupplierDataShareRequestController.RejectSubmission(testRejectSubmissionRequest);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());

            var typedResult = result as BadRequestObjectResult;

            Assert.That(typedResult!.StatusCode, Is.EqualTo(400));
            Assert.That(typedResult.Value, Is.EqualTo("test error message"));
        });
    }

    [Test]
    public async Task GivenTheSupplierDataShareRequestServiceWillReturnFailure_WhenIRejectSubmission_ThenTheErrorIsLogged()
    {
        var testItems = CreateTestItems();

        var testRejectSubmissionRequest = testItems.Fixture.Create<RejectSubmissionRequest>();

        testItems.MockSupplierDataShareRequestService.Setup(x => x.RejectSubmissionAsync(
                testRejectSubmissionRequest.DataShareRequestId,
                testRejectSubmissionRequest.CommentsToAcquirer))
            .ReturnsAsync(() => CreateTestServiceOperationDataResult<DataShareRequestRejectionResult>(success: false, error: "test error message"));

        await testItems.SupplierDataShareRequestController.RejectSubmission(testRejectSubmissionRequest);

        testItems.MockLogger.VerifyLog(LogLevel.Error, "Failed to RejectSubmission from SupplierDataShareRequestService: test error message");
    }

    [Test]
    public async Task GivenTheSupplierDataShareRequestServiceWillThrowAnException_WhenIRejectSubmission_ThenABadRequestIsReturnedReportingTheError()
    {
        var testItems = CreateTestItems();

        var testRejectSubmissionRequest = testItems.Fixture.Create<RejectSubmissionRequest>();

        testItems.MockSupplierDataShareRequestService.Setup(x => x.RejectSubmissionAsync(
                testRejectSubmissionRequest.DataShareRequestId,
                testRejectSubmissionRequest.CommentsToAcquirer))
            .Throws(new Exception("test exception error message"));

        var result = await testItems.SupplierDataShareRequestController.RejectSubmission(testRejectSubmissionRequest);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());

            var typedResult = result as BadRequestObjectResult;

            Assert.That(typedResult!.StatusCode, Is.EqualTo(400));
            Assert.That(typedResult.Value, Is.EqualTo("test exception error message"));
        });
    }

    [Test]
    public async Task GivenTheSupplierDataShareRequestServiceWillThrowAnException_WhenIRejectSubmission_ThenTheExceptionIsLogged()
    {
        var testItems = CreateTestItems();

        var testRejectSubmissionRequest = testItems.Fixture.Create<RejectSubmissionRequest>();

        var testException = new Exception("test exception error message");
        testItems.MockSupplierDataShareRequestService.Setup(x => x.RejectSubmissionAsync(
                testRejectSubmissionRequest.DataShareRequestId,
                testRejectSubmissionRequest.CommentsToAcquirer))
            .Throws(testException);

        await testItems.SupplierDataShareRequestController.RejectSubmission(testRejectSubmissionRequest);

        testItems.MockLogger.VerifyLog(LogLevel.Error, "test exception error message", testException);
    }
    #endregion

    #region ReturnSubmission() Tests
    [Test]
    public void GivenANullReturnSubmissionRequest_WhenIReturnSubmission_ThenAnArgumentNullExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        Assert.That(() => testItems.SupplierDataShareRequestController.ReturnSubmission(null!),
            Throws.ArgumentNullException.With.Property("ParamName").EqualTo("returnSubmissionRequest"));
    }

    [Test]
    public async Task GivenAReturnSubmissionRequest_WhenIGetDataShareRequestSummaries_ThenTheAnOkResultIsReturnedWithTheValueReportedByTheSupplierDataShareRequestService()
    {
        var testItems = CreateTestItems();

        var testReturnSubmissionRequest = testItems.Fixture.Create<ReturnSubmissionRequest>();

        var testDataShareRequestReturnResult = testItems.Fixture.Create<DataShareRequestReturnResult>();

        testItems.MockSupplierDataShareRequestService.Setup(x => x.ReturnSubmissionAsync(
                testReturnSubmissionRequest.DataShareRequestId,
                testReturnSubmissionRequest.CommentsToAcquirer))
            .ReturnsAsync(() => CreateTestServiceOperationDataResult(success: true, data: testDataShareRequestReturnResult));

        var testReturnSubmissionResponse = testItems.Fixture.Create<ReturnSubmissionResponse>();

        testItems.MockSupplierDataShareRequestResponseFactory.Setup(x =>
                x.CreateReturnSubmissionResponse(
                    testReturnSubmissionRequest,
                    testDataShareRequestReturnResult))
            .Returns(() => testReturnSubmissionResponse);

        var result = await testItems.SupplierDataShareRequestController.ReturnSubmission(testReturnSubmissionRequest);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<OkObjectResult>());

            var typedResult = result as OkObjectResult;
            Assert.That(typedResult!.StatusCode, Is.EqualTo(200));
            Assert.That(typedResult.Value, Is.EqualTo(testReturnSubmissionResponse));
        });
    }

    [Test]
    public async Task GivenTheSupplierDataShareRequestServiceWillReturnFailure_WhenIReturnSubmission_ThenABadRequestIsReturnedReportingTheError()
    {
        var testItems = CreateTestItems();

        var testReturnSubmissionRequest = testItems.Fixture.Create<ReturnSubmissionRequest>();

        testItems.MockSupplierDataShareRequestService.Setup(x => x.ReturnSubmissionAsync(
                testReturnSubmissionRequest.DataShareRequestId,
                testReturnSubmissionRequest.CommentsToAcquirer))
            .ReturnsAsync(() => CreateTestServiceOperationDataResult<DataShareRequestReturnResult>(success: false, error: "test error message"));

        var result = await testItems.SupplierDataShareRequestController.ReturnSubmission(testReturnSubmissionRequest);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());

            var typedResult = result as BadRequestObjectResult;

            Assert.That(typedResult!.StatusCode, Is.EqualTo(400));
            Assert.That(typedResult.Value, Is.EqualTo("test error message"));
        });
    }

    [Test]
    public async Task GivenTheSupplierDataShareRequestServiceWillReturnFailure_WhenIReturnSubmission_ThenTheErrorIsLogged()
    {
        var testItems = CreateTestItems();

        var testReturnSubmissionRequest = testItems.Fixture.Create<ReturnSubmissionRequest>();

        testItems.MockSupplierDataShareRequestService.Setup(x => x.ReturnSubmissionAsync(
                testReturnSubmissionRequest.DataShareRequestId,
                testReturnSubmissionRequest.CommentsToAcquirer))
            .ReturnsAsync(() => CreateTestServiceOperationDataResult<DataShareRequestReturnResult>(success: false, error: "test error message"));

        await testItems.SupplierDataShareRequestController.ReturnSubmission(testReturnSubmissionRequest);

        testItems.MockLogger.VerifyLog(LogLevel.Error, "Failed to ReturnSubmission from SupplierDataShareRequestService: test error message");
    }

    [Test]
    public async Task GivenTheSupplierDataShareRequestServiceWillThrowAnException_WhenIReturnSubmission_ThenABadRequestIsReturnedReportingTheError()
    {
        var testItems = CreateTestItems();

        var testReturnSubmissionRequest = testItems.Fixture.Create<ReturnSubmissionRequest>();

        testItems.MockSupplierDataShareRequestService.Setup(x => x.ReturnSubmissionAsync(
                testReturnSubmissionRequest.DataShareRequestId,
                testReturnSubmissionRequest.CommentsToAcquirer))
            .Throws(new Exception("test exception error message"));

        var result = await testItems.SupplierDataShareRequestController.ReturnSubmission(testReturnSubmissionRequest);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());

            var typedResult = result as BadRequestObjectResult;

            Assert.That(typedResult!.StatusCode, Is.EqualTo(400));
            Assert.That(typedResult.Value, Is.EqualTo("test exception error message"));
        });
    }

    [Test]
    public async Task GivenTheSupplierDataShareRequestServiceWillThrowAnException_WhenIReturnSubmission_ThenTheExceptionIsLogged()
    {
        var testItems = CreateTestItems();

        var testReturnSubmissionRequest = testItems.Fixture.Create<ReturnSubmissionRequest>();

        var testException = new Exception("test exception error message");
        testItems.MockSupplierDataShareRequestService.Setup(x => x.ReturnSubmissionAsync(
                testReturnSubmissionRequest.DataShareRequestId,
                testReturnSubmissionRequest.CommentsToAcquirer))
            .Throws(testException);

        await testItems.SupplierDataShareRequestController.ReturnSubmission(testReturnSubmissionRequest);

        testItems.MockLogger.VerifyLog(LogLevel.Error, "test exception error message", testException);
    }
    #endregion

    #region Test Data Creation
    private static IServiceOperationDataResult<T> CreateTestServiceOperationDataResult<T>(
        bool? success = null,
        string? error = null,
        T? data = default)
    {
        var mockServiceOperationDataResult = new Mock<IServiceOperationDataResult<T>>();

        mockServiceOperationDataResult.SetupGet(x => x.Success).Returns(success ?? true);
        mockServiceOperationDataResult.SetupGet(x => x.Error).Returns(error);
        mockServiceOperationDataResult.SetupGet(x => x.Data).Returns(data);

        return mockServiceOperationDataResult.Object;
    }

    private static IServiceOperationResult CreateTestServiceOperationResult(
        bool? success = null,
        string? error = null)
    {
        var mockServiceOperationResult = new Mock<IServiceOperationResult>();

        mockServiceOperationResult.SetupGet(x => x.Success).Returns(success ?? true);
        mockServiceOperationResult.SetupGet(x => x.Error).Returns(error);

        return mockServiceOperationResult.Object;
    }
    #endregion

    #region Test Item Creation
    private static TestItems CreateTestItems()
    {
        var fixture = new Fixture().Customize(new AutoMoqCustomization());

        var mockLogger = Mock.Get(fixture.Freeze<ILogger<SupplierDataShareRequestController>>());
        var mockSupplierDataShareRequestService = Mock.Get(fixture.Freeze<ISupplierDataShareRequestService>());
        var mockSupplierDataShareRequestResponseFactory = Mock.Get(fixture.Freeze<ISupplierDataShareRequestResponseFactory>());

        var supplierDataShareRequestController = new SupplierDataShareRequestController(
            mockLogger.Object,
            mockSupplierDataShareRequestService.Object,
            mockSupplierDataShareRequestResponseFactory.Object);

        return new TestItems(fixture, supplierDataShareRequestController,
            mockLogger, mockSupplierDataShareRequestService, mockSupplierDataShareRequestResponseFactory);
    }

    private class TestItems(
        IFixture fixture,
        SupplierDataShareRequestController supplierDataShareRequestController,
        Mock<ILogger<SupplierDataShareRequestController>> mockLogger,
        Mock<ISupplierDataShareRequestService> mockSupplierDataShareRequestService,
        Mock<ISupplierDataShareRequestResponseFactory> mockSupplierDataShareRequestResponseFactory)
    {
        public IFixture Fixture { get; } = fixture;
        public SupplierDataShareRequestController SupplierDataShareRequestController { get; } = supplierDataShareRequestController;
        public Mock<ILogger<SupplierDataShareRequestController>> MockLogger { get; } = mockLogger;
        public Mock<ISupplierDataShareRequestService> MockSupplierDataShareRequestService { get; } = mockSupplierDataShareRequestService;
        public Mock<ISupplierDataShareRequestResponseFactory> MockSupplierDataShareRequestResponseFactory { get; } = mockSupplierDataShareRequestResponseFactory;
    }
    #endregion
}