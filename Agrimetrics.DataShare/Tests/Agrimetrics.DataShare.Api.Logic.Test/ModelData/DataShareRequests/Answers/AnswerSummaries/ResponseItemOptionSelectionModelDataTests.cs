using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests.Answers.DataShareRequestAnswerSummaries;
using Agrimetrics.DataShare.Api.Logic.ModelData.Questions.QuestionParts.ResponseFormats;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.ModelData.DataShareRequests.Answers.DsrAnswerSummaries;

[TestFixture]
public class DataShareRequestAnswersSummaryQuestionPartAnswerResponseItemOptionSelectionModelDataTests
{
    [Test]
    public void GivenADataShareRequestAnswersSummaryQuestionPartAnswerResponseItemOptionSelectionModelData_WhenIGetResponseInputType_ThenResponseInputTypeIsOptionSelection()
    {
        var testDataShareRequestAnswersSummaryQuestionPartAnswerResponseItemOptionSelectionModelData = new DataShareRequestAnswersSummaryQuestionPartAnswerResponseItemOptionSelectionModelData();

        var result = testDataShareRequestAnswersSummaryQuestionPartAnswerResponseItemOptionSelectionModelData.DataShareRequestAnswersSummaryQuestionPartAnswerResponseItem_ResponseInputType;

        Assert.That(result, Is.EqualTo(QuestionPartResponseInputType.OptionSelection));
    }

    [Test]
    public void GivenADataShareRequestAnswersSummaryQuestionPartAnswerResponseItemOptionSelectionModelData_WhenISetSelectedOptions_ThenSelectedOptionsIsSet()
    {
        var testDataShareRequestAnswersSummaryQuestionPartAnswerResponseItemOptionSelectionModelData = new DataShareRequestAnswersSummaryQuestionPartAnswerResponseItemOptionSelectionModelData();

        var testSelectedOptions = new List<DataShareRequestAnswersSummaryQuestionPartAnswerResponseOptionSelectionSelectedOptionModelData>();

        testDataShareRequestAnswersSummaryQuestionPartAnswerResponseItemOptionSelectionModelData.DataShareRequestAnswersSummaryQuestionPartAnswerResponseItemOptionSelection_SelectedOptions = testSelectedOptions;

        var result = testDataShareRequestAnswersSummaryQuestionPartAnswerResponseItemOptionSelectionModelData.DataShareRequestAnswersSummaryQuestionPartAnswerResponseItemOptionSelection_SelectedOptions;

        Assert.That(result, Is.EqualTo(testSelectedOptions));
    }
}