using System.Net;
using AutoFixture.AutoMoq;
using AutoFixture;
using Moq;
using NUnit.Framework;
using Agrimetrics.DataShare.Api.Logic.Services.QuestionConfiguration;
using Microsoft.Extensions.Logging;
using Agrimetrics.DataShare.Api.Logic.Repositories.QuestionConfiguration;
using Agrimetrics.DataShare.Api.Logic.ModelData.QuestionConfiguration;
using Agrimetrics.DataShare.Api.Logic.Services.ServiceOperationResults;
using Agrimetrics.DataShare.Api.Logic.ModelData.QuestionConfiguration.CompulsoryQuestions;
using Agrimetrics.DataShare.Api.Dto.Models.QuestionConfiguration.CompulsoryQuestions;

namespace Agrimetrics.DataShare.Api.Logic.Test.Services.QuestionConfiguration;

[TestFixture]
public class QuestionConfigurationServiceTests
{
    #region GetCompulsoryQuestionsAsync() Tests
    [Test]
    public async Task GivenAQuestionConfigurationService_WhenIGetCompulsoryQuestionsAsync_ThenACreateCompulsoryQuestionSetIsCreatedForTheCompulsoryQuestionsInTheRepository()
    {
        var testItems = CreateTestItems();

        var testCompulsoryQuestionModelDatas = testItems.Fixture.CreateMany<CompulsoryQuestionModelData>().ToList();
        
        testItems.MockQuestionConfigurationRepository
            .Setup(x => x.GetCompulsoryQuestionsAsync())
            .ReturnsAsync(() => testCompulsoryQuestionModelDatas);

        var testCompulsoryQuestionSet = testItems.Fixture.Create<CompulsoryQuestionSet>();

        testItems.MockQuestionConfigurationModelDataFactory
            .Setup(x => x.CreateCompulsoryQuestionSet(testCompulsoryQuestionModelDatas))
            .Returns(() => testCompulsoryQuestionSet);

        testItems.MockServiceOperationResultFactory
            .Setup(x => x.CreateSuccessfulDataResult(It.IsAny<CompulsoryQuestionSet>(), It.IsAny<HttpStatusCode?>()))
            .Returns((CompulsoryQuestionSet compulsoryQuestionSet, HttpStatusCode? _) =>
            {
                var mockServiceOperationDataResult = new Mock<IServiceOperationDataResult<CompulsoryQuestionSet>>();
                
                mockServiceOperationDataResult.SetupGet(x => x.Data)
                    .Returns(compulsoryQuestionSet);

                return mockServiceOperationDataResult.Object;
            });

        var result = await testItems.QuestionConfigurationService.GetCompulsoryQuestionsAsync(It.IsAny<int>());

        Assert.That(result.Data!, Is.SameAs(testCompulsoryQuestionSet));
    }

    [Test]
    public async Task GivenGettingCompulsoryQuestionsWillThrowAnException_WhenIGetCompulsoryQuestionsAsync_ThenAFailedResultIsReturned()
    {
        var testItems = CreateTestItems();

        var testException = new Exception("Oh noes!");

        testItems.MockQuestionConfigurationRepository
            .Setup(x => x.GetCompulsoryQuestionsAsync())
            .ThrowsAsync(testException);

        testItems.MockServiceOperationResultFactory
            .Setup(x => x.CreateFailedDataResult<CompulsoryQuestionSet>(It.IsAny<string>(), It.IsAny<HttpStatusCode?>()))
            .Returns((string error, HttpStatusCode? _) =>
            {
                var mockServiceOperationDataResult = new Mock<IServiceOperationDataResult<CompulsoryQuestionSet>>();

                mockServiceOperationDataResult.SetupGet(x => x.Error)
                    .Returns(error);

                return mockServiceOperationDataResult.Object;
            });

        var result = await testItems.QuestionConfigurationService.GetCompulsoryQuestionsAsync(It.IsAny<int>());

        Assert.That(result.Error!, Is.EqualTo("Oh noes!"));
    }
    #endregion

