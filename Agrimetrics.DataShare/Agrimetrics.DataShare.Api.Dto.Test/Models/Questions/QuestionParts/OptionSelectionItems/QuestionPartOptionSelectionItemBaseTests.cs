using Agrimetrics.DataShare.Api.Dto.Models.Questions.QuestionParts;
using Agrimetrics.DataShare.Api.Dto.Models.Questions.QuestionParts.OptionSelectionItems;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Dto.Test.Models.Questions.QuestionParts.OptionSelectionItems;

[TestFixture]
public class QuestionPartOptionSelectionItemBaseTests
{
    [Test]
    public void GivenAQuestionPartOptionSelectionItemBase_WhenISetId_ThenIdIsSet()
    {
        var testQuestionPartOptionSelectionItemBase = new QuestionPartOptionSelectionItemBase();

        var testId = new Guid("FCEE7EDE-17F6-46A9-8AAF-8F7D5DD0EADC");

        testQuestionPartOptionSelectionItemBase.Id = testId;

        var result = testQuestionPartOptionSelectionItemBase.Id;

        Assert.That(result, Is.EqualTo(testId));
    }

    [Test]
    public void GivenAQuestionPartOptionSelectionItemBase_WhenISetValueText_ThenValueTextIsSet(
        [Values(null, "", "  ", "abc")] string? testValueText)
    {
        var testQuestionPartOptionSelectionItemBase = new QuestionPartOptionSelectionItemBase();

        testQuestionPartOptionSelectionItemBase.ValueText = testValueText;

        var result = testQuestionPartOptionSelectionItemBase.ValueText;

        Assert.That(result, Is.EqualTo(testValueText));
    }

    [Test]
    public void GivenAQuestionPartOptionSelectionItemBase_WhenISetHintText_ThenHintTextIsSet(
        [Values(null, "", "  ", "abc")] string? testHintText)
    {
        var testQuestionPartOptionSelectionItemBase = new QuestionPartOptionSelectionItemBase();

        testQuestionPartOptionSelectionItemBase.HintText = testHintText;

        var result = testQuestionPartOptionSelectionItemBase.HintText;

        Assert.That(result, Is.EqualTo(testHintText));
    }

    [Test]
    public void GivenAQuestionPartOptionSelectionItemBase_WhenISetOptionOrderWithinSelection_ThenOptionOrderWithinSelectionIsSet(
        [Values(-1, 0, 999)] int testOptionOrderWithinSelection)
    {
        var testQuestionPartOptionSelectionItemBase = new QuestionPartOptionSelectionItemBase();

        testQuestionPartOptionSelectionItemBase.OptionOrderWithinSelection = testOptionOrderWithinSelection;

        var result = testQuestionPartOptionSelectionItemBase.OptionOrderWithinSelection;

        Assert.That(result, Is.EqualTo(testOptionOrderWithinSelection));
    }

    [Test]
    public void GivenAQuestionPartOptionSelectionItemBase_WhenISetANullSupplementaryQuestion_ThenSupplementaryQuestionIsSet()
    {
        var testQuestionPartOptionSelectionItemBase = new QuestionPartOptionSelectionItemBase();

        var testSupplementaryQuestion = (QuestionPart?) null;

        testQuestionPartOptionSelectionItemBase.SupplementaryQuestion = testSupplementaryQuestion;

        var result = testQuestionPartOptionSelectionItemBase.SupplementaryQuestion;

        Assert.That(result, Is.EqualTo(testSupplementaryQuestion));
    }

    [Test]
    public void GivenAQuestionPartOptionSelectionItemBase_WhenISetSupplementaryQuestion_ThenSupplementaryQuestionIsSet()
    {
        var testQuestionPartOptionSelectionItemBase = new QuestionPartOptionSelectionItemBase();

        var testSupplementaryQuestion = new QuestionPart();

        testQuestionPartOptionSelectionItemBase.SupplementaryQuestion = testSupplementaryQuestion;

        var result = testQuestionPartOptionSelectionItemBase.SupplementaryQuestion;

        Assert.That(result, Is.SameAs(testSupplementaryQuestion));
    }
}
