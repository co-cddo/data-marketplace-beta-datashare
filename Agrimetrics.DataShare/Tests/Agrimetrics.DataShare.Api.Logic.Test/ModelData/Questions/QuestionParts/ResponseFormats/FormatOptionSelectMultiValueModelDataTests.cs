using Agrimetrics.DataShare.Api.Logic.ModelData.Questions.QuestionParts.OptionSelectionItems;
using Agrimetrics.DataShare.Api.Logic.ModelData.Questions.QuestionParts.ResponseFormats;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.ModelData.Questions.QuestionParts.ResponseFormats;

[TestFixture]
public class QuestionPartResponseFormatOptionSelectMultiValueModelDataTests
{
    [Test]
    public void GivenAQuestionPartResponseFormatOptionSelectMultiValueModelData_WhenIGetFormatType_ThenFormatTypeIsSelectMulti()
    {
        var testQuestionPartResponseFormatOptionSelectMultiValueModelData = new QuestionPartResponseFormatOptionSelectMultiValueModelData();

        var result = testQuestionPartResponseFormatOptionSelectMultiValueModelData.FormatType;

        Assert.That(result, Is.EqualTo(QuestionPartResponseFormatType.SelectMulti));
    }

    [Theory]
    public void GivenAQuestionPartResponseFormatOptionSelectMultiValueModelData_WhenISetFormatType_ThenFormatTypeIsSet(
        QuestionPartResponseFormatType testFormatType)
    {
        var testQuestionPartResponseFormatOptionSelectMultiValueModelData = new QuestionPartResponseFormatOptionSelectMultiValueModelData();

        testQuestionPartResponseFormatOptionSelectMultiValueModelData.FormatType = testFormatType;

        var result = testQuestionPartResponseFormatOptionSelectMultiValueModelData.FormatType;

        Assert.That(result, Is.EqualTo(testFormatType));
    }

    [Test]
    public void GivenAQuestionPartResponseFormatOptionSelectMultiValueModelData_WhenISetAnEmptySetOfMultiSelectionOptions_ThenMultiSelectionOptionsIsSet()
    {
        var testQuestionPartResponseFormatOptionSelectMultiValueModelData = new QuestionPartResponseFormatOptionSelectMultiValueModelData();

        var testMultiSelectionOptions = new List<QuestionPartOptionSelectionItemForMultiSelectionModelData>();

        testQuestionPartResponseFormatOptionSelectMultiValueModelData.ResponseFormatOptionSelectMultiValue_MultiSelectionOptions = testMultiSelectionOptions;

        var result = testQuestionPartResponseFormatOptionSelectMultiValueModelData.ResponseFormatOptionSelectMultiValue_MultiSelectionOptions;

        Assert.That(result, Is.EqualTo(testMultiSelectionOptions));
    }

    [Test]
    public void GivenAQuestionPartResponseFormatOptionSelectMultiValueModelData_WhenISetMultiSelectionOptions_ThenMultiSelectionOptionsIsSet()
    {
        var testQuestionPartResponseFormatOptionSelectMultiValueModelData = new QuestionPartResponseFormatOptionSelectMultiValueModelData();

        var testMultiSelectionOptions = new List<QuestionPartOptionSelectionItemForMultiSelectionModelData> {new(), new(), new()};

        testQuestionPartResponseFormatOptionSelectMultiValueModelData.ResponseFormatOptionSelectMultiValue_MultiSelectionOptions = testMultiSelectionOptions;

        var result = testQuestionPartResponseFormatOptionSelectMultiValueModelData.ResponseFormatOptionSelectMultiValue_MultiSelectionOptions;

        Assert.That(result, Is.EqualTo(testMultiSelectionOptions));
    }

    [Test]
    public void GivenAQuestionPartResponseFormatOptionSelectMultiValueModelData_WhenIGetResponseFormatOptionSelectOptionSelectionItems_ThenMultiSelectionOptionsAreReturned()
    {
        var testQuestionPartResponseFormatOptionSelectMultiValueModelData = new QuestionPartResponseFormatOptionSelectMultiValueModelData();

        var testMultiSelectionOptions = new List<QuestionPartOptionSelectionItemForMultiSelectionModelData> { new(), new(), new() };

        testQuestionPartResponseFormatOptionSelectMultiValueModelData.ResponseFormatOptionSelectMultiValue_MultiSelectionOptions = testMultiSelectionOptions;

        var result = testQuestionPartResponseFormatOptionSelectMultiValueModelData.GetResponseFormatOptionSelectOptionSelectionItems();

        Assert.That(result, Is.EqualTo(testMultiSelectionOptions));
    }
}