    #region SetCompulsoryQuestionAsync() Tests
    [Test]
    public async Task GivenAQuestionId_WhenISetCompulsoryQuestionAsync_ThenTheCompulsoryQuestionIsSetInTheRepository()
    {
        var testItems = CreateTestItems();

        var testQuestionId = testItems.Fixture.Create<Guid>();

        var testSuccessfulResult = Mock.Of<IServiceOperationResult>();

        testItems.MockServiceOperationResultFactory.Setup(x => x.CreateSuccessfulResult(It.IsAny<HttpStatusCode?>()))
            .Returns(testSuccessfulResult);

        var result = await testItems.QuestionConfigurationService.SetCompulsoryQuestionAsync(
            It.IsAny<int>(),
            testQuestionId);
        
        Assert.Multiple(() =>
        {
            testItems.MockQuestionConfigurationRepository.Verify(x =>
                    x.SetCompulsoryQuestionAsync(testQuestionId),
                Times.Once);

            Assert.That(result, Is.SameAs(testSuccessfulResult));
        });
    }

    [Test]
    public async Task GivenSettingACompulsoryQuestionWillThrowAnException_WhenISetCompulsoryQuestionAsync_ThenAFailedResultIsReturned()
    {
        var testItems = CreateTestItems();

        var testQuestionId = testItems.Fixture.Create<Guid>();

        var testException = new Exception("Oh noes!");

        testItems.MockQuestionConfigurationRepository
            .Setup(x => x.SetCompulsoryQuestionAsync(testQuestionId))
            .ThrowsAsync(testException);

        testItems.MockServiceOperationResultFactory
            .Setup(x => x.CreateFailedResult(It.IsAny<string>(), It.IsAny<HttpStatusCode?>()))
            .Returns((string error, HttpStatusCode? _) =>
            {
                var mockServiceOperationDataResult = new Mock<IServiceOperationResult>();

                mockServiceOperationDataResult.SetupGet(x => x.Error)
                    .Returns(error);

                return mockServiceOperationDataResult.Object;
            });

        var result = await testItems.QuestionConfigurationService.SetCompulsoryQuestionAsync(It.IsAny<int>(), testQuestionId);

        Assert.That(result.Error!, Is.EqualTo("Oh noes!"));
    }
    #endregion

    #region ClearCompulsoryQuestionAsync() Tests
    [Test]
    public async Task GivenAQuestionId_WhenIClearCompulsoryQuestionAsync_ThenTheCompulsoryQuestionIsClearedInTheRepository()
    {
        var testItems = CreateTestItems();

        var testQuestionId = testItems.Fixture.Create<Guid>();

        var testSuccessfulResult = Mock.Of<IServiceOperationResult>();

        testItems.MockServiceOperationResultFactory.Setup(x => x.CreateSuccessfulResult(It.IsAny<HttpStatusCode?>()))
            .Returns(testSuccessfulResult);

        var result = await testItems.QuestionConfigurationService.ClearCompulsoryQuestionAsync(
            It.IsAny<int>(),
            testQuestionId);

        Assert.Multiple(() =>
        {
            testItems.MockQuestionConfigurationRepository.Verify(x =>
                    x.ClearCompulsoryQuestionAsync(testQuestionId),
                Times.Once);

            Assert.That(result, Is.SameAs(testSuccessfulResult));
        });
    }

