using Agrimetrics.DataShare.Api.Logic.ModelData.Questions.QuestionSets;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.ModelData.Questions.QuestionSets;

[TestFixture]
public class QuestionSetQuestionPartModelDataTests
{
    [Test]
    public void GivenAQuestionSetQuestionPartModelData_WhenISetQuestionPartId_ThenQuestionPartIdIsSet()
    {
        var testQuestionSetQuestionPartModelData = new QuestionSetQuestionPartModelData();

        var testQuestionPartId = new Guid("C7A3C090-12D2-4046-93E2-E6AD6E17F8F6");

        testQuestionSetQuestionPartModelData.QuestionSet_QuestionPartId = testQuestionPartId;

        var result = testQuestionSetQuestionPartModelData.QuestionSet_QuestionPartId;

        Assert.That(result, Is.EqualTo(testQuestionPartId));
    }
}