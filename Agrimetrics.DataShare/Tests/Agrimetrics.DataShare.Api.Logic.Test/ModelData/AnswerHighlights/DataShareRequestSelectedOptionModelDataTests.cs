using Agrimetrics.DataShare.Api.Logic.ModelData.AnswerHighlights;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.ModelData.AnswerHighlights;

[TestFixture]
public class DataShareRequestSelectedOptionModelDataTests
{
    [Test]
    public void GivenADataShareRequestSelectedOptionModelData_WhenISetQuestionPartId_ThenQuestionPartIdIsSet()
    {
        var testDataShareRequestSelectedOptionModelData = new DataShareRequestSelectedOptionModelData();

        var testQuestionPartId = new Guid("897609FC-779D-431F-B380-3B3D1A4A014F");

        testDataShareRequestSelectedOptionModelData.DataShareRequestSelectedOption_QuestionPartId = testQuestionPartId;

        var result = testDataShareRequestSelectedOptionModelData.DataShareRequestSelectedOption_QuestionPartId;

        Assert.That(result, Is.EqualTo(testQuestionPartId));
    }

    [Test]
    public void GivenADataShareRequestSelectedOptionModelData_WhenISetAnswerPartId_ThenAnswerPartIdIsSet()
    {
        var testDataShareRequestSelectedOptionModelData = new DataShareRequestSelectedOptionModelData();

        var testAnswerPartId = new Guid("897609FC-779D-431F-B380-3B3D1A4A014F");

        testDataShareRequestSelectedOptionModelData.DataShareRequestSelectedOption_AnswerPartId = testAnswerPartId;

        var result = testDataShareRequestSelectedOptionModelData.DataShareRequestSelectedOption_AnswerPartId;

        Assert.That(result, Is.EqualTo(testAnswerPartId));
    }

    [Test]
    public void GivenADataShareRequestSelectedOptionModelData_WhenISetAnswerPartResponseId_ThenAnswerPartResponseIdIsSet()
    {
        var testDataShareRequestSelectedOptionModelData = new DataShareRequestSelectedOptionModelData();

        var testAnswerPartResponseId = new Guid("897609FC-779D-431F-B380-3B3D1A4A014F");

        testDataShareRequestSelectedOptionModelData.DataShareRequestSelectedOption_AnswerPartResponseId = testAnswerPartResponseId;

        var result = testDataShareRequestSelectedOptionModelData.DataShareRequestSelectedOption_AnswerPartResponseId;

        Assert.That(result, Is.EqualTo(testAnswerPartResponseId));
    }

    [Test]
    public void GivenADataShareRequestSelectedOptionModelData_WhenISetAnswerPartResponseItemId_ThenAnswerPartResponseItemIdIsSet()
    {
        var testDataShareRequestSelectedOptionModelData = new DataShareRequestSelectedOptionModelData();

        var testAnswerPartResponseItemId = new Guid("897609FC-779D-431F-B380-3B3D1A4A014F");

        testDataShareRequestSelectedOptionModelData.DataShareRequestSelectedOption_AnswerPartResponseItemId = testAnswerPartResponseItemId;

        var result = testDataShareRequestSelectedOptionModelData.DataShareRequestSelectedOption_AnswerPartResponseItemId;

        Assert.That(result, Is.EqualTo(testAnswerPartResponseItemId));
    }

    [Test]
    public void GivenADataShareRequestSelectedOptionModelData_WhenISetAnswerPartResponseItemSelectionOptionId_ThenAnswerPartResponseItemSelectionOptionIdIsSet()
    {
        var testDataShareRequestSelectedOptionModelData = new DataShareRequestSelectedOptionModelData();

        var testAnswerPartResponseItemSelectionOptionId = new Guid("897609FC-779D-431F-B380-3B3D1A4A014F");

        testDataShareRequestSelectedOptionModelData.DataShareRequestSelectedOption_AnswerPartResponseItemSelectionOptionId = testAnswerPartResponseItemSelectionOptionId;

        var result = testDataShareRequestSelectedOptionModelData.DataShareRequestSelectedOption_AnswerPartResponseItemSelectionOptionId;

        Assert.That(result, Is.EqualTo(testAnswerPartResponseItemSelectionOptionId));
    }

    [Test]
    public void GivenADataShareRequestSelectedOptionModelData_WhenISetOptionSelectionId_ThenOptionSelectionIdIsSet()
    {
        var testDataShareRequestSelectedOptionModelData = new DataShareRequestSelectedOptionModelData();

        var testOptionSelectionId = new Guid("897609FC-779D-431F-B380-3B3D1A4A014F");

        testDataShareRequestSelectedOptionModelData.DataShareRequestSelectedOption_OptionSelectionId = testOptionSelectionId;

        var result = testDataShareRequestSelectedOptionModelData.DataShareRequestSelectedOption_OptionSelectionId;

        Assert.That(result, Is.EqualTo(testOptionSelectionId));
    }
}