    [Test]
    public async Task GivenSettingACompulsoryQuestionWillThrowAnException_WhenIClearCompulsoryQuestionAsync_ThenAFailedResultIsReturned()
    {
        var testItems = CreateTestItems();

        var testQuestionId = testItems.Fixture.Create<Guid>();

        var testException = new Exception("Oh noes!");

        testItems.MockQuestionConfigurationRepository
            .Setup(x => x.ClearCompulsoryQuestionAsync(testQuestionId))
            .ThrowsAsync(testException);

        testItems.MockServiceOperationResultFactory
            .Setup(x => x.CreateFailedResult(It.IsAny<string>(), It.IsAny<HttpStatusCode?>()))
            .Returns((string error, HttpStatusCode? _) =>
            {
                var mockServiceOperationDataResult = new Mock<IServiceOperationResult>();

                mockServiceOperationDataResult.SetupGet(x => x.Error)
                    .Returns(error);

                return mockServiceOperationDataResult.Object;
            });

        var result = await testItems.QuestionConfigurationService.ClearCompulsoryQuestionAsync(It.IsAny<int>(), testQuestionId);

        Assert.That(result.Error!, Is.EqualTo("Oh noes!"));
    }
    #endregion

    #region GetCompulsorySupplierMandatedQuestionsAsync() Tests
    [Test]
    public async Task GivenAQuestionConfigurationService_WhenIGetCompulsorySupplierMandatedQuestionsAsync_ThenACreateCompulsorySupplierMandatedQuestionSetIsCreatedForTheCompulsorySupplierMandatedQuestionsInTheRepository()
    {
        var testItems = CreateTestItems();

        var testSupplierOrganisationId = testItems.Fixture.Create<int>();

        var testCompulsorySupplierMandatedQuestionModelDatas = testItems.Fixture.CreateMany<CompulsorySupplierMandatedQuestionModelData>().ToList();

        testItems.MockQuestionConfigurationRepository
            .Setup(x => x.GetCompulsorySupplierMandatedQuestionsAsync(testSupplierOrganisationId))
            .ReturnsAsync(() => testCompulsorySupplierMandatedQuestionModelDatas);

        var testCompulsorySupplierMandatedQuestionSet = testItems.Fixture.Create<CompulsorySupplierMandatedQuestionSet>();

        testItems.MockQuestionConfigurationModelDataFactory
            .Setup(x => x.CreateCompulsorySupplierMandatedQuestionSet(testCompulsorySupplierMandatedQuestionModelDatas))
            .Returns(() => testCompulsorySupplierMandatedQuestionSet);

        testItems.MockServiceOperationResultFactory
            .Setup(x => x.CreateSuccessfulDataResult(It.IsAny<CompulsorySupplierMandatedQuestionSet>(), It.IsAny<HttpStatusCode?>()))
            .Returns((CompulsorySupplierMandatedQuestionSet compulsoryQuestionSet, HttpStatusCode? _) =>
            {
                var mockServiceOperationDataResult = new Mock<IServiceOperationDataResult<CompulsorySupplierMandatedQuestionSet>>();

                mockServiceOperationDataResult.SetupGet(x => x.Data)
                    .Returns(compulsoryQuestionSet);

                return mockServiceOperationDataResult.Object;
            });

        var result = await testItems.QuestionConfigurationService.GetCompulsorySupplierMandatedQuestionsAsync(It.IsAny<int>(), testSupplierOrganisationId);

        Assert.That(result.Data!, Is.SameAs(testCompulsorySupplierMandatedQuestionSet));
    }

    [Test]
    public async Task GivenGettingCompulsorySupplierMandatedQuestionsWillThrowAnException_WhenIGetCompulsorySupplierMandatedQuestionsAsync_ThenAFailedResultIsReturned()
    {
        var testItems = CreateTestItems();

        var testSupplierOrganisationId = testItems.Fixture.Create<int>();

        var testException = new Exception("Oh noes!");

        testItems.MockQuestionConfigurationRepository
            .Setup(x => x.GetCompulsorySupplierMandatedQuestionsAsync(testSupplierOrganisationId))
            .ThrowsAsync(testException);

        testItems.MockServiceOperationResultFactory
            .Setup(x => x.CreateFailedDataResult<CompulsorySupplierMandatedQuestionSet>(It.IsAny<string>(), It.IsAny<HttpStatusCode?>()))
            .Returns((string error, HttpStatusCode? _) =>
            {
                var mockServiceOperationDataResult = new Mock<IServiceOperationDataResult<CompulsorySupplierMandatedQuestionSet>>();

                mockServiceOperationDataResult.SetupGet(x => x.Error)
                    .Returns(error);

                return mockServiceOperationDataResult.Object;
            });

        var result = await testItems.QuestionConfigurationService.GetCompulsorySupplierMandatedQuestionsAsync(It.IsAny<int>(), testSupplierOrganisationId);

        Assert.That(result.Error!, Is.EqualTo("Oh noes!"));
    }
    #endregion

