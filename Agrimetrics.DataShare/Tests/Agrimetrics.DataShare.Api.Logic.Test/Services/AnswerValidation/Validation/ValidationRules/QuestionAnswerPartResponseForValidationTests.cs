using Agrimetrics.DataShare.Api.Logic.Services.AnswerValidation.Validation.ValidationRules;
using AutoFixture.AutoMoq;
using AutoFixture;
using Moq;
using NUnit.Framework;
using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests.Answers.DataShareRequestQuestionAnswers;

namespace Agrimetrics.DataShare.Api.Logic.Test.Services.AnswerValidation.Validation.ValidationRules;

[TestFixture]
public class QuestionAnswerPartResponseForValidationTests
{
    [Theory]
    public void GivenAQuestionAnswerPartIsOptionalValue_WhenIConstructAnInstanceOfQuestionAnswerPartResponseForValidation_ThenQuestionAnswerPartIsOptionalIsConfiguredToTheGivenValue(
        bool questionAnswerPartIsOptional)
    {
        var questionAnswerPartResponseForValidation = new QuestionAnswerPartResponseForValidation
        {
            QuestionAnswerPartResponse = It.IsAny<DataShareRequestQuestionAnswerPartResponseBase>(),
            QuestionAnswerPartIsOptional = questionAnswerPartIsOptional
        };

        Assert.That(questionAnswerPartResponseForValidation.QuestionAnswerPartIsOptional, Is.EqualTo(questionAnswerPartIsOptional));
    }

    [Test]
    public void GivenAQuestionAnswerPartResponse_WhenIConstructAnInstanceOfQuestionAnswerPartResponseForValidation_ThenQuestionAnswerPartResponseIsConfiguredToTheGivenValue()
    {
        var testItems = CreateTestItems();

        var testQuestionAnswerPartResponse = testItems.Fixture.Create<DataShareRequestQuestionAnswerPartResponseBase>();

        var questionAnswerPartResponseForValidation = new QuestionAnswerPartResponseForValidation
        {
            QuestionAnswerPartResponse = testQuestionAnswerPartResponse,
            QuestionAnswerPartIsOptional = It.IsAny<bool>()
        };

        Assert.That(questionAnswerPartResponseForValidation.QuestionAnswerPartResponse, Is.EqualTo(testQuestionAnswerPartResponse));
    }

    #region Test Item Creation
    private static TestItems CreateTestItems()
    {
        var fixture = new Fixture().Customize(new AutoMoqCustomization());

        return new TestItems(
            fixture);
    }

    private class TestItems(
        IFixture fixture)
    {
        public IFixture Fixture { get; } = fixture;
    }
    #endregion
}