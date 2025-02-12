using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests.Questions;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Dto.Test.Models.DataShareRequests.Questions;

[TestFixture]
public class DataShareRequestQuestionTests
{
    [Test]
    public void GivenADataShareRequestQuestion_WhenISetDataShareRequestId_ThenDataShareRequestIdIsSet()
    {
        var testDataShareRequestQuestion = new DataShareRequestQuestion();

        var testDataShareRequestId = new Guid("E903A120-84CB-4712-B051-C21C65843D50");

        testDataShareRequestQuestion.DataShareRequestId = testDataShareRequestId;

        var result = testDataShareRequestQuestion.DataShareRequestId;

        Assert.That(result, Is.EqualTo(testDataShareRequestId));
    }

    [Test]
    public void GivenADataShareRequestQuestion_WhenISetDataShareRequestRequestId_ThenDataShareRequestRequestIdIsSet(
        [Values("", "  ", "abc")] string testDataShareRequestRequestId)
    {
        var testDataShareRequestQuestion = new DataShareRequestQuestion();

        testDataShareRequestQuestion.DataShareRequestRequestId = testDataShareRequestRequestId;

        var result = testDataShareRequestQuestion.DataShareRequestRequestId;

        Assert.That(result, Is.EqualTo(testDataShareRequestRequestId));
    }

    [Test]
    public void GivenADataShareRequestQuestion_WhenISetQuestionId_ThenQuestionIdIsSet()
    {
        var testDataShareRequestQuestion = new DataShareRequestQuestion();

        var testQuestionId = new Guid("D5C82765-3B44-4331-AF50-FE5727D5B319");

        testDataShareRequestQuestion.QuestionId = testQuestionId;

        var result = testDataShareRequestQuestion.QuestionId;

        Assert.That(result, Is.EqualTo(testQuestionId));
    }

    [Theory]
    public void GivenADataShareRequestQuestion_WhenISetIsOptional_ThenIsOptionalIsSet(
        bool testIsOptional)
    {
        var testDataShareRequestQuestion = new DataShareRequestQuestion();

        testDataShareRequestQuestion.IsOptional = testIsOptional;

        var result = testDataShareRequestQuestion.IsOptional;

        Assert.That(result, Is.EqualTo(testIsOptional));
    }

    [Test]
    public void GivenADataShareRequestQuestion_WhenISetAnEmptySetOfQuestionParts_ThenQuestionPartsIsSet()
    {
        var testDataShareRequestQuestion = new DataShareRequestQuestion();

        var testQuestionParts = new List<DataShareRequestQuestionPart>();

        testDataShareRequestQuestion.QuestionParts = testQuestionParts;

        var result = testDataShareRequestQuestion.QuestionParts;

        Assert.That(result, Is.EqualTo(testQuestionParts));
    }

    [Test]
    public void GivenADataShareRequestQuestion_WhenISetQuestionParts_ThenQuestionPartsIsSet()
    {
        var testDataShareRequestQuestion = new DataShareRequestQuestion();

        var testQuestionParts = new List<DataShareRequestQuestionPart>{ new(), new(), new() };

        testDataShareRequestQuestion.QuestionParts = testQuestionParts;

        var result = testDataShareRequestQuestion.QuestionParts;

        Assert.That(result, Is.EqualTo(testQuestionParts));
    }

    [Test]
    public void GivenADataShareRequestQuestion_WhenISetANullQuestionFooter_ThenQuestionFooterIsSet()
    {
        var testDataShareRequestQuestion = new DataShareRequestQuestion();

        var testQuestionFooter = (DataShareRequestQuestionFooter?) null;

        testDataShareRequestQuestion.QuestionFooter = testQuestionFooter;

        var result = testDataShareRequestQuestion.QuestionFooter;

        Assert.That(result, Is.EqualTo(testQuestionFooter));
    }

    [Test]
    public void GivenADataShareRequestQuestion_WhenISetQuestionFooter_ThenQuestionFooterIsSet()
    {
        var testDataShareRequestQuestion = new DataShareRequestQuestion();

        var testQuestionFooter = new DataShareRequestQuestionFooter();

        testDataShareRequestQuestion.QuestionFooter = testQuestionFooter;

        var result = testDataShareRequestQuestion.QuestionFooter;

        Assert.That(result, Is.SameAs(testQuestionFooter));
    }
}

