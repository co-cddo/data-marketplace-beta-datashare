using AutoFixture.AutoMoq;
using AutoFixture;
using NUnit.Framework;
using Agrimetrics.DataShare.Api.Controllers.QuestionConfiguration;
using Agrimetrics.DataShare.Api.Dto.Requests.QuestionConfiguration.CompulsoryQuestions;
using Agrimetrics.DataShare.Api.Dto.Models.QuestionConfiguration.CompulsoryQuestions;

namespace Agrimetrics.DataShare.Api.Test.Controllers.QuestionConfiguration
{
    [TestFixture]
    public class QuestionConfigurationResponseFactoryTests
    {
        #region CreateGetCompulsoryQuestionsResponse() Tests
        [Test]
        public void GivenANullGetCompulsoryQuestionsRequest_WhenICreateGetCompulsoryQuestionsResponse_ThenAnArgumentNullExceptionIsThrown()
        {
            var testItems = CreateTestItems();

            Assert.That(() => testItems.QuestionConfigurationResponseFactory.CreateGetCompulsoryQuestionsResponse(
                    null!, testItems.Fixture.Create<CompulsoryQuestionSet>()),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("getCompulsoryQuestionsRequest"));
        }

        [Test]
        public void GivenANullCompulsoryQuestionSet_WhenICreateGetCompulsoryQuestionsResponse_ThenAnArgumentNullExceptionIsThrown()
        {
            var testItems = CreateTestItems();

            Assert.That(() => testItems.QuestionConfigurationResponseFactory.CreateGetCompulsoryQuestionsResponse(
                    testItems.Fixture.Create<GetCompulsoryQuestionsRequest>(), null!),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("compulsoryQuestionSet"));
        }

        [Test]
        public void GivenAGetCompulsoryQuestionsRequest_WhenICreateGetCompulsoryQuestionsResponse_ThenAGetCompulsoryQuestionsResponseIsCreatedUsingTheValuesInTheRequest()
        {
            var testItems = CreateTestItems();

            var testGetCompulsoryQuestionsRequest = testItems.Fixture.Create<GetCompulsoryQuestionsRequest>();

            var result = testItems.QuestionConfigurationResponseFactory.CreateGetCompulsoryQuestionsResponse(
                testGetCompulsoryQuestionsRequest, testItems.Fixture.Create<CompulsoryQuestionSet>());

            Assert.That(result.RequestingUserId, Is.EqualTo(testGetCompulsoryQuestionsRequest.RequestingUserId));
        }

        [Test]
        public void GivenACompulsoryQuestionSet_WhenICreateGetCompulsoryQuestionsResponse_ThenAGetCompulsoryQuestionsResponseIsCreatedUsingTheCompulsoryQuestionSet()
        {
            var testItems = CreateTestItems();

            var testCompulsoryQuestionSet = testItems.Fixture.Create<CompulsoryQuestionSet>();

            var result = testItems.QuestionConfigurationResponseFactory.CreateGetCompulsoryQuestionsResponse(
                testItems.Fixture.Create<GetCompulsoryQuestionsRequest>(), testCompulsoryQuestionSet);

            Assert.That(result.CompulsoryQuestionSet, Is.EqualTo(testCompulsoryQuestionSet));
        }
        #endregion

        #region CreateSetCompulsoryQuestionResponse() Tests
        [Test]
        public void GivenANullSetCompulsoryQuestionRequest_WhenICreateSetCompulsoryQuestionResponse_ThenAnArgumentNullExceptionIsThrown()
        {
            var testItems = CreateTestItems();

            Assert.That(() => testItems.QuestionConfigurationResponseFactory.CreateSetCompulsoryQuestionResponse(
                    null!),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("setCompulsoryQuestionRequest"));
        }

        [Test]
        public void GivenASetCompulsoryQuestionRequest_WhenICreateSetCompulsoryQuestionResponse_ThenASetCompulsoryQuestionResponseIsCreatedUsingTheValuesInTheRequest()
        {
            var testItems = CreateTestItems();

            var testSetCompulsoryQuestionRequest = testItems.Fixture.Create<SetCompulsoryQuestionRequest>();

            var result = testItems.QuestionConfigurationResponseFactory.CreateSetCompulsoryQuestionResponse(
                testSetCompulsoryQuestionRequest);

            Assert.Multiple(() =>
            {
                Assert.That(result.RequestingUserId, Is.EqualTo(testSetCompulsoryQuestionRequest.RequestingUserId));
                Assert.That(result.QuestionId, Is.EqualTo(testSetCompulsoryQuestionRequest.QuestionId));
            });
        }
        #endregion

