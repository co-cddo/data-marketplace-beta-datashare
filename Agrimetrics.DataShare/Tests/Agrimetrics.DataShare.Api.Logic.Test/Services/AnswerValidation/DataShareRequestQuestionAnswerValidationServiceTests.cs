using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests.Answers.DataShareRequestQuestionAnswers;
using Agrimetrics.DataShare.Api.Logic.ModelData.AnswerValidation;
using Agrimetrics.DataShare.Api.Logic.Repositories.AnswerValidation;
using Agrimetrics.DataShare.Api.Logic.Services.AnswerValidation;
using Agrimetrics.DataShare.Api.Logic.Services.AnswerValidation.Validation;
using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.Services.AnswerValidation;

[TestFixture]
public class DataShareRequestQuestionAnswerValidationServiceTests
{
    #region ValidateDataShareRequestQuestionAnswerAsync() Tests
    [Test]
    public void GivenANullDataShareRequestQuestionAnswer_WhenIValidateDataShareRequestQuestionAnswerAsync_ThenAnArgumentNullExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        Assert.That(() => testItems.DataShareRequestQuestionAnswerValidationService.ValidateDataShareRequestQuestionAnswerAsync(
                null!),
            Throws.ArgumentNullException.With.Property("ParamName").EqualTo("dataShareRequestQuestionAnswer"));
    }

    [Test]
    public async Task GivenADataShareRequestQuestionAnswer_WhenIValidateDataShareRequestQuestionAnswerAsync_ThenTheValidationRulesAreObtainedUsingTheDataShareRequestIdOfThatAnswer()
    {
        var testItems = CreateTestItems();

        var testDataShareRequestId = Guid.Parse("9B591A37-0CBF-46D5-8504-1F74A8747DAF");

        var dataShareRequestQuestionAnswer = testItems.Fixture.Build<DataShareRequestQuestionAnswer>()
            .With(x => x.DataShareRequestId, testDataShareRequestId)
            .Create();

        await testItems.DataShareRequestQuestionAnswerValidationService.ValidateDataShareRequestQuestionAnswerAsync(
            dataShareRequestQuestionAnswer);

        testItems.MockAnswerValidationRepository.Verify(x => x.GetQuestionPartAnswerValidationRulesAsync(
                testDataShareRequestId,
                It.IsAny<Guid>()),
            Times.AtLeastOnce);
    }

    [Test]
    public async Task GivenADataShareRequestQuestionAnswerWithANumberOfAnswerParts_WhenIValidateDataShareRequestQuestionAnswerAsync_ThenTheValidationRulesAreObtainedForTheQuestionOfEachAnswerPartInTheAnswer()
    {
        var testItems = CreateTestItems();

        var testDataShareRequestQuestionAnswerParts = testItems.Fixture.CreateMany<DataShareRequestQuestionAnswerPart>().ToList();

        var dataShareRequestQuestionAnswer = testItems.Fixture.Build<DataShareRequestQuestionAnswer>()
            .With(x => x.AnswerParts, testDataShareRequestQuestionAnswerParts)
            .Create();

        await testItems.DataShareRequestQuestionAnswerValidationService.ValidateDataShareRequestQuestionAnswerAsync(
            dataShareRequestQuestionAnswer);

        Assert.Multiple(() =>
        {
            foreach (var testDataShareRequestQuestionAnswerPart in testDataShareRequestQuestionAnswerParts)
            {
                testItems.MockAnswerValidationRepository.Verify(x => x.GetQuestionPartAnswerValidationRulesAsync(
                        It.IsAny<Guid>(),
                        testDataShareRequestQuestionAnswerPart.QuestionPartId),
                    Times.Once);
            }
        });
    }

    [Test]
    public async Task GivenADataShareRequestQuestionAnswerWithANumberOfAnswerPartsThatContainsValidationErrors_WhenIValidateDataShareRequestQuestionAnswerAsync_ThenTheValidationErrorsForThoseAnswerPartsAreReturned()
    {
        var testItems = CreateTestItems();

        var questionPart1Id = Guid.Parse("A2F736C2-848E-43AB-9BB2-2641FB6B5EA0");
        var questionPart2Id = Guid.Parse("8180E296-6FC5-47B8-B16E-22EF7A9B5DF4");
        var questionPart3Id = Guid.Parse("612D4628-0746-408A-B3C1-E64BCEB2D626");

        var questionAnswerPart1 = testItems.Fixture.Build<DataShareRequestQuestionAnswerPart>().With(x => x.QuestionPartId, questionPart1Id).Create();
        var questionAnswerPart2 = testItems.Fixture.Build<DataShareRequestQuestionAnswerPart>().With(x => x.QuestionPartId, questionPart2Id).Create();
        var questionAnswerPart3 = testItems.Fixture.Build<DataShareRequestQuestionAnswerPart>().With(x => x.QuestionPartId, questionPart3Id).Create();

        var dataShareRequestQuestionAnswer = testItems.Fixture.Build<DataShareRequestQuestionAnswer>()
            .With(x => x.AnswerParts, [questionAnswerPart1, questionAnswerPart2, questionAnswerPart3])
            .Create();

        var questionPartAnswerValidationRuleSetModelData1 = testItems.Fixture.Create<QuestionPartAnswerValidationRuleSetModelData>();
        var questionPartAnswerValidationRuleSetModelData2 = testItems.Fixture.Create<QuestionPartAnswerValidationRuleSetModelData>();
        var questionPartAnswerValidationRuleSetModelData3 = testItems.Fixture.Create<QuestionPartAnswerValidationRuleSetModelData>();

        testItems.MockAnswerValidationRepository.Setup(x => x.GetQuestionPartAnswerValidationRulesAsync(It.IsAny<Guid>(), questionPart1Id))
            .ReturnsAsync(questionPartAnswerValidationRuleSetModelData1);
        testItems.MockAnswerValidationRepository.Setup(x => x.GetQuestionPartAnswerValidationRulesAsync(It.IsAny<Guid>(), questionPart2Id))
            .ReturnsAsync(questionPartAnswerValidationRuleSetModelData2);
        testItems.MockAnswerValidationRepository.Setup(x => x.GetQuestionPartAnswerValidationRulesAsync(It.IsAny<Guid>(), questionPart3Id))
            .ReturnsAsync(questionPartAnswerValidationRuleSetModelData3);

        var answerPart1ValidationErrors = testItems.Fixture.CreateMany<SetDataShareRequestQuestionAnswerPartResponseValidationError>(2).ToList();
        var answerPart2ValidationErrors = testItems.Fixture.CreateMany<SetDataShareRequestQuestionAnswerPartResponseValidationError>(0).ToList();
        var answerPart3ValidationErrors = testItems.Fixture.CreateMany<SetDataShareRequestQuestionAnswerPartResponseValidationError>(1).ToList();

        testItems.MockQuestionPartAnswerValidation.Setup(x => x.ValidateQuestionPartAnswer(questionAnswerPart1, questionPartAnswerValidationRuleSetModelData1))
            .Returns(answerPart1ValidationErrors);
        testItems.MockQuestionPartAnswerValidation.Setup(x => x.ValidateQuestionPartAnswer(questionAnswerPart2, questionPartAnswerValidationRuleSetModelData2))
            .Returns(answerPart2ValidationErrors);
        testItems.MockQuestionPartAnswerValidation.Setup(x => x.ValidateQuestionPartAnswer(questionAnswerPart3, questionPartAnswerValidationRuleSetModelData3))
            .Returns(answerPart3ValidationErrors);

        var result = await testItems.DataShareRequestQuestionAnswerValidationService.ValidateDataShareRequestQuestionAnswerAsync(
            dataShareRequestQuestionAnswer);

        Assert.Multiple(() =>
        {
            Assert.That(result.AnswerIsValid, Is.False);

            var expectedValidationErrors = answerPart1ValidationErrors
                .Concat(answerPart2ValidationErrors)
                .Concat(answerPart3ValidationErrors);

            Assert.That(result.ValidationErrors, Is.EqualTo(expectedValidationErrors));
        });
    }

    [Test]
    public async Task GivenADataShareRequestQuestionAnswerWithNoAnswerPartsThatContainValidationErrors_WhenIValidateDataShareRequestQuestionAnswerAsync_ThenNoValidationErrorsAreReturned()
    {
        var testItems = CreateTestItems();

        testItems.MockQuestionPartAnswerValidation.Setup(x => x.ValidateQuestionPartAnswer(
                It.IsAny<DataShareRequestQuestionAnswerPart>(),
                It.IsAny<QuestionPartAnswerValidationRuleSetModelData>()))
            .Returns(Enumerable.Empty<SetDataShareRequestQuestionAnswerPartResponseValidationError>);

        var result = await testItems.DataShareRequestQuestionAnswerValidationService.ValidateDataShareRequestQuestionAnswerAsync(
            testItems.Fixture.Create<DataShareRequestQuestionAnswer>());

        Assert.Multiple(() =>
        {
            Assert.That(result.AnswerIsValid, Is.True);

            Assert.That(result.ValidationErrors, Is.Empty);
        });
    }
    #endregion

    #region Test Item Creation
    private static TestItems CreateTestItems()
    {
        var fixture = new Fixture().Customize(new AutoMoqCustomization());

        var mockAnswerValidationRepository = Mock.Get(fixture.Create<IAnswerValidationRepository>());
        var mockQuestionPartAnswerValidation = Mock.Get(fixture.Create<IQuestionPartAnswerValidation>());

        var dataShareRequestQuestionAnswerValidationService = new DataShareRequestQuestionAnswerValidationService(
            mockAnswerValidationRepository.Object,
            mockQuestionPartAnswerValidation.Object);

        return new TestItems(
            fixture,
            dataShareRequestQuestionAnswerValidationService,
            mockAnswerValidationRepository,
            mockQuestionPartAnswerValidation);
    }

    private class TestItems(
        IFixture fixture,
        IDataShareRequestQuestionAnswerValidationService dataShareRequestQuestionAnswerValidationService,
        Mock<IAnswerValidationRepository> mockAnswerValidationRepository,
        Mock<IQuestionPartAnswerValidation> mockQuestionPartAnswerValidation)
    {
        public IFixture Fixture { get; } = fixture;
        public IDataShareRequestQuestionAnswerValidationService DataShareRequestQuestionAnswerValidationService { get; } = dataShareRequestQuestionAnswerValidationService;
        public Mock<IAnswerValidationRepository> MockAnswerValidationRepository { get; } = mockAnswerValidationRepository;
        public Mock<IQuestionPartAnswerValidation> MockQuestionPartAnswerValidation { get; } = mockQuestionPartAnswerValidation;
    }
    #endregion
}