    #region SetCompulsorySupplierMandatedQuestionAsync() Tests
    [Test]
    public async Task GivenAQuestionId_WhenISetCompulsorySupplierMandatedQuestionAsync_ThenTheCompulsorySupplierMandatedQuestionIsSetInTheRepository()
    {
        var testItems = CreateTestItems();

        var testSupplierOrganisationId = testItems.Fixture.Create<int>();

        var testQuestionId = testItems.Fixture.Create<Guid>();

        var testSuccessfulResult = Mock.Of<IServiceOperationResult>();

        testItems.MockServiceOperationResultFactory.Setup(x => x.CreateSuccessfulResult(It.IsAny<HttpStatusCode?>()))
            .Returns(testSuccessfulResult);

        var result = await testItems.QuestionConfigurationService.SetCompulsorySupplierMandatedQuestionAsync(
            It.IsAny<int>(),
            testSupplierOrganisationId,
            testQuestionId);

        Assert.Multiple(() =>
        {
            testItems.MockQuestionConfigurationRepository.Verify(x =>
                    x.SetCompulsorySupplierMandatedQuestionAsync(testSupplierOrganisationId, testQuestionId),
                Times.Once);

            Assert.That(result, Is.SameAs(testSuccessfulResult));
        });
    }

    [Test]
    public async Task GivenSettingACompulsorySupplierMandatedQuestionWillThrowAnException_WhenISetCompulsorySupplierMandatedQuestionAsync_ThenAFailedResultIsReturned()
    {
        var testItems = CreateTestItems();

        var testSupplierOrganisationId = testItems.Fixture.Create<int>();
        var testQuestionId = testItems.Fixture.Create<Guid>();

        var testException = new Exception("Oh noes!");

        testItems.MockQuestionConfigurationRepository
            .Setup(x => x.SetCompulsorySupplierMandatedQuestionAsync(testSupplierOrganisationId, testQuestionId))
            .ThrowsAsync(testException);

        testItems.MockServiceOperationResultFactory
            .Setup(x => x.CreateFailedResult(It.IsAny<string>(), It.IsAny<HttpStatusCode?>()))
            .Returns((string error, HttpStatusCode? _) =>
            {
                var mockServiceOperationDataResult = new Mock<IServiceOperationResult>();

                mockServiceOperationDataResult.SetupGet(x => x.Error)
                    .Returns(error);

                return mockServiceOperationDataResult.Object;
            });

        var result = await testItems.QuestionConfigurationService.SetCompulsorySupplierMandatedQuestionAsync(It.IsAny<int>(), testSupplierOrganisationId, testQuestionId);

        Assert.That(result.Error!, Is.EqualTo("Oh noes!"));
    }
    #endregion

    #region ClearCompulsorySupplierMandatedQuestionAsync() Tests
    [Test]
    public async Task GivenAQuestionId_WhenIClearCompulsorySupplierMandatedQuestionAsync_ThenTheCompulsorySupplierMandatedQuestionIsClearedInTheRepository()
    {
        var testItems = CreateTestItems();

        var testSupplierOrganisationId = testItems.Fixture.Create<int>();
        var testQuestionId = testItems.Fixture.Create<Guid>();

        var testSuccessfulResult = Mock.Of<IServiceOperationResult>();

        testItems.MockServiceOperationResultFactory.Setup(x => x.CreateSuccessfulResult(It.IsAny<HttpStatusCode?>()))
            .Returns(testSuccessfulResult);

        var result = await testItems.QuestionConfigurationService.ClearCompulsorySupplierMandatedQuestionAsync(
            It.IsAny<int>(),
            testSupplierOrganisationId,
            testQuestionId);

        Assert.Multiple(() =>
        {
            testItems.MockQuestionConfigurationRepository.Verify(x =>
                    x.ClearCompulsorySupplierMandatedQuestionAsync(testSupplierOrganisationId, testQuestionId),
                Times.Once);

            Assert.That(result, Is.SameAs(testSuccessfulResult));
        });
    }

