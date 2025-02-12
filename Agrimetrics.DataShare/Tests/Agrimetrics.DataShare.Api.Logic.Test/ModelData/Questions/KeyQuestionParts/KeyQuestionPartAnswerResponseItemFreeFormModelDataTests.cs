using Agrimetrics.DataShare.Api.Logic.ModelData.Questions.KeyQuestionParts;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.ModelData.Questions.KeyQuestionParts;

[TestFixture]
public class KeyQuestionPartAnswerResponseItemFreeFormModelDataTests
{
    [Test]
    public void GivenAKeyQuestionPartAnswerResponseItemFreeFormModelData_WhenISetEnteredValue_ThenEnteredValueIsSet(
        [Values("", "   ", "abc")] string testEnteredValue)
    {
        var testKeyQuestionPartAnswerResponseItemFreeFormModelData = new KeyQuestionPartAnswerResponseItemFreeFormModelData();

        testKeyQuestionPartAnswerResponseItemFreeFormModelData.KeyQuestionPartAnswerResponseItemFreeForm_EnteredValue = testEnteredValue;

        var result = testKeyQuestionPartAnswerResponseItemFreeFormModelData.KeyQuestionPartAnswerResponseItemFreeForm_EnteredValue;

        Assert.That(result, Is.EqualTo(testEnteredValue));
    }
}