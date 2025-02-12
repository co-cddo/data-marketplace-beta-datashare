using Agrimetrics.DataShare.Api.Controllers.Supplier.DataShareRequests;
using AutoFixture.AutoMoq;
using AutoFixture;
using NUnit.Framework;
using Agrimetrics.DataShare.Api.Dto.Models.Supplier.DataShareRequests;
using Agrimetrics.DataShare.Api.Dto.Requests.Supplier;

namespace Agrimetrics.DataShare.Api.Test.Controllers.Supplier.DataShareRequests
{
    [TestFixture]
    public class SupplierDataShareRequestResponseFactoryTests
    {
        #region CreateGetSubmissionSummariesResponse() Tests
        [Test]
        public void GivenANullSubmissionSummariesSet_WhenICreateGetSubmissionSummariesResponse_ThenAnArgumentNullExceptionIsThrown()
        {
            var testItems = CreateTestItems();

            Assert.That(() => testItems.SupplierDataShareRequestResponseFactory.CreateGetSubmissionSummariesResponse(
                    null!),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("submissionSummariesSet"));
        }

        [Test]
        public void GivenASubmissionSummariesSet_WhenICreateGetSubmissionSummariesResponse_ThenAGetSubmissionSummariesResponseIsCreatedUsingTheSubmissionSummariesSet()
        {
            var testItems = CreateTestItems();

            var testSubmissionSummariesSet = testItems.Fixture.Create<SubmissionSummariesSet>();

            var result = testItems.SupplierDataShareRequestResponseFactory.CreateGetSubmissionSummariesResponse(
                testSubmissionSummariesSet);

            Assert.That(result.SubmissionSummariesSet, Is.EqualTo(testSubmissionSummariesSet));
        }
        #endregion

        #region CreateGetSubmissionInformationResponse() Tests
        [Test]
        public void GivenANullGetSubmissionToReviewSummaryRequest_WhenICreateGetSubmissionInformationResponse_ThenAnArgumentNullExceptionIsThrown()
        {
            var testItems = CreateTestItems();

            Assert.That(() => testItems.SupplierDataShareRequestResponseFactory.CreateGetSubmissionInformationResponse(
                    null!,
                    testItems.Fixture.Create<SubmissionInformation>()),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("getSubmissionInformationRequest"));
        }

        [Test]
        public void GivenANullSubmissionInformation_WhenICreateGetSubmissionInformationResponse_ThenAnArgumentNullExceptionIsThrown()
        {
            var testItems = CreateTestItems();

            Assert.That(() => testItems.SupplierDataShareRequestResponseFactory.CreateGetSubmissionInformationResponse(
                    testItems.Fixture.Create<GetSubmissionInformationRequest>(),
                    null!),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("submissionInformation"));
        }

        [Test]
        public void GivenAGetSubmissionToReviewSummaryRequest_WhenICreateGetSubmissionInformationResponse_ThenAGetSubmissionToReviewSummaryResponseIsCreatedUsingTheValuesInTheRequest()
        {
            var testItems = CreateTestItems();

            var testGetSubmissionToReviewSummaryRequest = testItems.Fixture.Create<GetSubmissionInformationRequest>();

            var result = testItems.SupplierDataShareRequestResponseFactory.CreateGetSubmissionInformationResponse(
                testGetSubmissionToReviewSummaryRequest,
                testItems.Fixture.Create<SubmissionInformation>());

            Assert.That(result.DataShareRequestId, Is.EqualTo(testGetSubmissionToReviewSummaryRequest.DataShareRequestId));
        }

        [Test]
        public void GivenASubmissionInformation_WhenICreateGetSubmissionInformationResponse_ThenAGetSubmissionToReviewSummaryResponseIsCreatedUsingTheSubmissionToReviewSummary()
        {
            var testItems = CreateTestItems();

            var testSubmissionInformation = testItems.Fixture.Create<SubmissionInformation>();

            var result = testItems.SupplierDataShareRequestResponseFactory.CreateGetSubmissionInformationResponse(
                testItems.Fixture.Create<GetSubmissionInformationRequest>(),
                testSubmissionInformation);

            Assert.That(result.SubmissionInformation, Is.EqualTo(testSubmissionInformation));
        }
        #endregion

