using Agrimetrics.DataShare.Api.Dto.Models.Questions.QuestionParts.OptionSelectionItems;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Dto.Test.Models.Questions.QuestionParts.OptionSelectionItems;

[TestFixture]
public class QuestionPartOptionSelectionItemForSingleSelectionTests
{
    [Theory]
    public void GivenAQuestionPartOptionSelectionItemForSingleSelection_WhenISetIsAlternativeAnswer_ThenIsAlternativeAnswerIsSet(
        bool testIsAlternativeAnswer)
    {
        var testQuestionPartOptionSelectionItemForSingleSelection = new QuestionPartOptionSelectionItemForSingleSelection();

        testQuestionPartOptionSelectionItemForSingleSelection.IsAlternativeAnswer = testIsAlternativeAnswer;

        var result = testQuestionPartOptionSelectionItemForSingleSelection.IsAlternativeAnswer;

        Assert.That(result, Is.EqualTo(testIsAlternativeAnswer));
    }
}