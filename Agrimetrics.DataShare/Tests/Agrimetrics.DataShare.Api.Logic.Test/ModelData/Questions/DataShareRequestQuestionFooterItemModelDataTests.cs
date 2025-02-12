using Agrimetrics.DataShare.Api.Logic.ModelData.Questions;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.ModelData.Questions;

[TestFixture]
public class DataShareRequestQuestionFooterItemModelDataTests
{
    [Test]
    public void GivenADataShareRequestQuestionFooterItemModelData_WhenISetId_ThenIdIsSet()
    {
        var testDataShareRequestQuestionFooterItemModelData = new DataShareRequestQuestionFooterItemModelData();

        var testId = new Guid("AF4FBDAD-B551-4F79-9518-3F7EEF1CB0F0");

        testDataShareRequestQuestionFooterItemModelData.DataShareRequestQuestionFooterItem_Id = testId;

        var result = testDataShareRequestQuestionFooterItemModelData.DataShareRequestQuestionFooterItem_Id;

        Assert.That(result, Is.EqualTo(testId));
    }

    [Test]
    public void GivenADataShareRequestQuestionFooterItemModelData_WhenISetFooterId_ThenFooterIdIsSet()
    {
        var testDataShareRequestQuestionFooterItemModelData = new DataShareRequestQuestionFooterItemModelData();

        var testFooterId = new Guid("AF4FBDAD-B551-4F79-9518-3F7EEF1CB0F0");

        testDataShareRequestQuestionFooterItemModelData.DataShareRequestQuestionFooterItem_FooterId = testFooterId;

        var result = testDataShareRequestQuestionFooterItemModelData.DataShareRequestQuestionFooterItem_FooterId;

        Assert.That(result, Is.EqualTo(testFooterId));
    }

    [Test]
    public void GivenADataShareRequestQuestionFooterItemModelData_WhenISetText_ThenTextIsSet(
        [Values("", "  ", "abc")] string testText)
    {
        var testDataShareRequestQuestionFooterItemModelData = new DataShareRequestQuestionFooterItemModelData();

        testDataShareRequestQuestionFooterItemModelData.DataShareRequestQuestionFooterItem_Text = testText;

        var result = testDataShareRequestQuestionFooterItemModelData.DataShareRequestQuestionFooterItem_Text;

        Assert.That(result, Is.EqualTo(testText));
    }

    [Test]
    public void GivenADataShareRequestQuestionFooterItemModelData_WhenISetOrderWithinFooter_ThenOrderWithinFooterIsSet(
        [Values(-1, 0, 999)] int testOrderWithinFooter)
    {
        var testDataShareRequestQuestionFooterItemModelData = new DataShareRequestQuestionFooterItemModelData();

        testDataShareRequestQuestionFooterItemModelData.DataShareRequestQuestionFooterItem_OrderWithinFooter = testOrderWithinFooter;

        var result = testDataShareRequestQuestionFooterItemModelData.DataShareRequestQuestionFooterItem_OrderWithinFooter;

        Assert.That(result, Is.EqualTo(testOrderWithinFooter));
    }
}