        #region CreateGetSubmissionDetailsResponse() Tests
        [Test]
        public void GivenANullGetSubmissionDetailsRequest_WhenICreateGetSubmissionDetailsResponse_ThenAnArgumentNullExceptionIsThrown()
        {
            var testItems = CreateTestItems();

            Assert.That(() => testItems.SupplierDataShareRequestResponseFactory.CreateGetSubmissionDetailsResponse(
                    null!,
                    testItems.Fixture.Create<SubmissionDetails>()),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("getSubmissionDetailsRequest"));
        }

        [Test]
        public void GivenANullSubmissionDetails_WhenICreateGetSubmissionDetailsResponse_ThenAnArgumentNullExceptionIsThrown()
        {
            var testItems = CreateTestItems();

            Assert.That(() => testItems.SupplierDataShareRequestResponseFactory.CreateGetSubmissionDetailsResponse(
                    testItems.Fixture.Create<GetSubmissionDetailsRequest>(),
                    null!),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("submissionDetails"));
        }

        [Test]
        public void GivenAGetSubmissionDetailsRequest_WhenICreateGetSubmissionDetailsResponse_ThenAGetSubmissionDetailsResponseIsCreatedUsingTheValuesInTheRequest()
        {
            var testItems = CreateTestItems();

            var testGetSubmissionDetailsRequest = testItems.Fixture.Create<GetSubmissionDetailsRequest>();

            var result = testItems.SupplierDataShareRequestResponseFactory.CreateGetSubmissionDetailsResponse(
                testGetSubmissionDetailsRequest,
                testItems.Fixture.Create<SubmissionDetails>());

            Assert.That(result.DataShareRequestId, Is.EqualTo(testGetSubmissionDetailsRequest.DataShareRequestId));
        }

        [Test]
        public void GivenASubmissionDetails_WhenICreateGetSubmissionDetailsResponse_ThenAGetSubmissionDetailsResponseIsCreatedUsingTheSubmissionDetails()
        {
            var testItems = CreateTestItems();

            var testSubmissionDetails = testItems.Fixture.Create<SubmissionDetails>();

            var result = testItems.SupplierDataShareRequestResponseFactory.CreateGetSubmissionDetailsResponse(
                testItems.Fixture.Create<GetSubmissionDetailsRequest>(),
                testSubmissionDetails);

            Assert.That(result.SubmissionDetails, Is.EqualTo(testSubmissionDetails));
        }
        #endregion

        #region CreateStartSubmissionReviewResponse() Tests
        [Test]
        public void GivenANullStartSubmissionReviewRequest_WhenICreateStartSubmissionReviewResponse_ThenAnArgumentNullExceptionIsThrown()
        {
            var testItems = CreateTestItems();

            Assert.That(() => testItems.SupplierDataShareRequestResponseFactory.CreateStartSubmissionReviewResponse(
                    null!,
                    testItems.Fixture.Create<SubmissionReviewInformation>()),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("startSubmissionReviewRequest"));
        }

        [Test]
        public void GivenANullSubmissionReviewInformation_WhenICreateStartSubmissionReviewResponse_ThenAnArgumentNullExceptionIsThrown()
        {
            var testItems = CreateTestItems();

            Assert.That(() => testItems.SupplierDataShareRequestResponseFactory.CreateStartSubmissionReviewResponse(
                    testItems.Fixture.Create<StartSubmissionReviewRequest>(),
                    null!),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("submissionReviewInformation"));
        }

        [Test]
        public void GivenAStartSubmissionReviewRequest_WhenICreateStartSubmissionReviewResponse_ThenAStartSubmissionReviewResponseIsCreatedUsingTheValuesInTheRequest()
        {
            var testItems = CreateTestItems();

            var testStartSubmissionReviewRequest = testItems.Fixture.Create<StartSubmissionReviewRequest>();

            var result = testItems.SupplierDataShareRequestResponseFactory.CreateStartSubmissionReviewResponse(
                testStartSubmissionReviewRequest,
                testItems.Fixture.Create<SubmissionReviewInformation>());

            Assert.That(result.DataShareRequestId, Is.EqualTo(testStartSubmissionReviewRequest.DataShareRequestId));
        }

        [Test]
        public void GivenASubmissionReviewInformation_WhenICreateStartSubmissionReviewResponse_ThenAStartSubmissionReviewResponseIsCreatedUsingTheSubmissionReviewInformation()
        {
            var testItems = CreateTestItems();

            var testSubmissionReviewInformation = testItems.Fixture.Create<SubmissionReviewInformation>();

            var result = testItems.SupplierDataShareRequestResponseFactory.CreateStartSubmissionReviewResponse(
                testItems.Fixture.Create<StartSubmissionReviewRequest>(),
                testSubmissionReviewInformation);

            Assert.That(result.SubmissionReviewInformation, Is.EqualTo(testSubmissionReviewInformation));
        }
        #endregion

