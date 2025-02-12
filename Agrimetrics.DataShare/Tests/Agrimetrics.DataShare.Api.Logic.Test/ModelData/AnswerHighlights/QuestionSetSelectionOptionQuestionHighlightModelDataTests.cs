using Agrimetrics.DataShare.Api.Logic.ModelData.AnswerHighlights;
using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.ModelData.AnswerHighlights;

[TestFixture]
public class QuestionSetSelectionOptionQuestionHighlightModelDataTests
{
    [Test]
    public void GivenAQuestionSetSelectionOptionQuestionHighlightModelData_WhenISetId_ThenIdIsSet()
    {
        var testQuestionSetSelectionOptionQuestionHighlightModelData = new QuestionSetSelectionOptionQuestionHighlightModelData();

        var testId = new Guid("DB4D74A2-6673-48E9-AC70-DCFD23F3CD11");

        testQuestionSetSelectionOptionQuestionHighlightModelData.QuestionSetSelectionOptionQuestionHighlight_Id = testId;

        var result = testQuestionSetSelectionOptionQuestionHighlightModelData.QuestionSetSelectionOptionQuestionHighlight_Id;

        Assert.That(result, Is.EqualTo(testId));
    }

    [Test]
    public void GivenAQuestionSetSelectionOptionQuestionHighlightModelData_WhenISetQuestionSetId_ThenQuestionSetIdIsSet()
    {
        var testQuestionSetSelectionOptionQuestionHighlightModelData = new QuestionSetSelectionOptionQuestionHighlightModelData();

        var testQuestionSetId = new Guid("DB4D74A2-6673-48E9-AC70-DCFD23F3CD11");

        testQuestionSetSelectionOptionQuestionHighlightModelData.QuestionSetSelectionOptionQuestionHighlight_QuestionSetId = testQuestionSetId;

        var result = testQuestionSetSelectionOptionQuestionHighlightModelData.QuestionSetSelectionOptionQuestionHighlight_QuestionSetId;

        Assert.That(result, Is.EqualTo(testQuestionSetId));
    }

    [Test]
    public void GivenAQuestionSetSelectionOptionQuestionHighlightModelData_WhenISetSelectionOptionId_ThenSelectionOptionIdIsSet()
    {
        var testQuestionSetSelectionOptionQuestionHighlightModelData = new QuestionSetSelectionOptionQuestionHighlightModelData();

        var testSelectionOptionId = new Guid("DB4D74A2-6673-48E9-AC70-DCFD23F3CD11");

        testQuestionSetSelectionOptionQuestionHighlightModelData.QuestionSetSelectionOptionQuestionHighlight_SelectionOptionId = testSelectionOptionId;

        var result = testQuestionSetSelectionOptionQuestionHighlightModelData.QuestionSetSelectionOptionQuestionHighlight_SelectionOptionId;

        Assert.That(result, Is.EqualTo(testSelectionOptionId));
    }

    [Theory]
    public void GivenAQuestionSetSelectionOptionQuestionHighlightModelData_WhenISetHighlightCondition_ThenHighlightConditionIsSet(
        QuestionSetSelectionOptionQuestionHighlightConditionType testHighlightCondition)
    {
        var testQuestionSetSelectionOptionQuestionHighlightModelData = new QuestionSetSelectionOptionQuestionHighlightModelData();

        testQuestionSetSelectionOptionQuestionHighlightModelData.QuestionSetSelectionOptionQuestionHighlight_HighlightCondition = testHighlightCondition;

        var result = testQuestionSetSelectionOptionQuestionHighlightModelData.QuestionSetSelectionOptionQuestionHighlight_HighlightCondition;

        Assert.That(result, Is.EqualTo(testHighlightCondition));
    }

    [Test]
    public void GivenAQuestionSetSelectionOptionQuestionHighlightModelData_WhenISetReasonHighlighted_ThenReasonHighlightedIsSet(
        [Values("", "  ", "abc")] string testReasonHighlighted)
    {
        var testQuestionSetSelectionOptionQuestionHighlightModelData = new QuestionSetSelectionOptionQuestionHighlightModelData();

        testQuestionSetSelectionOptionQuestionHighlightModelData.QuestionSetSelectionOptionQuestionHighlight_ReasonHighlighted = testReasonHighlighted;

        var result = testQuestionSetSelectionOptionQuestionHighlightModelData.QuestionSetSelectionOptionQuestionHighlight_ReasonHighlighted;

        Assert.That(result, Is.EqualTo(testReasonHighlighted));
    }
}