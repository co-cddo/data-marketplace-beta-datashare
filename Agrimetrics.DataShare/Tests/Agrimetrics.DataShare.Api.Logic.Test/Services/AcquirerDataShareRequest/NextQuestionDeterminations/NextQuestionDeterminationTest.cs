using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests.QuestionStatusDeterminations;
using Agrimetrics.DataShare.Api.Logic.ModelData.Questions;
using Agrimetrics.DataShare.Api.Logic.Services.AcquirerDataShareRequest.NextQuestionDeterminations;
using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.Services.AcquirerDataShareRequest.NextQuestionDeterminations
{
    [TestFixture]
    public class NextQuestionDeterminationTest
    {
        #region DetermineNextQuestion() Tests
        #region Input Parameter Validation
        [Test]
        public void GivenANullSetOfQuestionStatuses_WhenIDetermineNextQuestion_ThenAnArgumentNullExceptionIsThrown()
        {
            var testItems = CreateTestItems();

            Assert.That(() => testItems.NextQuestionDetermination.DetermineNextQuestion(
                    testItems.Fixture.Create<Guid>(),
                    null!),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("questionStatuses"));
        }

        [Test]
        public void GivenACurrentQuestionIdThatIsNotInTheGivenSetOfQuestionStatuses_WhenIDetermineNextQuestion_ThenAnInvalidOperationExceptionIsThrown()
        {
            var testItems = CreateTestItems();

            var currentQuestionId = Guid.Parse("ED40CF6D-038C-4BFB-8493-6A92D2FCDD1E");

            var questionStatusIds = new Queue<Guid>(new List<Guid>
            {
                Guid.Parse("CB4A9920-34E4-479F-8463-379AE0A34865"),
                Guid.Parse("21E7E4A5-499C-4B0F-8D20-8177926AABA0"),
                Guid.Parse("D6D200BB-B9AD-48AB-A8A5-66C78084440D")
            });

            var questionStatuses = testItems.Fixture.Build<DataShareRequestQuestionSetQuestionStatusDataModel>()
                .With(x => x.QuestionId, () => questionStatusIds.Dequeue())
                .CreateMany(questionStatusIds.Count);

            Assert.That(() => testItems.NextQuestionDetermination.DetermineNextQuestion(currentQuestionId, questionStatuses),
                Throws.InvalidOperationException.With.Message.EqualTo("Current Question Id is not found in given Question Statuses"));
        }
        #endregion

        #region Question Set Ordering
        [Test]
        [TestCase("8F50E6E5-78C8-4656-8E8E-A6C5AA7D5893", "7C718204-0A8F-412C-ACE6-944459918DBD")]
        [TestCase("7C718204-0A8F-412C-ACE6-944459918DBD", "EDF29D61-C937-46B9-85F5-1FE69C4219D3")]
        [TestCase("EDF29D61-C937-46B9-85F5-1FE69C4219D3", "88B45F57-6865-49BC-A2F5-5986F1B637EB")]
        [TestCase("88B45F57-6865-49BC-A2F5-5986F1B637EB", "3289DBFC-BAF5-415D-B313-92726ED3E94B")]
        public void GivenACurrentQuestionInAnUnOrderedSetOfQuestionStatuses_WhenIDetermineNextQuestion_ThenTheIdOfTheNextQuestionInTheOrderedSetIsReturned(
            string currentQuestionId,
            string expectedNextQuestionId)
        {
            var testItems = CreateTestItems();

            var workableQuestionStatus = WorkableQuestionStatuses.First();

            var testQuestionStatuses = new List<IDataShareRequestQuestionSetQuestionStatusDataModel>
            {
                CreateTestDataShareRequestQuestionStatusDataModel(Guid.Parse("EDF29D61-C937-46B9-85F5-1FE69C4219D3"), 2, 3, workableQuestionStatus),
                CreateTestDataShareRequestQuestionStatusDataModel(Guid.Parse("7C718204-0A8F-412C-ACE6-944459918DBD"), 2, 1, workableQuestionStatus),
                CreateTestDataShareRequestQuestionStatusDataModel(Guid.Parse("8F50E6E5-78C8-4656-8E8E-A6C5AA7D5893"), 1, 1, workableQuestionStatus),
                CreateTestDataShareRequestQuestionStatusDataModel(Guid.Parse("3289DBFC-BAF5-415D-B313-92726ED3E94B"), 3, 5, workableQuestionStatus),
                CreateTestDataShareRequestQuestionStatusDataModel(Guid.Parse("88B45F57-6865-49BC-A2F5-5986F1B637EB"), 3, 2, workableQuestionStatus),
            };

            var result = testItems.NextQuestionDetermination.DetermineNextQuestion(
                Guid.Parse(currentQuestionId),
                testQuestionStatuses);

            Assert.That(result, Is.EqualTo(Guid.Parse(expectedNextQuestionId)));
        }

        [Test]
        public void GivenACurrentQuestionAtTheEndOfAnUnOrderedSetOfQuestionStatuses_WhenIDetermineNextQuestion_ThenANullQuestionIdIsReturned()
        {
            var testItems = CreateTestItems();

            var workableQuestionStatus = WorkableQuestionStatuses.First();

            var testQuestionStatuses = new List<IDataShareRequestQuestionSetQuestionStatusDataModel>
            {
                CreateTestDataShareRequestQuestionStatusDataModel(Guid.Parse("EDF29D61-C937-46B9-85F5-1FE69C4219D3"), 2, 3, workableQuestionStatus),
                CreateTestDataShareRequestQuestionStatusDataModel(Guid.Parse("7C718204-0A8F-412C-ACE6-944459918DBD"), 2, 1, workableQuestionStatus),
                CreateTestDataShareRequestQuestionStatusDataModel(Guid.Parse("8F50E6E5-78C8-4656-8E8E-A6C5AA7D5893"), 1, 1, workableQuestionStatus),
                CreateTestDataShareRequestQuestionStatusDataModel(Guid.Parse("3289DBFC-BAF5-415D-B313-92726ED3E94B"), 3, 5, workableQuestionStatus),
                CreateTestDataShareRequestQuestionStatusDataModel(Guid.Parse("88B45F57-6865-49BC-A2F5-5986F1B637EB"), 3, 2, workableQuestionStatus),
            };

            var result = testItems.NextQuestionDetermination.DetermineNextQuestion(
                Guid.Parse("3289DBFC-BAF5-415D-B313-92726ED3E94B"),
                testQuestionStatuses);

            Assert.That(result, Is.Null);
        }
        #endregion

        #region Current Question Immediately Succeeded By Workable Question
        [Test]
        public void GivenTheCurrentQuestionIsTheFirstInTheQuestionSetAndIsImmediatelySucceededByAWorkableQuestion_WhenIDetermineNextQuestion_ThenTheIdOfTheNextQuestionIsReturned(
            [ValueSource(nameof(WorkableQuestionStatuses))] QuestionStatusType questionStatusType)
        {
            var testItems = CreateTestItems();

            var testQuestionStatuses = new List<IDataShareRequestQuestionSetQuestionStatusDataModel>
            {
                CreateTestDataShareRequestQuestionStatusDataModel(Guid.Parse("8F50E6E5-78C8-4656-8E8E-A6C5AA7D5893"), 1, 1, questionStatusType),
                CreateTestDataShareRequestQuestionStatusDataModel(Guid.Parse("7C718204-0A8F-412C-ACE6-944459918DBD"), 2, 1, questionStatusType),
                CreateTestDataShareRequestQuestionStatusDataModel(Guid.Parse("EDF29D61-C937-46B9-85F5-1FE69C4219D3"), 2, 2, questionStatusType),
                CreateTestDataShareRequestQuestionStatusDataModel(Guid.Parse("88B45F57-6865-49BC-A2F5-5986F1B637EB"), 3, 1, questionStatusType),
                CreateTestDataShareRequestQuestionStatusDataModel(Guid.Parse("3289DBFC-BAF5-415D-B313-92726ED3E94B"), 3, 2, questionStatusType)
            };

            var result = testItems.NextQuestionDetermination.DetermineNextQuestion(
                Guid.Parse("8F50E6E5-78C8-4656-8E8E-A6C5AA7D5893"),
                testQuestionStatuses);

            Assert.That(result, Is.EqualTo(Guid.Parse("7C718204-0A8F-412C-ACE6-944459918DBD")));
        }

        [Test]
        public void GivenTheCurrentQuestionIsWithinTheQuestionSetAndIsImmediatelySucceededByAWorkableQuestion_WhenIDetermineNextQuestion_ThenTheIdOfTheNextQuestionIsReturned(
            [ValueSource(nameof(WorkableQuestionStatuses))] QuestionStatusType questionStatusType)
        {
            var testItems = CreateTestItems();

            var testQuestionStatuses = new List<IDataShareRequestQuestionSetQuestionStatusDataModel>
            {
                CreateTestDataShareRequestQuestionStatusDataModel(Guid.Parse("8F50E6E5-78C8-4656-8E8E-A6C5AA7D5893"), 1, 1, questionStatusType),
                CreateTestDataShareRequestQuestionStatusDataModel(Guid.Parse("7C718204-0A8F-412C-ACE6-944459918DBD"), 2, 1, questionStatusType),
                CreateTestDataShareRequestQuestionStatusDataModel(Guid.Parse("EDF29D61-C937-46B9-85F5-1FE69C4219D3"), 2, 2, questionStatusType),
                CreateTestDataShareRequestQuestionStatusDataModel(Guid.Parse("88B45F57-6865-49BC-A2F5-5986F1B637EB"), 3, 1, questionStatusType),
                CreateTestDataShareRequestQuestionStatusDataModel(Guid.Parse("3289DBFC-BAF5-415D-B313-92726ED3E94B"), 3, 2, questionStatusType)
            };

            var result = testItems.NextQuestionDetermination.DetermineNextQuestion(
                Guid.Parse("EDF29D61-C937-46B9-85F5-1FE69C4219D3"),
                testQuestionStatuses);

            Assert.That(result, Is.EqualTo(Guid.Parse("88B45F57-6865-49BC-A2F5-5986F1B637EB")));
        }

        [Test]
        public void GivenTheCurrentQuestionIsAtTheEndOfTheQuestionSetAndIsImmediatelySucceededByAWorkableQuestion_WhenIDetermineNextQuestion_ThenANullQuestionIdIsReturned(
            [ValueSource(nameof(WorkableQuestionStatuses))] QuestionStatusType questionStatusType)
        {
            var testItems = CreateTestItems();

            var testQuestionStatuses = new List<IDataShareRequestQuestionSetQuestionStatusDataModel>
            {
                CreateTestDataShareRequestQuestionStatusDataModel(Guid.Parse("8F50E6E5-78C8-4656-8E8E-A6C5AA7D5893"), 1, 1, questionStatusType),
                CreateTestDataShareRequestQuestionStatusDataModel(Guid.Parse("7C718204-0A8F-412C-ACE6-944459918DBD"), 2, 1, questionStatusType),
                CreateTestDataShareRequestQuestionStatusDataModel(Guid.Parse("EDF29D61-C937-46B9-85F5-1FE69C4219D3"), 2, 2, questionStatusType),
                CreateTestDataShareRequestQuestionStatusDataModel(Guid.Parse("88B45F57-6865-49BC-A2F5-5986F1B637EB"), 3, 1, questionStatusType),
                CreateTestDataShareRequestQuestionStatusDataModel(Guid.Parse("3289DBFC-BAF5-415D-B313-92726ED3E94B"), 3, 2, questionStatusType)
            };

            var result = testItems.NextQuestionDetermination.DetermineNextQuestion(
                Guid.Parse("3289DBFC-BAF5-415D-B313-92726ED3E94B"),
                testQuestionStatuses);

            Assert.That(result, Is.Null);
        }
        #endregion

        #region Current Question Not Immediately Succeeded By Workable Question
        [Test]
        public void GivenTheCurrentQuestionIsTheFirstInTheQuestionSetAndIsNotImmediatelySucceededByAWorkableQuestion_WhenIDetermineNextQuestion_ThenTheIdOfTheNextWorkableQuestionIsReturned(
            [ValueSource(nameof(WorkableQuestionStatuses))] QuestionStatusType workableQuestionStatusType,
            [ValueSource(nameof(NonWorkableQuestionStatuses))] QuestionStatusType nonWorkableQuestionStatusType)
        {
            var testItems = CreateTestItems();

            var testQuestionStatuses = new List<IDataShareRequestQuestionSetQuestionStatusDataModel>
            {
                CreateTestDataShareRequestQuestionStatusDataModel(Guid.Parse("8F50E6E5-78C8-4656-8E8E-A6C5AA7D5893"), 1, 1, It.IsAny<QuestionStatusType>()),
                CreateTestDataShareRequestQuestionStatusDataModel(Guid.Parse("7C718204-0A8F-412C-ACE6-944459918DBD"), 2, 1, nonWorkableQuestionStatusType),
                CreateTestDataShareRequestQuestionStatusDataModel(Guid.Parse("EDF29D61-C937-46B9-85F5-1FE69C4219D3"), 2, 2, nonWorkableQuestionStatusType),
                CreateTestDataShareRequestQuestionStatusDataModel(Guid.Parse("88B45F57-6865-49BC-A2F5-5986F1B637EB"), 3, 1, workableQuestionStatusType),
                CreateTestDataShareRequestQuestionStatusDataModel(Guid.Parse("3289DBFC-BAF5-415D-B313-92726ED3E94B"), 3, 2, workableQuestionStatusType)
            };

            var result = testItems.NextQuestionDetermination.DetermineNextQuestion(
                Guid.Parse("8F50E6E5-78C8-4656-8E8E-A6C5AA7D5893"),
                testQuestionStatuses);

            Assert.That(result, Is.EqualTo(Guid.Parse("88B45F57-6865-49BC-A2F5-5986F1B637EB")));
        }

        [Test]
        public void GivenTheCurrentQuestionIsWithinTheQuestionSetAndIsNotImmediatelySucceededByAWorkableQuestion_WhenIDetermineNextQuestion_ThenTheIdOfTheNextWorkableQuestionIsReturned(
            [ValueSource(nameof(WorkableQuestionStatuses))] QuestionStatusType workableQuestionStatusType,
            [ValueSource(nameof(NonWorkableQuestionStatuses))] QuestionStatusType nonWorkableQuestionStatusType)
        {
            var testItems = CreateTestItems();

            var testQuestionStatuses = new List<IDataShareRequestQuestionSetQuestionStatusDataModel>
            {
                CreateTestDataShareRequestQuestionStatusDataModel(Guid.Parse("8F50E6E5-78C8-4656-8E8E-A6C5AA7D5893"), 1, 1, It.IsAny<QuestionStatusType>()),
                CreateTestDataShareRequestQuestionStatusDataModel(Guid.Parse("7C718204-0A8F-412C-ACE6-944459918DBD"), 2, 1, It.IsAny<QuestionStatusType>()),
                CreateTestDataShareRequestQuestionStatusDataModel(Guid.Parse("EDF29D61-C937-46B9-85F5-1FE69C4219D3"), 2, 2, It.IsAny<QuestionStatusType>()),
                CreateTestDataShareRequestQuestionStatusDataModel(Guid.Parse("88B45F57-6865-49BC-A2F5-5986F1B637EB"), 3, 1, nonWorkableQuestionStatusType),
                CreateTestDataShareRequestQuestionStatusDataModel(Guid.Parse("3289DBFC-BAF5-415D-B313-92726ED3E94B"), 3, 2, workableQuestionStatusType)
            };

            var result = testItems.NextQuestionDetermination.DetermineNextQuestion(
                Guid.Parse("EDF29D61-C937-46B9-85F5-1FE69C4219D3"),
                testQuestionStatuses);

            Assert.That(result, Is.EqualTo(Guid.Parse("3289DBFC-BAF5-415D-B313-92726ED3E94B")));
        }
        #endregion

        #region Current Not Succeeded By Workable Question
        [Test]
        public void GivenTheCurrentQuestionIsTheFirstInTheQuestionSetAndIsNotSucceededByAWorkableQuestion_WhenIDetermineNextQuestion_ThenANullQuestionIdIsReturned(
            [ValueSource(nameof(NonWorkableQuestionStatuses))] QuestionStatusType nonWorkableQuestionStatusType)
        {
            var testItems = CreateTestItems();

            var testQuestionStatuses = new List<IDataShareRequestQuestionSetQuestionStatusDataModel>
            {
                CreateTestDataShareRequestQuestionStatusDataModel(Guid.Parse("8F50E6E5-78C8-4656-8E8E-A6C5AA7D5893"), 1, 1, It.IsAny<QuestionStatusType>()),
                CreateTestDataShareRequestQuestionStatusDataModel(Guid.Parse("7C718204-0A8F-412C-ACE6-944459918DBD"), 2, 1, nonWorkableQuestionStatusType),
                CreateTestDataShareRequestQuestionStatusDataModel(Guid.Parse("EDF29D61-C937-46B9-85F5-1FE69C4219D3"), 2, 2, nonWorkableQuestionStatusType),
                CreateTestDataShareRequestQuestionStatusDataModel(Guid.Parse("88B45F57-6865-49BC-A2F5-5986F1B637EB"), 3, 1, nonWorkableQuestionStatusType),
                CreateTestDataShareRequestQuestionStatusDataModel(Guid.Parse("3289DBFC-BAF5-415D-B313-92726ED3E94B"), 3, 2, nonWorkableQuestionStatusType)
            };

            var result = testItems.NextQuestionDetermination.DetermineNextQuestion(
                Guid.Parse("8F50E6E5-78C8-4656-8E8E-A6C5AA7D5893"),
                testQuestionStatuses);

            Assert.That(result, Is.Null);
        }

        [Test]
        public void GivenTheCurrentQuestionIsWithinTheQuestionSetAndIsNotSucceededByAWorkableQuestion_WhenIDetermineNextQuestion_ThenANullQuestionIdIsReturned(
            [ValueSource(nameof(NonWorkableQuestionStatuses))] QuestionStatusType nonWorkableQuestionStatusType)
        {
            var testItems = CreateTestItems();

            var testQuestionStatuses = new List<IDataShareRequestQuestionSetQuestionStatusDataModel>
            {
                CreateTestDataShareRequestQuestionStatusDataModel(Guid.Parse("8F50E6E5-78C8-4656-8E8E-A6C5AA7D5893"), 1, 1, It.IsAny<QuestionStatusType>()),
                CreateTestDataShareRequestQuestionStatusDataModel(Guid.Parse("7C718204-0A8F-412C-ACE6-944459918DBD"), 2, 1, It.IsAny<QuestionStatusType>()),
                CreateTestDataShareRequestQuestionStatusDataModel(Guid.Parse("EDF29D61-C937-46B9-85F5-1FE69C4219D3"), 2, 2, It.IsAny<QuestionStatusType>()),
                CreateTestDataShareRequestQuestionStatusDataModel(Guid.Parse("88B45F57-6865-49BC-A2F5-5986F1B637EB"), 3, 1, nonWorkableQuestionStatusType),
                CreateTestDataShareRequestQuestionStatusDataModel(Guid.Parse("3289DBFC-BAF5-415D-B313-92726ED3E94B"), 3, 2, nonWorkableQuestionStatusType)
            };

            var result = testItems.NextQuestionDetermination.DetermineNextQuestion(
                Guid.Parse("EDF29D61-C937-46B9-85F5-1FE69C4219D3"),
                testQuestionStatuses);

            Assert.That(result, Is.Null);
        }
        #endregion
        #endregion

        #region Test Data Creation
        private static IEnumerable<QuestionStatusType> NonWorkableQuestionStatuses => AllQuestionStatuses.Except(WorkableQuestionStatuses).ToList();

        private static IEnumerable<QuestionStatusType> WorkableQuestionStatuses => [QuestionStatusType.NotStarted, QuestionStatusType.Completed, QuestionStatusType.NoResponseNeeded];

        private static IEnumerable<QuestionStatusType> AllQuestionStatuses => Enum.GetValues<QuestionStatusType>().ToList();

        private static IDataShareRequestQuestionSetQuestionStatusDataModel CreateTestDataShareRequestQuestionStatusDataModel(
            Guid questionId,
            int sectionNumber,
            int questionOrderWithinSection,
            QuestionStatusType questionStatus)
        {
            var mockDataShareRequestQuestionStatusDataModel = new Mock<IDataShareRequestQuestionSetQuestionStatusDataModel>();

            mockDataShareRequestQuestionStatusDataModel.SetupGet(x => x.QuestionId).Returns(questionId);
            mockDataShareRequestQuestionStatusDataModel.SetupGet(x => x.SectionNumber).Returns(sectionNumber);
            mockDataShareRequestQuestionStatusDataModel.SetupGet(x => x.QuestionOrderWithinSection).Returns(questionOrderWithinSection);
            mockDataShareRequestQuestionStatusDataModel.SetupGet(x => x.QuestionStatus).Returns(questionStatus);

            return mockDataShareRequestQuestionStatusDataModel.Object;
        }
        #endregion

        #region Test Item Creation
        private static TestItems CreateTestItems()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());

            var nextQuestionDetermination = new NextQuestionDetermination();

            return new TestItems(fixture,
                nextQuestionDetermination);
        }

        private class TestItems(
            IFixture fixture,
            INextQuestionDetermination nextQuestionDetermination)
        {
            public IFixture Fixture { get; } = fixture;
            public INextQuestionDetermination NextQuestionDetermination { get; } = nextQuestionDetermination;
        }
        #endregion
    }
}