        #region CreateGetSubmissionReviewInformationResponse() Tests
        [Test]
        public void GivenANullGetSubmissionReviewInformationRequest_WhenICreateGetSubmissionReviewInformationResponse_ThenAnArgumentNullExceptionIsThrown()
        {
            var testItems = CreateTestItems();

            Assert.That(() => testItems.SupplierDataShareRequestResponseFactory.CreateGetSubmissionReviewInformationResponse(
                    null!,
                    testItems.Fixture.Create<SubmissionReviewInformation>()),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("getSubmissionReviewInformationRequest"));
        }

        [Test]
        public void GivenANullSubmissionReviewInformation_WhenICreateGetSubmissionReviewInformationResponse_ThenAnArgumentNullExceptionIsThrown()
        {
            var testItems = CreateTestItems();

            Assert.That(() => testItems.SupplierDataShareRequestResponseFactory.CreateGetSubmissionReviewInformationResponse(
                    testItems.Fixture.Create<GetSubmissionReviewInformationRequest>(),
                    null!),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("submissionReviewInformation"));
        }

        [Test]
        public void GivenAGetSubmissionReviewInformationRequest_WhenICreateGetSubmissionReviewInformationResponse_ThenAGetSubmissionReviewInformationResponseIsCreatedUsingTheValuesInTheRequest()
        {
            var testItems = CreateTestItems();

            var testGetSubmissionReviewInformationRequest = testItems.Fixture.Create<GetSubmissionReviewInformationRequest>();

            var result = testItems.SupplierDataShareRequestResponseFactory.CreateGetSubmissionReviewInformationResponse(
                testGetSubmissionReviewInformationRequest,
                testItems.Fixture.Create<SubmissionReviewInformation>());

            Assert.That(result.DataShareRequestId, Is.EqualTo(testGetSubmissionReviewInformationRequest.DataShareRequestId));
        }

        [Test]
        public void GivenASubmissionReviewInformation_WhenICreateGetSubmissionReviewInformationResponse_ThenAGetSubmissionReviewInformationResponseIsCreatedUsingTheSubmissionReviewInformation()
        {
            var testItems = CreateTestItems();

            var testGetSubmissionReviewInformationRequest = testItems.Fixture.Create<SubmissionReviewInformation>();

            var result = testItems.SupplierDataShareRequestResponseFactory.CreateGetSubmissionReviewInformationResponse(
                testItems.Fixture.Create<GetSubmissionReviewInformationRequest>(),
                testGetSubmissionReviewInformationRequest);

            Assert.That(result.SubmissionReviewInformation, Is.EqualTo(testGetSubmissionReviewInformationRequest));
        }
        #endregion

        #region CreateGetReturnedSubmissionInformationResponse() Tests
        [Test]
        public void GivenANullGetReturnedSubmissionInformationRequest_WhenICreateGetReturnedSubmissionInformationResponse_ThenAnArgumentNullExceptionIsThrown()
        {
            var testItems = CreateTestItems();

            Assert.That(() => testItems.SupplierDataShareRequestResponseFactory.CreateGetReturnedSubmissionInformationResponse(
                    null!,
                    testItems.Fixture.Create<ReturnedSubmissionInformation>()),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("getReturnedSubmissionInformationRequest"));
        }

        [Test]
        public void GivenANullReturnedSubmissionInformation_WhenICreateGetReturnedSubmissionInformationResponse_ThenAnArgumentNullExceptionIsThrown()
        {
            var testItems = CreateTestItems();

            Assert.That(() => testItems.SupplierDataShareRequestResponseFactory.CreateGetReturnedSubmissionInformationResponse(
                    testItems.Fixture.Create<GetReturnedSubmissionInformationRequest>(),
                    null!),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("returnedSubmissionInformation"));
        }

        [Test]
        public void GivenAGetReturnedSubmissionInformationRequest_WhenICreateGetReturnedSubmissionInformationResponse_ThenAGetReturnedSubmissionInformationResponseIsCreatedUsingTheValuesInTheRequest()
        {
            var testItems = CreateTestItems();

            var testGetReturnedSubmissionInformationRequest = testItems.Fixture.Create<GetReturnedSubmissionInformationRequest>();

            var result = testItems.SupplierDataShareRequestResponseFactory.CreateGetReturnedSubmissionInformationResponse(
                testGetReturnedSubmissionInformationRequest,
                testItems.Fixture.Create<ReturnedSubmissionInformation>());

            Assert.That(result.DataShareRequestId, Is.EqualTo(testGetReturnedSubmissionInformationRequest.DataShareRequestId));
        }

