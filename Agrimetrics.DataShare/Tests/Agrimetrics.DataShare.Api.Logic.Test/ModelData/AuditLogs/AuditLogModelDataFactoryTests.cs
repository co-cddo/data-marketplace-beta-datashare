using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests;
using Agrimetrics.DataShare.Api.Logic.ModelData.AuditLogs;
using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests;
using AutoFixture;
using AutoFixture.AutoMoq;
using NUnit.Framework;
using System.ComponentModel;
using Moq;

namespace Agrimetrics.DataShare.Api.Logic.Test.ModelData.AuditLogs;

[TestFixture]
public class AuditLogModelDataFactoryTests
{
    #region ConvertAuditLogDataShareRequestStatusChangeModelDatasToAuditLogDataShareRequestStatusChangesResult() Tests
    [Test]
    public void GivenADataShareRequestId_WhenIConvertAuditLogDataShareRequestStatusChangeModelDatasToAuditLogDataShareRequestStatusChangesResult_ThenADataShareRequestAuditLogIsReturnedWithTheGivenDataShareRequestId()
    {
        var testItems = CreateTestItems();

        var testDataShareRequestId = new Guid("0E167A14-0E73-4B6A-B4C3-1D7E0A7CA617");

        var testResult = testItems.AuditLogModelDataFactory.ConvertAuditLogDataShareRequestStatusChangeModelDatasToAuditLogDataShareRequestStatusChangesResult(
            testDataShareRequestId,
            []).DataShareRequestAuditLog;

        var result = testResult.DataShareRequestId;

        Assert.That(result, Is.EqualTo(testDataShareRequestId));
    }

    [Test]
    public void GivenAnEmptySetOfAuditLogDataShareRequestStatusChangeModelDatas_WhenIConvertAuditLogDataShareRequestStatusChangeModelDatasToAuditLogDataShareRequestStatusChangesResult_ThenADataShareRequestAuditLogIsReturnedWithAnEmptySetOfAuditLogEntries()
    {
        var testItems = CreateTestItems();

        var testResult = testItems.AuditLogModelDataFactory.ConvertAuditLogDataShareRequestStatusChangeModelDatasToAuditLogDataShareRequestStatusChangesResult(
            It.IsAny<Guid>(),
            []).DataShareRequestAuditLog;

        var result = testResult.AuditLogEntries;

        Assert.That(result, Is.Empty);
    }

    [Test]
    public void GivenASetOfAuditLogDataShareRequestStatusChangeModelDatas_WhenIConvertAuditLogDataShareRequestStatusChangeModelDatasToAuditLogDataShareRequestStatusChangesResult_ThenADataShareRequestAuditLogIsReturnedWithAnAuditLogEntryForEachStatusChange()
    {

        var testItems = CreateTestItems();

        var testAuditLogsForDataShareRequestStatusChangesModelDatas =
            testItems.Fixture.CreateMany<AuditLogDataShareRequestStatusChangeModelData>().ToList();

        var testResult = testItems.AuditLogModelDataFactory.ConvertAuditLogDataShareRequestStatusChangeModelDatasToAuditLogDataShareRequestStatusChangesResult(
            It.IsAny<Guid>(),
            testAuditLogsForDataShareRequestStatusChangesModelDatas).DataShareRequestAuditLog;

        var result = testResult.AuditLogEntries;

        Assert.That(result, Has.Exactly(testAuditLogsForDataShareRequestStatusChangesModelDatas.Count).Items);
    }

