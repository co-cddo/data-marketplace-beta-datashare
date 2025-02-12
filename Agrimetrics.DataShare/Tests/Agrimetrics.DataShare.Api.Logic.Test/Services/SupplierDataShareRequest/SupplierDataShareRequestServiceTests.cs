using System.Net;
using Agrimetrics.DataShare.Api.Dto.Models.Supplier.DataShareRequests;
using Agrimetrics.DataShare.Api.Logic.ModelData;
using Agrimetrics.DataShare.Api.Logic.Repositories.SupplierDataShareRequests;
using Agrimetrics.DataShare.Api.Logic.Services.AnswerHighlights;
using Agrimetrics.DataShare.Api.Logic.Services.AuditLog;
using Agrimetrics.DataShare.Api.Logic.Services.Esdas;
using Agrimetrics.DataShare.Api.Logic.Services.KeyQuestionPartAnswers;
using Agrimetrics.DataShare.Api.Logic.Services.Notification;
using Agrimetrics.DataShare.Api.Logic.Services.QuestionSetPlaceHolderReplacement;
using Agrimetrics.DataShare.Api.Logic.Services.ServiceOperationResults;
using AutoFixture.AutoMoq;
using AutoFixture;
using NUnit.Framework;
using Agrimetrics.DataShare.Api.Logic.Services.SupplierDataShareRequest;
using Agrimetrics.DataShare.Api.Logic.Services.SupplierDataShareRequest.SubmissionContentFileBuilding;
using Agrimetrics.DataShare.Api.Logic.Services.Users;
using Agrimetrics.DataShare.Api.Logic.Services.Users.Model;
using Microsoft.Extensions.Logging;
using Moq;
using Agrimetrics.DataShare.Api.Logic.ModelData.Submissions;
using Agrimetrics.DataShare.Api.Logic.ModelData.AuditLogs;
using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests;
using Agrimetrics.DataShare.Api.Dto.Models.Supplier.DataShareRequests.Decisions;
using Agrimetrics.DataShare.Api.Dto.Responses.Notifications;

namespace Agrimetrics.DataShare.Api.Logic.Test.Services.SupplierDataShareRequest;

[TestFixture]
public class SupplierDataShareRequestServiceTests
{
    #region GetSubmissionSummariesAsync() Tests
    [Test]
    public async Task GivenThereArePendingSubmissions_WhenIGetSubmissionSummariesAsync_ThenSummariesOfThoseSubmissionsIsReturned()
    {
        var testItems = CreateTestItems();

        var testInitiatingUserIdSet = testItems.Fixture.Create<UserIdSet>();

        testItems.MockUserProfilePresenter.Setup(x => x.GetInitiatingUserIdSetAsync())
            .ReturnsAsync(() => testInitiatingUserIdSet);

        var testPendingSubmissionSummaries = testItems.Fixture
            .Build<PendingSubmissionSummaryModelData>()
            .CreateMany().ToList();

        testItems.MockSupplierDataShareRequestRepository
            .Setup(x => x.GetPendingSubmissionSummariesAsync(testInitiatingUserIdSet.OrganisationId))
            .ReturnsAsync(() => testPendingSubmissionSummaries);

        #region Set Up Timestamp Information
        var testAuditLogStatusChanges = testItems.Fixture
            .Build<AuditLogDataShareRequestStatusChangeModelData>()
            .CreateMany().ToList();

        testItems.MockAuditLogService.Setup(x => x.GetMostRecentAuditLogForDataShareRequestStatusChangeToStatusAsync(
                It.IsAny<Guid>(),
                DataShareRequestStatusType.Submitted))
            .ReturnsAsync((Guid dataShareRequestId, DataShareRequestStatusType? _) =>
            {
                var indexOfSubmissionSummary = testPendingSubmissionSummaries
                    .FindIndex(x => x.PendingSubmissionSummary_DataShareRequestId == dataShareRequestId);

                return testAuditLogStatusChanges[indexOfSubmissionSummary]!;
            });
        #endregion

        #region Set Up Acquirer Information
        var testOrganisationInformations = testItems.Fixture
            .Build<OrganisationInformation>()
            .CreateMany().ToList();

        testItems.MockUserProfilePresenter.Setup(x => x.GetOrganisationDetailsByOrganisationIdAsync(
                It.IsAny<int>()))
            .ReturnsAsync((int acquirerOrganisationId) =>
            {
                var indexOfSubmissionSummary = testPendingSubmissionSummaries
                    .FindIndex(x => x.PendingSubmissionSummary_AcquirerOrganisationId == acquirerOrganisationId);

                return testOrganisationInformations[indexOfSubmissionSummary];
            });
        #endregion

        #region Set Up When Needed By Dates
        var testWhenNeededByDates = testItems.Fixture
            .Build<DateTime>()
            .CreateMany().ToList();

        testItems.MockKeyQuestionPartAnswerProviderService.Setup(x => x.GetDateRequiredQuestionPartAnswerAsync(
                It.IsAny<Guid>()))
            .ReturnsAsync((Guid dataShareRequestId) =>
            {
                var indexOfSubmissionSummary = testPendingSubmissionSummaries
                    .FindIndex(x => x.PendingSubmissionSummary_DataShareRequestId == dataShareRequestId);

                return testWhenNeededByDates[indexOfSubmissionSummary];
            });
        #endregion

        #region Set Up Response Data Building
        testItems.MockSupplierDataShareRequestModelDataFactory.Setup(x => x.CreateSubmissionSummarySet(
                It.IsAny<IEnumerable<PendingSubmissionSummaryModelData>>(),
                It.IsAny<IEnumerable<CompletedSubmissionSummaryModelData>>()))
            .Returns((
                IEnumerable<PendingSubmissionSummaryModelData> pendingSummaryModelDatas,
                IEnumerable<CompletedSubmissionSummaryModelData> _) =>
            {
                var pendingSubmissionSummaries = pendingSummaryModelDatas.Select(pendingSummaryModelData =>
                {
                    return testItems.Fixture
                        .Build<PendingSubmissionSummary>()
                        .With(x => x.DataShareRequestId, pendingSummaryModelData.PendingSubmissionSummary_DataShareRequestId)
                        .With(x => x.DataShareRequestRequestId, pendingSummaryModelData.PendingSubmissionSummary_DataShareRequestRequestId)
                        .With(x => x.SubmittedOn, pendingSummaryModelData.PendingSubmissionSummary_SubmittedOn)
                        .With(x => x.AcquirerOrganisationName, pendingSummaryModelData.PendingSubmissionSummary_AcquirerOrganisationName)
                        .With(x => x.WhenNeededBy, pendingSummaryModelData.PendingSubmissionSummary_WhenNeededBy)
                        .Create();
                });

                return testItems.Fixture
                    .Build<SubmissionSummariesSet>()
                    .With(x => x.PendingSubmissionSummaries, pendingSubmissionSummaries.ToList())
                    .Create();
            });
        #endregion
        
        TestSetUpSuccessfulDataResultHandling<SubmissionSummariesSet>(testItems);

        var result = await testItems.SupplierDataShareRequestService.GetSubmissionSummariesAsync();

        Assert.Multiple(() =>
        {
            var resultSubmissionSummarySet = result.Data!;

            var resultPendingSummaries = resultSubmissionSummarySet.PendingSubmissionSummaries.ToList();
            Assert.That(resultPendingSummaries, Has.Exactly(testPendingSubmissionSummaries.Count).Items);

            foreach (var testPendingSubmissionSummary in testPendingSubmissionSummaries)
            {
                var pendingSummaryIndex = resultPendingSummaries.FindIndex(x =>
                    x.DataShareRequestId == testPendingSubmissionSummary.PendingSubmissionSummary_DataShareRequestId);

                Assert.That(pendingSummaryIndex, Is.GreaterThanOrEqualTo(0).And.LessThan(testPendingSubmissionSummaries.Count));

                var resultPendingSummary = resultPendingSummaries[pendingSummaryIndex];
                Assert.That(resultPendingSummary.DataShareRequestRequestId, Is.EqualTo(testPendingSubmissionSummary.PendingSubmissionSummary_DataShareRequestRequestId));
                Assert.That(resultPendingSummary.SubmittedOn, Is.EqualTo(testAuditLogStatusChanges[pendingSummaryIndex].AuditLogDataShareRequestStatusChange_ChangedAtUtc));
                Assert.That(resultPendingSummary.AcquirerOrganisationName, Is.EqualTo(testOrganisationInformations[pendingSummaryIndex].OrganisationName));
                Assert.That(resultPendingSummary.WhenNeededBy, Is.EqualTo(testWhenNeededByDates[pendingSummaryIndex]));
            }
        });
    }

    [Test]
    public async Task GivenThereAreCompletedSubmissions_WhenIGetSubmissionSummariesAsync_ThenSummariesOfThoseSubmissionsIsReturned()
    {
        var testItems = CreateTestItems();

        var testInitiatingUserIdSet = testItems.Fixture.Create<UserIdSet>();

        testItems.MockUserProfilePresenter.Setup(x => x.GetInitiatingUserIdSetAsync())
            .ReturnsAsync(() => testInitiatingUserIdSet);

        var testCompletedSubmissionSummaries = testItems.Fixture
            .Build<CompletedSubmissionSummaryModelData>()
            .CreateMany().ToList();

        #region Set Up Timestamp Information
        var testAuditLogSubmittedStatusChanges = testItems.Fixture
            .Build<AuditLogDataShareRequestStatusChangeModelData>()
            .CreateMany().ToList();

        testItems.MockAuditLogService.Setup(x => x.GetMostRecentAuditLogForDataShareRequestStatusChangeToStatusAsync(
                It.IsAny<Guid>(),
                DataShareRequestStatusType.Submitted))
            .ReturnsAsync((Guid dataShareRequestId, DataShareRequestStatusType? _) =>
            {
                var indexOfSubmissionSummary = testCompletedSubmissionSummaries
                    .FindIndex(x => x.CompletedSubmissionSummary_DataShareRequestId == dataShareRequestId);

                return testAuditLogSubmittedStatusChanges[indexOfSubmissionSummary]!;
            });

        var testAuditLogCompletedStatusChanges = testItems.Fixture
            .Build<AuditLogDataShareRequestStatusChangeModelData>()
            .CreateMany().ToList();

        testItems.MockAuditLogService.Setup(x => x.GetMostRecentAuditLogForDataShareRequestStatusChangeToStatusAsync(
                It.IsAny<Guid>(),
                DataShareRequestStatusType.Accepted))
            .ReturnsAsync((Guid dataShareRequestId, DataShareRequestStatusType? _) =>
            {
                var indexOfSubmissionSummary = testCompletedSubmissionSummaries
                    .FindIndex(x => x.CompletedSubmissionSummary_DataShareRequestId == dataShareRequestId);

                return testAuditLogCompletedStatusChanges[indexOfSubmissionSummary]!;
            });
        #endregion

        #region Set Up Acquirer Information
        var testOrganisationInformations = testItems.Fixture
            .Build<OrganisationInformation>()
            .CreateMany().ToList();

        testItems.MockUserProfilePresenter.Setup(x => x.GetOrganisationDetailsByOrganisationIdAsync(
                It.IsAny<int>()))
            .ReturnsAsync((int acquirerOrganisationId) =>
            {
                var indexOfSubmissionSummary = testCompletedSubmissionSummaries
                    .FindIndex(x => x.CompletedSubmissionSummary_AcquirerOrganisationId == acquirerOrganisationId);

                return testOrganisationInformations[indexOfSubmissionSummary];
            });
        #endregion

        #region Set Up Response Data Building
        testItems.MockSupplierDataShareRequestModelDataFactory.Setup(x => x.CreateSubmissionSummarySet(
                It.IsAny<IEnumerable<PendingSubmissionSummaryModelData>>(),
                It.IsAny<IEnumerable<CompletedSubmissionSummaryModelData>>()))
            .Returns((
                IEnumerable<PendingSubmissionSummaryModelData> _,
                IEnumerable<CompletedSubmissionSummaryModelData> completedSummaryModelDatas) =>
            {
                var completedSubmissionSummaries = completedSummaryModelDatas.Select(completedSubmissionSummary =>
                {
                    return testItems.Fixture
                        .Build<CompletedSubmissionSummary>()
                        .With(x => x.DataShareRequestId, completedSubmissionSummary.CompletedSubmissionSummary_DataShareRequestId)
                        .With(x => x.DataShareRequestRequestId, completedSubmissionSummary.CompletedSubmissionSummary_DataShareRequestRequestId)
                        .With(x => x.SubmittedOn, completedSubmissionSummary.CompletedSubmissionSummary_SubmittedOn)
                        .With(x => x.AcquirerOrganisationName, completedSubmissionSummary.CompletedSubmissionSummary_AcquirerOrganisationName)
                        .With(x => x.CompletedOn, completedSubmissionSummary.CompletedSubmissionSummary_CompletedOn)
                        .Create();
                });
                
                return testItems.Fixture
                    .Build<SubmissionSummariesSet>()
                    .With(x => x.CompletedSubmissionSummaries, completedSubmissionSummaries.ToList())
                    .Create();
            });
        #endregion

        testItems.MockSupplierDataShareRequestRepository
            .Setup(x => x.GetCompletedSubmissionSummariesAsync(testInitiatingUserIdSet.OrganisationId))
            .ReturnsAsync(() => testCompletedSubmissionSummaries);

        TestSetUpSuccessfulDataResultHandling<SubmissionSummariesSet>(testItems);

        var result = await testItems.SupplierDataShareRequestService.GetSubmissionSummariesAsync();

        Assert.Multiple(() =>
        {
            var resultSubmissionSummarySet = result.Data!;

            var resultCompletedSummaries = resultSubmissionSummarySet.CompletedSubmissionSummaries.ToList();
            Assert.That(resultCompletedSummaries, Has.Exactly(testCompletedSubmissionSummaries.Count).Items);

            foreach (var testCompletedSubmissionSummary in testCompletedSubmissionSummaries)
            {
                var completedSummaryIndex = resultCompletedSummaries.FindIndex(x =>
                    x.DataShareRequestId == testCompletedSubmissionSummary.CompletedSubmissionSummary_DataShareRequestId);

                Assert.That(completedSummaryIndex, Is.GreaterThanOrEqualTo(0).And.LessThan(testCompletedSubmissionSummaries.Count));

                var resultPendingSummary = resultCompletedSummaries[completedSummaryIndex];
                Assert.That(resultPendingSummary.DataShareRequestRequestId, Is.EqualTo(testCompletedSubmissionSummary.CompletedSubmissionSummary_DataShareRequestRequestId));
                Assert.That(resultPendingSummary.SubmittedOn, Is.EqualTo(testAuditLogSubmittedStatusChanges[completedSummaryIndex].AuditLogDataShareRequestStatusChange_ChangedAtUtc));
                Assert.That(resultPendingSummary.AcquirerOrganisationName, Is.EqualTo(testOrganisationInformations[completedSummaryIndex].OrganisationName));
                Assert.That(resultPendingSummary.CompletedOn, Is.EqualTo(testAuditLogCompletedStatusChanges[completedSummaryIndex].AuditLogDataShareRequestStatusChange_ChangedAtUtc));
            }
        });
    }

    [Test]
    public async Task GivenGettingSubmissionSummaryInformationWillFail_WhenIGetSubmissionSummariesAsync_ThenFailureIsReturned()
    {
        var testItems = CreateTestItems();

        testItems.MockSupplierDataShareRequestRepository.Setup(x => x.GetPendingSubmissionSummariesAsync(It.IsAny<int>()))
            .Throws(() => new Exception("test error message"));

        TestSetUpFailedDataResultHandling<SubmissionSummariesSet>(testItems);

        var result = await testItems.SupplierDataShareRequestService.GetSubmissionSummariesAsync();

        Assert.Multiple(() =>
        {
            Assert.That(result.Success, Is.False);
            Assert.That(result.Error, Is.EqualTo("test error message"));
        });
    }
    #endregion

