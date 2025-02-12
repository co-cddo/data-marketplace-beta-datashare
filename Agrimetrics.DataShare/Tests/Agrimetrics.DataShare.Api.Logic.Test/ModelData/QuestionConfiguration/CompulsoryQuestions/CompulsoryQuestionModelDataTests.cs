using Agrimetrics.DataShare.Api.Logic.ModelData.QuestionConfiguration.CompulsoryQuestions;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.ModelData.QuestionConfiguration.CompulsoryQuestions;

[TestFixture]
public class CompulsoryQuestionModelDataTests
{
    [Test]
    public void GivenACompulsoryQuestionModelData_WhenISetQuestionId_ThenQuestionIdIsSet()
    {
        var testCompulsoryQuestionModelData = new CompulsoryQuestionModelData();

        var testQuestionId = new Guid("DF804714-0BE4-439D-899B-F79ACA2AB195");

        testCompulsoryQuestionModelData.CompulsoryQuestion_QuestionId = testQuestionId;

        var result = testCompulsoryQuestionModelData.CompulsoryQuestion_QuestionId;

        Assert.That(result, Is.EqualTo(testQuestionId));
    }
}
