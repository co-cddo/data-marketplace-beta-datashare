using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests.QuestionStatusDeterminations;
using Agrimetrics.DataShare.Api.Logic.ModelData.Questions.QuestionParts.ResponseFormats;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.ModelData.DataShareRequests.QuestionStatusDeterminations;

[TestFixture]
public class SupplementaryQuestionAnswerPartModelDataTests
{
    [Test]
    public void GivenASupplementaryQuestionAnswerPartModelData_WhenISetAnswerPartId_ThenAnswerPartIdIsSet()
    {
        var testSupplementaryQuestionAnswerPartModelData = new SupplementaryQuestionAnswerPartModelData();

        var testAnswerPartId = new Guid("FE9CD6C1-6B7C-4458-A2CF-F6632C4F7E96");

        testSupplementaryQuestionAnswerPartModelData.SupplementaryAnswerPart_AnswerPartId = testAnswerPartId;

        var result = testSupplementaryQuestionAnswerPartModelData.SupplementaryAnswerPart_AnswerPartId;

        Assert.That(result, Is.EqualTo(testAnswerPartId));
    }

    [Theory]
    public void GivenASupplementaryQuestionAnswerPartModelData_WhenISetInputType_ThenInputTypeIsSet(
        QuestionPartResponseInputType testInputType)
    {
        var testSupplementaryQuestionAnswerPartModelData = new SupplementaryQuestionAnswerPartModelData();

        testSupplementaryQuestionAnswerPartModelData.SupplementaryAnswerPart_InputType = testInputType;

        var result = testSupplementaryQuestionAnswerPartModelData.SupplementaryAnswerPart_InputType;

        Assert.That(result, Is.EqualTo(testInputType));
    }
}