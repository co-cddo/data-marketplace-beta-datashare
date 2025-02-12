using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests.QuestionStatusDeterminations;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.ModelData.DataShareRequests.QuestionStatusDeterminations;

[TestFixture]
public class QuestionSetQuestionInformationModelDataTests
{
    [Test]
    public void GivenAQuestionSetQuestionInformationModelData_WhenISetSectionNumber_ThenSectionNumberIsSet(
        [Values(-1, 0, 999)] int testSectionNumber)
    {
        var testQuestionSetQuestionInformationModelData = new QuestionSetQuestionInformationModelData();

        testQuestionSetQuestionInformationModelData.QuestionSet_SectionNumber = testSectionNumber;

        var result = testQuestionSetQuestionInformationModelData.QuestionSet_SectionNumber;

        Assert.That(result, Is.EqualTo(testSectionNumber));
    }

    [Test]
    public void GivenAQuestionSetQuestionInformationModelData_WhenISetQuestionOrderWithinSection_ThenQuestionOrderWithinSectionIsSet(
        [Values(-1, 0, 999)] int testQuestionOrderWithinSection)
    {
        var testQuestionSetQuestionInformationModelData = new QuestionSetQuestionInformationModelData();

        testQuestionSetQuestionInformationModelData.QuestionSet_QuestionOrerWithinSection = testQuestionOrderWithinSection;

        var result = testQuestionSetQuestionInformationModelData.QuestionSet_QuestionOrerWithinSection;

        Assert.That(result, Is.EqualTo(testQuestionOrderWithinSection));
    }
}