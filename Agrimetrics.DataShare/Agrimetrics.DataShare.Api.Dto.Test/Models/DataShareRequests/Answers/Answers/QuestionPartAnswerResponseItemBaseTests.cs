using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests.Answers.Answers;
using Agrimetrics.DataShare.Api.Dto.Models.Questions.QuestionParts.ResponseFormats;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Dto.Test.Models.DataShareRequests.Answers.Answers;

[TestFixture]
public class QuestionPartAnswerResponseItemBaseTests
{
    [Theory]
    public void GivenAQuestionPartAnswerResponseItemBase_WhenISetInputType_ThenInputTypeIsSet(
        QuestionPartResponseInputType testInputType)
    {
        var testQuestionPartAnswerResponseItemBase = new QuestionPartAnswerResponseItemBase();

        testQuestionPartAnswerResponseItemBase.InputType = testInputType;

        var result = testQuestionPartAnswerResponseItemBase.InputType;

        Assert.That(result, Is.EqualTo(testInputType));
    }
}