using System.Net;
using Agrimetrics.DataShare.Api.Controllers.Acquirer.DataShareRequests;
using Agrimetrics.DataShare.Api.Dto.Models.Acquirer.DataShareRequests;
using Agrimetrics.DataShare.Api.Logic.Services.ServiceOperationResults;
using AutoFixture.AutoMoq;
using AutoFixture;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Agrimetrics.DataShare.Api.Logic.Services.AcquirerDataShareRequest;
using Agrimetrics.DataShare.Api.Test.TestHelpers;
using Microsoft.AspNetCore.Mvc;
using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests;
using Agrimetrics.DataShare.Api.Dto.Models.Questions.QuestionSets;
using Agrimetrics.DataShare.Api.Dto.Requests.Acquirer.DataShareRequests;
using Agrimetrics.DataShare.Api.Dto.Responses.Acquirer.DataShareRequests;
using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests.Answers.DataShareRequestAnswerSummaries;
using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests.Answers.DataShareRequestQuestionAnswers;
using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests.Questions;

namespace Agrimetrics.DataShare.Api.Test.Controllers.Acquirer.DataShareRequests;

[TestFixture]
public class AcquirerDataShareRequestControllerTests
{
    #region GetEsdaQuestionSetOutline() Tests
    [Test]
    public void GivenANullGetEsdaQuestionSetOutlineRequest_WhenIGetEsdaQuestionSetOutline_ThenAnArgumentNullExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        Assert.That(() => testItems.AcquirerDataShareRequestController.GetEsdaQuestionSetOutline(null!),
            Throws.ArgumentNullException.With.Property("ParamName").EqualTo("getEsdaQuestionSetOutlineRequest"));
    }

    [Test]
    public async Task GivenAGetEsdaQuestionSetOutlineRequest_WhenIGetEsdaQuestionSetOutline_ThenTheAnOkResultIsReturnedWithTheValueReportedByTheAcquirerDataShareRequestService()
    {
        var testItems = CreateTestItems();

        var testGetEsdaQuestionSetOutlineRequest = testItems.Fixture.Create<GetEsdaQuestionSetOutlineRequest>();

        var testQuestionSetOutline = testItems.Fixture.Create<QuestionSetOutline>();

        testItems.MockAcquirerDataShareRequestService.Setup(x => x.GetEsdaQuestionSetOutlineRequestAsync(
                testGetEsdaQuestionSetOutlineRequest.SupplierDomainId,
                testGetEsdaQuestionSetOutlineRequest.SupplierOrganisationId,
                testGetEsdaQuestionSetOutlineRequest.EsdaId))
            .ReturnsAsync(() => CreateTestServiceOperationDataResult(success: true, data: testQuestionSetOutline));

        var testGetEsdaQuestionSetOutlineResponse = testItems.Fixture.Create<GetEsdaQuestionSetOutlineResponse>();

        testItems.MockAcquirerDataShareRequestResponseFactory.Setup(x =>
                x.CreateGetEsdaQuestionSetOutlineResponse(testGetEsdaQuestionSetOutlineRequest, testQuestionSetOutline))
            .Returns(() => testGetEsdaQuestionSetOutlineResponse);

        var result = await testItems.AcquirerDataShareRequestController.GetEsdaQuestionSetOutline(testGetEsdaQuestionSetOutlineRequest);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<OkObjectResult>());

            var typedResult = result as OkObjectResult;
            Assert.That(typedResult!.StatusCode, Is.EqualTo(200));
            Assert.That(typedResult.Value, Is.EqualTo(testGetEsdaQuestionSetOutlineResponse));
        });
    }

    [Test]
    public async Task GivenTheAcquirerDataShareRequestServiceWillReturnFailure_WhenIGetEsdaQuestionSetOutline_ThenABadRequestIsReturnedReportingTheError()
    {
        var testItems = CreateTestItems();

        var testGetEsdaQuestionSetOutlineRequest = testItems.Fixture.Create<GetEsdaQuestionSetOutlineRequest>();

        testItems.MockAcquirerDataShareRequestService.Setup(x => x.GetEsdaQuestionSetOutlineRequestAsync(
                testGetEsdaQuestionSetOutlineRequest.SupplierDomainId,
                testGetEsdaQuestionSetOutlineRequest.SupplierOrganisationId,
                testGetEsdaQuestionSetOutlineRequest.EsdaId))
            .ReturnsAsync(() => CreateTestServiceOperationDataResult<QuestionSetOutline>(success: false, error: "test error message"));

        var result = await testItems.AcquirerDataShareRequestController.GetEsdaQuestionSetOutline(testGetEsdaQuestionSetOutlineRequest);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());

            var typedResult = result as BadRequestObjectResult;

            Assert.That(typedResult!.StatusCode, Is.EqualTo(400));
            Assert.That(typedResult.Value, Is.EqualTo("test error message"));
        });
    }

    [Test]
    public async Task GivenTheAcquirerDataShareRequestServiceWillReturnFailure_WhenIGetEsdaQuestionSetOutline_ThenTheErrorIsLogged()
    {
        var testItems = CreateTestItems();

        var testGetEsdaQuestionSetOutlineRequest = testItems.Fixture.Create<GetEsdaQuestionSetOutlineRequest>();

        testItems.MockAcquirerDataShareRequestService.Setup(x => x.GetEsdaQuestionSetOutlineRequestAsync(
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<Guid>()))
            .ReturnsAsync(() => CreateTestServiceOperationDataResult<QuestionSetOutline>(success: false, error: "test error message"));
        
        await testItems.AcquirerDataShareRequestController.GetEsdaQuestionSetOutline(testGetEsdaQuestionSetOutlineRequest);

        testItems.MockLogger.VerifyLog(LogLevel.Error, "Failed to GetEsdaQuestionSetOutline from AcquirerDataShareRequestService: test error message");
    }

    [Test]
    public async Task GivenTheAcquirerDataShareRequestServiceWillThrowAnException_WhenIGetEsdaQuestionSetOutline_ThenABadRequestIsReturnedReportingTheError()
    {
        var testItems = CreateTestItems();

        var testGetEsdaQuestionSetOutlineRequest = testItems.Fixture.Create<GetEsdaQuestionSetOutlineRequest>();

        testItems.MockAcquirerDataShareRequestService.Setup(x => x.GetEsdaQuestionSetOutlineRequestAsync(
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<Guid>()))
            .Throws(new Exception("test exception error message"));

        var result = await testItems.AcquirerDataShareRequestController.GetEsdaQuestionSetOutline(testGetEsdaQuestionSetOutlineRequest);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());

            var typedResult = result as BadRequestObjectResult;

            Assert.That(typedResult!.StatusCode, Is.EqualTo(400));
            Assert.That(typedResult.Value, Is.EqualTo("test exception error message"));
        });
    }

    [Test]
    public async Task GivenTheAcquirerDataShareRequestServiceWillThrowAnException_WhenIGetEsdaQuestionSetOutline_ThenTheExceptionIsLogged()
    {
        var testItems = CreateTestItems();

        var testGetEsdaQuestionSetOutlineRequest = testItems.Fixture.Create<GetEsdaQuestionSetOutlineRequest>();

        var testException = new Exception("test exception error message");
        testItems.MockAcquirerDataShareRequestService.Setup(x => x.GetEsdaQuestionSetOutlineRequestAsync(
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<Guid>()))
            .Throws(testException);

        await testItems.AcquirerDataShareRequestController.GetEsdaQuestionSetOutline(testGetEsdaQuestionSetOutlineRequest);

        testItems.MockLogger.VerifyLog(LogLevel.Error, "test exception error message", testException);
    }
    #endregion

    #region StartDataShareRequest() Tests
    [Test]
    public void GivenANullStartDataShareRequestRequest_WhenIStartDataShareRequest_ThenAnArgumentNullExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        Assert.That(() => testItems.AcquirerDataShareRequestController.StartDataShareRequest(null!),
            Throws.ArgumentNullException.With.Property("ParamName").EqualTo("startDataShareRequestRequest"));
    }

    [Test]
    public async Task GivenAStartDataShareRequestRequest_WhenIStartDataShareRequest_ThenTheAnOkResultIsReturnedWithTheValueReportedByTheAcquirerDataShareRequestService()
    {
        var testItems = CreateTestItems();

        var testStartDataShareRequestRequest = testItems.Fixture.Create<StartDataShareRequestRequest>();

        var testDataShareRequestId = testItems.Fixture.Create<Guid>();

        testItems.MockAcquirerDataShareRequestService.Setup(x => x.StartDataShareRequestAsync(
                testStartDataShareRequestRequest.EsdaId))
            .ReturnsAsync(() => CreateTestServiceOperationDataResult(success: true, data: testDataShareRequestId));

        var testStartDataShareRequestResponse = testItems.Fixture.Create<StartDataShareRequestResponse>();

        testItems.MockAcquirerDataShareRequestResponseFactory.Setup(x =>
                x.CreateStartDataShareRequestResponse(testStartDataShareRequestRequest, testDataShareRequestId))
            .Returns(() => testStartDataShareRequestResponse);

        var result = await testItems.AcquirerDataShareRequestController.StartDataShareRequest(testStartDataShareRequestRequest);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<OkObjectResult>());

            var typedResult = result as OkObjectResult;
            Assert.That(typedResult!.StatusCode, Is.EqualTo(200));
            Assert.That(typedResult.Value, Is.EqualTo(testStartDataShareRequestResponse));
        });
    }

    [Test]
    public async Task GivenTheAcquirerDataShareRequestServiceWillReturnFailure_WhenIStartDataShareRequest_ThenABadRequestIsReturnedReportingTheError()
    {
        var testItems = CreateTestItems();

        var testStartDataShareRequestRequest = testItems.Fixture.Create<StartDataShareRequestRequest>();

        testItems.MockAcquirerDataShareRequestService.Setup(x => x.StartDataShareRequestAsync(
                It.IsAny<Guid>()))
            .ReturnsAsync(() => CreateTestServiceOperationDataResult<Guid>(success: false, error: "test error message"));

        var result = await testItems.AcquirerDataShareRequestController.StartDataShareRequest(testStartDataShareRequestRequest);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());

            var typedResult = result as BadRequestObjectResult;

            Assert.That(typedResult!.StatusCode, Is.EqualTo(400));
            Assert.That(typedResult.Value, Is.EqualTo("test error message"));
        });
    }

    [Test]
    public async Task GivenTheAcquirerDataShareRequestServiceWillReturnFailure_WhenIStartDataShareRequest_ThenTheErrorIsLogged()
    {
        var testItems = CreateTestItems();

        var testStartDataShareRequestRequest = testItems.Fixture.Create<StartDataShareRequestRequest>();

        testItems.MockAcquirerDataShareRequestService.Setup(x => x.StartDataShareRequestAsync(
                It.IsAny<Guid>()))
            .ReturnsAsync(() => CreateTestServiceOperationDataResult<Guid>(success: false, error: "test error message"));

        await testItems.AcquirerDataShareRequestController.StartDataShareRequest(testStartDataShareRequestRequest);

        testItems.MockLogger.VerifyLog(LogLevel.Error, "Failed to StartDataShareRequest with AcquirerDataShareRequestService: test error message");
    }

    [Test]
    public async Task GivenTheAcquirerDataShareRequestServiceWillThrowAnException_WhenIStartDataShareRequest_ThenABadRequestIsReturnedReportingTheError()
    {
        var testItems = CreateTestItems();

        var testStartDataShareRequestRequest = testItems.Fixture.Create<StartDataShareRequestRequest>();

        testItems.MockAcquirerDataShareRequestService.Setup(x => x.StartDataShareRequestAsync(
                It.IsAny<Guid>()))
            .Throws(new Exception("test exception error message"));

        var result = await testItems.AcquirerDataShareRequestController.StartDataShareRequest(testStartDataShareRequestRequest);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());

            var typedResult = result as BadRequestObjectResult;

            Assert.That(typedResult!.StatusCode, Is.EqualTo(400));
            Assert.That(typedResult.Value, Is.EqualTo("test exception error message"));
        });
    }

    [Test]
    public async Task GivenTheAcquirerDataShareRequestServiceWillThrowAnException_WhenIStartDataShareRequest_ThenTheExceptionIsLogged()
    {
        var testItems = CreateTestItems();

        var testStartDataShareRequestRequest = testItems.Fixture.Create<StartDataShareRequestRequest>();

        var testException = new Exception("test exception error message");
        testItems.MockAcquirerDataShareRequestService.Setup(x => x.StartDataShareRequestAsync(
                It.IsAny<Guid>()))
            .Throws(testException);

        await testItems.AcquirerDataShareRequestController.StartDataShareRequest(testStartDataShareRequestRequest);

        testItems.MockLogger.VerifyLog(LogLevel.Error, "test exception error message", testException);
    }
    #endregion

    #region GetDataShareRequestSummaries() Tests
    [Test]
    public void GivenANullGetDataShareRequestSummariesRequest_WhenIGetDataShareRequestSummaries_ThenAnArgumentNullExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        Assert.That(() => testItems.AcquirerDataShareRequestController.GetDataShareRequestSummaries(null!),
            Throws.ArgumentNullException.With.Property("ParamName").EqualTo("getDataShareRequestSummariesRequest"));
    }

    [Test]
    public async Task GivenAGetDataShareRequestsRequest_WhenIGetDataShareRequestSummaries_ThenTheAnOkResultIsReturnedWithTheValueReportedByTheAcquirerDataShareRequestService()
    {
        var testItems = CreateTestItems();

        var testGetDataShareRequestsRequest = testItems.Fixture.Create<GetDataShareRequestSummariesRequest>();

        var testDataShareRequestSummarySet = testItems.Fixture.Create<DataShareRequestSummarySet>();

        testItems.MockAcquirerDataShareRequestService.Setup(x => x.GetDataShareRequestSummariesAsync(
                testGetDataShareRequestsRequest.AcquirerUserId,
                testGetDataShareRequestsRequest.AcquirerDomainId,
                testGetDataShareRequestsRequest.AcquirerOrganisationId,
                testGetDataShareRequestsRequest.SupplierDomainId,
                testGetDataShareRequestsRequest.SupplierOrganisationId,
                testGetDataShareRequestsRequest.EsdaId,
                testGetDataShareRequestsRequest.DataShareRequestStatuses))
            .ReturnsAsync(() => CreateTestServiceOperationDataResult(success: true, data: testDataShareRequestSummarySet));

        var testGetDataShareRequestSummariesResponse = testItems.Fixture.Create<GetDataShareRequestSummariesResponse>();

        testItems.MockAcquirerDataShareRequestResponseFactory.Setup(x =>
                x.CreateGetDataShareRequestSummariesResponse(testDataShareRequestSummarySet))
            .Returns(() => testGetDataShareRequestSummariesResponse);

        var result = await testItems.AcquirerDataShareRequestController.GetDataShareRequestSummaries(testGetDataShareRequestsRequest);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<OkObjectResult>());

            var typedResult = result as OkObjectResult;
            Assert.That(typedResult!.StatusCode, Is.EqualTo(200));
            Assert.That(typedResult.Value, Is.EqualTo(testGetDataShareRequestSummariesResponse));
        });
    }

    [Test]
    public async Task GivenTheAcquirerDataShareRequestServiceWillReturnFailure_WhenIGetDataShareRequestSummaries_ThenABadRequestIsReturnedReportingTheError()
    {
        var testItems = CreateTestItems();

        var testGetDataShareRequestsRequest = testItems.Fixture.Create<GetDataShareRequestSummariesRequest>();

        testItems.MockAcquirerDataShareRequestService.Setup(x => x.GetDataShareRequestSummariesAsync(
                It.IsAny<int>(),
                It.IsAny<int?>(),
                It.IsAny<int?>(),
                It.IsAny<int?>(),
                It.IsAny<int>(),
                It.IsAny<Guid>(),
                It.IsAny<List<DataShareRequestStatus>>()))
            .ReturnsAsync(() => CreateTestServiceOperationDataResult<DataShareRequestSummarySet>(success: false, error: "test error message"));

        var result = await testItems.AcquirerDataShareRequestController.GetDataShareRequestSummaries(testGetDataShareRequestsRequest);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());

            var typedResult = result as BadRequestObjectResult;

            Assert.That(typedResult!.StatusCode, Is.EqualTo(400));
            Assert.That(typedResult.Value, Is.EqualTo("test error message"));
        });
    }

    [Test]
    public async Task GivenTheAcquirerDataShareRequestServiceWillReturnFailure_WhenIGetDataShareRequestSummaries_ThenTheErrorIsLogged()
    {
        var testItems = CreateTestItems();

        var testGetDataShareRequestsRequest = testItems.Fixture.Create<GetDataShareRequestSummariesRequest>();

        testItems.MockAcquirerDataShareRequestService.Setup(x => x.GetDataShareRequestSummariesAsync(
                It.IsAny<int>(),
                It.IsAny<int?>(),
                It.IsAny<int?>(),
                It.IsAny<int?>(),
                It.IsAny<int>(),
                It.IsAny<Guid>(),
                It.IsAny<List<DataShareRequestStatus>>()))
            .ReturnsAsync(() => CreateTestServiceOperationDataResult<DataShareRequestSummarySet>(success: false, error: "test error message"));

        await testItems.AcquirerDataShareRequestController.GetDataShareRequestSummaries(testGetDataShareRequestsRequest);

        testItems.MockLogger.VerifyLog(LogLevel.Error, "Failed to GetDataShareRequestSummaries from AcquirerDataShareRequestService: test error message");
    }

    [Test]
    public async Task GivenTheAcquirerDataShareRequestServiceWillThrowAnException_WhenIGetDataShareRequestSummaries_ThenABadRequestIsReturnedReportingTheError()
    {
        var testItems = CreateTestItems();

        var testGetDataShareRequestsRequest = testItems.Fixture.Create<GetDataShareRequestSummariesRequest>();

        testItems.MockAcquirerDataShareRequestService.Setup(x => x.GetDataShareRequestSummariesAsync(
                It.IsAny<int>(),
                It.IsAny<int?>(),
                It.IsAny<int?>(),
                It.IsAny<int?>(),
                It.IsAny<int>(),
                It.IsAny<Guid>(),
                It.IsAny<List<DataShareRequestStatus>>()))
            .Throws(new Exception("test exception error message"));

        var result = await testItems.AcquirerDataShareRequestController.GetDataShareRequestSummaries(testGetDataShareRequestsRequest);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());

            var typedResult = result as BadRequestObjectResult;

            Assert.That(typedResult!.StatusCode, Is.EqualTo(400));
            Assert.That(typedResult.Value, Is.EqualTo("test exception error message"));
        });
    }

    [Test]
    public async Task GivenTheAcquirerDataShareRequestServiceWillThrowAnException_WhenIGetDataShareRequestSummaries_ThenTheExceptionIsLogged()
    {
        var testItems = CreateTestItems();

        var testGetDataShareRequestsRequest = testItems.Fixture.Create<GetDataShareRequestSummariesRequest>();

        var testException = new Exception("test exception error message");
        testItems.MockAcquirerDataShareRequestService.Setup(x => x.GetDataShareRequestSummariesAsync(
                It.IsAny<int>(),
                It.IsAny<int?>(),
                It.IsAny<int?>(),
                It.IsAny<int?>(),
                It.IsAny<int>(),
                It.IsAny<Guid>(),
                It.IsAny<List<DataShareRequestStatus>>()))
            .Throws(testException);

        await testItems.AcquirerDataShareRequestController.GetDataShareRequestSummaries(testGetDataShareRequestsRequest);

        testItems.MockLogger.VerifyLog(LogLevel.Error, "test exception error message", testException);
    }
    #endregion
    
    #region GetDataShareRequestSummaries() Tests
    [Test]
    public void GivenANullGetDataShareRequestAdminSummariesRequest_WhenIGetDataShareRequestAdminSummaries_ThenAnArgumentNullExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        Assert.That(() => testItems.AcquirerDataShareRequestController.GetDataShareRequestAdminSummaries(null!),
            Throws.ArgumentNullException.With.Property("ParamName").EqualTo("getDataShareRequestAdminSummariesRequest"));
    }

    [Test]
    public async Task GivenAGetDataShareRequestAdminSummariesRequest_WhenIGetDataShareRequestAdminSummaries_ThenTheAnOkResultIsReturnedWithTheValueReportedByTheAcquirerDataShareRequestService()
    {
        var testItems = CreateTestItems();

        var testGetDataShareRequestAdminSummariesRequest = testItems.Fixture.Create<GetDataShareRequestAdminSummariesRequest>();

        var testDataShareRequestAdminSummarySet = testItems.Fixture.Create<DataShareRequestAdminSummarySet>();

        testItems.MockAcquirerDataShareRequestService.Setup(x => x.GetDataShareRequestAdminSummariesAsync(
                testGetDataShareRequestAdminSummariesRequest.AcquirerOrganisationId,
                testGetDataShareRequestAdminSummariesRequest.SupplierOrganisationId,
                testGetDataShareRequestAdminSummariesRequest.DataShareRequestStatuses))
            .ReturnsAsync(() => CreateTestServiceOperationDataResult(success: true, data: testDataShareRequestAdminSummarySet));

        var testGetDataShareRequestAdminSummariesResponse = testItems.Fixture.Create<GetDataShareRequestAdminSummariesResponse>();

        testItems.MockAcquirerDataShareRequestResponseFactory.Setup(x =>
                x.CreateGetDataShareRequestAdminSummariesResponse(testDataShareRequestAdminSummarySet))
            .Returns(() => testGetDataShareRequestAdminSummariesResponse);

        var result = await testItems.AcquirerDataShareRequestController.GetDataShareRequestAdminSummaries(testGetDataShareRequestAdminSummariesRequest);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<OkObjectResult>());

            var typedResult = result as OkObjectResult;
            Assert.That(typedResult!.StatusCode, Is.EqualTo(200));
            Assert.That(typedResult.Value, Is.EqualTo(testGetDataShareRequestAdminSummariesResponse));
        });
    }

    [Test]
    public async Task GivenTheAcquirerDataShareRequestServiceWillReturnFailure_WhenIGetDataShareRequestAdminSummaries_ThenABadRequestIsReturnedReportingTheError()
    {
        var testItems = CreateTestItems();

        var testGetDataShareRequestAdminSummariesRequest = testItems.Fixture.Create<GetDataShareRequestAdminSummariesRequest>();

        testItems.MockAcquirerDataShareRequestService.Setup(x => x.GetDataShareRequestAdminSummariesAsync(
                It.IsAny<int?>(),
                It.IsAny<int>(),
                It.IsAny<IEnumerable<DataShareRequestStatus>>()))
            .ReturnsAsync(() => CreateTestServiceOperationDataResult<DataShareRequestAdminSummarySet>(success: false, error: "test error message"));

        var result = await testItems.AcquirerDataShareRequestController.GetDataShareRequestAdminSummaries(testGetDataShareRequestAdminSummariesRequest);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());

            var typedResult = result as BadRequestObjectResult;

            Assert.That(typedResult!.StatusCode, Is.EqualTo(400));
            Assert.That(typedResult.Value, Is.EqualTo("test error message"));
        });
    }

    [Test]
    public async Task GivenTheAcquirerDataShareRequestServiceWillReturnFailure_WhenIGetDataShareRequestAdminSummaries_ThenTheErrorIsLogged()
    {
        var testItems = CreateTestItems();

        var testGetDataShareRequestAdminSummariesRequest = testItems.Fixture.Create<GetDataShareRequestAdminSummariesRequest>();

        testItems.MockAcquirerDataShareRequestService.Setup(x => x.GetDataShareRequestAdminSummariesAsync(
                It.IsAny<int?>(),
                It.IsAny<int>(),
                It.IsAny<IEnumerable<DataShareRequestStatus>>()))
            .ReturnsAsync(() => CreateTestServiceOperationDataResult<DataShareRequestAdminSummarySet>(success: false, error: "test error message"));

        await testItems.AcquirerDataShareRequestController.GetDataShareRequestAdminSummaries(testGetDataShareRequestAdminSummariesRequest);

        testItems.MockLogger.VerifyLog(LogLevel.Error, "Failed to GetDataShareRequestAdminSummaries from AcquirerDataShareRequestService: test error message");
    }

    [Test]
    public async Task GivenTheAcquirerDataShareRequestServiceWillThrowAnException_WhenIGetDataShareRequestAdminSummaries_ThenABadRequestIsReturnedReportingTheError()
    {
        var testItems = CreateTestItems();

        var testGetDataShareRequestAdminSummariesRequest = testItems.Fixture.Create<GetDataShareRequestAdminSummariesRequest>();

        testItems.MockAcquirerDataShareRequestService.Setup(x => x.GetDataShareRequestAdminSummariesAsync(
                It.IsAny<int?>(),
                It.IsAny<int>(),
                It.IsAny<IEnumerable<DataShareRequestStatus>>()))
            .Throws(new Exception("test exception error message"));

        var result = await testItems.AcquirerDataShareRequestController.GetDataShareRequestAdminSummaries(testGetDataShareRequestAdminSummariesRequest);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());

            var typedResult = result as BadRequestObjectResult;

            Assert.That(typedResult!.StatusCode, Is.EqualTo(400));
            Assert.That(typedResult.Value, Is.EqualTo("test exception error message"));
        });
    }

    [Test]
    public async Task GivenTheAcquirerDataShareRequestServiceWillThrowAnException_WhenIGetDataShareRequestAdminSummaries_ThenTheExceptionIsLogged()
    {
        var testItems = CreateTestItems();

        var testGetDataShareRequestAdminSummariesRequest = testItems.Fixture.Create<GetDataShareRequestAdminSummariesRequest>();

        var testException = new Exception("test exception error message");
        testItems.MockAcquirerDataShareRequestService.Setup(x => x.GetDataShareRequestAdminSummariesAsync(
                It.IsAny<int?>(),
                It.IsAny<int>(),
                It.IsAny<IEnumerable<DataShareRequestStatus>>()))
            .Throws(testException);

        await testItems.AcquirerDataShareRequestController.GetDataShareRequestAdminSummaries(testGetDataShareRequestAdminSummariesRequest);

        testItems.MockLogger.VerifyLog(LogLevel.Error, "test exception error message", testException);
    }
    #endregion

    #region GetAcquirerDataShareRequestSummaries() Tests
    [Test]
    public void GivenANullGetAcquirerDataShareRequestSummariesRequest_WhenIGetAcquirerDataShareRequestSummaries_ThenAnArgumentNullExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        Assert.That(() => testItems.AcquirerDataShareRequestController.GetAcquirerDataShareRequestSummaries(null!),
            Throws.ArgumentNullException.With.Property("ParamName").EqualTo("getAcquirerDataShareRequestSummariesRequest"));
    }

    [Test]
    public async Task GivenAGetAcquirerDataShareRequestsRequest_WhenIGetAcquirerDataShareRequestSummaries_ThenTheAnOkResultIsReturnedWithTheValueReportedByTheAcquirerDataShareRequestService()
    {
        var testItems = CreateTestItems();

        var testGetAcquirerDataShareRequestsRequest = testItems.Fixture.Create<GetAcquirerDataShareRequestSummariesRequest>();

        var testDataShareRequestSummarySet = testItems.Fixture.Create<DataShareRequestSummarySet>();

        testItems.MockAcquirerDataShareRequestService.Setup(x => x.GetAcquirerDataShareRequestSummariesAsync(
                testGetAcquirerDataShareRequestsRequest.SupplierDomainId,
                testGetAcquirerDataShareRequestsRequest.SupplierOrganisationId,
                testGetAcquirerDataShareRequestsRequest.EsdaId,
                testGetAcquirerDataShareRequestsRequest.DataShareRequestStatuses))
            .ReturnsAsync(() => CreateTestServiceOperationDataResult(success: true, data: testDataShareRequestSummarySet));

        var testGetDataShareRequestSummariesResponse = testItems.Fixture.Create<GetDataShareRequestSummariesResponse>();

        testItems.MockAcquirerDataShareRequestResponseFactory.Setup(x =>
                x.CreateGetDataShareRequestSummariesResponse(testDataShareRequestSummarySet))
            .Returns(() => testGetDataShareRequestSummariesResponse);

        var result = await testItems.AcquirerDataShareRequestController.GetAcquirerDataShareRequestSummaries(testGetAcquirerDataShareRequestsRequest);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<OkObjectResult>());

            var typedResult = result as OkObjectResult;
            Assert.That(typedResult!.StatusCode, Is.EqualTo(200));
            Assert.That(typedResult.Value, Is.EqualTo(testGetDataShareRequestSummariesResponse));
        });
    }

    [Test]
    public async Task GivenTheAcquirerDataShareRequestServiceWillReturnFailure_WhenIGetAcquirerDataShareRequestSummaries_ThenABadRequestIsReturnedReportingTheError()
    {
        var testItems = CreateTestItems();

        var testGetAcquirerDataShareRequestsRequest = testItems.Fixture.Create<GetAcquirerDataShareRequestSummariesRequest>();

        testItems.MockAcquirerDataShareRequestService.Setup(x => x.GetAcquirerDataShareRequestSummariesAsync(
                It.IsAny<int?>(),
                It.IsAny<int>(),
                It.IsAny<Guid>(),
                It.IsAny<List<DataShareRequestStatus>>()))
            .ReturnsAsync(() => CreateTestServiceOperationDataResult<DataShareRequestSummarySet>(success: false, error: "test error message"));

        var result = await testItems.AcquirerDataShareRequestController.GetAcquirerDataShareRequestSummaries(testGetAcquirerDataShareRequestsRequest);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());

            var typedResult = result as BadRequestObjectResult;

            Assert.That(typedResult!.StatusCode, Is.EqualTo(400));
            Assert.That(typedResult.Value, Is.EqualTo("test error message"));
        });
    }

    [Test]
    public async Task GivenTheAcquirerDataShareRequestServiceWillReturnFailure_WhenIGetAcquirerDataShareRequestSummaries_ThenTheErrorIsLogged()
    {
        var testItems = CreateTestItems();

        var testGetAcquirerDataShareRequestsRequest = testItems.Fixture.Create<GetAcquirerDataShareRequestSummariesRequest>();

        testItems.MockAcquirerDataShareRequestService.Setup(x => x.GetAcquirerDataShareRequestSummariesAsync(
                It.IsAny<int?>(),
                It.IsAny<int>(),
                It.IsAny<Guid>(),
                It.IsAny<List<DataShareRequestStatus>>()))
            .ReturnsAsync(() => CreateTestServiceOperationDataResult<DataShareRequestSummarySet>(success: false, error: "test error message"));

        await testItems.AcquirerDataShareRequestController.GetAcquirerDataShareRequestSummaries(testGetAcquirerDataShareRequestsRequest);

        testItems.MockLogger.VerifyLog(LogLevel.Error, "Failed to GetAcquirerDataShareRequestSummaries from AcquirerDataShareRequestService: test error message");
    }

    [Test]
    public async Task GivenTheAcquirerDataShareRequestServiceWillThrowAnException_WhenIGetAcquirerDataShareRequestSummaries_ThenABadRequestIsReturnedReportingTheError()
    {
        var testItems = CreateTestItems();

        var testGetAcquirerDataShareRequestsRequest = testItems.Fixture.Create<GetAcquirerDataShareRequestSummariesRequest>();

        testItems.MockAcquirerDataShareRequestService.Setup(x => x.GetAcquirerDataShareRequestSummariesAsync(
                It.IsAny<int?>(),
                It.IsAny<int>(),
                It.IsAny<Guid>(),
                It.IsAny<List<DataShareRequestStatus>>()))
            .Throws(new Exception("test exception error message"));

        var result = await testItems.AcquirerDataShareRequestController.GetAcquirerDataShareRequestSummaries(testGetAcquirerDataShareRequestsRequest);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());

            var typedResult = result as BadRequestObjectResult;

            Assert.That(typedResult!.StatusCode, Is.EqualTo(400));
            Assert.That(typedResult.Value, Is.EqualTo("test exception error message"));
        });
    }

    [Test]
    public async Task GivenTheAcquirerDataShareRequestServiceWillThrowAnException_WhenIGetAcquirerDataShareRequestSummaries_ThenTheExceptionIsLogged()
    {
        var testItems = CreateTestItems();

        var testGetAcquirerDataShareRequestsRequest = testItems.Fixture.Create<GetAcquirerDataShareRequestSummariesRequest>();

        var testException = new Exception("test exception error message");
        testItems.MockAcquirerDataShareRequestService.Setup(x => x.GetAcquirerDataShareRequestSummariesAsync(
                It.IsAny<int?>(),
                It.IsAny<int>(),
                It.IsAny<Guid>(),
                It.IsAny<List<DataShareRequestStatus>>()))
            .Throws(testException);

        await testItems.AcquirerDataShareRequestController.GetAcquirerDataShareRequestSummaries(testGetAcquirerDataShareRequestsRequest);

        testItems.MockLogger.VerifyLog(LogLevel.Error, "test exception error message", testException);
    }
    #endregion

    #region GetDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisation() Tests
    [Test]
    public void GivenANullGetDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisationRequest_WhenIGetDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisation_ThenAnArgumentNullExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        Assert.That(() => testItems.AcquirerDataShareRequestController.GetDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisation(null!),
            Throws.ArgumentNullException.With.Property("ParamName").EqualTo("getDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisationRequest"));
    }

    [Test]
    public async Task GivenAGetDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisationRequest_WhenIGetDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisation_ThenTheAnOkResultIsReturnedWithTheValueReportedByTheAcquirerDataShareRequestService()
    {
        var testItems = CreateTestItems();

        var testGetDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisationRequest = testItems.Fixture.Create<GetDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisationRequest>();

        var testDataShareRequestRaisedForEsdaByAcquirerOrganisationSummarySet = testItems.Fixture.Create<DataShareRequestRaisedForEsdaByAcquirerOrganisationSummarySet>();

        testItems.MockAcquirerDataShareRequestService.Setup(x => x.GetDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisationAsync(
                testGetDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisationRequest.EsdaId))
            .ReturnsAsync(() => CreateTestServiceOperationDataResult(success: true, data: testDataShareRequestRaisedForEsdaByAcquirerOrganisationSummarySet));

        var testGetDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisationResponse = testItems.Fixture.Create<GetDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisationResponse>();

        testItems.MockAcquirerDataShareRequestResponseFactory.Setup(x =>
                x.CreateGetDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisationResponse(
                    testGetDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisationRequest,
                    testDataShareRequestRaisedForEsdaByAcquirerOrganisationSummarySet))
            .Returns(() => testGetDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisationResponse);

        var result = await testItems.AcquirerDataShareRequestController.GetDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisation(testGetDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisationRequest);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<OkObjectResult>());

            var typedResult = result as OkObjectResult;
            Assert.That(typedResult!.StatusCode, Is.EqualTo(200));
            Assert.That(typedResult.Value, Is.EqualTo(testGetDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisationResponse));
        });
    }

    [Test]
    public async Task GivenTheAcquirerDataShareRequestServiceWillReturnFailure_WhenIGetDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisation_ThenABadRequestIsReturnedReportingTheError()
    {
        var testItems = CreateTestItems();

        var testGetDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisationRequest = testItems.Fixture.Create<GetDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisationRequest>();

        testItems.MockAcquirerDataShareRequestService.Setup(x => x.GetDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisationAsync(
                It.IsAny<Guid>()))
            .ReturnsAsync(() => CreateTestServiceOperationDataResult<DataShareRequestRaisedForEsdaByAcquirerOrganisationSummarySet>(success: false, error: "test error message"));

        var result = await testItems.AcquirerDataShareRequestController.GetDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisation(testGetDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisationRequest);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());

            var typedResult = result as BadRequestObjectResult;

            Assert.That(typedResult!.StatusCode, Is.EqualTo(400));
            Assert.That(typedResult.Value, Is.EqualTo("test error message"));
        });
    }

    [Test]
    public async Task GivenTheAcquirerDataShareRequestServiceWillReturnFailure_WhenIGetDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisation_ThenTheErrorIsLogged()
    {
        var testItems = CreateTestItems();

        var testGetDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisationRequest = testItems.Fixture.Create<GetDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisationRequest>();

        testItems.MockAcquirerDataShareRequestService.Setup(x => x.GetDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisationAsync(
                It.IsAny<Guid>()))
            .ReturnsAsync(() => CreateTestServiceOperationDataResult<DataShareRequestRaisedForEsdaByAcquirerOrganisationSummarySet>(success: false, error: "test error message"));

        await testItems.AcquirerDataShareRequestController.GetDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisation(testGetDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisationRequest);

        testItems.MockLogger.VerifyLog(LogLevel.Error, "Failed to GetDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisation from AcquirerDataShareRequestService: test error message");
    }

    [Test]
    public async Task GivenTheAcquirerDataShareRequestServiceWillThrowAnException_WhenIGetDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisation_ThenABadRequestIsReturnedReportingTheError()
    {
        var testItems = CreateTestItems();

        var testGetDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisationRequest = testItems.Fixture.Create<GetDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisationRequest>();

        testItems.MockAcquirerDataShareRequestService.Setup(x => x.GetDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisationAsync(
                It.IsAny<Guid>()))
            .Throws(new Exception("test exception error message"));

        var result = await testItems.AcquirerDataShareRequestController.GetDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisation(testGetDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisationRequest);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());

            var typedResult = result as BadRequestObjectResult;

            Assert.That(typedResult!.StatusCode, Is.EqualTo(400));
            Assert.That(typedResult.Value, Is.EqualTo("test exception error message"));
        });
    }

    [Test]
    public async Task GivenTheAcquirerDataShareRequestServiceWillThrowAnException_WhenIGetDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisation_ThenTheExceptionIsLogged()
    {
        var testItems = CreateTestItems();

        var testGetDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisationRequest = testItems.Fixture.Create<GetDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisationRequest>();

        var testException = new Exception("test exception error message");
        testItems.MockAcquirerDataShareRequestService.Setup(x => x.GetDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisationAsync(
                It.IsAny<Guid>()))
            .Throws(testException);

        await testItems.AcquirerDataShareRequestController.GetDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisation(testGetDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisationRequest);

        testItems.MockLogger.VerifyLog(LogLevel.Error, "test exception error message", testException);
    }
    #endregion

    #region GetDataShareRequestQuestionsSummary() Tests
    [Test]
    public void GivenANullGetDataShareRequestQuestionsSummaryRequest_WhenIGetDataShareRequestQuestionsSummary_ThenAnArgumentNullExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        Assert.That(() => testItems.AcquirerDataShareRequestController.GetDataShareRequestQuestionsSummary(null!),
            Throws.ArgumentNullException.With.Property("ParamName").EqualTo("getDataShareRequestQuestionsSummaryRequest"));
    }

    [Test]
    public async Task GivenAGetDataShareRequestQuestionsSummaryRequest_WhenIGetDataShareRequestQuestionsSummary_ThenTheAnOkResultIsReturnedWithTheValueReportedByTheAcquirerDataShareRequestService()
    {
        var testItems = CreateTestItems();

        var testGetDataShareRequestQuestionsSummaryRequest = testItems.Fixture.Create<GetDataShareRequestQuestionsSummaryRequest>();

        var testDataShareRequestQuestionsSummary = testItems.Fixture.Create<DataShareRequestQuestionsSummary>();

        testItems.MockAcquirerDataShareRequestService.Setup(x => x.GetDataShareRequestQuestionsSummaryAsync(
                testGetDataShareRequestQuestionsSummaryRequest.DataShareRequestId))
            .ReturnsAsync(() => CreateTestServiceOperationDataResult(success: true, data: testDataShareRequestQuestionsSummary));

        var testGetDataShareRequestQuestionsSummaryResponse = testItems.Fixture.Create<GetDataShareRequestQuestionsSummaryResponse>();

        testItems.MockAcquirerDataShareRequestResponseFactory.Setup(x =>
                x.CreateGetDataShareRequestQuestionsSummaryResponse(testGetDataShareRequestQuestionsSummaryRequest, testDataShareRequestQuestionsSummary))
            .Returns(() => testGetDataShareRequestQuestionsSummaryResponse);

        var result = await testItems.AcquirerDataShareRequestController.GetDataShareRequestQuestionsSummary(testGetDataShareRequestQuestionsSummaryRequest);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<OkObjectResult>());

            var typedResult = result as OkObjectResult;
            Assert.That(typedResult!.StatusCode, Is.EqualTo(200));
            Assert.That(typedResult.Value, Is.EqualTo(testGetDataShareRequestQuestionsSummaryResponse));
        });
    }

    [Test]
    public async Task GivenTheAcquirerDataShareRequestServiceWillReturnFailure_WhenIGetDataShareRequestQuestionsSummary_ThenABadRequestIsReturnedReportingTheError()
    {
        var testItems = CreateTestItems();

        var testGetDataShareRequestQuestionsSummaryRequest = testItems.Fixture.Create<GetDataShareRequestQuestionsSummaryRequest>();

        testItems.MockAcquirerDataShareRequestService.Setup(x => x.GetDataShareRequestQuestionsSummaryAsync(
                It.IsAny<Guid>()))
            .ReturnsAsync(() => CreateTestServiceOperationDataResult<DataShareRequestQuestionsSummary>(success: false, error: "test error message"));

        var result = await testItems.AcquirerDataShareRequestController.GetDataShareRequestQuestionsSummary(testGetDataShareRequestQuestionsSummaryRequest);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());

            var typedResult = result as BadRequestObjectResult;

            Assert.That(typedResult!.StatusCode, Is.EqualTo(400));
            Assert.That(typedResult.Value, Is.EqualTo("test error message"));
        });
    }

    [Test]
    public async Task GivenTheAcquirerDataShareRequestServiceWillReturnFailure_WhenIGetDataShareRequestQuestionsSummary_ThenTheErrorIsLogged()
    {
        var testItems = CreateTestItems();

        var testGetDataShareRequestQuestionsSummaryRequest = testItems.Fixture.Create<GetDataShareRequestQuestionsSummaryRequest>();

        testItems.MockAcquirerDataShareRequestService.Setup(x => x.GetDataShareRequestQuestionsSummaryAsync(
                It.IsAny<Guid>()))
            .ReturnsAsync(() => CreateTestServiceOperationDataResult<DataShareRequestQuestionsSummary>(success: false, error: "test error message"));

        await testItems.AcquirerDataShareRequestController.GetDataShareRequestQuestionsSummary(testGetDataShareRequestQuestionsSummaryRequest);

        testItems.MockLogger.VerifyLog(LogLevel.Error, "Failed to GetDataShareRequestQuestionsSummary from AcquirerDataShareRequestService: test error message");
    }

    [Test]
    public async Task GivenTheAcquirerDataShareRequestServiceWillThrowAnException_WhenIGetDataShareRequestQuestionsSummary_ThenABadRequestIsReturnedReportingTheError()
    {
        var testItems = CreateTestItems();

        var testGetDataShareRequestQuestionsSummaryRequest = testItems.Fixture.Create<GetDataShareRequestQuestionsSummaryRequest>();

        testItems.MockAcquirerDataShareRequestService.Setup(x => x.GetDataShareRequestQuestionsSummaryAsync(
                It.IsAny<Guid>()))
            .Throws(new Exception("test exception error message"));

        var result = await testItems.AcquirerDataShareRequestController.GetDataShareRequestQuestionsSummary(testGetDataShareRequestQuestionsSummaryRequest);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());

            var typedResult = result as BadRequestObjectResult;

            Assert.That(typedResult!.StatusCode, Is.EqualTo(400));
            Assert.That(typedResult.Value, Is.EqualTo("test exception error message"));
        });
    }

    [Test]
    public async Task GivenTheAcquirerDataShareRequestServiceWillThrowAnException_WhenIGetDataShareRequestQuestionsSummary_ThenTheExceptionIsLogged()
    {
        var testItems = CreateTestItems();

        var testGetDataShareRequestQuestionsSummaryRequest = testItems.Fixture.Create<GetDataShareRequestQuestionsSummaryRequest>();

        var testException = new Exception("test exception error message");
        testItems.MockAcquirerDataShareRequestService.Setup(x => x.GetDataShareRequestQuestionsSummaryAsync(
                It.IsAny<Guid>()))
            .Throws(testException);

        await testItems.AcquirerDataShareRequestController.GetDataShareRequestQuestionsSummary(testGetDataShareRequestQuestionsSummaryRequest);

        testItems.MockLogger.VerifyLog(LogLevel.Error, "test exception error message", testException);
    }
    #endregion

    #region GetDataShareRequestQuestionInformation() Tests
    [Test]
    public void GivenANullGetDataShareRequestQuestionInformationRequest_WhenIGetDataShareRequestQuestionInformation_ThenAnArgumentNullExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        Assert.That(() => testItems.AcquirerDataShareRequestController.GetDataShareRequestQuestionInformation(null!),
            Throws.ArgumentNullException.With.Property("ParamName").EqualTo("getDataShareRequestQuestionInformationRequest"));
    }

    [Test]
    public async Task GivenAGetDataShareRequestQuestionInformationRequest_WhenIGetDataShareRequestQuestionInformation_ThenTheAnOkResultIsReturnedWithTheValueReportedByTheAcquirerDataShareRequestService()
    {
        var testItems = CreateTestItems();

        var testGetDataShareRequestQuestionInformationRequest = testItems.Fixture.Create<GetDataShareRequestQuestionInformationRequest>();

        var testDataShareRequestQuestion = testItems.Fixture.Create<DataShareRequestQuestion>();

        testItems.MockAcquirerDataShareRequestService.Setup(x => x.GetDataShareRequestQuestionInformationAsync(
                testGetDataShareRequestQuestionInformationRequest.DataShareRequestId,
                testGetDataShareRequestQuestionInformationRequest.QuestionId))
            .ReturnsAsync(() => CreateTestServiceOperationDataResult(success: true, data: testDataShareRequestQuestion));

        var testGetDataShareRequestQuestionInformationResponse = testItems.Fixture.Create<GetDataShareRequestQuestionInformationResponse>();

        testItems.MockAcquirerDataShareRequestResponseFactory.Setup(x =>
                x.CreateGetDataShareRequestQuestionInformationResponse(testGetDataShareRequestQuestionInformationRequest, testDataShareRequestQuestion))
            .Returns(() => testGetDataShareRequestQuestionInformationResponse);

        var result = await testItems.AcquirerDataShareRequestController.GetDataShareRequestQuestionInformation(testGetDataShareRequestQuestionInformationRequest);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<OkObjectResult>());

            var typedResult = result as OkObjectResult;
            Assert.That(typedResult!.StatusCode, Is.EqualTo(200));
            Assert.That(typedResult.Value, Is.EqualTo(testGetDataShareRequestQuestionInformationResponse));
        });
    }

    [Test]
    public async Task GivenTheAcquirerDataShareRequestServiceWillReturnFailure_WhenIGetDataShareRequestQuestionInformation_ThenABadRequestIsReturnedReportingTheError()
    {
        var testItems = CreateTestItems();

        var testGetDataShareRequestQuestionInformationRequest = testItems.Fixture.Create<GetDataShareRequestQuestionInformationRequest>();

        testItems.MockAcquirerDataShareRequestService.Setup(x => x.GetDataShareRequestQuestionInformationAsync(
                It.IsAny<Guid>(),
                It.IsAny<Guid>()))
            .ReturnsAsync(() => CreateTestServiceOperationDataResult<DataShareRequestQuestion>(success: false, error: "test error message"));

        var result = await testItems.AcquirerDataShareRequestController.GetDataShareRequestQuestionInformation(testGetDataShareRequestQuestionInformationRequest);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());

            var typedResult = result as BadRequestObjectResult;

            Assert.That(typedResult!.StatusCode, Is.EqualTo(400));
            Assert.That(typedResult.Value, Is.EqualTo("test error message"));
        });
    }

    [Test]
    public async Task GivenTheAcquirerDataShareRequestServiceWillReturnFailure_WhenIGetDataShareRequestQuestionInformation_ThenTheErrorIsLogged()
    {
        var testItems = CreateTestItems();

        var testGetDataShareRequestQuestionInformationRequest = testItems.Fixture.Create<GetDataShareRequestQuestionInformationRequest>();

        testItems.MockAcquirerDataShareRequestService.Setup(x => x.GetDataShareRequestQuestionInformationAsync(
                It.IsAny<Guid>(),
                It.IsAny<Guid>()))
            .ReturnsAsync(() => CreateTestServiceOperationDataResult<DataShareRequestQuestion>(success: false, error: "test error message"));

        await testItems.AcquirerDataShareRequestController.GetDataShareRequestQuestionInformation(testGetDataShareRequestQuestionInformationRequest);

        testItems.MockLogger.VerifyLog(LogLevel.Error, "Failed to GetDataShareRequestQuestionInformation from AcquirerDataShareRequestService: test error message");
    }

    [Test]
    public async Task GivenTheAcquirerDataShareRequestServiceWillThrowAnException_WhenIGetDataShareRequestQuestionInformation_ThenABadRequestIsReturnedReportingTheError()
    {
        var testItems = CreateTestItems();

        var testGetDataShareRequestQuestionInformationRequest = testItems.Fixture.Create<GetDataShareRequestQuestionInformationRequest>();

        testItems.MockAcquirerDataShareRequestService.Setup(x => x.GetDataShareRequestQuestionInformationAsync(
                It.IsAny<Guid>(),
                It.IsAny<Guid>()))
            .Throws(new Exception("test exception error message"));

        var result = await testItems.AcquirerDataShareRequestController.GetDataShareRequestQuestionInformation(testGetDataShareRequestQuestionInformationRequest);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());

            var typedResult = result as BadRequestObjectResult;

            Assert.That(typedResult!.StatusCode, Is.EqualTo(400));
            Assert.That(typedResult.Value, Is.EqualTo("test exception error message"));
        });
    }

    [Test]
    public async Task GivenTheAcquirerDataShareRequestServiceWillThrowAnException_WhenIGetDataShareRequestQuestionInformation_ThenTheExceptionIsLogged()
    {
        var testItems = CreateTestItems();

        var testGetDataShareRequestQuestionInformationRequest = testItems.Fixture.Create<GetDataShareRequestQuestionInformationRequest>();

        var testException = new Exception("test exception error message");
        testItems.MockAcquirerDataShareRequestService.Setup(x => x.GetDataShareRequestQuestionInformationAsync(
                It.IsAny<Guid>(),
                It.IsAny<Guid>()))
            .Throws(testException);

        await testItems.AcquirerDataShareRequestController.GetDataShareRequestQuestionInformation(testGetDataShareRequestQuestionInformationRequest);

        testItems.MockLogger.VerifyLog(LogLevel.Error, "test exception error message", testException);
    }
    #endregion

    #region SetDataShareRequestQuestionAnswer() Tests
    [Test]
    public void GivenANullSetDataShareRequestQuestionAnswerRequest_WhenISetDataShareRequestQuestionAnswer_ThenAnArgumentNullExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        Assert.That(() => testItems.AcquirerDataShareRequestController.SetDataShareRequestQuestionAnswer(null!),
            Throws.ArgumentNullException.With.Property("ParamName").EqualTo("setDataShareRequestQuestionAnswerRequest"));
    }

    [Test]
    public async Task GivenASetDataShareRequestQuestionAnswerRequest_WhenISetDataShareRequestQuestionAnswer_ThenTheAnOkResultIsReturnedWithTheValueReportedByTheAcquirerDataShareRequestService()
    {
        var testItems = CreateTestItems();

        var testSetDataShareRequestQuestionAnswerRequest = testItems.Fixture.Create<SetDataShareRequestQuestionAnswerRequest>();

        var testSetDataShareRequestQuestionAnswerResult = testItems.Fixture.Create<SetDataShareRequestQuestionAnswerResult>();

        testItems.MockAcquirerDataShareRequestService.Setup(x => x.SetDataShareRequestQuestionAnswerAsync(
                testSetDataShareRequestQuestionAnswerRequest.DataShareRequestQuestionAnswer))
            .ReturnsAsync(() => CreateTestServiceOperationDataResult(success: true, data: testSetDataShareRequestQuestionAnswerResult));

        var testSetDataShareRequestQuestionAnswerResponse = testItems.Fixture.Create<SetDataShareRequestQuestionAnswerResponse>();

        testItems.MockAcquirerDataShareRequestResponseFactory.Setup(x =>
                x.CreateSetDataShareRequestQuestionAnswerResponse(testSetDataShareRequestQuestionAnswerRequest, testSetDataShareRequestQuestionAnswerResult))
            .Returns(() => testSetDataShareRequestQuestionAnswerResponse);

        var result = await testItems.AcquirerDataShareRequestController.SetDataShareRequestQuestionAnswer(testSetDataShareRequestQuestionAnswerRequest);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<OkObjectResult>());

            var typedResult = result as OkObjectResult;
            Assert.That(typedResult!.StatusCode, Is.EqualTo(200));
            Assert.That(typedResult.Value, Is.EqualTo(testSetDataShareRequestQuestionAnswerResponse));
        });
    }

    [Test]
    public async Task GivenTheAcquirerDataShareRequestServiceWillReturnFailure_WhenISetDataShareRequestQuestionAnswer_ThenABadRequestIsReturnedReportingTheError()
    {
        var testItems = CreateTestItems();

        var testSetDataShareRequestQuestionAnswerRequest = testItems.Fixture.Create<SetDataShareRequestQuestionAnswerRequest>();

        testItems.MockAcquirerDataShareRequestService.Setup(x => x.SetDataShareRequestQuestionAnswerAsync(
                It.IsAny<DataShareRequestQuestionAnswer>()))
            .ReturnsAsync(() => CreateTestServiceOperationDataResult<SetDataShareRequestQuestionAnswerResult>(success: false, error: "test error message"));

        var result = await testItems.AcquirerDataShareRequestController.SetDataShareRequestQuestionAnswer(testSetDataShareRequestQuestionAnswerRequest);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());

            var typedResult = result as BadRequestObjectResult;

            Assert.That(typedResult!.StatusCode, Is.EqualTo(400));
            Assert.That(typedResult.Value, Is.EqualTo("test error message"));
        });
    }

    [Test]
    public async Task GivenTheAcquirerDataShareRequestServiceWillReturnFailure_WhenISetDataShareRequestQuestionAnswer_ThenTheErrorIsLogged()
    {
        var testItems = CreateTestItems();

        var testSetDataShareRequestQuestionAnswerRequest = testItems.Fixture.Create<SetDataShareRequestQuestionAnswerRequest>();

        testItems.MockAcquirerDataShareRequestService.Setup(x => x.SetDataShareRequestQuestionAnswerAsync(
                It.IsAny<DataShareRequestQuestionAnswer>()))
            .ReturnsAsync(() => CreateTestServiceOperationDataResult<SetDataShareRequestQuestionAnswerResult>(success: false, error: "test error message"));

        await testItems.AcquirerDataShareRequestController.SetDataShareRequestQuestionAnswer(testSetDataShareRequestQuestionAnswerRequest);

        testItems.MockLogger.VerifyLog(LogLevel.Error, "Failed to SetDataShareRequestQuestionAnswer with AcquirerDataShareRequestService: test error message");
    }

    [Test]
    public async Task GivenTheAcquirerDataShareRequestServiceWillThrowAnException_WhenISetDataShareRequestQuestionAnswer_ThenABadRequestIsReturnedReportingTheError()
    {
        var testItems = CreateTestItems();

        var testSetDataShareRequestQuestionAnswerRequest = testItems.Fixture.Create<SetDataShareRequestQuestionAnswerRequest>();

        testItems.MockAcquirerDataShareRequestService.Setup(x => x.SetDataShareRequestQuestionAnswerAsync(
                It.IsAny<DataShareRequestQuestionAnswer>()))
            .Throws(new Exception("test exception error message"));

        var result = await testItems.AcquirerDataShareRequestController.SetDataShareRequestQuestionAnswer(testSetDataShareRequestQuestionAnswerRequest);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());

            var typedResult = result as BadRequestObjectResult;

            Assert.That(typedResult!.StatusCode, Is.EqualTo(400));
            Assert.That(typedResult.Value, Is.EqualTo("test exception error message"));
        });
    }

    [Test]
    public async Task GivenTheAcquirerDataShareRequestServiceWillThrowAnException_WhenISetDataShareRequestQuestionAnswer_ThenTheExceptionIsLogged()
    {
        var testItems = CreateTestItems();

        var testSetDataShareRequestQuestionAnswerRequest = testItems.Fixture.Create<SetDataShareRequestQuestionAnswerRequest>();

        var testException = new Exception("test exception error message");
        testItems.MockAcquirerDataShareRequestService.Setup(x => x.SetDataShareRequestQuestionAnswerAsync(
                It.IsAny<DataShareRequestQuestionAnswer>()))
            .Throws(testException);

        await testItems.AcquirerDataShareRequestController.SetDataShareRequestQuestionAnswer(testSetDataShareRequestQuestionAnswerRequest);

        testItems.MockLogger.VerifyLog(LogLevel.Error, "test exception error message", testException);
    }
    #endregion

    #region GetDataShareRequestAnswersSummary() Tests
    [Test]
    public void GivenANullGetDataShareRequestAnswersSummaryRequest_WhenIGetDataShareRequestAnswersSummary_ThenAnArgumentNullExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        Assert.That(() => testItems.AcquirerDataShareRequestController.GetDataShareRequestAnswersSummary(null!),
            Throws.ArgumentNullException.With.Property("ParamName").EqualTo("getDataShareRequestAnswersSummaryRequest"));
    }

    [Test]
    public async Task GivenAGetDataShareRequestAnswersSummaryRequest_WhenIGetDataShareRequestAnswersSummary_ThenTheAnOkResultIsReturnedWithTheValueReportedByTheAcquirerDataShareRequestService()
    {
        var testItems = CreateTestItems();

        var testGetDataShareRequestAnswersSummaryRequest = testItems.Fixture.Create<GetDataShareRequestAnswersSummaryRequest>();

        var testDataShareRequestAnswersSummary = testItems.Fixture.Create<DataShareRequestAnswersSummary>();

        testItems.MockAcquirerDataShareRequestService.Setup(x => x.GetDataShareRequestAnswersSummaryAsync(
                testGetDataShareRequestAnswersSummaryRequest.DataShareRequestId))
            .ReturnsAsync(() => CreateTestServiceOperationDataResult(success: true, data: testDataShareRequestAnswersSummary));

        var testGetDataShareRequestAnswersSummaryResponse = testItems.Fixture.Create<GetDataShareRequestAnswersSummaryResponse>();

        testItems.MockAcquirerDataShareRequestResponseFactory.Setup(x =>
                x.CreateGetDataShareRequestAnswersSummaryResponse(testGetDataShareRequestAnswersSummaryRequest, testDataShareRequestAnswersSummary))
            .Returns(() => testGetDataShareRequestAnswersSummaryResponse);

        var result = await testItems.AcquirerDataShareRequestController.GetDataShareRequestAnswersSummary(testGetDataShareRequestAnswersSummaryRequest);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<OkObjectResult>());

            var typedResult = result as OkObjectResult;
            Assert.That(typedResult!.StatusCode, Is.EqualTo(200));
            Assert.That(typedResult.Value, Is.EqualTo(testGetDataShareRequestAnswersSummaryResponse));
        });
    }

    [Test]
    public async Task GivenTheAcquirerDataShareRequestServiceWillReturnFailure_WhenIGetDataShareRequestAnswersSummary_ThenABadRequestIsReturnedReportingTheError()
    {
        var testItems = CreateTestItems();

        var testGetDataShareRequestAnswersSummaryRequest = testItems.Fixture.Create<GetDataShareRequestAnswersSummaryRequest>();

        testItems.MockAcquirerDataShareRequestService.Setup(x => x.GetDataShareRequestAnswersSummaryAsync(
                It.IsAny<Guid>()))
            .ReturnsAsync(() => CreateTestServiceOperationDataResult<DataShareRequestAnswersSummary>(success: false, error: "test error message"));

        var result = await testItems.AcquirerDataShareRequestController.GetDataShareRequestAnswersSummary(testGetDataShareRequestAnswersSummaryRequest);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());

            var typedResult = result as BadRequestObjectResult;

            Assert.That(typedResult!.StatusCode, Is.EqualTo(400));
            Assert.That(typedResult.Value, Is.EqualTo("test error message"));
        });
    }

    [Test]
    public async Task GivenTheAcquirerDataShareRequestServiceWillReturnFailure_WhenIGetDataShareRequestAnswersSummary_ThenTheErrorIsLogged()
    {
        var testItems = CreateTestItems();

        var testGetDataShareRequestAnswersSummaryRequest = testItems.Fixture.Create<GetDataShareRequestAnswersSummaryRequest>();

        testItems.MockAcquirerDataShareRequestService.Setup(x => x.GetDataShareRequestAnswersSummaryAsync(
                It.IsAny<Guid>()))
            .ReturnsAsync(() => CreateTestServiceOperationDataResult<DataShareRequestAnswersSummary>(success: false, error: "test error message"));

        await testItems.AcquirerDataShareRequestController.GetDataShareRequestAnswersSummary(testGetDataShareRequestAnswersSummaryRequest);

        testItems.MockLogger.VerifyLog(LogLevel.Error, "Failed to GetDataShareRequestAnswersSummary from AcquirerDataShareRequestService: test error message");
    }

    [Test]
    public async Task GivenTheAcquirerDataShareRequestServiceWillThrowAnException_WhenIGetDataShareRequestAnswersSummary_ThenABadRequestIsReturnedReportingTheError()
    {
        var testItems = CreateTestItems();

        var testGetDataShareRequestAnswersSummaryRequest = testItems.Fixture.Create<GetDataShareRequestAnswersSummaryRequest>();

        testItems.MockAcquirerDataShareRequestService.Setup(x => x.GetDataShareRequestAnswersSummaryAsync(
                It.IsAny<Guid>()))
            .Throws(new Exception("test exception error message"));

        var result = await testItems.AcquirerDataShareRequestController.GetDataShareRequestAnswersSummary(testGetDataShareRequestAnswersSummaryRequest);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());

            var typedResult = result as BadRequestObjectResult;

            Assert.That(typedResult!.StatusCode, Is.EqualTo(400));
            Assert.That(typedResult.Value, Is.EqualTo("test exception error message"));
        });
    }

    [Test]
    public async Task GivenTheAcquirerDataShareRequestServiceWillThrowAnException_WhenIGetDataShareRequestAnswersSummary_ThenTheExceptionIsLogged()
    {
        var testItems = CreateTestItems();

        var testGetDataShareRequestAnswersSummaryRequest = testItems.Fixture.Create<GetDataShareRequestAnswersSummaryRequest>();

        var testException = new Exception("test exception error message");
        testItems.MockAcquirerDataShareRequestService.Setup(x => x.GetDataShareRequestAnswersSummaryAsync(
                It.IsAny<Guid>()))
            .Throws(testException);

        await testItems.AcquirerDataShareRequestController.GetDataShareRequestAnswersSummary(testGetDataShareRequestAnswersSummaryRequest);

        testItems.MockLogger.VerifyLog(LogLevel.Error, "test exception error message", testException);
    }
    #endregion

    #region SubmitDataShareRequest() Tests
    [Test]
    public void GivenANullSubmitDataShareRequestRequest_WhenISubmitDataShareRequest_ThenAnArgumentNullExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        Assert.That(() => testItems.AcquirerDataShareRequestController.SubmitDataShareRequest(null!),
            Throws.ArgumentNullException.With.Property("ParamName").EqualTo("submitDataShareRequestRequest"));
    }

    [Test]
    public async Task GivenASubmitDataShareRequestRequest_WhenISubmitDataShareRequest_ThenTheAnOkResultIsReturned()
    {
        var testItems = CreateTestItems();

        var testSubmitDataShareRequestRequest = testItems.Fixture.Create<SubmitDataShareRequestRequest>();

        var testServiceOperationDataResult = CreateTestServiceOperationDataResult<DataShareRequestSubmissionResult>(success: true);

        testItems.MockAcquirerDataShareRequestService.Setup(x => x.SubmitDataShareRequestAsync(
                testSubmitDataShareRequestRequest.DataShareRequestId))
            .ReturnsAsync(() => testServiceOperationDataResult);

        var testSubmitDataShareRequestResponse = testItems.Fixture.Create<SubmitDataShareRequestResponse>();

        testItems.MockAcquirerDataShareRequestResponseFactory.Setup(x =>
                x.CreateSubmitDataShareRequestResponse(
                    testSubmitDataShareRequestRequest,
                    testServiceOperationDataResult.Data!))
            .Returns(() => testSubmitDataShareRequestResponse);

        var result = await testItems.AcquirerDataShareRequestController.SubmitDataShareRequest(testSubmitDataShareRequestRequest);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<OkObjectResult>());

            var typedResult = result as OkObjectResult;
            Assert.That(typedResult!.StatusCode, Is.EqualTo(200));
            Assert.That(typedResult.Value, Is.EqualTo(testSubmitDataShareRequestResponse));
        });
    }

    [Test]
    public async Task GivenTheAcquirerDataShareRequestServiceWillReturnFailure_WhenISubmitDataShareRequest_ThenABadRequestIsReturnedReportingTheError()
    {
        var testItems = CreateTestItems();

        var testSubmitDataShareRequestRequest = testItems.Fixture.Create<SubmitDataShareRequestRequest>();

        testItems.MockAcquirerDataShareRequestService.Setup(x => x.SubmitDataShareRequestAsync(
                It.IsAny<Guid>()))
            .ReturnsAsync(() => CreateTestServiceOperationDataResult<DataShareRequestSubmissionResult>(success: false, error: "test error message"));

        var result = await testItems.AcquirerDataShareRequestController.SubmitDataShareRequest(testSubmitDataShareRequestRequest);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());

            var typedResult = result as BadRequestObjectResult;

            Assert.That(typedResult!.StatusCode, Is.EqualTo(400));
            Assert.That(typedResult.Value, Is.EqualTo("test error message"));
        });
    }

    [Test]
    public async Task GivenTheAcquirerDataShareRequestServiceWillReturnFailure_WhenISubmitDataShareRequest_ThenTheErrorIsLogged()
    {
        var testItems = CreateTestItems();

        var testSubmitDataShareRequestRequest = testItems.Fixture.Create<SubmitDataShareRequestRequest>();

        testItems.MockAcquirerDataShareRequestService.Setup(x => x.SubmitDataShareRequestAsync(
                It.IsAny<Guid>()))
            .ReturnsAsync(() => CreateTestServiceOperationDataResult<DataShareRequestSubmissionResult>(success: false, error: "test error message"));

        await testItems.AcquirerDataShareRequestController.SubmitDataShareRequest(testSubmitDataShareRequestRequest);

        testItems.MockLogger.VerifyLog(LogLevel.Error, "Failed to SubmitDataShareRequest with AcquirerDataShareRequestService: test error message");
    }

    [Test]
    public async Task GivenTheAcquirerDataShareRequestServiceWillThrowAnException_WhenISubmitDataShareRequest_ThenABadRequestIsReturnedReportingTheError()
    {
        var testItems = CreateTestItems();

        var testSubmitDataShareRequestRequest = testItems.Fixture.Create<SubmitDataShareRequestRequest>();

        testItems.MockAcquirerDataShareRequestService.Setup(x => x.SubmitDataShareRequestAsync(
                It.IsAny<Guid>()))
            .Throws(new Exception("test exception error message"));

        var result = await testItems.AcquirerDataShareRequestController.SubmitDataShareRequest(testSubmitDataShareRequestRequest);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());

            var typedResult = result as BadRequestObjectResult;

            Assert.That(typedResult!.StatusCode, Is.EqualTo(400));
            Assert.That(typedResult.Value, Is.EqualTo("test exception error message"));
        });
    }

    [Test]
    public async Task GivenTheAcquirerDataShareRequestServiceWillThrowAnException_WhenISubmitDataShareRequest_ThenTheExceptionIsLogged()
    {
        var testItems = CreateTestItems();

        var testSubmitDataShareRequestRequest = testItems.Fixture.Create<SubmitDataShareRequestRequest>();

        var testException = new Exception("test exception error message");
        testItems.MockAcquirerDataShareRequestService.Setup(x => x.SubmitDataShareRequestAsync(
                It.IsAny<Guid>()))
            .Throws(testException);

        await testItems.AcquirerDataShareRequestController.SubmitDataShareRequest(testSubmitDataShareRequestRequest);

        testItems.MockLogger.VerifyLog(LogLevel.Error, "test exception error message", testException);
    }
    #endregion

    #region CancelDataShareRequest() Tests
    [Test]
    public void GivenANullCancelDataShareRequestRequest_WhenICancelDataShareRequest_ThenAnArgumentNullExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        Assert.That(() => testItems.AcquirerDataShareRequestController.CancelDataShareRequest(null!),
            Throws.ArgumentNullException.With.Property("ParamName").EqualTo("cancelDataShareRequestRequest"));
    }

    [Test]
    public async Task GivenACancelDataShareRequestRequest_WhenICancelDataShareRequest_ThenTheAnOkResultIsReturned()
    {
        var testItems = CreateTestItems();

        var testCancelDataShareRequestRequest = testItems.Fixture.Create<CancelDataShareRequestRequest>();

        testItems.MockAcquirerDataShareRequestService.Setup(x => x.CancelDataShareRequestAsync(
                testCancelDataShareRequestRequest.DataShareRequestId,
                testCancelDataShareRequestRequest.ReasonsForCancellation))
            .ReturnsAsync(() => CreateTestServiceOperationDataResult<DataShareRequestCancellationResult>(success: true));

        var testCancelDataShareRequestResponse = testItems.Fixture.Create<CancelDataShareRequestResponse>();

        testItems.MockAcquirerDataShareRequestResponseFactory.Setup(x =>
                x.CreateCancelDataShareRequestResponse(
                    testCancelDataShareRequestRequest,
                    It.IsAny<DataShareRequestCancellationResult>()))
            .Returns(() => testCancelDataShareRequestResponse);

        var result = await testItems.AcquirerDataShareRequestController.CancelDataShareRequest(testCancelDataShareRequestRequest);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<OkObjectResult>());

            var typedResult = result as OkObjectResult;
            Assert.That(typedResult!.StatusCode, Is.EqualTo(200));
            Assert.That(typedResult.Value, Is.EqualTo(testCancelDataShareRequestResponse));
        });
    }

    [Test]
    public async Task GivenTheAcquirerDataShareRequestServiceWillReturnFailure_WhenICancelDataShareRequest_ThenABadRequestIsReturnedReportingTheError()
    {
        var testItems = CreateTestItems();

        var testCancelDataShareRequestRequest = testItems.Fixture.Create<CancelDataShareRequestRequest>();

        testItems.MockAcquirerDataShareRequestService.Setup(x => x.CancelDataShareRequestAsync(
                It.IsAny<Guid>(),
                It.IsAny<string>()))
            .ReturnsAsync(() => CreateTestServiceOperationDataResult<DataShareRequestCancellationResult>(success: false, error: "test error message"));

        var result = await testItems.AcquirerDataShareRequestController.CancelDataShareRequest(testCancelDataShareRequestRequest);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());

            var typedResult = result as BadRequestObjectResult;

            Assert.That(typedResult!.StatusCode, Is.EqualTo(400));
            Assert.That(typedResult.Value, Is.EqualTo("test error message"));
        });
    }

    [Test]
    public async Task GivenTheAcquirerDataShareRequestServiceWillReturnFailure_WhenICancelDataShareRequest_ThenTheErrorIsLogged()
    {
        var testItems = CreateTestItems();

        var testCancelDataShareRequestRequest = testItems.Fixture.Create<CancelDataShareRequestRequest>();

        testItems.MockAcquirerDataShareRequestService.Setup(x => x.CancelDataShareRequestAsync(
                It.IsAny<Guid>(),
                It.IsAny<string>()))
            .ReturnsAsync(() => CreateTestServiceOperationDataResult<DataShareRequestCancellationResult>(success: false, error: "test error message"));

        await testItems.AcquirerDataShareRequestController.CancelDataShareRequest(testCancelDataShareRequestRequest);

        testItems.MockLogger.VerifyLog(LogLevel.Error, "Failed to CancelDataShareRequest with AcquirerDataShareRequestService: test error message");
    }

    [Test]
    public async Task GivenTheAcquirerDataShareRequestServiceWillThrowAnException_WhenICancelDataShareRequest_ThenABadRequestIsReturnedReportingTheError()
    {
        var testItems = CreateTestItems();

        var testCancelDataShareRequestRequest = testItems.Fixture.Create<CancelDataShareRequestRequest>();

        testItems.MockAcquirerDataShareRequestService.Setup(x => x.CancelDataShareRequestAsync(
                It.IsAny<Guid>(),
                It.IsAny<string>()))
            .Throws(new Exception("test exception error message"));

        var result = await testItems.AcquirerDataShareRequestController.CancelDataShareRequest(testCancelDataShareRequestRequest);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());

            var typedResult = result as BadRequestObjectResult;

            Assert.That(typedResult!.StatusCode, Is.EqualTo(400));
            Assert.That(typedResult.Value, Is.EqualTo("test exception error message"));
        });
    }

    [Test]
    public async Task GivenTheAcquirerDataShareRequestServiceWillThrowAnException_WhenICancelDataShareRequest_ThenTheExceptionIsLogged()
    {
        var testItems = CreateTestItems();

        var testCancelDataShareRequestRequest = testItems.Fixture.Create<CancelDataShareRequestRequest>();

        var testException = new Exception("test exception error message");
        testItems.MockAcquirerDataShareRequestService.Setup(x => x.CancelDataShareRequestAsync(
                It.IsAny<Guid>(),
                It.IsAny<string>()))
            .Throws(testException);

        await testItems.AcquirerDataShareRequestController.CancelDataShareRequest(testCancelDataShareRequestRequest);

        testItems.MockLogger.VerifyLog(LogLevel.Error, "test exception error message", testException);
    }
    #endregion

    #region DeleteDataShareRequest() Tests
    [Test]
    public void GivenANullDeleteDataShareRequestRequest_WhenIDeleteDataShareRequest_ThenAnArgumentNullExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        Assert.That(() => testItems.AcquirerDataShareRequestController.DeleteDataShareRequest(null!),
            Throws.ArgumentNullException.With.Property("ParamName").EqualTo("deleteDataShareRequestRequest"));
    }

    [Test]
    public async Task GivenADeleteDataShareRequestRequest_WhenIDeleteDataShareRequest_ThenTheAnOkResultIsReturned()
    {
        var testItems = CreateTestItems();

        var testDeleteDataShareRequestRequest = testItems.Fixture.Create<DeleteDataShareRequestRequest>();

        testItems.MockAcquirerDataShareRequestService.Setup(x => x.DeleteDataShareRequestAsync(
                testDeleteDataShareRequestRequest.DataShareRequestId))
            .ReturnsAsync(() => CreateTestServiceOperationDataResult<DataShareRequestDeletionResult>(success: true));

        var testDeleteDataShareRequestResponse = testItems.Fixture.Create<DeleteDataShareRequestResponse>();

        testItems.MockAcquirerDataShareRequestResponseFactory.Setup(x =>
                x.CreateDeleteDataShareRequestResponse(
                    testDeleteDataShareRequestRequest,
                    It.IsAny<DataShareRequestDeletionResult>()))
            .Returns(() => testDeleteDataShareRequestResponse);

        var result = await testItems.AcquirerDataShareRequestController.DeleteDataShareRequest(testDeleteDataShareRequestRequest);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<OkObjectResult>());

            var typedResult = result as OkObjectResult;
            Assert.That(typedResult!.StatusCode, Is.EqualTo((int)HttpStatusCode.OK));
            Assert.That(typedResult.Value, Is.EqualTo(testDeleteDataShareRequestResponse));
        });
    }

    [Test]
    public async Task GivenTheAcquirerDataShareRequestServiceWillReturnFailure_WhenIDeleteDataShareRequest_ThenABadRequestIsReturnedReportingTheError()
    {
        var testItems = CreateTestItems();

        var testDeleteDataShareRequestRequest = testItems.Fixture.Create<DeleteDataShareRequestRequest>();

        testItems.MockAcquirerDataShareRequestService.Setup(x => x.DeleteDataShareRequestAsync(
                It.IsAny<Guid>()))
            .ReturnsAsync(() => CreateTestServiceOperationDataResult<DataShareRequestDeletionResult>(success: false, error: "test error message"));

        var result = await testItems.AcquirerDataShareRequestController.DeleteDataShareRequest(testDeleteDataShareRequestRequest);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());

            var typedResult = result as BadRequestObjectResult;

            Assert.That(typedResult!.StatusCode, Is.EqualTo((int)HttpStatusCode.BadRequest));
            Assert.That(typedResult.Value, Is.EqualTo("test error message"));
        });
    }

    [Test]
    public async Task GivenTheAcquirerDataShareRequestServiceWillReturnFailureWithHttpStatusCode_WhenIDeleteDataShareRequest_ThenAnObjectResultWithTheGivenStatusCodeIsReturned()
    {
        var testItems = CreateTestItems();

        var testDeleteDataShareRequestRequest = testItems.Fixture.Create<DeleteDataShareRequestRequest>();

        testItems.MockAcquirerDataShareRequestService.Setup(x => x.DeleteDataShareRequestAsync(
                It.IsAny<Guid>()))
            .ReturnsAsync(() => CreateTestServiceOperationDataResult<DataShareRequestDeletionResult>(
                success: false, statusCode: HttpStatusCode.BadGateway));

        var result = await testItems.AcquirerDataShareRequestController.DeleteDataShareRequest(testDeleteDataShareRequestRequest);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<ObjectResult>());

            var typedResult = result as ObjectResult;

            Assert.That(typedResult!.StatusCode, Is.EqualTo((int)HttpStatusCode.BadGateway));
        });
    }

    [Test]
    public async Task GivenTheAcquirerDataShareRequestServiceWillReturnFailure_WhenIDeleteDataShareRequest_ThenTheErrorIsLogged()
    {
        var testItems = CreateTestItems();

        var testDeleteDataShareRequestRequest = testItems.Fixture.Create<DeleteDataShareRequestRequest>();

        testItems.MockAcquirerDataShareRequestService.Setup(x => x.DeleteDataShareRequestAsync(
                It.IsAny<Guid>()))
            .ReturnsAsync(() => CreateTestServiceOperationDataResult<DataShareRequestDeletionResult>(success: false, error: "test error message"));

        await testItems.AcquirerDataShareRequestController.DeleteDataShareRequest(testDeleteDataShareRequestRequest);

        testItems.MockLogger.VerifyLog(LogLevel.Error, "Failed to DeleteDataShareRequest with AcquirerDataShareRequestService: test error message");
    }

    [Test]
    public async Task GivenTheAcquirerDataShareRequestServiceWillThrowAnException_WhenIDeleteDataShareRequest_ThenABadRequestIsReturnedReportingTheError()
    {
        var testItems = CreateTestItems();

        var testDeleteDataShareRequestRequest = testItems.Fixture.Create<DeleteDataShareRequestRequest>();

        testItems.MockAcquirerDataShareRequestService.Setup(x => x.DeleteDataShareRequestAsync(
                It.IsAny<Guid>()))
            .Throws(new Exception("test exception error message"));

        var result = await testItems.AcquirerDataShareRequestController.DeleteDataShareRequest(testDeleteDataShareRequestRequest);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());

            var typedResult = result as BadRequestObjectResult;

            Assert.That(typedResult!.StatusCode, Is.EqualTo(400));
            Assert.That(typedResult.Value, Is.EqualTo("test exception error message"));
        });
    }

    [Test]
    public async Task GivenTheAcquirerDataShareRequestServiceWillThrowAnException_WhenIDeleteDataShareRequest_ThenTheExceptionIsLogged()
    {
        var testItems = CreateTestItems();

        var testDeleteDataShareRequestRequest = testItems.Fixture.Create<DeleteDataShareRequestRequest>();

        var testException = new Exception("test exception error message");
        testItems.MockAcquirerDataShareRequestService.Setup(x => x.DeleteDataShareRequestAsync(
                It.IsAny<Guid>()))
            .Throws(testException);

        await testItems.AcquirerDataShareRequestController.DeleteDataShareRequest(testDeleteDataShareRequestRequest);

        testItems.MockLogger.VerifyLog(LogLevel.Error, "test exception error message", testException);
    }
    #endregion

    #region Test Data Creation
    private static IServiceOperationDataResult<T> CreateTestServiceOperationDataResult<T>(
        bool? success = null,
        string? error = null,
        T? data = default,
        HttpStatusCode? statusCode = null)
    {
        var mockServiceOperationDataResult = new Mock<IServiceOperationDataResult<T>>();

        mockServiceOperationDataResult.SetupGet(x => x.Success).Returns(success ?? true);
        mockServiceOperationDataResult.SetupGet(x => x.Error).Returns(error);
        mockServiceOperationDataResult.SetupGet(x => x.Data).Returns(data);
        mockServiceOperationDataResult.SetupGet(x => x.StatusCode).Returns(statusCode);

        return mockServiceOperationDataResult.Object;
    }
    #endregion

    #region Test Item Creation
    private static TestItems CreateTestItems()
    {
        var fixture = new Fixture().Customize(new AutoMoqCustomization());

        var mockLogger = Mock.Get(fixture.Freeze<ILogger<AcquirerDataShareRequestController>>());
        var mockAcquirerDataShareRequestService = Mock.Get(fixture.Freeze<IAcquirerDataShareRequestService>());
        var mockAcquirerDataShareRequestResponseFactory = Mock.Get(fixture.Freeze<IAcquirerDataShareRequestResponseFactory>());

        var questionConfigurationController = new AcquirerDataShareRequestController(
            mockLogger.Object,
            mockAcquirerDataShareRequestService.Object,
            mockAcquirerDataShareRequestResponseFactory.Object);

        return new TestItems(fixture, questionConfigurationController,
            mockLogger, mockAcquirerDataShareRequestService, mockAcquirerDataShareRequestResponseFactory);
    }

    private class TestItems(
        IFixture fixture,
        AcquirerDataShareRequestController acquirerDataShareRequestController,
        Mock<ILogger<AcquirerDataShareRequestController>> mockLogger,
        Mock<IAcquirerDataShareRequestService> mockAcquirerDataShareRequestService,
        Mock<IAcquirerDataShareRequestResponseFactory> mockAcquirerDataShareRequestResponseFactory)
    {
        public IFixture Fixture { get; } = fixture;
        public AcquirerDataShareRequestController AcquirerDataShareRequestController { get; } = acquirerDataShareRequestController;
        public Mock<ILogger<AcquirerDataShareRequestController>> MockLogger { get; } = mockLogger;
        public Mock<IAcquirerDataShareRequestService> MockAcquirerDataShareRequestService { get; } = mockAcquirerDataShareRequestService;
        public Mock<IAcquirerDataShareRequestResponseFactory> MockAcquirerDataShareRequestResponseFactory { get; } = mockAcquirerDataShareRequestResponseFactory;
    }
    #endregion
}