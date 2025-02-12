using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests.Answers.DataShareRequestAnswerSummaries;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.ModelData.DataShareRequests.Answers.DsrAnswerSummaries;

[TestFixture]
public class DataShareRequestAnswersSummaryQuestionQuestionPartIdModelDataTests
{
    [Test]
    public void GivenADataShareRequestAnswersSummaryQuestionQuestionPartIdModelData_WhenISetQuestionPartId_ThenQuestionPartIdIsSet()
    {
        var testDataShareRequestAnswersSummaryQuestionQuestionPartIdModelData = new DataShareRequestAnswersSummaryQuestionQuestionPartIdModelData();

        var testQuestionPartId = new Guid("7318B894-2E68-43FC-91B0-A76AC841BDAE");

        testDataShareRequestAnswersSummaryQuestionQuestionPartIdModelData.DataShareRequestAnswersSummaryQuestion_QuestionPartId = testQuestionPartId;

        var result = testDataShareRequestAnswersSummaryQuestionQuestionPartIdModelData.DataShareRequestAnswersSummaryQuestion_QuestionPartId;

        Assert.That(result, Is.EqualTo(testQuestionPartId));
    }
}