using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests.Answers.Answers;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Dto.Test.Models.DataShareRequests.Answers.Answers;

[TestFixture]
public class QuestionPartAnswerItemSelectionOptionItemTests
{
    [Test]
    public void GivenAQuestionPartAnswerItemSelectionOptionItem_WhenISetOptionSelectionItemId_ThenQuestionPartIdIsSet()
    {
        var testQuestionPartAnswerItemSelectionOptionItem = new QuestionPartAnswerItemSelectionOptionItem();

        var testOptionSelectionItemId = new Guid("C483EAD0-CA65-43C8-8563-758BC611D592");

        testQuestionPartAnswerItemSelectionOptionItem.OptionSelectionItemId = testOptionSelectionItemId;

        var result = testQuestionPartAnswerItemSelectionOptionItem.OptionSelectionItemId;

        Assert.That(result, Is.EqualTo(testOptionSelectionItemId));
    }

    [Test]
    public void GivenAQuestionPartAnswerItemSelectionOptionItem_WhenISetANullSupplementaryQuestionPartAnswer_ThenSupplementaryQuestionPartAnswerIsSet()
    {
        var testQuestionPartAnswerItemSelectionOptionItem = new QuestionPartAnswerItemSelectionOptionItem();

        var testSupplementaryQuestionPartAnswer = (QuestionPartAnswer?) null;

        testQuestionPartAnswerItemSelectionOptionItem.SupplementaryQuestionPartAnswer = null;

        var result = testQuestionPartAnswerItemSelectionOptionItem.SupplementaryQuestionPartAnswer;

        Assert.That(result, Is.EqualTo(testSupplementaryQuestionPartAnswer));
    }

    [Test]
    public void GivenAQuestionPartAnswerItemSelectionOptionItem_WhenISetSupplementaryQuestionPartAnswer_ThenSupplementaryQuestionPartAnswerIsSet()
    {
        var testQuestionPartAnswerItemSelectionOptionItem = new QuestionPartAnswerItemSelectionOptionItem();

        var testSupplementaryQuestionPartAnswer = new QuestionPartAnswer();

        testQuestionPartAnswerItemSelectionOptionItem.SupplementaryQuestionPartAnswer =
            testSupplementaryQuestionPartAnswer;

        var result = testQuestionPartAnswerItemSelectionOptionItem.SupplementaryQuestionPartAnswer;

        Assert.That(result, Is.SameAs(testSupplementaryQuestionPartAnswer));
    }
}