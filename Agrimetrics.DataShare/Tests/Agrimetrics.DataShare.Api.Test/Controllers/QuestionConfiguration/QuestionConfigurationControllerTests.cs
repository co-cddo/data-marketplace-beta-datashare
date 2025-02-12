using AutoFixture.AutoMoq;
using AutoFixture;
using NUnit.Framework;
using Agrimetrics.DataShare.Api.Controllers.QuestionConfiguration;
using Microsoft.Extensions.Logging;
using Moq;
using Agrimetrics.DataShare.Api.Logic.Services.QuestionConfiguration;
using Agrimetrics.DataShare.Api.Dto.Requests.QuestionConfiguration.CompulsoryQuestions;
using Agrimetrics.DataShare.Api.Dto.Responses.QuestionConfiguration.CompulsoryQuestions;
using Agrimetrics.DataShare.Api.Logic.Services.ServiceOperationResults;
using Microsoft.AspNetCore.Mvc;
using Agrimetrics.DataShare.Api.Test.TestHelpers;
using Agrimetrics.DataShare.Api.Dto.Models.QuestionConfiguration.CompulsoryQuestions;

namespace Agrimetrics.DataShare.Api.Test.Controllers.QuestionConfiguration;

[TestFixture]
public class QuestionConfigurationControllerTests
{
    #region Compulsory Question-Related Tests
    #region GetCompulsoryQuestions() Tests
    [Test]
    public void GivenANullGetCompulsoryQuestionsRequest_WhenIGetCompulsoryQuestions_ThenAnArgumentNullExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        Assert.That(() => testItems.QuestionConfigurationController.GetCompulsoryQuestions(null!),
            Throws.ArgumentNullException.With.Property("ParamName").EqualTo("getCompulsoryQuestionsRequest"));
    }

    [Test]
    public async Task GivenAGetCompulsoryQuestionsRequest_WhenIGetCompulsoryQuestions_ThenTheAnOkResultIsReturnedWithTheValueReportedByTheQuestionConfigurationService()
    {
        var testItems = CreateTestItems();

        var testGetCompulsoryQuestionsRequest = testItems.Fixture.Create<GetCompulsoryQuestionsRequest>();

        var testCompulsoryQuestionSet = testItems.Fixture.Create<CompulsoryQuestionSet>();

        testItems.MockQuestionConfigurationService.Setup(x => x.GetCompulsoryQuestionsAsync(testGetCompulsoryQuestionsRequest.RequestingUserId))
            .ReturnsAsync(() => CreateTestServiceOperationDataResult(success: true, data: testCompulsoryQuestionSet));

        var testResponse = testItems.Fixture.Create<GetCompulsoryQuestionsResponse>();

        testItems.MockQuestionConfigurationResponseFactory.Setup(x =>
                x.CreateGetCompulsoryQuestionsResponse(testGetCompulsoryQuestionsRequest, testCompulsoryQuestionSet))
            .Returns(() => testResponse);

        var result = await testItems.QuestionConfigurationController.GetCompulsoryQuestions(testGetCompulsoryQuestionsRequest);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<OkObjectResult>());

            var typedResult = result as OkObjectResult;
            Assert.That(typedResult!.StatusCode, Is.EqualTo(200));
            Assert.That(typedResult.Value, Is.EqualTo(testResponse));
        });
    }

    [Test]
    public async Task GivenTheQuestionConfigurationServiceWillReturnFailure_WhenIGetCompulsoryQuestions_ThenABadRequestIsReturnedReportingTheError()
    {
        var testItems = CreateTestItems();

        var testGetCompulsoryQuestionsRequest = testItems.Fixture.Create<GetCompulsoryQuestionsRequest>();

        testItems.MockQuestionConfigurationService.Setup(x => x.GetCompulsoryQuestionsAsync(testGetCompulsoryQuestionsRequest.RequestingUserId))
            .ReturnsAsync(() => CreateTestServiceOperationDataResult<CompulsoryQuestionSet>(success: false, error: "test error message"));

        var result = await testItems.QuestionConfigurationController.GetCompulsoryQuestions(testGetCompulsoryQuestionsRequest);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());

            var typedResult = result as BadRequestObjectResult;

            Assert.That(typedResult!.StatusCode, Is.EqualTo(400));
            Assert.That(typedResult.Value, Is.EqualTo("test error message"));
        });
    }

    [Test]
    public async Task GivenTheQuestionConfigurationServiceWillReturnFailure_WhenIGetCompulsoryQuestions_ThenTheErrorIsLogged()
    {
        var testItems = CreateTestItems();

        var testGetCompulsoryQuestionsRequest = testItems.Fixture.Create<GetCompulsoryQuestionsRequest>();

        testItems.MockQuestionConfigurationService.Setup(x => x.GetCompulsoryQuestionsAsync(testGetCompulsoryQuestionsRequest.RequestingUserId))
            .ReturnsAsync(() => CreateTestServiceOperationDataResult<CompulsoryQuestionSet>(success: false, error: "test error message"));

        await testItems.QuestionConfigurationController.GetCompulsoryQuestions(testGetCompulsoryQuestionsRequest);

        testItems.MockLogger.VerifyLog(LogLevel.Error, "Failed to GetCompulsoryQuestions from QuestionConfigurationService: test error message");
    }

    [Test]
    public async Task GivenTheQuestionConfigurationServiceWillThrowAnException_WhenIGetCompulsoryQuestions_ThenABadRequestIsReturnedReportingTheError()
    {
        var testItems = CreateTestItems();

        testItems.MockQuestionConfigurationService.Setup(x => x.GetCompulsoryQuestionsAsync(It.IsAny<int>()))
            .Throws(new Exception("test exception error message"));

        var result = await testItems.QuestionConfigurationController.GetCompulsoryQuestions(testItems.Fixture.Create<GetCompulsoryQuestionsRequest>());

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());

            var typedResult = result as BadRequestObjectResult;

            Assert.That(typedResult!.StatusCode, Is.EqualTo(400));
            Assert.That(typedResult.Value, Is.EqualTo("test exception error message"));
        });
    }

    [Test]
    public async Task GivenTheQuestionConfigurationServiceWillThrowAnException_WhenIGetCompulsoryQuestions_ThenTheExceptionIsLogged()
    {
        var testItems = CreateTestItems();

        var testException = new Exception("test exception error message");
        testItems.MockQuestionConfigurationService.Setup(x => x.GetCompulsoryQuestionsAsync(It.IsAny<int>()))
            .Throws(testException);

        await testItems.QuestionConfigurationController.GetCompulsoryQuestions(testItems.Fixture.Create<GetCompulsoryQuestionsRequest>());

        testItems.MockLogger.VerifyLog(LogLevel.Error, "test exception error message", testException);
    }
    #endregion

    #region SetCompulsoryQuestion() Tests
    [Test]
    public void GivenANullSetCompulsoryQuestionRequest_WhenISetCompulsoryQuestion_ThenAnArgumentNullExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        Assert.That(() => testItems.QuestionConfigurationController.SetCompulsoryQuestion(null!),
            Throws.ArgumentNullException.With.Property("ParamName").EqualTo("setCompulsoryQuestionRequest"));
    }

    [Test]
    public async Task GivenASetCompulsoryQuestionRequest_WhenISetCompulsoryQuestion_ThenTheAnOkResultIsReturnedWithTheValueReportedByTheQuestionConfigurationService()
    {
        var testItems = CreateTestItems();

        var testSetCompulsoryQuestionRequest = testItems.Fixture.Create<SetCompulsoryQuestionRequest>();

        testItems.MockQuestionConfigurationService.Setup(x => x.SetCompulsoryQuestionAsync(
                testSetCompulsoryQuestionRequest.RequestingUserId,
                testSetCompulsoryQuestionRequest.QuestionId))
            .ReturnsAsync(() => CreateTestServiceOperationResult(success: true));

        var testResponse = testItems.Fixture.Create<SetCompulsoryQuestionResponse>();

        testItems.MockQuestionConfigurationResponseFactory.Setup(x =>
                x.CreateSetCompulsoryQuestionResponse(testSetCompulsoryQuestionRequest))
            .Returns(() => testResponse);

        var result = await testItems.QuestionConfigurationController.SetCompulsoryQuestion(testSetCompulsoryQuestionRequest);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<OkObjectResult>());

            var typedResult = result as OkObjectResult;
            Assert.That(typedResult!.StatusCode, Is.EqualTo(200));
            Assert.That(typedResult.Value, Is.EqualTo(testResponse));
        });
    }

    [Test]
    public async Task GivenTheQuestionConfigurationServiceWillReturnFailure_WhenISetCompulsoryQuestion_ThenABadRequestIsReturnedReportingTheError()
    {
        var testItems = CreateTestItems();

        var testSetCompulsoryQuestionRequest = testItems.Fixture.Create<SetCompulsoryQuestionRequest>();

        testItems.MockQuestionConfigurationService.Setup(x => x.SetCompulsoryQuestionAsync(
                testSetCompulsoryQuestionRequest.RequestingUserId, testSetCompulsoryQuestionRequest.QuestionId))
            .ReturnsAsync(() => CreateTestServiceOperationResult(success: false, error: "test error message"));

        var result = await testItems.QuestionConfigurationController.SetCompulsoryQuestion(testSetCompulsoryQuestionRequest);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());

            var typedResult = result as BadRequestObjectResult;

            Assert.That(typedResult!.StatusCode, Is.EqualTo(400));
            Assert.That(typedResult.Value, Is.EqualTo("test error message"));
        });
    }

    [Test]
    public async Task GivenTheQuestionConfigurationServiceWillReturnFailure_WhenISetCompulsoryQuestion_ThenTheErrorIsLogged()
    {
        var testItems = CreateTestItems();

        var testSetCompulsoryQuestionRequest = testItems.Fixture.Create<SetCompulsoryQuestionRequest>();

        testItems.MockQuestionConfigurationService.Setup(x => x.SetCompulsoryQuestionAsync(
                testSetCompulsoryQuestionRequest.RequestingUserId,
                testSetCompulsoryQuestionRequest.QuestionId))
            .ReturnsAsync(() => CreateTestServiceOperationResult(success: false, error: "test error message"));

        await testItems.QuestionConfigurationController.SetCompulsoryQuestion(testSetCompulsoryQuestionRequest);

        testItems.MockLogger.VerifyLog(LogLevel.Error, "Failed to SetCompulsoryQuestion from QuestionConfigurationService: test error message");
    }

    [Test]
    public async Task GivenTheQuestionConfigurationServiceWillThrowAnException_WhenISetCompulsoryQuestion_ThenABadRequestIsReturnedReportingTheError()
    {
        var testItems = CreateTestItems();

        testItems.MockQuestionConfigurationService.Setup(x => x.SetCompulsoryQuestionAsync(It.IsAny<int>(), It.IsAny<Guid>()))
            .Throws(new Exception("test exception error message"));

        var result = await testItems.QuestionConfigurationController.SetCompulsoryQuestion(testItems.Fixture.Create<SetCompulsoryQuestionRequest>());

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());

            var typedResult = result as BadRequestObjectResult;

            Assert.That(typedResult!.StatusCode, Is.EqualTo(400));
            Assert.That(typedResult.Value, Is.EqualTo("test exception error message"));
        });
    }

    [Test]
    public async Task GivenTheQuestionConfigurationServiceWillThrowAnException_WhenISetCompulsoryQuestion_ThenTheExceptionIsLogged()
    {
        var testItems = CreateTestItems();

        var testException = new Exception("test exception error message");
        testItems.MockQuestionConfigurationService.Setup(x => x.SetCompulsoryQuestionAsync(It.IsAny<int>(), It.IsAny<Guid>()))
            .Throws(testException);

        await testItems.QuestionConfigurationController.SetCompulsoryQuestion(testItems.Fixture.Create<SetCompulsoryQuestionRequest>());

        testItems.MockLogger.VerifyLog(LogLevel.Error, "test exception error message", testException);
    }
    #endregion

    #region ClearCompulsoryQuestion() Tests
    [Test]
    public void GivenANullClearCompulsoryQuestionRequest_WhenIClearCompulsoryQuestion_ThenAnArgumentNullExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        Assert.That(() => testItems.QuestionConfigurationController.ClearCompulsoryQuestion(null!),
            Throws.ArgumentNullException.With.Property("ParamName").EqualTo("clearCompulsoryQuestionRequest"));
    }

    [Test]
    public async Task GivenAClearCompulsoryQuestionRequest_WhenIClearCompulsoryQuestion_ThenTheAnOkResultIsReturnedWithTheValueReportedByTheQuestionConfigurationService()
    {
        var testItems = CreateTestItems();

        var testClearCompulsoryQuestionRequest = testItems.Fixture.Create<ClearCompulsoryQuestionRequest>();

        testItems.MockQuestionConfigurationService.Setup(x => x.ClearCompulsoryQuestionAsync(
                testClearCompulsoryQuestionRequest.RequestingUserId,
                testClearCompulsoryQuestionRequest.QuestionId))
            .ReturnsAsync(() => CreateTestServiceOperationResult(success: true));

        var testResponse = testItems.Fixture.Create<ClearCompulsoryQuestionResponse>();

        testItems.MockQuestionConfigurationResponseFactory.Setup(x =>
                x.CreateClearCompulsoryQuestionResponse(testClearCompulsoryQuestionRequest))
            .Returns(() => testResponse);

        var result = await testItems.QuestionConfigurationController.ClearCompulsoryQuestion(testClearCompulsoryQuestionRequest);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<OkObjectResult>());

            var typedResult = result as OkObjectResult;
            Assert.That(typedResult!.StatusCode, Is.EqualTo(200));
            Assert.That(typedResult.Value, Is.EqualTo(testResponse));
        });
    }

    [Test]
    public async Task GivenTheQuestionConfigurationServiceWillReturnFailure_WhenIClearCompulsoryQuestion_ThenABadRequestIsReturnedReportingTheError()
    {
        var testItems = CreateTestItems();

        var testClearCompulsoryQuestionRequest = testItems.Fixture.Create<ClearCompulsoryQuestionRequest>();

        testItems.MockQuestionConfigurationService.Setup(x => x.ClearCompulsoryQuestionAsync(
                testClearCompulsoryQuestionRequest.RequestingUserId, testClearCompulsoryQuestionRequest.QuestionId))
            .ReturnsAsync(() => CreateTestServiceOperationResult(success: false, error: "test error message"));

        var result = await testItems.QuestionConfigurationController.ClearCompulsoryQuestion(testClearCompulsoryQuestionRequest);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());

            var typedResult = result as BadRequestObjectResult;

            Assert.That(typedResult!.StatusCode, Is.EqualTo(400));
            Assert.That(typedResult.Value, Is.EqualTo("test error message"));
        });
    }

    [Test]
    public async Task GivenTheQuestionConfigurationServiceWillReturnFailure_WhenIClearCompulsoryQuestion_ThenTheErrorIsLogged()
    {
        var testItems = CreateTestItems();

        var testClearCompulsoryQuestionRequest = testItems.Fixture.Create<ClearCompulsoryQuestionRequest>();

        testItems.MockQuestionConfigurationService.Setup(x => x.ClearCompulsoryQuestionAsync(
                testClearCompulsoryQuestionRequest.RequestingUserId,
                testClearCompulsoryQuestionRequest.QuestionId))
            .ReturnsAsync(() => CreateTestServiceOperationResult(success: false, error: "test error message"));

        await testItems.QuestionConfigurationController.ClearCompulsoryQuestion(testClearCompulsoryQuestionRequest);

        testItems.MockLogger.VerifyLog(LogLevel.Error, "Failed to ClearCompulsoryQuestion from QuestionConfigurationService: test error message");
    }

    [Test]
    public async Task GivenTheQuestionConfigurationServiceWillThrowAnException_WhenIClearCompulsoryQuestion_ThenABadRequestIsReturnedReportingTheError()
    {
        var testItems = CreateTestItems();

        testItems.MockQuestionConfigurationService.Setup(x => x.ClearCompulsoryQuestionAsync(It.IsAny<int>(), It.IsAny<Guid>()))
            .Throws(new Exception("test exception error message"));

        var result = await testItems.QuestionConfigurationController.ClearCompulsoryQuestion(testItems.Fixture.Create<ClearCompulsoryQuestionRequest>());

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());

            var typedResult = result as BadRequestObjectResult;

            Assert.That(typedResult!.StatusCode, Is.EqualTo(400));
            Assert.That(typedResult.Value, Is.EqualTo("test exception error message"));
        });
    }

    [Test]
    public async Task GivenTheQuestionConfigurationServiceWillThrowAnException_WhenIClearCompulsoryQuestion_ThenTheExceptionIsLogged()
    {
        var testItems = CreateTestItems();

        var testException = new Exception("test exception error message");
        testItems.MockQuestionConfigurationService.Setup(x => x.ClearCompulsoryQuestionAsync(It.IsAny<int>(), It.IsAny<Guid>()))
            .Throws(testException);

        await testItems.QuestionConfigurationController.ClearCompulsoryQuestion(testItems.Fixture.Create<ClearCompulsoryQuestionRequest>());

        testItems.MockLogger.VerifyLog(LogLevel.Error, "test exception error message", testException);
    }
    #endregion
    #endregion

    #region Compulsory Supplier-Mandated Question-Related Tests
    #region GetCompulsorySupplierMandatedQuestions() Tests
    [Test]
    public void GivenANullGetCompulsorySupplierMandatedQuestionsRequest_WhenIGetCompulsorySupplierMandatedQuestions_ThenAnArgumentNullExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        Assert.That(() => testItems.QuestionConfigurationController.GetCompulsorySupplierMandatedQuestions(null!),
            Throws.ArgumentNullException.With.Property("ParamName").EqualTo("getCompulsorySupplierMandatedQuestionsRequest"));
    }

    [Test]
    public async Task GivenAGetCompulsorySupplierMandatedQuestionsRequest_WhenIGetCompulsorySupplierMandatedQuestions_ThenTheAnOkResultIsReturnedWithTheValueReportedByTheQuestionConfigurationService()
    {
        var testItems = CreateTestItems();

        var testGetCompulsorySupplierMandatedQuestionsRequest = testItems.Fixture.Create<GetCompulsorySupplierMandatedQuestionsRequest>();

        var testCompulsorySupplierMandatedQuestionSet = testItems.Fixture.Create<CompulsorySupplierMandatedQuestionSet>();

        testItems.MockQuestionConfigurationService.Setup(x => x.GetCompulsorySupplierMandatedQuestionsAsync(
                testGetCompulsorySupplierMandatedQuestionsRequest.RequestingUserId,
                testGetCompulsorySupplierMandatedQuestionsRequest.SupplierOrganisationId))
            .ReturnsAsync(() => CreateTestServiceOperationDataResult(success: true, data: testCompulsorySupplierMandatedQuestionSet));

        var testResponse = testItems.Fixture.Create<GetCompulsorySupplierMandatedQuestionsResponse>();

        testItems.MockQuestionConfigurationResponseFactory.Setup(x =>
                x.CreateGetCompulsorySupplierMandatedQuestionsResponse(testGetCompulsorySupplierMandatedQuestionsRequest, testCompulsorySupplierMandatedQuestionSet))
            .Returns(() => testResponse);

        var result = await testItems.QuestionConfigurationController.GetCompulsorySupplierMandatedQuestions(testGetCompulsorySupplierMandatedQuestionsRequest);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<OkObjectResult>());

            var typedResult = result as OkObjectResult;
            Assert.That(typedResult!.StatusCode, Is.EqualTo(200));
            Assert.That(typedResult.Value, Is.EqualTo(testResponse));
        });
    }

    [Test]
    public async Task GivenTheQuestionConfigurationServiceWillReturnFailure_WhenIGetCompulsorySupplierMandatedQuestions_ThenABadRequestIsReturnedReportingTheError()
    {
        var testItems = CreateTestItems();

        var testGetCompulsorySupplierMandatedQuestionsRequest = testItems.Fixture.Create<GetCompulsorySupplierMandatedQuestionsRequest>();

        testItems.MockQuestionConfigurationService.Setup(x => x.GetCompulsorySupplierMandatedQuestionsAsync(
                testGetCompulsorySupplierMandatedQuestionsRequest.RequestingUserId,
                testGetCompulsorySupplierMandatedQuestionsRequest.SupplierOrganisationId))
            .ReturnsAsync(() => CreateTestServiceOperationDataResult<CompulsorySupplierMandatedQuestionSet>(success: false, error: "test error message"));

        var result = await testItems.QuestionConfigurationController.GetCompulsorySupplierMandatedQuestions(testGetCompulsorySupplierMandatedQuestionsRequest);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());

            var typedResult = result as BadRequestObjectResult;

            Assert.That(typedResult!.StatusCode, Is.EqualTo(400));
            Assert.That(typedResult.Value, Is.EqualTo("test error message"));
        });
    }

    [Test]
    public async Task GivenTheQuestionConfigurationServiceWillReturnFailure_WhenIGetCompulsorySupplierMandatedQuestions_ThenTheErrorIsLogged()
    {
        var testItems = CreateTestItems();

        var testGetCompulsorySupplierMandatedQuestionsRequest = testItems.Fixture.Create<GetCompulsorySupplierMandatedQuestionsRequest>();

        testItems.MockQuestionConfigurationService.Setup(x => x.GetCompulsorySupplierMandatedQuestionsAsync(
                testGetCompulsorySupplierMandatedQuestionsRequest.RequestingUserId,
                testGetCompulsorySupplierMandatedQuestionsRequest.SupplierOrganisationId))
            .ReturnsAsync(() => CreateTestServiceOperationDataResult<CompulsorySupplierMandatedQuestionSet>(success: false, error: "test error message"));

        await testItems.QuestionConfigurationController.GetCompulsorySupplierMandatedQuestions(testGetCompulsorySupplierMandatedQuestionsRequest);

        testItems.MockLogger.VerifyLog(LogLevel.Error, "Failed to GetCompulsorySupplierMandatedQuestions from QuestionConfigurationService: test error message");
    }

    [Test]
    public async Task GivenTheQuestionConfigurationServiceWillThrowAnException_WhenIGetCompulsorySupplierMandatedQuestions_ThenABadRequestIsReturnedReportingTheError()
    {
        var testItems = CreateTestItems();

        testItems.MockQuestionConfigurationService.Setup(x => x.GetCompulsorySupplierMandatedQuestionsAsync(It.IsAny<int>(), It.IsAny<int>()))
            .Throws(new Exception("test exception error message"));

        var result = await testItems.QuestionConfigurationController.GetCompulsorySupplierMandatedQuestions(testItems.Fixture.Create<GetCompulsorySupplierMandatedQuestionsRequest>());

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());

            var typedResult = result as BadRequestObjectResult;

            Assert.That(typedResult!.StatusCode, Is.EqualTo(400));
            Assert.That(typedResult.Value, Is.EqualTo("test exception error message"));
        });
    }

    [Test]
    public async Task GivenTheQuestionConfigurationServiceWillThrowAnException_WhenIGetCompulsorySupplierMandatedQuestions_ThenTheExceptionIsLogged()
    {
        var testItems = CreateTestItems();

        var testException = new Exception("test exception error message");
        testItems.MockQuestionConfigurationService.Setup(x => x.GetCompulsorySupplierMandatedQuestionsAsync(It.IsAny<int>(), It.IsAny<int>()))
            .Throws(testException);

        await testItems.QuestionConfigurationController.GetCompulsorySupplierMandatedQuestions(testItems.Fixture.Create<GetCompulsorySupplierMandatedQuestionsRequest>());

        testItems.MockLogger.VerifyLog(LogLevel.Error, "test exception error message", testException);
    }
    #endregion

    #region SetCompulsorySupplierMandatedQuestion() Tests
    [Test]
    public void GivenANullSetCompulsorySupplierMandatedQuestionRequest_WhenISetCompulsorySupplierMandatedQuestion_ThenAnArgumentNullExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        Assert.That(() => testItems.QuestionConfigurationController.SetCompulsorySupplierMandatedQuestion(null!),
            Throws.ArgumentNullException.With.Property("ParamName").EqualTo("setCompulsorySupplierMandatedQuestionRequest"));
    }

    [Test]
    public async Task GivenASetCompulsorySupplierMandatedQuestionRequest_WhenISetCompulsorySupplierMandatedQuestion_ThenTheAnOkResultIsReturnedWithTheValueReportedByTheQuestionConfigurationService()
    {
        var testItems = CreateTestItems();

        var testSetCompulsorySupplierMandatedQuestionRequest = testItems.Fixture.Create<SetCompulsorySupplierMandatedQuestionRequest>();

        testItems.MockQuestionConfigurationService.Setup(x => x.SetCompulsorySupplierMandatedQuestionAsync(
                testSetCompulsorySupplierMandatedQuestionRequest.RequestingUserId,
                testSetCompulsorySupplierMandatedQuestionRequest.SupplierOrganisationId,
                testSetCompulsorySupplierMandatedQuestionRequest.QuestionId))
            .ReturnsAsync(() => CreateTestServiceOperationResult(success: true));

        var testResponse = testItems.Fixture.Create<SetCompulsorySupplierMandatedQuestionResponse>();

        testItems.MockQuestionConfigurationResponseFactory.Setup(x =>
                x.CreateSetCompulsorySupplierMandatedQuestionResponse(testSetCompulsorySupplierMandatedQuestionRequest))
            .Returns(() => testResponse);

        var result = await testItems.QuestionConfigurationController.SetCompulsorySupplierMandatedQuestion(testSetCompulsorySupplierMandatedQuestionRequest);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<OkObjectResult>());

            var typedResult = result as OkObjectResult;
            Assert.That(typedResult!.StatusCode, Is.EqualTo(200));
            Assert.That(typedResult.Value, Is.EqualTo(testResponse));
        });
    }

    [Test]
    public async Task GivenTheQuestionConfigurationServiceWillReturnFailure_WhenISetCompulsorySupplierMandatedQuestion_ThenABadRequestIsReturnedReportingTheError()
    {
        var testItems = CreateTestItems();

        var testSetCompulsorySupplierMandatedQuestionRequest = testItems.Fixture.Create<SetCompulsorySupplierMandatedQuestionRequest>();

        testItems.MockQuestionConfigurationService.Setup(x => x.SetCompulsorySupplierMandatedQuestionAsync(
                testSetCompulsorySupplierMandatedQuestionRequest.RequestingUserId,
                testSetCompulsorySupplierMandatedQuestionRequest.SupplierOrganisationId,
                testSetCompulsorySupplierMandatedQuestionRequest.QuestionId))
            .ReturnsAsync(() => CreateTestServiceOperationResult(success: false, error: "test error message"));

        var result = await testItems.QuestionConfigurationController.SetCompulsorySupplierMandatedQuestion(testSetCompulsorySupplierMandatedQuestionRequest);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());

            var typedResult = result as BadRequestObjectResult;

            Assert.That(typedResult!.StatusCode, Is.EqualTo(400));
            Assert.That(typedResult.Value, Is.EqualTo("test error message"));
        });
    }

    [Test]
    public async Task GivenTheQuestionConfigurationServiceWillReturnFailure_WhenISetCompulsorySupplierMandatedQuestion_ThenTheErrorIsLogged()
    {
        var testItems = CreateTestItems();

        var testSetCompulsorySupplierMandatedQuestionRequest = testItems.Fixture.Create<SetCompulsorySupplierMandatedQuestionRequest>();

        testItems.MockQuestionConfigurationService.Setup(x => x.SetCompulsorySupplierMandatedQuestionAsync(
                testSetCompulsorySupplierMandatedQuestionRequest.RequestingUserId,
                testSetCompulsorySupplierMandatedQuestionRequest.SupplierOrganisationId,
                testSetCompulsorySupplierMandatedQuestionRequest.QuestionId))
            .ReturnsAsync(() => CreateTestServiceOperationResult(success: false, error: "test error message"));

        await testItems.QuestionConfigurationController.SetCompulsorySupplierMandatedQuestion(testSetCompulsorySupplierMandatedQuestionRequest);

        testItems.MockLogger.VerifyLog(LogLevel.Error, "Failed to SetCompulsorySupplierMandatedQuestion from QuestionConfigurationService: test error message");
    }

    [Test]
    public async Task GivenTheQuestionConfigurationServiceWillThrowAnException_WhenISetCompulsorySupplierMandatedQuestion_ThenABadRequestIsReturnedReportingTheError()
    {
        var testItems = CreateTestItems();

        testItems.MockQuestionConfigurationService.Setup(x => x.SetCompulsorySupplierMandatedQuestionAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>()))
            .Throws(new Exception("test exception error message"));

        var result = await testItems.QuestionConfigurationController.SetCompulsorySupplierMandatedQuestion(testItems.Fixture.Create<SetCompulsorySupplierMandatedQuestionRequest>());

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());

            var typedResult = result as BadRequestObjectResult;

            Assert.That(typedResult!.StatusCode, Is.EqualTo(400));
            Assert.That(typedResult.Value, Is.EqualTo("test exception error message"));
        });
    }

    [Test]
    public async Task GivenTheQuestionConfigurationServiceWillThrowAnException_WhenISetCompulsorySupplierMandatedQuestion_ThenTheExceptionIsLogged()
    {
        var testItems = CreateTestItems();

        var testException = new Exception("test exception error message");
        testItems.MockQuestionConfigurationService.Setup(x => x.SetCompulsorySupplierMandatedQuestionAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>()))
            .Throws(testException);

        await testItems.QuestionConfigurationController.SetCompulsorySupplierMandatedQuestion(testItems.Fixture.Create<SetCompulsorySupplierMandatedQuestionRequest>());

        testItems.MockLogger.VerifyLog(LogLevel.Error, "test exception error message", testException);
    }
    #endregion

    #region ClearCompulsorySupplierMandatedQuestion() Tests
    [Test]
    public void GivenANullClearCompulsorySupplierMandatedQuestionRequest_WhenIClearCompulsorySupplierMandatedQuestion_ThenAnArgumentNullExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        Assert.That(() => testItems.QuestionConfigurationController.ClearCompulsorySupplierMandatedQuestion(null!),
            Throws.ArgumentNullException.With.Property("ParamName").EqualTo("clearCompulsorySupplierMandatedQuestionRequest"));
    }

    [Test]
    public async Task GivenAClearCompulsorySupplierMandatedQuestionRequest_WhenIClearCompulsorySupplierMandatedQuestion_ThenTheAnOkResultIsReturnedWithTheValueReportedByTheQuestionConfigurationService()
    {
        var testItems = CreateTestItems();

        var testClearCompulsorySupplierMandatedQuestionRequest = testItems.Fixture.Create<ClearCompulsorySupplierMandatedQuestionRequest>();

        testItems.MockQuestionConfigurationService.Setup(x => x.ClearCompulsorySupplierMandatedQuestionAsync(
                testClearCompulsorySupplierMandatedQuestionRequest.RequestingUserId,
                testClearCompulsorySupplierMandatedQuestionRequest.SupplierOrganisationId,
                testClearCompulsorySupplierMandatedQuestionRequest.QuestionId))
            .ReturnsAsync(() => CreateTestServiceOperationResult(success: true));

        var testResponse = testItems.Fixture.Create<ClearCompulsorySupplierMandatedQuestionResponse>();

        testItems.MockQuestionConfigurationResponseFactory.Setup(x =>
                x.CreateClearCompulsorySupplierMandatedQuestionResponse(testClearCompulsorySupplierMandatedQuestionRequest))
            .Returns(() => testResponse);

        var result = await testItems.QuestionConfigurationController.ClearCompulsorySupplierMandatedQuestion(testClearCompulsorySupplierMandatedQuestionRequest);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<OkObjectResult>());

            var typedResult = result as OkObjectResult;
            Assert.That(typedResult!.StatusCode, Is.EqualTo(200));
            Assert.That(typedResult.Value, Is.EqualTo(testResponse));
        });
    }

    [Test]
    public async Task GivenTheQuestionConfigurationServiceWillReturnFailure_WhenIClearCompulsorySupplierMandatedQuestion_ThenABadRequestIsReturnedReportingTheError()
    {
        var testItems = CreateTestItems();

        var testClearCompulsorySupplierMandatedQuestionRequest = testItems.Fixture.Create<ClearCompulsorySupplierMandatedQuestionRequest>();

        testItems.MockQuestionConfigurationService.Setup(x => x.ClearCompulsorySupplierMandatedQuestionAsync(
                testClearCompulsorySupplierMandatedQuestionRequest.RequestingUserId,
                testClearCompulsorySupplierMandatedQuestionRequest.SupplierOrganisationId,
                testClearCompulsorySupplierMandatedQuestionRequest.QuestionId))
            .ReturnsAsync(() => CreateTestServiceOperationResult(success: false, error: "test error message"));

        var result = await testItems.QuestionConfigurationController.ClearCompulsorySupplierMandatedQuestion(testClearCompulsorySupplierMandatedQuestionRequest);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());

            var typedResult = result as BadRequestObjectResult;

            Assert.That(typedResult!.StatusCode, Is.EqualTo(400));
            Assert.That(typedResult.Value, Is.EqualTo("test error message"));
        });
    }

    [Test]
    public async Task GivenTheQuestionConfigurationServiceWillReturnFailure_WhenIClearCompulsorySupplierMandatedQuestion_ThenTheErrorIsLogged()
    {
        var testItems = CreateTestItems();

        var testClearCompulsorySupplierMandatedQuestionRequest = testItems.Fixture.Create<ClearCompulsorySupplierMandatedQuestionRequest>();

        testItems.MockQuestionConfigurationService.Setup(x => x.ClearCompulsorySupplierMandatedQuestionAsync(
                testClearCompulsorySupplierMandatedQuestionRequest.RequestingUserId,
                testClearCompulsorySupplierMandatedQuestionRequest.SupplierOrganisationId,
                testClearCompulsorySupplierMandatedQuestionRequest.QuestionId))
            .ReturnsAsync(() => CreateTestServiceOperationResult(success: false, error: "test error message"));

        await testItems.QuestionConfigurationController.ClearCompulsorySupplierMandatedQuestion(testClearCompulsorySupplierMandatedQuestionRequest);

        testItems.MockLogger.VerifyLog(LogLevel.Error, "Failed to ClearCompulsorySupplierMandatedQuestion from QuestionConfigurationService: test error message");
    }

    [Test]
    public async Task GivenTheQuestionConfigurationServiceWillThrowAnException_WhenIClearCompulsorySupplierMandatedQuestion_ThenABadRequestIsReturnedReportingTheError()
    {
        var testItems = CreateTestItems();

        testItems.MockQuestionConfigurationService.Setup(x => x.ClearCompulsorySupplierMandatedQuestionAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>()))
            .Throws(new Exception("test exception error message"));

        var result = await testItems.QuestionConfigurationController.ClearCompulsorySupplierMandatedQuestion(testItems.Fixture.Create<ClearCompulsorySupplierMandatedQuestionRequest>());

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());

            var typedResult = result as BadRequestObjectResult;

            Assert.That(typedResult!.StatusCode, Is.EqualTo(400));
            Assert.That(typedResult.Value, Is.EqualTo("test exception error message"));
        });
    }

    [Test]
    public async Task GivenTheQuestionConfigurationServiceWillThrowAnException_WhenIClearCompulsorySupplierMandatedQuestion_ThenTheExceptionIsLogged()
    {
        var testItems = CreateTestItems();

        var testException = new Exception("test exception error message");
        testItems.MockQuestionConfigurationService.Setup(x => x.ClearCompulsorySupplierMandatedQuestionAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>()))
            .Throws(testException);

        await testItems.QuestionConfigurationController.ClearCompulsorySupplierMandatedQuestion(testItems.Fixture.Create<ClearCompulsorySupplierMandatedQuestionRequest>());

        testItems.MockLogger.VerifyLog(LogLevel.Error, "test exception error message", testException);
    }
    #endregion
    #endregion

    #region Test Data Creation
    private static IServiceOperationResult CreateTestServiceOperationResult(
        bool? success = null,
        string? error = null)
    {
        var mockServiceOperationResult = new Mock<IServiceOperationResult>();

        mockServiceOperationResult.SetupGet(x => x.Success).Returns(success ?? true);
        mockServiceOperationResult.SetupGet(x => x.Error).Returns(error);

        return mockServiceOperationResult.Object;
    }

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
    #endregion

    #region Test Item Creation
    private static TestItems CreateTestItems()
    {
        var fixture = new Fixture().Customize(new AutoMoqCustomization());

        var mockLogger = Mock.Get(fixture.Freeze<ILogger<QuestionConfigurationController>>());
        var mockQuestionConfigurationService = Mock.Get(fixture.Freeze<IQuestionConfigurationService>());
        var mockQuestionConfigurationResponseFactory = Mock.Get(fixture.Freeze<IQuestionConfigurationResponseFactory>());

        var questionConfigurationController = new QuestionConfigurationController(
            mockLogger.Object,
            mockQuestionConfigurationService.Object,
            mockQuestionConfigurationResponseFactory.Object);

        return new TestItems(fixture, questionConfigurationController,
            mockLogger, mockQuestionConfigurationService, mockQuestionConfigurationResponseFactory);
    }

    private class TestItems(
        IFixture fixture,
        QuestionConfigurationController questionConfigurationController,
        Mock<ILogger<QuestionConfigurationController>> mockLogger,
        Mock<IQuestionConfigurationService> mockQuestionConfigurationService,
        Mock<IQuestionConfigurationResponseFactory> mockQuestionConfigurationResponseFactory)
    {
        public IFixture Fixture { get; } = fixture;
        public QuestionConfigurationController QuestionConfigurationController { get; } = questionConfigurationController;
        public Mock<ILogger<QuestionConfigurationController>> MockLogger { get; } = mockLogger;
        public Mock<IQuestionConfigurationService> MockQuestionConfigurationService { get; } = mockQuestionConfigurationService;
        public Mock<IQuestionConfigurationResponseFactory> MockQuestionConfigurationResponseFactory { get; } = mockQuestionConfigurationResponseFactory;
    }
    #endregion
}