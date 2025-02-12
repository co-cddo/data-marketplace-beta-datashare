using Agrimetrics.DataShare.Api.Logic.ModelData.Questions.KeyQuestionParts;
using Agrimetrics.DataShare.Api.Dto.Models.Questions.QuestionParts.ResponseFormats;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.ModelData.Questions.KeyQuestionParts;

[TestFixture]
public class KeyQuestionPartAnswerModelDataTests
{
    [Test]
    public void GivenAKeyQuestionPartAnswerModelData_WhenISetQuestionSetId_ThenQuestionSetIdIsSet()
    {
        var testKeyQuestionPartAnswerModelData = new KeyQuestionPartAnswerModelData();

        var testQuestionSetId = new Guid("842A3BFD-B427-4ED1-9DF0-0D2F17EFA889");

        testKeyQuestionPartAnswerModelData.KeyQuestionPartAnswer_QuestionSetId = testQuestionSetId;

        var result = testKeyQuestionPartAnswerModelData.KeyQuestionPartAnswer_QuestionSetId;

        Assert.That(result, Is.EqualTo(testQuestionSetId));
    }

    [Test]
    public void GivenAKeyQuestionPartAnswerModelData_WhenISetQuestionPartId_ThenQuestionPartIdIsSet()
    {
        var testKeyQuestionPartAnswerModelData = new KeyQuestionPartAnswerModelData();

        var testQuestionPartId = new Guid("842A3BFD-B427-4ED1-9DF0-0D2F17EFA889");

        testKeyQuestionPartAnswerModelData.KeyQuestionPartAnswer_QuestionPartId = testQuestionPartId;

        var result = testKeyQuestionPartAnswerModelData.KeyQuestionPartAnswer_QuestionPartId;

        Assert.That(result, Is.EqualTo(testQuestionPartId));
    }

    [Theory]
    public void GivenAKeyQuestionPartAnswerModelData_WhenISetAllowMultipleResponses_ThenAllowMultipleResponsesIsSet(
        bool testAllowMultipleResponses)
    {
        var testKeyQuestionPartAnswerModelData = new KeyQuestionPartAnswerModelData();

        testKeyQuestionPartAnswerModelData.KeyQuestionPartAnswer_AllowMultipleResponses = testAllowMultipleResponses;

        var result = testKeyQuestionPartAnswerModelData.KeyQuestionPartAnswer_AllowMultipleResponses;

        Assert.That(result, Is.EqualTo(testAllowMultipleResponses));
    }

    [Theory]
    public void GivenAKeyQuestionPartAnswerModelData_WhenISetResponseFormatType_ThenResponseFormatTypeIsSet(
        QuestionPartResponseInputType testResponseFormatType)
    {
        var testKeyQuestionPartAnswerModelData = new KeyQuestionPartAnswerModelData();

        testKeyQuestionPartAnswerModelData.KeyQuestionPartAnswer_ResponseFormatType = testResponseFormatType;

        var result = testKeyQuestionPartAnswerModelData.KeyQuestionPartAnswer_ResponseFormatType;

        Assert.That(result, Is.EqualTo(testResponseFormatType));
    }

    [Test]
    public void GivenAKeyQuestionPartAnswerModelData_WhenISetAnswerPartId_ThenAnswerPartIdIsSet()
    {
        var testKeyQuestionPartAnswerModelData = new KeyQuestionPartAnswerModelData();

        var testAnswerPartId = new Guid("842A3BFD-B427-4ED1-9DF0-0D2F17EFA889");

        testKeyQuestionPartAnswerModelData.KeyQuestionPartAnswer_AnswerPartId = testAnswerPartId;

        var result = testKeyQuestionPartAnswerModelData.KeyQuestionPartAnswer_AnswerPartId;

        Assert.That(result, Is.EqualTo(testAnswerPartId));
    }

    [Test]
    public void GivenAKeyQuestionPartAnswerModelData_WhenISetAnEmptySetOfAnswerPartResponses_ThenAnswerPartResponsesIsSet()
    {
        var testKeyQuestionPartAnswerModelData = new KeyQuestionPartAnswerModelData();

        var testAnswerPartResponses = new List<KeyQuestionPartAnswerResponseModelData>();

        testKeyQuestionPartAnswerModelData.KeyQuestionPartAnswer_AnswerPartResponses = testAnswerPartResponses;

        var result = testKeyQuestionPartAnswerModelData.KeyQuestionPartAnswer_AnswerPartResponses;

        Assert.That(result, Is.EqualTo(testAnswerPartResponses));
    }

    [Test]
    public void GivenAKeyQuestionPartAnswerModelData_WhenISetAnswerPartResponses_ThenAnswerPartResponsesIsSet()
    {
        var testKeyQuestionPartAnswerModelData = new KeyQuestionPartAnswerModelData();

        var testAnswerPartResponses = new List<KeyQuestionPartAnswerResponseModelData> {new(), new(), new()};

        testKeyQuestionPartAnswerModelData.KeyQuestionPartAnswer_AnswerPartResponses = testAnswerPartResponses;

        var result = testKeyQuestionPartAnswerModelData.KeyQuestionPartAnswer_AnswerPartResponses;

        Assert.That(result, Is.EqualTo(testAnswerPartResponses));
    }
}