        #region CreateClearCompulsoryQuestionResponse() Tests
        [Test]
        public void GivenANullClearCompulsoryQuestionRequest_WhenICreateClearCompulsoryQuestionResponse_ThenAnArgumentNullExceptionIsThrown()
        {
            var testItems = CreateTestItems();

            Assert.That(() => testItems.QuestionConfigurationResponseFactory.CreateClearCompulsoryQuestionResponse(
                    null!),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("clearCompulsoryQuestionRequest"));
        }

        [Test]
        public void GivenAClearCompulsoryQuestionRequest_WhenICreateClearCompulsoryQuestionResponse_ThenAClearCompulsoryQuestionResponseIsCreatedUsingTheValuesInTheRequest()
        {
            var testItems = CreateTestItems();

            var testClearCompulsoryQuestionRequest = testItems.Fixture.Create<ClearCompulsoryQuestionRequest>();

            var result = testItems.QuestionConfigurationResponseFactory.CreateClearCompulsoryQuestionResponse(
                testClearCompulsoryQuestionRequest);

            Assert.Multiple(() =>
            {
                Assert.That(result.RequestingUserId, Is.EqualTo(testClearCompulsoryQuestionRequest.RequestingUserId));
                Assert.That(result.QuestionId, Is.EqualTo(testClearCompulsoryQuestionRequest.QuestionId));
            });
        }
        #endregion

        #region CreateGetCompulsorySupplierMandatedQuestionsResponse() Tests
        [Test]
        public void GivenANullGetCompulsorySupplierMandatedQuestionsRequest_WhenICreateGetCompulsorySupplierMandatedQuestionsResponse_ThenAnArgumentNullExceptionIsThrown()
        {
            var testItems = CreateTestItems();

            Assert.That(() => testItems.QuestionConfigurationResponseFactory.CreateGetCompulsorySupplierMandatedQuestionsResponse(
                    null!, testItems.Fixture.Create<CompulsorySupplierMandatedQuestionSet>()),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("getCompulsorySupplierMandatedQuestionsRequest"));
        }

        [Test]
        public void GivenANullCompulsorySupplierMandatedQuestionSet_WhenICreateGetCompulsorySupplierMandatedQuestionsResponse_ThenAnArgumentNullExceptionIsThrown()
        {
            var testItems = CreateTestItems();

            Assert.That(() => testItems.QuestionConfigurationResponseFactory.CreateGetCompulsorySupplierMandatedQuestionsResponse(
                    testItems.Fixture.Create<GetCompulsorySupplierMandatedQuestionsRequest>(), null!),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("compulsorySupplierMandatedQuestionSet"));
        }

        [Test]
        public void GivenAGetCompulsorySupplierMandatedQuestionsRequest_WhenICreateGetCompulsorySupplierMandatedQuestionsResponse_ThenAGetCompulsorySupplierMandatedQuestionsResponseIsCreatedUsingTheValuesInTheRequest()
        {
            var testItems = CreateTestItems();

            var testGetCompulsorySupplierMandatedQuestionsRequest = testItems.Fixture.Create<GetCompulsorySupplierMandatedQuestionsRequest>();

            var result = testItems.QuestionConfigurationResponseFactory.CreateGetCompulsorySupplierMandatedQuestionsResponse(
                testGetCompulsorySupplierMandatedQuestionsRequest, testItems.Fixture.Create<CompulsorySupplierMandatedQuestionSet>());

            Assert.Multiple(() =>
            {
                Assert.That(result.RequestingUserId, Is.EqualTo(testGetCompulsorySupplierMandatedQuestionsRequest.RequestingUserId));
                Assert.That(result.SupplierOrganisationId, Is.EqualTo(testGetCompulsorySupplierMandatedQuestionsRequest.SupplierOrganisationId));
            });
        }

        [Test]
        public void GivenACompulsorySupplierMandatedQuestionSet_WhenICreateGetCompulsorySupplierMandatedQuestionsResponse_ThenAGetCompulsorySupplierMandatedQuestionsResponseIsCreatedUsingTheCompulsoryMandatedQuestionSet()
        {
            var testItems = CreateTestItems();

            var testCompulsorySupplierMandatedQuestionSet = testItems.Fixture.Create<CompulsorySupplierMandatedQuestionSet>();

            var result = testItems.QuestionConfigurationResponseFactory.CreateGetCompulsorySupplierMandatedQuestionsResponse(
                testItems.Fixture.Create<GetCompulsorySupplierMandatedQuestionsRequest>(), testCompulsorySupplierMandatedQuestionSet);

            Assert.That(result.CompulsorySupplierMandatedQuestionSet, Is.EqualTo(testCompulsorySupplierMandatedQuestionSet));
        }
        #endregion

