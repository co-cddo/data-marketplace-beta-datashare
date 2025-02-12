using Agrimetrics.DataShare.Api.Logic.ModelData.Questions.QuestionParts.OptionSelectionItems;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.ModelData.Questions.QuestionParts.OptionSelectionItems;

[TestFixture]
public class QuestionPartOptionSelectionItemForMultiSelectionModelDataTests
{
    [Theory]
    public void GivenAQuestionPartOptionSelectionItemForMultiSelectionModelData_WhenISetIsMaster_ThenIsMasterIsSet(
        bool testIsMaster)
    {
        var testQuestionPartOptionSelectionItemForMultiSelectionModelData = new QuestionPartOptionSelectionItemForMultiSelectionModelData();

        testQuestionPartOptionSelectionItemForMultiSelectionModelData.MultiSelectOption_IsMaster = testIsMaster;

        var result = testQuestionPartOptionSelectionItemForMultiSelectionModelData.MultiSelectOption_IsMaster;

        Assert.That(result, Is.EqualTo(testIsMaster));
    }
}
