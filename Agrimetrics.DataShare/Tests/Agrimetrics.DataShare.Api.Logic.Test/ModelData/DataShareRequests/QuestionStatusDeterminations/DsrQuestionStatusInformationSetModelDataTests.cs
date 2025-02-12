using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests.QuestionStatusDeterminations;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.ModelData.DataShareRequests.QuestionStatusDeterminations;

[TestFixture]
public class DataShareRequestQuestionStatusInformationSetModelDataTests
{
    [Test]
    public void GivenADataShareRequestQuestionStatusInformationSetModelData_WhenISetDataShareRequestId_ThenDataShareRequestIdIsSet()
    {
        var testDataShareRequestQuestionStatusInformationSetModelData = new DataShareRequestQuestionStatusInformationSetModelData();

        var testDataShareRequestId = new Guid("4A89DAF7-8242-45C1-93B1-81952F9DBC5A");

        testDataShareRequestQuestionStatusInformationSetModelData.DataShareRequestId = testDataShareRequestId;

        var result = testDataShareRequestQuestionStatusInformationSetModelData.DataShareRequestId;

        Assert.That(result, Is.EqualTo(testDataShareRequestId));
    }

    [Test]
    public void GivenADataShareRequestQuestionStatusInformationSetModelData_WhenISetAnEmptySetOfDataShareRequestQuestionStatuses_ThenDataShareRequestQuestionStatusesIsSet()
    {
        var testDataShareRequestQuestionStatusInformationSetModelData = new DataShareRequestQuestionStatusInformationSetModelData();

        var testDataShareRequestQuestionStatuses = new List<DataShareRequestQuestionStatusInformationModelData>();

        testDataShareRequestQuestionStatusInformationSetModelData.DataShareRequestQuestionStatuses = testDataShareRequestQuestionStatuses;

        var result = testDataShareRequestQuestionStatusInformationSetModelData.DataShareRequestQuestionStatuses;

        Assert.That(result, Is.EqualTo(testDataShareRequestQuestionStatuses));
    }

    [Test]
    public void GivenADataShareRequestQuestionStatusInformationSetModelData_WhenISetDataShareRequestQuestionStatuses_ThenDataShareRequestQuestionStatusesIsSet()
    {
        var testDataShareRequestQuestionStatusInformationSetModelData = new DataShareRequestQuestionStatusInformationSetModelData();

        var testDataShareRequestQuestionStatuses = new List<DataShareRequestQuestionStatusInformationModelData> {new(), new(), new()};

        testDataShareRequestQuestionStatusInformationSetModelData.DataShareRequestQuestionStatuses = testDataShareRequestQuestionStatuses;

        var result = testDataShareRequestQuestionStatusInformationSetModelData.DataShareRequestQuestionStatuses;

        Assert.That(result, Is.EqualTo(testDataShareRequestQuestionStatuses));
    }
}