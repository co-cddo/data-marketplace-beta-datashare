using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests.Questions;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Dto.Test.Models.DataShareRequests.Questions;

[TestFixture]
public class DataShareRequestQuestionFooterItemTests
{
    [Test]
    public void GivenADataShareRequestQuestionFooterItem_WhenISetText_ThenTextIsSet(
        [Values("", "  ", "abc")] string testText)
    {
        var testDataShareRequestQuestionFooterItem = new DataShareRequestQuestionFooterItem();

        testDataShareRequestQuestionFooterItem.Text = testText;

        var result = testDataShareRequestQuestionFooterItem.Text;

        Assert.That(result, Is.EqualTo(testText));
    }
    
    [Test]
    public void GivenADataShareRequestQuestionFooterItem_WhenISetOrderWithinFooter_ThenOrderWithinFooterIsSet(
        [Values(-1, 0, 999)] int testOrderWithinFooter)
    {
        var testDataShareRequestQuestionFooterItem = new DataShareRequestQuestionFooterItem();

        testDataShareRequestQuestionFooterItem.OrderWithinFooter = testOrderWithinFooter;

        var result = testDataShareRequestQuestionFooterItem.OrderWithinFooter;

        Assert.That(result, Is.EqualTo(testOrderWithinFooter));
    }
}