    #region GetSubmissionInformationAsync() Tests
    [Test]
    public async Task GivenADataShareRequestHasBeenSubmitted_WhenIGetSubmissionInformationAsync_ThenTheSubmissionInformationForThatRequestIsReturned()
    {
        var testItems = CreateTestItems();

        var testDataShareRequestId = testItems.Fixture.Create<Guid>();

        var testSubmissionInformation = testItems.Fixture
            .Build<SubmissionInformationModelData>()
            .With(x => x.SubmissionInformation_DataShareRequestId, testDataShareRequestId)
            .Create();

        testItems.MockSupplierDataShareRequestRepository
            .Setup(x => x.GetSubmissionInformationModelDataAsync(testDataShareRequestId))
            .ReturnsAsync(() => testSubmissionInformation);

        #region Set Up Timestamp Information
        var testAuditLogStatusChange = testItems.Fixture
            .Build<AuditLogDataShareRequestStatusChangeModelData>()
            .Create();

        testItems.MockAuditLogService.Setup(x => x.GetMostRecentAuditLogForDataShareRequestStatusChangeToStatusAsync(
                testDataShareRequestId,
                DataShareRequestStatusType.Submitted))
            .ReturnsAsync(() => testAuditLogStatusChange);
        #endregion

        #region Set Up Acquirer Information
        var testAcquirerUserDetails = testItems.Fixture.Create<UserDetails>();

        testItems.MockUserProfilePresenter.Setup(x => x.GetUserDetailsByUserIdAsync(
                testSubmissionInformation.SubmissionInformation_AcquirerUserId))
            .ReturnsAsync(() => testAcquirerUserDetails);

        var testAcquirerOrganisationInformation = testItems.Fixture.Create<OrganisationInformation>();

        testItems.MockUserProfilePresenter.Setup(x => x.GetOrganisationDetailsByOrganisationIdAsync(
                testSubmissionInformation.SubmissionInformation_AcquirerOrganisationId))
            .ReturnsAsync(() => testAcquirerOrganisationInformation);
        #endregion

        #region Set Up Key Question Answers
        var testProjectAims = testItems.Fixture.Create<string>();
        testItems.MockKeyQuestionPartAnswerProviderService.Setup(x => x.GetProjectAimsQuestionPartAnswerAsync(testDataShareRequestId))
            .ReturnsAsync(() => testProjectAims);

        var testDataTypes = testItems.Fixture.CreateMany<string>().ToList();
        testItems.MockKeyQuestionPartAnswerProviderService.Setup(x => x.GetDataTypesQuestionPartAnswerAsync(testDataShareRequestId))
            .ReturnsAsync(() => testDataTypes);

        var testWhenNeededBy = testItems.Fixture.Create<DateTime>();
        testItems.MockKeyQuestionPartAnswerProviderService.Setup(x => x.GetDateRequiredQuestionPartAnswerAsync(testDataShareRequestId))
            .ReturnsAsync(() => testWhenNeededBy);
        #endregion

        #region Set Up Answer Highlights
        var testAnswerHighlights = testItems.Fixture.CreateMany<string>().ToList();
        testItems.MockAnswerHighlightsProviderService.Setup(x => x.GetDataShareRequestsAnswerHighlightsAsync(
                testDataShareRequestId))
            .ReturnsAsync(() => testAnswerHighlights);
        #endregion

        #region Set Up Response Data Building
        testItems.MockSupplierDataShareRequestModelDataFactory.Setup(x => x.CreateSubmissionInformation(
                It.IsAny<SubmissionInformationModelData>()))
            .Returns((SubmissionInformationModelData submissionInformationModelData) =>
            {
                return testItems.Fixture
                    .Build<SubmissionInformation>()
                    .With(x => x.DataShareRequestId, submissionInformationModelData.SubmissionInformation_DataShareRequestId)
                    .With(x => x.DataShareRequestRequestId, submissionInformationModelData.SubmissionInformation_DataShareRequestRequestId)
                    .With(x => x.SubmittedOn, submissionInformationModelData.SubmissionInformation_SubmittedOn)
                    .With(x => x.AcquirerEmailAddress, submissionInformationModelData.SubmissionInformation_AcquirerEmailAddress)
                    .With(x => x.AcquirerOrganisationName, submissionInformationModelData.SubmissionInformation_AcquirerOrganisationName)
                    .With(x => x.ProjectAims, submissionInformationModelData.SubmissionInformation_ProjectAims)
                    .With(x => x.DataTypes, submissionInformationModelData.SubmissionInformation_DataTypes)
                    .With(x => x.WhenNeededBy, submissionInformationModelData.SubmissionInformation_WhenNeededBy)
                    .With(x => x.AnswerHighlights, submissionInformationModelData.SubmissionInformation_AnswerHighlights)
                    .Create();
            });
        #endregion

        TestSetUpSuccessfulDataResultHandling<SubmissionInformation>(testItems);

        var result = await testItems.SupplierDataShareRequestService.GetSubmissionInformationAsync(testDataShareRequestId);

        Assert.Multiple(() =>
        {
            var resultSubmissionInformation = result.Data!;

            Assert.That(resultSubmissionInformation.DataShareRequestId, Is.EqualTo(testSubmissionInformation.SubmissionInformation_DataShareRequestId));
            Assert.That(resultSubmissionInformation.DataShareRequestRequestId, Is.EqualTo(testSubmissionInformation.SubmissionInformation_DataShareRequestRequestId));
            Assert.That(resultSubmissionInformation.SubmittedOn, Is.EqualTo(testSubmissionInformation.SubmissionInformation_SubmittedOn));
            Assert.That(resultSubmissionInformation.AcquirerEmailAddress, Is.EqualTo(testSubmissionInformation.SubmissionInformation_AcquirerEmailAddress));
            Assert.That(resultSubmissionInformation.AcquirerOrganisationName, Is.EqualTo(testSubmissionInformation.SubmissionInformation_AcquirerOrganisationName));
            Assert.That(resultSubmissionInformation.ProjectAims, Is.EqualTo(testProjectAims));
            Assert.That(resultSubmissionInformation.DataTypes, Is.EqualTo(testDataTypes));
            Assert.That(resultSubmissionInformation.WhenNeededBy, Is.EqualTo(testWhenNeededBy));
            Assert.That(resultSubmissionInformation.AnswerHighlights, Is.EqualTo(testAnswerHighlights));
        });
    }

    [Test]
    public async Task GivenADataShareRequestHasBeenSubmittedAndThereIsNoAuditLogForTheSubmission_WhenIGetSubmissionInformationAsync_ThenAFailedResultIsReturned()
    {
        var testItems = CreateTestItems();

        var testDataShareRequestId = testItems.Fixture.Create<Guid>();

        var testSubmissionInformation = testItems.Fixture
            .Build<SubmissionInformationModelData>()
            .With(x => x.SubmissionInformation_DataShareRequestId, testDataShareRequestId)
            .Create();

        testItems.MockSupplierDataShareRequestRepository
            .Setup(x => x.GetSubmissionInformationModelDataAsync(testDataShareRequestId))
            .ReturnsAsync(() => testSubmissionInformation);

        #region Set Up Timestamp Information
        testItems.MockAuditLogService.Setup(x => x.GetMostRecentAuditLogForDataShareRequestStatusChangeToStatusAsync(
                testDataShareRequestId,
                DataShareRequestStatusType.Submitted))
            .ReturnsAsync(() => null!);
        #endregion

        TestSetUpFailedDataResultHandling<SubmissionInformation>(testItems);

        var result = await testItems.SupplierDataShareRequestService.GetSubmissionInformationAsync(testDataShareRequestId);

        Assert.Multiple(() =>
        {
            Assert.That(result.Success, Is.False);
            Assert.That(result.Error, Is.EqualTo("Returned submission has no Submitted entry in the audit log"));
        });
    }

    [Test]
    public async Task GivenGettingSubmissionInformationWillFail_WhenIGetSubmissionInformationAsync_ThenAFailedResultIsReturned()
    {
        var testItems = CreateTestItems();

        var testDataShareRequestId = testItems.Fixture.Create<Guid>();

        testItems.MockSupplierDataShareRequestRepository
            .Setup(x => x.GetSubmissionInformationModelDataAsync(testDataShareRequestId))
            .Throws(() => new Exception("test error message"));

        TestSetUpFailedDataResultHandling<SubmissionInformation>(testItems);

        var result = await testItems.SupplierDataShareRequestService.GetSubmissionInformationAsync(testDataShareRequestId);

        Assert.Multiple(() =>
        {
            Assert.That(result.Success, Is.False);
            Assert.That(result.Error, Is.EqualTo("test error message"));
        });
    }
    #endregion

    #region GetSubmissionDetailsModelDataAsync() Tests
    [Test]
    public async Task GivenADataShareRequestHasBeenSubmitted_WhenIGetSubmissionDetailsAsync_ThenTheSubmissionDetailsForThatRequestIsReturned()
    {
        var testItems = CreateTestItems();

        var testDataShareRequestId = testItems.Fixture.Create<Guid>();

        var testSubmissionDetails = testItems.Fixture
            .Build<SubmissionDetailsModelData>()
            .With(x => x.SubmissionDetails_DataShareRequestId, testDataShareRequestId)
            .Create();

        testItems.MockSupplierDataShareRequestRepository
            .Setup(x => x.GetSubmissionDetailsModelDataAsync(testDataShareRequestId))
            .ReturnsAsync(() => testSubmissionDetails);

        #region Set Up Acquirer Information
        var testAcquirerOrganisationInformation = testItems.Fixture.Create<OrganisationInformation>();

        testItems.MockUserProfilePresenter.Setup(x => x.GetOrganisationDetailsByOrganisationIdAsync(
                testSubmissionDetails.SubmissionDetails_AcquirerOrganisationId))
            .ReturnsAsync(() => testAcquirerOrganisationInformation);
        #endregion

        #region Set Up Return Comments
        var testReturnedOn = testItems.Fixture.Create<DateTime>();

        var testReturnedStatusChange = testItems.Fixture
            .Build<AuditLogDataShareRequestStatusChangeModelData>()
            .With(x => x.AuditLogDataShareRequestStatusChange_ChangedAtUtc, testReturnedOn)
            .Create();

        testItems.MockAuditLogService.Setup(x => x.GetAuditLogsForDataShareRequestStatusChangeToStatusAsync(
                testDataShareRequestId,
                new List<DataShareRequestStatusType> {DataShareRequestStatusType.Returned}))
            .ReturnsAsync(() => [testReturnedStatusChange]);
        #endregion

        #region Set Up Response Data Building
        testItems.MockSupplierDataShareRequestModelDataFactory.Setup(x => x.CreateSubmissionDetails(
                It.IsAny<SubmissionDetailsModelData>()))
            .Returns((SubmissionDetailsModelData submissionDetailsModelData) =>
            {
                var testSubmissionReturnDetails = testReturnedStatusChange.AuditLogDataShareRequestStatusChange_Comments.Select(comment =>
                {
                    return testItems.Fixture
                        .Build<SubmissionReturnDetails>()
                        .With(x => x.ReturnComments, comment.AuditLogDataShareRequestStatusChangeComment_Comment)
                        .With(x => x.ReturnedOnUtc, testReturnedStatusChange.AuditLogDataShareRequestStatusChange_ChangedAtUtc)
                        .Create();
                }).ToList();

                var testSubmissionReturnDetailsSet = testItems.Fixture
                    .Build<SubmissionReturnDetailsSet>()
                    .With(x => x.SubmissionReturns, testSubmissionReturnDetails)
                    .Create();

                return testItems.Fixture
                    .Build<SubmissionDetails>()
                    .With(x => x.DataShareRequestId, submissionDetailsModelData.SubmissionDetails_DataShareRequestId)
                    .With(x => x.DataShareRequestRequestId, submissionDetailsModelData.SubmissionDetails_DataShareRequestRequestId)
                    .With(x => x.AcquirerOrganisationName, submissionDetailsModelData.SubmissionDetails_AcquirerOrganisationName)
                    .With(x => x.SubmissionReturnDetailsSet, testSubmissionReturnDetailsSet)
                    .Create();
            });
        #endregion

        TestSetUpSuccessfulDataResultHandling<SubmissionDetails>(testItems);

        var result = await testItems.SupplierDataShareRequestService.GetSubmissionDetailsAsync(testDataShareRequestId);

        Assert.Multiple(() =>
        {
            var resultSubmissionInformation = result.Data!;

            Assert.That(resultSubmissionInformation.DataShareRequestId, Is.EqualTo(testSubmissionDetails.SubmissionDetails_DataShareRequestId));
            Assert.That(resultSubmissionInformation.DataShareRequestRequestId, Is.EqualTo(testSubmissionDetails.SubmissionDetails_DataShareRequestRequestId));
            Assert.That(resultSubmissionInformation.AcquirerOrganisationName, Is.EqualTo(testSubmissionDetails.SubmissionDetails_AcquirerOrganisationName));

            var testSubmissionReturnComments = testReturnedStatusChange.AuditLogDataShareRequestStatusChange_Comments;
            var resultSubmissionReturns = resultSubmissionInformation.SubmissionReturnDetailsSet.SubmissionReturns;

            Assert.That(resultSubmissionReturns, Has.Exactly(testSubmissionReturnComments.Count).Items);

            foreach (var testSubmissionReturnComment in testSubmissionReturnComments)
            {
                var resultSubmissionReturn = resultSubmissionReturns.FirstOrDefault(x =>
                    x.ReturnComments == testSubmissionReturnComment.AuditLogDataShareRequestStatusChangeComment_Comment);

                Assert.That(resultSubmissionReturn!.ReturnedOnUtc, Is.EqualTo(testReturnedOn));
            }
        });
    }

    [Test]
    public async Task GivenGettingSubmissionDetailsWillFail_WhenIGetSubmissionDetailsAsync_ThenAFailedResultIsReturned()
    {
        var testItems = CreateTestItems();

        var testDataShareRequestId = testItems.Fixture.Create<Guid>();

        testItems.MockSupplierDataShareRequestRepository
            .Setup(x => x.GetSubmissionDetailsModelDataAsync(testDataShareRequestId))
            .Throws(() => new Exception());

        TestSetUpFailedDataResultHandling<SubmissionDetails>(testItems);

        var result = await testItems.SupplierDataShareRequestService.GetSubmissionDetailsAsync(testDataShareRequestId);

        Assert.Multiple(() =>
        {
            Assert.That(result.Success, Is.False);
            Assert.That(result.Error, Is.EqualTo("Failed to GetSubmissionDetailsModelData from repository"));
        });
    }
    #endregion

    #region GetSubmissionContentAsFileAsync() Tests
    [Test]
    public async Task GivenADataShareRequestHasBeenSubmitted_WhenIGetSubmissionContentAsFileAsync_ThenTheSubmissionContentForThatRequestIsReturned()
    {
        var testItems = CreateTestItems();

        var testDataShareRequestId = testItems.Fixture.Create<Guid>();

        testItems.MockSupplierDataShareRequestRepository.Setup(x => x.GetDataShareRequestStatusAsync(testDataShareRequestId))
            .ReturnsAsync(() => DataShareRequestStatusType.Submitted);

        #region Set Up Submission Information
        var testSubmissionInformation = testItems.Fixture
            .Build<SubmissionInformationModelData>()
            .With(x => x.SubmissionInformation_DataShareRequestId, testDataShareRequestId)
            .Create();

        testItems.MockSupplierDataShareRequestRepository
            .Setup(x => x.GetSubmissionInformationModelDataAsync(testDataShareRequestId))
            .ReturnsAsync(() => testSubmissionInformation);

        #region Set Up Timestamp Information
        var testAuditLogStatusChange = testItems.Fixture
            .Build<AuditLogDataShareRequestStatusChangeModelData>()
            .Create();

        testItems.MockAuditLogService.Setup(x => x.GetMostRecentAuditLogForDataShareRequestStatusChangeToStatusAsync(
                testDataShareRequestId,
                DataShareRequestStatusType.Submitted))
            .ReturnsAsync(() => testAuditLogStatusChange);
        #endregion

        #region Set Up Acquirer Information
        var testAcquirerUserDetails = testItems.Fixture.Create<UserDetails>();

        testItems.MockUserProfilePresenter.Setup(x => x.GetUserDetailsByUserIdAsync(
                testSubmissionInformation.SubmissionInformation_AcquirerUserId))
            .ReturnsAsync(() => testAcquirerUserDetails);

        var testAcquirerOrganisationInformation = testItems.Fixture.Create<OrganisationInformation>();

        testItems.MockUserProfilePresenter.Setup(x => x.GetOrganisationDetailsByOrganisationIdAsync(
                testSubmissionInformation.SubmissionInformation_AcquirerOrganisationId))
            .ReturnsAsync(() => testAcquirerOrganisationInformation);
        #endregion

        #region Set Up Key Question Answers
        var testProjectAims = testItems.Fixture.Create<string>();
        testItems.MockKeyQuestionPartAnswerProviderService.Setup(x => x.GetProjectAimsQuestionPartAnswerAsync(testDataShareRequestId))
            .ReturnsAsync(() => testProjectAims);

        var testDataTypes = testItems.Fixture.CreateMany<string>().ToList();
        testItems.MockKeyQuestionPartAnswerProviderService.Setup(x => x.GetDataTypesQuestionPartAnswerAsync(testDataShareRequestId))
            .ReturnsAsync(() => testDataTypes);

        var testWhenNeededBy = testItems.Fixture.Create<DateTime>();
        testItems.MockKeyQuestionPartAnswerProviderService.Setup(x => x.GetDateRequiredQuestionPartAnswerAsync(testDataShareRequestId))
            .ReturnsAsync(() => testWhenNeededBy);
        #endregion

        #region Set Up Answer Highlights
        var testAnswerHighlights = testItems.Fixture.CreateMany<string>().ToList();
        testItems.MockAnswerHighlightsProviderService.Setup(x => x.GetDataShareRequestsAnswerHighlightsAsync(
                testDataShareRequestId))
            .ReturnsAsync(() => testAnswerHighlights);
        #endregion

        #region Set Up Response Data Building
        testItems.MockSupplierDataShareRequestModelDataFactory.Setup(x => x.CreateSubmissionInformation(
                It.IsAny<SubmissionInformationModelData>()))
            .Returns((SubmissionInformationModelData submissionInformationModelData) =>
            {
                return testItems.Fixture
                    .Build<SubmissionInformation>()
                    .With(x => x.DataShareRequestId, submissionInformationModelData.SubmissionInformation_DataShareRequestId)
                    .With(x => x.DataShareRequestRequestId, submissionInformationModelData.SubmissionInformation_DataShareRequestRequestId)
                    .With(x => x.SubmittedOn, submissionInformationModelData.SubmissionInformation_SubmittedOn)
                    .With(x => x.AcquirerEmailAddress, submissionInformationModelData.SubmissionInformation_AcquirerEmailAddress)
                    .With(x => x.AcquirerOrganisationName, submissionInformationModelData.SubmissionInformation_AcquirerOrganisationName)
                    .With(x => x.ProjectAims, submissionInformationModelData.SubmissionInformation_ProjectAims)
                    .With(x => x.DataTypes, submissionInformationModelData.SubmissionInformation_DataTypes)
                    .With(x => x.WhenNeededBy, submissionInformationModelData.SubmissionInformation_WhenNeededBy)
                    .With(x => x.AnswerHighlights, submissionInformationModelData.SubmissionInformation_AnswerHighlights)
                    .Create();
            });
        #endregion
        #endregion

        #region Set Up Submission Details
        var testSubmissionDetails = testItems.Fixture
            .Build<SubmissionDetailsModelData>()
            .With(x => x.SubmissionDetails_DataShareRequestId, testDataShareRequestId)
            .Create();

        testItems.MockSupplierDataShareRequestRepository
            .Setup(x => x.GetSubmissionDetailsModelDataAsync(testDataShareRequestId))
            .ReturnsAsync(() => testSubmissionDetails);

        #region Set Up Response Data Building
        testItems.MockSupplierDataShareRequestModelDataFactory.Setup(x => x.CreateSubmissionDetails(
                It.IsAny<SubmissionDetailsModelData>()))
            .Returns((SubmissionDetailsModelData submissionDetailsModelData) =>
            {
                return testItems.Fixture
                    .Build<SubmissionDetails>()
                    .With(x => x.DataShareRequestId, submissionDetailsModelData.SubmissionDetails_DataShareRequestId)
                    .With(x => x.DataShareRequestRequestId, submissionDetailsModelData.SubmissionDetails_DataShareRequestRequestId)
                    .With(x => x.AcquirerOrganisationName, submissionDetailsModelData.SubmissionDetails_AcquirerOrganisationName)
                    .Create();
            });
        #endregion
        #endregion

        #region Set Up Submission Content Building
        var testFileContent = testItems.Fixture.Create<byte[]>();

        testItems.MockSubmissionContentPdfFileBuilder.Setup(x => x.BuildAsync(
                It.Is<SubmissionInformation>(information => information.DataShareRequestId == testDataShareRequestId),
                It.Is<SubmissionDetails>(details => details.DataShareRequestId == testDataShareRequestId)))
            .ReturnsAsync(() => testFileContent);
        #endregion

        TestSetUpSuccessfulDataResultHandling<SubmissionContentAsFile>(testItems);

        var result = await testItems.SupplierDataShareRequestService.GetSubmissionContentAsFileAsync(
            testDataShareRequestId, DataShareRequestFileFormat.Pdf);

        Assert.Multiple(() =>
        {
            var resultSubmissionContentAsFile = result.Data!;

            Assert.That(resultSubmissionContentAsFile.Content, Is.EqualTo(testFileContent));
            Assert.That(resultSubmissionContentAsFile.ContentType, Is.EqualTo("application/pdf"));
        });
    }

