using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.ModelData.DataShareRequests;

[TestFixture]
public class DataShareRequestSubmissionResultModelDataTests
{
    [Test]
    public void GivenADataShareRequestSubmissionResultModelData_WhenISetId_ThenIdIsSet()
    {
        var testDataShareRequestSubmissionResultModelData = new DataShareRequestSubmissionResultModelData();

        var testId = new Guid("39A40D9D-37D4-4AAA-A1CE-4BBA1C9FDD32");

        testDataShareRequestSubmissionResultModelData.DataShareRequest_Id = testId;

        var result = testDataShareRequestSubmissionResultModelData.DataShareRequest_Id;

        Assert.That(result, Is.EqualTo(testId));
    }

    [Test]
    public void GivenADataShareRequestSubmissionResultModelData_WhenISetRequestId_ThenRequestIdIsSet(
        [Values("", "  ", "abc")] string testRequestId)
    {
        var testDataShareRequestSubmissionResultModelData = new DataShareRequestSubmissionResultModelData();

        testDataShareRequestSubmissionResultModelData.DataShareRequest_RequestId = testRequestId;

        var result = testDataShareRequestSubmissionResultModelData.DataShareRequest_RequestId;

        Assert.That(result, Is.EqualTo(testRequestId));
    }
}