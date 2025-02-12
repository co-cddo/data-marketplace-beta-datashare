using Agrimetrics.DataShare.Api.Logic.ModelData.Submissions;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.ModelData.Submissions;

[TestFixture]
public class SubmissionDetailsMainQuestionModelDataTests
{
    [Test]
    public void GivenASubmissionDetailsMainQuestionModelData_WhenISetId_ThenIdIsSet()
    {
        var testSubmissionDetailsMainQuestionModelData = new SubmissionDetailsMainQuestionModelData();

        var testId = new Guid("DCF5EA7C-9E97-459C-A08A-7D49352528E0");

        testSubmissionDetailsMainQuestionModelData.SubmissionDetailsMainQuestion_Id = testId;

        var result = testSubmissionDetailsMainQuestionModelData.SubmissionDetailsMainQuestion_Id;

        Assert.That(result, Is.EqualTo(testId));
    }

    [Test]
    public void GivenASubmissionDetailsMainQuestionModelData_WhenISetOrderWithinSection_ThenOrderWithinSectionIsSet(
        [Values(-1, 0, 999)] int testOrderWithinSection)
    {
        var testSubmissionDetailsMainQuestionModelData = new SubmissionDetailsMainQuestionModelData();

        testSubmissionDetailsMainQuestionModelData.SubmissionDetailsMainQuestion_OrderWithinSection = testOrderWithinSection;

        var result = testSubmissionDetailsMainQuestionModelData.SubmissionDetailsMainQuestion_OrderWithinSection;

        Assert.That(result, Is.EqualTo(testOrderWithinSection));
    }

    [Test]
    public void GivenASubmissionDetailsMainQuestionModelData_WhenISetQuestionHeader_ThenQuestionHeaderIsSet(
        [Values("", "  ", "abc")] string testQuestionHeader)
    {
        var testSubmissionDetailsMainQuestionModelData = new SubmissionDetailsMainQuestionModelData();

        testSubmissionDetailsMainQuestionModelData.SubmissionDetailsMainQuestion_QuestionHeader = testQuestionHeader;

        var result = testSubmissionDetailsMainQuestionModelData.SubmissionDetailsMainQuestion_QuestionHeader;

        Assert.That(result, Is.EqualTo(testQuestionHeader));
    }

    [Test]
    public void GivenASubmissionDetailsMainQuestionModelData_WhenISetAnEmptySetOfAnswerParts_ThenAnswerPartsIsSet()
    {
        var testSubmissionDetailsMainQuestionModelData = new SubmissionDetailsMainQuestionModelData();

        var testAnswerParts = new List<SubmissionDetailsAnswerPartModelData>();

        testSubmissionDetailsMainQuestionModelData.SubmissionDetailsMainQuestion_AnswerParts = testAnswerParts;

        var result = testSubmissionDetailsMainQuestionModelData.SubmissionDetailsMainQuestion_AnswerParts;

        Assert.That(result, Is.EqualTo(testAnswerParts));
    }

    [Test]
    public void GivenASubmissionDetailsMainQuestionModelData_WhenISetAnswerParts_ThenAnswerPartsIsSet()
    {
        var testSubmissionDetailsMainQuestionModelData = new SubmissionDetailsMainQuestionModelData();

        var testAnswerParts = new List<SubmissionDetailsAnswerPartModelData> {new(), new(), new()};

        testSubmissionDetailsMainQuestionModelData.SubmissionDetailsMainQuestion_AnswerParts = testAnswerParts;

        var result = testSubmissionDetailsMainQuestionModelData.SubmissionDetailsMainQuestion_AnswerParts;

        Assert.That(result, Is.EqualTo(testAnswerParts));
    }

    [Test]
    public void GivenASubmissionDetailsMainQuestionModelData_WhenISetAnEmptySetOfBackingQuestions_ThenBackingQuestionsIsSet()
    {
        var testSubmissionDetailsMainQuestionModelData = new SubmissionDetailsMainQuestionModelData();

        var testBackingQuestions = new List<SubmissionDetailsBackingQuestionModelData>();

        testSubmissionDetailsMainQuestionModelData.SubmissionDetailsMainQuestion_BackingQuestions = testBackingQuestions;

        var result = testSubmissionDetailsMainQuestionModelData.SubmissionDetailsMainQuestion_BackingQuestions;

        Assert.That(result, Is.EqualTo(testBackingQuestions));
    }

    [Test]
    public void GivenASubmissionDetailsMainQuestionModelData_WhenISetBackingQuestions_ThenBackingQuestionsIsSet()
    {
        var testSubmissionDetailsMainQuestionModelData = new SubmissionDetailsMainQuestionModelData();

        var testBackingQuestions = new List<SubmissionDetailsBackingQuestionModelData> {new(), new(), new()};

        testSubmissionDetailsMainQuestionModelData.SubmissionDetailsMainQuestion_BackingQuestions = testBackingQuestions;

        var result = testSubmissionDetailsMainQuestionModelData.SubmissionDetailsMainQuestion_BackingQuestions;

        Assert.That(result, Is.EqualTo(testBackingQuestions));
    }
}