    [Test]
    public async Task GivenGettingSubmissionInformationForARequestInSupportedState_WhenIGetSubmissionContentAsFileAsync_ThenAFailedResultIsReturned()
    {
        var testItems = CreateTestItems();

        var testDataShareRequestId = testItems.Fixture.Create<Guid>();

        testItems.MockSupplierDataShareRequestRepository.Setup(x => x.GetDataShareRequestStatusAsync(testDataShareRequestId))
            .ReturnsAsync(() => DataShareRequestStatusType.Draft);

        TestSetUpFailedDataResultHandling<SubmissionContentAsFile>(testItems);

        var result = await testItems.SupplierDataShareRequestService.GetSubmissionContentAsFileAsync(
            testDataShareRequestId, DataShareRequestFileFormat.Pdf);

        Assert.Multiple(() =>
        {
            Assert.That(result.Success, Is.False);
            Assert.That(result.Error, Is.EqualTo("Unable to get submission content for Data Share Request with its current status"));
        });
    }

    [Test]
    public async Task GivenGettingSubmissionInformationWillFail_WhenIGetSubmissionContentAsFileAsync_ThenAFailedResultIsReturned()
    {
        var testItems = CreateTestItems();

        var testDataShareRequestId = testItems.Fixture.Create<Guid>();

        testItems.MockSupplierDataShareRequestRepository.Setup(x => x.GetDataShareRequestStatusAsync(testDataShareRequestId))
            .Throws(() => new Exception("test error message"));

        TestSetUpFailedDataResultHandling<SubmissionContentAsFile>(testItems);

        var result = await testItems.SupplierDataShareRequestService.GetSubmissionContentAsFileAsync(
            testDataShareRequestId, DataShareRequestFileFormat.Pdf);

        Assert.Multiple(() =>
        {
            Assert.That(result.Success, Is.False);
            Assert.That(result.Error, Is.EqualTo("test error message"));
        });
    }

    [Test]
    public async Task GivenGettingSubmissionDetailsWillFail_WhenIGetSubmissionContentAsFileAsync_ThenAFailedResultIsReturned()
    {
        var testItems = CreateTestItems();

        var testDataShareRequestId = testItems.Fixture.Create<Guid>();

        testItems.MockSupplierDataShareRequestRepository.Setup(x => x.GetDataShareRequestStatusAsync(testDataShareRequestId))
            .ReturnsAsync(() => DataShareRequestStatusType.Submitted);

        #region Set Up Submission Information
        var testSubmissionInformation = testItems.Fixture
            .Build<SubmissionInformationModelData>()
            .With(x => x.SubmissionInformation_DataShareRequestId, testDataShareRequestId)
            .Create();

        testItems.MockSupplierDataShareRequestRepository
            .Setup(x => x.GetSubmissionInformationModelDataAsync(testDataShareRequestId))
            .ReturnsAsync(() => testSubmissionInformation);

        #region Set Up Timestamp Information
        var testAuditLogStatusChange = testItems.Fixture
            .Build<AuditLogDataShareRequestStatusChangeModelData>()
            .Create();

        testItems.MockAuditLogService.Setup(x => x.GetMostRecentAuditLogForDataShareRequestStatusChangeToStatusAsync(
                testDataShareRequestId,
                DataShareRequestStatusType.Submitted))
            .ReturnsAsync(() => testAuditLogStatusChange);
        #endregion

        #region Set Up Acquirer Information
        var testAcquirerUserDetails = testItems.Fixture.Create<UserDetails>();

        testItems.MockUserProfilePresenter.Setup(x => x.GetUserDetailsByUserIdAsync(
                testSubmissionInformation.SubmissionInformation_AcquirerUserId))
            .ReturnsAsync(() => testAcquirerUserDetails);

        var testAcquirerOrganisationInformation = testItems.Fixture.Create<OrganisationInformation>();

        testItems.MockUserProfilePresenter.Setup(x => x.GetOrganisationDetailsByOrganisationIdAsync(
                testSubmissionInformation.SubmissionInformation_AcquirerOrganisationId))
            .ReturnsAsync(() => testAcquirerOrganisationInformation);
        #endregion

        #region Set Up Key Question Answers
        var testProjectAims = testItems.Fixture.Create<string>();
        testItems.MockKeyQuestionPartAnswerProviderService.Setup(x => x.GetProjectAimsQuestionPartAnswerAsync(testDataShareRequestId))
            .ReturnsAsync(() => testProjectAims);

        var testDataTypes = testItems.Fixture.CreateMany<string>().ToList();
        testItems.MockKeyQuestionPartAnswerProviderService.Setup(x => x.GetDataTypesQuestionPartAnswerAsync(testDataShareRequestId))
            .ReturnsAsync(() => testDataTypes);

        var testWhenNeededBy = testItems.Fixture.Create<DateTime>();
        testItems.MockKeyQuestionPartAnswerProviderService.Setup(x => x.GetDateRequiredQuestionPartAnswerAsync(testDataShareRequestId))
            .ReturnsAsync(() => testWhenNeededBy);
        #endregion

        #region Set Up Answer Highlights
        var testAnswerHighlights = testItems.Fixture.CreateMany<string>().ToList();
        testItems.MockAnswerHighlightsProviderService.Setup(x => x.GetDataShareRequestsAnswerHighlightsAsync(
                testDataShareRequestId))
            .ReturnsAsync(() => testAnswerHighlights);
        #endregion

        #region Set Up Response Data Building
        testItems.MockSupplierDataShareRequestModelDataFactory.Setup(x => x.CreateSubmissionInformation(
                It.IsAny<SubmissionInformationModelData>()))
            .Returns((SubmissionInformationModelData submissionInformationModelData) =>
            {
                return testItems.Fixture
                    .Build<SubmissionInformation>()
                    .With(x => x.DataShareRequestId, submissionInformationModelData.SubmissionInformation_DataShareRequestId)
                    .With(x => x.DataShareRequestRequestId, submissionInformationModelData.SubmissionInformation_DataShareRequestRequestId)
                    .With(x => x.SubmittedOn, submissionInformationModelData.SubmissionInformation_SubmittedOn)
                    .With(x => x.AcquirerEmailAddress, submissionInformationModelData.SubmissionInformation_AcquirerEmailAddress)
                    .With(x => x.AcquirerOrganisationName, submissionInformationModelData.SubmissionInformation_AcquirerOrganisationName)
                    .With(x => x.ProjectAims, submissionInformationModelData.SubmissionInformation_ProjectAims)
                    .With(x => x.DataTypes, submissionInformationModelData.SubmissionInformation_DataTypes)
                    .With(x => x.WhenNeededBy, submissionInformationModelData.SubmissionInformation_WhenNeededBy)
                    .With(x => x.AnswerHighlights, submissionInformationModelData.SubmissionInformation_AnswerHighlights)
                    .Create();
            });
        #endregion
        #endregion

        testItems.MockSupplierDataShareRequestRepository.Setup(x => x.GetSubmissionDetailsModelDataAsync(testDataShareRequestId))
            .Throws(() => new Exception("test error message"));

        TestSetUpFailedDataResultHandling<SubmissionContentAsFile>(testItems);

        var result = await testItems.SupplierDataShareRequestService.GetSubmissionContentAsFileAsync(
            testDataShareRequestId, DataShareRequestFileFormat.Pdf);

        Assert.Multiple(() =>
        {
            Assert.That(result.Success, Is.False);
            Assert.That(result.Error, Is.EqualTo("Failed to GetSubmissionDetailsModelData from repository"));
        });
    }

    [Test]
    public async Task GivenAnAttemptToBuildSubmissionContentForAnUnSupportedFileType_WhenIGetSubmissionContentAsFileAsync_ThenAFailedResultIsReturned()
    {
        var testItems = CreateTestItems();

        var testDataShareRequestId = testItems.Fixture.Create<Guid>();

        testItems.MockSupplierDataShareRequestRepository.Setup(x => x.GetDataShareRequestStatusAsync(testDataShareRequestId))
            .ReturnsAsync(() => DataShareRequestStatusType.Submitted);

        #region Set Up Submission Information
        var testSubmissionInformation = testItems.Fixture
            .Build<SubmissionInformationModelData>()
            .With(x => x.SubmissionInformation_DataShareRequestId, testDataShareRequestId)
            .Create();

        testItems.MockSupplierDataShareRequestRepository
            .Setup(x => x.GetSubmissionInformationModelDataAsync(testDataShareRequestId))
            .ReturnsAsync(() => testSubmissionInformation);

        #region Set Up Timestamp Information
        var testAuditLogStatusChange = testItems.Fixture
            .Build<AuditLogDataShareRequestStatusChangeModelData>()
            .Create();

        testItems.MockAuditLogService.Setup(x => x.GetMostRecentAuditLogForDataShareRequestStatusChangeToStatusAsync(
                testDataShareRequestId,
                DataShareRequestStatusType.Submitted))
            .ReturnsAsync(() => testAuditLogStatusChange);
        #endregion

        #region Set Up Acquirer Information
        var testAcquirerUserDetails = testItems.Fixture.Create<UserDetails>();

        testItems.MockUserProfilePresenter.Setup(x => x.GetUserDetailsByUserIdAsync(
                testSubmissionInformation.SubmissionInformation_AcquirerUserId))
            .ReturnsAsync(() => testAcquirerUserDetails);

        var testAcquirerOrganisationInformation = testItems.Fixture.Create<OrganisationInformation>();

        testItems.MockUserProfilePresenter.Setup(x => x.GetOrganisationDetailsByOrganisationIdAsync(
                testSubmissionInformation.SubmissionInformation_AcquirerOrganisationId))
            .ReturnsAsync(() => testAcquirerOrganisationInformation);
        #endregion

        #region Set Up Key Question Answers
        var testProjectAims = testItems.Fixture.Create<string>();
        testItems.MockKeyQuestionPartAnswerProviderService.Setup(x => x.GetProjectAimsQuestionPartAnswerAsync(testDataShareRequestId))
            .ReturnsAsync(() => testProjectAims);

        var testDataTypes = testItems.Fixture.CreateMany<string>().ToList();
        testItems.MockKeyQuestionPartAnswerProviderService.Setup(x => x.GetDataTypesQuestionPartAnswerAsync(testDataShareRequestId))
            .ReturnsAsync(() => testDataTypes);

        var testWhenNeededBy = testItems.Fixture.Create<DateTime>();
        testItems.MockKeyQuestionPartAnswerProviderService.Setup(x => x.GetDateRequiredQuestionPartAnswerAsync(testDataShareRequestId))
            .ReturnsAsync(() => testWhenNeededBy);
        #endregion

        #region Set Up Answer Highlights
        var testAnswerHighlights = testItems.Fixture.CreateMany<string>().ToList();
        testItems.MockAnswerHighlightsProviderService.Setup(x => x.GetDataShareRequestsAnswerHighlightsAsync(
                testDataShareRequestId))
            .ReturnsAsync(() => testAnswerHighlights);
        #endregion

        #region Set Up Response Data Building
        testItems.MockSupplierDataShareRequestModelDataFactory.Setup(x => x.CreateSubmissionInformation(
                It.IsAny<SubmissionInformationModelData>()))
            .Returns((SubmissionInformationModelData submissionInformationModelData) =>
            {
                return testItems.Fixture
                    .Build<SubmissionInformation>()
                    .With(x => x.DataShareRequestId, submissionInformationModelData.SubmissionInformation_DataShareRequestId)
                    .With(x => x.DataShareRequestRequestId, submissionInformationModelData.SubmissionInformation_DataShareRequestRequestId)
                    .With(x => x.SubmittedOn, submissionInformationModelData.SubmissionInformation_SubmittedOn)
                    .With(x => x.AcquirerEmailAddress, submissionInformationModelData.SubmissionInformation_AcquirerEmailAddress)
                    .With(x => x.AcquirerOrganisationName, submissionInformationModelData.SubmissionInformation_AcquirerOrganisationName)
                    .With(x => x.ProjectAims, submissionInformationModelData.SubmissionInformation_ProjectAims)
                    .With(x => x.DataTypes, submissionInformationModelData.SubmissionInformation_DataTypes)
                    .With(x => x.WhenNeededBy, submissionInformationModelData.SubmissionInformation_WhenNeededBy)
                    .With(x => x.AnswerHighlights, submissionInformationModelData.SubmissionInformation_AnswerHighlights)
                    .Create();
            });
        #endregion
        #endregion

        #region Set Up Submission Details
        var testSubmissionDetails = testItems.Fixture
            .Build<SubmissionDetailsModelData>()
            .With(x => x.SubmissionDetails_DataShareRequestId, testDataShareRequestId)
            .Create();

        testItems.MockSupplierDataShareRequestRepository
            .Setup(x => x.GetSubmissionDetailsModelDataAsync(testDataShareRequestId))
            .ReturnsAsync(() => testSubmissionDetails);

        #region Set Up Response Data Building
        testItems.MockSupplierDataShareRequestModelDataFactory.Setup(x => x.CreateSubmissionDetails(
                It.IsAny<SubmissionDetailsModelData>()))
            .Returns((SubmissionDetailsModelData submissionDetailsModelData) =>
            {
                return testItems.Fixture
                    .Build<SubmissionDetails>()
                    .With(x => x.DataShareRequestId, submissionDetailsModelData.SubmissionDetails_DataShareRequestId)
                    .With(x => x.DataShareRequestRequestId, submissionDetailsModelData.SubmissionDetails_DataShareRequestRequestId)
                    .With(x => x.AcquirerOrganisationName, submissionDetailsModelData.SubmissionDetails_AcquirerOrganisationName)
                    .Create();
            });
        #endregion
        #endregion

        #region Set Up Submission Content Building
        var testFileContent = testItems.Fixture.Create<byte[]>();

        testItems.MockSubmissionContentPdfFileBuilder.Setup(x => x.BuildAsync(
                It.Is<SubmissionInformation>(information => information.DataShareRequestId == testDataShareRequestId),
                It.Is<SubmissionDetails>(details => details.DataShareRequestId == testDataShareRequestId)))
            .ReturnsAsync(() => testFileContent);
        #endregion

        TestSetUpFailedDataResultHandling<SubmissionContentAsFile>(testItems);

        var invalidDataShareRequestFileFormat = (DataShareRequestFileFormat) Enum.GetValues<DataShareRequestFileFormat>().Cast<int>().Max() + 1;

        var result = await testItems.SupplierDataShareRequestService.GetSubmissionContentAsFileAsync(
            testDataShareRequestId, invalidDataShareRequestFileFormat);

        Assert.Multiple(() =>
        {
            Assert.That(result.Success, Is.False);
            Assert.That(result.Error, Is.EqualTo("Unhandled file format requested"));
        });
    }
    #endregion

    #region StartSubmissionReviewAsync() Tests
    [Test]
    public async Task GivenASubmittedDataShareRequest_WhenIStartSubmissionReviewAsync_ThenSubmissionReviewIsStartedForThatRequest()
    {
        var testItems = CreateTestItems();

        var testDataShareRequestId = testItems.Fixture.Create<Guid>();

        testItems.MockSupplierDataShareRequestRepository.Setup(x => x.GetDataShareRequestStatusAsync(testDataShareRequestId))
            .ReturnsAsync(() => DataShareRequestStatusType.Submitted);

        var testInitiatingUserIdSet = testItems.Fixture.Create<UserIdSet>();

        testItems.MockUserProfilePresenter.Setup(x => x.GetInitiatingUserIdSetAsync())
            .ReturnsAsync(() => testInitiatingUserIdSet);

        await testItems.SupplierDataShareRequestService.StartSubmissionReviewAsync(testDataShareRequestId);

        testItems.MockSupplierDataShareRequestRepository.Verify(x => x.StartSubmissionReviewAsync(
                testInitiatingUserIdSet,
                testDataShareRequestId),
            Times.Once);
    }

    [Test]
    public async Task GivenStartingASubmissionReviewWillFail_WhenIStartSubmissionReviewAsync_ThenAFailedResultIsReturned()
    {
        var testItems = CreateTestItems();

        var testDataShareRequestId = testItems.Fixture.Create<Guid>();

        testItems.MockSupplierDataShareRequestRepository.Setup(x => x.GetDataShareRequestStatusAsync(testDataShareRequestId))
            .ReturnsAsync(() => DataShareRequestStatusType.Submitted);

        var testInitiatingUserIdSet = testItems.Fixture.Create<UserIdSet>();

        testItems.MockUserProfilePresenter.Setup(x => x.GetInitiatingUserIdSetAsync())
            .ReturnsAsync(() => testInitiatingUserIdSet);

        testItems.MockSupplierDataShareRequestRepository.Setup(x => x.StartSubmissionReviewAsync(
                testInitiatingUserIdSet,
                testDataShareRequestId))
            .Throws(() => new Exception());

        TestSetUpFailedDataResultHandling<SubmissionReviewInformation>(testItems);

        var result = await testItems.SupplierDataShareRequestService.StartSubmissionReviewAsync(testDataShareRequestId);
        
        Assert.Multiple(() =>
        {
            Assert.That(result.Success, Is.False);
            Assert.That(result.Error, Is.EqualTo("Failed to StartSubmissionReview"));
        });
    }