    [Test]
    public void GivenAuditLogDataShareRequestStatusChangeModelDatasWithConfiguredIds_WhenIConvertAuditLogDataShareRequestStatusChangeModelDatasToAuditLogDataShareRequestStatusChangesResult_ThenDataShareRequestAuditLogsAreReturnedWithTheGivenIds()
    {
        var testItems = CreateTestItems();

        var testDataShareRequestIds = new List<Guid>
        {
            new("4A4D3CC9-73FC-4380-A5A9-D63201177081"),
            new("EFF4562A-FA00-40FD-B682-FBD594183FF8"),
            new("3EE98462-44B7-4802-B0CF-043A38A10E38")
        };

        var testAuditLogsForDataShareRequestStatusChangesModelDatas = testDataShareRequestIds.Select(dataShareRequestId =>
            CreateTestAuditLogDataShareRequestStatusChangeModelData(dataShareRequestId: dataShareRequestId)).ToList();

        var testResult = testItems.AuditLogModelDataFactory.ConvertAuditLogDataShareRequestStatusChangeModelDatasToAuditLogDataShareRequestStatusChangesResult(
            It.IsAny<Guid>(),
            testAuditLogsForDataShareRequestStatusChangesModelDatas).DataShareRequestAuditLog;

        var result = testResult.AuditLogEntries;

        Assert.Multiple(() =>
        {
            foreach (var testDataShareRequestId in testDataShareRequestIds)
            {
                Assert.That(result.FirstOrDefault(x => x.DataShareRequestId == testDataShareRequestId), Is.Not.Null);
            }
        });
    }

    [Test]
    [TestCaseSource(nameof(DataShareRequestStatusTypeToDataShareRequestStatusTestCaseData))]
    public void GivenAnAuditLogDataShareRequestStatusChangeModelDatasWithConfiguredFromStatus_WhenIConvertAuditLogDataShareRequestStatusChangeModelDatasToAuditLogDataShareRequestStatusChangesResult_ThenDataShareRequestAuditLogIsReturnedWithTheExpectedMappedFromStatus(
        DataShareRequestStatusType testFromStatus,
        DataShareRequestStatus expectedFromStatus)
    {
        var testItems = CreateTestItems();

        var testAuditLogsForDataShareRequestStatusChangesModelDatas = new List<AuditLogDataShareRequestStatusChangeModelData> 
        {
            CreateTestAuditLogDataShareRequestStatusChangeModelData(fromStatus: testFromStatus)
        };

        var testResult = testItems.AuditLogModelDataFactory.ConvertAuditLogDataShareRequestStatusChangeModelDatasToAuditLogDataShareRequestStatusChangesResult(
            It.IsAny<Guid>(),
            testAuditLogsForDataShareRequestStatusChangesModelDatas).DataShareRequestAuditLog;

        var result = testResult.AuditLogEntries;

        Assert.That(result.Single().FromStatus, Is.EqualTo(expectedFromStatus));
    }

    [Test]
    public void GivenAnAuditLogDataShareRequestStatusChangeModelDatasWithFromStatusOfNone_WhenIConvertAuditLogDataShareRequestStatusChangeModelDatasToAuditLogDataShareRequestStatusChangesResult_ThenDataShareRequestAuditLogIsReturnedWithNullFromStatus()
    {
        var testItems = CreateTestItems();

        var testAuditLogsForDataShareRequestStatusChangesModelDatas = new List<AuditLogDataShareRequestStatusChangeModelData>
        {
            CreateTestAuditLogDataShareRequestStatusChangeModelData(fromStatus: DataShareRequestStatusType.None)
        };

        var testResult = testItems.AuditLogModelDataFactory.ConvertAuditLogDataShareRequestStatusChangeModelDatasToAuditLogDataShareRequestStatusChangesResult(
            It.IsAny<Guid>(),
            testAuditLogsForDataShareRequestStatusChangesModelDatas).DataShareRequestAuditLog;

        var result = testResult.AuditLogEntries;

        Assert.That(result.Single().FromStatus, Is.Null);
    }