        [Test]
        public void GivenAReturnedSubmissionInformation_WhenICreateGetReturnedSubmissionInformationResponse_ThenAGetReturnedSubmissionInformationResponseIsCreatedUsingTheReturnedSubmissionInformation()
        {
            var testItems = CreateTestItems();

            var testReturnedSubmissionInformation = testItems.Fixture.Create<ReturnedSubmissionInformation>();

            var result = testItems.SupplierDataShareRequestResponseFactory.CreateGetReturnedSubmissionInformationResponse(
                testItems.Fixture.Create<GetReturnedSubmissionInformationRequest>(),
                testReturnedSubmissionInformation);

            Assert.That(result.ReturnedSubmissionInformation, Is.EqualTo(testReturnedSubmissionInformation));
        }
        #endregion

        #region CreateGetCompletedSubmissionInformationResponse() Tests
        [Test]
        public void GivenANullGetCompletedSubmissionInformationRequest_WhenICreateGetCompletedSubmissionInformationResponse_ThenAnArgumentNullExceptionIsThrown()
        {
            var testItems = CreateTestItems();

            Assert.That(() => testItems.SupplierDataShareRequestResponseFactory.CreateGetCompletedSubmissionInformationResponse(
                    null!,
                    testItems.Fixture.Create<CompletedSubmissionInformation>()),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("getCompletedSubmissionInformationRequest"));
        }

        [Test]
        public void GivenANullCompletedSubmissionInformation_WhenICreateGetCompletedSubmissionInformationResponse_ThenAnArgumentNullExceptionIsThrown()
        {
            var testItems = CreateTestItems();

            Assert.That(() => testItems.SupplierDataShareRequestResponseFactory.CreateGetCompletedSubmissionInformationResponse(
                    testItems.Fixture.Create<GetCompletedSubmissionInformationRequest>(),
                    null!),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("completedSubmissionInformation"));
        }

        [Test]
        public void GivenAGetCompletedSubmissionInformationRequest_WhenICreateGetCompletedSubmissionInformationResponse_ThenAGetCompletedSubmissionInformationResponseIsCreatedUsingTheValuesInTheRequest()
        {
            var testItems = CreateTestItems();

            var testGetCompletedSubmissionInformationRequest = testItems.Fixture.Create<GetCompletedSubmissionInformationRequest>();

            var result = testItems.SupplierDataShareRequestResponseFactory.CreateGetCompletedSubmissionInformationResponse(
                testGetCompletedSubmissionInformationRequest,
                testItems.Fixture.Create<CompletedSubmissionInformation>());

            Assert.That(result.DataShareRequestId, Is.EqualTo(testGetCompletedSubmissionInformationRequest.DataShareRequestId));
        }

        [Test]
        public void GivenACompletedSubmissionInformation_WhenICreateGetCompletedSubmissionInformationResponse_ThenAGetCompletedSubmissionInformationResponseIsCreatedUsingTheCompletedSubmissionInformation()
        {
            var testItems = CreateTestItems();

            var testCompletedSubmissionInformation = testItems.Fixture.Create<CompletedSubmissionInformation>();

            var result = testItems.SupplierDataShareRequestResponseFactory.CreateGetCompletedSubmissionInformationResponse(
                testItems.Fixture.Create<GetCompletedSubmissionInformationRequest>(),
                testCompletedSubmissionInformation);

            Assert.That(result.CompletedSubmissionInformation, Is.EqualTo(testCompletedSubmissionInformation));
        }
        #endregion

        #region CreateSetSubmissionNotesResponse() Tests
        [Test]
        public void GivenANullSetSubmissionNotesRequest_WhenICreateSetSubmissionNotesResponse_ThenAnArgumentNullExceptionIsThrown()
        {
            var testItems = CreateTestItems();

            Assert.That(() => testItems.SupplierDataShareRequestResponseFactory.CreateSetSubmissionNotesResponse(
                    null!),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("setSubmissionNotesRequest"));
        }

