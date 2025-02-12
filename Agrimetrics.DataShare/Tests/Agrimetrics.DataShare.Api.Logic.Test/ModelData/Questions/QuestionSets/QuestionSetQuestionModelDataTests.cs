using Agrimetrics.DataShare.Api.Logic.ModelData.Questions.QuestionSets;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.ModelData.Questions.QuestionSets;

[TestFixture]
public class QuestionSetQuestionModelDataTests
{
    [Test]
    public void GivenAQuestionSetQuestionModelData_WhenISetQuestionId_ThenQuestionIdIsSet()
    {
        var testQuestionSetQuestionModelData = new QuestionSetQuestionModelData();

        var testQuestionId = new Guid("ADFB48B9-9AB4-4D7E-A00C-49C53DFE68B5");

        testQuestionSetQuestionModelData.QuestionSet_QuestionId = testQuestionId;

        var result = testQuestionSetQuestionModelData.QuestionSet_QuestionId;

        Assert.That(result, Is.EqualTo(testQuestionId));
    }

    [Test]
    public void GivenAQuestionSetQuestionModelData_WhenISetAnEmptySetOfQuestionParts_ThenQuestionPartsIsSet()
    {
        var testQuestionSetQuestionModelData = new QuestionSetQuestionModelData();

        var testQuestionParts = new List<QuestionSetQuestionPartModelData>();

        testQuestionSetQuestionModelData.QuestionSet_QuestionParts = testQuestionParts;

        var result = testQuestionSetQuestionModelData.QuestionSet_QuestionParts;

        Assert.That(result, Is.EqualTo(testQuestionParts));
    }

    [Test]
    public void GivenAQuestionSetQuestionModelData_WhenISetQuestionParts_ThenQuestionPartsIsSet()
    {
        var testQuestionSetQuestionModelData = new QuestionSetQuestionModelData();

        var testQuestionParts = new List<QuestionSetQuestionPartModelData> {new(), new(), new()};

        testQuestionSetQuestionModelData.QuestionSet_QuestionParts = testQuestionParts;

        var result = testQuestionSetQuestionModelData.QuestionSet_QuestionParts;

        Assert.That(result, Is.EqualTo(testQuestionParts));
    }
}