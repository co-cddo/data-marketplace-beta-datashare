using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Dto.Test.Models.DataShareRequests;

[TestFixture]
public class DataShareRequestAdminSummaryTests
{
    [Test]
    public void GivenADataShareRequestAdminSummary_WhenISetId_ThenIdIsSet()
    {
        var testId = new Guid("E1FF2635-1C80-457B-9065-DBE6DAD24B02");

        var testDataShareRequestAdminSummary = new DataShareRequestAdminSummary
        {
            Id = testId,
            RequestId = "_",
            EsdaName = "_",
            WhenCreatedUtc = new DateTime(),
            WhenSubmittedUtc = null,
            CreatedByUserEmailAddress = "_",
            WhenNeededByUtc = null,
            DataShareRequestStatus = Enum.GetValues<DataShareRequestStatus>().First()
        };

        var result = testDataShareRequestAdminSummary.Id;

        Assert.That(result, Is.EqualTo(testId));
    }

    [Test]
    public void GivenADataShareRequestAdminSummary_WhenISetRequestId_ThenRequestIdIsSet(
        [Values("", "  ", "abc")] string testRequestId)
    {
        var testDataShareRequestAdminSummary = new DataShareRequestAdminSummary
        {
            Id = Guid.Empty,
            RequestId = testRequestId,
            EsdaName = "_",
            WhenCreatedUtc = new DateTime(),
            WhenSubmittedUtc = null,
            CreatedByUserEmailAddress = "_",
            WhenNeededByUtc = null,
            DataShareRequestStatus = Enum.GetValues<DataShareRequestStatus>().First()
        };

        var result = testDataShareRequestAdminSummary.RequestId;

        Assert.That(result, Is.EqualTo(testRequestId));
    }

    [Test]
    public void GivenADataShareRequestAdminSummary_WhenISetEsdaName_ThenEsdaNameIsSet(
        [Values("", "  ", "abc")] string testEsdaName)
    {
        var testDataShareRequestAdminSummary = new DataShareRequestAdminSummary
        {
            Id = Guid.Empty,
            RequestId = "_",
            EsdaName = testEsdaName,
            WhenCreatedUtc = new DateTime(),
            WhenSubmittedUtc = null,
            CreatedByUserEmailAddress = "_",
            WhenNeededByUtc = null,
            DataShareRequestStatus = Enum.GetValues<DataShareRequestStatus>().First()
        };

        var result = testDataShareRequestAdminSummary.EsdaName;

        Assert.That(result, Is.EqualTo(testEsdaName));
    }

    [Test]
    public void GivenADataShareRequestAdminSummary_WhenISetWhenCreatedUtc_ThenWhenCreatedUtcIsSet()
    {
        var testWhenCreatedUtc = new DateTime(2025, 12, 25, 14, 45, 59);

        var testDataShareRequestAdminSummary = new DataShareRequestAdminSummary
        {
            Id = Guid.Empty,
            RequestId = "_",
            EsdaName = "_",
            WhenCreatedUtc = testWhenCreatedUtc,
            WhenSubmittedUtc = null,
            CreatedByUserEmailAddress = "_",
            WhenNeededByUtc = null,
            DataShareRequestStatus = Enum.GetValues<DataShareRequestStatus>().First()
        };

        var result = testDataShareRequestAdminSummary.WhenCreatedUtc;

        Assert.That(result, Is.EqualTo(testWhenCreatedUtc));
    }

    [Test]
    public void GivenADataShareRequestAdminSummary_WhenISetANullWhenSubmittedUtc_ThenWhenSubmittedUtcIsSet()
    {
        var testWhenSubmittedUtc = (DateTime?) null;

        var testDataShareRequestAdminSummary = new DataShareRequestAdminSummary
        {
            Id = Guid.Empty,
            RequestId = "_",
            EsdaName = "_",
            WhenCreatedUtc = new DateTime(),
            WhenSubmittedUtc = testWhenSubmittedUtc,
            CreatedByUserEmailAddress = "_",
            WhenNeededByUtc = null,
            DataShareRequestStatus = Enum.GetValues<DataShareRequestStatus>().First()
        };

        var result = testDataShareRequestAdminSummary.WhenSubmittedUtc;

        Assert.That(result, Is.EqualTo(testWhenSubmittedUtc));
    }

