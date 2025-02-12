using Agrimetrics.DataShare.Api.Logic.ModelData.Questions.KeyQuestionParts;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.ModelData.Questions.KeyQuestionParts;

[TestFixture]
public class KeyQuestionPartAnswerResponseModelDataTests
{
    [Test]
    public void GivenAKeyQuestionPartAnswerResponseModelData_WhenISetAnswerPartResponseId_ThenAnswerPartResponseIdIsSet()
    {
        var testKeyQuestionPartAnswerResponseModelData = new KeyQuestionPartAnswerResponseModelData();

        var testAnswerPartResponseId = new Guid("15E1D2F9-2AF2-4663-A9E5-6D4CFFD69609");

        testKeyQuestionPartAnswerResponseModelData.KeyQuestionPart_AnswerPartResponseId = testAnswerPartResponseId;

        var result = testKeyQuestionPartAnswerResponseModelData.KeyQuestionPart_AnswerPartResponseId;

        Assert.That(result, Is.EqualTo(testAnswerPartResponseId));
    }

    [Test]
    public void GivenAKeyQuestionPartAnswerResponseModelData_WhenISetANullResponseItem_ThenResponseItemIsSet()
    {
        var testKeyQuestionPartAnswerResponseModelData = new KeyQuestionPartAnswerResponseModelData();

        var testResponseItem = (KeyQuestionPartAnswerResponseItemModelData?) null;

        testKeyQuestionPartAnswerResponseModelData.KeyQuestionPartAnswerResponse_ResponseItem = testResponseItem;

        var result = testKeyQuestionPartAnswerResponseModelData.KeyQuestionPartAnswerResponse_ResponseItem;

        Assert.That(result, Is.EqualTo(testResponseItem));
    }

    [Test]
    public void GivenAKeyQuestionPartAnswerResponseModelData_WhenISetResponseItem_ThenResponseItemIsSet()
    {
        var testKeyQuestionPartAnswerResponseModelData = new KeyQuestionPartAnswerResponseModelData();

        var testResponseItem = new TestKeyQuestionPartAnswerResponseItemModelData();

        testKeyQuestionPartAnswerResponseModelData.KeyQuestionPartAnswerResponse_ResponseItem = testResponseItem;

        var result = testKeyQuestionPartAnswerResponseModelData.KeyQuestionPartAnswerResponse_ResponseItem;

        Assert.That(result, Is.EqualTo(testResponseItem));
    }

    private class TestKeyQuestionPartAnswerResponseItemModelData : KeyQuestionPartAnswerResponseItemModelData;
}