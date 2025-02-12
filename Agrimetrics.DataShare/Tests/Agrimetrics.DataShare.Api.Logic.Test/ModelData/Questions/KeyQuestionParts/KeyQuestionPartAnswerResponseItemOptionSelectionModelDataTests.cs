using Agrimetrics.DataShare.Api.Logic.ModelData.Questions.KeyQuestionParts;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.ModelData.Questions.KeyQuestionParts;

[TestFixture]
public class KeyQuestionPartAnswerResponseItemOptionSelectionModelDataTests
{
    [Test]
    public void GivenAKeyQuestionPartAnswerResponseItemOptionSelectionModelData_WhenISetResponseItemId_ThenResponseItemIdIsSet()
    {
        var testKeyQuestionPartAnswerResponseItemOptionSelectionModelData = new KeyQuestionPartAnswerResponseItemOptionSelectionModelData();

        var testResponseItemId = new Guid("15E1D2F9-2AF2-4663-A9E5-6D4CFFD69609");

        testKeyQuestionPartAnswerResponseItemOptionSelectionModelData.KeyQuestionPartAnswerResponseItemOptionSelection_ResponseItemId = testResponseItemId;

        var result = testKeyQuestionPartAnswerResponseItemOptionSelectionModelData.KeyQuestionPartAnswerResponseItemOptionSelection_ResponseItemId;

        Assert.That(result, Is.EqualTo(testResponseItemId));
    }

    [Test]
    public void GivenAKeyQuestionPartAnswerResponseItemOptionSelectionModelData_WhenISetAnEmptySetOfSelectedOptions_ThenSelectedOptionsIsSet()
    {
        var testKeyQuestionPartAnswerResponseItemOptionSelectionModelData = new KeyQuestionPartAnswerResponseItemOptionSelectionModelData();

        var testSelectedOptions = new List<KeyQuestionPartAnswerResponseItemSelectedOptionModelData>();

        testKeyQuestionPartAnswerResponseItemOptionSelectionModelData.KeyQuestionPartAnswerResponseItemOptionSelection_SelectedOptions = testSelectedOptions;

        var result = testKeyQuestionPartAnswerResponseItemOptionSelectionModelData.KeyQuestionPartAnswerResponseItemOptionSelection_SelectedOptions;

        Assert.That(result, Is.EqualTo(testSelectedOptions));
    }

    [Test]
    public void GivenAKeyQuestionPartAnswerResponseItemOptionSelectionModelData_WhenISetSelectedOptions_ThenSelectedOptionsIsSet()
    {
        var testKeyQuestionPartAnswerResponseItemOptionSelectionModelData = new KeyQuestionPartAnswerResponseItemOptionSelectionModelData();

        var testSelectedOptions = new List<KeyQuestionPartAnswerResponseItemSelectedOptionModelData> {new(), new(), new()};

        testKeyQuestionPartAnswerResponseItemOptionSelectionModelData.KeyQuestionPartAnswerResponseItemOptionSelection_SelectedOptions = testSelectedOptions;

        var result = testKeyQuestionPartAnswerResponseItemOptionSelectionModelData.KeyQuestionPartAnswerResponseItemOptionSelection_SelectedOptions;

        Assert.That(result, Is.EqualTo(testSelectedOptions));
    }
}