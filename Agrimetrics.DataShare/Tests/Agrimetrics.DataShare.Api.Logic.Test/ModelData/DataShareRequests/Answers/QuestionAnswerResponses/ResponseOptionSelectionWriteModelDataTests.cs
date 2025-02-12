using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests.Answers.DataShareRequestQuestionAnswerResponses;
using Agrimetrics.DataShare.Api.Logic.ModelData.Questions.QuestionParts.ResponseFormats;
using Moq;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.ModelData.DataShareRequests.Answers.DsrQuestionAnswerResponses;

[TestFixture]
public class DsrQuestionAnswerPartResponseOptionSelectionWriteModelDataTests
{
    [Test]
    public void GivenADataShareRequestQuestionAnswerPartResponseOptionSelectionWriteModelData_WhenIGetInputType_ThenInputTypeIsOptionSelection()
    {
        var testDataShareRequestQuestionAnswerPartResponseOptionSelectionWriteModelData = new DataShareRequestQuestionAnswerPartResponseOptionSelectionWriteModelData
        {
            OrderWithinAnswerPart = It.IsAny<int>(),
            SelectionOptions = []
        };

        var result = testDataShareRequestQuestionAnswerPartResponseOptionSelectionWriteModelData.InputType;

        Assert.That(result, Is.EqualTo(QuestionPartResponseInputType.OptionSelection));
    }

    [Test]
    public void GivenADataShareRequestQuestionAnswerPartResponseOptionSelectionWriteModelData_WhenISetAnEmptySetOfSelectionOptions_ThenSelectionOptionsIsSet()
    {
        var testSelectionOptions = new List<DataShareRequestQuestionAnswerPartResponseSelectionOptionWriteModelData>();

        var testDataShareRequestQuestionAnswerPartResponseOptionSelectionWriteModelData = new DataShareRequestQuestionAnswerPartResponseOptionSelectionWriteModelData
        {
            OrderWithinAnswerPart = It.IsAny<int>(),
            SelectionOptions = testSelectionOptions
        };

        var result = testDataShareRequestQuestionAnswerPartResponseOptionSelectionWriteModelData.SelectionOptions;

        Assert.That(result, Is.EqualTo(testSelectionOptions));
    }

    [Test]
    public void GivenADataShareRequestQuestionAnswerPartResponseOptionSelectionWriteModelData_WhenISetSelectionOptions_ThenSelectionOptionsIsSet()
    {
        var testSelectionOptions = new List<DataShareRequestQuestionAnswerPartResponseSelectionOptionWriteModelData>
        {
            new()
            {
                OptionSelectionId = It.IsAny<Guid>(),
                SupplementaryQuestionAnswerPart = null
            },
            new()
            {
                OptionSelectionId = It.IsAny<Guid>(),
                SupplementaryQuestionAnswerPart = null
            }
        };

        var testDataShareRequestQuestionAnswerPartResponseOptionSelectionWriteModelData = new DataShareRequestQuestionAnswerPartResponseOptionSelectionWriteModelData
        {
            OrderWithinAnswerPart = It.IsAny<int>(),
            SelectionOptions = testSelectionOptions
        };

        var result = testDataShareRequestQuestionAnswerPartResponseOptionSelectionWriteModelData.SelectionOptions;

        Assert.That(result, Is.EqualTo(testSelectionOptions));
    }
}