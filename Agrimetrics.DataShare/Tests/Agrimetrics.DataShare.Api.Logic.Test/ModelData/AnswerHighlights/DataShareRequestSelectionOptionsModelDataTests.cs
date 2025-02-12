using Agrimetrics.DataShare.Api.Logic.ModelData.AnswerHighlights;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.ModelData.AnswerHighlights;

[TestFixture]
public class DataShareRequestSelectionOptionsModelDataTests
{
    [Test]
    public void GivenADataShareRequestSelectionOptionsModelData_WhenISetDataShareRequestId_ThenDataShareRequestIdIsSet()
    {
        var testDataShareRequestSelectionOptionsModelData = new DataShareRequestSelectionOptionsModelData();

        var testDataShareRequestId = new Guid("897609FC-779D-431F-B380-3B3D1A4A014F");

        testDataShareRequestSelectionOptionsModelData.DataShareRequestSelectionOptions_DataShareRequestId = testDataShareRequestId;

        var result = testDataShareRequestSelectionOptionsModelData.DataShareRequestSelectionOptions_DataShareRequestId;

        Assert.That(result, Is.EqualTo(testDataShareRequestId));
    }

    [Test]
    public void GivenADataShareRequestSelectionOptionsModelData_WhenISetAnEmptySetOfSelectedOptions_ThenSelectedOptionsIsSet()
    {
        var testDataShareRequestSelectionOptionsModelData = new DataShareRequestSelectionOptionsModelData();

        var testSelectedOptions = new List<DataShareRequestSelectedOptionModelData>();

        testDataShareRequestSelectionOptionsModelData.DataShareRequestSelectionOptions_SelectedOptions = testSelectedOptions;

        var result = testDataShareRequestSelectionOptionsModelData.DataShareRequestSelectionOptions_SelectedOptions;

        Assert.That(result, Is.EqualTo(testSelectedOptions));
    }

    [Test]
    public void GivenADataShareRequestSelectionOptionsModelData_WhenISetSelectedOptions_ThenSelectedOptionsIsSet()
    {
        var testDataShareRequestSelectionOptionsModelData = new DataShareRequestSelectionOptionsModelData();

        var testSelectedOptions = new List<DataShareRequestSelectedOptionModelData> {new(), new(), new()};

        testDataShareRequestSelectionOptionsModelData.DataShareRequestSelectionOptions_SelectedOptions = testSelectedOptions;

        var result = testDataShareRequestSelectionOptionsModelData.DataShareRequestSelectionOptions_SelectedOptions;

        Assert.That(result, Is.EqualTo(testSelectedOptions));
    }
}