    [Test]
    public async Task GivenASubmittedDataShareRequest_WhenIStartSubmissionReviewAsync_ThenSubmissionReviewInformationForThatRequestIsReturned()
    {
        var testItems = CreateTestItems();

        var testDataShareRequestId = testItems.Fixture.Create<Guid>();

        testItems.MockSupplierDataShareRequestRepository.Setup(x => x.GetDataShareRequestStatusAsync(testDataShareRequestId))
            .ReturnsAsync(() => DataShareRequestStatusType.Submitted);

        #region Set Up Review Information
        var testSubmissionDetailsModelData = testItems.Fixture
            .Build<SubmissionDetailsModelData>()
            .With(x => x.SubmissionDetails_DataShareRequestId, testDataShareRequestId)
            .Create();

        var testSupplierNotes = testItems.Fixture.Create<string>();

        var testSubmissionReviewInformationModelData = testItems.Fixture
            .Build<SubmissionReviewInformationModelData>()
            .With(x => x.SubmissionReviewInformation_SubmissionDetails, testSubmissionDetailsModelData)
            .With(x => x.SubmissionReviewInformation_SupplierNotes, testSupplierNotes)
            .Create();

        testItems.MockSupplierDataShareRequestRepository.Setup(x => x.GetSubmissionReviewInformationModelDataAsync(testDataShareRequestId))
            .ReturnsAsync(() => testSubmissionReviewInformationModelData);

        #region Set Up Acquirer Information
        var testAcquirerOrganisationInformation = testItems.Fixture.Build<OrganisationInformation>()
            .With(x => x.OrganisationName, testSubmissionDetailsModelData.SubmissionDetails_AcquirerOrganisationName)
            .Create();

        testItems.MockUserProfilePresenter.Setup(x => x.GetOrganisationDetailsByOrganisationIdAsync(
                testSubmissionReviewInformationModelData.SubmissionReviewInformation_SubmissionDetails.SubmissionDetails_AcquirerOrganisationId))
            .ReturnsAsync(() => testAcquirerOrganisationInformation);
        #endregion

        #region Set Up Return Comments
        var testReturnedOn = testItems.Fixture.Create<DateTime>();

        var testReturnedStatusChange = testItems.Fixture
            .Build<AuditLogDataShareRequestStatusChangeModelData>()
            .With(x => x.AuditLogDataShareRequestStatusChange_ChangedAtUtc, testReturnedOn)
            .Create();

        testItems.MockAuditLogService.Setup(x => x.GetAuditLogsForDataShareRequestStatusChangeToStatusAsync(
                testDataShareRequestId,
                new List<DataShareRequestStatusType> { DataShareRequestStatusType.Returned }))
            .ReturnsAsync(() => [testReturnedStatusChange]);
        #endregion

        #region Set Up Response Data Building

        var testSubmissionDetails = testItems.Fixture
            .Build<SubmissionDetails>()
            .With(x => x.DataShareRequestId, testSubmissionDetailsModelData.SubmissionDetails_DataShareRequestId)
            .With(x => x.DataShareRequestRequestId, testSubmissionDetailsModelData.SubmissionDetails_DataShareRequestRequestId)
            .With(x => x.AcquirerOrganisationName, testSubmissionDetailsModelData.SubmissionDetails_AcquirerOrganisationName)
            .Create();

        testItems.MockSupplierDataShareRequestModelDataFactory.Setup(x => x.CreateSubmissionReviewInformation(
                testSubmissionReviewInformationModelData))
            .Returns(() =>
            {
                return testItems.Fixture
                    .Build<SubmissionReviewInformation>()
                    .With(x => x.SubmissionDetails, testSubmissionDetails)
                    .With(x => x.SupplierNotes, testSubmissionReviewInformationModelData.SubmissionReviewInformation_SupplierNotes)
                    .Create();
            });
        #endregion
        #endregion

        TestSetUpSuccessfulDataResultHandling<SubmissionReviewInformation>(testItems);

        var result = await testItems.SupplierDataShareRequestService.StartSubmissionReviewAsync(testDataShareRequestId);

        Assert.Multiple(() =>
        {
            var resultSubmissionReviewInformation = result.Data!;

            var resultSubmissionDetails = resultSubmissionReviewInformation.SubmissionDetails;
            Assert.That(resultSubmissionDetails.DataShareRequestId, Is.EqualTo(testSubmissionReviewInformationModelData.SubmissionReviewInformation_SubmissionDetails.SubmissionDetails_DataShareRequestId));
            Assert.That(resultSubmissionDetails.DataShareRequestRequestId, Is.EqualTo(testSubmissionReviewInformationModelData.SubmissionReviewInformation_SubmissionDetails.SubmissionDetails_DataShareRequestRequestId));
            Assert.That(resultSubmissionDetails.AcquirerOrganisationName, Is.EqualTo(testSubmissionReviewInformationModelData.SubmissionReviewInformation_SubmissionDetails.SubmissionDetails_AcquirerOrganisationName));

            Assert.That(resultSubmissionReviewInformation.SupplierNotes, Is.EqualTo(testSupplierNotes));
        });
    }
    #endregion

    #region GetSubmissionReviewInformationAsync() Tests
    [Test]
    public async Task GivenASubmittedDataShareRequest_WhenIGetSubmissionReviewInformationAsync_ThenSubmissionReviewInformationForThatRequestIsReturned()
    {
        var testItems = CreateTestItems();

        var testDataShareRequestId = testItems.Fixture.Create<Guid>();

        testItems.MockSupplierDataShareRequestRepository.Setup(x => x.GetDataShareRequestStatusAsync(testDataShareRequestId))
            .ReturnsAsync(() => DataShareRequestStatusType.Submitted);

        #region Set Up Review Information
        var testSubmissionDetailsModelData = testItems.Fixture
            .Build<SubmissionDetailsModelData>()
            .With(x => x.SubmissionDetails_DataShareRequestId, testDataShareRequestId)
            .Create();

        var testSupplierNotes = testItems.Fixture.Create<string>();

        var testSubmissionReviewInformationModelData = testItems.Fixture
            .Build<SubmissionReviewInformationModelData>()
            .With(x => x.SubmissionReviewInformation_SubmissionDetails, testSubmissionDetailsModelData)
            .With(x => x.SubmissionReviewInformation_SupplierNotes, testSupplierNotes)
            .Create();

        testItems.MockSupplierDataShareRequestRepository.Setup(x => x.GetSubmissionReviewInformationModelDataAsync(testDataShareRequestId))
            .ReturnsAsync(() => testSubmissionReviewInformationModelData);

        #region Set Up Acquirer Information
        var testAcquirerOrganisationInformation = testItems.Fixture.Build<OrganisationInformation>()
            .With(x => x.OrganisationName, testSubmissionDetailsModelData.SubmissionDetails_AcquirerOrganisationName)
            .Create();

        testItems.MockUserProfilePresenter.Setup(x => x.GetOrganisationDetailsByOrganisationIdAsync(
                testSubmissionReviewInformationModelData.SubmissionReviewInformation_SubmissionDetails.SubmissionDetails_AcquirerOrganisationId))
            .ReturnsAsync(() => testAcquirerOrganisationInformation);
        #endregion

        #region Set Up Return Comments
        var testReturnedOn = testItems.Fixture.Create<DateTime>();

        var testReturnedStatusChange = testItems.Fixture
            .Build<AuditLogDataShareRequestStatusChangeModelData>()
            .With(x => x.AuditLogDataShareRequestStatusChange_ChangedAtUtc, testReturnedOn)
            .Create();

        testItems.MockAuditLogService.Setup(x => x.GetAuditLogsForDataShareRequestStatusChangeToStatusAsync(
                testDataShareRequestId,
                new List<DataShareRequestStatusType> { DataShareRequestStatusType.Returned }))
            .ReturnsAsync(() => [testReturnedStatusChange]);
        #endregion

        #region Set Up Response Data Building
        var testSubmissionDetails = testItems.Fixture
            .Build<SubmissionDetails>()
            .With(x => x.DataShareRequestId, testSubmissionDetailsModelData.SubmissionDetails_DataShareRequestId)
            .With(x => x.DataShareRequestRequestId, testSubmissionDetailsModelData.SubmissionDetails_DataShareRequestRequestId)
            .With(x => x.AcquirerOrganisationName, testSubmissionDetailsModelData.SubmissionDetails_AcquirerOrganisationName)
            .Create();

        testItems.MockSupplierDataShareRequestModelDataFactory.Setup(x => x.CreateSubmissionReviewInformation(
                testSubmissionReviewInformationModelData))
            .Returns(() =>
            {
                return testItems.Fixture
                    .Build<SubmissionReviewInformation>()
                    .With(x => x.SubmissionDetails, testSubmissionDetails)
                    .With(x => x.SupplierNotes, testSubmissionReviewInformationModelData.SubmissionReviewInformation_SupplierNotes)
                    .Create();
            });
        #endregion
        #endregion

        TestSetUpSuccessfulDataResultHandling<SubmissionReviewInformation>(testItems);

        var result = await testItems.SupplierDataShareRequestService.GetSubmissionReviewInformationAsync(testDataShareRequestId);

        Assert.Multiple(() =>
        {
            var resultSubmissionReviewInformation = result.Data!;

            var resultSubmissionDetails = resultSubmissionReviewInformation.SubmissionDetails;
            Assert.That(resultSubmissionDetails.DataShareRequestId, Is.EqualTo(testSubmissionReviewInformationModelData.SubmissionReviewInformation_SubmissionDetails.SubmissionDetails_DataShareRequestId));
            Assert.That(resultSubmissionDetails.DataShareRequestRequestId, Is.EqualTo(testSubmissionReviewInformationModelData.SubmissionReviewInformation_SubmissionDetails.SubmissionDetails_DataShareRequestRequestId));
            Assert.That(resultSubmissionDetails.AcquirerOrganisationName, Is.EqualTo(testSubmissionReviewInformationModelData.SubmissionReviewInformation_SubmissionDetails.SubmissionDetails_AcquirerOrganisationName));

            Assert.That(resultSubmissionReviewInformation.SupplierNotes, Is.EqualTo(testSupplierNotes));
        });
    }

    [Test]
    public async Task GivenGettingSubmissionReviewInformationWillFail_WhenIGetSubmissionReviewInformationAsync_ThenAFailedResultIsReturned()
    {
        var testItems = CreateTestItems();

        var testDataShareRequestId = testItems.Fixture.Create<Guid>();

        testItems.MockSupplierDataShareRequestRepository.Setup(x => x.GetSubmissionReviewInformationModelDataAsync(testDataShareRequestId))
            .Throws(() => new Exception());

        TestSetUpFailedDataResultHandling<SubmissionReviewInformation>(testItems);

        var result = await testItems.SupplierDataShareRequestService.GetSubmissionReviewInformationAsync(testDataShareRequestId);

        Assert.Multiple(() =>
        {
            Assert.That(result.Success, Is.False);
            Assert.That(result.Error, Is.EqualTo("Failed to GetSubmissionReviewInformationModelData from repository"));
        });
    }
    #endregion

    #region GetReturnedSubmissionInformationAsync() Tests
    [Test]
    public async Task GivenADataShareRequestHasBeenReturned_WhenIGetReturnedSubmissionInformationAsync_ThenTheReturnedSubmissionInformationForThatRequestIsReturned()
    {
        var testItems = CreateTestItems();

        var testDataShareRequestId = testItems.Fixture.Create<Guid>();

        var testReturnedSubmissionInformationModelData = testItems.Fixture
            .Build<ReturnedSubmissionInformationModelData>()
            .With(x => x.ReturnedSubmission_DataShareRequestId, testDataShareRequestId)
            .Create();

        testItems.MockSupplierDataShareRequestRepository.Setup(x => x.GetReturnedSubmissionInformationAsync(testDataShareRequestId))
            .ReturnsAsync(() => testReturnedSubmissionInformationModelData);

        #region Set Up Timestamp Information
        var testAuditLogSubmittedStatusChange = testItems.Fixture
            .Build<AuditLogDataShareRequestStatusChangeModelData>()
            .Create();

        testItems.MockAuditLogService.Setup(x => x.GetMostRecentAuditLogForDataShareRequestStatusChangeToStatusAsync(
                testDataShareRequestId,
                DataShareRequestStatusType.Submitted))
            .ReturnsAsync(() => testAuditLogSubmittedStatusChange);

        var testAuditLogReturnedStatusChange = testItems.Fixture
            .Build<AuditLogDataShareRequestStatusChangeModelData>()
            .Create();

        testItems.MockAuditLogService.Setup(x => x.GetMostRecentAuditLogForDataShareRequestStatusChangeToStatusAsync(
                testDataShareRequestId,
                DataShareRequestStatusType.Returned))
            .ReturnsAsync(() => testAuditLogReturnedStatusChange);
        #endregion

        #region Set Up Acquirer Information
        var testAcquirerOrganisationInformation = testItems.Fixture.Create<OrganisationInformation>();

        testItems.MockUserProfilePresenter.Setup(x => x.GetOrganisationDetailsByOrganisationIdAsync(
                testReturnedSubmissionInformationModelData.ReturnedSubmission_AcquirerOrganisationId))
            .ReturnsAsync(() => testAcquirerOrganisationInformation);
        #endregion

        #region Set Up Key Question Answers
        var testWhenNeededBy = testItems.Fixture.Create<DateTime>();
        testItems.MockKeyQuestionPartAnswerProviderService.Setup(x => x.GetDateRequiredQuestionPartAnswerAsync(testDataShareRequestId))
            .ReturnsAsync(() => testWhenNeededBy);
        #endregion

        #region Set Up Response Data Building
        testItems.MockSupplierDataShareRequestModelDataFactory.Setup(x => x.CreateReturnedSubmissionInformation(
                It.IsAny<ReturnedSubmissionInformationModelData>()))
            .Returns((ReturnedSubmissionInformationModelData returnedSubmissionInformationModelData) =>
            {
                return testItems.Fixture
                    .Build<ReturnedSubmissionInformation>()
                    .With(x => x.DataShareRequestId, returnedSubmissionInformationModelData.ReturnedSubmission_DataShareRequestId)
                    .With(x => x.DataShareRequestRequestId, returnedSubmissionInformationModelData.ReturnedSubmission_DataShareRequestRequestId)
                    .With(x => x.SubmittedOn, returnedSubmissionInformationModelData.ReturnedSubmission_SubmittedOn)
                    .With(x => x.ReturnedOn, returnedSubmissionInformationModelData.ReturnedSubmission_ReturnedOn)
                    .With(x => x.AcquirerOrganisationName, returnedSubmissionInformationModelData.ReturnedSubmission_AcquirerOrganisationName)
                    .With(x => x.WhenNeededBy, returnedSubmissionInformationModelData.ReturnedSubmission_WhenNeededBy)
                    .Create();
            });
        #endregion

        TestSetUpSuccessfulDataResultHandling<ReturnedSubmissionInformation>(testItems);

        var result = await testItems.SupplierDataShareRequestService.GetReturnedSubmissionInformationAsync(testDataShareRequestId);

        Assert.Multiple(() =>
        {
            var resultReturnedSubmissionInformation = result.Data!;

            Assert.That(resultReturnedSubmissionInformation.DataShareRequestId, Is.EqualTo(testReturnedSubmissionInformationModelData.ReturnedSubmission_DataShareRequestId));
            Assert.That(resultReturnedSubmissionInformation.DataShareRequestRequestId, Is.EqualTo(testReturnedSubmissionInformationModelData.ReturnedSubmission_DataShareRequestRequestId));
            Assert.That(resultReturnedSubmissionInformation.SubmittedOn, Is.EqualTo(testReturnedSubmissionInformationModelData.ReturnedSubmission_SubmittedOn));
            Assert.That(resultReturnedSubmissionInformation.ReturnedOn, Is.EqualTo(testReturnedSubmissionInformationModelData.ReturnedSubmission_ReturnedOn));
            Assert.That(resultReturnedSubmissionInformation.AcquirerOrganisationName, Is.EqualTo(testReturnedSubmissionInformationModelData.ReturnedSubmission_AcquirerOrganisationName));
            Assert.That(resultReturnedSubmissionInformation.WhenNeededBy, Is.EqualTo(testReturnedSubmissionInformationModelData.ReturnedSubmission_WhenNeededBy));
        });
    }

    [Test]
    public async Task GivenGettingReturnedSubmissionInformationWillFail_WhenIGetReturnedSubmissionInformationAsync_ThenAFailedResultIsReturned()
    {
        var testItems = CreateTestItems();

        var testDataShareRequestId = testItems.Fixture.Create<Guid>();

        testItems.MockSupplierDataShareRequestRepository.Setup(x => x.GetReturnedSubmissionInformationAsync(testDataShareRequestId))
            .Throws(() => new Exception("test error message"));

        TestSetUpFailedDataResultHandling<ReturnedSubmissionInformation>(testItems);

        var result = await testItems.SupplierDataShareRequestService.GetReturnedSubmissionInformationAsync(testDataShareRequestId);

        Assert.Multiple(() =>
        {
            Assert.That(result.Success, Is.False);
            Assert.That(result.Error, Is.EqualTo("test error message"));
        });
    }

    [Test]
    public async Task GivenADataShareRequestHasBeenReturnedButThereIsNoSubmitEntryInTheAuditLog_WhenIGetReturnedSubmissionInformationAsync_ThenAFailedResultIsReturned()
    {
        var testItems = CreateTestItems();

        var testDataShareRequestId = testItems.Fixture.Create<Guid>();

        var testReturnedSubmissionInformationModelData = testItems.Fixture
            .Build<ReturnedSubmissionInformationModelData>()
            .With(x => x.ReturnedSubmission_DataShareRequestId, testDataShareRequestId)
            .Create();

        testItems.MockSupplierDataShareRequestRepository.Setup(x => x.GetReturnedSubmissionInformationAsync(testDataShareRequestId))
            .ReturnsAsync(() => testReturnedSubmissionInformationModelData);

        #region Set Up Timestamp Information

        testItems.MockAuditLogService.Setup(x => x.GetMostRecentAuditLogForDataShareRequestStatusChangeToStatusAsync(
                testDataShareRequestId,
                DataShareRequestStatusType.Submitted))
            .ReturnsAsync(() => null);
        #endregion

        TestSetUpFailedDataResultHandling<ReturnedSubmissionInformation>(testItems);

        var result = await testItems.SupplierDataShareRequestService.GetReturnedSubmissionInformationAsync(testDataShareRequestId);

        Assert.Multiple(() =>
        {
            Assert.That(result.Success, Is.False);
            Assert.That(result.Error, Is.EqualTo("Returned submission has no Submitted entry in the audit log"));
        });
    }

