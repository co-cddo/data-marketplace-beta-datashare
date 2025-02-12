using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests.Answers.DataShareRequestQuestionAnswers;
using Agrimetrics.DataShare.Api.Logic.ModelData.Questions.QuestionParts.ResponseFormats;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.ModelData.DataShareRequests.Answers.DsrRequestQuestionAnswers;

[TestFixture]
public class QuestionPartAnswerResponseItemModelDataTests
{
    [Theory]
    public void GivenAQuestionPartAnswerResponseItemModelData_WhenISetInputType_ThenInputTypeIsSet(
        QuestionPartResponseInputType testInputType)
    {
        var testQuestionPartAnswerResponseItemModelData = new TestQuestionPartAnswerResponseItemModelData();

        testQuestionPartAnswerResponseItemModelData.QuestionPartAnswerItem_InputType = testInputType;

        var result = testQuestionPartAnswerResponseItemModelData.QuestionPartAnswerItem_InputType;

        Assert.That(result, Is.EqualTo(testInputType));
    }

    [Test]
    public void GivenAQuestionPartAnswerResponseItemModelData_WhenISetId_ThenIdIsSet()
    {
        var testQuestionPartAnswerResponseItemModelData = new TestQuestionPartAnswerResponseItemModelData();

        var testId = new Guid("B9BFF13B-99D5-437E-A538-0D44A620172A");

        testQuestionPartAnswerResponseItemModelData.QuestionPartAnswerItem_Id = testId;

        var result = testQuestionPartAnswerResponseItemModelData.QuestionPartAnswerItem_Id;

        Assert.That(result, Is.EqualTo(testId));
    }

    private class TestQuestionPartAnswerResponseItemModelData : QuestionPartAnswerResponseItemModelData;
}