        [Test]
        public void GivenASetSubmissionNotesRequest_WhenICreateSetSubmissionNotesResponse_ThenASetSubmissionNotesResponseIsCreatedUsingTheValuesInTheRequest()
        {
            var testItems = CreateTestItems();

            var testSetSubmissionNotesRequest = testItems.Fixture.Create<SetSubmissionNotesRequest>();

            var result = testItems.SupplierDataShareRequestResponseFactory.CreateSetSubmissionNotesResponse(
                testSetSubmissionNotesRequest);

            Assert.That(result.DataShareRequestId, Is.EqualTo(testSetSubmissionNotesRequest.DataShareRequestId));
        }
        #endregion

        #region CreateAcceptSubmissionResponse() Tests
        [Test]
        public void GivenANullAcceptSubmissionRequest_WhenICreateAcceptSubmissionResponse_ThenAnArgumentNullExceptionIsThrown()
        {
            var testItems = CreateTestItems();

            Assert.That(() => testItems.SupplierDataShareRequestResponseFactory.CreateAcceptSubmissionResponse(
                    null!,
                    testItems.Fixture.Create<DataShareRequestAcceptanceResult>()),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("acceptSubmissionRequest"));
        }

        [Test]
        public void GivenANullAcceptedDecisionSummary_WhenICreateAcceptSubmissionResponse_ThenAnArgumentNullExceptionIsThrown()
        {
            var testItems = CreateTestItems();

            Assert.That(() => testItems.SupplierDataShareRequestResponseFactory.CreateAcceptSubmissionResponse(
                    testItems.Fixture.Create<AcceptSubmissionRequest>(),
                    null!),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("dataShareRequestAcceptanceResult"));
        }

        [Test]
        public void GivenAAcceptSubmissionRequest_WhenICreateAcceptSubmissionResponse_ThenAAcceptSubmissionResponseIsCreatedUsingTheValuesInTheRequest()
        {
            var testItems = CreateTestItems();

            var testAcceptSubmissionRequest = testItems.Fixture.Create<AcceptSubmissionRequest>();

            var result = testItems.SupplierDataShareRequestResponseFactory.CreateAcceptSubmissionResponse(
                testAcceptSubmissionRequest,
                testItems.Fixture.Create<DataShareRequestAcceptanceResult>());

            Assert.That(result.DataShareRequestId, Is.EqualTo(testAcceptSubmissionRequest.DataShareRequestId));
        }

        [Test]
        public void GivenACompletedSubmissionInformation_WhenICreateAcceptSubmissionResponse_ThenAAcceptSubmissionResponseIsCreatedUsingTheAcceptedDecisionSummary()
        {
            var testItems = CreateTestItems();

            var testDataShareRequestAcceptanceResult = testItems.Fixture.Create<DataShareRequestAcceptanceResult>();

            var result = testItems.SupplierDataShareRequestResponseFactory.CreateAcceptSubmissionResponse(
                testItems.Fixture.Create<AcceptSubmissionRequest>(),
                testDataShareRequestAcceptanceResult);

            Assert.Multiple(() =>
            {
                Assert.That(result.AcceptedDecisionSummary, Is.EqualTo(testDataShareRequestAcceptanceResult.AcceptedDecisionSummary));
                Assert.That(result.NotificationSuccess, Is.EqualTo(testDataShareRequestAcceptanceResult.NotificationSuccess));
            });
        }
        #endregion

        #region CreateRejectSubmissionResponse() Tests
        [Test]
        public void GivenANullRejectSubmissionRequest_WhenICreateRejectSubmissionResponse_ThenAnArgumentNullExceptionIsThrown()
        {
            var testItems = CreateTestItems();

            Assert.That(() => testItems.SupplierDataShareRequestResponseFactory.CreateRejectSubmissionResponse(
                    null!,
                    testItems.Fixture.Create<DataShareRequestRejectionResult>()),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("rejectSubmissionRequest"));
        }

        [Test]
        public void GivenANullRejectedDecisionSummary_WhenICreateRejectSubmissionResponse_ThenAnArgumentNullExceptionIsThrown()
        {
            var testItems = CreateTestItems();

            Assert.That(() => testItems.SupplierDataShareRequestResponseFactory.CreateRejectSubmissionResponse(
                    testItems.Fixture.Create<RejectSubmissionRequest>(),
                    null!),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("dataShareRequestRejectionResult"));
        }

        [Test]
        public void GivenARejectSubmissionRequest_WhenICreateRejectSubmissionResponse_ThenARejectSubmissionResponseIsCreatedUsingTheValuesInTheRequest()
        {
            var testItems = CreateTestItems();

            var testRejectSubmissionRequest = testItems.Fixture.Create<RejectSubmissionRequest>();

            var result = testItems.SupplierDataShareRequestResponseFactory.CreateRejectSubmissionResponse(
                testRejectSubmissionRequest,
                testItems.Fixture.Create<DataShareRequestRejectionResult>());

            Assert.That(result.DataShareRequestId, Is.EqualTo(testRejectSubmissionRequest.DataShareRequestId));
        }

