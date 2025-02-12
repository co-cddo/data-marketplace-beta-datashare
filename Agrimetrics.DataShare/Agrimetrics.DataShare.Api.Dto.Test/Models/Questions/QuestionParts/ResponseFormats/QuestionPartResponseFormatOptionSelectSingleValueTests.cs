using Agrimetrics.DataShare.Api.Dto.Models.Questions.QuestionParts.OptionSelectionItems;
using Agrimetrics.DataShare.Api.Dto.Models.Questions.QuestionParts.ResponseFormats;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Dto.Test.Models.Questions.QuestionParts.ResponseFormats;

[TestFixture]
public class QuestionPartResponseFormatOptionSelectSingleValueTests
{
    [Test]
    public void GivenAQuestionPartResponseFormatOptionSelectSingleValue_WhenIGetFormatType_ThenFormatTypeIsSelectSingle()
    {
        var testQuestionPartResponseFormatOptionSelectSingleValue = new QuestionPartResponseFormatOptionSelectSingleValue();

        var result = testQuestionPartResponseFormatOptionSelectSingleValue.FormatType;

        Assert.That(result, Is.EqualTo(QuestionPartResponseFormatType.SelectSingle));
    }

    [Theory]
    public void GivenAQuestionPartResponseFormatOptionSelectSingleValue_WhenISetFormatType_ThenFormatTypeIsSet(
        QuestionPartResponseFormatType testFormatType)
    {
        var testQuestionPartResponseFormatOptionSelectSingleValue = new QuestionPartResponseFormatOptionSelectSingleValue();

        testQuestionPartResponseFormatOptionSelectSingleValue.FormatType = testFormatType;

        var result = testQuestionPartResponseFormatOptionSelectSingleValue.FormatType;

        Assert.That(result, Is.EqualTo(testFormatType));
    }

    [Test]
    public void GivenAQuestionPartResponseFormatOptionSelectSingleValue_WhenISetAnEmptySetOfSingleSelectionOptions_ThenSingleSelectionOptionsIsSet()
    {
        var testQuestionPartResponseFormatOptionSelectSingleValue = new QuestionPartResponseFormatOptionSelectSingleValue();

        var testSingleSelectionOptions = new List<QuestionPartOptionSelectionItemForSingleSelection>();

        testQuestionPartResponseFormatOptionSelectSingleValue.SingleSelectionOptions = testSingleSelectionOptions;

        var result = testQuestionPartResponseFormatOptionSelectSingleValue.SingleSelectionOptions;

        Assert.That(result, Is.EqualTo(testSingleSelectionOptions));
    }

    [Test]
    public void GivenAQuestionPartResponseFormatOptionSelectSingleValue_WhenISetSingleSelectionOptions_ThenSingleSelectionOptionsIsSet()
    {
        var testQuestionPartResponseFormatOptionSelectSingleValue = new QuestionPartResponseFormatOptionSelectSingleValue();

        var testSingleSelectionOptions = new List<QuestionPartOptionSelectionItemForSingleSelection> { new(), new(), new() };

        testQuestionPartResponseFormatOptionSelectSingleValue.SingleSelectionOptions = testSingleSelectionOptions;

        var result = testQuestionPartResponseFormatOptionSelectSingleValue.SingleSelectionOptions;

        Assert.That(result, Is.EqualTo(testSingleSelectionOptions));
    }
}