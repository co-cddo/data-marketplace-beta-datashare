using System.Diagnostics.CodeAnalysis;
using System.Net;
using Agrimetrics.DataShare.Api.Dto.Models.Acquirer.DataShareRequests;
using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests;
using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests.Answers.DataShareRequestAnswerSummaries;
using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests.Answers.DataShareRequestQuestionAnswers;
using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests.Questions;
using Agrimetrics.DataShare.Api.Dto.Models.Questions.QuestionSets;
using Agrimetrics.DataShare.Api.Logic.ModelData;
using Agrimetrics.DataShare.Api.Logic.ModelData.AnswerValidation;
using Agrimetrics.DataShare.Api.Logic.ModelData.AuditLogs;
using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests;
using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests.Answers.DataShareRequestAnswerSummaries;
using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests.Answers.DataShareRequestQuestionAnswerResponses;
using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests.QuestionStatusDeterminations;
using Agrimetrics.DataShare.Api.Logic.ModelData.Questions;
using Agrimetrics.DataShare.Api.Logic.ModelData.Questions.QuestionSets;
using Agrimetrics.DataShare.Api.Logic.Repositories.AcquirerDataShareRequests;
using Agrimetrics.DataShare.Api.Logic.Services.AcquirerDataShareRequest;
using Agrimetrics.DataShare.Api.Logic.Services.AcquirerDataShareRequest.DataShareRequestNotificationRecipientDeterminations;
using Agrimetrics.DataShare.Api.Logic.Services.AcquirerDataShareRequest.DataShareRequestQuestionStatusesDeterminations;
using Agrimetrics.DataShare.Api.Logic.Services.AcquirerDataShareRequest.NextQuestionDeterminations;
using Agrimetrics.DataShare.Api.Logic.Services.AnswerValidation;
using Agrimetrics.DataShare.Api.Logic.Services.AuditLog;
using Agrimetrics.DataShare.Api.Logic.Services.Esdas;
using Agrimetrics.DataShare.Api.Logic.Services.Esdas.Model;
using Agrimetrics.DataShare.Api.Logic.Services.KeyQuestionPartAnswers;
using Agrimetrics.DataShare.Api.Logic.Services.Notification;
using Agrimetrics.DataShare.Api.Logic.Services.QuestionSetPlaceHolderReplacement;
using Agrimetrics.DataShare.Api.Logic.Services.ServiceOperationResults;
using Agrimetrics.DataShare.Api.Logic.Services.Users;
using Agrimetrics.DataShare.Api.Logic.Services.Users.Model;
using Agrimetrics.DataShare.Api.Logic.Test.TestHelpers;
using AutoFixture;
using AutoFixture.AutoMoq;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.Services.AcquirerDataShareRequest
{
    [TestFixture]
    public class AcquirerDataShareRequestServiceTests
    {
        #region GetEsdaQuestionSetOutlineRequestAsync() Tests
        [Test]
        public async Task GivenTheRequestCompletesSuccessfully_WhenIGetEsdaQuestionSetOutlineRequestAsync_ThenASuccessfulDataResultIsReturnedReportingTheRequestedQuestionSetOutline()
        {
            var testItems = CreateTestItems();

            var testSupplierDomainId = testItems.Fixture.Create<int>();
            var testSupplierOrganisationId = testItems.Fixture.Create<int>();
            var esdaId = testItems.Fixture.Create<Guid>();

            var testQuestionSetId = testItems.Fixture.Create<Guid>();

            testItems.MockAcquirerDataShareRequestRepository.Setup(x => x.FindQuestionSetAsync(
                    testSupplierDomainId,
                    testSupplierOrganisationId,
                    esdaId))
                .ReturnsAsync(() => testQuestionSetId);

            var testQuestionSetOutlineModelData = testItems.Fixture.Create<QuestionSetOutlineModelData>();

            testItems.MockAcquirerDataShareRequestRepository.Setup(x => x.GetQuestionSetOutlineRequestAsync(
                    testQuestionSetId))
                .ReturnsAsync(() => testQuestionSetOutlineModelData);

            var testQuestionSetOutline = testItems.Fixture.Create<QuestionSetOutline>();

            testItems.MockAcquirerDataShareRequestModelDataFactory.Setup(x => x.CreateQuestionSetOutline(
                    testQuestionSetOutlineModelData))
                .Returns(() => testQuestionSetOutline);

            var testSuccessfulDataResult = Mock.Of<IServiceOperationDataResult<QuestionSetOutline>>();
            testItems.MockServiceOperationResultFactory.Setup(x => x.CreateSuccessfulDataResult(testQuestionSetOutline, It.IsAny<HttpStatusCode?>()))
                .Returns(() => testSuccessfulDataResult);

            var result = await testItems.AcquirerDataShareRequestService.GetEsdaQuestionSetOutlineRequestAsync(
                testSupplierDomainId,
                testSupplierOrganisationId,
                esdaId);

            Assert.That(result, Is.SameAs(testSuccessfulDataResult));
        }

        [Test]
        public async Task GivenAnExceptionIsThrownWhilstPerformingTheRequest_WhenIGetEsdaQuestionSetOutlineRequestAsync_ThenAnErrorIsLogged()
        {
            var testItems = CreateTestItems();

            var testException = new Exception("test exception message");

            testItems.MockAcquirerDataShareRequestRepository.Setup(x => x.FindQuestionSetAsync(
                It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>()))
                .ThrowsAsync(testException);

            await testItems.AcquirerDataShareRequestService.GetEsdaQuestionSetOutlineRequestAsync(
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<Guid>());

            testItems.MockLogger.VerifyLog(LogLevel.Error, "Failed to GetEsdaQuestionSetOutlineRequest", testException);
        }

        [Test]
        public async Task GivenAnExceptionIsThrownWhilstPerformingTheRequest_WhenIGetEsdaQuestionSetOutlineRequestAsync_ThenAFailedDataResultIsReturnedReportingTheError()
        {
            var testItems = CreateTestItems();

            testItems.MockAcquirerDataShareRequestRepository.Setup(x => x.FindQuestionSetAsync(
                    It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>()))
                .ThrowsAsync(new Exception());

            var testFailedDataResult = Mock.Of<IServiceOperationDataResult<QuestionSetOutline>>();
            testItems.MockServiceOperationResultFactory.Setup(x => x.CreateFailedDataResult<QuestionSetOutline>(It.IsAny<string>(), It.IsAny<HttpStatusCode?>()))
                .Returns(() => testFailedDataResult);

            var result = await testItems.AcquirerDataShareRequestService.GetEsdaQuestionSetOutlineRequestAsync(
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<Guid>());

            Assert.That(result, Is.SameAs(testFailedDataResult));
        }
        #endregion

        #region StartDataShareRequestAsync() Tests
        [Test]
        public async Task GivenARequestingUserIdSet_WhenIStartDataShareRequestAsync_ThenTheRequestToTheRepositoryIsMadeUsingTheGivenRequestingUserIdSet()
        {
            var testItems = CreateTestItems();

            var testUserIdSet = CreateTestUserIdSet(userId: 100, domainId: 200, organisationId: 300);
            testItems.MockUserProfilePresenter.Setup(x => x.GetInitiatingUserIdSetAsync())
                .ReturnsAsync(() => testUserIdSet);

            await testItems.AcquirerDataShareRequestService.StartDataShareRequestAsync(
                It.IsAny<Guid>());

            testItems.MockAcquirerDataShareRequestRepository.Verify(x => x.StartDataShareRequestAsync(
                    testUserIdSet,
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<Guid>(),
                    It.IsAny<string>(),
                    It.IsAny<Guid>()),
                Times.Once);
        }

        [Test]
        public async Task GivenAnEsdaId_WhenIStartDataShareRequestAsync_ThenTheRequestToTheRepositoryIsMadeUsingTheDetailsFromTheEsdaWithThatId()
        {
            var testItems = CreateTestItems();

            var testEsdaId = Guid.Parse("8336F592-FFA0-41A0-AE0F-B97C7B2A041C");
            const string testEsdaName = "test esda name";
            const int testSupplierOrganisationId = 123;
            const int testSupplierDomainId = 456;

            testItems.MockEsdaInformationPresenter.Setup(x => x.GetEsdaDetailsByIdAsync(testEsdaId))
                .ReturnsAsync((Guid esdaId) =>
                    CreateTestEsdaDetails(
                        esdaId: esdaId,
                        esdaTitle: testEsdaName,
                        supplierOrganisationId: testSupplierOrganisationId,
                        supplierDomainId: testSupplierDomainId));

            await testItems.AcquirerDataShareRequestService.StartDataShareRequestAsync(
                testEsdaId);

            testItems.MockAcquirerDataShareRequestRepository.Verify(x => x.StartDataShareRequestAsync(
                    It.IsAny<IUserIdSet>(),
                    testSupplierDomainId,
                    testSupplierOrganisationId,
                    testEsdaId,
                    testEsdaName,
                    It.IsAny<Guid>()),
                Times.Once);
        }

        [Test]
        public async Task GivenAnEsdaId_WhenIStartDataShareRequestAsync_ThenTheRequestToTheRepositoryIsMadeUsingTheQuestionSetForThatEsdaAndItsOrganisationDetails()
        {
            var testItems = CreateTestItems();

            var testEsdaId = Guid.Parse("8336F592-FFA0-41A0-AE0F-B97C7B2A041C");
            const int testSupplierOrganisationId = 123;
            const int testSupplierDomainId = 456;

            var questionSetId = Guid.Parse("90E3A3F3-CFFE-4187-ABDB-093AAF96FBD0");

            testItems.MockAcquirerDataShareRequestRepository.Setup(x => x.FindQuestionSetAsync(
                    testSupplierDomainId,
                    testSupplierOrganisationId,
                    testEsdaId))
                .ReturnsAsync(() => questionSetId);

            testItems.MockEsdaInformationPresenter.Setup(x => x.GetEsdaDetailsByIdAsync(testEsdaId))
                .ReturnsAsync(() =>
                    CreateTestEsdaDetails(
                        supplierOrganisationId: testSupplierOrganisationId,
                        supplierDomainId: testSupplierDomainId));

            await testItems.AcquirerDataShareRequestService.StartDataShareRequestAsync(
                testEsdaId);

            testItems.MockAcquirerDataShareRequestRepository.Verify(x => x.StartDataShareRequestAsync(
                    It.IsAny<IUserIdSet>(),
                    testSupplierDomainId,
                    testSupplierOrganisationId,
                    testEsdaId,
                    It.IsAny<string>(),
                    questionSetId),
                Times.Once);
        }

        [Test]
        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration", Justification = "Multiple enumeration in mock verification")]
        public async Task GivenADataShareRequestIsStartedSuccessfully_WhenIStartDataShareRequestAsync_ThenTheQuestionStatusesForTheQuestionsThatHaveAnUpdatedStatusInTheDataShareRequestAreUpdated()
        {
            var testItems = CreateTestItems();

            var dataShareRequestId = Guid.Parse("6B9F557E-2F05-41EF-98CC-43C2C3B0A9DD");

            testItems.MockAcquirerDataShareRequestRepository.Setup(x => x.StartDataShareRequestAsync(
                    It.IsAny<IUserIdSet>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<Guid>()))
                .ReturnsAsync(() => dataShareRequestId);

            var testDataShareRequestQuestionStatusInformationSetModelData = new DataShareRequestQuestionStatusInformationSetModelData();
            testItems.MockAcquirerDataShareRequestRepository.Setup(x => x.GetDataShareRequestQuestionStatusInformationsAsync(dataShareRequestId))
                .ReturnsAsync(() => testDataShareRequestQuestionStatusInformationSetModelData);

            var mockDataShareRequestQuestionStatusesDeterminationResult = CreateTestDataShareRequestQuestionStatusesDeterminationResult(
                dataShareRequestQuestionStatusDeterminationResults:
                [
                    CreateTestDataShareRequestQuestionStatusDeterminationResult(Guid.Parse("B25BF52B-7D8F-405C-8FE8-7BFFCB703B16"), QuestionStatusType.CannotStartYet, QuestionStatusType.NotSet),
                    CreateTestDataShareRequestQuestionStatusDeterminationResult(Guid.Parse("54878B2E-1649-4245-B3DE-8ACBB68EAC9D"), QuestionStatusType.NoResponseNeeded, QuestionStatusType.NotSet),
                    CreateTestDataShareRequestQuestionStatusDeterminationResult(Guid.Parse("45AACDD1-664A-42E0-BAFB-DF2231E591AA"), QuestionStatusType.Completed, QuestionStatusType.Completed)
                ]);

            testItems.MockDataShareRequestQuestionStatusesDetermination.Setup(x => x.DetermineQuestionStatuses(testDataShareRequestQuestionStatusInformationSetModelData))
                .Returns(mockDataShareRequestQuestionStatusesDeterminationResult);

            await testItems.AcquirerDataShareRequestService.StartDataShareRequestAsync(
                It.IsAny<Guid>());

            Assert.Multiple(() =>
            {
                testItems.MockAcquirerDataShareRequestRepository.Verify(x =>
                        x.UpdateDataShareRequestQuestionStatusesAsync(It.IsAny<Guid>(), It.IsAny<bool>(), It.IsAny<IEnumerable<IDataShareRequestQuestionStatusDataModel>>()),
                    Times.Once);

                testItems.MockAcquirerDataShareRequestRepository.Verify(x =>
                        x.UpdateDataShareRequestQuestionStatusesAsync(
                            dataShareRequestId,
                            It.IsAny<bool>(),
                            It.Is<IEnumerable<IDataShareRequestQuestionStatusDataModel>>(questionStatuses =>
                                questionStatuses.Count() == 2 &&
                                questionStatuses.Any(questionStatus => questionStatus.QuestionId == Guid.Parse("B25BF52B-7D8F-405C-8FE8-7BFFCB703B16") && questionStatus.QuestionStatus == QuestionStatusType.CannotStartYet) && 
                                questionStatuses.Any(questionStatus => questionStatus.QuestionId == Guid.Parse("54878B2E-1649-4245-B3DE-8ACBB68EAC9D") && questionStatus.QuestionStatus == QuestionStatusType.NoResponseNeeded)
                            )),
                    Times.Once);
            });
        }

        [Theory]
        public async Task GivenADataShareRequestIsStartedSuccessfully_WhenIStartDataShareRequestAsync_ThenTheQuestionsRemainThatRequireAResponseIsSetForTheDataShareRequest(
            bool questionsRemainThatRequireAResponse)
        {
            var testItems = CreateTestItems();

            var dataShareRequestId = Guid.Parse("6B9F557E-2F05-41EF-98CC-43C2C3B0A9DD");

            testItems.MockAcquirerDataShareRequestRepository.Setup(x => x.StartDataShareRequestAsync(
                    It.IsAny<IUserIdSet>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<Guid>()))
                .ReturnsAsync(() => dataShareRequestId);

            var mockDataShareRequestQuestionStatusesDeterminationResult = CreateTestDataShareRequestQuestionStatusesDeterminationResult(
                questionsRemainThatRequireAResponse: questionsRemainThatRequireAResponse);

            testItems.MockDataShareRequestQuestionStatusesDetermination.Setup(x => x.DetermineQuestionStatuses(It.IsAny<DataShareRequestQuestionStatusInformationSetModelData>()))
                .Returns(mockDataShareRequestQuestionStatusesDeterminationResult);

            await testItems.AcquirerDataShareRequestService.StartDataShareRequestAsync(
                It.IsAny<Guid>());

            Assert.Multiple(() =>
            {
                testItems.MockAcquirerDataShareRequestRepository.Verify(x =>
                        x.UpdateDataShareRequestQuestionStatusesAsync(It.IsAny<Guid>(), It.IsAny<bool>(), It.IsAny<IEnumerable<IDataShareRequestQuestionStatusDataModel>>()),
                    Times.Once);

                testItems.MockAcquirerDataShareRequestRepository.Verify(x =>
                        x.UpdateDataShareRequestQuestionStatusesAsync(
                            dataShareRequestId,
                            questionsRemainThatRequireAResponse,
                            It.IsAny<IEnumerable<IDataShareRequestQuestionStatusDataModel>>()),
                    Times.Once);
            });
        }

        [Test]
        public async Task GivenTheRequestCompletesSuccessfully_WhenIStartDataShareRequestAsync_ThenASuccessfulDataResultIsReturnedReportingTheIdOfTheCreatedDataShareRequest()
        {
            var testItems = CreateTestItems();

            var dataShareRequestId = Guid.Parse("6B9F557E-2F05-41EF-98CC-43C2C3B0A9DD");

            testItems.MockAcquirerDataShareRequestRepository.Setup(x => x.StartDataShareRequestAsync(
                    It.IsAny<IUserIdSet>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<Guid>()))
                .ReturnsAsync(() => dataShareRequestId);

            var testSuccessfulDataResult = Mock.Of<IServiceOperationDataResult<Guid>>();
            testItems.MockServiceOperationResultFactory.Setup(x => x.CreateSuccessfulDataResult(dataShareRequestId, It.IsAny<HttpStatusCode?>()))
                .Returns(() => testSuccessfulDataResult);

            var result = await testItems.AcquirerDataShareRequestService.StartDataShareRequestAsync(
                It.IsAny<Guid>());

            Assert.That(result, Is.SameAs(testSuccessfulDataResult));
        }

        [Test]
        public async Task GivenAnExceptionIsThrownWhilstPerformingTheRequest_WhenIStartDataShareRequestAsync_ThenAnErrorIsLogged()
        {
            var testItems = CreateTestItems();

            var testException = new Exception("test exception message");

            testItems.MockAcquirerDataShareRequestRepository.Setup(x => x.FindQuestionSetAsync(
                It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>()))
                .ThrowsAsync(testException);

            await testItems.AcquirerDataShareRequestService.StartDataShareRequestAsync(
                It.IsAny<Guid>());

            testItems.MockLogger.VerifyLog(LogLevel.Error, "Failed to StartDataShareRequest", testException);
        }

        [Test]
        public async Task GivenAnExceptionIsThrownWhilstPerformingTheRequest_WhenIStartDataShareRequestAsync_ThenAFailedDataResultIsReturnedReportingTheError()
        {
            var testItems = CreateTestItems();

            testItems.MockAcquirerDataShareRequestRepository.Setup(x => x.FindQuestionSetAsync(
                    It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>()))
                .ThrowsAsync(new Exception());

            var testFailedDataResult = Mock.Of<IServiceOperationDataResult<Guid>>();
            testItems.MockServiceOperationResultFactory.Setup(x => x.CreateFailedDataResult<Guid>(It.IsAny<string>(), It.IsAny<HttpStatusCode?>()))
                .Returns(() => testFailedDataResult);

            var result = await testItems.AcquirerDataShareRequestService.StartDataShareRequestAsync(
                It.IsAny<Guid>());

            Assert.That(result, Is.SameAs(testFailedDataResult));
        }
        #endregion
        
        #region GetDataShareRequestSummariesAsync() Tests
        [Test]
        public async Task GivenAcquirerDetails_WhenIGetDataShareRequestSummariesAsync_ThenTheRequestToTheRepositoryIsMadeUsingTheGivenAcquirerDetails()
        {
            var testItems = CreateTestItems();

            const int testAcquirerUserId = 123;
            const int testAcquirerDomainId = 456;
            const int testAcquirerOrganisationId = 999;

            await testItems.AcquirerDataShareRequestService.GetDataShareRequestSummariesAsync(
                It.IsAny<int>(),
                testAcquirerUserId,
                testAcquirerDomainId,
                testAcquirerOrganisationId,
                It.IsAny<int?>(),
                It.IsAny<Guid?>(),
                It.IsAny<List<DataShareRequestStatus>>());

            testItems.MockAcquirerDataShareRequestRepository.Verify(x => x.GetDataShareRequestsAsync(
                    It.IsAny<int>(),
                    testAcquirerUserId,
                    testAcquirerDomainId,
                    testAcquirerOrganisationId,
                    It.IsAny<int?>(),
                    It.IsAny<Guid?>(),
                    It.IsAny<List<DataShareRequestStatus>>()),
                Times.Once);
        }

        [Test]
        public async Task GivenASupplierOrganisationIdAndEsdaDetails_WhenIGetDataShareRequestSummariesAsync_ThenTheRequestToTheRepositoryIsMadeUsingTheGivenSupplierOrganisationIdAndEsdaDetails()
        {
            var testItems = CreateTestItems();

            const int testSupplierDomainId = 888;
            const int testSupplierOrganisationId = 999;
            var esdaId = Guid.Parse("8336F592-FFA0-41A0-AE0F-B97C7B2A041C");

            await testItems.AcquirerDataShareRequestService.GetDataShareRequestSummariesAsync(
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<int>(),
                testSupplierDomainId,
                testSupplierOrganisationId,
                esdaId,
                It.IsAny<List<DataShareRequestStatus>>());

            testItems.MockAcquirerDataShareRequestRepository.Verify(x => x.GetDataShareRequestsAsync(
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    testSupplierDomainId,
                    testSupplierOrganisationId,
                    esdaId,
                    It.IsAny<List<DataShareRequestStatus>>()),
                Times.Once);
        }

        [Test]
        [TestCaseSource(nameof(GetDataShareRequestSummariesDataShareRequestStatusesTestCaseData))]
        public async Task GivenASetOfDataShareRequestStatuses_WhenIGetDataShareRequestSummariesAsync_ThenTheRequestToTheRepositoryIsMadeUsingTheGivenSetOfDataShareRequestStatuses(
            List<DataShareRequestStatus>? dataShareRequestStatuses)
        {
            var testItems = CreateTestItems();

            await testItems.AcquirerDataShareRequestService.GetDataShareRequestSummariesAsync(
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<Guid>(),
                dataShareRequestStatuses);

            testItems.MockAcquirerDataShareRequestRepository.Verify(x => x.GetDataShareRequestsAsync(
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<Guid>(),
                    dataShareRequestStatuses),
                Times.Once);
        }

        private static IEnumerable<TestCaseData> GetDataShareRequestSummariesDataShareRequestStatusesTestCaseData()
        {
            yield return new TestCaseData((List<DataShareRequestStatus>?) null);

            yield return new TestCaseData(new List<DataShareRequestStatus> {DataShareRequestStatus.Draft });

            yield return new TestCaseData(Enum.GetValues<DataShareRequestStatus>().ToList());
        }

        [Test]
        public async Task GivenValidParameters_WhenIGetDataShareRequestSummariesAsync_ThenTheRequestedDataShareRequestSummarySetIsReturned()
        {
            var testItems = CreateTestItems();

            var testEsdaId = testItems.Fixture.Create<Guid>();

            var testDataShareRequestModelDatas = testItems.Fixture.CreateMany<DataShareRequestModelData>().ToList();
            var testDataShareRequestSummarySet = testItems.Fixture.Create<DataShareRequestSummarySet>();
                
            testItems.MockAcquirerDataShareRequestRepository.Setup(x => x.GetDataShareRequestsAsync(
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<Guid>(),
                    It.IsAny<List<DataShareRequestStatus>>()))
                .ReturnsAsync(testDataShareRequestModelDatas);

            testItems.MockAcquirerDataShareRequestModelDataFactory.Setup(x => x.CreateDataShareRequestSummarySet(
                    testDataShareRequestModelDatas))
                .Returns(testDataShareRequestSummarySet);

            testItems.MockServiceOperationResultFactory.Setup(x => x.CreateSuccessfulDataResult(It.IsAny<DataShareRequestSummarySet>(), It.IsAny<HttpStatusCode?>()))
                .Returns((DataShareRequestSummarySet summarySet, HttpStatusCode? _) => CreateTestSuccessfulDataResult(summarySet));

            var result = await testItems.AcquirerDataShareRequestService.GetDataShareRequestSummariesAsync(
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<int>(),
                testEsdaId,
                It.IsAny<List<DataShareRequestStatus>>());

            Assert.Multiple(() =>
            {
                Assert.That(result.Success, Is.True);
                Assert.That(result.Data, Is.EqualTo(testDataShareRequestSummarySet));
                Assert.That(result.Error, Is.Null);
            });
        }

        [Test]
        public async Task GivenAnExceptionIsThrownWhilstGettingTheDataShareRequestSummaries_WhenIGetDataShareRequestSummariesAsync_ThenAnErrorIsLogged()
        {
            var testItems = CreateTestItems();

            var testException = new Exception("test exception message");

            testItems.MockAcquirerDataShareRequestRepository.Setup(x => x.GetDataShareRequestsAsync(
                    It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<Guid?>(), It.IsAny<IEnumerable<DataShareRequestStatus>?>()))
                .ThrowsAsync(testException);

            await testItems.AcquirerDataShareRequestService.GetDataShareRequestSummariesAsync(
                It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>(), It.IsAny<List<DataShareRequestStatus>>());

            testItems.MockLogger.VerifyLog(LogLevel.Error, "Failed to GetDataShareRequestSummaries", testException);
        }

        [Test]
        public async Task GivenAnExceptionIsThrownWhilstGettingTheDataShareRequestSummaries_WhenIGetDataShareRequestSummariesAsync_ThenAFailedDataResultIsReturnedReportingTheError()
        {
            var testItems = CreateTestItems();

            var testException = new Exception("test exception message");

            testItems.MockAcquirerDataShareRequestRepository.Setup(x => x.GetDataShareRequestsAsync(
                    It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<Guid?>(), It.IsAny<IEnumerable<DataShareRequestStatus>?>()))
                .ThrowsAsync(testException);

            var testFailedDataResult = Mock.Of<IServiceOperationDataResult<DataShareRequestSummarySet>>();
            testItems.MockServiceOperationResultFactory.Setup(x => x.CreateFailedDataResult<DataShareRequestSummarySet>(It.IsAny<string>(), It.IsAny<HttpStatusCode?>()))
                .Returns(() => testFailedDataResult);

            var result = await testItems.AcquirerDataShareRequestService.GetDataShareRequestSummariesAsync(
                It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>(), It.IsAny<List<DataShareRequestStatus>>());

            Assert.That(result, Is.SameAs(testFailedDataResult));
        }
        #endregion

        #region GetDataShareRequestAdminSummariesAsync() Tests
        [Test]
        public async Task GivenFilterParameters_WhenIGetDataShareRequestAdminSummariesAsync_ThenTheRequestToTheRepositoryIsMadeUsingTheGivenParameters()
        {
            var testItems = CreateTestItems();

            var testAcquirerOrganisationId = testItems.Fixture.Create<int>();
            var testSupplierOrganisationId = testItems.Fixture.Create<int>();
            var testDataShareRequestStatuses = testItems.Fixture.CreateMany<DataShareRequestStatus>().ToList();

            await testItems.AcquirerDataShareRequestService.GetDataShareRequestAdminSummariesAsync(
                testAcquirerOrganisationId,
                testSupplierOrganisationId,
                testDataShareRequestStatuses);

            testItems.MockAcquirerDataShareRequestRepository.Verify(x => x.GetDataShareRequestsAsync(
                    null,
                    null,
                    testAcquirerOrganisationId,
                    null,
                    testSupplierOrganisationId,
                    null,
                    testDataShareRequestStatuses),
                Times.Once);
        }

        [Test]
        public async Task GivenValidParameters_WhenIGetDataShareRequestAdminSummariesAsync_ThenTheMatchingDataShareRequestAdminSummariesAreReturned()
        {
            var testItems = CreateTestItems();

            var testAcquirerOrganisationId = testItems.Fixture.Create<int>();
            var testSupplierOrganisationId = testItems.Fixture.Create<int>();
            var testDataShareRequestStatuses = testItems.Fixture.CreateMany<DataShareRequestStatus>().ToList();

            var testMatchingDataShareRequestModelData = testItems.Fixture
                .Build<DataShareRequestModelData>()
                .Create();

            var testMatchingDataShareRequestModelDatas = new List<DataShareRequestModelData> {testMatchingDataShareRequestModelData};

            var testDataShareRequestAdminSummary = testItems.Fixture
                .Build<DataShareRequestAdminSummary>()
                .Create();

            var testDataShareRequestAdminSummaries = new List<DataShareRequestAdminSummary> { testDataShareRequestAdminSummary };

            testItems.MockAcquirerDataShareRequestRepository.Setup(x => x.GetDataShareRequestsAsync(
                    null,
                    null,
                    testAcquirerOrganisationId,
                    null,
                    testSupplierOrganisationId,
                    null,
                    testDataShareRequestStatuses))
                .ReturnsAsync(() => testMatchingDataShareRequestModelDatas);

            var testDataShareRequestAdminSummarySet = testItems.Fixture
                .Build<DataShareRequestAdminSummarySet>()
                .With(x => x.DataShareRequestAdminSummaries, [testDataShareRequestAdminSummary])
                .Create();

            testItems.MockAcquirerDataShareRequestModelDataFactory
                .Setup(x => x.CreateDataShareRequestAdminSummarySet(testDataShareRequestAdminSummaries))
                .Returns(() => testDataShareRequestAdminSummarySet);

            var testUserIdSet = testItems.Fixture
                .Build<UserIdSet>()
                .With(x => x.UserId, testMatchingDataShareRequestModelData.DataShareRequest_AcquirerUserId)
                .Create();
            var testUserDetails = testItems.Fixture.Build<UserDetails>()
                .With(x => x.UserIdSet, testUserIdSet)
                .With(x => x.UserContactDetails, testItems.Fixture.Create<UserContactDetails>())
                .Create();

            testItems.MockUserProfilePresenter.Setup(x => x.GetUserDetailsByUserIdsAsync(new List<int> {testMatchingDataShareRequestModelData.DataShareRequest_AcquirerUserId}))
                .ReturnsAsync(() => new List<IUserDetails>{testUserDetails});

            var testWhenCreated = testItems.Fixture.Create<DateTime>();
            var testCreatedAuditLogDataShareRequestStatusChangeModelDatas = testItems.Fixture
                .Build<AuditLogDataShareRequestStatusChangeModelData>()
                .With(x => x.AuditLogDataShareRequestStatusChange_FromStatus, DataShareRequestStatusType.None)
                .With(x => x.AuditLogDataShareRequestStatusChange_ChangedAtUtc, testWhenCreated)
                .CreateMany(1)
                .ToList();

            testItems.MockAuditLogService
                .Setup(x => x.GetAuditLogsForDataShareRequestStatusChangeToStatusAsync(testMatchingDataShareRequestModelData.DataShareRequest_Id, It.IsAny<IEnumerable<DataShareRequestStatusType>>()))
                .ReturnsAsync(() => testCreatedAuditLogDataShareRequestStatusChangeModelDatas);

            var testWhenSubmitted = testItems.Fixture.Create<DateTime>();
            var testSubmittedAuditLogDataShareRequestStatusChangeModelData = testItems.Fixture
                .Build<AuditLogDataShareRequestStatusChangeModelData>()
                .With(x => x.AuditLogDataShareRequestStatusChange_ChangedAtUtc, testWhenSubmitted)
                .Create();

            testItems.MockAuditLogService
                .Setup(x => x.GetMostRecentAuditLogForDataShareRequestStatusChangeToStatusAsync(testMatchingDataShareRequestModelData.DataShareRequest_Id, DataShareRequestStatusType.Submitted))
                .ReturnsAsync(() => testSubmittedAuditLogDataShareRequestStatusChangeModelData);

            var testWhenNeededBy = testItems.Fixture.Create<DateTime>();
            testItems.MockKeyQuestionPartAnswerProviderService.Setup(x => x.GetDateRequiredQuestionPartAnswerAsync(testMatchingDataShareRequestModelData.DataShareRequest_Id))
                .ReturnsAsync(() => testWhenNeededBy);

            testItems.MockAcquirerDataShareRequestModelDataFactory
                .Setup(x => x.CreateDataShareRequestAdminSummary(
                    testMatchingDataShareRequestModelData,
                    testWhenCreated,
                    testWhenSubmitted,
                    testUserDetails.UserContactDetails.EmailAddress,
                    testWhenNeededBy))
                .Returns(() => testDataShareRequestAdminSummary);

            testItems.MockServiceOperationResultFactory.Setup(x => x.CreateSuccessfulDataResult(It.IsAny<DataShareRequestAdminSummarySet>(), It.IsAny<HttpStatusCode?>()))
                .Returns((DataShareRequestAdminSummarySet adminSummarySet, HttpStatusCode? _) => CreateTestSuccessfulDataResult(adminSummarySet));

            var result = await testItems.AcquirerDataShareRequestService.GetDataShareRequestAdminSummariesAsync(
                testAcquirerOrganisationId,
                testSupplierOrganisationId,
                testDataShareRequestStatuses);

            Assert.Multiple(() =>
            {
                Assert.That(result.Success, Is.True);
                Assert.That(result.Error, Is.Null);

                Assert.That(result.Data, Is.EqualTo(testDataShareRequestAdminSummarySet));
            });
        }

        [Test]
        public async Task GivenAnExceptionIsThrownWhilstGettingTheDataShareAdminRequestSummaries_WhenIGetDataShareRequestAdminSummariesAsync_ThenAnErrorIsLogged()
        {
            var testItems = CreateTestItems();

            var testException = new Exception("test exception message");

            testItems.MockAcquirerDataShareRequestRepository.Setup(x => x.GetDataShareRequestsAsync(
                    It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<Guid?>(), It.IsAny<IEnumerable<DataShareRequestStatus>?>()))
                .ThrowsAsync(testException);

            await testItems.AcquirerDataShareRequestService.GetDataShareRequestAdminSummariesAsync(
                It.IsAny<int>(), It.IsAny<int>(), It.IsAny<IEnumerable<DataShareRequestStatus>>());

            testItems.MockLogger.VerifyLog(LogLevel.Error, "Failed to GetDataShareRequestAdminSummaries", testException);
        }

        [Test]
        public async Task GivenAnExceptionIsThrownWhilstGettingTheDataShareAdminRequestSummaries_WhenIGetDataShareRequestAdminSummariesAsync_ThenAFailedDataResultIsReturnedReportingTheError()
        {
            var testItems = CreateTestItems();

            var testException = new Exception("test exception message");

            testItems.MockAcquirerDataShareRequestRepository.Setup(x => x.GetDataShareRequestsAsync(
                    It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<Guid?>(), It.IsAny<IEnumerable<DataShareRequestStatus>?>()))
                .ThrowsAsync(testException);

            var testFailedDataResult = Mock.Of<IServiceOperationDataResult<DataShareRequestAdminSummarySet>>();
            testItems.MockServiceOperationResultFactory.Setup(x => x.CreateFailedDataResult<DataShareRequestAdminSummarySet>(It.IsAny<string>(), It.IsAny<HttpStatusCode?>()))
                .Returns(() => testFailedDataResult);

            var result = await testItems.AcquirerDataShareRequestService.GetDataShareRequestAdminSummariesAsync(
                It.IsAny<int>(), It.IsAny<int>(), It.IsAny<List<DataShareRequestStatus>>());

            Assert.That(result, Is.SameAs(testFailedDataResult));
        }
        #endregion

        #region GetAcquirerDataShareRequestSummariesAsync() Tests
        [Test]
        public async Task GivenAcquirerIsLoggedIn_WhenIGetAcquirerDataShareRequestSummariesAsync_ThenTheRequestToTheRepositoryIsMadeUsingTheGivenTheLoggingInAcquirerDetails()
        {
            var testItems = CreateTestItems();

            var testUserIdSet = CreateTestUserIdSet(userId: 100, domainId: 200, organisationId: 300);
            testItems.MockUserProfilePresenter.Setup(x => x.GetInitiatingUserIdSetAsync())
                .ReturnsAsync(() => testUserIdSet);

            await testItems.AcquirerDataShareRequestService.GetAcquirerDataShareRequestSummariesAsync(
                It.IsAny<int?>(),
                It.IsAny<int?>(),
                It.IsAny<Guid?>(),
                It.IsAny<List<DataShareRequestStatus>>());

            testItems.MockAcquirerDataShareRequestRepository.Verify(x => x.GetDataShareRequestsAsync(
                    100,
                    200,
                    300,
                    It.IsAny<int?>(),
                    It.IsAny<int?>(),
                    It.IsAny<Guid?>(),
                    It.IsAny<List<DataShareRequestStatus>>()),
                Times.Once);
        }

        [Test]
        public async Task GivenASupplierAndEsdaDetails_WhenIGetAcquirerDataShareRequestSummariesAsync_ThenTheRequestToTheRepositoryIsMadeUsingTheGivenSupplierAndEsdaDetails()
        {
            var testItems = CreateTestItems();

            const int testSupplierDomainId = 888;
            const int testSupplierOrganisationId = 999;
            var esdaId = Guid.Parse("8336F592-FFA0-41A0-AE0F-B97C7B2A041C");

            await testItems.AcquirerDataShareRequestService.GetAcquirerDataShareRequestSummariesAsync(
                testSupplierDomainId,
                testSupplierOrganisationId,
                esdaId,
                It.IsAny<List<DataShareRequestStatus>>());

            testItems.MockAcquirerDataShareRequestRepository.Verify(x => x.GetDataShareRequestsAsync(
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    testSupplierDomainId,
                    testSupplierOrganisationId,
                    esdaId,
                    It.IsAny<List<DataShareRequestStatus>>()),
                Times.Once);
        }

        [Test]
        [TestCaseSource(nameof(GetAcquirerDataShareRequestSummariesDataShareRequestStatusesTestCaseData))]
        public async Task GivenASetOfDataShareRequestStatuses_WhenIAcquirerGetDataShareRequestSummariesAsync_ThenTheRequestToTheRepositoryIsMadeUsingTheGivenSetOfDataShareRequestStatuses(
            List<DataShareRequestStatus>? dataShareRequestStatuses)
        {
            var testItems = CreateTestItems();

            await testItems.AcquirerDataShareRequestService.GetAcquirerDataShareRequestSummariesAsync(
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<Guid>(),
                dataShareRequestStatuses);

            testItems.MockAcquirerDataShareRequestRepository.Verify(x => x.GetDataShareRequestsAsync(
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<Guid>(),
                    dataShareRequestStatuses),
                Times.Once);
        }

        private static IEnumerable<TestCaseData> GetAcquirerDataShareRequestSummariesDataShareRequestStatusesTestCaseData()
        {
            yield return new TestCaseData((List<DataShareRequestStatus>?)null);

            yield return new TestCaseData(new List<DataShareRequestStatus> { DataShareRequestStatus.Draft });

            yield return new TestCaseData(Enum.GetValues<DataShareRequestStatus>().ToList());
        }

        [Test]
        public async Task GivenValidParameters_WhenIGetAcquirerDataShareRequestSummariesAsync_ThenTheRequestedDataShareRequestSummarySetIsReturned()
        {
            var testItems = CreateTestItems();

            var testEsdaId = testItems.Fixture.Create<Guid>();

            var testDataShareRequestModelDatas = testItems.Fixture.CreateMany<DataShareRequestModelData>().ToList();
            var testDataShareRequestSummarySet = testItems.Fixture.Create<DataShareRequestSummarySet>();

            testItems.MockAcquirerDataShareRequestRepository.Setup(x => x.GetDataShareRequestsAsync(
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<Guid>(),
                    It.IsAny<List<DataShareRequestStatus>>()))
                .ReturnsAsync(testDataShareRequestModelDatas);

            testItems.MockAcquirerDataShareRequestModelDataFactory.Setup(x => x.CreateDataShareRequestSummarySet(
                    testDataShareRequestModelDatas))
                .Returns(testDataShareRequestSummarySet);

            testItems.MockServiceOperationResultFactory.Setup(x => x.CreateSuccessfulDataResult(It.IsAny<DataShareRequestSummarySet>(), It.IsAny<HttpStatusCode?>()))
                .Returns((DataShareRequestSummarySet summarySet, HttpStatusCode? _) => CreateTestSuccessfulDataResult(summarySet));

            var result = await testItems.AcquirerDataShareRequestService.GetAcquirerDataShareRequestSummariesAsync(
                It.IsAny<int>(),
                It.IsAny<int>(),
                testEsdaId,
                It.IsAny<List<DataShareRequestStatus>>());

            Assert.Multiple(() =>
            {
                Assert.That(result.Success, Is.True);
                Assert.That(result.Data, Is.EqualTo(testDataShareRequestSummarySet));
                Assert.That(result.Error, Is.Null);
            });
        }

        [Test]
        public async Task GivenAnExceptionIsThrownWhilstGettingTheAcquirerDataShareRequestSummaries_WhenIGetAcquirerDataShareRequestSummariesAsync_ThenAnErrorIsLogged()
        {
            var testItems = CreateTestItems();

            var testException = new Exception("test exception message");

            testItems.MockAcquirerDataShareRequestRepository.Setup(x => x.GetDataShareRequestsAsync(
                    It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>(), It.IsAny<List<DataShareRequestStatus>>()))
                .ThrowsAsync(testException);

            await testItems.AcquirerDataShareRequestService.GetAcquirerDataShareRequestSummariesAsync(
                It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>(), It.IsAny<List<DataShareRequestStatus>>());

            testItems.MockLogger.VerifyLog(LogLevel.Error, "Failed to GetAcquirerDataShareRequestSummaries", testException);
        }

        [Test]
        public async Task GivenAnExceptionIsThrownWhilstGettingTheAcquirerDataShareRequestSummaries_WhenIGetAcquirerDataShareRequestSummariesAsync_ThenAFailedDataResultIsReturnedReportingTheError()
        {
            var testItems = CreateTestItems();

            var testException = new Exception("test exception message");

            testItems.MockAcquirerDataShareRequestRepository.Setup(x => x.GetDataShareRequestsAsync(
                    It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>(), It.IsAny<List<DataShareRequestStatus>>()))
                .ThrowsAsync(testException);

            var testFailedDataResult = Mock.Of<IServiceOperationDataResult<DataShareRequestSummarySet>>();
            testItems.MockServiceOperationResultFactory.Setup(x => x.CreateFailedDataResult<DataShareRequestSummarySet>(It.IsAny<string>(), It.IsAny<HttpStatusCode?>()))
                .Returns(() => testFailedDataResult);

            var result = await testItems.AcquirerDataShareRequestService.GetAcquirerDataShareRequestSummariesAsync(
                It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>(), It.IsAny<List<DataShareRequestStatus>>());

            Assert.That(result, Is.SameAs(testFailedDataResult));
        }
        #endregion

        #region GetDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisationAsync() Tests
        [Test]
        public async Task GivenAnEsdaId_WhenIGetDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisationAsync_ThenTheRequestToTheRepositoryIsMadeUsingTheEsdaId()
        {
            var testItems = CreateTestItems();

            var testEsdaId = testItems.Fixture.Create<Guid>();

            await testItems.AcquirerDataShareRequestService.GetDataShareRequestSummariesAsync(
                It.IsAny<int?>(),
                It.IsAny<int?>(),
                It.IsAny<int?>(),
                It.IsAny<int?>(),
                It.IsAny<int?>(),
                testEsdaId,
                It.IsAny<IEnumerable<DataShareRequestStatus>?>());

            testItems.MockAcquirerDataShareRequestRepository.Verify(x => x.GetDataShareRequestsAsync(
                    It.IsAny<int?>(),
                    It.IsAny<int?>(),
                    It.IsAny<int?>(),
                    It.IsAny<int?>(),
                    It.IsAny<int?>(),
                    testEsdaId,
                    It.IsAny<IEnumerable<DataShareRequestStatus>?>()),
                Times.Once);
        }

        [Test]
        public async Task GivenAnEsdaId_WhenIGetDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisationAsync_ThenADataShareRequestRaisedForEsdaByAcquirerOrganisationSummarySetIsReturned()
        {
            var testItems = CreateTestItems();

            var testEsdaId = testItems.Fixture.Create<Guid>();
            var testEsdaTitle = testItems.Fixture.Create<string>();

            var testAcquirerUserIdSet = testItems.Fixture.Create<UserIdSet>();
            testItems.MockUserProfilePresenter.Setup(x => x.GetInitiatingUserIdSetAsync())
                .ReturnsAsync(() => testAcquirerUserIdSet);

            var testMatchingDataShareRequestModelData = testItems.Fixture
                .Build<DataShareRequestModelData>()
                .Create();

            var testMatchingDataShareRequestModelDatas = new List<DataShareRequestModelData> { testMatchingDataShareRequestModelData };

            testItems.MockAcquirerDataShareRequestRepository.Setup(x => x.GetDataShareRequestsAsync(
                    null,
                    null,
                    testAcquirerUserIdSet.OrganisationId,
                    null,
                    null,
                    testEsdaId,
                    It.IsAny<IEnumerable<DataShareRequestStatus>?>()))
                .ReturnsAsync(() => testMatchingDataShareRequestModelDatas);

            var testDataShareRequestAcquirerUserIdSet = testItems.Fixture
                .Build<UserIdSet>()
                .With(x => x.UserId, testMatchingDataShareRequestModelData.DataShareRequest_AcquirerUserId)
                .Create();

            var testDataShareRequestAcquirerUserDetails = testItems.Fixture.Build<UserDetails>()
                .With(x => x.UserIdSet, testDataShareRequestAcquirerUserIdSet)
                .With(x => x.UserContactDetails, testItems.Fixture.Create<UserContactDetails>())
                .Create();

            testItems.MockUserProfilePresenter.Setup(x => x.GetUserDetailsByUserIdAsync(testMatchingDataShareRequestModelData.DataShareRequest_AcquirerUserId))
                .ReturnsAsync(() => testDataShareRequestAcquirerUserDetails);

            var testEsdaDetails = testItems.Fixture
                .Build<EsdaDetails>()
                .With(x => x.Title, testEsdaTitle)
                .Create();

            testItems.MockEsdaInformationPresenter
                .Setup(x => x.GetEsdaDetailsByIdAsync(testEsdaId))
                .ReturnsAsync(() => testEsdaDetails);

            var testWhenCreated = testItems.Fixture.Create<DateTime>();

            var testCreationAuditLog = testItems.Fixture
                .Build<AuditLogDataShareRequestStatusChangeModelData>()
                .With(x => x.AuditLogDataShareRequestStatusChange_ChangedAtUtc, testWhenCreated)
                .Create();

            testItems.MockAuditLogService
                .Setup(x => x.GetMostRecentAuditLogForDataShareRequestStatusChangeToStatusAsync(
                    testMatchingDataShareRequestModelData.DataShareRequest_Id, DataShareRequestStatusType.Draft))
                .ReturnsAsync(() => testCreationAuditLog);

            var testWhenSubmitted = testItems.Fixture.Create<DateTime>();

            var testSubmissionAuditLog = testItems.Fixture
                .Build<AuditLogDataShareRequestStatusChangeModelData>()
                .With(x => x.AuditLogDataShareRequestStatusChange_ChangedAtUtc, testWhenSubmitted)
                .Create();

            testItems.MockAuditLogService
                .Setup(x => x.GetMostRecentAuditLogForDataShareRequestStatusChangeToStatusAsync(
                    testMatchingDataShareRequestModelData.DataShareRequest_Id, DataShareRequestStatusType.Submitted))
                .ReturnsAsync(() => testSubmissionAuditLog);

            var testDataShareRequestRaisedForEsdaByAcquirerOrganisationSummary = testItems.Fixture
                .Build<DataShareRequestRaisedForEsdaByAcquirerOrganisationSummary>()
                .Create();

            testItems.MockAcquirerDataShareRequestModelDataFactory
                .Setup(x => x.CreateDataShareRequestRaisedForEsdaByAcquirerOrganisationSummary(
                    testMatchingDataShareRequestModelData,
                    testCreationAuditLog,
                    testSubmissionAuditLog,
                    testDataShareRequestAcquirerUserDetails))
                .Returns(() => testDataShareRequestRaisedForEsdaByAcquirerOrganisationSummary);

            testItems.MockServiceOperationResultFactory.Setup(x => x.CreateSuccessfulDataResult(It.IsAny<DataShareRequestRaisedForEsdaByAcquirerOrganisationSummarySet>(), It.IsAny<HttpStatusCode?>()))
                .Returns((DataShareRequestRaisedForEsdaByAcquirerOrganisationSummarySet summarySet, HttpStatusCode? _) => CreateTestSuccessfulDataResult(summarySet));

            var result = await testItems.AcquirerDataShareRequestService.GetDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisationAsync(
                testEsdaId);

            Assert.Multiple(() =>
            {
                Assert.That(result.Success, Is.True);
                Assert.That(result.Error, Is.Null);
                
                Assert.That(result.Data!.EsdaId, Is.EqualTo(testEsdaId));
                Assert.That(result.Data.EsdaName, Is.EqualTo(testEsdaTitle));
                Assert.That(result.Data.DataShareRequestRaisedForEsdaByAcquirerOrganisationSummaries.Single(), Is.SameAs(testDataShareRequestRaisedForEsdaByAcquirerOrganisationSummary));
            });
        }

        [Test]
        public async Task GivenAnEsdaIdForWhichAnEsdaCannotBeFound_WhenIGetDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisationAsync_ThenADataShareRequestRaisedForEsdaByAcquirerOrganisationSummarySetIsReturnedWithEmptyTitle()
        {
            var testItems = CreateTestItems();

            var testEsdaId = testItems.Fixture.Create<Guid>();

            var testAcquirerUserIdSet = testItems.Fixture.Create<UserIdSet>();
            testItems.MockUserProfilePresenter.Setup(x => x.GetInitiatingUserIdSetAsync())
                .ReturnsAsync(() => testAcquirerUserIdSet);

            var testMatchingDataShareRequestModelData = testItems.Fixture
                .Build<DataShareRequestModelData>()
                .Create();

            var testMatchingDataShareRequestModelDatas = new List<DataShareRequestModelData> { testMatchingDataShareRequestModelData };

            testItems.MockAcquirerDataShareRequestRepository.Setup(x => x.GetDataShareRequestsAsync(
                    null,
                    null,
                    testAcquirerUserIdSet.OrganisationId,
                    null,
                    null,
                    testEsdaId,
                    It.IsAny<IEnumerable<DataShareRequestStatus>?>()))
                .ReturnsAsync(() => testMatchingDataShareRequestModelDatas);

            var testDataShareRequestAcquirerUserIdSet = testItems.Fixture
                .Build<UserIdSet>()
                .With(x => x.UserId, testMatchingDataShareRequestModelData.DataShareRequest_AcquirerUserId)
                .Create();

            var testDataShareRequestAcquirerUserDetails = testItems.Fixture.Build<UserDetails>()
                .With(x => x.UserIdSet, testDataShareRequestAcquirerUserIdSet)
                .With(x => x.UserContactDetails, testItems.Fixture.Create<UserContactDetails>())
                .Create();

            testItems.MockUserProfilePresenter.Setup(x => x.GetUserDetailsByUserIdAsync(testMatchingDataShareRequestModelData.DataShareRequest_AcquirerUserId))
                .ReturnsAsync(() => testDataShareRequestAcquirerUserDetails);

            testItems.MockEsdaInformationPresenter
                .Setup(x => x.GetEsdaDetailsByIdAsync(testEsdaId))
                .ThrowsAsync(new Exception("Oh noes!"));

            var testWhenCreated = testItems.Fixture.Create<DateTime>();

            var testCreationAuditLog = testItems.Fixture
                .Build<AuditLogDataShareRequestStatusChangeModelData>()
                .With(x => x.AuditLogDataShareRequestStatusChange_ChangedAtUtc, testWhenCreated)
                .Create();

            testItems.MockAuditLogService
                .Setup(x => x.GetMostRecentAuditLogForDataShareRequestStatusChangeToStatusAsync(
                    testMatchingDataShareRequestModelData.DataShareRequest_Id, DataShareRequestStatusType.Draft))
                .ReturnsAsync(() => testCreationAuditLog);

            var testWhenSubmitted = testItems.Fixture.Create<DateTime>();

            var testSubmissionAuditLog = testItems.Fixture
                .Build<AuditLogDataShareRequestStatusChangeModelData>()
                .With(x => x.AuditLogDataShareRequestStatusChange_ChangedAtUtc, testWhenSubmitted)
                .Create();

            testItems.MockAuditLogService
                .Setup(x => x.GetMostRecentAuditLogForDataShareRequestStatusChangeToStatusAsync(
                    testMatchingDataShareRequestModelData.DataShareRequest_Id, DataShareRequestStatusType.Submitted))
                .ReturnsAsync(() => testSubmissionAuditLog);

            var testDataShareRequestRaisedForEsdaByAcquirerOrganisationSummary = testItems.Fixture
                .Build<DataShareRequestRaisedForEsdaByAcquirerOrganisationSummary>()
                .Create();

            testItems.MockAcquirerDataShareRequestModelDataFactory
                .Setup(x => x.CreateDataShareRequestRaisedForEsdaByAcquirerOrganisationSummary(
                    testMatchingDataShareRequestModelData,
                    testCreationAuditLog,
                    testSubmissionAuditLog,
                    testDataShareRequestAcquirerUserDetails))
                .Returns(() => testDataShareRequestRaisedForEsdaByAcquirerOrganisationSummary);

            testItems.MockServiceOperationResultFactory.Setup(x => x.CreateSuccessfulDataResult(It.IsAny<DataShareRequestRaisedForEsdaByAcquirerOrganisationSummarySet>(), It.IsAny<HttpStatusCode?>()))
                .Returns((DataShareRequestRaisedForEsdaByAcquirerOrganisationSummarySet summarySet, HttpStatusCode? _) => CreateTestSuccessfulDataResult(summarySet));

            var result = await testItems.AcquirerDataShareRequestService.GetDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisationAsync(
                testEsdaId);

            Assert.Multiple(() =>
            {
                Assert.That(result.Success, Is.True);
                Assert.That(result.Error, Is.Null);

                Assert.That(result.Data!.EsdaId, Is.EqualTo(testEsdaId));
                Assert.That(result.Data.EsdaName, Is.Empty);
                Assert.That(result.Data.DataShareRequestRaisedForEsdaByAcquirerOrganisationSummaries.Single(), Is.SameAs(testDataShareRequestRaisedForEsdaByAcquirerOrganisationSummary));
            });
        }

        [Test]
        public async Task GivenAnExceptionIsThrownWhilstGettingTheDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisation_WhenIGetDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisationAsync_ThenAnErrorIsLogged()
        {
            var testItems = CreateTestItems();

            var testException = new Exception("test exception message");

            testItems.MockAcquirerDataShareRequestRepository.Setup(x => x.GetDataShareRequestsAsync(
                    It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<Guid?>(), It.IsAny<IEnumerable<DataShareRequestStatus>?>()))
                .ThrowsAsync(testException);

            await testItems.AcquirerDataShareRequestService.GetDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisationAsync(
                It.IsAny<Guid>());

            testItems.MockLogger.VerifyLog(LogLevel.Error, "Failed to GetDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisation", testException);
        }

        [Test]
        public async Task GivenAnExceptionIsThrownWhilstGettingTheDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisation_WhenIGetDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisationAsync_ThenAFailedDataResultIsReturnedReportingTheError()
        {
            var testItems = CreateTestItems();

            var testException = new Exception("test exception message");

            testItems.MockAcquirerDataShareRequestRepository.Setup(x => x.GetDataShareRequestsAsync(
                    It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<Guid?>(), It.IsAny<IEnumerable<DataShareRequestStatus>?>()))
                .ThrowsAsync(testException);

            var testFailedDataResult = Mock.Of<IServiceOperationDataResult<DataShareRequestRaisedForEsdaByAcquirerOrganisationSummarySet>>();
            testItems.MockServiceOperationResultFactory.Setup(x => x.CreateFailedDataResult<DataShareRequestRaisedForEsdaByAcquirerOrganisationSummarySet>(It.IsAny<string>(), It.IsAny<HttpStatusCode?>()))
                .Returns(() => testFailedDataResult);

            var result = await testItems.AcquirerDataShareRequestService.GetDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisationAsync(
                It.IsAny<Guid>());

            Assert.That(result, Is.SameAs(testFailedDataResult));
        }
        #endregion

        #region GetDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisationAsync() Tests
        [Test]
        public async Task GivenAnEsdaId_WhenIGetDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisationAsync_ThenTheDataShareRequestsForTheEsdaAreReadFromTheRepository()
        {
            var testItems = CreateTestItems();

            var testEsdaId = testItems.Fixture.Create<Guid>();

            await testItems.AcquirerDataShareRequestService.GetDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisationAsync(
                testEsdaId);

            testItems.MockAcquirerDataShareRequestRepository.Verify(x => x.GetDataShareRequestsAsync(
                    It.IsAny<int?>(),
                    It.IsAny<int?>(),
                    It.IsAny<int?>(),
                    It.IsAny<int?>(),
                    It.IsAny<int?>(),
                    testEsdaId,
                    It.IsAny<IEnumerable<DataShareRequestStatus>?>()),
                Times.Once);
        }

        [Test]
        public async Task GivenAnAcquirerIsLoggedIn_WhenIGetDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisationAsync_ThenTheDataShareRequestsMadeByTheOrganisationOfTheAcquirerAreReadFromTheRepository()
        {
            var testItems = CreateTestItems();

            var testUserIdSet = CreateTestUserIdSet(userId: 100, domainId: 200, organisationId: 300);
            testItems.MockUserProfilePresenter.Setup(x => x.GetInitiatingUserIdSetAsync())
                .ReturnsAsync(() => testUserIdSet);

            await testItems.AcquirerDataShareRequestService.GetDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisationAsync(
                It.IsAny<Guid>());

            testItems.MockAcquirerDataShareRequestRepository.Verify(x => x.GetDataShareRequestsAsync(
                    null,
                    null,
                    300,
                    null,
                    null,
                    It.IsAny<Guid?>(),
                    null),
                Times.Once);
        }

        [Test]
        public async Task GivenNoDataShareRequestsHaveBeenMadeForAnEsdaByTheAcquirersOrganisation_WhenIGetDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisationAsync_ThenAnEmptySetIsReturned()
        {
            var testItems = CreateTestItems();

            testItems.MockAcquirerDataShareRequestRepository.Setup(x => x.GetDataShareRequestsAsync(
                    It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<Guid?>(), It.IsAny<IEnumerable<DataShareRequestStatus>?>()))
                .ReturnsAsync(Enumerable.Empty<DataShareRequestModelData>);

            testItems.MockServiceOperationResultFactory.Setup(x => x.CreateSuccessfulDataResult(It.IsAny<DataShareRequestRaisedForEsdaByAcquirerOrganisationSummarySet>(), It.IsAny<HttpStatusCode?>()))
                .Returns((DataShareRequestRaisedForEsdaByAcquirerOrganisationSummarySet summarySet, HttpStatusCode? _) => CreateTestSuccessfulDataResult(summarySet));

            var result = await testItems.AcquirerDataShareRequestService.GetDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisationAsync(
                It.IsAny<Guid>());

            Assert.Multiple(() =>
            {
                Assert.That(result.Success, Is.True);
                Assert.That(result.Data!.DataShareRequestRaisedForEsdaByAcquirerOrganisationSummaries, Is.Empty);
            });
        }

        [Test]
        public async Task GivenDataShareRequestsHaveBeenMadeForAnEsdaByTheAcquirersOrganisation_WhenIGetDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisationAsync_ThenTheUserDetailsOfTheCreatingAcquirersAreFetchedOncePerAcquirer()
        {
            var testItems = CreateTestItems();

            testItems.MockUserProfilePresenter.Setup(x => x.GetUserDetailsByUserIdAsync(It.IsAny<int>()))
                .ReturnsAsync((int userId) => CreateTestUserDetails(userId: userId));

            var testDataShareRequestModelDatas = new List<DataShareRequestModelData>
            {
                CreateTestDataShareRequestModelData(acquirerUserId: 100),
                CreateTestDataShareRequestModelData(acquirerUserId: 200),
                CreateTestDataShareRequestModelData(acquirerUserId: 300),
                CreateTestDataShareRequestModelData(acquirerUserId: 200),
                CreateTestDataShareRequestModelData(acquirerUserId: 200),
                CreateTestDataShareRequestModelData(acquirerUserId: 100)
            };

            testItems.MockAcquirerDataShareRequestRepository.Setup(x => x.GetDataShareRequestsAsync(
                    It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<Guid?>(), It.IsAny<IEnumerable<DataShareRequestStatus>?>()))
                .ReturnsAsync(() => testDataShareRequestModelDatas);

            await testItems.AcquirerDataShareRequestService.GetDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisationAsync(
                It.IsAny<Guid>());

            Assert.Multiple(() =>
            {
                testItems.MockUserProfilePresenter.Verify(x => x.GetUserDetailsByUserIdAsync(It.IsAny<int>()), Times.Exactly(3));

                testItems.MockUserProfilePresenter.Verify(x => x.GetUserDetailsByUserIdAsync(100), Times.Once);
                testItems.MockUserProfilePresenter.Verify(x => x.GetUserDetailsByUserIdAsync(200), Times.Once);
                testItems.MockUserProfilePresenter.Verify(x => x.GetUserDetailsByUserIdAsync(300), Times.Once);
            });
        }

        [Test]
        public async Task GivenDataShareRequestsHaveBeenMadeForAnEsdaByTheAcquirersOrganisation_WhenIGetDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisationAsync_ThenTheAuditLogForTheChangeToDraftStatusIsQueriedForEachDataShareRequest()
        {
            var testItems = CreateTestItems();

            testItems.MockUserProfilePresenter.Setup(x => x.GetUserDetailsByUserIdAsync(It.IsAny<int>()))
                .ReturnsAsync((int userId) => CreateTestUserDetails(userId: userId));

            var testDataShareRequestModelDatas = testItems.Fixture.CreateMany<DataShareRequestModelData>(3);

            testItems.MockAcquirerDataShareRequestRepository.Setup(x => x.GetDataShareRequestsAsync(
                    It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<Guid?>(), It.IsAny<IEnumerable<DataShareRequestStatus>?>()))
                .ReturnsAsync(() => testDataShareRequestModelDatas);

            await testItems.AcquirerDataShareRequestService.GetDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisationAsync(
                It.IsAny<Guid>());

            Assert.Multiple(() =>
            {
                testItems.MockAuditLogService.Verify(x => x.GetMostRecentAuditLogForDataShareRequestStatusChangeToStatusAsync(
                    It.IsAny<Guid>(), DataShareRequestStatusType.Draft),
                    Times.Exactly(3));

                foreach (var testDataShareRequestModelData in testDataShareRequestModelDatas)
                {
                    testItems.MockAuditLogService.Verify(x => x.GetMostRecentAuditLogForDataShareRequestStatusChangeToStatusAsync(
                            testDataShareRequestModelData.DataShareRequest_Id,
                            DataShareRequestStatusType.Draft),
                        Times.Once);
                }
            });
        }

        [Test]
        public async Task GivenDataShareRequestsHasBeenMadeForAnEsdaByTheAcquirersOrganisationButHasNoAuditEntryForTheChangeToDraftStatus_WhenIGetDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisationAsync_ThenAFailedResultIsReturned()
        {
            var testItems = CreateTestItems();

            testItems.MockUserProfilePresenter.Setup(x => x.GetUserDetailsByUserIdAsync(It.IsAny<int>()))
                .ReturnsAsync((int userId) => CreateTestUserDetails(userId: userId));

            var testDataShareRequestModelDatas = testItems.Fixture.CreateMany<DataShareRequestModelData>(3);

            testItems.MockAcquirerDataShareRequestRepository.Setup(x => x.GetDataShareRequestsAsync(
                    It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<Guid?>(), It.IsAny<IEnumerable<DataShareRequestStatus>?>()))
                .ReturnsAsync(() => testDataShareRequestModelDatas);

            testItems.MockAuditLogService.Setup(x =>
                    x.GetMostRecentAuditLogForDataShareRequestStatusChangeToStatusAsync(It.IsAny<Guid>(), It.IsAny<DataShareRequestStatusType?>()))
                .ReturnsAsync((AuditLogDataShareRequestStatusChangeModelData?)null);

            testItems.MockServiceOperationResultFactory.Setup(x => x.CreateFailedDataResult<DataShareRequestRaisedForEsdaByAcquirerOrganisationSummarySet>(It.IsAny<string>(), It.IsAny<HttpStatusCode?>()))
                .Returns((string errorMessage, HttpStatusCode? _) => CreateTestFailedDataResult<DataShareRequestRaisedForEsdaByAcquirerOrganisationSummarySet>(errorMessage));

            var result = await testItems.AcquirerDataShareRequestService.GetDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisationAsync(It.IsAny<Guid>());

            Assert.Multiple(() =>
            {
                Assert.That(result.Success, Is.False);
                Assert.That(result.Data, Is.Null);
                Assert.That(result.Error!.Contains("DataShareRequest has no audit log for creation"));
            });
        }

        [Test]
        public async Task GivenDataShareRequestsHaveBeenMadeForAnEsdaByTheAcquirersOrganisation_WhenIGetDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisationAsync_ThenTheAuditLogForTheChangeToSubmittedStatusIsQueriedForEachDataShareRequest()
        {
            var testItems = CreateTestItems();

            testItems.MockUserProfilePresenter.Setup(x => x.GetUserDetailsByUserIdAsync(It.IsAny<int>()))
                .ReturnsAsync((int userId) => CreateTestUserDetails(userId: userId));

            var testDataShareRequestModelDatas = testItems.Fixture.CreateMany<DataShareRequestModelData>(3);

            testItems.MockAcquirerDataShareRequestRepository.Setup(x => x.GetDataShareRequestsAsync(
                    It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<Guid?>(), It.IsAny<IEnumerable<DataShareRequestStatus>?>()))
                .ReturnsAsync(() => testDataShareRequestModelDatas);

            await testItems.AcquirerDataShareRequestService.GetDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisationAsync(
                It.IsAny<Guid>());

            Assert.Multiple(() =>
            {
                testItems.MockAuditLogService.Verify(x => x.GetMostRecentAuditLogForDataShareRequestStatusChangeToStatusAsync(
                        It.IsAny<Guid>(), DataShareRequestStatusType.Submitted),
                    Times.Exactly(3));

                foreach (var testDataShareRequestModelData in testDataShareRequestModelDatas)
                {
                    testItems.MockAuditLogService.Verify(x => x.GetMostRecentAuditLogForDataShareRequestStatusChangeToStatusAsync(
                            testDataShareRequestModelData.DataShareRequest_Id,
                            DataShareRequestStatusType.Submitted),
                        Times.Once);
                }
            });
        }

        [Test]
        public async Task GivenDataShareRequestsHaveBeenMadeForAnEsdaByTheAcquirersOrganisation_WhenIGetDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisationAsync_ThenDataShareRequestRaisedForEsdaByAcquirerOrganisationSummarySetIsReturnedForThoseDataShareRequests()
        {
            var testItems = CreateTestItems();

            var testEsdaId = testItems.Fixture.Create<Guid>();
            var testAcquirerUserId = testItems.Fixture.Create<int>();
            var testAcquirerOrganisationId = testItems.Fixture.Create<int>();

            testItems.MockUserProfilePresenter.Setup(x => x.GetInitiatingUserIdSetAsync())
                .ReturnsAsync(CreateTestUserIdSet(userId: testAcquirerUserId, organisationId: testAcquirerOrganisationId));

            testItems.MockUserProfilePresenter.Setup(x => x.GetUserDetailsByUserIdAsync(It.IsAny<int>()))
                .ReturnsAsync((int userId) => CreateTestUserDetails(userId: userId));

            // -- Set up existing data share requests
            var testDataShareRequestModelData1 = CreateDataShareRequestModelData(200, "test-request-1", DataShareRequestStatusType.Draft);
            var testDataShareRequestModelData2 = CreateDataShareRequestModelData(300, "test-request-2", DataShareRequestStatusType.Rejected);
            var testDataShareRequestModelData3 = CreateDataShareRequestModelData(200, "test-request-3", DataShareRequestStatusType.Submitted);
            var testDataShareRequestModelDatas = new List<DataShareRequestModelData>{ testDataShareRequestModelData1, testDataShareRequestModelData2, testDataShareRequestModelData3 };

            testItems.MockAcquirerDataShareRequestRepository.Setup(x => x.GetDataShareRequestsAsync(
                    null,
                    null,
                    testAcquirerOrganisationId,
                    null,
                    null,
                    testEsdaId,
                    null))
                .ReturnsAsync(() => testDataShareRequestModelDatas);

            // -- Set up audit logs
            var testDataShareRequestModelData1_CreatedAtUtc = testItems.Fixture.Create<DateTime>();
            var testDataShareRequestModelData1_SubmittedAtUtc = (DateTime?) null;
            SetupAuditLog(testDataShareRequestModelData1, testDataShareRequestModelData1_CreatedAtUtc, testDataShareRequestModelData1_SubmittedAtUtc);

            var testDataShareRequestModelData2_CreatedAtUtc = testItems.Fixture.Create<DateTime>();
            var testDataShareRequestModelData2_SubmittedAtUtc = testItems.Fixture.Create<DateTime>();
            SetupAuditLog(testDataShareRequestModelData2, testDataShareRequestModelData2_CreatedAtUtc, testDataShareRequestModelData2_SubmittedAtUtc);

            var testDataShareRequestModelData3_CreatedAtUtc = testItems.Fixture.Create<DateTime>();
            var testDataShareRequestModelData3_SubmittedAtUtc = testItems.Fixture.Create<DateTime>();
            SetupAuditLog(testDataShareRequestModelData3, testDataShareRequestModelData3_CreatedAtUtc, testDataShareRequestModelData3_SubmittedAtUtc);

            // -- Set up user creating acquirer contact details
            SetupDataShareRequestAcquirerContactDetails(200, "test user 200", "testuser200@testemail.com");
            SetupDataShareRequestAcquirerContactDetails(300, "test user 300", "testuser300@testemail.com");

            testItems.MockServiceOperationResultFactory.Setup(x => x.CreateSuccessfulDataResult(It.IsAny<DataShareRequestRaisedForEsdaByAcquirerOrganisationSummarySet>(), It.IsAny<HttpStatusCode?>()))
                .Returns((DataShareRequestRaisedForEsdaByAcquirerOrganisationSummarySet summarySet, HttpStatusCode? _) => CreateTestSuccessfulDataResult(summarySet));

            var result = await testItems.AcquirerDataShareRequestService.GetDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisationAsync(
                testEsdaId);

            Assert.Multiple(() =>
            {
                Assert.That(result.Success, Is.True);
                Assert.That(result.Error, Is.Null);
                Assert.That(result.Data!.DataShareRequestRaisedForEsdaByAcquirerOrganisationSummaries.Count == 3);

                Assert.That(result.Data!.DataShareRequestRaisedForEsdaByAcquirerOrganisationSummaries.Any(x => 
                    x.Id == testDataShareRequestModelData1.DataShareRequest_Id &&
                    x.RequestId == testDataShareRequestModelData1.DataShareRequest_RequestId &&
                    x.Status == ProvisionToLocalStatusType(testDataShareRequestModelData1.DataShareRequest_RequestStatus) &&
                    x.DateStarted == testDataShareRequestModelData1_CreatedAtUtc &&
                    x.DateSubmitted == testDataShareRequestModelData1_SubmittedAtUtc &&
                    x.OriginatingAcquirerContactDetails is {UserName: "test user 200", EmailAddress: "testuser200@testemail.com"}));

                Assert.That(result.Data!.DataShareRequestRaisedForEsdaByAcquirerOrganisationSummaries.Any(x =>
                    x.Id == testDataShareRequestModelData2.DataShareRequest_Id &&
                    x.RequestId == testDataShareRequestModelData2.DataShareRequest_RequestId &&
                    x.Status == ProvisionToLocalStatusType(testDataShareRequestModelData2.DataShareRequest_RequestStatus) &&
                    x.DateStarted == testDataShareRequestModelData2_CreatedAtUtc &&
                    x.DateSubmitted == testDataShareRequestModelData2_SubmittedAtUtc &&
                    x.OriginatingAcquirerContactDetails is {UserName: "test user 300", EmailAddress: "testuser300@testemail.com"}));

                Assert.That(result.Data!.DataShareRequestRaisedForEsdaByAcquirerOrganisationSummaries.Any(x =>
                    x.Id == testDataShareRequestModelData3.DataShareRequest_Id &&
                    x.RequestId == testDataShareRequestModelData3.DataShareRequest_RequestId &&
                    x.Status == ProvisionToLocalStatusType(testDataShareRequestModelData3.DataShareRequest_RequestStatus) &&
                    x.DateStarted == testDataShareRequestModelData3_CreatedAtUtc &&
                    x.DateSubmitted == testDataShareRequestModelData3_SubmittedAtUtc &&
                    x.OriginatingAcquirerContactDetails is {UserName: "test user 200", EmailAddress: "testuser200@testemail.com"}));
            });

            DataShareRequestStatus ProvisionToLocalStatusType(DataShareRequestStatusType requestStatusInternal)
            {
                return requestStatusInternal switch
                {
                    DataShareRequestStatusType.Draft => DataShareRequestStatus.Draft,
                    DataShareRequestStatusType.Rejected => DataShareRequestStatus.Rejected,
                    DataShareRequestStatusType.Submitted => DataShareRequestStatus.Submitted,
                    _ => throw new Exception("This request status is not in the arranged test data")
                };
            }

            DataShareRequestModelData CreateDataShareRequestModelData(
                int acquirerUserId,
                string requestRequestId,
                DataShareRequestStatusType requestStatus)
            {
                return CreateTestDataShareRequestModelData(
                    dataShareRequestId: testItems.Fixture.Create<Guid>(), dataShareRequestRequestId: requestRequestId, acquirerUserId: acquirerUserId, acquirerOrganisationId: testAcquirerOrganisationId, requestStatus: requestStatus);
            }

            void SetupAuditLog(DataShareRequestModelData dataShareRequestModelData, DateTime createdAtUtc, DateTime? submittedAtUtc)
            {
                testItems.MockAuditLogService.Setup(x => x.GetMostRecentAuditLogForDataShareRequestStatusChangeToStatusAsync(
                        dataShareRequestModelData.DataShareRequest_Id, DataShareRequestStatusType.Draft))
                    .ReturnsAsync(() => CreateTestAuditLogDataShareRequestStatusChangeModelData(changedAtUtc: createdAtUtc));

                testItems.MockAuditLogService.Setup(x => x.GetMostRecentAuditLogForDataShareRequestStatusChangeToStatusAsync(
                        dataShareRequestModelData.DataShareRequest_Id, DataShareRequestStatusType.Submitted))
                    .ReturnsAsync(() =>
                        submittedAtUtc != null
                            ? CreateTestAuditLogDataShareRequestStatusChangeModelData(changedAtUtc: submittedAtUtc)
                            : null);
            }

            void SetupDataShareRequestAcquirerContactDetails(int userId, string userName, string emailAddress)
            {
                var acquirerUserDetails = CreateTestUserDetails(userId: userId, userName: userName, emailAddress: emailAddress);
                testItems.MockUserProfilePresenter.Setup(x => x.GetUserDetailsByUserIdAsync(userId))
                    .ReturnsAsync(() => acquirerUserDetails);
            }

        }
        #endregion

        #region GetDataShareRequestQuestionsSummaryAsync() Tests
        [Test]
        public async Task GivenADataShareRequestId_WhenIGetDataShareRequestQuestionsSummaryAsync_ThenTheQuestionsSummaryForTheDataShareRequestIsReturned()
        {
            var testItems = CreateTestItems();

            var testDataShareRequestId = testItems.Fixture.Create<Guid>();

            var testDataShareRequestQuestionsSummaryModelData = testItems.Fixture.Create<DataShareRequestQuestionsSummaryModelData>();
            var testDataShareRequestQuestionsSummary = testItems.Fixture.Create<DataShareRequestQuestionsSummary>();
            
            testItems.MockAcquirerDataShareRequestRepository.Setup(x => x.GetDataShareRequestQuestionsSummaryAsync(
                    testDataShareRequestId))
                .ReturnsAsync(testDataShareRequestQuestionsSummaryModelData);

            testItems.MockAcquirerDataShareRequestModelDataFactory.Setup(x => x.CreateDataShareRequestQuestionsSummary(
                    testDataShareRequestQuestionsSummaryModelData))
                .Returns(testDataShareRequestQuestionsSummary);

            testItems.MockServiceOperationResultFactory.Setup(x => x.CreateSuccessfulDataResult(It.IsAny<DataShareRequestQuestionsSummary>(), It.IsAny<HttpStatusCode?>()))
                .Returns((DataShareRequestQuestionsSummary questionsSummary, HttpStatusCode? _) => CreateTestSuccessfulDataResult(questionsSummary));

            var result = await testItems.AcquirerDataShareRequestService.GetDataShareRequestQuestionsSummaryAsync(
                testDataShareRequestId);

            Assert.Multiple(() =>
            {
                Assert.That(result.Success, Is.True);
                Assert.That(result.Data, Is.EqualTo(testDataShareRequestQuestionsSummary));
                Assert.That(result.Error, Is.Null);
            });
        }

        [Test]
        public async Task GivenAnExceptionIsThrownWhilstGettingTheDataShareRequestQuestionsSummary_WhenIGetDataShareRequestQuestionsSummaryAsync_ThenAnErrorIsLogged()
        {
            var testItems = CreateTestItems();

            var testException = new Exception("test exception message");

            testItems.MockAcquirerDataShareRequestRepository.Setup(x => x.GetDataShareRequestQuestionsSummaryAsync(
                    It.IsAny<Guid>()))
                .ThrowsAsync(testException);

            await testItems.AcquirerDataShareRequestService.GetDataShareRequestQuestionsSummaryAsync(
                It.IsAny<Guid>());

            testItems.MockLogger.VerifyLog(LogLevel.Error, "Failed to GetDataShareRequestQuestionsSummary", testException);
        }

        [Test]
        public async Task GivenAnExceptionIsThrownWhilstGettingTheDataShareRequestQuestionsSummary_WhenIGetDataShareRequestQuestionsSummaryAsync_ThenAFailedDataResultIsReturnedReportingTheError()
        {
            var testItems = CreateTestItems();

            var testException = new Exception("test exception message");

            testItems.MockAcquirerDataShareRequestRepository.Setup(x => x.GetDataShareRequestQuestionsSummaryAsync(
                    It.IsAny<Guid>()))
                .ThrowsAsync(testException);

            var testFailedDataResult = Mock.Of<IServiceOperationDataResult<DataShareRequestQuestionsSummary>>();
            testItems.MockServiceOperationResultFactory.Setup(x => x.CreateFailedDataResult<DataShareRequestQuestionsSummary>(It.IsAny<string>(), It.IsAny<HttpStatusCode?>()))
                .Returns(() => testFailedDataResult);

            var result = await testItems.AcquirerDataShareRequestService.GetDataShareRequestQuestionsSummaryAsync(
                It.IsAny<Guid>());

            Assert.That(result, Is.SameAs(testFailedDataResult));
        }

        [Test]
        public async Task GivenQuestionsAreInAnAnswerableState_WhenIGetDataShareRequestQuestionsSummaryAsync_ThenQuestionCanBeAnsweredIsTrue(
            [ValueSource(nameof(AnswerableQuestionStatuses))] QuestionStatusType answerableQuestionStatus)
        {
            var testItems = CreateTestItems();

            var testDataShareRequestId = testItems.Fixture.Create<Guid>();

            var testDataShareRequestQuestionsSummaryModelData = testItems.Fixture.Create<DataShareRequestQuestionsSummaryModelData>();
            var testDataShareRequestQuestionsSummary = testItems.Fixture.Create<DataShareRequestQuestionsSummary>();

            var allQuestionSummaries = testDataShareRequestQuestionsSummaryModelData.DataShareRequest_QuestionSetSummary
                .QuestionSet_SectionSummaries.SelectMany(x => x.QuestionSetSection_QuestionSummaries).ToList();

            foreach (var questionSummary in allQuestionSummaries)
            {
                questionSummary.Question_QuestionStatus = answerableQuestionStatus;
            }

            testItems.MockAcquirerDataShareRequestRepository.Setup(x => x.GetDataShareRequestQuestionsSummaryAsync(
                    testDataShareRequestId))
                .ReturnsAsync(testDataShareRequestQuestionsSummaryModelData);

            testItems.MockAcquirerDataShareRequestModelDataFactory.Setup(x => x.CreateDataShareRequestQuestionsSummary(
                    testDataShareRequestQuestionsSummaryModelData))
                .Returns(testDataShareRequestQuestionsSummary);

            testItems.MockServiceOperationResultFactory.Setup(x => x.CreateSuccessfulDataResult(It.IsAny<DataShareRequestQuestionsSummary>(), It.IsAny<HttpStatusCode?>()))
                .Returns((DataShareRequestQuestionsSummary questionsSummary, HttpStatusCode? _) => CreateTestSuccessfulDataResult(questionsSummary));

            await testItems.AcquirerDataShareRequestService.GetDataShareRequestQuestionsSummaryAsync(
                testDataShareRequestId);

            Assert.Multiple(() =>
            {
                foreach (var questionSummary in allQuestionSummaries)
                {
                    Assert.That(questionSummary.Question_QuestionCanBeAnswered, Is.True);
                }
            });
        }

        [Test]
        public async Task GivenQuestionsAreInANonAnswerableState_WhenIGetDataShareRequestQuestionsSummaryAsync_ThenQuestionCanBeAnsweredIsFalse(
            [ValueSource(nameof(NonAnswerableQuestionStatuses))] QuestionStatusType nonAnswerableQuestionStatus)
        {
            var testItems = CreateTestItems();

            var testDataShareRequestId = testItems.Fixture.Create<Guid>();

            var testDataShareRequestQuestionsSummaryModelData = testItems.Fixture.Create<DataShareRequestQuestionsSummaryModelData>();
            var testDataShareRequestQuestionsSummary = testItems.Fixture.Create<DataShareRequestQuestionsSummary>();

            var allQuestionSummaries = testDataShareRequestQuestionsSummaryModelData.DataShareRequest_QuestionSetSummary
                .QuestionSet_SectionSummaries.SelectMany(x => x.QuestionSetSection_QuestionSummaries).ToList();

            foreach (var questionSummary in allQuestionSummaries)
            {
                questionSummary.Question_QuestionStatus = nonAnswerableQuestionStatus;
            }

            testItems.MockAcquirerDataShareRequestRepository.Setup(x => x.GetDataShareRequestQuestionsSummaryAsync(
                    testDataShareRequestId))
                .ReturnsAsync(testDataShareRequestQuestionsSummaryModelData);

            testItems.MockAcquirerDataShareRequestModelDataFactory.Setup(x => x.CreateDataShareRequestQuestionsSummary(
                    testDataShareRequestQuestionsSummaryModelData))
                .Returns(testDataShareRequestQuestionsSummary);

            testItems.MockServiceOperationResultFactory.Setup(x => x.CreateSuccessfulDataResult(It.IsAny<DataShareRequestQuestionsSummary>(), It.IsAny<HttpStatusCode?>()))
                .Returns((DataShareRequestQuestionsSummary questionsSummary, HttpStatusCode? _) => CreateTestSuccessfulDataResult(questionsSummary));

            await testItems.AcquirerDataShareRequestService.GetDataShareRequestQuestionsSummaryAsync(
                testDataShareRequestId);

            Assert.Multiple(() =>
            {
                foreach (var questionSummary in allQuestionSummaries)
                {
                    Assert.That(questionSummary.Question_QuestionCanBeAnswered, Is.False);
                }
            });
        }

        [Test]
        public async Task GivenAQuestionsSummaryModel_WhenIGetDataShareRequestQuestionsSummaryAsync_ThenTheCompletenessStateOfEachSectionInTheModelIsDetermined()
        {
            var testItems = CreateTestItems();

            var testDataShareRequestId = testItems.Fixture.Create<Guid>();

            var testDataShareRequestQuestionsSummaryModelData = testItems.Fixture.Create<DataShareRequestQuestionsSummaryModelData>();
            testDataShareRequestQuestionsSummaryModelData.DataShareRequest_QuestionSetSummary
                .QuestionSet_SectionSummaries = testItems.Fixture.CreateMany<QuestionSetSectionSummaryModelData>(4).ToList();

            testItems.MockAcquirerDataShareRequestRepository.Setup(x => x.GetDataShareRequestQuestionsSummaryAsync(
                    testDataShareRequestId))
                .ReturnsAsync(testDataShareRequestQuestionsSummaryModelData);

            testItems.MockServiceOperationResultFactory.Setup(x => x.CreateSuccessfulDataResult(It.IsAny<DataShareRequestQuestionsSummary>(), It.IsAny<HttpStatusCode?>()))
                .Returns((DataShareRequestQuestionsSummary questionsSummary, HttpStatusCode? _) => CreateTestSuccessfulDataResult(questionsSummary));

            await testItems.AcquirerDataShareRequestService.GetDataShareRequestQuestionsSummaryAsync(
                testDataShareRequestId);

            Assert.Multiple(() =>
            {
                foreach (var sectionSummary in testDataShareRequestQuestionsSummaryModelData.DataShareRequest_QuestionSetSummary.QuestionSet_SectionSummaries)
                {
                    testItems.MockDataShareRequestQuestionSetCompletenessDetermination.Verify(x =>
                            x.DetermineDataShareRequestQuestionSetSectionCompleteness(sectionSummary),
                        Times.Once);
                }
            });
        }

        [Test]
        public async Task GivenAQuestionsSummaryModel_WhenIGetDataShareRequestQuestionsSummaryAsync_ThenTheCompletenessStateOfEachSectionInTheModelIsSet()
        {
            var testItems = CreateTestItems();

            var testDataShareRequestId = testItems.Fixture.Create<Guid>();

            var testQuestionSetSectionSummaries = testItems.Fixture.CreateMany<QuestionSetSectionSummaryModelData>(4).ToList();

            var testDataShareRequestQuestionsSummaryModelData = testItems.Fixture.Create<DataShareRequestQuestionsSummaryModelData>();
            testDataShareRequestQuestionsSummaryModelData.DataShareRequest_QuestionSetSummary.QuestionSet_SectionSummaries = testQuestionSetSectionSummaries;

            SetupQuestionSetSectionCompletenessDeterminationResult(testQuestionSetSectionSummaries[0], 1);
            SetupQuestionSetSectionCompletenessDeterminationResult(testQuestionSetSectionSummaries[1], 0);
            SetupQuestionSetSectionCompletenessDeterminationResult(testQuestionSetSectionSummaries[2], 3);
            SetupQuestionSetSectionCompletenessDeterminationResult(testQuestionSetSectionSummaries[3], 0);

            testItems.MockAcquirerDataShareRequestRepository.Setup(x => x.GetDataShareRequestQuestionsSummaryAsync(
                    testDataShareRequestId))
                .ReturnsAsync(testDataShareRequestQuestionsSummaryModelData);

            testItems.MockServiceOperationResultFactory.Setup(x => x.CreateSuccessfulDataResult(It.IsAny<DataShareRequestQuestionsSummary>(), It.IsAny<HttpStatusCode?>()))
                .Returns((DataShareRequestQuestionsSummary questionsSummary, HttpStatusCode? _) => CreateTestSuccessfulDataResult(questionsSummary));

            await testItems.AcquirerDataShareRequestService.GetDataShareRequestQuestionsSummaryAsync(
                testDataShareRequestId);

            Assert.Multiple(() =>
            {
                Assert.That(testQuestionSetSectionSummaries[0].QuestionSetSection_IsComplete, Is.False);
                Assert.That(testQuestionSetSectionSummaries[1].QuestionSetSection_IsComplete, Is.True);
                Assert.That(testQuestionSetSectionSummaries[2].QuestionSetSection_IsComplete, Is.False);
                Assert.That(testQuestionSetSectionSummaries[3].QuestionSetSection_IsComplete, Is.True);
            });

            void SetupQuestionSetSectionCompletenessDeterminationResult(QuestionSetSectionSummaryModelData questionSetSection, int numberOfQuestionsRequiringAResponse)
            {
                var questionsRequiringAResponse = testItems.Fixture.CreateMany<QuestionSummaryModelData>(numberOfQuestionsRequiringAResponse);

                testItems.MockDataShareRequestQuestionSetCompletenessDetermination.Setup(x => x.DetermineDataShareRequestQuestionSetSectionCompleteness(questionSetSection))
                    .Returns(() => testItems.Fixture.Build<DataShareRequestQuestionSetSectionCompletionDeterminationResult>()
                        .With(x => x.QuestionsRequiringAResponse, questionsRequiringAResponse)
                        .Create());
            }
        }
        #endregion

        #region GetDataShareRequestQuestionInformationAsync() Tests
        [Test]
        public async Task GivenADataShareRequestIdAndQuestionId_WhenIGetDataShareRequestQuestionInformationAsync_ThenTheQuestionsInformationForTheDataShareRequestAndQuestionIsReturned()
        {
            var testItems = CreateTestItems();

            var testDataShareRequestId = testItems.Fixture.Create<Guid>();
            var testQuestionId = testItems.Fixture.Create<Guid>();

            var testDataShareRequestQuestionModelData = testItems.Fixture.Create<DataShareRequestQuestionModelData>();
            var testDataShareRequestQuestion = testItems.Fixture.Create<DataShareRequestQuestion>();

            testItems.MockAcquirerDataShareRequestRepository.Setup(x => x.GetDataShareRequestQuestionAsync(
                    testDataShareRequestId,
                    testQuestionId))
                .ReturnsAsync(testDataShareRequestQuestionModelData);

            testItems.MockAcquirerDataShareRequestModelDataFactory.Setup(x => x.CreateDataShareRequestQuestion(
                    testDataShareRequestQuestionModelData))
                .Returns(testDataShareRequestQuestion);

            testItems.MockServiceOperationResultFactory.Setup(x => x.CreateSuccessfulDataResult(It.IsAny<DataShareRequestQuestion>(), It.IsAny<HttpStatusCode?>()))
                .Returns((DataShareRequestQuestion question, HttpStatusCode? _) => CreateTestSuccessfulDataResult(question));

            var result = await testItems.AcquirerDataShareRequestService.GetDataShareRequestQuestionInformationAsync(
                testDataShareRequestId,
                testQuestionId);

            Assert.Multiple(() =>
            {
                Assert.That(result.Success, Is.True);
                Assert.That(result.Data, Is.EqualTo(testDataShareRequestQuestion));
                Assert.That(result.Error, Is.Null);
            });
        }


        [Test]
        public async Task GivenADataShareRequestIdAndQuestionId_WhenIGetDataShareRequestQuestionInformationAsync_ThenPlaceHoldersAreReplacedInTheQuestionModel()
        {
            var testItems = CreateTestItems();

            var testDataShareRequestId = testItems.Fixture.Create<Guid>();
            var testQuestionId = testItems.Fixture.Create<Guid>();

            var testDataShareRequestQuestionModelData = testItems.Fixture.Build<DataShareRequestQuestionModelData>()
                .With(x => x.DataShareRequestQuestion_DataShareRequestId, testDataShareRequestId)
                .Create();

            var testEsdaName = testItems.Fixture.Create<string>();

            testItems.MockAcquirerDataShareRequestRepository.Setup(x => x.GetDataShareRequestResourceNameAsync(
                    testDataShareRequestId))
                .ReturnsAsync(() => testEsdaName);

            testItems.MockAcquirerDataShareRequestRepository.Setup(x => x.GetDataShareRequestQuestionAsync(
                    testDataShareRequestId,
                    testQuestionId))
                .ReturnsAsync(testDataShareRequestQuestionModelData);

            testItems.MockServiceOperationResultFactory.Setup(x => x.CreateSuccessfulDataResult(It.IsAny<DataShareRequestQuestion>(), It.IsAny<HttpStatusCode?>()))
                .Returns((DataShareRequestQuestion question, HttpStatusCode? _) => CreateTestSuccessfulDataResult(question));

            await testItems.AcquirerDataShareRequestService.GetDataShareRequestQuestionInformationAsync(
                testDataShareRequestId,
                testQuestionId);

            testItems.MockQuestionSetPlaceholderReplacementService.Verify(x => x.ReplacePlaceholderDataInQuestionModelData(
                    testDataShareRequestQuestionModelData,
                    testEsdaName),
                Times.Once);
        }

        [Test]
        public async Task GivenAnExceptionIsThrownWhilstGettingTheDataShareRequestQuestionInformation_WhenIGetDataShareRequestQuestionInformationAsync_ThenAnErrorIsLogged()
        {
            var testItems = CreateTestItems();

            var testException = new Exception("test exception message");

            testItems.MockAcquirerDataShareRequestRepository.Setup(x => x.GetDataShareRequestQuestionAsync(
                    It.IsAny<Guid>(), It.IsAny<Guid>()))
                .ThrowsAsync(testException);

            await testItems.AcquirerDataShareRequestService.GetDataShareRequestQuestionInformationAsync(
                It.IsAny<Guid>(), It.IsAny<Guid>());

            testItems.MockLogger.VerifyLog(LogLevel.Error, "Failed to GetDataShareRequestQuestionInformation", testException);
        }

        [Test]
        public async Task GivenAnExceptionIsThrownWhilstGettingTheDataShareRequestQuestionInformation_WhenIGetDataShareRequestQuestionInformationAsync_ThenAFailedDataResultIsReturnedReportingTheError()
        {
            var testItems = CreateTestItems();

            var testException = new Exception("test exception message");

            testItems.MockAcquirerDataShareRequestRepository.Setup(x => x.GetDataShareRequestQuestionAsync(
                    It.IsAny<Guid>(), It.IsAny<Guid>()))
                .ThrowsAsync(testException);

            var testFailedDataResult = Mock.Of<IServiceOperationDataResult<DataShareRequestQuestion>>();
            testItems.MockServiceOperationResultFactory.Setup(x => x.CreateFailedDataResult<DataShareRequestQuestion>(It.IsAny<string>(), It.IsAny<HttpStatusCode?>()))
                .Returns(() => testFailedDataResult);

            var result = await testItems.AcquirerDataShareRequestService.GetDataShareRequestQuestionInformationAsync(
                It.IsAny<Guid>(), It.IsAny<Guid>());

            Assert.That(result, Is.SameAs(testFailedDataResult));
        }
        #endregion

        #region SetDataShareRequestQuestionAnswerAsync() Tests
        [Test]
        public async Task GivenAnAnswerForADataShareRequestThatIsNotInASubmittableStatus_WhenISetDataShareRequestQuestionAnswerAsync_ThenAFailedResponseIsReturnedReportingTheError(
            [ValueSource(nameof(NonSubmittableDataShareRequestStatusTypes))] DataShareRequestStatusType dataShareRequestStatusType)
        {
            var testItems = CreateTestItems();

            testItems.MockAcquirerDataShareRequestRepository.Setup(x => x.GetDataShareRequestStatusAsync(
                    It.IsAny<Guid>()))
                .ReturnsAsync(() =>
                    testItems.Fixture.Build<DataShareRequestStatusTypeModelData>()
                        .With(x => x.DataShareRequestStatus_RequestStatus, dataShareRequestStatusType)
                        .Create());

            testItems.MockServiceOperationResultFactory.Setup(x => x.CreateFailedDataResult<SetDataShareRequestQuestionAnswerResult>(It.IsAny<string>(), It.IsAny<HttpStatusCode?>()))
                .Returns((string errorMessage, HttpStatusCode? _) => CreateTestFailedDataResult<SetDataShareRequestQuestionAnswerResult>(errorMessage));

            var result = await testItems.AcquirerDataShareRequestService.SetDataShareRequestQuestionAnswerAsync(
                CreateTestDataShareRequestQuestionAnswer(testItems));

            Assert.Multiple(() =>
            {
                Assert.That(result.Success, Is.False);
                Assert.That(result.Error, Is.EqualTo($"DataShareRequest status does not allow Question Answers to be set: '{dataShareRequestStatusType}'"));
            });
        }

        [Test]
        public async Task GivenAnAnswerWithMultiplePartResponsesWithOrderOne_WhenISetDataShareRequestQuestionAnswerAsync_ThenAnInconsistentDataExceptionIsThrown()
        {
            var testItems = CreateTestItems();

            ReportDataShareRequestAsHavingRequestStatus(testItems);

            var testDataShareRequestQuestionAnswer = CreateTestDataShareRequestQuestionAnswer(testItems);

            foreach (var answerPartResponse in testDataShareRequestQuestionAnswer.AnswerParts[0].AnswerPartResponses)
            {
                answerPartResponse.OrderWithinAnswerPart = 1;
            }

            var testFailedDataResult = Mock.Of<IServiceOperationDataResult<SetDataShareRequestQuestionAnswerResult>>();
            testItems.MockServiceOperationResultFactory.Setup(x => x.CreateFailedDataResult<SetDataShareRequestQuestionAnswerResult>(
                    "At least one response in each answer part must have an order value of 1", It.IsAny<HttpStatusCode?>()))
                .Returns(() => testFailedDataResult);

            var result = await testItems.AcquirerDataShareRequestService.SetDataShareRequestQuestionAnswerAsync(testDataShareRequestQuestionAnswer);

            Assert.That(result, Is.SameAs(testFailedDataResult));
        }

        [Test]
        public async Task GivenSettingDataShareQuestionAnswerInTheRepositoryWillThrowAnException_WhenISetDataShareRequestQuestionAnswerAsync_ThenAFailedResponseIsReturnedReportingTheError()
        {
            var testItems = CreateTestItems();

            ReportDataShareRequestAsHavingRequestStatus(testItems);

            testItems.MockAcquirerDataShareRequestRepository.Setup(x => x.SetDataShareRequestQuestionAnswerAsync(
                    It.IsAny<DataShareRequestQuestionAnswerWriteModelData>()))
                .ThrowsAsync(new Exception("test exception message"));

            testItems.MockServiceOperationResultFactory.Setup(x => x.CreateFailedDataResult<SetDataShareRequestQuestionAnswerResult>(It.IsAny<string>(), It.IsAny<HttpStatusCode?>()))
                .Returns((string errorMessage, HttpStatusCode? _) => CreateTestFailedDataResult<SetDataShareRequestQuestionAnswerResult>(errorMessage));

            var result = await testItems.AcquirerDataShareRequestService.SetDataShareRequestQuestionAnswerAsync(
                CreateTestDataShareRequestQuestionAnswer(testItems));

            Assert.Multiple(() =>
            {
                Assert.That(result.Success, Is.False);
                Assert.That(result.Error, Is.EqualTo("test exception message"));
            });
        }

        [Test]
        public async Task GivenSettingDataShareQuestionAnswerInTheRepositoryWillThrowAnException_WhenISetDataShareRequestQuestionAnswerAsync_ThenAnErrorIsLogged()
        {
            var testItems = CreateTestItems();

            ReportDataShareRequestAsHavingRequestStatus(testItems);

            var testException = new Exception("test exception message");
            testItems.MockAcquirerDataShareRequestRepository.Setup(x => x.SetDataShareRequestQuestionAnswerAsync(
                    It.IsAny<DataShareRequestQuestionAnswerWriteModelData>()))
                .ThrowsAsync(testException);

            testItems.MockServiceOperationResultFactory.Setup(x => x.CreateFailedDataResult<SetDataShareRequestQuestionAnswerResult>(It.IsAny<string>(), It.IsAny<HttpStatusCode?>()))
                .Returns((string errorMessage, HttpStatusCode? _) => CreateTestFailedDataResult<SetDataShareRequestQuestionAnswerResult>(errorMessage));

            await testItems.AcquirerDataShareRequestService.SetDataShareRequestQuestionAnswerAsync(
                CreateTestDataShareRequestQuestionAnswer(testItems));

            testItems.MockLogger.VerifyLog(LogLevel.Error, "Failed to SetDataShareRequestQuestionAnswer", testException);
        }

        [Test]
        public async Task GivenAValidDataShareRequestQuestionAnswer_WhenISetDataShareRequestQuestionAnswerAsync_ThenTheAnswerIsSetInTheRepository()
        {
            var testItems = CreateTestItems();

            ReportDataShareRequestAsHavingRequestStatus(testItems);

            var testDataShareRequestQuestionAnswer = CreateTestDataShareRequestQuestionAnswer(testItems);
            var testDataShareRequestQuestionAnswerWriteModelData = testItems.Fixture.Create<DataShareRequestQuestionAnswerWriteModelData>();

            testItems.MockAcquirerDataShareRequestModelDataFactory.Setup(x => x.CreateQuestionAnswerWriteData(testDataShareRequestQuestionAnswer))
                .Returns(() => testDataShareRequestQuestionAnswerWriteModelData);

            testItems.MockServiceOperationResultFactory.Setup(x => x.CreateSuccessfulDataResult(It.IsAny<SetDataShareRequestQuestionAnswerResult>(), It.IsAny<HttpStatusCode?>()))
                .Returns((SetDataShareRequestQuestionAnswerResult setResult, HttpStatusCode? _) => CreateTestSuccessfulDataResult(setResult));

            await testItems.AcquirerDataShareRequestService.SetDataShareRequestQuestionAnswerAsync(
                testDataShareRequestQuestionAnswer);

            testItems.MockAcquirerDataShareRequestRepository.Verify(x => x.SetDataShareRequestQuestionAnswerAsync(
                    testDataShareRequestQuestionAnswerWriteModelData),
                Times.Once);
        }

        [Test]
        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration", Justification = "Multiple enumeration in mock verification")]
        public async Task GivenAValidDataShareRequestQuestionAnswer_WhenISetDataShareRequestQuestionAnswerAsync_ThenTheQuestionStatusesForTheQuestionsThatHaveAnUpdatedStatusInTheDataShareRequestAreUpdated()
        {
            var testItems = CreateTestItems();

            ReportDataShareRequestAsHavingRequestStatus(testItems);

            var testDataShareRequestId = Guid.Parse("7172D529-DADA-48D4-8D55-CCC47E81E993");
            var testDataShareRequestQuestionAnswer = CreateTestDataShareRequestQuestionAnswer(testItems, dataShareRequestId: testDataShareRequestId);

            testItems.MockServiceOperationResultFactory.Setup(x => x.CreateSuccessfulDataResult(It.IsAny<SetDataShareRequestQuestionAnswerResult>(), It.IsAny<HttpStatusCode?>()))
                .Returns((SetDataShareRequestQuestionAnswerResult setResult, HttpStatusCode? _) => CreateTestSuccessfulDataResult(setResult));

            var testDataShareRequestQuestionStatusInformationSetModelData = new DataShareRequestQuestionStatusInformationSetModelData();
            testItems.MockAcquirerDataShareRequestRepository.Setup(x => x.GetDataShareRequestQuestionStatusInformationsAsync(testDataShareRequestId))
                .ReturnsAsync(() => testDataShareRequestQuestionStatusInformationSetModelData);

            var mockDataShareRequestQuestionStatusesDeterminationResult = CreateTestDataShareRequestQuestionStatusesDeterminationResult(
                dataShareRequestQuestionStatusDeterminationResults:
                [
                    CreateTestDataShareRequestQuestionStatusDeterminationResult(Guid.Parse("B25BF52B-7D8F-405C-8FE8-7BFFCB703B16"), QuestionStatusType.CannotStartYet, QuestionStatusType.NotSet),
                    CreateTestDataShareRequestQuestionStatusDeterminationResult(Guid.Parse("54878B2E-1649-4245-B3DE-8ACBB68EAC9D"), QuestionStatusType.NoResponseNeeded, QuestionStatusType.NotSet),
                    CreateTestDataShareRequestQuestionStatusDeterminationResult(Guid.Parse("45AACDD1-664A-42E0-BAFB-DF2231E591AA"), QuestionStatusType.Completed, QuestionStatusType.Completed)
                ]);

            testItems.MockDataShareRequestQuestionStatusesDetermination.Setup(x => x.DetermineQuestionStatuses(testDataShareRequestQuestionStatusInformationSetModelData))
                .Returns(mockDataShareRequestQuestionStatusesDeterminationResult);

            await testItems.AcquirerDataShareRequestService.SetDataShareRequestQuestionAnswerAsync(
                testDataShareRequestQuestionAnswer);

            Assert.Multiple(() =>
            {
                testItems.MockAcquirerDataShareRequestRepository.Verify(x =>
                        x.UpdateDataShareRequestQuestionStatusesAsync(It.IsAny<Guid>(), It.IsAny<bool>(), It.IsAny<IEnumerable<IDataShareRequestQuestionStatusDataModel>>()),
                    Times.Once);

                testItems.MockAcquirerDataShareRequestRepository.Verify(x =>
                        x.UpdateDataShareRequestQuestionStatusesAsync(
                            testDataShareRequestId,
                            It.IsAny<bool>(),
                            It.Is<IEnumerable<IDataShareRequestQuestionStatusDataModel>>(questionStatuses =>
                                questionStatuses.Count() == 2 &&
                                questionStatuses.Any(questionStatus => questionStatus.QuestionId == Guid.Parse("B25BF52B-7D8F-405C-8FE8-7BFFCB703B16") && questionStatus.QuestionStatus == QuestionStatusType.CannotStartYet) &&
                                questionStatuses.Any(questionStatus => questionStatus.QuestionId == Guid.Parse("54878B2E-1649-4245-B3DE-8ACBB68EAC9D") && questionStatus.QuestionStatus == QuestionStatusType.NoResponseNeeded)
                            )),
                    Times.Once);
            });
        }

        [Theory]
        public async Task GivenAValidDataShareRequestQuestionAnswer_WhenISetDataShareRequestQuestionAnswerAsync_ThenTheQuestionsRemainThatRequireAResponseIsSetForTheDataShareRequest(
            bool questionsRemainThatRequireAResponse)
        {
            var testItems = CreateTestItems();

            ReportDataShareRequestAsHavingRequestStatus(testItems);

            var testDataShareRequestId = Guid.Parse("7172D529-DADA-48D4-8D55-CCC47E81E993");
            var testDataShareRequestQuestionAnswer = CreateTestDataShareRequestQuestionAnswer(testItems, dataShareRequestId: testDataShareRequestId);

            testItems.MockServiceOperationResultFactory.Setup(x => x.CreateSuccessfulDataResult(It.IsAny<SetDataShareRequestQuestionAnswerResult>(), It.IsAny<HttpStatusCode?>()))
                .Returns((SetDataShareRequestQuestionAnswerResult setResult, HttpStatusCode? _) => CreateTestSuccessfulDataResult(setResult));

            var mockDataShareRequestQuestionStatusesDeterminationResult = CreateTestDataShareRequestQuestionStatusesDeterminationResult(
                questionsRemainThatRequireAResponse: questionsRemainThatRequireAResponse);

            testItems.MockDataShareRequestQuestionStatusesDetermination.Setup(x => x.DetermineQuestionStatuses(It.IsAny<DataShareRequestQuestionStatusInformationSetModelData>()))
                .Returns(mockDataShareRequestQuestionStatusesDeterminationResult);

            await testItems.AcquirerDataShareRequestService.SetDataShareRequestQuestionAnswerAsync(
                testDataShareRequestQuestionAnswer);

            Assert.Multiple(() =>
            {
                testItems.MockAcquirerDataShareRequestRepository.Verify(x =>
                        x.UpdateDataShareRequestQuestionStatusesAsync(It.IsAny<Guid>(), It.IsAny<bool>(), It.IsAny<IEnumerable<IDataShareRequestQuestionStatusDataModel>>()),
                    Times.Once);

                testItems.MockAcquirerDataShareRequestRepository.Verify(x =>
                        x.UpdateDataShareRequestQuestionStatusesAsync(
                            testDataShareRequestId,
                            questionsRemainThatRequireAResponse,
                            It.IsAny<IEnumerable<IDataShareRequestQuestionStatusDataModel>>()),
                    Times.Once);
            });
        }

        [Test]
        public async Task GivenAnInvalidValidDataShareRequestQuestionAnswer_WhenISetDataShareRequestQuestionAnswerAsync_ThenTheDeterminedAnswerValidationErrorsAreReturned()
        {
            var testItems = CreateTestItems();

            ReportDataShareRequestAsHavingRequestStatus(testItems);

            var testDataShareRequestQuestionAnswer = CreateTestDataShareRequestQuestionAnswer(testItems);

            var testQuestionAnswerValidationResult = CreateTestDataShareRequestQuestionAnswerValidationResult(
            [
                new SetDataShareRequestQuestionAnswerPartResponseValidationError
                {
                    QuestionPartId = Guid.Parse("37C9761B-47C3-414E-8703-DD3E54F1CDA8"),
                    ResponseOrderWithinAnswerPart = 1,
                    ValidationErrors = ["some error", "another error"]
                },
                new SetDataShareRequestQuestionAnswerPartResponseValidationError
                {
                    QuestionPartId = Guid.Parse("37C9761B-47C3-414E-8703-DD3E54F1CDA8"),
                    ResponseOrderWithinAnswerPart = 2,
                    ValidationErrors = ["same error on another response"]
                },
                new SetDataShareRequestQuestionAnswerPartResponseValidationError
                {
                    QuestionPartId = Guid.Parse("8C46B701-E004-4A60-91B0-36727EE3CAA2"),
                    ResponseOrderWithinAnswerPart = 99,
                    ValidationErrors = ["just the one error"]
                },
            ]);

            testItems.MockDataShareRequestQuestionAnswerValidationService.Setup(x => x.ValidateDataShareRequestQuestionAnswerAsync(
                    testDataShareRequestQuestionAnswer))
                .ReturnsAsync(() => testQuestionAnswerValidationResult);

            testItems.MockServiceOperationResultFactory.Setup(x => x.CreateSuccessfulDataResult(It.IsAny<SetDataShareRequestQuestionAnswerResult>(), It.IsAny<HttpStatusCode?>()))
                .Returns((SetDataShareRequestQuestionAnswerResult setResult, HttpStatusCode? _) => CreateTestSuccessfulDataResult(setResult));

            var result = await testItems.AcquirerDataShareRequestService.SetDataShareRequestQuestionAnswerAsync(
                testDataShareRequestQuestionAnswer);

            Assert.Multiple(() =>
            {
                Assert.That(result.Success, Is.True);

                Assert.That(result.Data!.NextQuestionId, Is.Null);

                Assert.That(result.Data!.AnswerIsValid, Is.False);
            });
        }

        [Test]
        public async Task GivenAValidDataShareRequestQuestionAnswer_WhenISetDataShareRequestQuestionAnswerAsync_ThenTheIdOfTheNextAnswerableQuestionInTheSetIsReturned()
        {
            var testItems = CreateTestItems();

            ReportDataShareRequestAsHavingRequestStatus(testItems);

            var testQuestionId = Guid.Parse("D6A902EB-BAFF-4420-B950-DFB9E1CFF6DC");

            var dataShareRequestQuestionAnswer = CreateTestDataShareRequestQuestionAnswer(testItems, questionId: testQuestionId);

            var mockDataShareRequestQuestionStatusesDeterminationResult = CreateTestDataShareRequestQuestionStatusesDeterminationResult(
                dataShareRequestQuestionStatusDeterminationResults:
                [
                    CreateTestDataShareRequestQuestionStatusDeterminationResult(Guid.Parse("B25BF52B-7D8F-405C-8FE8-7BFFCB703B16"), QuestionStatusType.CannotStartYet, QuestionStatusType.NotSet),
                    CreateTestDataShareRequestQuestionStatusDeterminationResult(Guid.Parse("54878B2E-1649-4245-B3DE-8ACBB68EAC9D"), QuestionStatusType.NoResponseNeeded, QuestionStatusType.NotSet),
                    CreateTestDataShareRequestQuestionStatusDeterminationResult(Guid.Parse("45AACDD1-664A-42E0-BAFB-DF2231E591AA"), QuestionStatusType.Completed, QuestionStatusType.Completed)
                ]);

            var testDataShareRequestQuestionStatusInformationSetModelData = new DataShareRequestQuestionStatusInformationSetModelData();
            testItems.MockAcquirerDataShareRequestRepository.Setup(x => x.GetDataShareRequestQuestionStatusInformationsAsync(It.IsAny<Guid>()))
                .ReturnsAsync(() => testDataShareRequestQuestionStatusInformationSetModelData);

            testItems.MockDataShareRequestQuestionStatusesDetermination.Setup(x => x.DetermineQuestionStatuses(testDataShareRequestQuestionStatusInformationSetModelData))
                .Returns(mockDataShareRequestQuestionStatusesDeterminationResult);

            var questionStatusData = mockDataShareRequestQuestionStatusesDeterminationResult.QuestionStatusDeterminationResults
                .Select(x => x.QuestionSetQuestionStatusData).ToList();

            var testNextQuestionId = Guid.Parse("FDE359F8-F300-4F63-8FAF-63AFBA1660D1");
            testItems.MockNextQuestionDetermination.Setup(x => x.DetermineNextQuestion(
                    testQuestionId,
                    questionStatusData))
                .Returns(testNextQuestionId);

            testItems.MockServiceOperationResultFactory.Setup(x => x.CreateSuccessfulDataResult(It.IsAny<SetDataShareRequestQuestionAnswerResult>(), It.IsAny<HttpStatusCode?>()))
                .Returns((SetDataShareRequestQuestionAnswerResult setResult, HttpStatusCode? _) => CreateTestSuccessfulDataResult(setResult));
            
            var result = await testItems.AcquirerDataShareRequestService.SetDataShareRequestQuestionAnswerAsync(
                dataShareRequestQuestionAnswer);

            Assert.Multiple(() =>
            {
                Assert.That(result.Success, Is.True);

                Assert.That(result.Data!.NextQuestionId, Is.EqualTo(testNextQuestionId));

                Assert.That(result.Data!.AnswerIsValid, Is.True);
            });
        }

        [Test]
        public async Task GivenAValidDataShareRequestQuestionAnswerForTheFinalQuestionInTheSet_WhenISetDataShareRequestQuestionAnswerAsync_ThenANullNextAnswerableQuestionIdsReturned()
        {
            var testItems = CreateTestItems();

            ReportDataShareRequestAsHavingRequestStatus(testItems);

            var testQuestionId = Guid.Parse("D6A902EB-BAFF-4420-B950-DFB9E1CFF6DC");

            var dataShareRequestQuestionAnswer = CreateTestDataShareRequestQuestionAnswer(testItems, questionId: testQuestionId);

            var mockDataShareRequestQuestionStatusesDeterminationResult = CreateTestDataShareRequestQuestionStatusesDeterminationResult(
                dataShareRequestQuestionStatusDeterminationResults:
                [
                    CreateTestDataShareRequestQuestionStatusDeterminationResult(Guid.Parse("B25BF52B-7D8F-405C-8FE8-7BFFCB703B16"), QuestionStatusType.CannotStartYet, QuestionStatusType.NotSet),
                    CreateTestDataShareRequestQuestionStatusDeterminationResult(Guid.Parse("54878B2E-1649-4245-B3DE-8ACBB68EAC9D"), QuestionStatusType.NoResponseNeeded, QuestionStatusType.NotSet),
                    CreateTestDataShareRequestQuestionStatusDeterminationResult(Guid.Parse("45AACDD1-664A-42E0-BAFB-DF2231E591AA"), QuestionStatusType.Completed, QuestionStatusType.Completed)
                ]);

            var testDataShareRequestQuestionStatusInformationSetModelData = new DataShareRequestQuestionStatusInformationSetModelData();
            testItems.MockAcquirerDataShareRequestRepository.Setup(x => x.GetDataShareRequestQuestionStatusInformationsAsync(It.IsAny<Guid>()))
                .ReturnsAsync(() => testDataShareRequestQuestionStatusInformationSetModelData);

            testItems.MockDataShareRequestQuestionStatusesDetermination.Setup(x => x.DetermineQuestionStatuses(testDataShareRequestQuestionStatusInformationSetModelData))
                .Returns(mockDataShareRequestQuestionStatusesDeterminationResult);

            var questionStatusData = mockDataShareRequestQuestionStatusesDeterminationResult.QuestionStatusDeterminationResults
                .Select(x => x.QuestionSetQuestionStatusData).ToList();

            testItems.MockNextQuestionDetermination.Setup(x => x.DetermineNextQuestion(
                    testQuestionId,
                    questionStatusData))
                .Returns((Guid?)null);

            testItems.MockServiceOperationResultFactory.Setup(x => x.CreateSuccessfulDataResult(It.IsAny<SetDataShareRequestQuestionAnswerResult>(), It.IsAny<HttpStatusCode?>()))
                .Returns((SetDataShareRequestQuestionAnswerResult setResult, HttpStatusCode? _) => CreateTestSuccessfulDataResult(setResult));

            var result = await testItems.AcquirerDataShareRequestService.SetDataShareRequestQuestionAnswerAsync(
                dataShareRequestQuestionAnswer);

            Assert.Multiple(() =>
            {
                Assert.That(result.Success, Is.True);

                Assert.That(result.Data!.NextQuestionId, Is.Null);

                Assert.That(result.Data!.AnswerIsValid, Is.True);
            });
        }
        #endregion

        #region SubmitDataShareRequestAsync() Tests
        [Test]
        public async Task GivenARequestingUserId_WhenISubmitDataShareRequestAsync_ThenTheStatusOfTheDataShareRequestIsVerifiedUsingTheGivenRequestingUserId()
        {
            var testItems = CreateTestItems();

            ReportDataShareRequestAsBeingInDraftStatusAndReadyForSubmission(testItems);

            var testUserDetails = CreateTestUserDetails(userName: "test user name");
            testItems.MockUserProfilePresenter.Setup(x => x.GetInitiatingUserDetailsAsync())
                .ReturnsAsync(() => testUserDetails);

            var dataShareRequestId = testItems.Fixture.Create<Guid>();

            await testItems.AcquirerDataShareRequestService.SubmitDataShareRequestAsync(
                dataShareRequestId);

            testItems.MockAcquirerDataShareRequestRepository.Verify(x => x.SubmitDataShareRequestAsync(
                    testUserDetails.UserIdSet,
                    dataShareRequestId),
                Times.Once);
        }

        [Test]
        public async Task GivenAnAnswerForADataShareRequestThatIsNotInASubmittableStatus_WhenISubmitDataShareRequestAsync_ThenAFailedResponseIsReturnedReportingTheError(
            [ValueSource(nameof(NonSubmittableDataShareRequestStatusTypes))] DataShareRequestStatusType dataShareRequestStatusType)
        {
            var testItems = CreateTestItems();

            testItems.MockAcquirerDataShareRequestRepository.Setup(x => x.GetDataShareRequestStatusAsync(
                     It.IsAny<Guid>()))
                .ReturnsAsync(() => testItems.Fixture.Build<DataShareRequestStatusTypeModelData>()
                    .With(x => x.DataShareRequestStatus_QuestionsRemainThatRequireAResponse, false)
                    .With(x => x.DataShareRequestStatus_RequestStatus, dataShareRequestStatusType).Create());

            testItems.MockServiceOperationResultFactory.Setup(x => x.CreateFailedDataResult<DataShareRequestSubmissionResult>(It.IsAny<string>(), It.IsAny<HttpStatusCode?>()))
                .Returns((string errorMessage, HttpStatusCode? _) => CreateTestFailedDataResult<DataShareRequestSubmissionResult>(errorMessage));

            var result = await testItems.AcquirerDataShareRequestService.SubmitDataShareRequestAsync(
                It.IsAny<Guid>());

            Assert.Multiple(() =>
            {
                Assert.That(result.Success, Is.False);
                Assert.That(result.Error, Is.EqualTo($"DataShareRequest status does not allow it to be submitted: '{dataShareRequestStatusType}'"));
            });
        }

        [Test]
        public async Task GivenAnAnswerForADataShareRequestWithQuestionsThatRequireAResponse_WhenISubmitDataShareRequestAsync_ThenAFailedResponseIsReturnedReportingTheError()
        {
            var testItems = CreateTestItems();

            testItems.MockAcquirerDataShareRequestRepository.Setup(x => x.GetDataShareRequestStatusAsync(
                    It.IsAny<Guid>()))
                .ReturnsAsync(() => testItems.Fixture.Build<DataShareRequestStatusTypeModelData>()
                    .With(x => x.DataShareRequestStatus_QuestionsRemainThatRequireAResponse, true)
                    .With(x => x.DataShareRequestStatus_RequestStatus, DataShareRequestStatusType.Draft)
                    .Create());

            testItems.MockServiceOperationResultFactory.Setup(x => x.CreateFailedDataResult<DataShareRequestSubmissionResult>(It.IsAny<string>(), It.IsAny<HttpStatusCode?>()))
                .Returns((string errorMessage, HttpStatusCode? _) => CreateTestFailedDataResult<DataShareRequestSubmissionResult>(errorMessage));

            var result = await testItems.AcquirerDataShareRequestService.SubmitDataShareRequestAsync(
                It.IsAny<Guid>());

            Assert.Multiple(() =>
            {
                Assert.That(result.Success, Is.False);
                Assert.That(result.Error, Is.EqualTo("DataShareRequest has questions that require a response"));
            });
        }

        [Test]
        public async Task GivenSubmittingADataShareRequestInTheRepositoryWillThrowAnException_WhenISubmitDataShareRequestAsync_ThenAFailedResponseIsReturnedReportingTheError()
        {
            var testItems = CreateTestItems();

            ReportDataShareRequestAsBeingInDraftStatusAndReadyForSubmission(testItems);

            testItems.MockAcquirerDataShareRequestRepository.Setup(x => x.SubmitDataShareRequestAsync(
                    It.IsAny<IUserIdSet>(), It.IsAny<Guid>()))
                .ThrowsAsync(new Exception("test exception message"));

            testItems.MockServiceOperationResultFactory.Setup(x => x.CreateFailedDataResult<DataShareRequestSubmissionResult>(It.IsAny<string>(), It.IsAny<HttpStatusCode?>()))
                .Returns((string errorMessage, HttpStatusCode? _) => CreateTestFailedDataResult<DataShareRequestSubmissionResult>(errorMessage));

            var result = await testItems.AcquirerDataShareRequestService.SubmitDataShareRequestAsync(
                It.IsAny<Guid>());

            Assert.Multiple(() =>
            {
                Assert.That(result.Success, Is.False);
                Assert.That(result.Error, Is.EqualTo("test exception message"));
            });
        }

        [Test]
        public async Task GivenSubmittingADataShareRequestInTheRepositoryWillThrowAnException_WhenISubmitDataShareRequestAsync_ThenAnErrorIsLogged()
        {
            var testItems = CreateTestItems();

            ReportDataShareRequestAsBeingInDraftStatusAndReadyForSubmission(testItems);

            var testException = new Exception("test exception message");
            testItems.MockAcquirerDataShareRequestRepository.Setup(x => x.SubmitDataShareRequestAsync(
                    It.IsAny<IUserIdSet>(), It.IsAny<Guid>()))
                .ThrowsAsync(testException);

            testItems.MockServiceOperationResultFactory.Setup(x => x.CreateFailedDataResult<DataShareRequestSubmissionResult>(It.IsAny<string>(), It.IsAny<HttpStatusCode?>()))
                .Returns((string errorMessage, HttpStatusCode? _) => CreateTestFailedDataResult<DataShareRequestSubmissionResult>(errorMessage));

            await testItems.AcquirerDataShareRequestService.SubmitDataShareRequestAsync(
                It.IsAny<Guid>());

            testItems.MockLogger.VerifyLog(LogLevel.Error, "Failed to SubmitDataShareRequest", testException);
        }

        [Test]
        public async Task GivenADataShareRequestCanBeSubmitted_WhenISubmitDataShareRequestAsync_ThenTheDataShareRequestIsSubmittedInTheRepository()
        {
            var testItems = CreateTestItems();

            ReportDataShareRequestAsBeingInDraftStatusAndReadyForSubmission(testItems);

            var testUserDetails = CreateTestUserDetails(userName: "test user name");
            testItems.MockUserProfilePresenter.Setup(x => x.GetInitiatingUserDetailsAsync())
                .ReturnsAsync(() => testUserDetails);

            var dataShareRequestId = Guid.Parse("B32F6D84-E3E3-4133-A8DC-6F56A092C2EA");

            await testItems.AcquirerDataShareRequestService.SubmitDataShareRequestAsync(
                dataShareRequestId);

            testItems.MockAcquirerDataShareRequestRepository.Verify(x => x.SubmitDataShareRequestAsync(
                    testUserDetails.UserIdSet,
                    dataShareRequestId),
                Times.Once);
        }

        [Test]
        public async Task GivenADataShareRequestIsSubmitted_WhenISubmitDataShareRequestAsync_ThenANotificationIsSentToTheDeterminedRecipientForTheEsda()
        {
            var testItems = CreateTestItems();

            ReportDataShareRequestAsBeingInDraftStatusAndReadyForSubmission(testItems);

            testItems.MockUserProfilePresenter.Setup(x => x.GetInitiatingUserOrganisationInformationAsync())
                .ReturnsAsync(() => CreateTestOrganisationInformation(organisationName: "test acquirer organisation name"));

            var testDataShareRequestId = Guid.Parse("B32F6D84-E3E3-4133-A8DC-6F56A092C2EA");

            var testEsdaId = Guid.Parse("F241AAB3-730E-4462-932E-1766982A81A4");
            var testDataShareRequestNotificationInformationModelData = testItems.Fixture.Build<DataShareRequestNotificationInformationModelData>()
                .With(x => x.EsdaName, "test esda name")
                .With(x => x.EsdaId, testEsdaId)
                .Create();

            testItems.MockAcquirerDataShareRequestRepository.Setup(x =>
                    x.GetDataShareRequestNotificationInformationAsync(testDataShareRequestId))
                .ReturnsAsync(testDataShareRequestNotificationInformationModelData);

            testItems.MockEsdaInformationPresenter.Setup(x => x.GetEsdaDetailsByIdAsync(testEsdaId))
                .ReturnsAsync(() =>
                {
                    return testItems.Fixture.Build<EsdaDetails>()
                        .With(x => x.Id, testEsdaId)
                        .Create();
                });

            testItems.MockDataShareRequestNotificationRecipientDetermination.Setup(x => 
                    x.DetermineDataShareRequestNotificationRecipientAsync(testDataShareRequestNotificationInformationModelData))
                .ReturnsAsync(() =>
                {
                    return testItems.Fixture.Build<DataShareRequestNotificationRecipient>()
                        .With(x => x.EmailAddress, "test esda notification email address")
                        .With(x => x.RecipientName, "test esda notification user name")
                        .Create();
                });

            testItems.MockServiceOperationResultFactory.Setup(x => x.CreateSuccessfulResult(It.IsAny<HttpStatusCode?>()))
                .Returns(CreateTestSuccessfulResult);

            testItems.MockUserProfilePresenter.Setup(x => x.GetUserDetailsByUserEmailAddressAsync(
                    It.IsAny<string>()))
                .ReturnsAsync(() => CreateTestUserDetails(emailNotification: true));

            await testItems.AcquirerDataShareRequestService.SubmitDataShareRequestAsync(
                testDataShareRequestId);

            testItems.MockNotificationService.Verify(x => x.SendToSupplierNewDataShareRequestReceivedNotificationAsync(
                    "test esda notification email address",
                    "test esda notification user name",
                    "test acquirer organisation name",
                    "test esda name"),
                Times.Once);
        }

        [Test]
        public async Task GivenNotificationWillFailToSend_WhenISubmitDataShareRequestAsync_ThenAMessageIsLoggedButSuccessIsStillReported()
        {
            var testItems = CreateTestItems();

            ReportDataShareRequestAsBeingInDraftStatusAndReadyForSubmission(testItems);

            var testException = new Exception("test exception message");
            testItems.MockNotificationService.Setup(x => x.SendToSupplierNewDataShareRequestReceivedNotificationAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .ThrowsAsync(testException);

            testItems.MockUserProfilePresenter.Setup(x => x.GetUserDetailsByUserEmailAddressAsync(
                    It.IsAny<string>()))
                .ReturnsAsync(() => CreateTestUserDetails(emailNotification: true));

            await testItems.AcquirerDataShareRequestService.SubmitDataShareRequestAsync(
                It.IsAny<Guid>());

            testItems.MockLogger.VerifyLog(LogLevel.Error, "Failed to send notification of Data Share Request Submission", testException);
        }

        [Test]
        public async Task GivenADataShareRequestCanBeSubmitted_WhenISubmitDataShareRequestAsync_ThenASuccessfulResponseIsReturned()
        {
            var testItems = CreateTestItems();

            ReportDataShareRequestAsBeingInDraftStatusAndReadyForSubmission(testItems);

            testItems.MockServiceOperationResultFactory.Setup(x => x.CreateSuccessfulDataResult(
                    It.IsAny<DataShareRequestSubmissionResult>(), It.IsAny<HttpStatusCode?>()))
                .Returns((DataShareRequestSubmissionResult result, HttpStatusCode? _) => CreateTestSuccessfulDataResult(result));

            var result = await testItems.AcquirerDataShareRequestService.SubmitDataShareRequestAsync(
                It.IsAny<Guid>());

            Assert.Multiple(() =>
            {
                Assert.That(result.Success, Is.True);
                Assert.That(result.Error, Is.Null);
            });
        }
        #endregion

        #region GetDataShareRequestAnswersSummaryAsync() Tests
        [Test]
        public async Task GivenADataShareRequestId_WhenIGetDataShareRequestAnswersSummaryAsync_ThenTheAnswerSummaryForThatDataShareRequestIdIsReturned()
        {
            var testItems = CreateTestItems();

            var testDataShareRequestId = testItems.Fixture.Create<Guid>();

            var testDataShareRequestAnswersSummaryModelData = testItems.Fixture.Create<DataShareRequestAnswersSummaryModelData>();
            var testDataShareRequestAnswersSummary = testItems.Fixture.Create<DataShareRequestAnswersSummary>();

            testItems.MockAcquirerDataShareRequestRepository.Setup(x => x.GetDataShareRequestAnswersSummaryAsync(
                    testDataShareRequestId))
                .ReturnsAsync(testDataShareRequestAnswersSummaryModelData);
            
            testItems.MockAcquirerDataShareRequestModelDataFactory.Setup(x => x.CreateDataShareRequestAnswersSummary(
                    testDataShareRequestAnswersSummaryModelData))
                .Returns(testDataShareRequestAnswersSummary);

            testItems.MockServiceOperationResultFactory.Setup(x => x.CreateSuccessfulDataResult(It.IsAny<DataShareRequestAnswersSummary>(), It.IsAny<HttpStatusCode?>()))
                .Returns((DataShareRequestAnswersSummary answerSummary, HttpStatusCode? _) => CreateTestSuccessfulDataResult(answerSummary));

            var result = await testItems.AcquirerDataShareRequestService.GetDataShareRequestAnswersSummaryAsync(
                testDataShareRequestId);

            Assert.Multiple(() =>
            {
                Assert.That(result.Success, Is.True);
                Assert.That(result.Data, Is.EqualTo(testDataShareRequestAnswersSummary));
                Assert.That(result.Error, Is.Null);
            });
        }

        [Test]
        public async Task GivenACancelledDataShareRequestId_WhenIGetDataShareRequestAnswersSummaryAsync_ThenTheCancellationReasonsForThatDataShareRequestAreReturned()
        {
            var testItems = CreateTestItems();

            var testDataShareRequestId = testItems.Fixture.Create<Guid>();

            var testDataShareRequestAnswersSummaryModelData = testItems.Fixture
                .Build<DataShareRequestAnswersSummaryModelData>()
                .With(x => x.DataShareRequestAnswersSummary_DataShareRequestId, testDataShareRequestId)
                .With(x => x.DataShareRequestAnswersSummary_RequestStatus, DataShareRequestStatusType.Cancelled)
                .Create();
            var testDataShareRequestAnswersSummary = testItems.Fixture.Create<DataShareRequestAnswersSummary>();

            testItems.MockAcquirerDataShareRequestRepository.Setup(x => x.GetDataShareRequestAnswersSummaryAsync(
                    testDataShareRequestId))
                .ReturnsAsync(testDataShareRequestAnswersSummaryModelData);

            testItems.MockAcquirerDataShareRequestModelDataFactory.Setup(x => x.CreateDataShareRequestAnswersSummary(
                    testDataShareRequestAnswersSummaryModelData))
                .Returns(testDataShareRequestAnswersSummary);

            var testCancellationComment = testItems.Fixture
                .Build<AuditLogDataShareRequestStatusChangeCommentModelData>()
                .With(x => x.AuditLogDataShareRequestStatusChangeComment_Comment, "test cancellation comment")
                .Create();

            var testCancellationAuditLogDataShareRequestStatusChangeModelData = testItems.Fixture
                .Build<AuditLogDataShareRequestStatusChangeModelData>()
                .With(x => x.AuditLogDataShareRequestStatusChange_Comments, [testCancellationComment])
                .Create();

            testItems.MockAuditLogService
                .Setup(x => x.GetMostRecentAuditLogForDataShareRequestStatusChangeToStatusAsync(
                    testDataShareRequestId, DataShareRequestStatusType.Cancelled))
                .ReturnsAsync(() => testCancellationAuditLogDataShareRequestStatusChangeModelData);

            testItems.MockServiceOperationResultFactory.Setup(x => x.CreateSuccessfulDataResult(It.IsAny<DataShareRequestAnswersSummary>(), It.IsAny<HttpStatusCode?>()))
                .Returns((DataShareRequestAnswersSummary answerSummary, HttpStatusCode? _) => CreateTestSuccessfulDataResult(answerSummary));

            await testItems.AcquirerDataShareRequestService.GetDataShareRequestAnswersSummaryAsync(testDataShareRequestId);

            testItems.MockAcquirerDataShareRequestModelDataFactory.Verify(x => x.CreateDataShareRequestAnswersSummary(
                    It.Is<DataShareRequestAnswersSummaryModelData>(data => data.DataShareRequestAnswersSummary_CancellationReasonsFromAcquirer == "test cancellation comment")),
                Times.Once);
        }

        [Test]
        public async Task GivenCancellationReasonsCannotBeFound_WhenIGetDataShareRequestAnswersSummaryAsync_ThenTheCancellationReasonsForThatDataShareRequestAreReportedAsNotFound()
        {
            var testItems = CreateTestItems();

            var testDataShareRequestId = testItems.Fixture.Create<Guid>();

            var testDataShareRequestAnswersSummaryModelData = testItems.Fixture
                .Build<DataShareRequestAnswersSummaryModelData>()
                .With(x => x.DataShareRequestAnswersSummary_DataShareRequestId, testDataShareRequestId)
                .With(x => x.DataShareRequestAnswersSummary_RequestStatus, DataShareRequestStatusType.Cancelled)
                .Create();
            var testDataShareRequestAnswersSummary = testItems.Fixture.Create<DataShareRequestAnswersSummary>();

            testItems.MockAcquirerDataShareRequestRepository.Setup(x => x.GetDataShareRequestAnswersSummaryAsync(
                    testDataShareRequestId))
                .ReturnsAsync(testDataShareRequestAnswersSummaryModelData);

            testItems.MockAcquirerDataShareRequestModelDataFactory.Setup(x => x.CreateDataShareRequestAnswersSummary(
                    testDataShareRequestAnswersSummaryModelData))
                .Returns(testDataShareRequestAnswersSummary);

            testItems.MockAuditLogService
                .Setup(x => x.GetMostRecentAuditLogForDataShareRequestStatusChangeToStatusAsync(
                    testDataShareRequestId, DataShareRequestStatusType.Cancelled))
                .ReturnsAsync(() => null);

            testItems.MockServiceOperationResultFactory.Setup(x => x.CreateSuccessfulDataResult(It.IsAny<DataShareRequestAnswersSummary>(), It.IsAny<HttpStatusCode?>()))
                .Returns((DataShareRequestAnswersSummary answerSummary, HttpStatusCode? _) => CreateTestSuccessfulDataResult(answerSummary));

            await testItems.AcquirerDataShareRequestService.GetDataShareRequestAnswersSummaryAsync(testDataShareRequestId);

            testItems.MockAcquirerDataShareRequestModelDataFactory.Verify(x => x.CreateDataShareRequestAnswersSummary(
                    It.Is<DataShareRequestAnswersSummaryModelData>(data => data.DataShareRequestAnswersSummary_CancellationReasonsFromAcquirer == "Unable to find cancellation reasons from acquirer")),
                Times.Once);
        }

        [Test]
        public async Task GivenADecidedDataShareRequestId_WhenIGetDataShareRequestAnswersSummaryAsync_ThenTheSupplierCommentsForThatDataShareRequestAreReturned(
            [Values(DataShareRequestStatusType.Accepted, DataShareRequestStatusType.Rejected, DataShareRequestStatusType.Returned)] DataShareRequestStatusType testRequestStatus)
        {
            var testItems = CreateTestItems();

            var testDataShareRequestId = testItems.Fixture.Create<Guid>();

            var testDataShareRequestAnswersSummaryModelData = testItems.Fixture
                .Build<DataShareRequestAnswersSummaryModelData>()
                .With(x => x.DataShareRequestAnswersSummary_DataShareRequestId, testDataShareRequestId)
                .With(x => x.DataShareRequestAnswersSummary_RequestStatus, testRequestStatus)
                .Create();
            var testDataShareRequestAnswersSummary = testItems.Fixture.Create<DataShareRequestAnswersSummary>();

            testItems.MockAcquirerDataShareRequestRepository.Setup(x => x.GetDataShareRequestAnswersSummaryAsync(
                    testDataShareRequestId))
                .ReturnsAsync(testDataShareRequestAnswersSummaryModelData);

            testItems.MockAcquirerDataShareRequestModelDataFactory.Setup(x => x.CreateDataShareRequestAnswersSummary(
                    testDataShareRequestAnswersSummaryModelData))
                .Returns(testDataShareRequestAnswersSummary);

            var testDecisionComment = testItems.Fixture
                .Build<AuditLogDataShareRequestStatusChangeCommentModelData>()
                .With(x => x.AuditLogDataShareRequestStatusChangeComment_Comment, "test supplier comment")
                .Create();

            var testDecisionAuditLogDataShareRequestStatusChangeModelData = testItems.Fixture
                .Build<AuditLogDataShareRequestStatusChangeModelData>()
                .With(x => x.AuditLogDataShareRequestStatusChange_Comments, [testDecisionComment])
                .Create();

            testItems.MockAuditLogService
                .Setup(x => x.GetMostRecentAuditLogForDataShareRequestStatusChangeToStatusAsync(
                    testDataShareRequestId, testRequestStatus))
                .ReturnsAsync(() => testDecisionAuditLogDataShareRequestStatusChangeModelData);

            testItems.MockServiceOperationResultFactory.Setup(x => x.CreateSuccessfulDataResult(It.IsAny<DataShareRequestAnswersSummary>(), It.IsAny<HttpStatusCode?>()))
                .Returns((DataShareRequestAnswersSummary answerSummary, HttpStatusCode? _) => CreateTestSuccessfulDataResult(answerSummary));

            await testItems.AcquirerDataShareRequestService.GetDataShareRequestAnswersSummaryAsync(testDataShareRequestId);

            testItems.MockAcquirerDataShareRequestModelDataFactory.Verify(x => x.CreateDataShareRequestAnswersSummary(
                    It.Is<DataShareRequestAnswersSummaryModelData>(data => data.DataShareRequestAnswersSummary_SubmissionResponseFromSupplier == "test supplier comment")),
                Times.Once);
        }

        [Test]
        public async Task GivenDecisionReasonsCannotBeFound_WhenIGetDataShareRequestAnswersSummaryAsync_ThenTheDecisionReasonsForThatDataShareRequestAreReportedAsNotFound(
            [Values(DataShareRequestStatusType.Accepted, DataShareRequestStatusType.Rejected, DataShareRequestStatusType.Returned)] DataShareRequestStatusType testRequestStatus)
        {
            var testItems = CreateTestItems();

            var testDataShareRequestId = testItems.Fixture.Create<Guid>();

            var testDataShareRequestAnswersSummaryModelData = testItems.Fixture
                .Build<DataShareRequestAnswersSummaryModelData>()
                .With(x => x.DataShareRequestAnswersSummary_DataShareRequestId, testDataShareRequestId)
                .With(x => x.DataShareRequestAnswersSummary_RequestStatus, testRequestStatus)
                .Create();
            var testDataShareRequestAnswersSummary = testItems.Fixture.Create<DataShareRequestAnswersSummary>();

            testItems.MockAcquirerDataShareRequestRepository.Setup(x => x.GetDataShareRequestAnswersSummaryAsync(
                    testDataShareRequestId))
                .ReturnsAsync(testDataShareRequestAnswersSummaryModelData);

            testItems.MockAcquirerDataShareRequestModelDataFactory.Setup(x => x.CreateDataShareRequestAnswersSummary(
                    testDataShareRequestAnswersSummaryModelData))
                .Returns(testDataShareRequestAnswersSummary);

            testItems.MockAuditLogService
                .Setup(x => x.GetMostRecentAuditLogForDataShareRequestStatusChangeToStatusAsync(
                    testDataShareRequestId, testRequestStatus))
                .ReturnsAsync(() => null);

            testItems.MockServiceOperationResultFactory.Setup(x => x.CreateSuccessfulDataResult(It.IsAny<DataShareRequestAnswersSummary>(), It.IsAny<HttpStatusCode?>()))
                .Returns((DataShareRequestAnswersSummary answerSummary, HttpStatusCode? _) => CreateTestSuccessfulDataResult(answerSummary));

            await testItems.AcquirerDataShareRequestService.GetDataShareRequestAnswersSummaryAsync(testDataShareRequestId);

            testItems.MockAcquirerDataShareRequestModelDataFactory.Verify(x => x.CreateDataShareRequestAnswersSummary(
                    It.Is<DataShareRequestAnswersSummaryModelData>(data => data.DataShareRequestAnswersSummary_SubmissionResponseFromSupplier == "Unable to find decision comments from supplier")),
                Times.Once);
        }

        [Test]
        public async Task GivenAnExceptionIsThrownWhilstGettingTheDataShareRequestAnswersSummary_GetDataShareRequestAnswersSummaryAsync_ThenAnErrorIsLogged()
        {
            var testItems = CreateTestItems();

            var testException = new Exception("test exception message");

            testItems.MockAcquirerDataShareRequestRepository.Setup(x => x.GetDataShareRequestAnswersSummaryAsync(
                    It.IsAny<Guid>()))
                .ThrowsAsync(testException);

            await testItems.AcquirerDataShareRequestService.GetDataShareRequestAnswersSummaryAsync(
                It.IsAny<Guid>());

            testItems.MockLogger.VerifyLog(LogLevel.Error, "Failed to GetDataShareRequestAnswersSummary", testException);
        }

        [Test]
        public async Task GivenAnExceptionIsThrownWhilstGettingTheDataShareRequestAnswersSummary_GetDataShareRequestAnswersSummaryAsync_ThenAFailedDataResultIsReturnedReportingTheError()
        {
            var testItems = CreateTestItems();

            var testException = new Exception("test exception message");

            testItems.MockAcquirerDataShareRequestRepository.Setup(x => x.GetDataShareRequestAnswersSummaryAsync(
                    It.IsAny<Guid>()))
                .ThrowsAsync(testException);

            var testFailedDataResult = Mock.Of<IServiceOperationDataResult<DataShareRequestAnswersSummary>>();
            testItems.MockServiceOperationResultFactory.Setup(x => x.CreateFailedDataResult<DataShareRequestAnswersSummary>(It.IsAny<string>(), It.IsAny<HttpStatusCode?>()))
                .Returns(() => testFailedDataResult);

            var result = await testItems.AcquirerDataShareRequestService.GetDataShareRequestAnswersSummaryAsync(
                It.IsAny<Guid>());

            Assert.That(result, Is.SameAs(testFailedDataResult));
        }
        #endregion

        #region CancelDataShareRequestAsync() Tests
        [Test]
        public async Task GivenARequestingUserId_WhenICancelDataShareRequestAsync_ThenTheStatusOfTheDataShareRequestIsVerifiedUsingTheGivenRequestingUserId(
            [ValueSource(nameof(CancellableDataShareRequestStatusTypes))] DataShareRequestStatusType dataShareRequestStatus)
        {
            var testItems = CreateTestItems();

            ReportDataShareRequestAsHavingRequestStatus(testItems, dataShareRequestStatus);

            var testUserDetails = CreateTestUserDetails(userId: 100, domainId: 200, organisationId: 300);
            testItems.MockUserProfilePresenter.Setup(x => x.GetInitiatingUserDetailsAsync())
                .ReturnsAsync(() => testUserDetails);

            var dataShareRequestId = testItems.Fixture.Create<Guid>();

            await testItems.AcquirerDataShareRequestService.CancelDataShareRequestAsync(
                dataShareRequestId,
                It.IsAny<string>());

            testItems.MockAcquirerDataShareRequestRepository.Verify(x => x.CancelDataShareRequestAsync(
                    testUserDetails.UserIdSet,
                    dataShareRequestId,
                    It.IsAny<string>()),
                Times.Once);
        }

        [Test]
        public async Task GivenADataShareRequestIsNotInACancellableStatus_WhenICancelDataShareRequestAsync_ThenAFailedResponseIsReturnedReportingTheError(
            [ValueSource(nameof(NonCancellableDataShareRequestStatusTypes))] DataShareRequestStatusType dataShareRequestStatusType)
        {
            var testItems = CreateTestItems();

            testItems.MockAcquirerDataShareRequestRepository.Setup(x => x.GetDataShareRequestStatusAsync(
                    It.IsAny<Guid>()))
                .ReturnsAsync(() => testItems.Fixture.Build<DataShareRequestStatusTypeModelData>()
                    .With(x => x.DataShareRequestStatus_RequestStatus, dataShareRequestStatusType).Create());

            testItems.MockServiceOperationResultFactory.Setup(x => x.CreateFailedDataResult<DataShareRequestCancellationResult>(It.IsAny<string>(), It.IsAny<HttpStatusCode?>()))
                .Returns((string errorMessage, HttpStatusCode? _) => CreateTestFailedDataResult<DataShareRequestCancellationResult>(errorMessage));

            var result = await testItems.AcquirerDataShareRequestService.CancelDataShareRequestAsync(
                It.IsAny<Guid>(),
                It.IsAny<string>());

            Assert.Multiple(() =>
            {
                Assert.That(result.Success, Is.False);
                Assert.That(result.Error, Is.EqualTo("DataShareRequest status does not allow it to be cancelled"));
            });
        }

        [Test]
        public async Task GivenCancellingDataShareQuestionAnswerInTheRepositoryWillThrowAnException_WhenICancelDataShareRequestAsync_ThenAFailedResponseIsReturnedReportingTheError(
            [ValueSource(nameof(CancellableDataShareRequestStatusTypes))] DataShareRequestStatusType dataShareRequestStatus)
        {
            var testItems = CreateTestItems();

            ReportDataShareRequestAsHavingRequestStatus(testItems, dataShareRequestStatus);

            testItems.MockAcquirerDataShareRequestRepository.Setup(x => x.CancelDataShareRequestAsync(
                    It.IsAny<IUserIdSet>(), It.IsAny<Guid>(), It.IsAny<string>()))
                .ThrowsAsync(new Exception("test exception message"));

            testItems.MockServiceOperationResultFactory.Setup(x => x.CreateFailedDataResult<DataShareRequestCancellationResult>(It.IsAny<string>(), It.IsAny<HttpStatusCode?>()))
                .Returns((string errorMessage, HttpStatusCode? _) => CreateTestFailedDataResult<DataShareRequestCancellationResult>(errorMessage));

            var result = await testItems.AcquirerDataShareRequestService.CancelDataShareRequestAsync(
                It.IsAny<Guid>(),
                It.IsAny<string>());

            Assert.Multiple(() =>
            {
                Assert.That(result.Success, Is.False);
                Assert.That(result.Error, Is.EqualTo("test exception message"));
            });
        }

        [Test]
        public async Task GivenCancellingDataShareQuestionAnswerInTheRepositoryWillThrowAnException_WhenICancelDataShareRequestAsync_ThenAnErrorIsLogged(
            [ValueSource(nameof(CancellableDataShareRequestStatusTypes))] DataShareRequestStatusType dataShareRequestStatus)
        {
            var testItems = CreateTestItems();

            ReportDataShareRequestAsHavingRequestStatus(testItems, dataShareRequestStatus);

            var testException = new Exception("test exception message");
            testItems.MockAcquirerDataShareRequestRepository.Setup(x => x.CancelDataShareRequestAsync(
                    It.IsAny<IUserIdSet>(), It.IsAny<Guid>(), It.IsAny<string>()))
                .ThrowsAsync(testException);

            testItems.MockServiceOperationResultFactory.Setup(x => x.CreateFailedDataResult<DataShareRequestCancellationResult>(It.IsAny<string>(), It.IsAny<HttpStatusCode?>()))
                .Returns((string errorMessage, HttpStatusCode? _) => CreateTestFailedDataResult<DataShareRequestCancellationResult>(errorMessage));

            await testItems.AcquirerDataShareRequestService.CancelDataShareRequestAsync(
                It.IsAny<Guid>(),
                It.IsAny<string>());

            testItems.MockLogger.VerifyLog(LogLevel.Error, "Failed to CancelDataShareRequest", testException);
        }

        [Test]
        public async Task GivenADataShareRequestCanBeCancelled_WhenISubmitDataShareRequestAsync_ThenTheDataShareRequestIsCancelledInTheRepository(
            [ValueSource(nameof(CancellableDataShareRequestStatusTypes))] DataShareRequestStatusType dataShareRequestStatusType)
        {
            var testItems = CreateTestItems();

            testItems.MockAcquirerDataShareRequestRepository.Setup(x => x.GetDataShareRequestStatusAsync(
                    It.IsAny<Guid>()))
                .ReturnsAsync(() => testItems.Fixture.Build<DataShareRequestStatusTypeModelData>()
                        .With(x => x.DataShareRequestStatus_RequestStatus, dataShareRequestStatusType)
                        .Create());

            var dataShareRequestId = Guid.Parse("B32F6D84-E3E3-4133-A8DC-6F56A092C2EA");
            var reasonsForCancellation = testItems.Fixture.Create<string>();

            await testItems.AcquirerDataShareRequestService.CancelDataShareRequestAsync(
                dataShareRequestId,
                reasonsForCancellation);

            testItems.MockAcquirerDataShareRequestRepository.Verify(x => x.CancelDataShareRequestAsync(
                    It.IsAny<IUserIdSet>(),
                    dataShareRequestId,
                    reasonsForCancellation),
                Times.Once);
        }

        [Test]
        public async Task GivenADataShareRequestCanBeCancelled_WhenISubmitDataShareRequestAsync_ThenANotificationIsSentToTheDeterminedRecipientForTheEsda()
        {
            var testItems = CreateTestItems();

            ReportDataShareRequestAsHavingRequestStatus(testItems, CancellableDataShareRequestStatusTypes.First());

            var testDataShareRequestId = Guid.Parse("B32F6D84-E3E3-4133-A8DC-6F56A092C2EA");
            var reasonsForCancellation = testItems.Fixture.Create<string>();
            var dataShareRequestRequestId = testItems.Fixture.Create<string>();
            var testEsdaId = Guid.Parse("F241AAB3-730E-4462-932E-1766982A81A4");

            var testUserDetails = CreateTestUserDetails(userName: "test acquirer user name");
            testItems.MockUserProfilePresenter.Setup(x => x.GetInitiatingUserDetailsAsync())
                .ReturnsAsync(() => testUserDetails);

            var testDataShareRequestNotificationInformationModelData = testItems.Fixture.Build<DataShareRequestNotificationInformationModelData>()
                .With(x => x.EsdaName, "test esda name")
                .With(x => x.EsdaId, testEsdaId)
                .With(x => x.DataShareRequestRequestId, dataShareRequestRequestId)
                .Create();

            testItems.MockAcquirerDataShareRequestRepository.Setup(x =>
                    x.GetDataShareRequestNotificationInformationAsync(testDataShareRequestId))
                .ReturnsAsync(testDataShareRequestNotificationInformationModelData);

            testItems.MockEsdaInformationPresenter.Setup(x => x.GetEsdaDetailsByIdAsync(testEsdaId))
                .ReturnsAsync(() =>
                {
                    return testItems.Fixture.Build<EsdaDetails>()
                        .With(x => x.Id, testEsdaId)
                        .Create();
                });

            testItems.MockDataShareRequestNotificationRecipientDetermination.Setup(x =>
                    x.DetermineDataShareRequestNotificationRecipientAsync(testDataShareRequestNotificationInformationModelData))
                .ReturnsAsync(() =>
                {
                    return testItems.Fixture.Build<DataShareRequestNotificationRecipient>()
                        .With(x => x.EmailAddress, "test esda notification email address")
                        .With(x => x.RecipientName, "test esda notification user name")
                        .Create();
                });

            testItems.MockUserProfilePresenter.Setup(x => x.GetUserDetailsByUserEmailAddressAsync(
                    It.IsAny<string>()))
                .ReturnsAsync(() => CreateTestUserDetails(emailNotification: true));

            await testItems.AcquirerDataShareRequestService.CancelDataShareRequestAsync(
                testDataShareRequestId,
                reasonsForCancellation);

            testItems.MockNotificationService.Verify(x => x.SendToSupplierDataShareRequestCancelledNotificationAsync(
                    "test esda notification email address",
                    "test esda notification user name",
                    "test acquirer user name",
                    "test esda name",
                    dataShareRequestRequestId,
                    reasonsForCancellation),
                Times.Once);
        }

        [Test]
        public async Task GivenGettingNotificationInformationWithFail_WhenISubmitDataShareRequestAsync_ThenNotificationSuccessIsFalse()
        {
            var testItems = CreateTestItems();

            ReportDataShareRequestAsHavingRequestStatus(testItems, CancellableDataShareRequestStatusTypes.First());

            var testDataShareRequestId = Guid.Parse("B32F6D84-E3E3-4133-A8DC-6F56A092C2EA");
            var reasonsForCancellation = testItems.Fixture.Create<string>();

            var testUserDetails = CreateTestUserDetails(userName: "test acquirer user name");
            testItems.MockUserProfilePresenter.Setup(x => x.GetInitiatingUserDetailsAsync())
                .ReturnsAsync(() => testUserDetails);

            testItems.MockAcquirerDataShareRequestRepository.Setup(x =>
                    x.GetDataShareRequestNotificationInformationAsync(testDataShareRequestId))
                .ThrowsAsync(new Exception("oh noes!"));

            await testItems.AcquirerDataShareRequestService.CancelDataShareRequestAsync(
                testDataShareRequestId,
                reasonsForCancellation);

            testItems.MockAcquirerDataShareRequestModelDataFactory.Verify(x => x.CreateDataShareRequestCancellationResult(
                    testDataShareRequestId,
                    reasonsForCancellation,
                    false),
                Times.Once);
        }

        [Test]
        public async Task GivenSendingANotificationToTheSupplierWithFail_WhenISubmitDataShareRequestAsync_ThenNotificationSuccessIsFalse()
        {
            var testItems = CreateTestItems();

            ReportDataShareRequestAsHavingRequestStatus(testItems, CancellableDataShareRequestStatusTypes.First());

            var testDataShareRequestId = Guid.Parse("B32F6D84-E3E3-4133-A8DC-6F56A092C2EA");
            var reasonsForCancellation = testItems.Fixture.Create<string>();
            var dataShareRequestRequestId = testItems.Fixture.Create<string>();
            var testEsdaId = Guid.Parse("F241AAB3-730E-4462-932E-1766982A81A4");

            var testUserDetails = CreateTestUserDetails(userName: "test acquirer user name");
            testItems.MockUserProfilePresenter.Setup(x => x.GetInitiatingUserDetailsAsync())
                .ReturnsAsync(() => testUserDetails);

            var testDataShareRequestNotificationInformationModelData = testItems.Fixture.Build<DataShareRequestNotificationInformationModelData>()
                .With(x => x.EsdaName, "test esda name")
                .With(x => x.EsdaId, testEsdaId)
                .With(x => x.DataShareRequestRequestId, dataShareRequestRequestId)
                .Create();

            testItems.MockAcquirerDataShareRequestRepository.Setup(x =>
                    x.GetDataShareRequestNotificationInformationAsync(testDataShareRequestId))
                .ReturnsAsync(testDataShareRequestNotificationInformationModelData);

            testItems.MockEsdaInformationPresenter.Setup(x => x.GetEsdaDetailsByIdAsync(testEsdaId))
                .ReturnsAsync(() =>
                {
                    return testItems.Fixture.Build<EsdaDetails>()
                        .With(x => x.Id, testEsdaId)
                        .Create();
                });

            testItems.MockDataShareRequestNotificationRecipientDetermination.Setup(x =>
                    x.DetermineDataShareRequestNotificationRecipientAsync(testDataShareRequestNotificationInformationModelData))
                .ReturnsAsync(() =>
                {
                    return testItems.Fixture.Build<DataShareRequestNotificationRecipient>()
                        .With(x => x.EmailAddress, "test esda notification email address")
                        .With(x => x.RecipientName, "test esda notification user name")
                        .Create();
                });

            testItems.MockUserProfilePresenter.Setup(x => x.GetUserDetailsByUserEmailAddressAsync(
                    It.IsAny<string>()))
                .ReturnsAsync(() => CreateTestUserDetails(emailNotification: true));

            testItems.MockNotificationService.Setup(x => x.SendToSupplierDataShareRequestCancelledNotificationAsync(
                    "test esda notification email address",
                    "test esda notification user name",
                    "test acquirer user name",
                    "test esda name",
                    dataShareRequestRequestId,
                    reasonsForCancellation))
                .ThrowsAsync(new Exception("oh noes!"));

            await testItems.AcquirerDataShareRequestService.CancelDataShareRequestAsync(
                testDataShareRequestId,
                reasonsForCancellation);

            testItems.MockAcquirerDataShareRequestModelDataFactory.Verify(x => x.CreateDataShareRequestCancellationResult(
                    testDataShareRequestId,
                    reasonsForCancellation,
                    false),
                Times.Once);
        }

        [Test]
        public async Task GivenADataShareRequestCanBeCancelled_WhenICancelDataShareRequestAsync_ThenASuccessfulResponseIsReturned(
            [ValueSource(nameof(CancellableDataShareRequestStatusTypes))] DataShareRequestStatusType requestStatus)
        {
            var testItems = CreateTestItems();

            ReportDataShareRequestAsHavingRequestStatus(testItems, requestStatus);

            var testDataShareRequestCancellationResult = testItems.Fixture.Create<DataShareRequestCancellationResult>();
            testItems.MockServiceOperationResultFactory.Setup(x => x.CreateSuccessfulDataResult(It.IsAny<DataShareRequestCancellationResult>(), It.IsAny<HttpStatusCode?>()))
                .Returns(() => CreateTestSuccessfulDataResult(testDataShareRequestCancellationResult));

            var result = await testItems.AcquirerDataShareRequestService.CancelDataShareRequestAsync(
                It.IsAny<Guid>(),
                It.IsAny<string>());

            Assert.Multiple(() =>
            {
                Assert.That(result.Success, Is.True);
                Assert.That(result.Error, Is.Null);
            });
        }
        #endregion

        #region DeleteDataShareRequestAsync() Tests
        [Test]
        public async Task GivenARequestingUserId_WhenIDeleteDataShareRequestAsync_ThenTheStatusOfTheDataShareRequestIsVerifiedUsingTheGivenRequestingUserId(
            [ValueSource(nameof(DeletableDataShareRequestStatusTypes))] DataShareRequestStatusType dataShareRequestStatus)
        {
            var testItems = CreateTestItems();

            ReportDataShareRequestAsHavingRequestStatus(testItems, dataShareRequestStatus);

            var testUserDetails = CreateTestUserDetails(userId: 100, domainId: 200, organisationId: 300);
            testItems.MockUserProfilePresenter.Setup(x => x.GetInitiatingUserDetailsAsync())
                .ReturnsAsync(() => testUserDetails);

            var dataShareRequestId = testItems.Fixture.Create<Guid>();

            await testItems.AcquirerDataShareRequestService.DeleteDataShareRequestAsync(
                dataShareRequestId);

            testItems.MockAcquirerDataShareRequestRepository.Verify(x => x.DeleteDataShareRequestAsync(
                    testUserDetails.UserIdSet,
                    dataShareRequestId),
                Times.Once);
        }

        [Test]
        public async Task GivenADataShareRequestDoesNotExist_WhenIDeleteDataShareRequestAsync_ThenAFailedResponseIsReturnedReportingTheError(
            [ValueSource(nameof(NonDeletableDataShareRequestStatusTypes))] DataShareRequestStatusType dataShareRequestStatus)
        {
            var testItems = CreateTestItems();

            testItems.MockAcquirerDataShareRequestRepository
                .Setup(x => x.CheckIfDataShareRequestExistsAsync(It.IsAny<Guid>()))
                .ReturnsAsync(false);

            testItems.MockServiceOperationResultFactory.Setup(x => x.CreateFailedDataResult<DataShareRequestDeletionResult>(It.IsAny<string>(), It.IsAny<HttpStatusCode?>()))
                .Returns((string errorMessage, HttpStatusCode? statusCode) => CreateTestFailedDataResult<DataShareRequestDeletionResult>(errorMessage, statusCode));

            var result = await testItems.AcquirerDataShareRequestService.DeleteDataShareRequestAsync(
                It.IsAny<Guid>());

            Assert.Multiple(() =>
            {
                Assert.That(result.Success, Is.False);
                Assert.That(result.Error, Is.EqualTo("Data share request not found"));
                Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            });
        }

        [Test]
        public async Task GivenADataShareRequestIsNotInADeletableStatus_WhenIDeleteDataShareRequestAsync_ThenAFailedResponseIsReturnedReportingTheError(
            [ValueSource(nameof(NonDeletableDataShareRequestStatusTypes))] DataShareRequestStatusType dataShareRequestStatus)
        {
            var testItems = CreateTestItems();

            ReportDataShareRequestAsHavingRequestStatus(testItems, dataShareRequestStatus);

            testItems.MockServiceOperationResultFactory.Setup(x => x.CreateFailedDataResult<DataShareRequestDeletionResult>(It.IsAny<string>(), It.IsAny<HttpStatusCode?>()))
                .Returns((string errorMessage, HttpStatusCode? statusCode) => CreateTestFailedDataResult<DataShareRequestDeletionResult>(errorMessage, statusCode));

            var result = await testItems.AcquirerDataShareRequestService.DeleteDataShareRequestAsync(
                It.IsAny<Guid>());

            Assert.Multiple(() =>
            {
                Assert.That(result.Success, Is.False);
                Assert.That(result.Error, Is.EqualTo("DataShareRequest status does not allow it to be deleted"));
                Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Conflict));
            });
        }

        [Test]
        public async Task GivenDeletingADataShareRequestWillThrowAnException_WhenIDeleteDataShareRequestAsync_ThenAFailedResponseIsReturnedReportingTheError(
            [ValueSource(nameof(DeletableDataShareRequestStatusTypes))] DataShareRequestStatusType dataShareRequestStatus)
        {
            var testItems = CreateTestItems();

            ReportDataShareRequestAsHavingRequestStatus(testItems, dataShareRequestStatus);

            testItems.MockAcquirerDataShareRequestRepository.Setup(x => x.DeleteDataShareRequestAsync(
                    It.IsAny<IUserIdSet>(), It.IsAny<Guid>()))
                .ThrowsAsync(new Exception("test exception message"));

            testItems.MockServiceOperationResultFactory.Setup(x => x.CreateFailedDataResult<DataShareRequestDeletionResult>(It.IsAny<string>(), It.IsAny<HttpStatusCode?>()))
                .Returns((string errorMessage, HttpStatusCode? statusCode) => CreateTestFailedDataResult<DataShareRequestDeletionResult>(errorMessage, statusCode));

            var result = await testItems.AcquirerDataShareRequestService.DeleteDataShareRequestAsync(
                It.IsAny<Guid>());

            Assert.Multiple(() =>
            {
                Assert.That(result.Success, Is.False);
                Assert.That(result.Error, Is.EqualTo("test exception message"));
                Assert.That(result.StatusCode, Is.Null);
            });
        }

        [Test]
        public async Task GivenADataShareRequestCanBeDeleted_WhenIDeleteDataShareRequestAsync_ThenTheDataShareRequestIsDeletedInTheRepository(
            [ValueSource(nameof(DeletableDataShareRequestStatusTypes))] DataShareRequestStatusType dataShareRequestStatus)
        {
            var testItems = CreateTestItems();

            ReportDataShareRequestAsHavingRequestStatus(testItems, dataShareRequestStatus);

            var dataShareRequestId = testItems.Fixture.Create<Guid>();

            await testItems.AcquirerDataShareRequestService.DeleteDataShareRequestAsync(
                dataShareRequestId);

            testItems.MockAcquirerDataShareRequestRepository.Verify(x => x.DeleteDataShareRequestAsync(
                    It.IsAny<IUserIdSet>(),
                    dataShareRequestId),
                Times.Once);
        }

        [Test]
        public async Task GivenADataShareRequestCanBeDeleted_WhenIDeleteDataShareRequestAsync_ThenASuccessfulResponseIsReturned(
            [ValueSource(nameof(DeletableDataShareRequestStatusTypes))] DataShareRequestStatusType dataShareRequestStatus)
        {
            var testItems = CreateTestItems();

            ReportDataShareRequestAsHavingRequestStatus(testItems, dataShareRequestStatus);

            var testDataShareRequestDeletionResult = testItems.Fixture.Create<DataShareRequestDeletionResult>();

            testItems.MockServiceOperationResultFactory.Setup(x => x.CreateSuccessfulDataResult(It.IsAny<DataShareRequestDeletionResult>(), It.IsAny<HttpStatusCode?>()))
                .Returns(() => CreateTestSuccessfulDataResult(testDataShareRequestDeletionResult));

            var result = await testItems.AcquirerDataShareRequestService.DeleteDataShareRequestAsync(
                It.IsAny<Guid>());

            Assert.Multiple(() =>
            {
                Assert.That(result.Success, Is.True);
                Assert.That(result.Error, Is.Null);
            });
        }
        #endregion

        #region Helpers
        private static void ReportDataShareRequestAsHavingRequestStatus(
            TestItems testItems,
            DataShareRequestStatusType requestStatus = DataShareRequestStatusType.Draft)
        {
            testItems.MockAcquirerDataShareRequestRepository.Setup(x => x.GetDataShareRequestStatusAsync(
                    It.IsAny<Guid>()))
                .ReturnsAsync(() => testItems.Fixture.Build<DataShareRequestStatusTypeModelData>()
                        .With(x => x.DataShareRequestStatus_RequestStatus, requestStatus)
                        .Create());
        }

        private static void ReportDataShareRequestAsBeingInDraftStatusAndReadyForSubmission(TestItems testItems)
        {
            testItems.MockAcquirerDataShareRequestRepository.Setup(x => x.GetDataShareRequestStatusAsync(
                    It.IsAny<Guid>()))
                .ReturnsAsync(() =>
                    testItems.Fixture.Build<DataShareRequestStatusTypeModelData>()
                        .With(x => x.DataShareRequestStatus_RequestStatus, DataShareRequestStatusType.Draft)
                        .With(x => x.DataShareRequestStatus_QuestionsRemainThatRequireAResponse, false)
                        .Create());
        }
        #endregion

        #region Test Data Creation
        private static IServiceOperationDataResult<T> CreateTestSuccessfulDataResult<T>(T data)
        {
            var mockServiceOperationDataResult = new Mock<IServiceOperationDataResult<T>>();

            mockServiceOperationDataResult.SetupGet(x => x.Success).Returns(true);
            mockServiceOperationDataResult.SetupGet(x => x.Error).Returns((string?)null);
            mockServiceOperationDataResult.SetupGet(x => x.Data).Returns(data);

            return mockServiceOperationDataResult.Object;
        }

        private static IServiceOperationDataResult<T> CreateTestFailedDataResult<T>(
            string errorMessage,
            HttpStatusCode? statusCode = null)
        {
            var mockServiceOperationDataResult = new Mock<IServiceOperationDataResult<T>>();

            mockServiceOperationDataResult.SetupGet(x => x.Success).Returns(false);
            mockServiceOperationDataResult.SetupGet(x => x.Error).Returns(errorMessage);
            mockServiceOperationDataResult.SetupGet(x => x.StatusCode).Returns(statusCode);

            return mockServiceOperationDataResult.Object;
        }

        private static IServiceOperationResult CreateTestSuccessfulResult()
        {
            var mockServiceOperationResult = new Mock<IServiceOperationResult>();

            mockServiceOperationResult.SetupGet(x => x.Success).Returns(true);
            mockServiceOperationResult.SetupGet(x => x.Error).Returns((string?)null);

            return mockServiceOperationResult.Object;
        }

        private static IDataShareRequestQuestionStatusDeterminationResult CreateTestDataShareRequestQuestionStatusDeterminationResult(
            Guid questionId,
            QuestionStatusType questionStatus,
            QuestionStatusType previousQuestionStatus)
        {
            var mockDataShareRequestQuestionStatusDeterminationResult = new Mock<IDataShareRequestQuestionStatusDeterminationResult>();

            var mockDataShareRequestQuestionStatusDataModel = new Mock<IDataShareRequestQuestionSetQuestionStatusDataModel>();
            mockDataShareRequestQuestionStatusDataModel.SetupGet(x => x.QuestionId).Returns(questionId);
            mockDataShareRequestQuestionStatusDataModel.SetupGet(x => x.QuestionStatus).Returns(questionStatus);

            mockDataShareRequestQuestionStatusDeterminationResult.SetupGet(x => x.QuestionSetQuestionStatusData).Returns(mockDataShareRequestQuestionStatusDataModel.Object);
            mockDataShareRequestQuestionStatusDeterminationResult.SetupGet(x => x.PreviousQuestionStatus).Returns(previousQuestionStatus);

            return mockDataShareRequestQuestionStatusDeterminationResult.Object;
        }

        private static IDataShareRequestQuestionStatusesDeterminationResult CreateTestDataShareRequestQuestionStatusesDeterminationResult(
            bool? questionsRemainThatRequireAResponse = null,
            IEnumerable<IDataShareRequestQuestionStatusDeterminationResult>? dataShareRequestQuestionStatusDeterminationResults = null)
        {
            var mockDataShareRequestQuestionStatusesDeterminationResult = new Mock<IDataShareRequestQuestionStatusesDeterminationResult>();

            mockDataShareRequestQuestionStatusesDeterminationResult.SetupGet(x => x.QuestionsRemainThatRequireAResponse)
                .Returns(questionsRemainThatRequireAResponse ?? false);

            mockDataShareRequestQuestionStatusesDeterminationResult.SetupGet(x => x.QuestionStatusDeterminationResults)
                .Returns(dataShareRequestQuestionStatusDeterminationResults ?? []);

            return mockDataShareRequestQuestionStatusesDeterminationResult.Object;
        }

        private static IEnumerable<DataShareRequestStatusType> AllDataShareRequestStatusTypes => Enum.GetValues<DataShareRequestStatusType>();

        private static IEnumerable<DataShareRequestStatusType> SubmittableDataShareRequestStatusTypes =>
            [DataShareRequestStatusType.Draft, DataShareRequestStatusType.Returned];

        private static IEnumerable<DataShareRequestStatusType> NonSubmittableDataShareRequestStatusTypes =>
            AllDataShareRequestStatusTypes.Except(SubmittableDataShareRequestStatusTypes);

        private static IEnumerable<DataShareRequestStatusType> CancellableDataShareRequestStatusTypes =>
            [DataShareRequestStatusType.Submitted, DataShareRequestStatusType.InReview];

        private static IEnumerable<DataShareRequestStatusType> NonCancellableDataShareRequestStatusTypes =>
            AllDataShareRequestStatusTypes.Except(CancellableDataShareRequestStatusTypes);

        private static IEnumerable<DataShareRequestStatusType> DeletableDataShareRequestStatusTypes =>
            [DataShareRequestStatusType.Draft];

        private static IEnumerable<DataShareRequestStatusType> NonDeletableDataShareRequestStatusTypes =>
            AllDataShareRequestStatusTypes.Except(DeletableDataShareRequestStatusTypes);

        private static IEnumerable<QuestionStatusType> AllQuestionStatuses => Enum.GetValues<QuestionStatusType>();
        private static IEnumerable<QuestionStatusType> AnswerableQuestionStatuses => [QuestionStatusType.NotStarted, QuestionStatusType.Completed];
        private static IEnumerable<QuestionStatusType> NonAnswerableQuestionStatuses => AllQuestionStatuses.Except(AnswerableQuestionStatuses);

        private static IDataShareRequestQuestionAnswerValidationResult CreateTestDataShareRequestQuestionAnswerValidationResult(
            IEnumerable<SetDataShareRequestQuestionAnswerPartResponseValidationError>? validationErrors = null)
        {
            var mockDataShareRequestQuestionAnswerValidationResult = new Mock<IDataShareRequestQuestionAnswerValidationResult>();

            mockDataShareRequestQuestionAnswerValidationResult.SetupGet(x => x.AnswerIsValid)
                .Returns(() => validationErrors?.Any() != true);
            mockDataShareRequestQuestionAnswerValidationResult.SetupGet(x => x.ValidationErrors)
                .Returns(() => validationErrors ?? []);

            return mockDataShareRequestQuestionAnswerValidationResult.Object;
        }

        private static IUserIdSet CreateTestUserIdSet(
            int? userId = null,
            int? domainId = null,
            int? organisationId = null)
        {
            var mockUserIdSet = new Mock<IUserIdSet>();

            mockUserIdSet.SetupGet(x => x.UserId).Returns(userId ?? 0);
            mockUserIdSet.SetupGet(x => x.DomainId).Returns(domainId ?? 0);
            mockUserIdSet.SetupGet(x => x.OrganisationId).Returns(organisationId ?? 0);

            return mockUserIdSet.Object;
        }

        private static IUserDetails CreateTestUserDetails(
            int? userId = null,
            int? domainId = null,
            int? organisationId = null,
            string? userName = null,
            string? emailAddress = null,
            bool? emailNotification = null)
        {
            var mockUserIdSet = new Mock<IUserIdSet>();
            mockUserIdSet.SetupGet(x => x.UserId).Returns(userId ?? 0);
            mockUserIdSet.SetupGet(x => x.DomainId).Returns(domainId ?? 0);
            mockUserIdSet.SetupGet(x => x.OrganisationId).Returns(organisationId ?? 0);
            mockUserIdSet.SetupGet(x => x.EmailNotification).Returns(emailNotification ?? false);
            var mockUserContactDetails = new Mock<IUserContactDetails>();
            mockUserContactDetails.SetupGet(x => x.UserName).Returns(userName ?? "");
            mockUserContactDetails.SetupGet(x => x.EmailAddress).Returns(emailAddress ?? "");

            var mockUserDetails = new Mock<IUserDetails>();
            mockUserDetails.SetupGet(x => x.UserIdSet).Returns(mockUserIdSet.Object);
            mockUserDetails.SetupGet(x => x.UserContactDetails).Returns(mockUserContactDetails.Object);
            return mockUserDetails.Object;
        }

        private static IOrganisationInformation CreateTestOrganisationInformation(
            int? organisationId = null,
            string? organisationName = null)
        {
            var mockOrganisationInformation = new Mock<IOrganisationInformation>();

            mockOrganisationInformation.SetupGet(x => x.OrganisationId).Returns(organisationId ?? 0);
            mockOrganisationInformation.SetupGet(x => x.OrganisationName).Returns(organisationName ?? "");

            return mockOrganisationInformation.Object;
        }

        private static DataShareRequestModelData CreateTestDataShareRequestModelData(
            Guid? dataShareRequestId = null,
            string? dataShareRequestRequestId = null,
            int? acquirerUserId = null,
            int? acquirerOrganisationId = null,
            DataShareRequestStatusType? requestStatus = null)
        {
            return new DataShareRequestModelData
            {
                DataShareRequest_Id = dataShareRequestId ?? It.IsAny<Guid>(),
                DataShareRequest_RequestId = dataShareRequestRequestId ?? It.IsAny<string>(),
                DataShareRequest_AcquirerUserId = acquirerUserId ?? It.IsAny<int>(),
                DataShareRequest_AcquirerOrganisationId = acquirerOrganisationId ?? It.IsAny<int>(),
                DataShareRequest_RequestStatus = requestStatus ?? It.IsAny<DataShareRequestStatusType>()
            };
        }
        private static AuditLogDataShareRequestStatusChangeModelData CreateTestAuditLogDataShareRequestStatusChangeModelData(
            DateTime? changedAtUtc = null)
        {
            return new AuditLogDataShareRequestStatusChangeModelData
            {
                AuditLogDataShareRequestStatusChange_ChangedAtUtc = changedAtUtc ?? It.IsAny<DateTime>()
            };
        }

        private static DataShareRequestQuestionAnswer CreateTestDataShareRequestQuestionAnswer(
            TestItems testItems,
            Guid? dataShareRequestId = null,
            Guid? questionId = null)
        {
            var testAnswer = testItems.Fixture.Build<DataShareRequestQuestionAnswer>()
                .With(x => x.DataShareRequestId, dataShareRequestId ?? testItems.Fixture.Create<Guid>())
                .With(x => x.QuestionId, questionId ?? testItems.Fixture.Create<Guid>())
                .Create();

            foreach (var answerPart in testAnswer.AnswerParts)
            {
                foreach (var indexedResponse in answerPart.AnswerPartResponses.Select((response, index) => new { Index = index, Response = response }))
                {
                    indexedResponse.Response.OrderWithinAnswerPart = indexedResponse.Index + 1;
                }
            }

            return testAnswer;
        }

        private static IEsdaDetails CreateTestEsdaDetails(
            Guid? esdaId = null,
            string? esdaTitle = null,
            int? supplierOrganisationId = null,
            int? supplierDomainId = null)
        {
            var mockEsdaDetails = new Mock<IEsdaDetails>();

            mockEsdaDetails.SetupGet(x => x.Id).Returns(esdaId ?? Guid.Empty);
            mockEsdaDetails.SetupGet(x => x.SupplierOrganisationId).Returns(supplierOrganisationId ?? 0);
            mockEsdaDetails.SetupGet(x => x.SupplierDomainId).Returns(supplierDomainId ?? 0);
            mockEsdaDetails.SetupGet(x => x.Title).Returns(esdaTitle ?? "");

            return mockEsdaDetails.Object;
        }
        #endregion

        #region Test Item Creation
        private static TestItems CreateTestItems()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());

            var mockLogger = Mock.Get(fixture.Freeze<ILogger<AcquirerDataShareRequestService>>());

            var mockUserProfilePresenter = Mock.Get(fixture.Create<IUserProfilePresenter>());
            var mockEsdaInformationPresenter = Mock.Get(fixture.Create<IEsdaInformationPresenter>());
            var mockAcquirerDataShareRequestRepository = Mock.Get(fixture.Create<IAcquirerDataShareRequestRepository>());
            var mockAuditLogService = Mock.Get(fixture.Create<IAuditLogService>());
            var mockNotificationService = Mock.Get(fixture.Create<INotificationService>());
            var mockAcquirerDataShareRequestModelDataFactory = Mock.Get(fixture.Create<IAcquirerDataShareRequestModelDataFactory>());
            var mockDataShareRequestQuestionAnswerValidationService = Mock.Get(fixture.Create<IDataShareRequestQuestionAnswerValidationService>());
            var mockQuestionSetPlaceholderReplacementService = Mock.Get(fixture.Create<IQuestionSetPlaceholderReplacementService>());
            var mockDataShareRequestQuestionStatusesDetermination = Mock.Get(fixture.Create<IDataShareRequestQuestionStatusesDetermination>());
            var mockDataShareRequestQuestionSetCompletenessDetermination = Mock.Get(fixture.Create<IDataShareRequestQuestionSetCompletenessDetermination>());
            var mockNextQuestionDetermination = Mock.Get(fixture.Create<INextQuestionDetermination>());
            var mockDataShareRequestNotificationRecipientDetermination = Mock.Get(fixture.Create<IDataShareRequestNotificationRecipientDetermination>());
            var mockKeyQuestionPartAnswerProviderService = Mock.Get(fixture.Create<IKeyQuestionPartAnswerProviderService>());
            var mockServiceOperationResultFactory = Mock.Get(fixture.Create<IServiceOperationResultFactory>());

            ConfigureHappyPathTesting();

            IAcquirerDataShareRequestService acquirerDataShareRequestService = new AcquirerDataShareRequestService(
                mockLogger.Object,
                mockUserProfilePresenter.Object,
                mockEsdaInformationPresenter.Object,
                mockAcquirerDataShareRequestRepository.Object,
                mockAuditLogService.Object,
                mockNotificationService.Object,
                mockAcquirerDataShareRequestModelDataFactory.Object,
                mockDataShareRequestQuestionAnswerValidationService.Object,
                mockQuestionSetPlaceholderReplacementService.Object,
                mockDataShareRequestQuestionStatusesDetermination.Object,
                mockDataShareRequestQuestionSetCompletenessDetermination.Object,
                mockNextQuestionDetermination.Object,
                mockDataShareRequestNotificationRecipientDetermination.Object,
                mockKeyQuestionPartAnswerProviderService.Object,
                mockServiceOperationResultFactory.Object);

            return new TestItems(fixture,
                acquirerDataShareRequestService,
                mockLogger,
                mockUserProfilePresenter,
                mockEsdaInformationPresenter,
                mockAcquirerDataShareRequestRepository,
                mockAuditLogService,
                mockNotificationService,
                mockAcquirerDataShareRequestModelDataFactory,
                mockDataShareRequestQuestionAnswerValidationService,
                mockQuestionSetPlaceholderReplacementService,
                mockDataShareRequestQuestionStatusesDetermination,
                mockDataShareRequestQuestionSetCompletenessDetermination,
                mockNextQuestionDetermination,
                mockDataShareRequestNotificationRecipientDetermination,
                mockKeyQuestionPartAnswerProviderService,
                mockServiceOperationResultFactory);

            void ConfigureHappyPathTesting()
            {
                mockUserProfilePresenter.Setup(x => x.GetInitiatingUserIdSetAsync())
                    .ReturnsAsync(() => CreateTestUserIdSet());

                mockUserProfilePresenter.Setup(x => x.GetInitiatingUserDetailsAsync())
                    .ReturnsAsync(() => CreateTestUserDetails());

                mockEsdaInformationPresenter.Setup(x => x.GetEsdaDetailsByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync(() => CreateTestEsdaDetails());

                mockEsdaInformationPresenter.Setup(x => x.GetEsdaDetailsByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync(() => fixture.Create<EsdaDetails>());

                mockDataShareRequestQuestionAnswerValidationService.Setup(x => x.ValidateDataShareRequestQuestionAnswerAsync(
                        It.IsAny<DataShareRequestQuestionAnswer>()))
                    .ReturnsAsync(() => CreateTestDataShareRequestQuestionAnswerValidationResult());

                mockAuditLogService.Setup(x => x.GetMostRecentAuditLogForDataShareRequestStatusChangeToStatusAsync(
                        It.IsAny<Guid>(), It.IsAny<DataShareRequestStatusType?>()))
                    .ReturnsAsync(() => CreateTestAuditLogDataShareRequestStatusChangeModelData());

                mockAcquirerDataShareRequestModelDataFactory.Setup(x =>
                        x.CreateDataShareRequestRaisedForEsdaByAcquirerOrganisationSummary(
                            It.IsAny<DataShareRequestModelData>(),
                            It.IsAny<AuditLogDataShareRequestStatusChangeModelData>(),
                            It.IsAny<AuditLogDataShareRequestStatusChangeModelData?>(),
                            It.IsAny<IUserDetails>()))
                    .Returns((DataShareRequestModelData dataShareRequestModelData,
                        AuditLogDataShareRequestStatusChangeModelData auditLogForCreation,
                        AuditLogDataShareRequestStatusChangeModelData? auditLogForMostRecentSubmission,
                        IUserDetails dataShareRequestAcquirerUserDetails) =>
                    {
                        return new DataShareRequestRaisedForEsdaByAcquirerOrganisationSummary
                        {
                            Id = dataShareRequestModelData.DataShareRequest_Id,
                            RequestId = dataShareRequestModelData.DataShareRequest_RequestId,
                            Status = DoConvertDataShareRequestStatus(dataShareRequestModelData.DataShareRequest_RequestStatus),
                            DateStarted = auditLogForCreation.AuditLogDataShareRequestStatusChange_ChangedAtUtc,
                            DateSubmitted = auditLogForMostRecentSubmission?.AuditLogDataShareRequestStatusChange_ChangedAtUtc,
                            OriginatingAcquirerContactDetails = new AcquirerContactDetails
                            {
                                UserName = dataShareRequestAcquirerUserDetails.UserContactDetails.UserName,
                                EmailAddress = dataShareRequestAcquirerUserDetails.UserContactDetails.EmailAddress
                            }
                        };

                        DataShareRequestStatus DoConvertDataShareRequestStatus(DataShareRequestStatusType dataShareRequestStatusType)
                        {
                            return dataShareRequestStatusType switch
                            {
                                DataShareRequestStatusType.None => DataShareRequestStatus.Draft, // this should never happen in real, but for testing purposes this will do
                                DataShareRequestStatusType.Draft => DataShareRequestStatus.Draft,
                                DataShareRequestStatusType.Submitted => DataShareRequestStatus.Submitted,
                                DataShareRequestStatusType.Accepted => DataShareRequestStatus.Accepted,
                                DataShareRequestStatusType.Rejected => DataShareRequestStatus.Rejected,
                                DataShareRequestStatusType.Cancelled => DataShareRequestStatus.Cancelled,
                                DataShareRequestStatusType.Returned => DataShareRequestStatus.Returned,
                                DataShareRequestStatusType.InReview => DataShareRequestStatus.InReview,
                                _ => throw new Exception("Invalid DataShareRequestStatusType provided")
                            };
                        }
                    });

                mockAcquirerDataShareRequestRepository.Setup(x => x.CheckIfDataShareRequestExistsAsync(It.IsAny<Guid>()))
                    .ReturnsAsync(true);
            }
        }

    private class TestItems(
        IFixture fixture,
        IAcquirerDataShareRequestService acquirerDataShareRequestService,
        Mock<ILogger<AcquirerDataShareRequestService>> mockLogger,
        Mock<IUserProfilePresenter> mockUserProfilePresenter,
        Mock<IEsdaInformationPresenter> mockEsdaInformationPresenter,
        Mock<IAcquirerDataShareRequestRepository> mockAcquirerDataShareRequestRepository,
        Mock<IAuditLogService> mockAuditLogService,
        Mock<INotificationService> mockNotificationService,
        Mock<IAcquirerDataShareRequestModelDataFactory> mockAcquirerDataShareRequestModelDataFactory,
        Mock<IDataShareRequestQuestionAnswerValidationService> mockDataShareRequestQuestionAnswerValidationService,
        Mock<IQuestionSetPlaceholderReplacementService> mockQuestionSetPlaceholderReplacementService,
        Mock<IDataShareRequestQuestionStatusesDetermination> mockDataShareRequestQuestionStatusesDetermination,
        Mock<IDataShareRequestQuestionSetCompletenessDetermination> mockDataShareRequestQuestionSetCompletenessDetermination,
        Mock<INextQuestionDetermination> mockNextQuestionDetermination, Mock<IDataShareRequestNotificationRecipientDetermination> mockDataShareRequestNotificationRecipientDetermination,
        Mock<IKeyQuestionPartAnswerProviderService> mockKeyQuestionPartAnswerProviderService,
        Mock<IServiceOperationResultFactory> mockServiceOperationResultFactory)
        {
            public IFixture Fixture { get; } = fixture;
            public IAcquirerDataShareRequestService AcquirerDataShareRequestService { get; } = acquirerDataShareRequestService;
            public Mock<ILogger<AcquirerDataShareRequestService>> MockLogger { get; } = mockLogger;
            public Mock<IUserProfilePresenter> MockUserProfilePresenter { get; } = mockUserProfilePresenter;
            public Mock<IEsdaInformationPresenter> MockEsdaInformationPresenter { get; } = mockEsdaInformationPresenter;
            public Mock<IAcquirerDataShareRequestRepository> MockAcquirerDataShareRequestRepository { get; } = mockAcquirerDataShareRequestRepository;
            public Mock<IAuditLogService> MockAuditLogService { get; } = mockAuditLogService;
            public Mock<INotificationService> MockNotificationService { get; } = mockNotificationService;
            public Mock<IAcquirerDataShareRequestModelDataFactory> MockAcquirerDataShareRequestModelDataFactory { get; } = mockAcquirerDataShareRequestModelDataFactory;
            public Mock<IDataShareRequestQuestionAnswerValidationService> MockDataShareRequestQuestionAnswerValidationService { get; } = mockDataShareRequestQuestionAnswerValidationService;
            public Mock<IQuestionSetPlaceholderReplacementService> MockQuestionSetPlaceholderReplacementService { get; } = mockQuestionSetPlaceholderReplacementService;
            public Mock<IDataShareRequestQuestionStatusesDetermination> MockDataShareRequestQuestionStatusesDetermination { get; } = mockDataShareRequestQuestionStatusesDetermination;
            public Mock<IDataShareRequestQuestionSetCompletenessDetermination> MockDataShareRequestQuestionSetCompletenessDetermination { get; } = mockDataShareRequestQuestionSetCompletenessDetermination;
            public Mock<INextQuestionDetermination> MockNextQuestionDetermination { get; } = mockNextQuestionDetermination;
            public Mock<IDataShareRequestNotificationRecipientDetermination> MockDataShareRequestNotificationRecipientDetermination { get; } = mockDataShareRequestNotificationRecipientDetermination;
            public Mock<IKeyQuestionPartAnswerProviderService> MockKeyQuestionPartAnswerProviderService { get; } = mockKeyQuestionPartAnswerProviderService;
            public Mock<IServiceOperationResultFactory> MockServiceOperationResultFactory { get; } = mockServiceOperationResultFactory;
        }
        #endregion
    }
}