    [Test]
    public async Task GivenADataShareRequestHasBeenReturnedButThereIsNoReturnEntryInTheLog_WhenIGetReturnedSubmissionInformationAsync_ThenAFailedResultIsReturned()
    {
        var testItems = CreateTestItems();

        var testDataShareRequestId = testItems.Fixture.Create<Guid>();

        var testReturnedSubmissionInformationModelData = testItems.Fixture
            .Build<ReturnedSubmissionInformationModelData>()
            .With(x => x.ReturnedSubmission_DataShareRequestId, testDataShareRequestId)
            .Create();

        testItems.MockSupplierDataShareRequestRepository.Setup(x => x.GetReturnedSubmissionInformationAsync(testDataShareRequestId))
            .ReturnsAsync(() => testReturnedSubmissionInformationModelData);

        #region Set Up Timestamp Information
        var testAuditLogSubmittedStatusChange = testItems.Fixture
            .Build<AuditLogDataShareRequestStatusChangeModelData>()
            .Create();

        testItems.MockAuditLogService.Setup(x => x.GetMostRecentAuditLogForDataShareRequestStatusChangeToStatusAsync(
                testDataShareRequestId,
                DataShareRequestStatusType.Submitted))
            .ReturnsAsync(() => testAuditLogSubmittedStatusChange);

        testItems.MockAuditLogService.Setup(x => x.GetMostRecentAuditLogForDataShareRequestStatusChangeToStatusAsync(
                testDataShareRequestId,
                DataShareRequestStatusType.Returned))
            .ReturnsAsync(() => null);
        #endregion

        TestSetUpFailedDataResultHandling<ReturnedSubmissionInformation>(testItems);

        var result = await testItems.SupplierDataShareRequestService.GetReturnedSubmissionInformationAsync(testDataShareRequestId);

        Assert.Multiple(() =>
        {
            Assert.That(result.Success, Is.False);
            Assert.That(result.Error, Is.EqualTo("Returned submission has no Returned entry in the audit log"));
        });
    }
    #endregion

    #region GetCompletedSubmissionInformationAsync() Tests
    [Test]
    public async Task GivenADataShareRequestHasBeenCompleted_WhenIGetCompletedSubmissionInformationAsync_ThenTheCompletedSubmissionInformationForThatRequestIsCompleted()
    {
        var testItems = CreateTestItems();

        var testDataShareRequestId = testItems.Fixture.Create<Guid>();

        var testCompletedSubmissionInformationModelData = testItems.Fixture
            .Build<CompletedSubmissionInformationModelData>()
            .With(x => x.CompletedSubmission_DataShareRequestId, testDataShareRequestId)
            .Create();

        testItems.MockSupplierDataShareRequestRepository.Setup(x => x.GetCompletedSubmissionInformationAsync(testDataShareRequestId))
            .ReturnsAsync(() => testCompletedSubmissionInformationModelData);

        #region Set Up Timestamp Information
        var testAuditLogCompletedStatusChange = testItems.Fixture
            .Build<AuditLogDataShareRequestStatusChangeModelData>()
            .Create();

        testItems.MockAuditLogService.Setup(x => x.GetMostRecentAuditLogForDataShareRequestStatusChangeToStatusAsync(
                testDataShareRequestId,
                DataShareRequestStatusType.Submitted))
            .ReturnsAsync(() => testAuditLogCompletedStatusChange);
        #endregion

        #region Set Up Acquirer Information
        var testAcquirerOrganisationInformation = testItems.Fixture.Create<OrganisationInformation>();

        testItems.MockUserProfilePresenter.Setup(x => x.GetOrganisationDetailsByOrganisationIdAsync(
                testCompletedSubmissionInformationModelData.CompletedSubmission_AcquirerOrganisationId))
            .ReturnsAsync(() => testAcquirerOrganisationInformation);
        #endregion

        #region Set Up Key Question Answers
        var testWhenNeededBy = testItems.Fixture.Create<DateTime>();
        testItems.MockKeyQuestionPartAnswerProviderService.Setup(x => x.GetDateRequiredQuestionPartAnswerAsync(testDataShareRequestId))
            .ReturnsAsync(() => testWhenNeededBy);
        #endregion

        #region Set Up Response Data Building
        testItems.MockSupplierDataShareRequestModelDataFactory.Setup(x => x.CreateCompletedSubmissionInformation(
                It.IsAny<CompletedSubmissionInformationModelData>()))
            .Returns((CompletedSubmissionInformationModelData completedSubmissionInformationModelData) =>
            {
                return testItems.Fixture
                    .Build<CompletedSubmissionInformation>()
                    .With(x => x.DataShareRequestId, completedSubmissionInformationModelData.CompletedSubmission_DataShareRequestId)
                    .With(x => x.DataShareRequestRequestId, completedSubmissionInformationModelData.CompletedSubmission_DataShareRequestRequestId)
                    .With(x => x.SubmittedOn, completedSubmissionInformationModelData.CompletedSubmission_SubmittedOn)
                    .With(x => x.CompletedOn, completedSubmissionInformationModelData.CompletedSubmission_CompletedOn)
                    .With(x => x.AcquirerOrganisationName, completedSubmissionInformationModelData.CompletedSubmission_AcquirerOrganisationName)
                    .With(x => x.WhenNeededBy, completedSubmissionInformationModelData.CompletedSubmission_WhenNeededBy)
                    .Create();
            });
        #endregion

        TestSetUpSuccessfulDataResultHandling<CompletedSubmissionInformation>(testItems);

        var result = await testItems.SupplierDataShareRequestService.GetCompletedSubmissionInformationAsync(testDataShareRequestId);

        Assert.Multiple(() =>
        {
            var resultCompletedSubmissionInformation = result.Data!;

            Assert.That(resultCompletedSubmissionInformation.DataShareRequestId, Is.EqualTo(testCompletedSubmissionInformationModelData.CompletedSubmission_DataShareRequestId));
            Assert.That(resultCompletedSubmissionInformation.DataShareRequestRequestId, Is.EqualTo(testCompletedSubmissionInformationModelData.CompletedSubmission_DataShareRequestRequestId));
            Assert.That(resultCompletedSubmissionInformation.SubmittedOn, Is.EqualTo(testCompletedSubmissionInformationModelData.CompletedSubmission_SubmittedOn));
            Assert.That(resultCompletedSubmissionInformation.CompletedOn, Is.EqualTo(testCompletedSubmissionInformationModelData.CompletedSubmission_CompletedOn));
            Assert.That(resultCompletedSubmissionInformation.AcquirerOrganisationName, Is.EqualTo(testCompletedSubmissionInformationModelData.CompletedSubmission_AcquirerOrganisationName));
            Assert.That(resultCompletedSubmissionInformation.WhenNeededBy, Is.EqualTo(testCompletedSubmissionInformationModelData.CompletedSubmission_WhenNeededBy));
        });
    }

    [Test]
    public async Task GivenGettingCompletedSubmissionInformationWillFail_WhenIGetCompletedSubmissionInformationAsync_ThenAFailedResultIsCompleted()
    {
        var testItems = CreateTestItems();

        var testDataShareRequestId = testItems.Fixture.Create<Guid>();

        testItems.MockSupplierDataShareRequestRepository.Setup(x => x.GetCompletedSubmissionInformationAsync(testDataShareRequestId))
            .Throws(() => new Exception("test error message"));

        TestSetUpFailedDataResultHandling<CompletedSubmissionInformation>(testItems);

        var result = await testItems.SupplierDataShareRequestService.GetCompletedSubmissionInformationAsync(testDataShareRequestId);

        Assert.Multiple(() =>
        {
            Assert.That(result.Success, Is.False);
            Assert.That(result.Error, Is.EqualTo("test error message"));
        });
    }

    [Test]
    public async Task GivenADataShareRequestHasBeenCompletedButThereIsNoSubmitEntryInTheAuditLog_WhenIGetCompletedSubmissionInformationAsync_ThenAFailedResultIsCompleted()
    {
        var testItems = CreateTestItems();

        var testDataShareRequestId = testItems.Fixture.Create<Guid>();

        var testCompletedSubmissionInformationModelData = testItems.Fixture
            .Build<CompletedSubmissionInformationModelData>()
            .With(x => x.CompletedSubmission_DataShareRequestId, testDataShareRequestId)
            .Create();

        testItems.MockSupplierDataShareRequestRepository.Setup(x => x.GetCompletedSubmissionInformationAsync(testDataShareRequestId))
            .ReturnsAsync(() => testCompletedSubmissionInformationModelData);

        #region Set Up Timestamp Information

        testItems.MockAuditLogService.Setup(x => x.GetMostRecentAuditLogForDataShareRequestStatusChangeToStatusAsync(
                testDataShareRequestId,
                DataShareRequestStatusType.Submitted))
            .ReturnsAsync(() => null);
        #endregion

        TestSetUpFailedDataResultHandling<CompletedSubmissionInformation>(testItems);

        var result = await testItems.SupplierDataShareRequestService.GetCompletedSubmissionInformationAsync(testDataShareRequestId);

        Assert.Multiple(() =>
        {
            Assert.That(result.Success, Is.False);
            Assert.That(result.Error, Is.EqualTo("Completed submission has no Submitted entry in the audit log"));
        });
    }

    [Test]
    public async Task GivenADataShareRequestHasBeenCompletedButThereIsNoReturnEntryInTheLog_WhenIGetCompletedSubmissionInformationAsync_ThenAFailedResultIsCompleted()
    {
        var testItems = CreateTestItems();

        var testDataShareRequestId = testItems.Fixture.Create<Guid>();

        var testCompletedSubmissionInformationModelData = testItems.Fixture
            .Build<CompletedSubmissionInformationModelData>()
            .With(x => x.CompletedSubmission_DataShareRequestId, testDataShareRequestId)
            .Create();

        testItems.MockSupplierDataShareRequestRepository.Setup(x => x.GetCompletedSubmissionInformationAsync(testDataShareRequestId))
            .ReturnsAsync(() => testCompletedSubmissionInformationModelData);

        #region Set Up Timestamp Information
        var testAuditLogSubmittedStatusChange = testItems.Fixture
            .Build<AuditLogDataShareRequestStatusChangeModelData>()
            .Create();

        testItems.MockAuditLogService.Setup(x => x.GetMostRecentAuditLogForDataShareRequestStatusChangeToStatusAsync(
                testDataShareRequestId,
                DataShareRequestStatusType.Submitted))
            .ReturnsAsync(() => testAuditLogSubmittedStatusChange);

        testItems.MockAuditLogService.Setup(x => x.GetMostRecentAuditLogForDataShareRequestStatusChangeToStatusAsync(
                testDataShareRequestId,
                DataShareRequestStatusType.Accepted))
            .ReturnsAsync(() => null);

        testItems.MockAuditLogService.Setup(x => x.GetMostRecentAuditLogForDataShareRequestStatusChangeToStatusAsync(
                testDataShareRequestId,
                DataShareRequestStatusType.Rejected))
            .ReturnsAsync(() => null);
        #endregion

        TestSetUpFailedDataResultHandling<CompletedSubmissionInformation>(testItems);

        var result = await testItems.SupplierDataShareRequestService.GetCompletedSubmissionInformationAsync(testDataShareRequestId);

        Assert.Multiple(() =>
        {
            Assert.That(result.Success, Is.False);
            Assert.That(result.Error, Is.EqualTo("Completed submission has no Completed entry in the audit log"));
        });
    }
    #endregion

    #region SetSubmissionNotesAsync() Tests

    [Test]
    public async Task GivenADataShareRequest_WhenISetSubmissionNotesAsync_ThenTheSubmissionNotesAreSet()
    {
        var testItems = CreateTestItems();

        var testDataShareRequestId = testItems.Fixture.Create<Guid>();

        testItems.MockSupplierDataShareRequestRepository.Setup(x => x.GetDataShareRequestStatusAsync(
                testDataShareRequestId))
            .ReturnsAsync(() => DataShareRequestStatusType.Submitted);

        var testNotes = testItems.Fixture.Create<string>();

        await testItems.SupplierDataShareRequestService.SetSubmissionNotesAsync(testDataShareRequestId, testNotes);

        testItems.MockSupplierDataShareRequestRepository.Verify(x => x.SetSubmissionNotesAsync(
                testDataShareRequestId,
                testNotes),
            Times.Once);
    }

    [Test]
    public async Task GivenSettingSubmissionNotesSucceeds_WhenISetSubmissionNotesAsync_ThenSuccessIsReturned()
    {
        var testItems = CreateTestItems();

        var testDataShareRequestId = testItems.Fixture.Create<Guid>();
        var testNotes = testItems.Fixture.Create<string>();

        TestSetUpSuccessfulResultHandling(testItems);

        var result = await testItems.SupplierDataShareRequestService.SetSubmissionNotesAsync(testDataShareRequestId, testNotes);

        Assert.That(result.Success, Is.True);
    }

    [Test]
    public async Task GivenSettingSubmissionNotesWillFail_WhenISetSubmissionNotesAsync_ThenFailureIsReturned()
    {
        var testItems = CreateTestItems();

        var testDataShareRequestId = testItems.Fixture.Create<Guid>();
        var testNotes = testItems.Fixture.Create<string>();

        testItems.MockSupplierDataShareRequestRepository.Setup(x => x.GetDataShareRequestStatusAsync(
                testDataShareRequestId))
            .ReturnsAsync(() => DataShareRequestStatusType.Submitted);

        testItems.MockSupplierDataShareRequestRepository.Setup(x => x.SetSubmissionNotesAsync(
                testDataShareRequestId,
                testNotes))
            .Throws(() => new Exception("test error message"));

        TestSetUpFailedResultHandling(testItems);

        var result = await testItems.SupplierDataShareRequestService.SetSubmissionNotesAsync(testDataShareRequestId, testNotes);

        Assert.Multiple(() =>
        {
            Assert.That(result.Success, Is.False);
            Assert.That(result.Error, Is.EqualTo("test error message"));
        });
    }
    #endregion

    #region AcceptSubmissionAsync() Tests
    [Test]
    public async Task GivenASubmittedDataShareRequest_WhenIAcceptSubmissionAsync_ThenASummaryOfTheAcceptanceIsReturned()
    {
        var testItems = CreateTestItems();

        var testDataShareRequestId = testItems.Fixture.Create<Guid>();
        var testSupplierFeedback = testItems.Fixture.Create<string>();

        testItems.MockSupplierDataShareRequestRepository.Setup(x => x.GetDataShareRequestStatusAsync(testDataShareRequestId))
            .ReturnsAsync(() => DataShareRequestStatusType.Submitted);

        #region Set Up Decision Summary
        var testAcceptedDecisionSummaryModelData = testItems.Fixture
            .Build<AcceptedDecisionSummaryModelData>()
            .With(x => x.AcceptedDecisionSummary_DataShareRequestId, testDataShareRequestId)
            .Create();

        testItems.MockSupplierDataShareRequestRepository.Setup(x => x.GetAcceptedDecisionSummaryAsync(testDataShareRequestId))
            .ReturnsAsync(() => testAcceptedDecisionSummaryModelData);
        #endregion

        #region Set Up Notification Information
        var testNotificationInformation = testItems.Fixture
            .Build<DataShareRequestNotificationInformationModelData>()
            .With(x => x.AcquirerUserId, testAcceptedDecisionSummaryModelData.AcceptedDecisionSummary_AcquirerUserId)
            .Create();

        testItems.MockSupplierDataShareRequestRepository.Setup(x => x.GetDataShareRequestNotificationInformationAsync(
                testDataShareRequestId))
            .ReturnsAsync(() => testNotificationInformation);

        var testAcquirerUserContactDetails = testItems.Fixture
            .Build<UserContactDetails>()
            .With(x => x.EmailNotification, true)
            .Create();

        var testAcquirerUserDetails = testItems.Fixture
            .Build<UserDetails>()
            .With(x => x.UserContactDetails, testAcquirerUserContactDetails)
            .Create();

        testItems.MockUserProfilePresenter.Setup(x => x.GetUserDetailsByUserIdAsync(testAcceptedDecisionSummaryModelData.AcceptedDecisionSummary_AcquirerUserId))
            .ReturnsAsync(() => testAcquirerUserDetails);
        #endregion

        #region Set Up Acquirer Information
        var testAcquirerOrganisationInformation = testItems.Fixture.Create<OrganisationInformation>();

        testItems.MockUserProfilePresenter.Setup(x => x.GetOrganisationDetailsByOrganisationIdAsync(
                testAcceptedDecisionSummaryModelData.AcceptedDecisionSummary_AcquirerOrganisationId))
            .ReturnsAsync(() => testAcquirerOrganisationInformation);
        
        testItems.MockUserProfilePresenter.Setup(x => x.GetUserDetailsByUserIdAsync(
                testAcceptedDecisionSummaryModelData.AcceptedDecisionSummary_AcquirerUserId))
            .ReturnsAsync(() => testAcquirerUserDetails);
        #endregion

        #region Set Up Response Data Building
        testItems.MockSupplierDataShareRequestModelDataFactory.Setup(x => x.CreateDataShareRequestAcceptanceResult(
                It.IsAny<AcceptedDecisionSummaryModelData>(),
                It.IsAny<bool?>()))
            .Returns((AcceptedDecisionSummaryModelData acceptedDecisionSummaryModelData, bool? notificationSentSuccess) =>
            {
                var testAcceptedDecisionSummary = testItems.Fixture
                    .Build<AcceptedDecisionSummary>()
                    .With(x => x.DataShareRequestId, acceptedDecisionSummaryModelData.AcceptedDecisionSummary_DataShareRequestId)
                    .With(x => x.DataShareRequestRequestId, acceptedDecisionSummaryModelData.AcceptedDecisionSummary_DataShareRequestRequestId)
                    .With(x => x.AcquirerUserEmailAddress, acceptedDecisionSummaryModelData.AcceptedDecisionSummary_AcquirerUserEmailAddress)
                    .With(x => x.AcquirerOrganisationName, acceptedDecisionSummaryModelData.AcceptedDecisionSummary_AcquirerOrganisationName)
                    .Create();

                var testNotificationSuccess = notificationSentSuccess switch
                {
                    true => NotificationSuccess.SentSuccessfully,
                    false => NotificationSuccess.FailedToSend,
                    null => NotificationSuccess.NotSent
                };

                return testItems.Fixture
                    .Build<DataShareRequestAcceptanceResult>()
                    .With(x => x.DataShareRequestId, acceptedDecisionSummaryModelData.AcceptedDecisionSummary_DataShareRequestId)
                    .With(x => x.AcceptedDecisionSummary, testAcceptedDecisionSummary)
                    .With(x => x.NotificationSuccess, testNotificationSuccess)
                    .Create();
            });
        #endregion

        TestSetUpSuccessfulDataResultHandling<DataShareRequestAcceptanceResult>(testItems);

        var result = await testItems.SupplierDataShareRequestService.AcceptSubmissionAsync(
            testDataShareRequestId, testSupplierFeedback);

        Assert.Multiple(() =>
        {
            var resultData = result.Data!;

            Assert.That(resultData.DataShareRequestId, Is.EqualTo(testDataShareRequestId));

            Assert.That(resultData.AcceptedDecisionSummary.DataShareRequestId, Is.EqualTo(testAcceptedDecisionSummaryModelData.AcceptedDecisionSummary_DataShareRequestId));
            Assert.That(resultData.AcceptedDecisionSummary.DataShareRequestRequestId, Is.EqualTo(testAcceptedDecisionSummaryModelData.AcceptedDecisionSummary_DataShareRequestRequestId));
            Assert.That(resultData.AcceptedDecisionSummary.AcquirerUserEmailAddress, Is.EqualTo(testAcceptedDecisionSummaryModelData.AcceptedDecisionSummary_AcquirerUserEmailAddress));
            Assert.That(resultData.AcceptedDecisionSummary.AcquirerOrganisationName, Is.EqualTo(testAcceptedDecisionSummaryModelData.AcceptedDecisionSummary_AcquirerOrganisationName));
        });
    }

