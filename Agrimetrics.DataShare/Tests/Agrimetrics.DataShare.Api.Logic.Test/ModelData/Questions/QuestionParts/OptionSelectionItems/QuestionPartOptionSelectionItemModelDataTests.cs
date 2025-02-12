using Agrimetrics.DataShare.Api.Logic.ModelData.Questions.QuestionParts;
using Agrimetrics.DataShare.Api.Logic.ModelData.Questions.QuestionParts.OptionSelectionItems;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.ModelData.Questions.QuestionParts.OptionSelectionItems;

[TestFixture]
public class QuestionPartOptionSelectionItemModelDataTests
{
    [Test]
    public void GivenAQuestionPartOptionSelectionItemModelData_WhenISetId_ThenIdIsSet()
    {
        var testQuestionPartOptionSelectionItemModelData = new TestQuestionPartOptionSelectionItemModelData();

        var testId = new Guid("15E1D2F9-2AF2-4663-A9E5-6D4CFFD69609");

        testQuestionPartOptionSelectionItemModelData.OptionSelectionItem_Id = testId;

        var result = testQuestionPartOptionSelectionItemModelData.OptionSelectionItem_Id;

        Assert.That(result, Is.EqualTo(testId));
    }

    [Test]
    public void GivenAQuestionPartOptionSelectionItemModelData_WhenISetValueText_ThenValueTextIsSet(
        [Values("", "  ", "abc")] string testValueText)
    {
        var testQuestionPartOptionSelectionItemModelData = new TestQuestionPartOptionSelectionItemModelData();

        testQuestionPartOptionSelectionItemModelData.OptionSelectionItem_ValueText = testValueText;

        var result = testQuestionPartOptionSelectionItemModelData.OptionSelectionItem_ValueText;

        Assert.That(result, Is.EqualTo(testValueText));
    }

    [Test]
    public void GivenAQuestionPartOptionSelectionItemModelData_WhenISetHintText_ThenHintTextIsSet(
        [Values(null, "", "  ", "abc")] string? testHintText)
    {
        var testQuestionPartOptionSelectionItemModelData = new TestQuestionPartOptionSelectionItemModelData();

        testQuestionPartOptionSelectionItemModelData.OptionSelectionItem_HintText = testHintText;

        var result = testQuestionPartOptionSelectionItemModelData.OptionSelectionItem_HintText;

        Assert.That(result, Is.EqualTo(testHintText));
    }

    [Test]
    public void GivenAQuestionPartOptionSelectionItemModelData_WhenISetOptionOrderWithinSelection_ThenOptionOrderWithinSelectionIsSet(
        [Values(-1, 0, 999)] int testOptionOrderWithinSelection)
    {
        var testQuestionPartOptionSelectionItemModelData = new TestQuestionPartOptionSelectionItemModelData();

        testQuestionPartOptionSelectionItemModelData.OptionSelectionItem_OptionOrderWithinSelection = testOptionOrderWithinSelection;

        var result = testQuestionPartOptionSelectionItemModelData.OptionSelectionItem_OptionOrderWithinSelection;

        Assert.That(result, Is.EqualTo(testOptionOrderWithinSelection));
    }

    [Test]
    public void GivenAQuestionPartOptionSelectionItemModelData_WhenISetANullSupplementaryQuestionPartId_ThenSupplementaryQuestionPartIdIsSet()
    {
        var testQuestionPartOptionSelectionItemModelData = new TestQuestionPartOptionSelectionItemModelData();

        var testSupplementaryQuestionPartId = (Guid?) null;

        testQuestionPartOptionSelectionItemModelData.OptionSelectionItem_SupplementaryQuestionPartId = testSupplementaryQuestionPartId;

        var result = testQuestionPartOptionSelectionItemModelData.OptionSelectionItem_SupplementaryQuestionPartId;

        Assert.That(result, Is.EqualTo(testSupplementaryQuestionPartId));
    }

    [Test]
    public void GivenAQuestionPartOptionSelectionItemModelData_WhenISetSupplementaryQuestionPartId_ThenSupplementaryQuestionPartIdIsSet()
    {
        var testQuestionPartOptionSelectionItemModelData = new TestQuestionPartOptionSelectionItemModelData();

        var testSupplementaryQuestionPartId = new Guid("15E1D2F9-2AF2-4663-A9E5-6D4CFFD69609");

        testQuestionPartOptionSelectionItemModelData.OptionSelectionItem_SupplementaryQuestionPartId = testSupplementaryQuestionPartId;

        var result = testQuestionPartOptionSelectionItemModelData.OptionSelectionItem_SupplementaryQuestionPartId;

        Assert.That(result, Is.EqualTo(testSupplementaryQuestionPartId));
    }

    [Test]
    public void GivenAQuestionPartOptionSelectionItemModelData_WhenISetANullSupplementaryQuestionPart_ThenSupplementaryQuestionPartIsSet()
    {
        var testQuestionPartOptionSelectionItemModelData = new TestQuestionPartOptionSelectionItemModelData();

        var testSupplementaryQuestionPart = (QuestionPartModelData?) null;

        testQuestionPartOptionSelectionItemModelData.OptionSelectionItem_SupplementaryQuestionPart = testSupplementaryQuestionPart;

        var result = testQuestionPartOptionSelectionItemModelData.OptionSelectionItem_SupplementaryQuestionPart;

        Assert.That(result, Is.EqualTo(testSupplementaryQuestionPart));
    }

    [Test]
    public void GivenAQuestionPartOptionSelectionItemModelData_WhenISetSupplementaryQuestionPart_ThenSupplementaryQuestionPartIsSet()
    {
        var testQuestionPartOptionSelectionItemModelData = new TestQuestionPartOptionSelectionItemModelData();

        var testSupplementaryQuestionPart = new QuestionPartModelData();

        testQuestionPartOptionSelectionItemModelData.OptionSelectionItem_SupplementaryQuestionPart = testSupplementaryQuestionPart;

        var result = testQuestionPartOptionSelectionItemModelData.OptionSelectionItem_SupplementaryQuestionPart;

        Assert.That(result, Is.EqualTo(testSupplementaryQuestionPart));
    }

    private class TestQuestionPartOptionSelectionItemModelData : QuestionPartOptionSelectionItemModelData;
}