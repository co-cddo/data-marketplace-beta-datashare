using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.ModelData.DataShareRequests;

[TestFixture]
public class QuestionSetSectionOutlineModelDataTests
{
    [Test]
    public void GivenAQuestionSetSectionOutlineModelData_WhenISetId_ThenIdIsSet()
    {
        var testQuestionSetSectionOutlineModelData = new QuestionSetSectionOutlineModelData();

        var testId = new Guid("39A40D9D-37D4-4AAA-A1CE-4BBA1C9FDD32");

        testQuestionSetSectionOutlineModelData.QuestionSetSectionOutline_Id = testId;

        var result = testQuestionSetSectionOutlineModelData.QuestionSetSectionOutline_Id;

        Assert.That(result, Is.EqualTo(testId));
    }

    [Test]
    public void GivenAQuestionSetSectionOutlineModelData_WhenISetOrderWithinQuestionSetOutline_ThenOrderWithinQuestionSetOutlineIsSet(
        [Values(-1, 0, 999)] int testOrderWithinQuestionSetOutline)
    {
        var testQuestionSetSectionOutlineModelData = new QuestionSetSectionOutlineModelData();

        testQuestionSetSectionOutlineModelData.QuestionSetSectionOutline_OrderWithinQuestionSetOutline = testOrderWithinQuestionSetOutline;

        var result = testQuestionSetSectionOutlineModelData.QuestionSetSectionOutline_OrderWithinQuestionSetOutline;

        Assert.That(result, Is.EqualTo(testOrderWithinQuestionSetOutline));
    }

    [Test]
    public void GivenAQuestionSetSectionOutlineModelData_WhenISetSectionHeader_ThenSectionHeaderIsSet(
        [Values("", "  ", "abc")] string testSectionHeader)
    {
        var testQuestionSetSectionOutlineModelData = new QuestionSetSectionOutlineModelData();

        testQuestionSetSectionOutlineModelData.QuestionSetSectionOutline_SectionHeader = testSectionHeader;

        var result = testQuestionSetSectionOutlineModelData.QuestionSetSectionOutline_SectionHeader;

        Assert.That(result, Is.EqualTo(testSectionHeader));
    }

    [Test]
    public void GivenAQuestionSetSectionOutlineModelData_WhenISetAnEmptySetOfQuestions_ThenQuestionsIsSet()
    {
        var testQuestionSetSectionOutlineModelData = new QuestionSetSectionOutlineModelData();

        var testQuestions = new List<QuestionSetQuestionOutlineModelData>();

        testQuestionSetSectionOutlineModelData.Questions = testQuestions;

        var result = testQuestionSetSectionOutlineModelData.Questions;

        Assert.That(result, Is.EqualTo(testQuestions));
    }

    [Test]
    public void GivenAQuestionSetSectionOutlineModelData_WhenISetQuestions_ThenQuestionsIsSet()
    {
        var testQuestionSetSectionOutlineModelData = new QuestionSetSectionOutlineModelData();

        var testQuestions = new List<QuestionSetQuestionOutlineModelData> {new(), new(), new()};

        testQuestionSetSectionOutlineModelData.Questions = testQuestions;

        var result = testQuestionSetSectionOutlineModelData.Questions;

        Assert.That(result, Is.EqualTo(testQuestions));
    }
}