    [Test]
    public async Task GivenSendingAnAcceptanceNotificationWillFail_WhenIAcceptSubmissionAsync_ThenAFailedNotificationSuccessIsReturned()
    {
        var testItems = CreateTestItems();

        var testDataShareRequestId = testItems.Fixture.Create<Guid>();
        var testSupplierFeedback = testItems.Fixture.Create<string>();

        testItems.MockSupplierDataShareRequestRepository.Setup(x => x.GetDataShareRequestStatusAsync(testDataShareRequestId))
            .ReturnsAsync(() => DataShareRequestStatusType.Submitted);

        #region Set Up Decision Summary
        var testAcceptedDecisionSummaryModelData = testItems.Fixture
            .Build<AcceptedDecisionSummaryModelData>()
            .With(x => x.AcceptedDecisionSummary_DataShareRequestId, testDataShareRequestId)
            .Create();

        testItems.MockSupplierDataShareRequestRepository.Setup(x => x.GetAcceptedDecisionSummaryAsync(testDataShareRequestId))
            .ReturnsAsync(() => testAcceptedDecisionSummaryModelData);
        #endregion

        #region Set Up Notification Information
        var testNotificationInformation = testItems.Fixture
            .Build<DataShareRequestNotificationInformationModelData>()
            .With(x => x.AcquirerUserId, testAcceptedDecisionSummaryModelData.AcceptedDecisionSummary_AcquirerUserId)
            .Create();

        testItems.MockSupplierDataShareRequestRepository.Setup(x => x.GetDataShareRequestNotificationInformationAsync(
                testDataShareRequestId))
            .ReturnsAsync(() => testNotificationInformation);

        var testAcquirerUserContactDetails = testItems.Fixture
            .Build<UserContactDetails>()
            .With(x => x.EmailNotification, true)
            .Create();

        var testAcquirerUserDetails = testItems.Fixture
            .Build<UserDetails>()
            .With(x => x.UserContactDetails, testAcquirerUserContactDetails)
            .Create();

        testItems.MockUserProfilePresenter.Setup(x => x.GetUserDetailsByUserIdAsync(testAcceptedDecisionSummaryModelData.AcceptedDecisionSummary_AcquirerUserId))
            .ReturnsAsync(() => testAcquirerUserDetails);
        #endregion

        #region Set Up Acquirer Information
        var testAcquirerOrganisationInformation = testItems.Fixture.Create<OrganisationInformation>();

        testItems.MockUserProfilePresenter.Setup(x => x.GetOrganisationDetailsByOrganisationIdAsync(
                testAcceptedDecisionSummaryModelData.AcceptedDecisionSummary_AcquirerOrganisationId))
            .ReturnsAsync(() => testAcquirerOrganisationInformation);

        testItems.MockUserProfilePresenter.Setup(x => x.GetUserDetailsByUserIdAsync(
                testAcceptedDecisionSummaryModelData.AcceptedDecisionSummary_AcquirerUserId))
            .ReturnsAsync(() => testAcquirerUserDetails);
        #endregion

        #region Set Up Response Data Building
        testItems.MockSupplierDataShareRequestModelDataFactory.Setup(x => x.CreateDataShareRequestAcceptanceResult(
                It.IsAny<AcceptedDecisionSummaryModelData>(),
                It.IsAny<bool?>()))
            .Returns((AcceptedDecisionSummaryModelData acceptedDecisionSummaryModelData, bool? notificationSentSuccess) =>
            {
                var testAcceptedDecisionSummary = testItems.Fixture
                    .Build<AcceptedDecisionSummary>()
                    .With(x => x.DataShareRequestId, acceptedDecisionSummaryModelData.AcceptedDecisionSummary_DataShareRequestId)
                    .With(x => x.DataShareRequestRequestId, acceptedDecisionSummaryModelData.AcceptedDecisionSummary_DataShareRequestRequestId)
                    .With(x => x.AcquirerUserEmailAddress, acceptedDecisionSummaryModelData.AcceptedDecisionSummary_AcquirerUserEmailAddress)
                    .With(x => x.AcquirerOrganisationName, acceptedDecisionSummaryModelData.AcceptedDecisionSummary_AcquirerOrganisationName)
                    .Create();

                var testNotificationSuccess = notificationSentSuccess switch
                {
                    true => NotificationSuccess.SentSuccessfully,
                    false => NotificationSuccess.FailedToSend,
                    null => NotificationSuccess.NotSent
                };

                return testItems.Fixture
                    .Build<DataShareRequestAcceptanceResult>()
                    .With(x => x.DataShareRequestId, acceptedDecisionSummaryModelData.AcceptedDecisionSummary_DataShareRequestId)
                    .With(x => x.AcceptedDecisionSummary, testAcceptedDecisionSummary)
                    .With(x => x.NotificationSuccess, testNotificationSuccess)
                    .Create();
            });
        #endregion

        TestSetUpSuccessfulDataResultHandling<DataShareRequestAcceptanceResult>(testItems);

        testItems.MockNotificationService.Setup(x => x.SendToAcquirerDataShareRequestAcceptedNotificationAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>()))
            .Throws(() => new Exception());

        var result = await testItems.SupplierDataShareRequestService.AcceptSubmissionAsync(
            testDataShareRequestId, testSupplierFeedback);

