using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests.Answers.DataShareRequestQuestionAnswerResponses;
using Moq;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.ModelData.DataShareRequests.Answers.DsrQuestionAnswerResponses;

[TestFixture]
public class DsrQuestionAnswerWriteModelDataTests
{
    [Test]
    public void GivenADataShareRequestQuestionAnswerWriteModelData_WhenISetDataShareRequestId_ThenDataShareRequestIdIsSet()
    {
        var testDataShareRequestId = new Guid("B6CA960F-3582-41B1-BF79-AA581FAAC048");

        var testDataShareRequestQuestionAnswerWriteModelData = new DataShareRequestQuestionAnswerWriteModelData
        {
            DataShareRequestId = testDataShareRequestId,
            QuestionId = It.IsAny<Guid>(),
            AnswerParts = []
        };

        var result = testDataShareRequestQuestionAnswerWriteModelData.DataShareRequestId;

        Assert.That(result, Is.EqualTo(testDataShareRequestId));
    }

    [Test]
    public void GivenADataShareRequestQuestionAnswerWriteModelData_WhenISetQuestionId_ThenQuestionIdIsSet()
    {
        var testQuestionId = new Guid("B6CA960F-3582-41B1-BF79-AA581FAAC048");

        var testDataShareRequestQuestionAnswerWriteModelData = new DataShareRequestQuestionAnswerWriteModelData
        {
            DataShareRequestId = It.IsAny<Guid>(),
            QuestionId = testQuestionId,
            AnswerParts = []
        };

        var result = testDataShareRequestQuestionAnswerWriteModelData.QuestionId;

        Assert.That(result, Is.EqualTo(testQuestionId));
    }

    [Test]
    public void GivenADataShareRequestQuestionAnswerWriteModelData_WhenISetAnEmptySetOfAnswerParts_ThenAnswerPartsIsSet()
    {
        var testAnswerParts = new List<DataShareRequestQuestionAnswerPartWriteModelData>();

        var testDataShareRequestQuestionAnswerWriteModelData = new DataShareRequestQuestionAnswerWriteModelData
        {
            DataShareRequestId = It.IsAny<Guid>(),
            QuestionId = It.IsAny<Guid>(),
            AnswerParts = testAnswerParts
        };

        var result = testDataShareRequestQuestionAnswerWriteModelData.AnswerParts;

        Assert.That(result, Is.EqualTo(testAnswerParts));
    }

    [Test]
    public void GivenADataShareRequestQuestionAnswerWriteModelData_WhenISetAnswerParts_ThenAnswerPartsIsSet()
    {
        var testAnswerParts = new List<DataShareRequestQuestionAnswerPartWriteModelData>
        {
            new()
            {
                QuestionPartId = It.IsAny<Guid>(),
                AnswerPartResponses = []
            },
            new()
            {
                QuestionPartId = It.IsAny<Guid>(),
                AnswerPartResponses = []
            }
        };

        var testDataShareRequestQuestionAnswerWriteModelData = new DataShareRequestQuestionAnswerWriteModelData
        {
            DataShareRequestId = It.IsAny<Guid>(),
            QuestionId = It.IsAny<Guid>(),
            AnswerParts = testAnswerParts
        };

        var result = testDataShareRequestQuestionAnswerWriteModelData.AnswerParts;

        Assert.That(result, Is.EqualTo(testAnswerParts));
    }
}