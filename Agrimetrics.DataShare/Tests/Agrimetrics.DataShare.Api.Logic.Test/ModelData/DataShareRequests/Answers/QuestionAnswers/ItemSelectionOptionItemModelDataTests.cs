using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests.Answers.DataShareRequestQuestionAnswers;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.ModelData.DataShareRequests.Answers.DsrRequestQuestionAnswers;

[TestFixture]
public class QuestionPartAnswerItemSelectionOptionItemModelDataTests
{
    [Test]
    public void GivenAOptionSelectionItemIdSelectionOptionItemModelData_WhenISetOptionSelectionItemId_ThenOptionSelectionItemIdIsSet()
    {
        var testQuestionPartAnswerItemSelectionOptionItemModelData = new QuestionPartAnswerItemSelectionOptionItemModelData();

        var testOptionSelectionItemId = new Guid("B05F3442-11D9-408A-887A-9F750E6E3EBD");

        testQuestionPartAnswerItemSelectionOptionItemModelData.QuestionPartAnswerItem_OptionSelectionItemId = testOptionSelectionItemId;

        var result = testQuestionPartAnswerItemSelectionOptionItemModelData.QuestionPartAnswerItem_OptionSelectionItemId;

        Assert.That(result, Is.EqualTo(testOptionSelectionItemId));
    }

    [Test]
    public void GivenAQuestionPartAnswerItemSelectionOptionItemModelData_WhenISetANullSupplementaryQuestionPartAnswerId_ThenSupplementaryQuestionPartAnswerIdIsSet()
    {
        var testQuestionPartAnswerItemSelectionOptionItemModelData = new QuestionPartAnswerItemSelectionOptionItemModelData();

        var testSupplementaryQuestionPartAnswerId = (Guid?) null;

        testQuestionPartAnswerItemSelectionOptionItemModelData.QuestionPartAnswerItem_SupplementaryQuestionPartAnswerId = testSupplementaryQuestionPartAnswerId;

        var result = testQuestionPartAnswerItemSelectionOptionItemModelData.QuestionPartAnswerItem_SupplementaryQuestionPartAnswerId;

        Assert.That(result, Is.EqualTo(testSupplementaryQuestionPartAnswerId));
    }

    [Test]
    public void GivenAQuestionPartAnswerItemSelectionOptionItemModelData_WhenISetSupplementaryQuestionPartAnswerId_ThenSupplementaryQuestionPartAnswerIdIsSet()
    {
        var testQuestionPartAnswerItemSelectionOptionItemModelData = new QuestionPartAnswerItemSelectionOptionItemModelData();

        var testSupplementaryQuestionPartAnswerId = new Guid("B05F3442-11D9-408A-887A-9F750E6E3EBD");

        testQuestionPartAnswerItemSelectionOptionItemModelData.QuestionPartAnswerItem_SupplementaryQuestionPartAnswerId = testSupplementaryQuestionPartAnswerId;

        var result = testQuestionPartAnswerItemSelectionOptionItemModelData.QuestionPartAnswerItem_SupplementaryQuestionPartAnswerId;

        Assert.That(result, Is.EqualTo(testSupplementaryQuestionPartAnswerId));
    }

    [Test]
    public void GivenAQuestionPartAnswerItemSelectionOptionItemModelData_WhenISetANullSupplementaryQuestionPartAnswer_ThenSupplementaryQuestionPartAnswerIsSet()
    {
        var testQuestionPartAnswerItemSelectionOptionItemModelData = new QuestionPartAnswerItemSelectionOptionItemModelData();

        var testSupplementaryQuestionPartAnswer = (QuestionPartAnswerModelData?) null;

        testQuestionPartAnswerItemSelectionOptionItemModelData.QuestionPartAnswerItem_SupplementaryQuestionPartAnswer = testSupplementaryQuestionPartAnswer;

        var result = testQuestionPartAnswerItemSelectionOptionItemModelData.QuestionPartAnswerItem_SupplementaryQuestionPartAnswer;

        Assert.That(result, Is.EqualTo(testSupplementaryQuestionPartAnswer));
    }

    [Test]
    public void GivenAQuestionPartAnswerItemSelectionOptionItemModelData_WhenISetSupplementaryQuestionPartAnswer_ThenSupplementaryQuestionPartAnswerIsSet()
    {
        var testQuestionPartAnswerItemSelectionOptionItemModelData = new QuestionPartAnswerItemSelectionOptionItemModelData();

        var testSupplementaryQuestionPartAnswer = new QuestionPartAnswerModelData();

        testQuestionPartAnswerItemSelectionOptionItemModelData.QuestionPartAnswerItem_SupplementaryQuestionPartAnswer = testSupplementaryQuestionPartAnswer;

        var result = testQuestionPartAnswerItemSelectionOptionItemModelData.QuestionPartAnswerItem_SupplementaryQuestionPartAnswer;

        Assert.That(result, Is.EqualTo(testSupplementaryQuestionPartAnswer));
    }
}

