using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests.Answers.DataShareRequestAnswerSummaries;
using Agrimetrics.DataShare.Api.Dto.Models.Questions.QuestionParts.ResponseFormats;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Dto.Test.Models.DataShareRequests.Answers.DataShareRequestAnswerSummaries;

[TestFixture]
public class DataShareRequestAnswersSummaryQuestionPartAnswerResponseItemBaseTests
{
    [Theory]
    public void GivenADataShareRequestAnswersSummaryQuestionPartAnswerResponseItemBase_WhenISetInputType_ThenInputTypeIsSet(
        QuestionPartResponseInputType testInputType)
    {
        var testDataShareRequestAnswersSummaryQuestionPartAnswerResponseItemBase = new DataShareRequestAnswersSummaryQuestionPartAnswerResponseItemBase();

        testDataShareRequestAnswersSummaryQuestionPartAnswerResponseItemBase.InputType = testInputType;

        var result = testDataShareRequestAnswersSummaryQuestionPartAnswerResponseItemBase.InputType;

        Assert.That(result, Is.EqualTo(testInputType));
    }
}