        #region CreateSetCompulsorySupplierMandatedQuestionResponse() Tests
        [Test]
        public void GivenANullSetCompulsorySupplierMandatedQuestionResponse_WhenICreateSetCompulsorySupplierMandatedQuestionResponse_ThenAnArgumentNullExceptionIsThrown()
        {
            var testItems = CreateTestItems();

            Assert.That(() => testItems.QuestionConfigurationResponseFactory.CreateSetCompulsorySupplierMandatedQuestionResponse(
                    null!),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("setCompulsorySupplierMandatedQuestionRequest"));
        }

        [Test]
        public void GivenASetCompulsorySupplierMandatedQuestionResponse_WhenICreateSetCompulsorySupplierMandatedQuestionResponse_ThenASetCompulsorySupplierMandatedQuestionResponseIsCreatedUsingTheValuesInTheRequest()
        {
            var testItems = CreateTestItems();

            var testSetCompulsorySupplierMandatedQuestionRequest = testItems.Fixture.Create<SetCompulsorySupplierMandatedQuestionRequest>();

            var result = testItems.QuestionConfigurationResponseFactory.CreateSetCompulsorySupplierMandatedQuestionResponse(
                testSetCompulsorySupplierMandatedQuestionRequest);

            Assert.Multiple(() =>
            {
                Assert.That(result.RequestingUserId, Is.EqualTo(testSetCompulsorySupplierMandatedQuestionRequest.RequestingUserId));
                Assert.That(result.SupplierOrganisationId, Is.EqualTo(testSetCompulsorySupplierMandatedQuestionRequest.SupplierOrganisationId));
                Assert.That(result.QuestionId, Is.EqualTo(testSetCompulsorySupplierMandatedQuestionRequest.QuestionId));
            });
        }
        #endregion

        #region CreateClearCompulsorySupplierMandatedQuestionResponse() Tests
        [Test]
        public void GivenANullClearCompulsorySupplierMandatedQuestionRequest_WhenICreateClearCompulsorySupplierMandatedQuestionResponse_ThenAnArgumentNullExceptionIsThrown()
        {
            var testItems = CreateTestItems();

            Assert.That(() => testItems.QuestionConfigurationResponseFactory.CreateClearCompulsorySupplierMandatedQuestionResponse(
                    null!),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("clearCompulsorySupplierMandatedQuestionRequest"));
        }

        [Test]
        public void GivenAClearCompulsorySupplierMandatedQuestionRequest_WhenICreateClearCompulsorySupplierMandatedQuestionResponse_ThenAClearCompulsorySupplierMandatedQuestionResponseIsCreatedUsingTheValuesInTheRequest()
        {
            var testItems = CreateTestItems();

            var testClearCompulsorySupplierMandatedQuestionRequest = testItems.Fixture.Create<ClearCompulsorySupplierMandatedQuestionRequest>();

            var result = testItems.QuestionConfigurationResponseFactory.CreateClearCompulsorySupplierMandatedQuestionResponse(
                testClearCompulsorySupplierMandatedQuestionRequest);

            Assert.Multiple(() =>
            {
                Assert.That(result.RequestingUserId, Is.EqualTo(testClearCompulsorySupplierMandatedQuestionRequest.RequestingUserId));
                Assert.That(result.SupplierOrganisationId, Is.EqualTo(testClearCompulsorySupplierMandatedQuestionRequest.SupplierOrganisationId));
                Assert.That(result.QuestionId, Is.EqualTo(testClearCompulsorySupplierMandatedQuestionRequest.QuestionId));
            });
        }
        #endregion

        #region Test Item Creation
        private static TestItems CreateTestItems()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());

            var questionConfigurationResponseFactory = new QuestionConfigurationResponseFactory();

            return new TestItems(fixture, questionConfigurationResponseFactory);
        }

        private class TestItems(
            IFixture fixture,
            IQuestionConfigurationResponseFactory questionConfigurationResponseFactory)
        {
            public IFixture Fixture { get; } = fixture;
            public IQuestionConfigurationResponseFactory QuestionConfigurationResponseFactory { get; } = questionConfigurationResponseFactory;
        }
        #endregion
    }
}
