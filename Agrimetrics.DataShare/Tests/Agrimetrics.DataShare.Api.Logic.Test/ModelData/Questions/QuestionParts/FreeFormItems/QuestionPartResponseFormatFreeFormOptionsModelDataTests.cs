using Agrimetrics.DataShare.Api.Logic.ModelData.Questions.QuestionParts.FreeFormItems;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.ModelData.Questions.QuestionParts.FreeFormItems;


[TestFixture]
public class QuestionPartResponseFormatFreeFormOptionsModelDataTests
{
    [Test]
    public void GivenAQuestionPartResponseFormatFreeFormOptionsModelData_WhenISetId_ThenIdIsSet()
    {
        var testQuestionPartResponseFormatFreeFormOptionsModelData = new QuestionPartResponseFormatFreeFormOptionsModelData();

        var testId = new Guid("15E1D2F9-2AF2-4663-A9E5-6D4CFFD69609");

        testQuestionPartResponseFormatFreeFormOptionsModelData.QuestionPartResponseFormatFreeFormOptions_Id = testId;

        var result = testQuestionPartResponseFormatFreeFormOptionsModelData.QuestionPartResponseFormatFreeFormOptions_Id;

        Assert.That(result, Is.EqualTo(testId));
    }

    [Test]
    public void GivenAQuestionPartResponseFormatFreeFormOptionsModelData_WhenISetQuestionPartId_ThenQuestionPartIdIsSet()
    {
        var testQuestionPartResponseFormatFreeFormOptionsModelData = new QuestionPartResponseFormatFreeFormOptionsModelData();
        
        var testQuestionPartId = new Guid("15E1D2F9-2AF2-4663-A9E5-6D4CFFD69609");

        testQuestionPartResponseFormatFreeFormOptionsModelData.QuestionPartResponseFormatFreeFormOptions_QuestionPartId = testQuestionPartId;

        var result = testQuestionPartResponseFormatFreeFormOptionsModelData.QuestionPartResponseFormatFreeFormOptions_QuestionPartId;

        Assert.That(result, Is.EqualTo(testQuestionPartId));
    }

    [Theory]
    public void GivenAQuestionPartResponseFormatFreeFormOptionsModelData_WhenISetValueEntryMayBeDeclined_ThenValueEntryMayBeDeclinedIsSet(
        bool testValueEntryMayBeDeclined)
    {
        var testQuestionPartResponseFormatFreeFormOptionsModelData = new QuestionPartResponseFormatFreeFormOptionsModelData();

        testQuestionPartResponseFormatFreeFormOptionsModelData.QuestionPartResponseFormatFreeFormOptions_ValueEntryMayBeDeclined = testValueEntryMayBeDeclined;

        var result = testQuestionPartResponseFormatFreeFormOptionsModelData.QuestionPartResponseFormatFreeFormOptions_ValueEntryMayBeDeclined;

        Assert.That(result, Is.EqualTo(testValueEntryMayBeDeclined));
    }
}