        Assert.Multiple(() =>
        {
            var resultData = result.Data!;

            Assert.That(resultData.DataShareRequestId, Is.EqualTo(testDataShareRequestId));

            Assert.That(resultData.NotificationSuccess, Is.EqualTo(NotificationSuccess.FailedToSend));
        });
    }

    [Test]
    public async Task GivenAcceptanceNotificationIsNotRequired_WhenIAcceptSubmissionAsync_ThenASuccessfulNotificationSuccessIsReturned()
    {
        var testItems = CreateTestItems();

        var testDataShareRequestId = testItems.Fixture.Create<Guid>();
        var testSupplierFeedback = testItems.Fixture.Create<string>();

        testItems.MockSupplierDataShareRequestRepository.Setup(x => x.GetDataShareRequestStatusAsync(testDataShareRequestId))
            .ReturnsAsync(() => DataShareRequestStatusType.Submitted);

        #region Set Up Decision Summary
        var testAcceptedDecisionSummaryModelData = testItems.Fixture
            .Build<AcceptedDecisionSummaryModelData>()
            .With(x => x.AcceptedDecisionSummary_DataShareRequestId, testDataShareRequestId)
            .Create();

        testItems.MockSupplierDataShareRequestRepository.Setup(x => x.GetAcceptedDecisionSummaryAsync(testDataShareRequestId))
            .ReturnsAsync(() => testAcceptedDecisionSummaryModelData);
        #endregion

        #region Set Up Notification Information
        var testNotificationInformation = testItems.Fixture
            .Build<DataShareRequestNotificationInformationModelData>()
            .With(x => x.AcquirerUserId, testAcceptedDecisionSummaryModelData.AcceptedDecisionSummary_AcquirerUserId)
            .Create();

        testItems.MockSupplierDataShareRequestRepository.Setup(x => x.GetDataShareRequestNotificationInformationAsync(
                testDataShareRequestId))
            .ReturnsAsync(() => testNotificationInformation);

        var testAcquirerUserContactDetails = testItems.Fixture
            .Build<UserContactDetails>()
            .With(x => x.EmailNotification, false)
            .Create();

        var testAcquirerUserDetails = testItems.Fixture
            .Build<UserDetails>()
            .With(x => x.UserContactDetails, testAcquirerUserContactDetails)
            .Create();

        testItems.MockUserProfilePresenter.Setup(x => x.GetUserDetailsByUserIdAsync(testAcceptedDecisionSummaryModelData.AcceptedDecisionSummary_AcquirerUserId))
            .ReturnsAsync(() => testAcquirerUserDetails);
        #endregion

        #region Set Up Acquirer Information
        var testAcquirerOrganisationInformation = testItems.Fixture.Create<OrganisationInformation>();

        testItems.MockUserProfilePresenter.Setup(x => x.GetOrganisationDetailsByOrganisationIdAsync(
                testAcceptedDecisionSummaryModelData.AcceptedDecisionSummary_AcquirerOrganisationId))
            .ReturnsAsync(() => testAcquirerOrganisationInformation);

        testItems.MockUserProfilePresenter.Setup(x => x.GetUserDetailsByUserIdAsync(
                testAcceptedDecisionSummaryModelData.AcceptedDecisionSummary_AcquirerUserId))
            .ReturnsAsync(() => testAcquirerUserDetails);
        #endregion

        #region Set Up Response Data Building
        testItems.MockSupplierDataShareRequestModelDataFactory.Setup(x => x.CreateDataShareRequestAcceptanceResult(
                It.IsAny<AcceptedDecisionSummaryModelData>(),
                It.IsAny<bool?>()))
            .Returns((AcceptedDecisionSummaryModelData acceptedDecisionSummaryModelData, bool? notificationSentSuccess) =>
            {
                var testAcceptedDecisionSummary = testItems.Fixture
                    .Build<AcceptedDecisionSummary>()
                    .With(x => x.DataShareRequestId, acceptedDecisionSummaryModelData.AcceptedDecisionSummary_DataShareRequestId)
                    .With(x => x.DataShareRequestRequestId, acceptedDecisionSummaryModelData.AcceptedDecisionSummary_DataShareRequestRequestId)
                    .With(x => x.AcquirerUserEmailAddress, acceptedDecisionSummaryModelData.AcceptedDecisionSummary_AcquirerUserEmailAddress)
                    .With(x => x.AcquirerOrganisationName, acceptedDecisionSummaryModelData.AcceptedDecisionSummary_AcquirerOrganisationName)
                    .Create();

                var testNotificationSuccess = notificationSentSuccess switch
                {
                    true => NotificationSuccess.SentSuccessfully,
                    false => NotificationSuccess.FailedToSend,
                    null => NotificationSuccess.NotSent
                };

                return testItems.Fixture
                    .Build<DataShareRequestAcceptanceResult>()
                    .With(x => x.DataShareRequestId, acceptedDecisionSummaryModelData.AcceptedDecisionSummary_DataShareRequestId)
                    .With(x => x.AcceptedDecisionSummary, testAcceptedDecisionSummary)
                    .With(x => x.NotificationSuccess, testNotificationSuccess)
                    .Create();
            });
        #endregion

        TestSetUpSuccessfulDataResultHandling<DataShareRequestAcceptanceResult>(testItems);

        testItems.MockNotificationService.Setup(x => x.SendToAcquirerDataShareRequestAcceptedNotificationAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>()))
            .Throws(() => new Exception());

        var result = await testItems.SupplierDataShareRequestService.AcceptSubmissionAsync(
            testDataShareRequestId, testSupplierFeedback);

        Assert.Multiple(() =>
        {
            var resultData = result.Data!;

            Assert.That(resultData.DataShareRequestId, Is.EqualTo(testDataShareRequestId));

            Assert.That(resultData.NotificationSuccess, Is.EqualTo(NotificationSuccess.SentSuccessfully));
        });
    }

    [Test]
    public async Task GivenAcceptingADataShareRequestWillFail_WhenIAcceptSubmissionAsync_ThenAFailureResultIsReturned()
    {
        var testItems = CreateTestItems();

        var testDataShareRequestId = testItems.Fixture.Create<Guid>();
        var testSupplierFeedback = testItems.Fixture.Create<string>();

        testItems.MockSupplierDataShareRequestRepository.Setup(x => x.GetDataShareRequestStatusAsync(testDataShareRequestId))
            .Throws(() => new Exception("test error message"));
       

        TestSetUpFailedDataResultHandling<DataShareRequestAcceptanceResult>(testItems);

        var result = await testItems.SupplierDataShareRequestService.AcceptSubmissionAsync(
            testDataShareRequestId, testSupplierFeedback);

        Assert.Multiple(() =>
        {
            Assert.That(result.Success, Is.False);
            Assert.That(result.Error, Is.EqualTo("test error message"));
        });
    }
    #endregion

    #region RejectSubmissionAsync() Tests
    [Test]
    public async Task GivenASubmittedDataShareRequest_WhenIRejectSubmissionAsync_ThenASummaryOfTheRejectionIsReturned()
    {
        var testItems = CreateTestItems();

        var testDataShareRequestId = testItems.Fixture.Create<Guid>();
        var testSupplierFeedback = testItems.Fixture.Create<string>();

        testItems.MockSupplierDataShareRequestRepository.Setup(x => x.GetDataShareRequestStatusAsync(testDataShareRequestId))
            .ReturnsAsync(() => DataShareRequestStatusType.Submitted);

        #region Set Up Decision Summary
        var testRejectedDecisionSummaryModelData = testItems.Fixture
            .Build<RejectedDecisionSummaryModelData>()
            .With(x => x.RejectedDecisionSummary_DataShareRequestId, testDataShareRequestId)
            .Create();

        testItems.MockSupplierDataShareRequestRepository.Setup(x => x.GetRejectedDecisionSummaryAsync(testDataShareRequestId))
            .ReturnsAsync(() => testRejectedDecisionSummaryModelData);
        #endregion

        #region Set Up Notification Information
        var testAcquirerUserId = testItems.Fixture.Create<int>();

        var testNotificationInformation = testItems.Fixture
            .Build<DataShareRequestNotificationInformationModelData>()
            .With(x => x.AcquirerUserId, testAcquirerUserId)
            .Create();

        testItems.MockSupplierDataShareRequestRepository.Setup(x => x.GetDataShareRequestNotificationInformationAsync(
                testDataShareRequestId))
            .ReturnsAsync(() => testNotificationInformation);

        var testAcquirerUserContactDetails = testItems.Fixture
            .Build<UserContactDetails>()
            .With(x => x.EmailNotification, true)
            .Create();

        var testAcquirerUserDetails = testItems.Fixture
            .Build<UserDetails>()
            .With(x => x.UserContactDetails, testAcquirerUserContactDetails)
            .Create();

        testItems.MockUserProfilePresenter.Setup(x => x.GetUserDetailsByUserIdAsync(testAcquirerUserId))
            .ReturnsAsync(() => testAcquirerUserDetails);
        #endregion

        #region Set Up Acquirer Information
        var testAcquirerOrganisationInformation = testItems.Fixture.Create<OrganisationInformation>();

        testItems.MockUserProfilePresenter.Setup(x => x.GetOrganisationDetailsByOrganisationIdAsync(
                testRejectedDecisionSummaryModelData.RejectedDecisionSummary_AcquirerOrganisationId))
            .ReturnsAsync(() => testAcquirerOrganisationInformation);

        testItems.MockUserProfilePresenter.Setup(x => x.GetUserDetailsByUserIdAsync(
                testAcquirerUserId))
            .ReturnsAsync(() => testAcquirerUserDetails);
        #endregion

        #region Set Up Response Data Building
        testItems.MockSupplierDataShareRequestModelDataFactory.Setup(x => x.CreateDataShareRequestRejectionResult(
                It.IsAny<RejectedDecisionSummaryModelData>(),
                It.IsAny<bool?>()))
            .Returns((RejectedDecisionSummaryModelData rejectedDecisionSummaryModelData, bool? notificationSentSuccess) =>
            {
                var testRejectedDecisionSummary = testItems.Fixture
                    .Build<RejectedDecisionSummary>()
                    .With(x => x.DataShareRequestId, rejectedDecisionSummaryModelData.RejectedDecisionSummary_DataShareRequestId)
                    .With(x => x.DataShareRequestRequestId, rejectedDecisionSummaryModelData.RejectedDecisionSummary_DataShareRequestRequestId)
                    .With(x => x.AcquirerOrganisationName, rejectedDecisionSummaryModelData.RejectedDecisionSummary_AcquirerOrganisationName)
                    .Create();

                var testNotificationSuccess = notificationSentSuccess switch
                {
                    true => NotificationSuccess.SentSuccessfully,
                    false => NotificationSuccess.FailedToSend,
                    null => NotificationSuccess.NotSent
                };

                return testItems.Fixture
                    .Build<DataShareRequestRejectionResult>()
                    .With(x => x.DataShareRequestId, rejectedDecisionSummaryModelData.RejectedDecisionSummary_DataShareRequestId)
                    .With(x => x.RejectedDecisionSummary, testRejectedDecisionSummary)
                    .With(x => x.NotificationSuccess, testNotificationSuccess)
                    .Create();
            });
        #endregion

        TestSetUpSuccessfulDataResultHandling<DataShareRequestRejectionResult>(testItems);

        var result = await testItems.SupplierDataShareRequestService.RejectSubmissionAsync(
            testDataShareRequestId, testSupplierFeedback);

        Assert.Multiple(() =>
        {
            var resultData = result.Data!;

            Assert.That(resultData.DataShareRequestId, Is.EqualTo(testDataShareRequestId));

            Assert.That(resultData.RejectedDecisionSummary.DataShareRequestId, Is.EqualTo(testRejectedDecisionSummaryModelData.RejectedDecisionSummary_DataShareRequestId));
            Assert.That(resultData.RejectedDecisionSummary.DataShareRequestRequestId, Is.EqualTo(testRejectedDecisionSummaryModelData.RejectedDecisionSummary_DataShareRequestRequestId));
            Assert.That(resultData.RejectedDecisionSummary.AcquirerOrganisationName, Is.EqualTo(testRejectedDecisionSummaryModelData.RejectedDecisionSummary_AcquirerOrganisationName));
        });
    }

    [Test]
    public async Task GivenSendingAnRejectionNotificationWillFail_WhenIRejectSubmissionAsync_ThenAFailedNotificationSuccessIsReturned()
    {
        var testItems = CreateTestItems();

        var testDataShareRequestId = testItems.Fixture.Create<Guid>();
        var testSupplierFeedback = testItems.Fixture.Create<string>();

        testItems.MockSupplierDataShareRequestRepository.Setup(x => x.GetDataShareRequestStatusAsync(testDataShareRequestId))
            .ReturnsAsync(() => DataShareRequestStatusType.Submitted);

        #region Set Up Decision Summary
        var testRejectedDecisionSummaryModelData = testItems.Fixture
            .Build<RejectedDecisionSummaryModelData>()
            .With(x => x.RejectedDecisionSummary_DataShareRequestId, testDataShareRequestId)
            .Create();

        testItems.MockSupplierDataShareRequestRepository.Setup(x => x.GetRejectedDecisionSummaryAsync(testDataShareRequestId))
            .ReturnsAsync(() => testRejectedDecisionSummaryModelData);
        #endregion

        #region Set Up Notification Information
        var testAcquirerUserId = testItems.Fixture.Create<int>();

        var testNotificationInformation = testItems.Fixture
            .Build<DataShareRequestNotificationInformationModelData>()
            .With(x => x.AcquirerUserId, testAcquirerUserId)
            .Create();

        testItems.MockSupplierDataShareRequestRepository.Setup(x => x.GetDataShareRequestNotificationInformationAsync(
                testDataShareRequestId))
            .ReturnsAsync(() => testNotificationInformation);

        var testAcquirerUserContactDetails = testItems.Fixture
            .Build<UserContactDetails>()
            .With(x => x.EmailNotification, true)
            .Create();

        var testAcquirerUserDetails = testItems.Fixture
            .Build<UserDetails>()
            .With(x => x.UserContactDetails, testAcquirerUserContactDetails)
            .Create();

        testItems.MockUserProfilePresenter.Setup(x => x.GetUserDetailsByUserIdAsync(testAcquirerUserId))
            .ReturnsAsync(() => testAcquirerUserDetails);
        #endregion

        #region Set Up Acquirer Information
        var testAcquirerOrganisationInformation = testItems.Fixture.Create<OrganisationInformation>();

        testItems.MockUserProfilePresenter.Setup(x => x.GetOrganisationDetailsByOrganisationIdAsync(
                testRejectedDecisionSummaryModelData.RejectedDecisionSummary_AcquirerOrganisationId))
            .ReturnsAsync(() => testAcquirerOrganisationInformation);

        testItems.MockUserProfilePresenter.Setup(x => x.GetUserDetailsByUserIdAsync(testAcquirerUserId))
            .ReturnsAsync(() => testAcquirerUserDetails);
        #endregion

        #region Set Up Response Data Building
        testItems.MockSupplierDataShareRequestModelDataFactory.Setup(x => x.CreateDataShareRequestRejectionResult(
                It.IsAny<RejectedDecisionSummaryModelData>(),
                It.IsAny<bool?>()))
            .Returns((RejectedDecisionSummaryModelData rejectedDecisionSummaryModelData, bool? notificationSentSuccess) =>
            {
                var testRejectedDecisionSummary = testItems.Fixture
                    .Build<RejectedDecisionSummary>()
                    .With(x => x.DataShareRequestId, rejectedDecisionSummaryModelData.RejectedDecisionSummary_DataShareRequestId)
                    .With(x => x.DataShareRequestRequestId, rejectedDecisionSummaryModelData.RejectedDecisionSummary_DataShareRequestRequestId)
                    .With(x => x.AcquirerOrganisationName, rejectedDecisionSummaryModelData.RejectedDecisionSummary_AcquirerOrganisationName)
                    .Create();

                var testNotificationSuccess = notificationSentSuccess switch
                {
                    true => NotificationSuccess.SentSuccessfully,
                    false => NotificationSuccess.FailedToSend,
                    null => NotificationSuccess.NotSent
                };

                return testItems.Fixture
                    .Build<DataShareRequestRejectionResult>()
                    .With(x => x.DataShareRequestId, rejectedDecisionSummaryModelData.RejectedDecisionSummary_DataShareRequestId)
                    .With(x => x.RejectedDecisionSummary, testRejectedDecisionSummary)
                    .With(x => x.NotificationSuccess, testNotificationSuccess)
                    .Create();
            });
        #endregion

        TestSetUpSuccessfulDataResultHandling<DataShareRequestRejectionResult>(testItems);

        testItems.MockNotificationService.Setup(x => x.SendToAcquirerDataShareRequestRejectedNotificationAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>()))
            .Throws(() => new Exception());

        var result = await testItems.SupplierDataShareRequestService.RejectSubmissionAsync(
            testDataShareRequestId, testSupplierFeedback);

        Assert.Multiple(() =>
        {
            var resultData = result.Data!;

            Assert.That(resultData.DataShareRequestId, Is.EqualTo(testDataShareRequestId));

            Assert.That(resultData.NotificationSuccess, Is.EqualTo(NotificationSuccess.FailedToSend));
        });
    }

    [Test]
    public async Task GivenRejectionNotificationIsNotRequired_WhenIRejectSubmissionAsync_ThenASuccessfulNotificationSuccessIsReturned()
    {
        var testItems = CreateTestItems();

        var testDataShareRequestId = testItems.Fixture.Create<Guid>();
        var testSupplierFeedback = testItems.Fixture.Create<string>();

        testItems.MockSupplierDataShareRequestRepository.Setup(x => x.GetDataShareRequestStatusAsync(testDataShareRequestId))
            .ReturnsAsync(() => DataShareRequestStatusType.Submitted);

        #region Set Up Decision Summary
        var testRejectedDecisionSummaryModelData = testItems.Fixture
            .Build<RejectedDecisionSummaryModelData>()
            .With(x => x.RejectedDecisionSummary_DataShareRequestId, testDataShareRequestId)
            .Create();

        testItems.MockSupplierDataShareRequestRepository.Setup(x => x.GetRejectedDecisionSummaryAsync(testDataShareRequestId))
            .ReturnsAsync(() => testRejectedDecisionSummaryModelData);
        #endregion

        #region Set Up Notification Information
        var testAcquirerUserId = testItems.Fixture.Create<int>();

        var testNotificationInformation = testItems.Fixture
            .Build<DataShareRequestNotificationInformationModelData>()
            .With(x => x.AcquirerUserId, testAcquirerUserId)
            .Create();

        testItems.MockSupplierDataShareRequestRepository.Setup(x => x.GetDataShareRequestNotificationInformationAsync(
                testDataShareRequestId))
            .ReturnsAsync(() => testNotificationInformation);

        var testAcquirerUserContactDetails = testItems.Fixture
            .Build<UserContactDetails>()
            .With(x => x.EmailNotification, false)
            .Create();

        var testAcquirerUserDetails = testItems.Fixture
            .Build<UserDetails>()
            .With(x => x.UserContactDetails, testAcquirerUserContactDetails)
            .Create();

        testItems.MockUserProfilePresenter.Setup(x => x.GetUserDetailsByUserIdAsync(testAcquirerUserId))
            .ReturnsAsync(() => testAcquirerUserDetails);
        #endregion

        #region Set Up Acquirer Information
        var testAcquirerOrganisationInformation = testItems.Fixture.Create<OrganisationInformation>();

        testItems.MockUserProfilePresenter.Setup(x => x.GetOrganisationDetailsByOrganisationIdAsync(
                testRejectedDecisionSummaryModelData.RejectedDecisionSummary_AcquirerOrganisationId))
            .ReturnsAsync(() => testAcquirerOrganisationInformation);

        testItems.MockUserProfilePresenter.Setup(x => x.GetUserDetailsByUserIdAsync(testAcquirerUserId))
            .ReturnsAsync(() => testAcquirerUserDetails);
        #endregion

        #region Set Up Response Data Building
        testItems.MockSupplierDataShareRequestModelDataFactory.Setup(x => x.CreateDataShareRequestRejectionResult(
                It.IsAny<RejectedDecisionSummaryModelData>(),
                It.IsAny<bool?>()))
            .Returns((RejectedDecisionSummaryModelData rejectedDecisionSummaryModelData, bool? notificationSentSuccess) =>
            {
                var testRejectedDecisionSummary = testItems.Fixture
                    .Build<RejectedDecisionSummary>()
                    .With(x => x.DataShareRequestId, rejectedDecisionSummaryModelData.RejectedDecisionSummary_DataShareRequestId)
                    .With(x => x.DataShareRequestRequestId, rejectedDecisionSummaryModelData.RejectedDecisionSummary_DataShareRequestRequestId)
                    .With(x => x.AcquirerOrganisationName, rejectedDecisionSummaryModelData.RejectedDecisionSummary_AcquirerOrganisationName)
                    .Create();

                var testNotificationSuccess = notificationSentSuccess switch
                {
                    true => NotificationSuccess.SentSuccessfully,
                    false => NotificationSuccess.FailedToSend,
                    null => NotificationSuccess.NotSent
                };

                return testItems.Fixture
                    .Build<DataShareRequestRejectionResult>()
                    .With(x => x.DataShareRequestId, rejectedDecisionSummaryModelData.RejectedDecisionSummary_DataShareRequestId)
                    .With(x => x.RejectedDecisionSummary, testRejectedDecisionSummary)
                    .With(x => x.NotificationSuccess, testNotificationSuccess)
                    .Create();
            });
        #endregion

        TestSetUpSuccessfulDataResultHandling<DataShareRequestRejectionResult>(testItems);

        testItems.MockNotificationService.Setup(x => x.SendToAcquirerDataShareRequestRejectedNotificationAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>()))
            .Throws(() => new Exception());

        var result = await testItems.SupplierDataShareRequestService.RejectSubmissionAsync(
            testDataShareRequestId, testSupplierFeedback);

        Assert.Multiple(() =>
        {
            var resultData = result.Data!;

            Assert.That(resultData.DataShareRequestId, Is.EqualTo(testDataShareRequestId));

            Assert.That(resultData.NotificationSuccess, Is.EqualTo(NotificationSuccess.SentSuccessfully));
        });
    }

    [Test]
    public async Task GivenRejectingADataShareRequestWillFail_WhenIRejectSubmissionAsync_ThenAFailureResultIsReturned()
    {
        var testItems = CreateTestItems();

        var testDataShareRequestId = testItems.Fixture.Create<Guid>();
        var testSupplierFeedback = testItems.Fixture.Create<string>();

        testItems.MockSupplierDataShareRequestRepository.Setup(x => x.GetDataShareRequestStatusAsync(testDataShareRequestId))
            .Throws(() => new Exception("test error message"));


        TestSetUpFailedDataResultHandling<DataShareRequestRejectionResult>(testItems);

        var result = await testItems.SupplierDataShareRequestService.RejectSubmissionAsync(
            testDataShareRequestId, testSupplierFeedback);

        Assert.Multiple(() =>
        {
            Assert.That(result.Success, Is.False);
            Assert.That(result.Error, Is.EqualTo("test error message"));
        });
    }
    #endregion

    #region ReturnSubmissionAsync() Tests
    [Test]
    public async Task GivenASubmittedDataShareRequest_WhenIReturnSubmissionAsync_ThenASummaryOfTheReturnIsReturned()
    {
        var testItems = CreateTestItems();

        var testDataShareRequestId = testItems.Fixture.Create<Guid>();
        var testSupplierFeedback = testItems.Fixture.Create<string>();

        testItems.MockSupplierDataShareRequestRepository.Setup(x => x.GetDataShareRequestStatusAsync(testDataShareRequestId))
            .ReturnsAsync(() => DataShareRequestStatusType.Submitted);

        #region Set Up Decision Summary
        var testReturnedDecisionSummaryModelData = testItems.Fixture
            .Build<ReturnedDecisionSummaryModelData>()
            .With(x => x.ReturnedDecisionSummary_DataShareRequestId, testDataShareRequestId)
            .Create();

        testItems.MockSupplierDataShareRequestRepository.Setup(x => x.GetReturnedDecisionSummaryAsync(testDataShareRequestId))
            .ReturnsAsync(() => testReturnedDecisionSummaryModelData);
        #endregion

        #region Set Up Notification Information
        var testAcquirerUserId = testItems.Fixture.Create<int>();

        var testNotificationInformation = testItems.Fixture
            .Build<DataShareRequestNotificationInformationModelData>()
            .With(x => x.AcquirerUserId, testAcquirerUserId)
            .Create();

        testItems.MockSupplierDataShareRequestRepository.Setup(x => x.GetDataShareRequestNotificationInformationAsync(
                testDataShareRequestId))
            .ReturnsAsync(() => testNotificationInformation);

        var testAcquirerUserContactDetails = testItems.Fixture
            .Build<UserContactDetails>()
            .With(x => x.EmailNotification, true)
            .Create();

        var testAcquirerUserDetails = testItems.Fixture
            .Build<UserDetails>()
            .With(x => x.UserContactDetails, testAcquirerUserContactDetails)
            .Create();

        testItems.MockUserProfilePresenter.Setup(x => x.GetUserDetailsByUserIdAsync(testAcquirerUserId))
            .ReturnsAsync(() => testAcquirerUserDetails);
        #endregion

        #region Set Up Acquirer Information
        var testAcquirerOrganisationInformation = testItems.Fixture.Create<OrganisationInformation>();

        testItems.MockUserProfilePresenter.Setup(x => x.GetOrganisationDetailsByOrganisationIdAsync(
                testReturnedDecisionSummaryModelData.ReturnedDecisionSummary_AcquirerOrganisationId))
            .ReturnsAsync(() => testAcquirerOrganisationInformation);

        testItems.MockUserProfilePresenter.Setup(x => x.GetUserDetailsByUserIdAsync(
                testAcquirerUserId))
            .ReturnsAsync(() => testAcquirerUserDetails);
        #endregion

        #region Set Up Response Data Building
        testItems.MockSupplierDataShareRequestModelDataFactory.Setup(x => x.CreateDataShareRequestReturnResult(
                It.IsAny<ReturnedDecisionSummaryModelData>(),
                It.IsAny<bool?>()))
            .Returns((ReturnedDecisionSummaryModelData returnedDecisionSummaryModelData, bool? notificationSentSuccess) =>
            {
                var testReturnedDecisionSummary = testItems.Fixture
                    .Build<ReturnedDecisionSummary>()
                    .With(x => x.DataShareRequestId, returnedDecisionSummaryModelData.ReturnedDecisionSummary_DataShareRequestId)
                    .With(x => x.DataShareRequestRequestId, returnedDecisionSummaryModelData.ReturnedDecisionSummary_DataShareRequestRequestId)
                    .With(x => x.AcquirerOrganisationName, returnedDecisionSummaryModelData.ReturnedDecisionSummary_AcquirerOrganisationName)
                    .Create();

                var testNotificationSuccess = notificationSentSuccess switch
                {
                    true => NotificationSuccess.SentSuccessfully,
                    false => NotificationSuccess.FailedToSend,
                    null => NotificationSuccess.NotSent
                };

                return testItems.Fixture
                    .Build<DataShareRequestReturnResult>()
                    .With(x => x.DataShareRequestId, returnedDecisionSummaryModelData.ReturnedDecisionSummary_DataShareRequestId)
                    .With(x => x.ReturnedDecisionSummary, testReturnedDecisionSummary)
                    .With(x => x.NotificationSuccess, testNotificationSuccess)
                    .Create();
            });
        #endregion

        TestSetUpSuccessfulDataResultHandling<DataShareRequestReturnResult>(testItems);

        var result = await testItems.SupplierDataShareRequestService.ReturnSubmissionAsync(
            testDataShareRequestId, testSupplierFeedback);

        Assert.Multiple(() =>
        {
            var resultData = result.Data!;

            Assert.That(resultData.DataShareRequestId, Is.EqualTo(testDataShareRequestId));

            Assert.That(resultData.ReturnedDecisionSummary.DataShareRequestId, Is.EqualTo(testReturnedDecisionSummaryModelData.ReturnedDecisionSummary_DataShareRequestId));
            Assert.That(resultData.ReturnedDecisionSummary.DataShareRequestRequestId, Is.EqualTo(testReturnedDecisionSummaryModelData.ReturnedDecisionSummary_DataShareRequestRequestId));
            Assert.That(resultData.ReturnedDecisionSummary.AcquirerOrganisationName, Is.EqualTo(testReturnedDecisionSummaryModelData.ReturnedDecisionSummary_AcquirerOrganisationName));
        });
    }

    [Test]
    public async Task GivenSendingAnReturnNotificationWillFail_WhenIReturnSubmissionAsync_ThenAFailedNotificationSuccessIsReturned()
    {
        var testItems = CreateTestItems();

        var testDataShareRequestId = testItems.Fixture.Create<Guid>();
        var testSupplierFeedback = testItems.Fixture.Create<string>();

        testItems.MockSupplierDataShareRequestRepository.Setup(x => x.GetDataShareRequestStatusAsync(testDataShareRequestId))
            .ReturnsAsync(() => DataShareRequestStatusType.Submitted);

        #region Set Up Decision Summary
        var testReturnedDecisionSummaryModelData = testItems.Fixture
            .Build<ReturnedDecisionSummaryModelData>()
            .With(x => x.ReturnedDecisionSummary_DataShareRequestId, testDataShareRequestId)
            .Create();

        testItems.MockSupplierDataShareRequestRepository.Setup(x => x.GetReturnedDecisionSummaryAsync(testDataShareRequestId))
            .ReturnsAsync(() => testReturnedDecisionSummaryModelData);
        #endregion

        #region Set Up Notification Information
        var testAcquirerUserId = testItems.Fixture.Create<int>();

        var testNotificationInformation = testItems.Fixture
            .Build<DataShareRequestNotificationInformationModelData>()
            .With(x => x.AcquirerUserId, testAcquirerUserId)
            .Create();

        testItems.MockSupplierDataShareRequestRepository.Setup(x => x.GetDataShareRequestNotificationInformationAsync(
                testDataShareRequestId))
            .ReturnsAsync(() => testNotificationInformation);

        var testAcquirerUserContactDetails = testItems.Fixture
            .Build<UserContactDetails>()
            .With(x => x.EmailNotification, true)
            .Create();

        var testAcquirerUserDetails = testItems.Fixture
            .Build<UserDetails>()
            .With(x => x.UserContactDetails, testAcquirerUserContactDetails)
            .Create();

        testItems.MockUserProfilePresenter.Setup(x => x.GetUserDetailsByUserIdAsync(testAcquirerUserId))
            .ReturnsAsync(() => testAcquirerUserDetails);
        #endregion

        #region Set Up Acquirer Information
        var testAcquirerOrganisationInformation = testItems.Fixture.Create<OrganisationInformation>();

        testItems.MockUserProfilePresenter.Setup(x => x.GetOrganisationDetailsByOrganisationIdAsync(
                testReturnedDecisionSummaryModelData.ReturnedDecisionSummary_AcquirerOrganisationId))
            .ReturnsAsync(() => testAcquirerOrganisationInformation);

        testItems.MockUserProfilePresenter.Setup(x => x.GetUserDetailsByUserIdAsync(testAcquirerUserId))
            .ReturnsAsync(() => testAcquirerUserDetails);
        #endregion

        #region Set Up Response Data Building
        testItems.MockSupplierDataShareRequestModelDataFactory.Setup(x => x.CreateDataShareRequestReturnResult(
                It.IsAny<ReturnedDecisionSummaryModelData>(),
                It.IsAny<bool?>()))
            .Returns((ReturnedDecisionSummaryModelData returnedDecisionSummaryModelData, bool? notificationSentSuccess) =>
            {
                var testReturnedDecisionSummary = testItems.Fixture
                    .Build<ReturnedDecisionSummary>()
                    .With(x => x.DataShareRequestId, returnedDecisionSummaryModelData.ReturnedDecisionSummary_DataShareRequestId)
                    .With(x => x.DataShareRequestRequestId, returnedDecisionSummaryModelData.ReturnedDecisionSummary_DataShareRequestRequestId)
                    .With(x => x.AcquirerOrganisationName, returnedDecisionSummaryModelData.ReturnedDecisionSummary_AcquirerOrganisationName)
                    .Create();

                var testNotificationSuccess = notificationSentSuccess switch
                {
                    true => NotificationSuccess.SentSuccessfully,
                    false => NotificationSuccess.FailedToSend,
                    null => NotificationSuccess.NotSent
                };

                return testItems.Fixture
                    .Build<DataShareRequestReturnResult>()
                    .With(x => x.DataShareRequestId, returnedDecisionSummaryModelData.ReturnedDecisionSummary_DataShareRequestId)
                    .With(x => x.ReturnedDecisionSummary, testReturnedDecisionSummary)
                    .With(x => x.NotificationSuccess, testNotificationSuccess)
                    .Create();
            });
        #endregion

        TestSetUpSuccessfulDataResultHandling<DataShareRequestReturnResult>(testItems);

        testItems.MockNotificationService.Setup(x => x.SendToAcquirerDataShareRequestReturnedWithCommentsNotificationAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>()))
            .Throws(() => new Exception());

        var result = await testItems.SupplierDataShareRequestService.ReturnSubmissionAsync(
            testDataShareRequestId, testSupplierFeedback);

        Assert.Multiple(() =>
        {
            var resultData = result.Data!;

            Assert.That(resultData.DataShareRequestId, Is.EqualTo(testDataShareRequestId));

            Assert.That(resultData.NotificationSuccess, Is.EqualTo(NotificationSuccess.FailedToSend));
        });
    }

    [Test]
    public async Task GivenReturnNotificationIsNotRequired_WhenIReturnSubmissionAsync_ThenASuccessfulNotificationSuccessIsReturned()
    {
        var testItems = CreateTestItems();

        var testDataShareRequestId = testItems.Fixture.Create<Guid>();
        var testSupplierFeedback = testItems.Fixture.Create<string>();

        testItems.MockSupplierDataShareRequestRepository.Setup(x => x.GetDataShareRequestStatusAsync(testDataShareRequestId))
            .ReturnsAsync(() => DataShareRequestStatusType.Submitted);

        #region Set Up Decision Summary
        var testReturnedDecisionSummaryModelData = testItems.Fixture
            .Build<ReturnedDecisionSummaryModelData>()
            .With(x => x.ReturnedDecisionSummary_DataShareRequestId, testDataShareRequestId)
            .Create();

        testItems.MockSupplierDataShareRequestRepository.Setup(x => x.GetReturnedDecisionSummaryAsync(testDataShareRequestId))
            .ReturnsAsync(() => testReturnedDecisionSummaryModelData);
        #endregion

        #region Set Up Notification Information
        var testAcquirerUserId = testItems.Fixture.Create<int>();

        var testNotificationInformation = testItems.Fixture
            .Build<DataShareRequestNotificationInformationModelData>()
            .With(x => x.AcquirerUserId, testAcquirerUserId)
            .Create();

        testItems.MockSupplierDataShareRequestRepository.Setup(x => x.GetDataShareRequestNotificationInformationAsync(
                testDataShareRequestId))
            .ReturnsAsync(() => testNotificationInformation);

        var testAcquirerUserContactDetails = testItems.Fixture
            .Build<UserContactDetails>()
            .With(x => x.EmailNotification, false)
            .Create();

        var testAcquirerUserDetails = testItems.Fixture
            .Build<UserDetails>()
            .With(x => x.UserContactDetails, testAcquirerUserContactDetails)
            .Create();

        testItems.MockUserProfilePresenter.Setup(x => x.GetUserDetailsByUserIdAsync(testAcquirerUserId))
            .ReturnsAsync(() => testAcquirerUserDetails);
        #endregion

        #region Set Up Acquirer Information
        var testAcquirerOrganisationInformation = testItems.Fixture.Create<OrganisationInformation>();

        testItems.MockUserProfilePresenter.Setup(x => x.GetOrganisationDetailsByOrganisationIdAsync(
                testReturnedDecisionSummaryModelData.ReturnedDecisionSummary_AcquirerOrganisationId))
            .ReturnsAsync(() => testAcquirerOrganisationInformation);

        testItems.MockUserProfilePresenter.Setup(x => x.GetUserDetailsByUserIdAsync(testAcquirerUserId))
            .ReturnsAsync(() => testAcquirerUserDetails);
        #endregion

        #region Set Up Response Data Building
        testItems.MockSupplierDataShareRequestModelDataFactory.Setup(x => x.CreateDataShareRequestReturnResult(
                It.IsAny<ReturnedDecisionSummaryModelData>(),
                It.IsAny<bool?>()))
            .Returns((ReturnedDecisionSummaryModelData returnedDecisionSummaryModelData, bool? notificationSentSuccess) =>
            {
                var testReturnedDecisionSummary = testItems.Fixture
                    .Build<ReturnedDecisionSummary>()
                    .With(x => x.DataShareRequestId, returnedDecisionSummaryModelData.ReturnedDecisionSummary_DataShareRequestId)
                    .With(x => x.DataShareRequestRequestId, returnedDecisionSummaryModelData.ReturnedDecisionSummary_DataShareRequestRequestId)
                    .With(x => x.AcquirerOrganisationName, returnedDecisionSummaryModelData.ReturnedDecisionSummary_AcquirerOrganisationName)
                    .Create();

                var testNotificationSuccess = notificationSentSuccess switch
                {
                    true => NotificationSuccess.SentSuccessfully,
                    false => NotificationSuccess.FailedToSend,
                    null => NotificationSuccess.NotSent
                };

                return testItems.Fixture
                    .Build<DataShareRequestReturnResult>()
                    .With(x => x.DataShareRequestId, returnedDecisionSummaryModelData.ReturnedDecisionSummary_DataShareRequestId)
                    .With(x => x.ReturnedDecisionSummary, testReturnedDecisionSummary)
                    .With(x => x.NotificationSuccess, testNotificationSuccess)
                    .Create();
            });
        #endregion

        TestSetUpSuccessfulDataResultHandling<DataShareRequestReturnResult>(testItems);

        testItems.MockNotificationService.Setup(x => x.SendToAcquirerDataShareRequestReturnedWithCommentsNotificationAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>()))
            .Throws(() => new Exception());

        var result = await testItems.SupplierDataShareRequestService.ReturnSubmissionAsync(
            testDataShareRequestId, testSupplierFeedback);

        Assert.Multiple(() =>
        {
            var resultData = result.Data!;

            Assert.That(resultData.DataShareRequestId, Is.EqualTo(testDataShareRequestId));

            Assert.That(resultData.NotificationSuccess, Is.EqualTo(NotificationSuccess.SentSuccessfully));
        });
    }

    [Test]
    public async Task GivenReturningADataShareRequestWillFail_WhenIReturnSubmissionAsync_ThenAFailureResultIsReturned()
    {
        var testItems = CreateTestItems();

        var testDataShareRequestId = testItems.Fixture.Create<Guid>();
        var testSupplierFeedback = testItems.Fixture.Create<string>();

        testItems.MockSupplierDataShareRequestRepository.Setup(x => x.GetDataShareRequestStatusAsync(testDataShareRequestId))
            .Throws(() => new Exception("test error message"));


        TestSetUpFailedDataResultHandling<DataShareRequestReturnResult>(testItems);

        var result = await testItems.SupplierDataShareRequestService.ReturnSubmissionAsync(
            testDataShareRequestId, testSupplierFeedback);

        Assert.Multiple(() =>
        {
            Assert.That(result.Success, Is.False);
            Assert.That(result.Error, Is.EqualTo("test error message"));
        });
    }
    #endregion

    #region Helpers
    private static void TestSetUpSuccessfulDataResultHandling<T>(TestItems testItems) where T : class
    {
        testItems.MockServiceOperationResultFactory.Setup(x => x.CreateSuccessfulDataResult(
                It.IsAny<T>(),
                It.IsAny<HttpStatusCode?>()))
            .Returns((T data, HttpStatusCode? _) =>
            {
                var mockResult = new Mock<IServiceOperationDataResult<T>>();

                mockResult.SetupGet(x => x.Success).Returns(true);
                mockResult.SetupGet(x => x.Data).Returns(data);
                mockResult.SetupGet(x => x.Error).Returns((string?)null);

                return mockResult.Object;
            });
    }

    private static void TestSetUpFailedDataResultHandling<T>(TestItems testItems) where T : class
    {
        testItems.MockServiceOperationResultFactory.Setup(x => x.CreateFailedDataResult<T>(
                It.IsAny<string>(),
                It.IsAny<HttpStatusCode?>()))
            .Returns((string error, HttpStatusCode? _) =>
            {
                var mockResult = new Mock<IServiceOperationDataResult<T>>();

                mockResult.SetupGet(x => x.Success).Returns(false);
                mockResult.SetupGet(x => x.Data).Returns((T?)null);
                mockResult.SetupGet(x => x.Error).Returns(error);

                return mockResult.Object;
            });
    }

    private static void TestSetUpSuccessfulResultHandling(TestItems testItems)
    {
        testItems.MockServiceOperationResultFactory.Setup(x => x.CreateSuccessfulResult(
                It.IsAny<HttpStatusCode?>()))
            .Returns((HttpStatusCode? _) =>
            {
                var mockResult = new Mock<IServiceOperationResult>();

                mockResult.SetupGet(x => x.Success).Returns(true);
                mockResult.SetupGet(x => x.Error).Returns((string?)null);

                return mockResult.Object;
            });
    }

    private static void TestSetUpFailedResultHandling(TestItems testItems)
    {
        testItems.MockServiceOperationResultFactory.Setup(x => x.CreateFailedResult(
                It.IsAny<string>(),
                It.IsAny<HttpStatusCode?>()))
            .Returns((string error, HttpStatusCode? _) =>
            {
                var mockResult = new Mock<IServiceOperationResult>();

                mockResult.SetupGet(x => x.Success).Returns(false);
                mockResult.SetupGet(x => x.Error).Returns(error);

                return mockResult.Object;
            });
    }
    #endregion

    #region Test Item Creation
    private static TestItems CreateTestItems()
    {
        var fixture = new Fixture().Customize(new AutoMoqCustomization());

        var mockLogger = Mock.Get(fixture.Freeze<ILogger<SupplierDataShareRequestService>>());
        var mockUserProfilePresenter = Mock.Get(fixture.Freeze<IUserProfilePresenter>());
        var mockSupplierDataShareRequestRepository = Mock.Get(fixture.Freeze<ISupplierDataShareRequestRepository>());
        var mockKeyQuestionPartAnswerProviderService = Mock.Get(fixture.Freeze<IKeyQuestionPartAnswerProviderService>());
        var mockAnswerHighlightsProviderService = Mock.Get(fixture.Freeze<IAnswerHighlightsService>());
        var mockQuestionSetPlaceholderReplacementService = Mock.Get(fixture.Freeze<IQuestionSetPlaceholderReplacementService>());
        var mockEsdaInformationPresenter = Mock.Get(fixture.Freeze<IEsdaInformationPresenter>());
        var mockAuditLogService = Mock.Get(fixture.Freeze<IAuditLogService>());
        var mockNotificationService = Mock.Get(fixture.Freeze<INotificationService>());
        var mockSubmissionContentPdfFileBuilder = Mock.Get(fixture.Freeze<ISubmissionContentPdfFileBuilder>());
        var mockSupplierDataShareRequestModelDataFactory = Mock.Get(fixture.Freeze<ISupplierDataShareRequestModelDataFactory>());
        var mockServiceOperationResultFactory = Mock.Get(fixture.Freeze<IServiceOperationResultFactory>());

        var supplierDataShareRequestService = new SupplierDataShareRequestService(
            mockLogger.Object,
            mockUserProfilePresenter.Object,
            mockSupplierDataShareRequestRepository.Object,
            mockKeyQuestionPartAnswerProviderService.Object,
            mockAnswerHighlightsProviderService.Object,
            mockQuestionSetPlaceholderReplacementService.Object,
            mockEsdaInformationPresenter.Object,
            mockAuditLogService.Object,
            mockNotificationService.Object,
            mockSubmissionContentPdfFileBuilder.Object,
            mockSupplierDataShareRequestModelDataFactory.Object,
            mockServiceOperationResultFactory.Object);

        return new TestItems(
            fixture,
            supplierDataShareRequestService,
            mockUserProfilePresenter,
            mockSupplierDataShareRequestRepository,
            mockKeyQuestionPartAnswerProviderService,
            mockAnswerHighlightsProviderService,
            mockAuditLogService,
            mockNotificationService,
            mockSubmissionContentPdfFileBuilder,
            mockSupplierDataShareRequestModelDataFactory,
            mockServiceOperationResultFactory);
        }

    private class TestItems(
        IFixture fixture,
        ISupplierDataShareRequestService supplierDataShareRequestService,
        Mock<IUserProfilePresenter> mockUserProfilePresenter,
        Mock<ISupplierDataShareRequestRepository> mockSupplierDataShareRequestRepository,
        Mock<IKeyQuestionPartAnswerProviderService> mockKeyQuestionPartAnswerProviderService,
        Mock<IAnswerHighlightsService> mockAnswerHighlightsProviderService,
        Mock<IAuditLogService> mockAuditLogService,
        Mock<INotificationService> mockNotificationService,
        Mock<ISubmissionContentPdfFileBuilder> mockSubmissionContentPdfFileBuilder,
        Mock<ISupplierDataShareRequestModelDataFactory> mockSupplierDataShareRequestModelDataFactory,
        Mock<IServiceOperationResultFactory> mockServiceOperationResultFactory)
    {
        public IFixture Fixture { get; } = fixture;
        public ISupplierDataShareRequestService SupplierDataShareRequestService { get; } = supplierDataShareRequestService;
        public Mock<IUserProfilePresenter> MockUserProfilePresenter { get; } = mockUserProfilePresenter;
        public Mock<ISupplierDataShareRequestRepository> MockSupplierDataShareRequestRepository { get; } = mockSupplierDataShareRequestRepository;
        public Mock<IKeyQuestionPartAnswerProviderService> MockKeyQuestionPartAnswerProviderService { get; } = mockKeyQuestionPartAnswerProviderService;
        public Mock<IAnswerHighlightsService> MockAnswerHighlightsProviderService { get; } = mockAnswerHighlightsProviderService;
        public Mock<IAuditLogService> MockAuditLogService { get; } = mockAuditLogService;
        public Mock<INotificationService> MockNotificationService { get; } = mockNotificationService;
        public Mock<ISubmissionContentPdfFileBuilder> MockSubmissionContentPdfFileBuilder { get; } = mockSubmissionContentPdfFileBuilder;
        public Mock<ISupplierDataShareRequestModelDataFactory> MockSupplierDataShareRequestModelDataFactory { get; } = mockSupplierDataShareRequestModelDataFactory;
        public Mock<IServiceOperationResultFactory> MockServiceOperationResultFactory { get; } = mockServiceOperationResultFactory;
    }
    #endregion
}