    [Test]
    public async Task GivenSettingACompulsorySupplierMandatedQuestionWillThrowAnException_WhenIClearCompulsorySupplierMandatedQuestionAsync_ThenAFailedResultIsReturned()
    {
        var testItems = CreateTestItems();

        var testSupplierOrganisationId = testItems.Fixture.Create<int>();
        var testQuestionId = testItems.Fixture.Create<Guid>();

        var testException = new Exception("Oh noes!");

        testItems.MockQuestionConfigurationRepository
            .Setup(x => x.ClearCompulsorySupplierMandatedQuestionAsync(testSupplierOrganisationId, testQuestionId))
            .ThrowsAsync(testException);

        testItems.MockServiceOperationResultFactory
            .Setup(x => x.CreateFailedResult(It.IsAny<string>(), It.IsAny<HttpStatusCode?>()))
            .Returns((string error, HttpStatusCode? _) =>
            {
                var mockServiceOperationDataResult = new Mock<IServiceOperationResult>();

                mockServiceOperationDataResult.SetupGet(x => x.Error)
                    .Returns(error);

                return mockServiceOperationDataResult.Object;
            });

        var result = await testItems.QuestionConfigurationService.ClearCompulsorySupplierMandatedQuestionAsync(It.IsAny<int>(), testSupplierOrganisationId, testQuestionId);

        Assert.That(result.Error!, Is.EqualTo("Oh noes!"));
    }
    #endregion

    #region Test Item Creation
    private static TestItems CreateTestItems()
    {
        var fixture = new Fixture().Customize(new AutoMoqCustomization());

        var mockLogger = Mock.Get(fixture.Freeze<ILogger<QuestionConfigurationService>>());
        var mockQuestionConfigurationRepository = Mock.Get(fixture.Freeze<IQuestionConfigurationRepository>());
        var mockQuestionConfigurationModelDataFactory = Mock.Get(fixture.Freeze<IQuestionConfigurationModelDataFactory>());
        var mockServiceOperationResultFactory = Mock.Get(fixture.Freeze<IServiceOperationResultFactory>());

        var questionConfigurationService = new QuestionConfigurationService(
            mockLogger.Object,
            mockQuestionConfigurationRepository.Object,
            mockQuestionConfigurationModelDataFactory.Object,
            mockServiceOperationResultFactory.Object);

        return new TestItems(
            fixture,
            questionConfigurationService,
            mockQuestionConfigurationRepository,
            mockQuestionConfigurationModelDataFactory,
            mockServiceOperationResultFactory);
    }

    private class TestItems(
        IFixture fixture,
        IQuestionConfigurationService questionConfigurationService,
        Mock<IQuestionConfigurationRepository> mockQuestionConfigurationRepository,
        Mock<IQuestionConfigurationModelDataFactory> mockQuestionConfigurationModelDataFactory,
        Mock<IServiceOperationResultFactory> mockServiceOperationResultFactory)
    {
        public IFixture Fixture { get; } = fixture;
        public IQuestionConfigurationService QuestionConfigurationService { get; } = questionConfigurationService;
        public Mock<IQuestionConfigurationRepository> MockQuestionConfigurationRepository { get; } = mockQuestionConfigurationRepository;
        public Mock<IQuestionConfigurationModelDataFactory> MockQuestionConfigurationModelDataFactory { get; } = mockQuestionConfigurationModelDataFactory;
        public Mock<IServiceOperationResultFactory> MockServiceOperationResultFactory { get; } = mockServiceOperationResultFactory;
    }
    #endregion
}
