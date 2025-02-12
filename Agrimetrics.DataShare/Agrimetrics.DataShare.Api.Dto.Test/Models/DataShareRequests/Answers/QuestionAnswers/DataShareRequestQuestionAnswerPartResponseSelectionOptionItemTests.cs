using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests.Answers.DataShareRequestQuestionAnswers;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Dto.Test.Models.DataShareRequests.Answers.DataShareRequestQuestionAnswers;

[TestFixture]
public class DataShareRequestQuestionAnswerPartResponseSelectionOptionItemTests
{
    [Test]
    public void GivenADataShareRequestQuestionAnswerPartResponseSelectionOptionItem_WhenISetOptionSelectionItemId_ThenOptionSelectionItemIdIsSet()
    {
        var testDataShareRequestQuestionAnswerPartResponseSelectionOptionItem = new DataShareRequestQuestionAnswerPartResponseSelectionOptionItem();

        var testOptionSelectionItemId = new Guid("DEED9E12-5F74-42B1-9F20-2E7A57692384");

        testDataShareRequestQuestionAnswerPartResponseSelectionOptionItem.OptionSelectionItemId = testOptionSelectionItemId;

        var result = testDataShareRequestQuestionAnswerPartResponseSelectionOptionItem.OptionSelectionItemId;

        Assert.That(result, Is.EqualTo(testOptionSelectionItemId));
    }

    [Test]
    public void GivenADataShareRequestQuestionAnswerPartResponseSelectionOptionItem_WhenISetANullSupplementaryQuestionAnswerPart_ThenSupplementaryQuestionAnswerPartIsSet()
    {
        var testDataShareRequestQuestionAnswerPartResponseSelectionOptionItem = new DataShareRequestQuestionAnswerPartResponseSelectionOptionItem();

        var testSupplementaryQuestionAnswerPart = (DataShareRequestQuestionAnswerPart?) null;

        testDataShareRequestQuestionAnswerPartResponseSelectionOptionItem.SupplementaryQuestionAnswerPart = testSupplementaryQuestionAnswerPart;

        var result = testDataShareRequestQuestionAnswerPartResponseSelectionOptionItem.SupplementaryQuestionAnswerPart;

        Assert.That(result, Is.EqualTo(testSupplementaryQuestionAnswerPart));
    }

    [Test]
    public void GivenADataShareRequestQuestionAnswerPartResponseSelectionOptionItem_WhenISetSupplementaryQuestionAnswerPart_ThenSupplementaryQuestionAnswerPartIsSet()
    {
        var testDataShareRequestQuestionAnswerPartResponseSelectionOptionItem = new DataShareRequestQuestionAnswerPartResponseSelectionOptionItem();

        var testSupplementaryQuestionAnswerPart = new DataShareRequestQuestionAnswerPart();

        testDataShareRequestQuestionAnswerPartResponseSelectionOptionItem.SupplementaryQuestionAnswerPart = testSupplementaryQuestionAnswerPart;

        var result = testDataShareRequestQuestionAnswerPartResponseSelectionOptionItem.SupplementaryQuestionAnswerPart;

        Assert.That(result, Is.SameAs(testSupplementaryQuestionAnswerPart));
    }
}