    [Test]
    public void GivenADataShareRequestAdminSummary_WhenISetWhenSubmittedUtc_ThenWhenSubmittedUtcIsSet()
    {
        var testWhenSubmittedUtc = new DateTime(2025, 12, 25, 14, 45, 59);

        var testDataShareRequestAdminSummary = new DataShareRequestAdminSummary
        {
            Id = Guid.Empty,
            RequestId = "_",
            EsdaName = "_",
            WhenCreatedUtc = new DateTime(),
            WhenSubmittedUtc = testWhenSubmittedUtc,
            CreatedByUserEmailAddress = "_",
            WhenNeededByUtc = null,
            DataShareRequestStatus = Enum.GetValues<DataShareRequestStatus>().First()
        };

        var result = testDataShareRequestAdminSummary.WhenSubmittedUtc;

        Assert.That(result, Is.EqualTo(testWhenSubmittedUtc));
    }

    [Test]
    public void GivenADataShareRequestAdminSummary_WhenISetCreatedByUserEmailAddress_ThenCreatedByUserEmailAddressIsSet(
        [Values("", "  ", "abc")] string testCreatedByUserEmailAddress)
    {
        var testDataShareRequestAdminSummary = new DataShareRequestAdminSummary
        {
            Id = Guid.Empty,
            RequestId = "_",
            EsdaName = "_",
            WhenCreatedUtc = new DateTime(),
            WhenSubmittedUtc = null,
            CreatedByUserEmailAddress = testCreatedByUserEmailAddress,
            WhenNeededByUtc = null,
            DataShareRequestStatus = Enum.GetValues<DataShareRequestStatus>().First()
        };

        var result = testDataShareRequestAdminSummary.CreatedByUserEmailAddress;

        Assert.That(result, Is.EqualTo(testCreatedByUserEmailAddress));
    }

    [Test]
    public void GivenADataShareRequestAdminSummary_WhenISetANullWhenNeededByUtc_ThenWhenNeededByUtcIsSet()
    {
        var testWhenNeededByUtc = (DateTime?) null;

        var testDataShareRequestAdminSummary = new DataShareRequestAdminSummary
        {
            Id = Guid.Empty,
            RequestId = "_",
            EsdaName = "_",
            WhenCreatedUtc = new DateTime(),
            WhenSubmittedUtc = null,
            CreatedByUserEmailAddress = "_",
            WhenNeededByUtc = testWhenNeededByUtc,
            DataShareRequestStatus = Enum.GetValues<DataShareRequestStatus>().First()
        };

        var result = testDataShareRequestAdminSummary.WhenNeededByUtc;

        Assert.That(result, Is.EqualTo(testWhenNeededByUtc));
    }

    [Test]
    public void GivenADataShareRequestAdminSummary_WhenISetWhenNeededByUtc_ThenWhenNeededByUtcIsSet()
    {
        var testWhenNeededByUtc = new DateTime(2025, 12, 25, 14, 45, 59);

        var testDataShareRequestAdminSummary = new DataShareRequestAdminSummary
        {
            Id = Guid.Empty,
            RequestId = "_",
            EsdaName = "_",
            WhenCreatedUtc = new DateTime(),
            WhenSubmittedUtc = null,
            CreatedByUserEmailAddress = "_",
            WhenNeededByUtc = testWhenNeededByUtc,
            DataShareRequestStatus = Enum.GetValues<DataShareRequestStatus>().First()
        };

        var result = testDataShareRequestAdminSummary.WhenNeededByUtc;

        Assert.That(result, Is.EqualTo(testWhenNeededByUtc));
    }

    [Theory]
    public void GivenADataShareRequestAdminSummary_WhenISetDataShareRequestStatus_ThenDataShareRequestStatusIsSet(
        DataShareRequestStatus testDataShareRequestStatus)
    {
        var testDataShareRequestAdminSummary = new DataShareRequestAdminSummary
        {
            Id = Guid.Empty,
            RequestId = "_",
            EsdaName = "_",
            WhenCreatedUtc = new DateTime(),
            WhenSubmittedUtc = null,
            CreatedByUserEmailAddress = "_",
            WhenNeededByUtc = null,
            DataShareRequestStatus = testDataShareRequestStatus
        };

        var result = testDataShareRequestAdminSummary.DataShareRequestStatus;

        Assert.That(result, Is.EqualTo(testDataShareRequestStatus));
    }
}

