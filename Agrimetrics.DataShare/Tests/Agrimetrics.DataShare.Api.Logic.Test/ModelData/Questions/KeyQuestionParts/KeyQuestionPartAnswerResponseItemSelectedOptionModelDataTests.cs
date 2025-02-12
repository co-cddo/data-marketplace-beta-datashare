using Agrimetrics.DataShare.Api.Logic.ModelData.Questions.KeyQuestionParts;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.ModelData.Questions.KeyQuestionParts;

[TestFixture]
public class KeyQuestionPartAnswerResponseItemSelectedOptionModelDataTests
{
    [Test]
    public void GivenAKeyQuestionPartAnswerResponseItemSelectedOptionModelData_WhenISetSelectedOptionValue_ThenSelectedOptionValueIsSet(
        [Values("", " ", "abc")] string testSelectedOptionValue)
    {
        var testKeyQuestionPartAnswerResponseItemSelectedOptionModelData = new KeyQuestionPartAnswerResponseItemSelectedOptionModelData();

        testKeyQuestionPartAnswerResponseItemSelectedOptionModelData.KeyQuestionPartAnswerResponseItemSelectedOption_SelectedOptionValue = testSelectedOptionValue;

        var result = testKeyQuestionPartAnswerResponseItemSelectedOptionModelData.KeyQuestionPartAnswerResponseItemSelectedOption_SelectedOptionValue;

        Assert.That(result, Is.EqualTo(testSelectedOptionValue));
    }
}