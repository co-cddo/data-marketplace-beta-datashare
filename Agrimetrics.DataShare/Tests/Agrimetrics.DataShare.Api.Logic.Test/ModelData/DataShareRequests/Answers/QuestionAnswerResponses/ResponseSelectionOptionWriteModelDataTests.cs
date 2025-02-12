using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests.Answers.DataShareRequestQuestionAnswerResponses;
using Moq;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.ModelData.DataShareRequests.Answers.DsrQuestionAnswerResponses;

[TestFixture]
public class DsrQuestionAnswerPartResponseSelectionOptionWriteModelDataTests
{
    [Test]
    public void GivenADataShareRequestQuestionAnswerPartResponseSelectionOptionWriteModelData_WhenISetOptionSelectionId_ThenOptionSelectionIdIsSet()
    {
        var testOptionSelectionId = new Guid("6D043030-3E20-4AA6-B02A-B5F0283C073F");

        var testDataShareRequestQuestionAnswerPartResponseSelectionOptionWriteModelData = new DataShareRequestQuestionAnswerPartResponseSelectionOptionWriteModelData 
        {
            OptionSelectionId = testOptionSelectionId,
            SupplementaryQuestionAnswerPart = It.IsAny<DataShareRequestQuestionAnswerPartWriteModelData?>()
        };

        var result = testDataShareRequestQuestionAnswerPartResponseSelectionOptionWriteModelData.OptionSelectionId;

        Assert.That(result, Is.EqualTo(testOptionSelectionId));
    }

    [Test]
    public void GivenADataShareRequestQuestionAnswerPartResponseSelectionOptionWriteModelData_WhenISetANullSupplementaryQuestionAnswerPart_ThenSupplementaryQuestionAnswerPartIsSet()
    {
        var testSupplementaryQuestionAnswerPart = (DataShareRequestQuestionAnswerPartWriteModelData?) null;

        var testDataShareRequestQuestionAnswerPartResponseSelectionOptionWriteModelData = new DataShareRequestQuestionAnswerPartResponseSelectionOptionWriteModelData
        {
            OptionSelectionId = It.IsAny<Guid>(),
            SupplementaryQuestionAnswerPart = testSupplementaryQuestionAnswerPart
        };

        var result = testDataShareRequestQuestionAnswerPartResponseSelectionOptionWriteModelData.SupplementaryQuestionAnswerPart;

        Assert.That(result, Is.EqualTo(testSupplementaryQuestionAnswerPart));
    }

    [Test]
    public void GivenADataShareRequestQuestionAnswerPartResponseSelectionOptionWriteModelData_WhenISetSupplementaryQuestionAnswerPart_ThenSupplementaryQuestionAnswerPartIsSet()
    {
        var testSupplementaryQuestionAnswerPart = new DataShareRequestQuestionAnswerPartWriteModelData
        {
            QuestionPartId = It.IsAny<Guid>(),
            AnswerPartResponses = []
        };

        var testDataShareRequestQuestionAnswerPartResponseSelectionOptionWriteModelData = new DataShareRequestQuestionAnswerPartResponseSelectionOptionWriteModelData
        {
            OptionSelectionId = It.IsAny<Guid>(),
            SupplementaryQuestionAnswerPart = testSupplementaryQuestionAnswerPart
        };

        var result = testDataShareRequestQuestionAnswerPartResponseSelectionOptionWriteModelData.SupplementaryQuestionAnswerPart;

        Assert.That(result, Is.EqualTo(testSupplementaryQuestionAnswerPart));
    }
}