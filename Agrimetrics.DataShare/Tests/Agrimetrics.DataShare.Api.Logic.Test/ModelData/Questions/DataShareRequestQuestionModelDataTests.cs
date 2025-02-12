using Agrimetrics.DataShare.Api.Logic.ModelData.Questions;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.ModelData.Questions;

[TestFixture]
public class DataShareRequestQuestionModelDataTests
{
    [Test]
    public void GivenADataShareRequestQuestionModelData_WhenISetDataShareRequestId_ThenDataShareRequestIdIsSet()
    {
        var testDataShareRequestQuestionModelData = new DataShareRequestQuestionModelData();

        var testDataShareRequestId = new Guid("0783A3EA-2A53-49D6-9B67-2AAC753E387C");

        testDataShareRequestQuestionModelData.DataShareRequestQuestion_DataShareRequestId = testDataShareRequestId;

        var result = testDataShareRequestQuestionModelData.DataShareRequestQuestion_DataShareRequestId;

        Assert.That(result, Is.EqualTo(testDataShareRequestId));
    }

    [Test]
    public void GivenADataShareRequestQuestionModelData_WhenISetDataShareRequestRequestId_ThenDataShareRequestRequestIdIsSet(
        [Values("", "  ", "abc")] string testDataShareRequestRequestId)
    {
        var testDataShareRequestQuestionModelData = new DataShareRequestQuestionModelData();

        testDataShareRequestQuestionModelData.DataShareRequestQuestion_DataShareRequestRequestId = testDataShareRequestRequestId;

        var result = testDataShareRequestQuestionModelData.DataShareRequestQuestion_DataShareRequestRequestId;

        Assert.That(result, Is.EqualTo(testDataShareRequestRequestId));
    }

    [Test]
    public void GivenADataShareRequestQuestionModelData_WhenISetQuestionId_ThenQuestionIdIsSet()
    {
        var testDataShareRequestQuestionModelData = new DataShareRequestQuestionModelData();

        var testQuestionId = new Guid("0783A3EA-2A53-49D6-9B67-2AAC753E387C");

        testDataShareRequestQuestionModelData.DataShareRequestQuestion_QuestionId = testQuestionId;

        var result = testDataShareRequestQuestionModelData.DataShareRequestQuestion_QuestionId;

        Assert.That(result, Is.EqualTo(testQuestionId));
    }

    [Theory]
    public void GivenADataShareRequestQuestionModelData_WhenISetIsOptional_ThenIsOptionalIsSet(
        bool testIsOptional)
    {
        var testDataShareRequestQuestionModelData = new DataShareRequestQuestionModelData();

        testDataShareRequestQuestionModelData.DataShareRequestQuestion_IsOptional = testIsOptional;

        var result = testDataShareRequestQuestionModelData.DataShareRequestQuestion_IsOptional;

        Assert.That(result, Is.EqualTo(testIsOptional));
    }

    [Test]
    public void GivenADataShareRequestQuestionModelData_WhenISetAnEmptySetOfQuestionParts_ThenQuestionPartsIsSet()
    {
        var testDataShareRequestQuestionModelData = new DataShareRequestQuestionModelData();

        var testQuestionParts = new List<DataShareRequestQuestionPartModelData>();

        testDataShareRequestQuestionModelData.DataShareRequestQuestion_QuestionParts = testQuestionParts;

        var result = testDataShareRequestQuestionModelData.DataShareRequestQuestion_QuestionParts;

        Assert.That(result, Is.EqualTo(testQuestionParts));
    }

    [Test]
    public void GivenADataShareRequestQuestionModelData_WhenISetQuestionParts_ThenQuestionPartsIsSet()
    {
        var testDataShareRequestQuestionModelData = new DataShareRequestQuestionModelData();

        var testQuestionParts = new List<DataShareRequestQuestionPartModelData> {new(), new(), new()};

        testDataShareRequestQuestionModelData.DataShareRequestQuestion_QuestionParts = testQuestionParts;

        var result = testDataShareRequestQuestionModelData.DataShareRequestQuestion_QuestionParts;

        Assert.That(result, Is.EqualTo(testQuestionParts));
    }

    [Test]
    public void GivenADataShareRequestQuestionModelData_WhenISetQuestionFooter_ThenQuestionFooterIsSet()
    {
        var testDataShareRequestQuestionModelData = new DataShareRequestQuestionModelData();

        var testQuestionFooter = new DataShareRequestQuestionFooterModelData();

        testDataShareRequestQuestionModelData.DataShareRequestQuestion_QuestionFooter = testQuestionFooter;

        var result = testDataShareRequestQuestionModelData.DataShareRequestQuestion_QuestionFooter;

        Assert.That(result, Is.EqualTo(testQuestionFooter));
    }
}