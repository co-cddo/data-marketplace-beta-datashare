using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests.Questions;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Dto.Test.Models.DataShareRequests.Questions;

[TestFixture]
public class DataShareRequestQuestionFooterTests
{
    [Test]
    public void GivenADataShareRequestQuestionFooter_WhenISetFooterHeader_ThenFooterHeaderIsSet(
        [Values(null, "", "  ", "abc")] string? testFooterHeader)
    {
        var testDataShareRequestQuestionFooter = new DataShareRequestQuestionFooter();

        testDataShareRequestQuestionFooter.FooterHeader = testFooterHeader;

        var result = testDataShareRequestQuestionFooter.FooterHeader;

        Assert.That(result, Is.EqualTo(testFooterHeader));
    }

    [Test]
    public void GivenADataShareRequestQuestionFooter_WhenISetAnEmptySetOfFooterItems_ThenFooterItemsIsSet()
    {
        var testDataShareRequestQuestionFooter = new DataShareRequestQuestionFooter();

        var testFooterItems = new List<DataShareRequestQuestionFooterItem>();

        testDataShareRequestQuestionFooter.FooterItems = testFooterItems;

        var result = testDataShareRequestQuestionFooter.FooterItems;

        Assert.That(result, Is.EqualTo(testFooterItems));
    }

    [Test]
    public void GivenADataShareRequestQuestionFooter_WhenISetFooterItems_ThenFooterItemsIsSet()
    {
        var testDataShareRequestQuestionFooter = new DataShareRequestQuestionFooter();

        var testFooterItems = new List<DataShareRequestQuestionFooterItem> {new(), new(), new()};

        testDataShareRequestQuestionFooter.FooterItems = testFooterItems;

        var result = testDataShareRequestQuestionFooter.FooterItems;

        Assert.That(result, Is.EqualTo(testFooterItems));
    }
}