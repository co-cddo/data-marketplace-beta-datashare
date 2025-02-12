using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests.Answers.DataShareRequestQuestionAnswers;
using Agrimetrics.DataShare.Api.Logic.ModelData.Questions;
using Agrimetrics.DataShare.Api.Logic.ModelData.Questions.QuestionParts;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.ModelData.Questions;

[TestFixture]
public class DataShareRequestQuestionPartModelDataTests
{
    [Test]
    public void GivenADataShareRequestQuestionPartModelData_WhenISetQuestion_ThenQuestionIsSet()
    {
        var testDataShareRequestQuestionPartModelData = new DataShareRequestQuestionPartModelData();

        var testQuestion = new QuestionPartModelData();

        testDataShareRequestQuestionPartModelData.DataShareRequestQuestionPart_Question = testQuestion;

        var result = testDataShareRequestQuestionPartModelData.DataShareRequestQuestionPart_Question;

        Assert.That(result, Is.EqualTo(testQuestion));
    }

    [Test]
    public void GivenADataShareRequestQuestionPartModelData_WhenISetAnswer_ThenAnswerIsSet()
    {
        var testDataShareRequestQuestionPartModelData = new DataShareRequestQuestionPartModelData();

        var testAnswer = new QuestionPartAnswerModelData();

        testDataShareRequestQuestionPartModelData.DataShareRequestQuestionPart_Answer = testAnswer;

        var result = testDataShareRequestQuestionPartModelData.DataShareRequestQuestionPart_Answer;

        Assert.That(result, Is.EqualTo(testAnswer));
    }
}