    [Test]
    public void GivenAnAuditLogDataShareRequestStatusChangeModelDatasWithInvalidFromStatus_WhenIConvertAuditLogDataShareRequestStatusChangeModelDatasToAuditLogDataShareRequestStatusChangesResult_ThenDataShareRequestAuditLogIsReturnedWithNullFromStatus()
    {
        var testItems = CreateTestItems();

        var testFromStatus = (DataShareRequestStatusType)(Enum.GetValues<DataShareRequestStatusType>().Cast<int>().Max() + 1);

        var testAuditLogsForDataShareRequestStatusChangesModelDatas = new List<AuditLogDataShareRequestStatusChangeModelData>
        {
            CreateTestAuditLogDataShareRequestStatusChangeModelData(fromStatus: testFromStatus)
        };

        var testResult = testItems.AuditLogModelDataFactory.ConvertAuditLogDataShareRequestStatusChangeModelDatasToAuditLogDataShareRequestStatusChangesResult(
            It.IsAny<Guid>(),
            testAuditLogsForDataShareRequestStatusChangesModelDatas).DataShareRequestAuditLog;

        var result = testResult.AuditLogEntries;

        Assert.That(result.Single().FromStatus, Is.Null);
    }

    [Test]
    [TestCaseSource(nameof(DataShareRequestStatusTypeToDataShareRequestStatusTestCaseData))]
    public void GivenAnAuditLogDataShareRequestStatusChangeModelDatasWithConfiguredToStatus_WhenIConvertAuditLogDataShareRequestStatusChangeModelDatasToAuditLogDataShareRequestStatusChangesResult_ThenDataShareRequestAuditLogIsReturnedWithTheExpectedMappedToStatus(
        DataShareRequestStatusType testToStatus,
        DataShareRequestStatus expectedToStatus)
    {
        var testItems = CreateTestItems();

        var testAuditLogsForDataShareRequestStatusChangesModelDatas = new List<AuditLogDataShareRequestStatusChangeModelData> 
        {
            CreateTestAuditLogDataShareRequestStatusChangeModelData(toStatus: testToStatus)
        };

        var testResult = testItems.AuditLogModelDataFactory.ConvertAuditLogDataShareRequestStatusChangeModelDatasToAuditLogDataShareRequestStatusChangesResult(
            It.IsAny<Guid>(),
            testAuditLogsForDataShareRequestStatusChangesModelDatas).DataShareRequestAuditLog;

        var result = testResult.AuditLogEntries;

        Assert.That(result.Single().ToStatus, Is.EqualTo(expectedToStatus));
    }

    [Test]
    public void GivenAnAuditLogDataShareRequestStatusChangeModelDatasWithNoConfiguredToStatus_WhenIConvertAuditLogDataShareRequestStatusChangeModelDatasToAuditLogDataShareRequestStatusChangesResult_ThenDataShareRequestAuditLogIsReturnedWithNullToStatus(
        [Values(null, DataShareRequestStatusType.None)] DataShareRequestStatusType? testToStatus)
    {
        var testItems = CreateTestItems();

        var testAuditLogsForDataShareRequestStatusChangesModelDatas = new List<AuditLogDataShareRequestStatusChangeModelData>
        {
            CreateTestAuditLogDataShareRequestStatusChangeModelData(toStatus: testToStatus)
        };

        var testResult = testItems.AuditLogModelDataFactory.ConvertAuditLogDataShareRequestStatusChangeModelDatasToAuditLogDataShareRequestStatusChangesResult(
            It.IsAny<Guid>(),
            testAuditLogsForDataShareRequestStatusChangesModelDatas).DataShareRequestAuditLog;

        var result = testResult.AuditLogEntries;

        Assert.That(result.Single().ToStatus, Is.Null);
    }

