using Agrimetrics.DataShare.Api.Logic.ModelData.Questions.QuestionParts;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.ModelData.Questions.QuestionParts;

[TestFixture]
public class QuestionPartMultipleAnswerItemControlModelDataTests
{
    [Test]
    public void GivenAQuestionPartMultipleAnswerItemControlModelData_WhenISetQuestionPartId_ThenQuestionPartIdIsSet()
    {
        var testQuestionPartMultipleAnswerItemControlModelData = new QuestionPartMultipleAnswerItemControlModelData();

        var testQuestionPartId = new Guid("B3263990-3302-4D13-9515-770A01B1E113");

        testQuestionPartMultipleAnswerItemControlModelData.QuestionPartMultipleAnswerItemControl_QuestionPartId = testQuestionPartId;

        var result = testQuestionPartMultipleAnswerItemControlModelData.QuestionPartMultipleAnswerItemControl_QuestionPartId;

        Assert.That(result, Is.EqualTo(testQuestionPartId));
    }

    [Theory]
    public void GivenAQuestionPartMultipleAnswerItemControlModelData_WhenISetMultipleAnswerItemsAreAllowed_ThenMultipleAnswerItemsAreAllowedIsSet(
        bool testMultipleAnswerItemsAreAllowed)
    {
        var testQuestionPartMultipleAnswerItemControlModelData = new QuestionPartMultipleAnswerItemControlModelData();

        testQuestionPartMultipleAnswerItemControlModelData.QuestionPartMultipleAnswerItemControl_MultipleAnswerItemsAreAllowed = testMultipleAnswerItemsAreAllowed;

        var result = testQuestionPartMultipleAnswerItemControlModelData.QuestionPartMultipleAnswerItemControl_MultipleAnswerItemsAreAllowed;

        Assert.That(result, Is.EqualTo(testMultipleAnswerItemsAreAllowed));
    }

    [Test]
    public void GivenAQuestionPartMultipleAnswerItemControlModelData_WhenISetItemDescription_ThenItemDescriptionIsSet(
        [Values(null, "", "  ", "abc")] string? testItemDescription)
    {
        var testQuestionPartMultipleAnswerItemControlModelData = new QuestionPartMultipleAnswerItemControlModelData();

        testQuestionPartMultipleAnswerItemControlModelData.QuestionPartMultipleAnswerItemControl_ItemDescription = testItemDescription;

        var result = testQuestionPartMultipleAnswerItemControlModelData.QuestionPartMultipleAnswerItemControl_ItemDescription;

        Assert.That(result, Is.EqualTo(testItemDescription));
    }

    [Test]
    public void GivenAQuestionPartMultipleAnswerItemControlModelData_WhenISetCollectionDescription_ThenCollectionDescriptionIsSet(
        [Values(null, "", "  ", "abc")] string? testCollectionDescription)
    {
        var testQuestionPartMultipleAnswerItemControlModelData = new QuestionPartMultipleAnswerItemControlModelData();

        testQuestionPartMultipleAnswerItemControlModelData.QuestionPartMultipleAnswerItemControl_CollectionDescription = testCollectionDescription;

        var result = testQuestionPartMultipleAnswerItemControlModelData.QuestionPartMultipleAnswerItemControl_CollectionDescription;

        Assert.That(result, Is.EqualTo(testCollectionDescription));
    }
}