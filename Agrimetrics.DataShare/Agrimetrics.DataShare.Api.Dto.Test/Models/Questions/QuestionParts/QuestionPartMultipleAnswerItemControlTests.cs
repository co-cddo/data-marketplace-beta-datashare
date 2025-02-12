using Agrimetrics.DataShare.Api.Dto.Models.Questions.QuestionParts;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Dto.Test.Models.Questions.QuestionParts;

[TestFixture]
public class QuestionPartMultipleAnswerItemControlTests
{
    [Theory]
    public void GivenAQuestionPartMultipleAnswerItemControl_WhenISetMultipleAnswerItemsAreAllowed_ThenMultipleAnswerItemsAreAllowedIsSet(
        bool testMultipleAnswerItemsAreAllowed)
    {
        var testQuestionPartMultipleAnswerItemControl = new QuestionPartMultipleAnswerItemControl();

        testQuestionPartMultipleAnswerItemControl.MultipleAnswerItemsAreAllowed = testMultipleAnswerItemsAreAllowed;

        var result = testQuestionPartMultipleAnswerItemControl.MultipleAnswerItemsAreAllowed;

        Assert.That(result, Is.EqualTo(testMultipleAnswerItemsAreAllowed));
    }

    [Test]
    public void GivenAQuestionPartMultipleAnswerItemControl_WhenISetItemDescription_ThenItemDescriptionIsSet(
        [Values(null, "", "  ", "abc")] string? testItemDescription)
    {
        var testQuestionPartMultipleAnswerItemControl = new QuestionPartMultipleAnswerItemControl();

        testQuestionPartMultipleAnswerItemControl.ItemDescription = testItemDescription;

        var result = testQuestionPartMultipleAnswerItemControl.ItemDescription;

        Assert.That(result, Is.EqualTo(testItemDescription));
    }

    [Test]
    public void GivenAQuestionPartMultipleAnswerItemControl_WhenISetCollectionDescription_ThenCollectionDescriptionIsSet(
        [Values(null, "", "  ", "abc")] string? testCollectionDescription)
    {
        var testQuestionPartMultipleAnswerItemControl = new QuestionPartMultipleAnswerItemControl();

        testQuestionPartMultipleAnswerItemControl.CollectionDescription = testCollectionDescription;

        var result = testQuestionPartMultipleAnswerItemControl.CollectionDescription;

        Assert.That(result, Is.EqualTo(testCollectionDescription));
    }
}