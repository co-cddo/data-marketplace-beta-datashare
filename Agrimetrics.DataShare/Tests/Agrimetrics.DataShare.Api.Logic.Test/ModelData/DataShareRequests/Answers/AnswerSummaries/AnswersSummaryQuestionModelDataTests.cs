using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests.Answers.DataShareRequestAnswerSummaries;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.ModelData.DataShareRequests.Answers.DsrAnswerSummaries;

[TestFixture]
public class DataShareRequestAnswersSummaryQuestionModelDataTests
{
    [Test]
    public void GivenADataShareRequestAnswersSummaryQuestionModelData_WhenISetQuestionId_ThenQuestionIdIsSet()
    {
        var testDataShareRequestAnswersSummaryQuestionModelData = new DataShareRequestAnswersSummaryQuestionModelData();

        var testQuestionId = new Guid("2D9B44E6-F24A-403E-88AB-6DCB03B39BEC");

        testDataShareRequestAnswersSummaryQuestionModelData.DataShareRequestAnswersSummaryQuestion_QuestionId = testQuestionId;

        var result = testDataShareRequestAnswersSummaryQuestionModelData.DataShareRequestAnswersSummaryQuestion_QuestionId;

        Assert.That(result, Is.EqualTo(testQuestionId));
    }

    [Test]
    public void GivenADataShareRequestAnswersSummaryQuestionModelData_WhenISetQuestionHeader_ThenQuestionHeaderIsSet(
        [Values("", "  ", "abc")] string testQuestionHeader)
    {
        var testDataShareRequestAnswersSummaryQuestionModelData = new DataShareRequestAnswersSummaryQuestionModelData();

        testDataShareRequestAnswersSummaryQuestionModelData.DataShareRequestAnswersSummaryQuestion_QuestionHeader = testQuestionHeader;

        var result = testDataShareRequestAnswersSummaryQuestionModelData.DataShareRequestAnswersSummaryQuestion_QuestionHeader;

        Assert.That(result, Is.EqualTo(testQuestionHeader));
    }

    [Theory]
    public void GivenADataShareRequestAnswersSummaryQuestionModelData_WhenISetQuestionIsApplicable_ThenQuestionIsApplicableIsSet(
        bool testQuestionIsApplicable)
    {
        var testDataShareRequestAnswersSummaryQuestionModelData = new DataShareRequestAnswersSummaryQuestionModelData();

        testDataShareRequestAnswersSummaryQuestionModelData.DataShareRequestAnswersSummaryQuestion_QuestionIsApplicable = testQuestionIsApplicable;

        var result = testDataShareRequestAnswersSummaryQuestionModelData.DataShareRequestAnswersSummaryQuestion_QuestionIsApplicable;

        Assert.That(result, Is.EqualTo(testQuestionIsApplicable));
    }

    [Test]
    public void GivenADataShareRequestAnswersSummaryQuestionModelData_WhenISetAnEmptySetOfQuestionPartIds_ThenQuestionPartIdsIsSet()
    {
        var testDataShareRequestAnswersSummaryQuestionModelData = new DataShareRequestAnswersSummaryQuestionModelData();

        var testQuestionPartIds = new List<DataShareRequestAnswersSummaryQuestionQuestionPartIdModelData>();

        testDataShareRequestAnswersSummaryQuestionModelData.DataShareRequestAnswersSummaryQuestion_QuestionPartIds = testQuestionPartIds;

        var result = testDataShareRequestAnswersSummaryQuestionModelData.DataShareRequestAnswersSummaryQuestion_QuestionPartIds;

        Assert.That(result, Is.EqualTo(testQuestionPartIds));
    }

    [Test]
    public void GivenADataShareRequestAnswersSummaryQuestionModelData_WhenISetQuestionPartIds_ThenQuestionPartIdsIsSet()
    {
        var testDataShareRequestAnswersSummaryQuestionModelData = new DataShareRequestAnswersSummaryQuestionModelData();

        var testQuestionPartIds = new List<DataShareRequestAnswersSummaryQuestionQuestionPartIdModelData> {new(), new(), new()};

        testDataShareRequestAnswersSummaryQuestionModelData.DataShareRequestAnswersSummaryQuestion_QuestionPartIds = testQuestionPartIds;

        var result = testDataShareRequestAnswersSummaryQuestionModelData.DataShareRequestAnswersSummaryQuestion_QuestionPartIds;

        Assert.That(result, Is.EqualTo(testQuestionPartIds));
    }

    [Test]
    public void GivenADataShareRequestAnswersSummaryQuestionModelData_WhenISetAnEmptySetOfQuestionParts_ThenQuestionPartsIsSet()
    {
        var testDataShareRequestAnswersSummaryQuestionModelData = new DataShareRequestAnswersSummaryQuestionModelData();

        var testQuestionParts = new List<DataShareRequestAnswersSummaryQuestionPartModelData>();

        testDataShareRequestAnswersSummaryQuestionModelData.DataShareRequestAnswersSummaryQuestion_QuestionParts = testQuestionParts;

        var result = testDataShareRequestAnswersSummaryQuestionModelData.DataShareRequestAnswersSummaryQuestion_QuestionParts;

        Assert.That(result, Is.EqualTo(testQuestionParts));
    }

    [Test]
    public void GivenADataShareRequestAnswersSummaryQuestionModelData_WhenISetQuestionParts_ThenQuestionPartsIsSet()
    {
        var testDataShareRequestAnswersSummaryQuestionModelData = new DataShareRequestAnswersSummaryQuestionModelData();

        var testQuestionParts = new List<DataShareRequestAnswersSummaryQuestionPartModelData> {new(), new(), new()};

        testDataShareRequestAnswersSummaryQuestionModelData.DataShareRequestAnswersSummaryQuestion_QuestionParts = testQuestionParts;

        var result = testDataShareRequestAnswersSummaryQuestionModelData.DataShareRequestAnswersSummaryQuestion_QuestionParts;

        Assert.That(result, Is.EqualTo(testQuestionParts));
    }
}