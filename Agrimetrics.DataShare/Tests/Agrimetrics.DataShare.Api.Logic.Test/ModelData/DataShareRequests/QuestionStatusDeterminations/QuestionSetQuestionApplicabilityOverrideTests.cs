using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests.QuestionStatusDeterminations;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.ModelData.DataShareRequests.QuestionStatusDeterminations;

[TestFixture]
public class QuestionSetQuestionApplicabilityOverrideTests
{
    [Test]
    public void GivenAQuestionSetQuestionApplicabilityOverride_WhenISetControlledQuestionId_ThenControlledQuestionIdIsSet()
    {
        var testQuestionSetQuestionApplicabilityOverride = new QuestionSetQuestionApplicabilityOverride();
        
        var testControlledQuestionId = new Guid("E308E58A-8920-4048-9D4D-A2633E4A1C08");

        testQuestionSetQuestionApplicabilityOverride.QuestionSetQuestionApplicabilityOverride_ControlledQuestionId = testControlledQuestionId;

        var result = testQuestionSetQuestionApplicabilityOverride.QuestionSetQuestionApplicabilityOverride_ControlledQuestionId;

        Assert.That(result, Is.EqualTo(testControlledQuestionId));
    }

    [Test]
    public void GivenAQuestionSetQuestionApplicabilityOverride_WhenISetControllingSelectionOptionId_ThenControllingSelectionOptionIdIsSet()
    {
        var testQuestionSetQuestionApplicabilityOverride = new QuestionSetQuestionApplicabilityOverride();

        var testControllingSelectionOptionId = new Guid("E308E58A-8920-4048-9D4D-A2633E4A1C08");

        testQuestionSetQuestionApplicabilityOverride.QuestionSetQuestionApplicabilityOverride_ControllingSelectionOptionId = testControllingSelectionOptionId;

        var result = testQuestionSetQuestionApplicabilityOverride.QuestionSetQuestionApplicabilityOverride_ControllingSelectionOptionId;

        Assert.That(result, Is.EqualTo(testControllingSelectionOptionId));
    }

    [Theory]
    public void GivenAQuestionSetQuestionApplicabilityOverride_WhenISetControlledQuestionApplicabilityCondition_ThenControlledQuestionApplicabilityConditionIsSet(
        QuestionSetQuestionApplicabilityConditionType testControlledQuestionApplicabilityCondition)
    {
        var testQuestionSetQuestionApplicabilityOverride = new QuestionSetQuestionApplicabilityOverride();

        testQuestionSetQuestionApplicabilityOverride.QuestionSetQuestionApplicabilityOverride_ControlledQuestionApplicabilityCondition = testControlledQuestionApplicabilityCondition;

        var result = testQuestionSetQuestionApplicabilityOverride.QuestionSetQuestionApplicabilityOverride_ControlledQuestionApplicabilityCondition;

        Assert.That(result, Is.EqualTo(testControlledQuestionApplicabilityCondition));
    }

    [Theory]
    public void GivenAQuestionSetQuestionApplicabilityOverride_WhenISetControllingSelectionOptionIsSelected_ThenControllingSelectionOptionIsSelectedIsSet(
        bool testControllingSelectionOptionIsSelected)
    {
        var testQuestionSetQuestionApplicabilityOverride = new QuestionSetQuestionApplicabilityOverride();

        testQuestionSetQuestionApplicabilityOverride.QuestionSetQuestionApplicabilityOverride_ControllingSelectionOptionIsSelected = testControllingSelectionOptionIsSelected;

        var result = testQuestionSetQuestionApplicabilityOverride.QuestionSetQuestionApplicabilityOverride_ControllingSelectionOptionIsSelected;

        Assert.That(result, Is.EqualTo(testControllingSelectionOptionIsSelected));
    }
}