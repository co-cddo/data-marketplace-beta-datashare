using Agrimetrics.DataShare.Api.Logic.ModelData.Questions.KeyQuestionParts;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.ModelData.Questions.KeyQuestionParts;

[TestFixture]
public class KeyQuestionPartAnswerResponseItemModelDataTests
{
    [Test]
    public void GivenAKeyQuestionPartAnswerResponseItemModelData_WhenISetResponseItemId_ThenResponseItemIdIsSet()
    {
        var testKeyQuestionPartAnswerResponseItemModelData = new TestKeyQuestionPartAnswerResponseItemModelData();

        var testResponseItemId = new Guid("80C292E3-2CE9-437D-BFB4-EA1089C71FF9");

        testKeyQuestionPartAnswerResponseItemModelData.KeyQuestionPartAnswerResponseItem_ResponseItemId = testResponseItemId;

        var result = testKeyQuestionPartAnswerResponseItemModelData.KeyQuestionPartAnswerResponseItem_ResponseItemId;

        Assert.That(result, Is.EqualTo(testResponseItemId));
    }

    private class TestKeyQuestionPartAnswerResponseItemModelData : KeyQuestionPartAnswerResponseItemModelData;
}