    [Test]
    public void GivenAnAuditLogDataShareRequestStatusChangeModelDatasWithConfiguredChangedByIds_WhenIConvertAuditLogDataShareRequestStatusChangeModelDatasToAuditLogDataShareRequestStatusChangesResult_ThenDataShareRequestAuditLogIsReturnedWithTheGivenChangedByIds()
    {
        var testItems = CreateTestItems();

        const int testChangedByUserId = 100;
        const int testChangedByUserDomainId = 200;
        const int testChangedByUserOrganisationId = 300;

        var testAuditLogsForDataShareRequestStatusChangesModelDatas = new List<AuditLogDataShareRequestStatusChangeModelData>
        {
            CreateTestAuditLogDataShareRequestStatusChangeModelData(
                changedByUserId: testChangedByUserId,
                changedByUserDomainId: testChangedByUserDomainId,
                changedByUserOrganisationId: testChangedByUserOrganisationId)
        };

        var testResult = testItems.AuditLogModelDataFactory.ConvertAuditLogDataShareRequestStatusChangeModelDatasToAuditLogDataShareRequestStatusChangesResult(
            It.IsAny<Guid>(),
            testAuditLogsForDataShareRequestStatusChangesModelDatas).DataShareRequestAuditLog;

        var result = testResult.AuditLogEntries;

        Assert.Multiple(() =>
        {
            Assert.That(result.Single().ChangedByUserId, Is.EqualTo(testChangedByUserId));
            Assert.That(result.Single().ChangedByDomainId, Is.EqualTo(testChangedByUserDomainId));
            Assert.That(result.Single().ChangedByOrganisationId, Is.EqualTo(testChangedByUserOrganisationId));
        });
    }

    [Test]
    public void GivenAnAuditLogDataShareRequestStatusChangeModelDatasWithConfiguredChangedAtDate_WhenIConvertAuditLogDataShareRequestStatusChangeModelDatasToAuditLogDataShareRequestStatusChangesResult_ThenDataShareRequestAuditLogIsReturnedWithTheGivenChangedAtDate()
    {
        var testItems = CreateTestItems();

        var testChangedAtDate = new DateTime(2025, 12, 25, 13, 45, 59);

        var testAuditLogsForDataShareRequestStatusChangesModelDatas = new List<AuditLogDataShareRequestStatusChangeModelData>
        {
            CreateTestAuditLogDataShareRequestStatusChangeModelData(changedAtUtc: testChangedAtDate)
        };

        var testResult = testItems.AuditLogModelDataFactory.ConvertAuditLogDataShareRequestStatusChangeModelDatasToAuditLogDataShareRequestStatusChangesResult(
            It.IsAny<Guid>(),
            testAuditLogsForDataShareRequestStatusChangesModelDatas).DataShareRequestAuditLog;

        var result = testResult.AuditLogEntries;

        Assert.That(result.Single().ChangedOnUtc, Is.EqualTo(testChangedAtDate));
    }

    [Test]
    public void GivenAnAuditLogDataShareRequestStatusChangeModelDatasWithConfiguredComments_WhenIConvertAuditLogDataShareRequestStatusChangeModelDatasToAuditLogDataShareRequestStatusChangesResult_ThenDataShareRequestAuditLogIsReturnedWithTheGivenComments()
    {
        var testItems = CreateTestItems();

        var testComments = testItems.Fixture.CreateMany<AuditLogDataShareRequestStatusChangeCommentModelData>().ToList();

        var testAuditLogsForDataShareRequestStatusChangesModelDatas = new List<AuditLogDataShareRequestStatusChangeModelData>
        {
            CreateTestAuditLogDataShareRequestStatusChangeModelData(comments: testComments)
        };

        var testResult = testItems.AuditLogModelDataFactory.ConvertAuditLogDataShareRequestStatusChangeModelDatasToAuditLogDataShareRequestStatusChangesResult(
            It.IsAny<Guid>(),
            testAuditLogsForDataShareRequestStatusChangesModelDatas).DataShareRequestAuditLog;

        var result = testResult.AuditLogEntries;

        Assert.Multiple(() =>
        {
            foreach (var testComment in testComments)
            {
                Assert.That(result.Single().Comments.FirstOrDefault(x =>
                    x.Comment == testComment.AuditLogDataShareRequestStatusChangeComment_Comment &&
                    x.CommentOrder == testComment.AuditLogDataShareRequestStatusChangeComment_CommentOrder), Is.Not.Null);
            }
        });
    }

