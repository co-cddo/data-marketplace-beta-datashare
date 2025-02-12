using Agrimetrics.DataShare.Api.Logic.ModelData.Questions.QuestionParts.OptionSelectionItems;
using Agrimetrics.DataShare.Api.Logic.ModelData.Questions.QuestionParts.ResponseFormats;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.ModelData.Questions.QuestionParts.ResponseFormats;

[TestFixture]
public class QuestionPartResponseFormatOptionSelectSingleValueModelDataTests
{
    [Test]
    public void GivenAQuestionPartResponseFormatOptionSelectSingleValueModelData_WhenIGetFormatType_ThenFormatTypeIsSelectSingle()
    {
        var testQuestionPartResponseFormatOptionSelectSingleValueModelData = new QuestionPartResponseFormatOptionSelectSingleValueModelData();

        var result = testQuestionPartResponseFormatOptionSelectSingleValueModelData.FormatType;

        Assert.That(result, Is.EqualTo(QuestionPartResponseFormatType.SelectSingle));
    }

    [Theory]
    public void GivenAQuestionPartResponseFormatOptionSelectSingleValueModelData_WhenISetFormatType_ThenFormatTypeIsSet(
        QuestionPartResponseFormatType testFormatType)
    {
        var testQuestionPartResponseFormatOptionSelectSingleValueModelData = new QuestionPartResponseFormatOptionSelectSingleValueModelData();

        testQuestionPartResponseFormatOptionSelectSingleValueModelData.FormatType = testFormatType;

        var result = testQuestionPartResponseFormatOptionSelectSingleValueModelData.FormatType;

        Assert.That(result, Is.EqualTo(testFormatType));
    }

    [Test]
    public void GivenAQuestionPartResponseFormatOptionSelectSingleValueModelData_WhenISetAnEmptySetOfSingleSelectionOptions_ThenSingleSelectionOptionsIsSet()
    {
        var testQuestionPartResponseFormatOptionSelectSingleValueModelData = new QuestionPartResponseFormatOptionSelectSingleValueModelData();

        var testSingleSelectionOptions = new List<QuestionPartOptionSelectionItemForSingleSelectionModelData>();

        testQuestionPartResponseFormatOptionSelectSingleValueModelData.ResponseFormatOptionSelectSingleValue_SingleSelectionOptions = testSingleSelectionOptions;

        var result = testQuestionPartResponseFormatOptionSelectSingleValueModelData.ResponseFormatOptionSelectSingleValue_SingleSelectionOptions;

        Assert.That(result, Is.EqualTo(testSingleSelectionOptions));
    }

    [Test]
    public void GivenAQuestionPartResponseFormatOptionSelectSingleValueModelData_WhenISetSingleSelectionOptions_ThenSingleSelectionOptionsIsSet()
    {
        var testQuestionPartResponseFormatOptionSelectSingleValueModelData = new QuestionPartResponseFormatOptionSelectSingleValueModelData();

        var testSingleSelectionOptions = new List<QuestionPartOptionSelectionItemForSingleSelectionModelData> { new(), new(), new() };

        testQuestionPartResponseFormatOptionSelectSingleValueModelData.ResponseFormatOptionSelectSingleValue_SingleSelectionOptions = testSingleSelectionOptions;

        var result = testQuestionPartResponseFormatOptionSelectSingleValueModelData.ResponseFormatOptionSelectSingleValue_SingleSelectionOptions;

        Assert.That(result, Is.EqualTo(testSingleSelectionOptions));
    }

    [Test]
    public void GivenAQuestionPartResponseFormatOptionSelectSingleValueModelData_WhenIGetResponseFormatOptionSelectOptionSelectionItems_ThenSingleSelectionOptionsAreReturned()
    {
        var testQuestionPartResponseFormatOptionSelectSingleValueModelData = new QuestionPartResponseFormatOptionSelectSingleValueModelData();

        var testSingleSelectionOptions = new List<QuestionPartOptionSelectionItemForSingleSelectionModelData> { new(), new(), new() };

        testQuestionPartResponseFormatOptionSelectSingleValueModelData.ResponseFormatOptionSelectSingleValue_SingleSelectionOptions = testSingleSelectionOptions;

        var result = testQuestionPartResponseFormatOptionSelectSingleValueModelData.GetResponseFormatOptionSelectOptionSelectionItems();

        Assert.That(result, Is.EqualTo(testSingleSelectionOptions));
    }
}