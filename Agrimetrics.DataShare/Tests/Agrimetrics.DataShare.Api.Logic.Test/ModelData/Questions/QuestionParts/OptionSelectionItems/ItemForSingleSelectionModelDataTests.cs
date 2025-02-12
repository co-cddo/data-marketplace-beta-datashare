using Agrimetrics.DataShare.Api.Logic.ModelData.Questions.QuestionParts.OptionSelectionItems;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.ModelData.Questions.QuestionParts.OptionSelectionItems;

[TestFixture]
public class QuestionPartOptionSelectionItemForSingleSelectionModelDataTests
{
    [Theory]
    public void GivenAQuestionPartOptionSelectionItemForSingleSelectionModelData_WhenISetIsAlternativeAnswer_ThenIsAlternativeAnswerIsSet(
        bool testIsAlternativeAnswer)
    {
        var testQuestionPartOptionSelectionItemForSingleSelectionModelData = new QuestionPartOptionSelectionItemForSingleSelectionModelData();

        testQuestionPartOptionSelectionItemForSingleSelectionModelData.SingleSelectionOption_IsAlternativeAnswer = testIsAlternativeAnswer;

        var result = testQuestionPartOptionSelectionItemForSingleSelectionModelData.SingleSelectionOption_IsAlternativeAnswer;

        Assert.That(result, Is.EqualTo(testIsAlternativeAnswer));
    }
}