    [Test]
    public void GivenAnAuditLogDataShareRequestStatusChangeModelDatasWithAConfiguredNullComment_WhenIConvertAuditLogDataShareRequestStatusChangeModelDatasToAuditLogDataShareRequestStatusChangesResult_ThenDataShareRequestAuditLogIsReturnedWithoutANullComment()
    {
        var testItems = CreateTestItems();

        var testComments = testItems.Fixture.CreateMany<AuditLogDataShareRequestStatusChangeCommentModelData>().ToList();
        testComments[1] = null!;

        var testAuditLogsForDataShareRequestStatusChangesModelDatas = new List<AuditLogDataShareRequestStatusChangeModelData>
        {
            CreateTestAuditLogDataShareRequestStatusChangeModelData(comments: testComments)
        };

        var testResult = testItems.AuditLogModelDataFactory.ConvertAuditLogDataShareRequestStatusChangeModelDatasToAuditLogDataShareRequestStatusChangesResult(
            It.IsAny<Guid>(),
            testAuditLogsForDataShareRequestStatusChangesModelDatas).DataShareRequestAuditLog;

        var result = testResult.AuditLogEntries;

        Assert.Multiple(() =>
        {
            Assert.That(result.Single().Comments, Has.Exactly(testComments.Count - 1).Items);

            foreach (var resultComment in result.Single().Comments)
            {
                Assert.That(resultComment, Is.Not.Null);
            }
        });
    }

    private static IEnumerable<TestCaseData> DataShareRequestStatusTypeToDataShareRequestStatusTestCaseData()
    {
        yield return new TestCaseData(DataShareRequestStatusType.Draft, DataShareRequestStatus.Draft);
        yield return new TestCaseData(DataShareRequestStatusType.Submitted, DataShareRequestStatus.Submitted);
        yield return new TestCaseData(DataShareRequestStatusType.Rejected, DataShareRequestStatus.Rejected);
        yield return new TestCaseData(DataShareRequestStatusType.Accepted, DataShareRequestStatus.Accepted);
        yield return new TestCaseData(DataShareRequestStatusType.Cancelled, DataShareRequestStatus.Cancelled);
        yield return new TestCaseData(DataShareRequestStatusType.Returned, DataShareRequestStatus.Returned);
        yield return new TestCaseData(DataShareRequestStatusType.InReview, DataShareRequestStatus.InReview);
        yield return new TestCaseData(DataShareRequestStatusType.Deleted, DataShareRequestStatus.Deleted);
    }

    private static AuditLogDataShareRequestStatusChangeModelData CreateTestAuditLogDataShareRequestStatusChangeModelData(
        Guid? id = null,
        Guid? dataShareRequestId = null,
        DataShareRequestStatusType? fromStatus = null,
        DataShareRequestStatusType? toStatus = null,
        int? changedByUserId = null,
        int? changedByUserDomainId = null,
        int? changedByUserOrganisationId = null,
        DateTime? changedAtUtc = null,
        IEnumerable<AuditLogDataShareRequestStatusChangeCommentModelData>? comments = null)
    {
        return new AuditLogDataShareRequestStatusChangeModelData
        {
            AuditLogDataShareRequestStatusChange_Id = id ?? It.IsAny<Guid>(),
            AuditLogDataShareRequestStatusChange_DataShareRequestId = dataShareRequestId ?? It.IsAny<Guid>(),
            AuditLogDataShareRequestStatusChange_FromStatus = fromStatus ?? It.IsAny<DataShareRequestStatusType>(),
            AuditLogDataShareRequestStatusChange_ToStatus = toStatus ?? It.IsAny<DataShareRequestStatusType>(),
            AuditLogDataShareRequestStatusChange_ChangedByUserId = changedByUserId ?? It.IsAny<int>(),
            AuditLogDataShareRequestStatusChange_ChangedByUserDomainId = changedByUserDomainId ?? It.IsAny<int>(),
            AuditLogDataShareRequestStatusChange_ChangedByUserOrganisationId = changedByUserOrganisationId ?? It.IsAny<int>(),
            AuditLogDataShareRequestStatusChange_ChangedAtUtc = changedAtUtc ?? It.IsAny<DateTime>(),
            AuditLogDataShareRequestStatusChange_Comments = comments?.ToList() ?? []
        };

    }
    #endregion

