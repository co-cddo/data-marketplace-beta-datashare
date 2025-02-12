using Agrimetrics.DataShare.Api.Controllers.Acquirer.DataShareRequests;
using Agrimetrics.DataShare.Api.Dto.Models.Acquirer.DataShareRequests;
using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests;
using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests.Answers.DataShareRequestAnswerSummaries;
using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests.Answers.DataShareRequestQuestionAnswers;
using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests.Questions;
using Agrimetrics.DataShare.Api.Dto.Models.Questions.QuestionSets;
using Agrimetrics.DataShare.Api.Dto.Requests.Acquirer.DataShareRequests;
using Agrimetrics.DataShare.Api.Dto.Responses.Notifications;
using AutoFixture;
using AutoFixture.AutoMoq;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Test.Controllers.Acquirer.DataShareRequests
{
    [TestFixture]
    public class AcquirerDataShareRequestResponseFactoryTests
    {
        #region CreateGetEsdaQuestionSetOutlineResponse() Tests
        [Test]
        public void GivenANullGetEsdaQuestionSetOutlineRequest_WhenICreateGetEsdaQuestionSetOutlineResponse_ThenAnArgumentNullExceptionIsThrown()
        {
            var testItems = CreateTestItems();

            Assert.That(() => testItems.AcquirerDataShareRequestResponseFactory.CreateGetEsdaQuestionSetOutlineResponse(
                    null!, testItems.Fixture.Create<QuestionSetOutline>()),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("getEsdaQuestionSetOutlineRequest"));
        }

        [Test]
        public void GivenANullQuestionSetOutline_WhenICreateGetEsdaQuestionSetOutlineResponse_ThenAnArgumentNullExceptionIsThrown()
        {
            var testItems = CreateTestItems();

            Assert.That(() => testItems.AcquirerDataShareRequestResponseFactory.CreateGetEsdaQuestionSetOutlineResponse(
                    testItems.Fixture.Create<GetEsdaQuestionSetOutlineRequest>(), null!),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("questionSetOutline"));
        }

        [Test]
        public void GivenAGetEsdaQuestionSetOutlineRequest_WhenICreateGetEsdaQuestionSetOutlineResponse_ThenAGetEsdaQuestionSetOutlineResponseIsCreatedUsingTheValuesInTheRequest()
        {
            var testItems = CreateTestItems();

            var testGetEsdaQuestionSetOutlineRequest = testItems.Fixture.Create<GetEsdaQuestionSetOutlineRequest>();

            var result = testItems.AcquirerDataShareRequestResponseFactory.CreateGetEsdaQuestionSetOutlineResponse(
                testGetEsdaQuestionSetOutlineRequest, testItems.Fixture.Create<QuestionSetOutline>());

            Assert.That(result.EsdaId, Is.EqualTo(testGetEsdaQuestionSetOutlineRequest.EsdaId));
        }

        [Test]
        public void GivenAQuestionSetOutline_WhenICreateGetEsdaQuestionSetOutlineResponse_ThenAGetEsdaQuestionSetOutlineResponseIsCreatedUsingTheQuestionSetOutline()
        {
            var testItems = CreateTestItems();

            var testQuestionSetOutline = testItems.Fixture.Create<QuestionSetOutline>();

            var result = testItems.AcquirerDataShareRequestResponseFactory.CreateGetEsdaQuestionSetOutlineResponse(
                testItems.Fixture.Create<GetEsdaQuestionSetOutlineRequest>(), testQuestionSetOutline);

            Assert.That(result.QuestionSetOutline, Is.EqualTo(testQuestionSetOutline));
        }
        #endregion

        #region CreateStartDataShareRequestResponse() Tests
        [Test]
        public void GivenANullStartDataShareRequestRequest_WhenICreateStartDataShareRequestResponse_ThenAnArgumentNullExceptionIsThrown()
        {
            var testItems = CreateTestItems();

            Assert.That(() => testItems.AcquirerDataShareRequestResponseFactory.CreateStartDataShareRequestResponse(
                    null!, testItems.Fixture.Create<Guid>()),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("startDataShareRequestRequest"));
        }

        [Test]
        public void GivenAStartDataShareRequestRequest_WhenICreateStartDataShareRequestResponse_ThenAStartDataShareRequestResponseIsCreatedUsingTheValuesInTheRequest()
        {
            var testItems = CreateTestItems();

            var testStartDataShareRequestRequest = testItems.Fixture.Create<StartDataShareRequestRequest>();

            var result = testItems.AcquirerDataShareRequestResponseFactory.CreateStartDataShareRequestResponse(
                testStartDataShareRequestRequest, testItems.Fixture.Create<Guid>());

            Assert.Multiple(() =>
            {
                Assert.That(result.EsdaId, Is.EqualTo(testStartDataShareRequestRequest.EsdaId));
            });
        }

        [Test]
        public void GivenAStartDataShareRequestId_WhenICreateStartDataShareRequestResponse_ThenAStartDataShareRequestResponseIsCreatedUsingTheDataShareRequestId()
        {
            var testItems = CreateTestItems();

            var testDataShareRequestId = testItems.Fixture.Create<Guid>();

            var result = testItems.AcquirerDataShareRequestResponseFactory.CreateStartDataShareRequestResponse(
                testItems.Fixture.Create<StartDataShareRequestRequest>(), testDataShareRequestId);

            Assert.That(result.DataShareRequestId, Is.EqualTo(testDataShareRequestId));
        }
        #endregion

        #region CreateGetDataShareRequestSummariesResponse() Tests
        [Test]
        public void GivenANullDataShareRequestSummarySet_WhenICreateGetDataShareRequestSummariesResponse_ThenAnArgumentNullExceptionIsThrown()
        {
            var testItems = CreateTestItems();

            Assert.That(() => testItems.AcquirerDataShareRequestResponseFactory.CreateGetDataShareRequestSummariesResponse(null!),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("dataShareRequestSummaries"));
        }

        [Test]
        public void GivenADataShareRequestSummarySet_WhenICreateGetDataShareRequestSummariesResponse_ThenAStartDataShareRequestResponseIsCreatedUsingTheDataShareRequestSummarySet()
        {
            var testItems = CreateTestItems();

            var testDataShareRequestSummaries = testItems.Fixture.Create<DataShareRequestSummarySet>();

            var result = testItems.AcquirerDataShareRequestResponseFactory.CreateGetDataShareRequestSummariesResponse(
                testDataShareRequestSummaries);

            Assert.That(result.DataShareRequestSummaries, Is.EqualTo(testDataShareRequestSummaries));
        }
        #endregion

        #region CreateGetDataShareRequestSummariesResponse() Tests
        [Test]
        public void GivenANullDataShareRequestAdminSummarySet_WhenICreateGetDataShareRequestAdminSummariesResponse_ThenAnArgumentNullExceptionIsThrown()
        {
            var testItems = CreateTestItems();

            Assert.That(() => testItems.AcquirerDataShareRequestResponseFactory.CreateGetDataShareRequestAdminSummariesResponse(null!),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("dataShareRequestAdminSummaries"));
        }

        [Test]
        public void GivenADataShareRequestAdminSummarySet_WhenICreateGetDataShareRequestAdminSummariesResponse_ThenAGetDataShareRequestAdminSummariesResponseIsCreatedUsingTheDataShareRequestAdminSummarySet()
        {
            var testItems = CreateTestItems();

            var testDataShareRequestAdminSummarySet = testItems.Fixture.Create<DataShareRequestAdminSummarySet>();

            var result = testItems.AcquirerDataShareRequestResponseFactory.CreateGetDataShareRequestAdminSummariesResponse(
                testDataShareRequestAdminSummarySet);

            Assert.That(result.DataShareRequestAdminSummaries, Is.EqualTo(testDataShareRequestAdminSummarySet));
        }
        #endregion

        #region CreateGetDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisationResponse() Tests
        [Test]
        public void GivenAGetDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisationRequest_WhenICreateGetDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisationResponse_ThenAnArgumentNullExceptionIsThrown()
        {
            var testItems = CreateTestItems();

            Assert.That(() => testItems.AcquirerDataShareRequestResponseFactory.CreateGetDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisationResponse(
                    null!,
                    testItems.Fixture.Create<DataShareRequestRaisedForEsdaByAcquirerOrganisationSummarySet>()),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("getDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisationRequest"));
        }

        [Test]
        public void GivenANullDataShareRequestRaisedForEsdaByAcquirerOrganisationSummarySet_WhenICreateGetDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisationResponse_ThenAnArgumentNullExceptionIsThrown()
        {
            var testItems = CreateTestItems();

            Assert.That(() => testItems.AcquirerDataShareRequestResponseFactory.CreateGetDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisationResponse(
                    testItems.Fixture.Create<GetDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisationRequest>(),
                    null!),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("dataShareRequestRaisedForEsdaByAcquirerOrganisationSummarySet"));
        }

        [Test]
        public void GivenAGetDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisationRequest_WhenICreateGetDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisationResponse_ThenAGetDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisationResponseIsCreatedUsingTheValuesInTheRequest()
        {
            var testItems = CreateTestItems();

            var testGetDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisationRequest = testItems.Fixture.Create<GetDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisationRequest>();

            var result = testItems.AcquirerDataShareRequestResponseFactory.CreateGetDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisationResponse(
                testGetDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisationRequest,
                testItems.Fixture.Create<DataShareRequestRaisedForEsdaByAcquirerOrganisationSummarySet>());

            Assert.Multiple(() =>
            {
                Assert.That(result.EsdaId, Is.EqualTo(testGetDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisationRequest.EsdaId));
            });
        }

        [Test]
        public void GivenADataShareRequestRaisedForEsdaByAcquirerOrganisationSummarySet_WhenICreateGetDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisationResponse_ThenAGetDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisationResponseIsCreatedUsingTheValuesInTheResult()
        {
            var testItems = CreateTestItems();

            var testDataShareRequestRaisedForEsdaByAcquirerOrganisationSummarySet = testItems.Fixture.Create<DataShareRequestRaisedForEsdaByAcquirerOrganisationSummarySet>();

            var result = testItems.AcquirerDataShareRequestResponseFactory.CreateGetDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisationResponse(
                testItems.Fixture.Create<GetDataShareRequestSummariesRaisedForEsdaByAcquirerOrganisationRequest>(),
                testDataShareRequestRaisedForEsdaByAcquirerOrganisationSummarySet);

            Assert.That(result.DataShareRequestSummaries, Is.EqualTo(testDataShareRequestRaisedForEsdaByAcquirerOrganisationSummarySet));
        }
        #endregion

        #region CreateGetDataShareRequestQuestionsSummaryResponse() Tests
        [Test]
        public void GivenANullGetDataShareRequestQuestionsSummaryRequest_WhenICreateGetDataShareRequestQuestionsSummaryResponse_ThenAnArgumentNullExceptionIsThrown()
        {
            var testItems = CreateTestItems();

            Assert.That(() => testItems.AcquirerDataShareRequestResponseFactory.CreateGetDataShareRequestQuestionsSummaryResponse(
                    null!, testItems.Fixture.Create<DataShareRequestQuestionsSummary>()),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("getDataShareRequestQuestionsSummaryRequest"));
        }

        [Test]
        public void GivenANullDataShareRequestQuestionsSummary_WhenICreateGetDataShareRequestQuestionsSummaryResponse_ThenAnArgumentNullExceptionIsThrown()
        {
            var testItems = CreateTestItems();

            Assert.That(() => testItems.AcquirerDataShareRequestResponseFactory.CreateGetDataShareRequestQuestionsSummaryResponse(
                    testItems.Fixture.Create<GetDataShareRequestQuestionsSummaryRequest>(), null!),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("dataShareRequestQuestionsSummary"));
        }

        [Test]
        public void GivenAGetDataShareRequestQuestionsSummaryRequest_WhenICreateGetDataShareRequestQuestionsSummaryResponse_ThenAGetDataShareRequestQuestionsSummaryResponseIsCreatedUsingTheValuesInTheRequest()
        {
            var testItems = CreateTestItems();

            var testGetDataShareRequestQuestionsSummaryRequest = testItems.Fixture.Create<GetDataShareRequestQuestionsSummaryRequest>();

            var result = testItems.AcquirerDataShareRequestResponseFactory.CreateGetDataShareRequestQuestionsSummaryResponse(
                testGetDataShareRequestQuestionsSummaryRequest, testItems.Fixture.Create<DataShareRequestQuestionsSummary>());

            Assert.Multiple(() =>
            {
                Assert.That(result.DataShareRequestId, Is.EqualTo(testGetDataShareRequestQuestionsSummaryRequest.DataShareRequestId));
            });
        }

        [Theory]
        public void GivenADataShareRequestQuestionsSummary_WhenICreateGetDataShareRequestQuestionsSummaryResponse_ThenAGetDataShareRequestQuestionsSummaryResponseIsCreatedUsingTheValuesInTheSummary(
            DataShareRequestStatus dataShareRequestStatus,
            bool questionsRemainThatRequireAResponse)
        {
            var testItems = CreateTestItems();

            var testDataShareRequestQuestionsSummary = testItems.Fixture.Create<DataShareRequestQuestionsSummary>();

            var result = testItems.AcquirerDataShareRequestResponseFactory.CreateGetDataShareRequestQuestionsSummaryResponse(
                testItems.Fixture.Create<GetDataShareRequestQuestionsSummaryRequest>(), testDataShareRequestQuestionsSummary);

            Assert.Multiple(() =>
            {
                Assert.That(result.DataShareRequestRequestId, Is.EqualTo(testDataShareRequestQuestionsSummary.DataShareRequestRequestId));
                Assert.That(result.QuestionSetSummary, Is.EqualTo(testDataShareRequestQuestionsSummary.QuestionSetSummary));
                Assert.That(result.EsdaName, Is.EqualTo(testDataShareRequestQuestionsSummary.EsdaName));
            });
        }
        #endregion

        #region CreateGetDataShareRequestQuestionInformationResponse() Tests
        [Test]
        public void GivenANullGetDataShareRequestQuestionInformationRequest_WhenICreateGetDataShareRequestQuestionInformationResponse_ThenAnArgumentNullExceptionIsThrown()
        {
            var testItems = CreateTestItems();

            Assert.That(() => testItems.AcquirerDataShareRequestResponseFactory.CreateGetDataShareRequestQuestionInformationResponse(
                    null!, testItems.Fixture.Create<DataShareRequestQuestion>()),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("getDataShareRequestQuestionInformationRequest"));
        }

        [Test]
        public void GivenANullDataShareRequestQuestion_WhenICreateGetDataShareRequestQuestionsSummaryResponse_ThenAnArgumentNullExceptionIsThrown()
        {
            var testItems = CreateTestItems();

            Assert.That(() => testItems.AcquirerDataShareRequestResponseFactory.CreateGetDataShareRequestQuestionInformationResponse(
                    testItems.Fixture.Create<GetDataShareRequestQuestionInformationRequest>(), null!),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("dataShareRequestQuestion"));
        }

        [Test]
        public void GivenAGetDataShareRequestQuestionInformationRequest_WhenICreateGetDataShareRequestQuestionInformationResponse_ThenAGetDataShareRequestQuestionInformationResponseIsCreatedUsingTheValuesInTheRequest()
        {
            var testItems = CreateTestItems();

            var testGetDataShareRequestQuestionInformationRequest = testItems.Fixture.Create<GetDataShareRequestQuestionInformationRequest>();

            var result = testItems.AcquirerDataShareRequestResponseFactory.CreateGetDataShareRequestQuestionInformationResponse(
                testGetDataShareRequestQuestionInformationRequest, testItems.Fixture.Create<DataShareRequestQuestion>());

            Assert.Multiple(() =>
            {
                Assert.That(result.DataShareRequestId, Is.EqualTo(testGetDataShareRequestQuestionInformationRequest.DataShareRequestId));
                Assert.That(result.QuestionId, Is.EqualTo(testGetDataShareRequestQuestionInformationRequest.QuestionId));
            });
        }

        [Test]
        public void GivenADataShareRequestQuestion_WhenICreateGetDataShareRequestQuestionInformationResponse_ThenAGetDataShareRequestQuestionInformationResponseIsCreatedUsingTheDataShareRequestQuestion()
        {
            var testItems = CreateTestItems();

            var testDataShareRequestQuestion = testItems.Fixture.Create<DataShareRequestQuestion>();

            var result = testItems.AcquirerDataShareRequestResponseFactory.CreateGetDataShareRequestQuestionInformationResponse(
                testItems.Fixture.Create<GetDataShareRequestQuestionInformationRequest>(), testDataShareRequestQuestion);

            Assert.That(result.DataShareRequestQuestion, Is.EqualTo(testDataShareRequestQuestion));
        }
        #endregion

        #region CreateSetDataShareRequestQuestionAnswerResponse() Tests
        [Test]
        public void GivenANullSetDataShareRequestQuestionAnswerRequest_WhenICreateSetDataShareRequestQuestionAnswerResponse_ThenAnArgumentNullExceptionIsThrown()
        {
            var testItems = CreateTestItems();

            Assert.That(() => testItems.AcquirerDataShareRequestResponseFactory.CreateSetDataShareRequestQuestionAnswerResponse(
                    null!, testItems.Fixture.Create<SetDataShareRequestQuestionAnswerResult>()),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("setDataShareRequestQuestionAnswerRequest"));
        }

        [Test]
        public void GivenANullSetDataShareRequestQuestionAnswerResult_WhenICreateSetDataShareRequestQuestionAnswerResponse_ThenAnArgumentNullExceptionIsThrown()
        {
            var testItems = CreateTestItems();

            Assert.That(() => testItems.AcquirerDataShareRequestResponseFactory.CreateSetDataShareRequestQuestionAnswerResponse(
                    testItems.Fixture.Create<SetDataShareRequestQuestionAnswerRequest>(), null!),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("setDataShareRequestQuestionAnswerResult"));
        }

        [Test]
        [TestCaseSource(nameof(SetDataShareRequestQuestionAnswerResultTestCaseData))]
        public void GivenASetDataShareRequestQuestionAnswerResult_WhenICreateSetDataShareRequestQuestionAnswerResponse_ThenASetDataShareRequestQuestionAnswerResponseIsCreatedUsingTheValuesInTheResult(
            Guid? nextQuestionId)
        {
            var testItems = CreateTestItems();

            var testSetDataShareRequestQuestionAnswerResult = testItems.Fixture.Build<SetDataShareRequestQuestionAnswerResult>()
                .With(x => x.NextQuestionId, nextQuestionId)
                .Create();

            var result = testItems.AcquirerDataShareRequestResponseFactory.CreateSetDataShareRequestQuestionAnswerResponse(
                testItems.Fixture.Create<SetDataShareRequestQuestionAnswerRequest>(), testSetDataShareRequestQuestionAnswerResult);

            Assert.That(result.Result, Is.EqualTo(testSetDataShareRequestQuestionAnswerResult));
        }

        private static IEnumerable<TestCaseData> SetDataShareRequestQuestionAnswerResultTestCaseData()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());

            var nextQuestionId = fixture.Create<Guid>();

            yield return new TestCaseData(nextQuestionId);
            yield return new TestCaseData(null);
        }
        #endregion

        #region CreateGetDataShareRequestAnswersSummaryResponse() Tests
        [Test]
        public void GivenANullGetDataShareRequestAnswersSummaryRequest_WhenICreateGetDataShareRequestAnswersSummaryResponse_ThenAnArgumentNullExceptionIsThrown()
        {
            var testItems = CreateTestItems();

            Assert.That(() => testItems.AcquirerDataShareRequestResponseFactory.CreateGetDataShareRequestAnswersSummaryResponse(
                    null!, testItems.Fixture.Create<DataShareRequestAnswersSummary>()),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("getDataShareRequestAnswersSummaryRequest"));
        }

        [Test]
        public void GivenANullSetDataShareRequestQuestionAnswerResult_WhenICreateGetDataShareRequestAnswersSummaryResponse_ThenAnArgumentNullExceptionIsThrown()
        {
            var testItems = CreateTestItems();

            Assert.That(() => testItems.AcquirerDataShareRequestResponseFactory.CreateGetDataShareRequestAnswersSummaryResponse(
                    testItems.Fixture.Create<GetDataShareRequestAnswersSummaryRequest>(), null!),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("dataShareRequestAnswersSummary"));
        }

        [Test]
        public void GivenAGetDataShareRequestAnswersSummaryRequest_WhenICreateGetDataShareRequestAnswersSummaryResponse_ThenAGetDataShareRequestAnswersSummaryResponseIsCreatedUsingTheValuesInTheRequest()
        {
            var testItems = CreateTestItems();

            var testGetDataShareRequestAnswersSummaryRequest = testItems.Fixture.Create<GetDataShareRequestAnswersSummaryRequest>();

            var result = testItems.AcquirerDataShareRequestResponseFactory.CreateGetDataShareRequestAnswersSummaryResponse(
                testGetDataShareRequestAnswersSummaryRequest, testItems.Fixture.Create<DataShareRequestAnswersSummary>());

            Assert.Multiple(() =>
            {
                Assert.That(result.DataShareRequestId, Is.EqualTo(testGetDataShareRequestAnswersSummaryRequest.DataShareRequestId));
            });
        }

        [Test]
        public void GivenADataShareRequestAnswersSummary_WhenICreateGetDataShareRequestAnswersSummaryResponse_ThenAGetDataShareRequestAnswersSummaryResponseIsCreatedUsingTheSummary()
        {
            var testItems = CreateTestItems();

            var testGetDataShareRequestAnswersSummaryRequest = testItems.Fixture.Create<DataShareRequestAnswersSummary>();

            var result = testItems.AcquirerDataShareRequestResponseFactory.CreateGetDataShareRequestAnswersSummaryResponse(
                testItems.Fixture.Create<GetDataShareRequestAnswersSummaryRequest>(), testGetDataShareRequestAnswersSummaryRequest);

            Assert.That(result.AnswersSummary, Is.EqualTo(testGetDataShareRequestAnswersSummaryRequest));
        }
        #endregion

        #region CreateSubmitDataShareRequestResponse() Tests
        [Test]
        public void GivenANullSubmitDataShareRequestRequest_WhenICreateSubmitDataShareRequestResponse_ThenAnArgumentNullExceptionIsThrown()
        {
            var testItems = CreateTestItems();

            Assert.That(() => testItems.AcquirerDataShareRequestResponseFactory.CreateSubmitDataShareRequestResponse(
                    null!,
                    testItems.Fixture.Create<DataShareRequestSubmissionResult>()),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("submitDataShareRequestRequest"));
        }

        [Test]
        public void GivenANullDataShareRequestSubmissionResult_WhenICreateSubmitDataShareRequestResponse_ThenAnArgumentNullExceptionIsThrown()
        {
            var testItems = CreateTestItems();

            Assert.That(() => testItems.AcquirerDataShareRequestResponseFactory.CreateSubmitDataShareRequestResponse(
                    testItems.Fixture.Create<SubmitDataShareRequestRequest>(),
                    null!),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("dataShareRequestSubmissionResult"));
        }

        [Test]
        public void GivenASubmitDataShareRequestRequest_WhenICreateSubmitDataShareRequestResponse_ThenASubmitDataShareRequestResponseIsCreatedUsingTheValuesInTheRequest()
        {
            var testItems = CreateTestItems();

            var testSubmitDataShareRequestRequest = testItems.Fixture.Create<SubmitDataShareRequestRequest>();

            var result = testItems.AcquirerDataShareRequestResponseFactory.CreateSubmitDataShareRequestResponse(
                testSubmitDataShareRequestRequest,
                testItems.Fixture.Create<DataShareRequestSubmissionResult>());

            Assert.Multiple(() =>
            {
                Assert.That(result.DataShareRequestId, Is.EqualTo(testSubmitDataShareRequestRequest.DataShareRequestId));
            });
        }

        [Theory]
        public void GivenADataShareRequestSubmissionResult_WhenICreateSubmitDataShareRequestResponse_ThenASubmitDataShareRequestResponseIsCreatedUsingTheValuesInTheResult(
            NotificationSuccess notificationSuccess)
        {
            var testItems = CreateTestItems();

            var testDataShareRequestSubmissionResult = testItems.Fixture.Build<DataShareRequestSubmissionResult>()
                .With(x => x.NotificationSuccess, notificationSuccess)
                .Create();

            var result = testItems.AcquirerDataShareRequestResponseFactory.CreateSubmitDataShareRequestResponse(
                testItems.Fixture.Create<SubmitDataShareRequestRequest>(),
                testDataShareRequestSubmissionResult);

            Assert.Multiple(() =>
            {
                Assert.That(result.DataShareRequestRequestId, Is.EqualTo(testDataShareRequestSubmissionResult.DataShareRequestRequestId));

                Assert.That(result.NotificationSuccess, Is.EqualTo(testDataShareRequestSubmissionResult.NotificationSuccess));
            });
            
        }
        #endregion

        #region CreateCancelDataShareRequestResponse() Tests
        [Test]
        public void GivenANullCancelDataShareRequestRequest_WhenICreateCancelDataShareRequestResponse_ThenAnArgumentNullExceptionIsThrown()
        {
            var testItems = CreateTestItems();

            Assert.That(() => testItems.AcquirerDataShareRequestResponseFactory.CreateCancelDataShareRequestResponse(
                    null!,
                    testItems.Fixture.Create<DataShareRequestCancellationResult>()),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("cancelDataShareRequestRequest"));
        }

        [Test]
        public void GivenANullCancelDataShareRequestResult_WhenICreateCancelDataShareRequestResponse_ThenAnArgumentNullExceptionIsThrown()
        {
            var testItems = CreateTestItems();

            Assert.That(() => testItems.AcquirerDataShareRequestResponseFactory.CreateCancelDataShareRequestResponse(
                    testItems.Fixture.Create<CancelDataShareRequestRequest>(),
                    null!),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("dataShareRequestCancellationResult"));
        }

        [Test]
        public void GivenACancelDataShareRequestRequest_WhenICreateCancelDataShareRequestResponse_ThenACancelDataShareRequestResponseIsCreatedUsingTheValuesInTheRequest()
        {
            var testItems = CreateTestItems();

            var testDataShareRequestCancellationRequest = testItems.Fixture.Create<CancelDataShareRequestRequest>();

            var result = testItems.AcquirerDataShareRequestResponseFactory.CreateCancelDataShareRequestResponse(
                testDataShareRequestCancellationRequest,
                testItems.Fixture.Create<DataShareRequestCancellationResult>());

            Assert.Multiple(() =>
            {
                Assert.That(result.DataShareRequestId, Is.EqualTo(testDataShareRequestCancellationRequest.DataShareRequestId));
                Assert.That(result.ReasonsForCancellation, Is.EqualTo(testDataShareRequestCancellationRequest.ReasonsForCancellation));
            });
        }

        [Theory]
        public void GivenACancelDataShareRequestRequest_WhenICreateCancelDataShareRequestResponse_ThenACancelDataShareRequestResponseIsCreatedUsingTheValuesInTheResponse(
            NotificationSuccess notificationSuccess)
        {
            var testItems = CreateTestItems();

            var testDataShareRequestCancellationResult = testItems.Fixture.Build<DataShareRequestCancellationResult>()
                .With(x => x.NotificationSuccess, notificationSuccess)
                .Create();

            var result = testItems.AcquirerDataShareRequestResponseFactory.CreateCancelDataShareRequestResponse(
                testItems.Fixture.Create<CancelDataShareRequestRequest>(),
                testDataShareRequestCancellationResult);

            Assert.Multiple(() =>
            {
                Assert.That(result.NotificationSuccess, Is.EqualTo(testDataShareRequestCancellationResult.NotificationSuccess));
            });
        }
        #endregion

        #region CreateDeleteDataShareRequestResponse() Tests
        [Test]
        public void GivenANullDeleteDataShareRequestRequest_WhenICreateDeleteDataShareRequestResponse_ThenAnArgumentNullExceptionIsThrown()
        {
            var testItems = CreateTestItems();

            Assert.That(() => testItems.AcquirerDataShareRequestResponseFactory.CreateDeleteDataShareRequestResponse(
                    null!,
                    testItems.Fixture.Create<DataShareRequestDeletionResult>()),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("deleteDataShareRequestRequest"));
        }

        [Test]
        public void GivenANullDeleteDataShareRequestResult_WhenICreateDeleteDataShareRequestResponse_ThenAnArgumentNullExceptionIsThrown()
        {
            var testItems = CreateTestItems();

            Assert.That(() => testItems.AcquirerDataShareRequestResponseFactory.CreateDeleteDataShareRequestResponse(
                    testItems.Fixture.Create<DeleteDataShareRequestRequest>(),
                    null!),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("dataShareRequestDeletionResult"));
        }

        [Test]
        public void GivenADeleteDataShareRequestRequest_WhenICreateDeleteDataShareRequestResponse_ThenADeleteDataShareRequestResponseIsCreatedUsingTheValuesInTheRequest()
        {
            var testItems = CreateTestItems();

            var testDataShareRequestDeletionRequest = testItems.Fixture.Create<DeleteDataShareRequestRequest>();

            var testDeletionResult = testItems.Fixture.Build<DataShareRequestDeletionResult>()
                .With(x => x.DataShareRequestId, testDataShareRequestDeletionRequest.DataShareRequestId)
                .Create();

            var result = testItems.AcquirerDataShareRequestResponseFactory.CreateDeleteDataShareRequestResponse(
                testDataShareRequestDeletionRequest,
                testDeletionResult);

            Assert.That(result.DataShareRequestId, Is.EqualTo(testDataShareRequestDeletionRequest.DataShareRequestId));
        }
        #endregion

        #region Test Item Creation
        private static TestItems CreateTestItems()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());

            var acquirerDataShareRequestResponseFactory = new AcquirerDataShareRequestResponseFactory();

            return new TestItems(fixture, acquirerDataShareRequestResponseFactory);
        }

        private class TestItems(
            IFixture fixture,
            IAcquirerDataShareRequestResponseFactory acquirerDataShareRequestResponseFactory)
        {
            public IFixture Fixture { get; } = fixture;
            public IAcquirerDataShareRequestResponseFactory AcquirerDataShareRequestResponseFactory { get; } = acquirerDataShareRequestResponseFactory;
        }
        #endregion
    }
}
