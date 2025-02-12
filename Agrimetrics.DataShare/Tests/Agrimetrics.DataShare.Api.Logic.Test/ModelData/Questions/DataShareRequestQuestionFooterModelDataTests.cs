using Agrimetrics.DataShare.Api.Logic.ModelData.Questions;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.ModelData.Questions;

[TestFixture]
public class DataShareRequestQuestionFooterModelDataTests
{
    [Test]
    public void GivenADataShareRequestQuestionFooterModelData_WhenISetId_ThenIdIsSet()
    {
        var testDataShareRequestQuestionFooterModelData = new DataShareRequestQuestionFooterModelData();

        var testId = new Guid("109F0762-A78C-48CE-877D-AA957E2826E5");

        testDataShareRequestQuestionFooterModelData.DataShareRequestQuestionFooter_Id = testId;

        var result = testDataShareRequestQuestionFooterModelData.DataShareRequestQuestionFooter_Id;

        Assert.That(result, Is.EqualTo(testId));
    }

    [Test]
    public void GivenADataShareRequestQuestionFooterModelData_WhenISetHeader_ThenHeaderIsSet(
        [Values(null, "", "  ", "abc")] string? testHeader)
    {
        var testDataShareRequestQuestionFooterModelData = new DataShareRequestQuestionFooterModelData();

        testDataShareRequestQuestionFooterModelData.DataShareRequestQuestionFooter_Header = testHeader;

        var result = testDataShareRequestQuestionFooterModelData.DataShareRequestQuestionFooter_Header;

        Assert.That(result, Is.EqualTo(testHeader));
    }

    [Test]
    public void GivenADataShareRequestQuestionFooterModelData_WhenISetAnEmptySetOfItems_ThenItemsIsSet()
    {
        var testDataShareRequestQuestionFooterModelData = new DataShareRequestQuestionFooterModelData();

        var testItems = new List<DataShareRequestQuestionFooterItemModelData>();

        testDataShareRequestQuestionFooterModelData.DataShareRequestQuestionFooter_Items = testItems;

        var result = testDataShareRequestQuestionFooterModelData.DataShareRequestQuestionFooter_Items;

        Assert.That(result, Is.EqualTo(testItems));
    }

    [Test]
    public void GivenADataShareRequestQuestionFooterModelData_WhenISetItems_ThenItemsIsSet()
    {
        var testDataShareRequestQuestionFooterModelData = new DataShareRequestQuestionFooterModelData();

        var testItems = new List<DataShareRequestQuestionFooterItemModelData> {new(), new(), new()};

        testDataShareRequestQuestionFooterModelData.DataShareRequestQuestionFooter_Items = testItems;

        var result = testDataShareRequestQuestionFooterModelData.DataShareRequestQuestionFooter_Items;

        Assert.That(result, Is.EqualTo(testItems));
    }
}