using Agrimetrics.DataShare.Api.Logic.ModelData.Questions;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.ModelData.Questions;

[TestFixture]
public class QuestionSummaryModelDataTests
{
    [Test]
    public void GivenAQuestionSummaryModelData_WhenISetId_ThenIdIsSet()
    {
        var testQuestionSummaryModelData = new QuestionSummaryModelData();

        var testId = new Guid("806946EF-DCAE-4520-BD77-71CC2A6E6B9D");

        testQuestionSummaryModelData.Question_Id = testId;

        var result = testQuestionSummaryModelData.Question_Id;

        Assert.That(result, Is.EqualTo(testId));
    }

    [Test]
    public void GivenAQuestionSummaryModelData_WhenISetOrderWithinQuestionSetSection_ThenOrderWithinQuestionSetSectionIsSet(
        [Values(-1, 0, 999)] int testOrderWithinQuestionSetSection)
    {
        var testQuestionSummaryModelData = new QuestionSummaryModelData();

        testQuestionSummaryModelData.Question_OrderWithinQuestionSetSection = testOrderWithinQuestionSetSection;

        var result = testQuestionSummaryModelData.Question_OrderWithinQuestionSetSection;

        Assert.That(result, Is.EqualTo(testOrderWithinQuestionSetSection));
    }

    [Test]
    public void GivenAQuestionSummaryModelData_WhenISetHeader_ThenHeaderIsSet(
        [Values("", "  ", "abc")] string testHeader)
    {
        var testQuestionSummaryModelData = new QuestionSummaryModelData();

        testQuestionSummaryModelData.Question_Header = testHeader;

        var result = testQuestionSummaryModelData.Question_Header;

        Assert.That(result, Is.EqualTo(testHeader));
    }

    [Theory]
    public void GivenAQuestionSummaryModelData_WhenISetQuestionStatus_ThenQuestionStatusIsSet(
        QuestionStatusType testQuestionStatus)
    {
        var testQuestionSummaryModelData = new QuestionSummaryModelData();

        testQuestionSummaryModelData.Question_QuestionStatus = testQuestionStatus;

        var result = testQuestionSummaryModelData.Question_QuestionStatus;

        Assert.That(result, Is.EqualTo(testQuestionStatus));
    }

    [Theory]
    public void GivenAQuestionSummaryModelData_WhenISetQuestionCanBeAnswered_ThenQuestionCanBeAnsweredIsSet(
        bool testQuestionCanBeAnswered)
    {
        var testQuestionSummaryModelData = new QuestionSummaryModelData();

        testQuestionSummaryModelData.Question_QuestionCanBeAnswered = testQuestionCanBeAnswered;

        var result = testQuestionSummaryModelData.Question_QuestionCanBeAnswered;

        Assert.That(result, Is.EqualTo(testQuestionCanBeAnswered));
    }
}