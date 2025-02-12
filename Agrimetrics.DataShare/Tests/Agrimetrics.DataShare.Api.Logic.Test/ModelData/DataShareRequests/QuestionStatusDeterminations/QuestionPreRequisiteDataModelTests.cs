using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests.QuestionStatusDeterminations;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.ModelData.DataShareRequests.QuestionStatusDeterminations;

[TestFixture]
public class QuestionPreRequisiteDataModelTests
{
    [Test]
    public void GivenAQuestionPreRequisiteDataModel_WhenISetQuestionId_ThenQuestionIdIsSet()
    {
        var testQuestionPreRequisiteDataModel = new QuestionPreRequisiteDataModel();

        var testQuestionId = new Guid("E308E58A-8920-4048-9D4D-A2633E4A1C08");

        testQuestionPreRequisiteDataModel.QuestionPreRequisite_QuestionId = testQuestionId;

        var result = testQuestionPreRequisiteDataModel.QuestionPreRequisite_QuestionId;

        Assert.That(result, Is.EqualTo(testQuestionId));
    }

    [Test]
    public void GivenAQuestionPreRequisiteDataModel_WhenISetPreRequisiteQuestionId_ThenPreRequisiteQuestionIdIsSet()
    {
        var testQuestionPreRequisiteDataModel = new QuestionPreRequisiteDataModel();

        var testPreRequisiteQuestionId = new Guid("E308E58A-8920-4048-9D4D-A2633E4A1C08");

        testQuestionPreRequisiteDataModel.QuestionPreRequisite_PreRequisiteQuestionId = testPreRequisiteQuestionId;

        var result = testQuestionPreRequisiteDataModel.QuestionPreRequisite_PreRequisiteQuestionId;

        Assert.That(result, Is.EqualTo(testPreRequisiteQuestionId));
    }
}