    #region ConvertDataShareRequestStatusToDataShareRequestStatusType() Tests
    [Theory]
    public void GivenAValidDataShareRequestStatus_WhenIConvertDataShareRequestStatusToDataShareRequestStatusType_ThenAConvertedValueIsReturned(
        DataShareRequestStatus testFromDataShareRequestStatus)
    {
        var testItems = CreateTestItems();

        Assert.That(() => testItems.AuditLogModelDataFactory.ConvertDataShareRequestStatusToDataShareRequestStatusType(
                testFromDataShareRequestStatus),
            Throws.Nothing);
    }

    [Test]
    public void GivenAnInvalidDataShareRequestStatus_WhenIConvertDataShareRequestStatusToDataShareRequestStatusType_ThenAnInvalidEnumArgumentExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        var testFromDataShareRequestStatus = (DataShareRequestStatus) (Enum.GetValues<DataShareRequestStatus>().Cast<int>().Max() + 1);

        Assert.That(() => testItems.AuditLogModelDataFactory.ConvertDataShareRequestStatusToDataShareRequestStatusType(
                testFromDataShareRequestStatus),
            Throws.TypeOf<InvalidEnumArgumentException>());
    }

    [Test]
    [TestCaseSource(nameof(DataShareRequestStatusToDataShareRequestStatusTypeTestCaseData))]
    public void GivenAKnownDataShareRequestStatus_WhenIConvertDataShareRequestStatusToDataShareRequestStatusType_ThenTheExpectedValueIsReturned(
        DataShareRequestStatus testFromDataShareRequestStatus,
        DataShareRequestStatusType expectedToDataShareRequestStatusType)
    {
        var testItems = CreateTestItems();

        var result = testItems.AuditLogModelDataFactory.ConvertDataShareRequestStatusToDataShareRequestStatusType(
            testFromDataShareRequestStatus);

        Assert.That(result, Is.EqualTo(expectedToDataShareRequestStatusType));
    }

    private static IEnumerable<TestCaseData> DataShareRequestStatusToDataShareRequestStatusTypeTestCaseData()
    {
        yield return new TestCaseData(DataShareRequestStatus.Draft, DataShareRequestStatusType.Draft);
        yield return new TestCaseData(DataShareRequestStatus.Submitted, DataShareRequestStatusType.Submitted);
        yield return new TestCaseData(DataShareRequestStatus.Rejected, DataShareRequestStatusType.Rejected);
        yield return new TestCaseData(DataShareRequestStatus.Accepted, DataShareRequestStatusType.Accepted);
        yield return new TestCaseData(DataShareRequestStatus.Cancelled, DataShareRequestStatusType.Cancelled);
        yield return new TestCaseData(DataShareRequestStatus.Returned, DataShareRequestStatusType.Returned);
        yield return new TestCaseData(DataShareRequestStatus.InReview, DataShareRequestStatusType.InReview);
        yield return new TestCaseData(DataShareRequestStatus.Deleted, DataShareRequestStatusType.Deleted);
    }
    #endregion

    #region Test Item Creation
    private static TestItems CreateTestItems()
    {
        var fixture = new Fixture().Customize(new AutoMoqCustomization());

        var testAuditLogModelDataFactory = new AuditLogModelDataFactory();

        return new TestItems(
            fixture,
            testAuditLogModelDataFactory);
    }

    private class TestItems(
        IFixture fixture,
        IAuditLogModelDataFactory auditLogModelDataFactory)
    {
        public IFixture Fixture { get; } = fixture;

        public IAuditLogModelDataFactory AuditLogModelDataFactory { get; } = auditLogModelDataFactory;
    }
    #endregion

}