        [Test]
        public void GivenACompletedSubmissionInformation_WhenICreateRejectSubmissionResponse_ThenARejectSubmissionResponseIsCreatedUsingTheRejectedDecisionSummary()
        {
            var testItems = CreateTestItems();

            var testDataShareRequestRejectionResult = testItems.Fixture.Create<DataShareRequestRejectionResult>();

            var result = testItems.SupplierDataShareRequestResponseFactory.CreateRejectSubmissionResponse(
                testItems.Fixture.Create<RejectSubmissionRequest>(),
                testDataShareRequestRejectionResult);

            Assert.Multiple(() =>
            {
                Assert.That(result.RejectedDecisionSummary, Is.EqualTo(testDataShareRequestRejectionResult.RejectedDecisionSummary));
                Assert.That(result.NotificationSuccess, Is.EqualTo(testDataShareRequestRejectionResult.NotificationSuccess));
            });
            
        }
        #endregion

        #region CreateReturnSubmissionResponse() Tests
        [Test]
        public void GivenANullReturnSubmissionRequest_WhenICreateReturnSubmissionResponse_ThenAnArgumentNullExceptionIsThrown()
        {
            var testItems = CreateTestItems();

            Assert.That(() => testItems.SupplierDataShareRequestResponseFactory.CreateReturnSubmissionResponse(
                    null!,
                    testItems.Fixture.Create<DataShareRequestReturnResult>()),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("returnSubmissionRequest"));
        }

        [Test]
        public void GivenANullReturnedDecisionSummary_WhenICreateReturnSubmissionResponse_ThenAnArgumentNullExceptionIsThrown()
        {
            var testItems = CreateTestItems();

            Assert.That(() => testItems.SupplierDataShareRequestResponseFactory.CreateReturnSubmissionResponse(
                    testItems.Fixture.Create<ReturnSubmissionRequest>(),
                    null!),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("dataShareRequestReturnResult"));
        }

        [Test]
        public void GivenAReturnSubmissionRequest_WhenICreateReturnSubmissionResponse_ThenAReturnSubmissionResponseIsCreatedUsingTheValuesInTheRequest()
        {
            var testItems = CreateTestItems();

            var testReturnSubmissionRequest = testItems.Fixture.Create<ReturnSubmissionRequest>();

            var result = testItems.SupplierDataShareRequestResponseFactory.CreateReturnSubmissionResponse(
                testReturnSubmissionRequest,
                testItems.Fixture.Create<DataShareRequestReturnResult>());

            Assert.That(result.DataShareRequestId, Is.EqualTo(testReturnSubmissionRequest.DataShareRequestId));
        }

        [Test]
        public void GivenACompletedSubmissionInformation_WhenICreateReturnSubmissionResponse_ThenAReturnSubmissionResponseIsCreatedUsingTheReturnedDecisionSummary()
        {
            var testItems = CreateTestItems();

            var testDataShareRequestReturnResult = testItems.Fixture.Create<DataShareRequestReturnResult>();

            var result = testItems.SupplierDataShareRequestResponseFactory.CreateReturnSubmissionResponse(
                testItems.Fixture.Create<ReturnSubmissionRequest>(),
                testDataShareRequestReturnResult);

            Assert.Multiple(() =>
            {
                Assert.That(result.ReturnedDecisionSummary, Is.EqualTo(testDataShareRequestReturnResult.ReturnedDecisionSummary));
                Assert.That(result.NotificationSuccess, Is.EqualTo(testDataShareRequestReturnResult.NotificationSuccess));
            });
        }
        #endregion

        #region Test Item Creation
        private static TestItems CreateTestItems()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());

            var supplierDataShareRequestResponseFactory = new SupplierDataShareRequestResponseFactory();

            return new TestItems(fixture, supplierDataShareRequestResponseFactory);
        }

        private class TestItems(
            IFixture fixture,
            ISupplierDataShareRequestResponseFactory supplierDataShareRequestResponseFactory)
        {
            public IFixture Fixture { get; } = fixture;
            public ISupplierDataShareRequestResponseFactory SupplierDataShareRequestResponseFactory { get; } = supplierDataShareRequestResponseFactory;
        }
        #endregion
    }
}
