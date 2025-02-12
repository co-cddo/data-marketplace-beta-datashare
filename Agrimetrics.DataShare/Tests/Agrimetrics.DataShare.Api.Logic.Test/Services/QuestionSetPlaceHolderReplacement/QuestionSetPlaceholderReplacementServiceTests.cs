using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests;
using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests.Answers.DataShareRequestAnswerSummaries;
using Agrimetrics.DataShare.Api.Logic.ModelData.Questions;
using Agrimetrics.DataShare.Api.Logic.ModelData.Questions.QuestionParts;
using Agrimetrics.DataShare.Api.Logic.ModelData.Submissions;
using Agrimetrics.DataShare.Api.Logic.Services.QuestionSetPlaceHolderReplacement;
using AutoFixture;
using AutoFixture.AutoMoq;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.Services.QuestionSetPlaceHolderReplacement
{
    [TestFixture]
    public class QuestionSetPlaceholderReplacementServiceTests
    {
        #region ReplacePlaceholderDataInQuestionModelData() Tests
        [Test]
        public void GivenANullDataShareRequestQuestionModelData_WhenIReplacePlaceholderDataInQuestionModelData_ThenAnArgumentNullExceptionIsThrown()
        {
            var testItems = CreateTestItems();

            Assert.That(() => testItems.QuestionSetPlaceholderReplacementService.ReplacePlaceholderDataInQuestionModelData(
                    null!,
                    testItems.Fixture.Create<string>()),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("questionsSummaryModelData"));
        }

        [Test]
        public void GivenANullEsdaName_WhenIReplacePlaceholderDataInQuestionModelData_ThenAnArgumentNullExceptionIsThrown()
        {
            var testItems = CreateTestItems();

            Assert.That(() => testItems.QuestionSetPlaceholderReplacementService.ReplacePlaceholderDataInQuestionModelData(
                    testItems.Fixture.Create<DataShareRequestQuestionModelData>(),
                    null!),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("esdaName"));
        }

        [Test]
        public void GivenAnEmptyEsdaName_WhenIReplacePlaceholderDataInQuestionModelData_ThenAnArgumentExceptionIsThrown(
            [Values("", "  ")] string esdaName)
        {
            var testItems = CreateTestItems();

            Assert.That(() => testItems.QuestionSetPlaceholderReplacementService.ReplacePlaceholderDataInQuestionModelData(
                    testItems.Fixture.Create<DataShareRequestQuestionModelData>(),
                    esdaName),
                Throws.ArgumentException.With.Property("ParamName").EqualTo("esdaName"));
        }

        [Test]
        public void GivenADataShareRequestQuestionModelData_WhenIReplacePlaceholderDataInQuestionModelData_ThenTheResourceNamePlaceholderIsReplacedInEachQuestionPrompt()
        {
            var testItems = CreateTestItems();

            var testDataShareRequestId = Guid.Parse("9BDD1943-75B9-4D3B-A5BE-C1E49081D076");

            var testQuestionParts = new List<DataShareRequestQuestionPartModelData>
            {
                CreateTestQuestionPartWithQuestionText("question header with no placeholder in it 1"),
                CreateTestQuestionPartWithQuestionText("placeholder [[<<EsdaName>>]] in the middle"),
                CreateTestQuestionPartWithQuestionText("question header with no placeholder in it 2"),
                CreateTestQuestionPartWithQuestionText("[[<<EsdaName>>]] placeholder at the start"),
                CreateTestQuestionPartWithQuestionText(null),
                CreateTestQuestionPartWithQuestionText("placeholder at the end [[<<EsdaName>>]]"),
            };

            var testQuestionSummaryModelData = testItems.Fixture.Build<DataShareRequestQuestionModelData>()
                .With(x => x.DataShareRequestQuestion_DataShareRequestId, testDataShareRequestId)
                .With(x => x.DataShareRequestQuestion_QuestionParts, testQuestionParts)
                .Create();

            testItems.QuestionSetPlaceholderReplacementService.ReplacePlaceholderDataInQuestionModelData(testQuestionSummaryModelData, "TEST ESDA NAME");

            var allQuestionTextHeaders = testQuestionSummaryModelData.DataShareRequestQuestion_QuestionParts.Select(x =>
                x.DataShareRequestQuestionPart_Question.QuestionPart_Prompts.QuestionPartPrompt_QuestionText).ToList();

            Assert.Multiple(() =>
            {
                Assert.That(allQuestionTextHeaders, Has.Exactly(6).Items);

                Assert.That(allQuestionTextHeaders, Has.Member("question header with no placeholder in it 1"));
                Assert.That(allQuestionTextHeaders, Has.Member("placeholder TEST ESDA NAME in the middle"));
                Assert.That(allQuestionTextHeaders, Has.Member("question header with no placeholder in it 2"));
                Assert.That(allQuestionTextHeaders, Has.Member("TEST ESDA NAME placeholder at the start"));
                Assert.That(allQuestionTextHeaders, Has.Member(null));
                Assert.That(allQuestionTextHeaders, Has.Member("placeholder at the end TEST ESDA NAME"));
            });

            DataShareRequestQuestionPartModelData CreateTestQuestionPartWithQuestionText(
                string? questionText)
            {
                var testPrompts = testItems.Fixture.Build<QuestionPartPromptsModelData>()
                    .With(x => x.QuestionPartPrompt_QuestionText, questionText)
                    .Create();

                var testQuestion = testItems.Fixture.Build<QuestionPartModelData>()
                    .With(x => x.QuestionPart_Prompts, testPrompts)
                    .Create();

                return testItems.Fixture.Build<DataShareRequestQuestionPartModelData>()
                    .With(x => x.DataShareRequestQuestionPart_Question, testQuestion)
                    .Create();
            }
        }
        #endregion

        #region ReplacePlaceholderDataInQuestionSetOutlineModelData() Tests
        [Test]
        public void GivenANullDataShareRequestQuestionModelData_WhenIReplacePlaceholderDataInQuestionSetOutlineModelData_ThenAnArgumentNullExceptionIsThrown()
        {
            var testItems = CreateTestItems();

            Assert.That(() => testItems.QuestionSetPlaceholderReplacementService.ReplacePlaceholderDataInQuestionSetOutlineModelData(
                    null!,
                    testItems.Fixture.Create<string>()),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("questionSetOutlineModelData"));
        }

        [Test]
        public void GivenANullEsdaName_WhenIReplacePlaceholderDataInQuestionSetOutlineModelData_ThenAnArgumentNullExceptionIsThrown()
        {
            var testItems = CreateTestItems();

            Assert.That(() => testItems.QuestionSetPlaceholderReplacementService.ReplacePlaceholderDataInQuestionSetOutlineModelData(
                    testItems.Fixture.Create<QuestionSetOutlineModelData>(),
                    null!),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("esdaName"));
        }

        [Test]
        public void GivenAnEmptyEsdaName_WhenIReplacePlaceholderDataInQuestionSetOutlineModelData_ThenAnArgumentExceptionIsThrown(
            [Values("", "  ")] string esdaName)
        {
            var testItems = CreateTestItems();

            Assert.That(() => testItems.QuestionSetPlaceholderReplacementService.ReplacePlaceholderDataInQuestionSetOutlineModelData(
                    testItems.Fixture.Create<QuestionSetOutlineModelData>(),
                    esdaName),
                Throws.ArgumentException.With.Property("ParamName").EqualTo("esdaName"));
        }

        [Test]
        public void GivenADataShareRequestQuestionModelData_WhenIReplacePlaceholderDataInQuestionSetOutlineModelData_ThenTheResourceNamePlaceholderIsReplacedInEachQuestionPrompt()
        {
            var testItems = CreateTestItems();

            var testQuestionSetSection1QuestionOutlineModelDatas = new List<QuestionSetQuestionOutlineModelData>
            {
                CreateTestQuestionSetQuestionOutlineModelDataWithQuestionText("question header with no placeholder in it 1"),
                CreateTestQuestionSetQuestionOutlineModelDataWithQuestionText("placeholder [[<<EsdaName>>]] in the middle"),
            };
            var testQuestionSetSectionOutlineModelData1 = testItems.Fixture.Build<QuestionSetSectionOutlineModelData>()
                .With(x => x.Questions, testQuestionSetSection1QuestionOutlineModelDatas)
                .Create();

            var testQuestionSetSection2QuestionOutlineModelDatas = new List<QuestionSetQuestionOutlineModelData>
            {
                CreateTestQuestionSetQuestionOutlineModelDataWithQuestionText("question header with no placeholder in it 2"),
                CreateTestQuestionSetQuestionOutlineModelDataWithQuestionText("[[<<EsdaName>>]] placeholder at the start"),
            };
            var testQuestionSetSectionOutlineModelData2 = testItems.Fixture.Build<QuestionSetSectionOutlineModelData>()
                .With(x => x.Questions, testQuestionSetSection2QuestionOutlineModelDatas)
                .Create();

            var testQuestionSetSection3QuestionOutlineModelDatas = new List<QuestionSetQuestionOutlineModelData>
            {
                CreateTestQuestionSetQuestionOutlineModelDataWithQuestionText(""),
                CreateTestQuestionSetQuestionOutlineModelDataWithQuestionText("placeholder at the end [[<<EsdaName>>]]"),
            };
            var testQuestionSetSectionOutlineModelData3 = testItems.Fixture.Build<QuestionSetSectionOutlineModelData>()
                .With(x => x.Questions, testQuestionSetSection3QuestionOutlineModelDatas)
                .Create();

            var testQuestionSetSectionOutlineModelDatas = new List<QuestionSetSectionOutlineModelData>
            {
                testQuestionSetSectionOutlineModelData1, testQuestionSetSectionOutlineModelData2, testQuestionSetSectionOutlineModelData3
            };

            var testQuestionSummaryModelData = testItems.Fixture.Build<QuestionSetOutlineModelData>()
                .With(x => x.Sections, testQuestionSetSectionOutlineModelDatas)
                .Create();

            testItems.QuestionSetPlaceholderReplacementService.ReplacePlaceholderDataInQuestionSetOutlineModelData(testQuestionSummaryModelData, "TEST ESDA NAME");

            var allQuestionTexts = testQuestionSummaryModelData.Sections.SelectMany(section => 
                    section.Questions.Select(question => question.QuestionSetQuestionOutline_QuestionText))
                .ToList();

            Assert.Multiple(() =>
            {
                Assert.That(allQuestionTexts, Has.Exactly(6).Items);

                Assert.That(allQuestionTexts, Has.Member("question header with no placeholder in it 1"));
                Assert.That(allQuestionTexts, Has.Member("placeholder TEST ESDA NAME in the middle"));
                Assert.That(allQuestionTexts, Has.Member("question header with no placeholder in it 2"));
                Assert.That(allQuestionTexts, Has.Member("TEST ESDA NAME placeholder at the start"));
                Assert.That(allQuestionTexts, Has.Member(""));
                Assert.That(allQuestionTexts, Has.Member("placeholder at the end TEST ESDA NAME"));
            });

            QuestionSetQuestionOutlineModelData CreateTestQuestionSetQuestionOutlineModelDataWithQuestionText(
                string questionText)
            {
                return testItems.Fixture.Build<QuestionSetQuestionOutlineModelData>()
                    .With(x => x.QuestionSetQuestionOutline_QuestionText, questionText)
                    .Create();
            }
        }
        #endregion

        #region ReplacePlaceholderDataInSubmissionDetailsModelData() Tests
        [Test]
        public void GivenANullSubmissionDetailsModelData_WhenIReplacePlaceholderDataInSubmissionDetailsModelData_ThenAnArgumentNullExceptionIsThrown()
        {
            var testItems = CreateTestItems();

            Assert.That(() => testItems.QuestionSetPlaceholderReplacementService.ReplacePlaceholderDataInSubmissionDetailsModelData(
                    null!,
                    testItems.Fixture.Create<string>()),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("submissionDetailsModelData"));
        }

        [Test]
        public void GivenANullEsdaName_WhenIReplacePlaceholderDataInSubmissionDetailsModelData_ThenAnArgumentNullExceptionIsThrown()
        {
            var testItems = CreateTestItems();

            Assert.That(() => testItems.QuestionSetPlaceholderReplacementService.ReplacePlaceholderDataInSubmissionDetailsModelData(
                    testItems.Fixture.Create<SubmissionDetailsModelData>(),
                    null!),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("esdaName"));
        }

        [Test]
        public void GivenAnEmptyEsdaName_WhenIReplacePlaceholderDataInSubmissionDetailsModelData_ThenAnArgumentExceptionIsThrown(
            [Values("", "  ")] string esdaName)
        {
            var testItems = CreateTestItems();

            Assert.That(() => testItems.QuestionSetPlaceholderReplacementService.ReplacePlaceholderDataInSubmissionDetailsModelData(
                    testItems.Fixture.Create<SubmissionDetailsModelData>(),
                    esdaName),
                Throws.ArgumentException.With.Property("ParamName").EqualTo("esdaName"));
        }

        [Test]
        public void GivenASubmissionDetailsModelData_WhenIReplacePlaceholderDataInSubmissionDetailsModelData_ThenTheResourceNamePlaceholderIsReplacedInEachQuestionPartText()
        {
            var testItems = CreateTestItems();

            var testSubmissionDetailsBackingQuestionAnswerPartModelDatas = new List<SubmissionDetailsAnswerPartModelData>
            {
                CreateTestSubmissionDetailsAnswerPartModelDataWithQuestionPartText("backing question part text with no placeholder in it"),
                CreateTestSubmissionDetailsAnswerPartModelDataWithQuestionPartText("[[<<EsdaName>>]] placeholder at the start"),
                CreateTestSubmissionDetailsAnswerPartModelDataWithQuestionPartText("placeholder [[<<EsdaName>>]] in the middle"),
                CreateTestSubmissionDetailsAnswerPartModelDataWithQuestionPartText("placeholder at the end [[<<EsdaName>>]]")
            };

            var testSubmissionDetailsBackingQuestionModelData = testItems.Fixture
                .Build<SubmissionDetailsBackingQuestionModelData>()
                .With(x => x.SubmissionDetailsBackingQuestion_AnswerParts, testSubmissionDetailsBackingQuestionAnswerPartModelDatas)
                .Create();

            var testSubmissionDetailsMainQuestionAnswerPartModelDatas = new List<SubmissionDetailsAnswerPartModelData>
            {
                CreateTestSubmissionDetailsAnswerPartModelDataWithQuestionPartText("main question part text with no placeholder in it"),
                CreateTestSubmissionDetailsAnswerPartModelDataWithQuestionPartText("[[<<EsdaName>>]] placeholder at the start"),
                CreateTestSubmissionDetailsAnswerPartModelDataWithQuestionPartText("placeholder [[<<EsdaName>>]] in the middle"),
                CreateTestSubmissionDetailsAnswerPartModelDataWithQuestionPartText("placeholder at the end [[<<EsdaName>>]]")
            };

            var testSubmissionDetailsMainQuestionModelData = testItems.Fixture
                .Build<SubmissionDetailsMainQuestionModelData>()
                .With(x => x.SubmissionDetailsMainQuestion_AnswerParts, testSubmissionDetailsMainQuestionAnswerPartModelDatas)
                .With(x => x.SubmissionDetailsMainQuestion_BackingQuestions, [testSubmissionDetailsBackingQuestionModelData])
                .Create();

            var testSubmissionDetailsSectionModelData = testItems.Fixture
                .Build<SubmissionDetailsSectionModelData>()
                .With(x => x.SubmissionDetailsSection_Questions, [testSubmissionDetailsMainQuestionModelData])
                .Create();

            var testSubmissionDetailsModelData = testItems.Fixture
                .Build<SubmissionDetailsModelData>()
                .With(x => x.SubmissionDetails_Sections, [testSubmissionDetailsSectionModelData])
                .Create();

            testItems.QuestionSetPlaceholderReplacementService.ReplacePlaceholderDataInSubmissionDetailsModelData(testSubmissionDetailsModelData, "TEST ESDA NAME");

            Assert.Multiple(() =>
            {
                var mainQuestionAnswerPartQuestionTexts = testSubmissionDetailsModelData.SubmissionDetails_Sections
                    .SelectMany(section => section.SubmissionDetailsSection_Questions
                        .SelectMany(question => question.SubmissionDetailsMainQuestion_AnswerParts
                            .Select(answerPart => answerPart.SubmissionDetailsAnswerPart_QuestionPartText))).ToList();

                Assert.That(mainQuestionAnswerPartQuestionTexts, Has.Exactly(4).Items);

                Assert.That(mainQuestionAnswerPartQuestionTexts, Has.Member("main question part text with no placeholder in it"));
                Assert.That(mainQuestionAnswerPartQuestionTexts, Has.Member("TEST ESDA NAME placeholder at the start"));
                Assert.That(mainQuestionAnswerPartQuestionTexts, Has.Member("placeholder TEST ESDA NAME in the middle"));
                Assert.That(mainQuestionAnswerPartQuestionTexts, Has.Member("placeholder at the end TEST ESDA NAME"));

                var backingQuestionAnswerPartQuestionTexts = testSubmissionDetailsModelData.SubmissionDetails_Sections
                    .SelectMany(section => section.SubmissionDetailsSection_Questions
                        .SelectMany(question => question.SubmissionDetailsMainQuestion_BackingQuestions
                            .SelectMany(backingQuestion => backingQuestion.SubmissionDetailsBackingQuestion_AnswerParts)
                            .Select(answerPart => answerPart.SubmissionDetailsAnswerPart_QuestionPartText))).ToList();

                Assert.That(backingQuestionAnswerPartQuestionTexts, Has.Exactly(4).Items);

                Assert.That(backingQuestionAnswerPartQuestionTexts, Has.Member("backing question part text with no placeholder in it"));
                Assert.That(backingQuestionAnswerPartQuestionTexts, Has.Member("TEST ESDA NAME placeholder at the start"));
                Assert.That(backingQuestionAnswerPartQuestionTexts, Has.Member("placeholder TEST ESDA NAME in the middle"));
                Assert.That(backingQuestionAnswerPartQuestionTexts, Has.Member("placeholder at the end TEST ESDA NAME"));
            });

            SubmissionDetailsAnswerPartModelData CreateTestSubmissionDetailsAnswerPartModelDataWithQuestionPartText(
                string questionPartText)
            {
                return testItems.Fixture.Build<SubmissionDetailsAnswerPartModelData>()
                    .With(x => x.SubmissionDetailsAnswerPart_QuestionPartText, questionPartText)
                    .Create();
            }
        }
        #endregion

        #region ReplacePlaceholderDataInDataShareRequestAnswersSummaryModelData() Tests
        [Test]
        public void GivenANullDataShareRequestAnswersSummaryModelData_WhenIReplacePlaceholderDataInDataShareRequestAnswersSummaryModelData_ThenAnArgumentNullExceptionIsThrown()
        {
            var testItems = CreateTestItems();

            Assert.That(() => testItems.QuestionSetPlaceholderReplacementService.ReplacePlaceholderDataInDataShareRequestAnswersSummaryModelData(
                    null!,
                    testItems.Fixture.Create<string>()),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("dataShareRequestAnswersSummaryModelData"));
        }

        [Test]
        public void GivenANullEsdaName_WhenIReplacePlaceholderDataInDataShareRequestAnswersSummaryModelData_ThenAnArgumentNullExceptionIsThrown()
        {
            var testItems = CreateTestItems();

            Assert.That(() => testItems.QuestionSetPlaceholderReplacementService.ReplacePlaceholderDataInDataShareRequestAnswersSummaryModelData(
                    testItems.Fixture.Create<DataShareRequestAnswersSummaryModelData>(),
                    null!),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("esdaName"));
        }

        [Test]
        public void GivenAnEmptyEsdaName_WhenIReplacePlaceholderDataInDataShareRequestAnswersSummaryModelData_ThenAnArgumentExceptionIsThrown(
            [Values("", "  ")] string esdaName)
        {
            var testItems = CreateTestItems();

            Assert.That(() => testItems.QuestionSetPlaceholderReplacementService.ReplacePlaceholderDataInDataShareRequestAnswersSummaryModelData(
                    testItems.Fixture.Create<DataShareRequestAnswersSummaryModelData>(),
                    esdaName),
                Throws.ArgumentException.With.Property("ParamName").EqualTo("esdaName"));
        }

        [Test]
        public void GivenADataShareRequestAnswersSummaryModelData_WhenIReplacePlaceholderDataInDataShareRequestAnswersSummaryModelData_ThenTheResourceNamePlaceholderIsReplacedInEachQuestionPartText()
        {
            var testItems = CreateTestItems();

            var testMainQuestionPartModelData = new List<DataShareRequestAnswersSummaryQuestionPartModelData>
            {
                CreateTestDataShareRequestAnswersSummaryQuestionPartModelDataWithQuestionPartText("main question part text with no placeholder in it"),
                CreateTestDataShareRequestAnswersSummaryQuestionPartModelDataWithQuestionPartText("[[<<EsdaName>>]] placeholder at the start"),
                CreateTestDataShareRequestAnswersSummaryQuestionPartModelDataWithQuestionPartText("placeholder [[<<EsdaName>>]] in the middle"),
                CreateTestDataShareRequestAnswersSummaryQuestionPartModelDataWithQuestionPartText("placeholder at the end [[<<EsdaName>>]]")
            };

            var testMainQuestionAnswersSummaryQuestionModelData = testItems.Fixture
                .Build<DataShareRequestAnswersSummaryQuestionModelData>()
                .With(x => x.DataShareRequestAnswersSummaryQuestion_QuestionParts, testMainQuestionPartModelData)
                .Create();

            var testBackingQuestionPartModelData = new List<DataShareRequestAnswersSummaryQuestionPartModelData>
            {
                CreateTestDataShareRequestAnswersSummaryQuestionPartModelDataWithQuestionPartText("backing question part text with no placeholder in it"),
                CreateTestDataShareRequestAnswersSummaryQuestionPartModelDataWithQuestionPartText("[[<<EsdaName>>]] placeholder at the start"),
                CreateTestDataShareRequestAnswersSummaryQuestionPartModelDataWithQuestionPartText("placeholder [[<<EsdaName>>]] in the middle"),
                CreateTestDataShareRequestAnswersSummaryQuestionPartModelDataWithQuestionPartText("placeholder at the end [[<<EsdaName>>]]")
            };

            var testBackingQuestionAnswersSummaryQuestionModelData = testItems.Fixture
                .Build<DataShareRequestAnswersSummaryQuestionModelData>()
                .With(x => x.DataShareRequestAnswersSummaryQuestion_QuestionParts, testBackingQuestionPartModelData)
                .Create();

            var testDataShareRequestAnswersSummaryQuestionGroupModelData = testItems.Fixture
                .Build<DataShareRequestAnswersSummaryQuestionGroupModelData>()
                .With(x => x.DataShareRequestAnswersSummaryQuestionGroup_SummaryMainQuestion, testMainQuestionAnswersSummaryQuestionModelData)
                .With(x => x.DataShareRequestAnswersSummaryQuestionGroup_SummaryBackingQuestions, [testBackingQuestionAnswersSummaryQuestionModelData])
                .Create();

            var testDataShareRequestAnswersSummarySectionModelData = testItems.Fixture
                .Build<DataShareRequestAnswersSummarySectionModelData>()
                .With(x => x.DataShareRequestAnswersSummarySection_QuestionGroups, [testDataShareRequestAnswersSummaryQuestionGroupModelData])
                .Create();

            var testDataShareRequestAnswersSummaryModelData = testItems.Fixture
                .Build<DataShareRequestAnswersSummaryModelData>()
                .With(x => x.DataShareRequestAnswersSummary_SummarySections, [testDataShareRequestAnswersSummarySectionModelData])
                .Create();

            testItems.QuestionSetPlaceholderReplacementService.ReplacePlaceholderDataInDataShareRequestAnswersSummaryModelData(testDataShareRequestAnswersSummaryModelData, "TEST ESDA NAME");

            Assert.Multiple(() =>
            {
                var mainQuestionAnswerPartQuestionTexts = testDataShareRequestAnswersSummaryModelData.DataShareRequestAnswersSummary_SummarySections
                    .SelectMany(section => section.DataShareRequestAnswersSummarySection_QuestionGroups
                        .Select(group => group.DataShareRequestAnswersSummaryQuestionGroup_SummaryMainQuestion)
                            .SelectMany(question => question.DataShareRequestAnswersSummaryQuestion_QuestionParts
                                .Select(answerPart => answerPart.DataShareRequestAnswersSummaryQuestionPart_QuestionPartText))).ToList();

                Assert.That(mainQuestionAnswerPartQuestionTexts, Has.Exactly(4).Items);

                Assert.That(mainQuestionAnswerPartQuestionTexts, Has.Member("main question part text with no placeholder in it"));
                Assert.That(mainQuestionAnswerPartQuestionTexts, Has.Member("TEST ESDA NAME placeholder at the start"));
                Assert.That(mainQuestionAnswerPartQuestionTexts, Has.Member("placeholder TEST ESDA NAME in the middle"));
                Assert.That(mainQuestionAnswerPartQuestionTexts, Has.Member("placeholder at the end TEST ESDA NAME"));

                var backingQuestionAnswerPartQuestionTexts = testDataShareRequestAnswersSummaryModelData.DataShareRequestAnswersSummary_SummarySections
                    .SelectMany(section => section.DataShareRequestAnswersSummarySection_QuestionGroups
                        .SelectMany(group => group.DataShareRequestAnswersSummaryQuestionGroup_SummaryBackingQuestions)
                            .SelectMany(question => question.DataShareRequestAnswersSummaryQuestion_QuestionParts
                                .Select(answerPart => answerPart.DataShareRequestAnswersSummaryQuestionPart_QuestionPartText))).ToList();

                Assert.That(backingQuestionAnswerPartQuestionTexts, Has.Exactly(4).Items);

                Assert.That(backingQuestionAnswerPartQuestionTexts, Has.Member("backing question part text with no placeholder in it"));
                Assert.That(backingQuestionAnswerPartQuestionTexts, Has.Member("TEST ESDA NAME placeholder at the start"));
                Assert.That(backingQuestionAnswerPartQuestionTexts, Has.Member("placeholder TEST ESDA NAME in the middle"));
                Assert.That(backingQuestionAnswerPartQuestionTexts, Has.Member("placeholder at the end TEST ESDA NAME"));
            });

            DataShareRequestAnswersSummaryQuestionPartModelData CreateTestDataShareRequestAnswersSummaryQuestionPartModelDataWithQuestionPartText(
                string questionPartText)
            {
                return testItems.Fixture.Build<DataShareRequestAnswersSummaryQuestionPartModelData>()
                    .With(x => x.DataShareRequestAnswersSummaryQuestionPart_QuestionPartText, questionPartText)
                    .Create();
            }
        }
        #endregion

        #region ReplacePlaceholderDataInSubmissionReviewInformationModelData() Tests
        [Test]
        public void GivenANullSubmissionReviewInformationModelData_WhenIReplacePlaceholderDataInSubmissionReviewInformationModelData_ThenAnArgumentNullExceptionIsThrown()
        {
            var testItems = CreateTestItems();

            Assert.That(() => testItems.QuestionSetPlaceholderReplacementService.ReplacePlaceholderDataInSubmissionReviewInformationModelData(
                    null!,
                    testItems.Fixture.Create<string>()),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("submissionReviewInformation"));
        }

        [Test]
        public void GivenANullEsdaName_WhenIReplacePlaceholderDataInSubmissionReviewInformationModelData_ThenAnArgumentNullExceptionIsThrown()
        {
            var testItems = CreateTestItems();

            Assert.That(() => testItems.QuestionSetPlaceholderReplacementService.ReplacePlaceholderDataInSubmissionReviewInformationModelData(
                    testItems.Fixture.Create<SubmissionReviewInformationModelData>(),
                    null!),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("esdaName"));
        }

        [Test]
        public void GivenAnEmptyEsdaName_WhenIReplacePlaceholderDataInSubmissionReviewInformationModelData_ThenAnArgumentExceptionIsThrown(
            [Values("", "  ")] string esdaName)
        {
            var testItems = CreateTestItems();

            Assert.That(() => testItems.QuestionSetPlaceholderReplacementService.ReplacePlaceholderDataInSubmissionReviewInformationModelData(
                    testItems.Fixture.Create<SubmissionReviewInformationModelData>(),
                    esdaName),
                Throws.ArgumentException.With.Property("ParamName").EqualTo("esdaName"));
        }

        [Test]
        public void GivenASubmissionReviewInformationModelData_WhenIReplacePlaceholderDataInSubmissionReviewInformationModelData_ThenTheResourceNamePlaceholderIsReplacedInEachQuestionPartText()
        {
            var testItems = CreateTestItems();

            var testSubmissionDetailsBackingQuestionAnswerPartModelDatas = new List<SubmissionDetailsAnswerPartModelData>
            {
                CreateTestSubmissionDetailsAnswerPartModelDataWithQuestionPartText("backing question part text with no placeholder in it"),
                CreateTestSubmissionDetailsAnswerPartModelDataWithQuestionPartText("[[<<EsdaName>>]] placeholder at the start"),
                CreateTestSubmissionDetailsAnswerPartModelDataWithQuestionPartText("placeholder [[<<EsdaName>>]] in the middle"),
                CreateTestSubmissionDetailsAnswerPartModelDataWithQuestionPartText("placeholder at the end [[<<EsdaName>>]]")
            };

            var testSubmissionDetailsBackingQuestionModelData = testItems.Fixture
                .Build<SubmissionDetailsBackingQuestionModelData>()
                .With(x => x.SubmissionDetailsBackingQuestion_AnswerParts, testSubmissionDetailsBackingQuestionAnswerPartModelDatas)
                .Create();

            var testSubmissionDetailsMainQuestionAnswerPartModelDatas = new List<SubmissionDetailsAnswerPartModelData>
            {
                CreateTestSubmissionDetailsAnswerPartModelDataWithQuestionPartText("main question part text with no placeholder in it"),
                CreateTestSubmissionDetailsAnswerPartModelDataWithQuestionPartText("[[<<EsdaName>>]] placeholder at the start"),
                CreateTestSubmissionDetailsAnswerPartModelDataWithQuestionPartText("placeholder [[<<EsdaName>>]] in the middle"),
                CreateTestSubmissionDetailsAnswerPartModelDataWithQuestionPartText("placeholder at the end [[<<EsdaName>>]]")
            };

            var testSubmissionDetailsMainQuestionModelData = testItems.Fixture
                .Build<SubmissionDetailsMainQuestionModelData>()
                .With(x => x.SubmissionDetailsMainQuestion_AnswerParts, testSubmissionDetailsMainQuestionAnswerPartModelDatas)
                .With(x => x.SubmissionDetailsMainQuestion_BackingQuestions, [testSubmissionDetailsBackingQuestionModelData])
                .Create();


            var testSubmissionDetailsSectionModelData = testItems.Fixture
                .Build<SubmissionDetailsSectionModelData>()
                .With(x => x.SubmissionDetailsSection_Questions, [testSubmissionDetailsMainQuestionModelData])
                .Create();

            var testSubmissionDetailsModelData = testItems.Fixture
                .Build<SubmissionDetailsModelData>()
                .With(x => x.SubmissionDetails_Sections, [testSubmissionDetailsSectionModelData])
                .Create();

            var testSubmissionReviewInformationModelData = testItems.Fixture
                .Build<SubmissionReviewInformationModelData>()
                .With(x => x.SubmissionReviewInformation_SubmissionDetails, testSubmissionDetailsModelData)
                .Create();

            testItems.QuestionSetPlaceholderReplacementService.ReplacePlaceholderDataInSubmissionReviewInformationModelData(testSubmissionReviewInformationModelData, "TEST ESDA NAME");

            Assert.Multiple(() =>
            {
                var mainQuestionAnswerPartQuestionTexts = testSubmissionReviewInformationModelData.SubmissionReviewInformation_SubmissionDetails
                    .SubmissionDetails_Sections
                        .SelectMany(section => section.SubmissionDetailsSection_Questions
                            .SelectMany(question => question.SubmissionDetailsMainQuestion_AnswerParts
                                .Select(answerPart => answerPart.SubmissionDetailsAnswerPart_QuestionPartText))).ToList();

                Assert.That(mainQuestionAnswerPartQuestionTexts, Has.Exactly(4).Items);

                Assert.That(mainQuestionAnswerPartQuestionTexts, Has.Member("main question part text with no placeholder in it"));
                Assert.That(mainQuestionAnswerPartQuestionTexts, Has.Member("TEST ESDA NAME placeholder at the start"));
                Assert.That(mainQuestionAnswerPartQuestionTexts, Has.Member("placeholder TEST ESDA NAME in the middle"));
                Assert.That(mainQuestionAnswerPartQuestionTexts, Has.Member("placeholder at the end TEST ESDA NAME"));

                var backingQuestionAnswerPartQuestionTexts = testSubmissionReviewInformationModelData.SubmissionReviewInformation_SubmissionDetails
                    .SubmissionDetails_Sections
                        .SelectMany(section => section.SubmissionDetailsSection_Questions
                            .SelectMany(question => question.SubmissionDetailsMainQuestion_BackingQuestions
                                .SelectMany(backingQuestion => backingQuestion.SubmissionDetailsBackingQuestion_AnswerParts)
                                .Select(answerPart => answerPart.SubmissionDetailsAnswerPart_QuestionPartText))).ToList();

                Assert.That(backingQuestionAnswerPartQuestionTexts, Has.Exactly(4).Items);

                Assert.That(backingQuestionAnswerPartQuestionTexts, Has.Member("backing question part text with no placeholder in it"));
                Assert.That(backingQuestionAnswerPartQuestionTexts, Has.Member("TEST ESDA NAME placeholder at the start"));
                Assert.That(backingQuestionAnswerPartQuestionTexts, Has.Member("placeholder TEST ESDA NAME in the middle"));
                Assert.That(backingQuestionAnswerPartQuestionTexts, Has.Member("placeholder at the end TEST ESDA NAME"));
            });

            SubmissionDetailsAnswerPartModelData CreateTestSubmissionDetailsAnswerPartModelDataWithQuestionPartText(
                string questionPartText)
            {
                return testItems.Fixture.Build<SubmissionDetailsAnswerPartModelData>()
                    .With(x => x.SubmissionDetailsAnswerPart_QuestionPartText, questionPartText)
                    .Create();
            }
        }
        #endregion

        #region Test Item Creation
        private static TestItems CreateTestItems()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());

            var questionSetPlaceholderReplacementService = new QuestionSetPlaceholderReplacementService();

            return new TestItems(fixture,
                questionSetPlaceholderReplacementService);
        }

        private class TestItems(
            IFixture fixture,
            IQuestionSetPlaceholderReplacementService questionSetPlaceholderReplacementService)
        {
            public IFixture Fixture { get; } = fixture;
            public IQuestionSetPlaceholderReplacementService QuestionSetPlaceholderReplacementService { get; } = questionSetPlaceholderReplacementService;
        }
        #endregion
    }
}
