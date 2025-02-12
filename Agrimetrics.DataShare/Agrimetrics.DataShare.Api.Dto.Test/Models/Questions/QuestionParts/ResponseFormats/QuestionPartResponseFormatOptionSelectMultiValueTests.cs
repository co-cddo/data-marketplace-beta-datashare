using Agrimetrics.DataShare.Api.Dto.Models.Questions.QuestionParts.OptionSelectionItems;
using Agrimetrics.DataShare.Api.Dto.Models.Questions.QuestionParts.ResponseFormats;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Dto.Test.Models.Questions.QuestionParts.ResponseFormats;

[TestFixture]
public class QuestionPartResponseFormatOptionSelectMultiValueTests
{
    [Test]
    public void GivenAQuestionPartResponseFormatOptionSelectMultiValue_WhenIGetFormatType_ThenFormatTypeIsSelectMulti()
    {
        var testQuestionPartResponseFormatOptionSelectMultiValue = new QuestionPartResponseFormatOptionSelectMultiValue();

        var result = testQuestionPartResponseFormatOptionSelectMultiValue.FormatType;

        Assert.That(result, Is.EqualTo(QuestionPartResponseFormatType.SelectMulti));
    }

    [Theory]
    public void GivenAQuestionPartResponseFormatOptionSelectMultiValue_WhenISetFormatType_ThenFormatTypeIsSet(
        QuestionPartResponseFormatType testFormatType)
    {
        var testQuestionPartResponseFormatOptionSelectMultiValue = new QuestionPartResponseFormatOptionSelectMultiValue();

        testQuestionPartResponseFormatOptionSelectMultiValue.FormatType = testFormatType;

        var result = testQuestionPartResponseFormatOptionSelectMultiValue.FormatType;

        Assert.That(result, Is.EqualTo(testFormatType));
    }

    [Test]
    public void GivenAQuestionPartResponseFormatOptionSelectMultiValue_WhenISetANullSetOfMultiSelectionOptions_ThenMultiSelectionOptionsIsSet()
    {
        var testQuestionPartResponseFormatOptionSelectMultiValue = new QuestionPartResponseFormatOptionSelectMultiValue();

        var testMultiSelectionOptions = (List<QuestionPartOptionSelectionItemForMultiSelection>?) null;

        testQuestionPartResponseFormatOptionSelectMultiValue.MultiSelectionOptions = testMultiSelectionOptions;

        var result = testQuestionPartResponseFormatOptionSelectMultiValue.MultiSelectionOptions;

        Assert.That(result, Is.EqualTo(testMultiSelectionOptions));
    }

    [Test]
    public void GivenAQuestionPartResponseFormatOptionSelectMultiValue_WhenISetMultiSelectionOptions_ThenMultiSelectionOptionsIsSet()
    {
        var testQuestionPartResponseFormatOptionSelectMultiValue = new QuestionPartResponseFormatOptionSelectMultiValue();

        var testMultiSelectionOptions = new List<QuestionPartOptionSelectionItemForMultiSelection> {new(), new(), new()};

        testQuestionPartResponseFormatOptionSelectMultiValue.MultiSelectionOptions = testMultiSelectionOptions;

        var result = testQuestionPartResponseFormatOptionSelectMultiValue.MultiSelectionOptions;

        Assert.That(result, Is.EqualTo(testMultiSelectionOptions));
    }
}