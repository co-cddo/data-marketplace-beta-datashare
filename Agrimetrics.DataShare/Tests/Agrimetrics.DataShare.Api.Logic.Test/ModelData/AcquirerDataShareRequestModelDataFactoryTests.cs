using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests;
using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests.Answers.Answers;
using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests.Answers.DataShareRequestQuestionAnswers;
using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests.Questions;
using Agrimetrics.DataShare.Api.Dto.Models.Questions;
using Agrimetrics.DataShare.Api.Dto.Responses.Notifications;
using Agrimetrics.DataShare.Api.Logic.Exceptions;
using Agrimetrics.DataShare.Api.Logic.ModelData;
using Agrimetrics.DataShare.Api.Logic.ModelData.AuditLogs;
using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests;
using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests.Answers.DataShareRequestAnswerSummaries;
using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests.Answers.DataShareRequestQuestionAnswerResponses;
using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests.Answers.DataShareRequestQuestionAnswers;
using Agrimetrics.DataShare.Api.Logic.ModelData.Questions;
using Agrimetrics.DataShare.Api.Logic.ModelData.Questions.QuestionParts;
using Agrimetrics.DataShare.Api.Logic.ModelData.Questions.QuestionParts.OptionSelectionItems;
using Agrimetrics.DataShare.Api.Logic.ModelData.Questions.QuestionParts.ResponseFormats;
using Agrimetrics.DataShare.Api.Logic.ModelData.Questions.QuestionSets;
using Agrimetrics.DataShare.Api.Logic.Services.Users.Model;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Kernel;
using Moq;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.ModelData;

[TestFixture]
public class AcquirerDataShareRequestModelDataFactoryTests
{
    #region CreateQuestionSetOutline() Tests
    [Test]
    public void GivenQuestionSetOutlineModelData_WhenICreateQuestionSetOutline_ThenAQuestionSetOutlineIsCreatedFromTheSectionsInTheModelData()
    {
        var testItems = CreateTestItems();

        var testSections = testItems.Fixture.CreateMany<QuestionSetSectionOutlineModelData>().ToList();

        var testQuestionSetOutlineModelData = new QuestionSetOutlineModelData
        {
            Sections = testSections
        };

        var result = testItems.AcquirerDataShareRequestModelDataFactory.CreateQuestionSetOutline(testQuestionSetOutlineModelData);

        Assert.Multiple(() =>
        {
            Assert.That(result.Sections, Has.Exactly(testSections.Count).Items);

            foreach (var testSection in testSections)
            {
                var resultSection = result.Sections.FirstOrDefault(section =>
                    section.OrderWithinQuestionSetOutline == testSection.QuestionSetSectionOutline_OrderWithinQuestionSetOutline &&
                    section.Header == testSection.QuestionSetSectionOutline_SectionHeader);

                Assert.That(resultSection, Is.Not.Null);

                Assert.That(resultSection!.Questions, Has.Exactly(testSection.Questions.Count).Items);

                foreach (var testQuestion in testSection.Questions)
                {
                    var resultQuestion = resultSection.Questions.FirstOrDefault(question =>
                        question.OrderWithinQuestionSetSection == testQuestion.QuestionSetQuestionOutline_OrderWithinSection &&
                        question.QuestionText == testQuestion.QuestionSetQuestionOutline_QuestionText);

                    Assert.That(resultQuestion, Is.Not.Null);
                }
            }
        });
    }
    #endregion

    #region CreateDataShareRequestSummarySet() Tests
    [Test]
    public void GivenANullSetOfDataShareRequestModelData_WhenICreateDataShareRequestSummarySet_ThenAnArgumentNullExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        Assert.That(() => testItems.AcquirerDataShareRequestModelDataFactory.CreateDataShareRequestSummarySet(null!),
            Throws.ArgumentNullException.With.Property("ParamName").EqualTo("dataShareRequestModelDatas"));
    }

    [Test]
    public void GivenASetOfDataShareRequestModelData_WhenICreateDataShareRequestSummarySet_ThenADataShareRequestSummarySetIsCreatedUsingTheGivenModelData()
    {
        var testItems = CreateTestItems();

        var validInputDataShareRequestStatuses = ValidInputDataShareRequestStatuses.ToList();

        var testDataShareRequestModelData = testItems.Fixture
            .Build<DataShareRequestModelData>()
            .Without(x => x.DataShareRequest_RequestStatus)
            .CreateMany(validInputDataShareRequestStatuses.Count)
            .Select((item, index) =>
            {
                item.DataShareRequest_RequestStatus = validInputDataShareRequestStatuses[index];
                return item;
            })
            .ToList();

        var result = testItems.AcquirerDataShareRequestModelDataFactory.CreateDataShareRequestSummarySet(testDataShareRequestModelData);

        Assert.Multiple(() =>
        {
            Assert.That(result.DataShareRequestSummaries, Has.Exactly(testDataShareRequestModelData.Count).Items);

            foreach (var testDataShareRequest in  testDataShareRequestModelData)
            {
                var resultSummary = result.DataShareRequestSummaries.FirstOrDefault(summary =>
                    summary.Id == testDataShareRequest.DataShareRequest_Id &&
                    summary.RequestId == testDataShareRequest.DataShareRequest_RequestId &&
                    summary.EsdaName == testDataShareRequest.DataShareRequest_EsdaName);

                Assert.That(resultSummary, Is.Not.Null);

            }
        });
    }

    [Test]
    [TestCaseSource(nameof(InvalidInputDataShareRequestStatuses))]
    public void GivenADataShareRequestModelDataWithInvalidInputDataShareRequestStatus_WhenICreateDataShareRequestSummarySet_ThenAnInvalidEnumValueExceptionIsThrown(
        DataShareRequestStatusType tesDataShareRequestStatusType)
    {
        var testItems = CreateTestItems();

        var testDataShareRequestModelData = testItems.Fixture
            .Build<DataShareRequestModelData>()
            .With(x => x.DataShareRequest_RequestStatus, tesDataShareRequestStatusType)
            .CreateMany(1)
            .ToList();

        Assert.That(() => testItems.AcquirerDataShareRequestModelDataFactory.CreateDataShareRequestSummarySet(testDataShareRequestModelData),
            Throws.TypeOf<InvalidEnumValueException>().With.Message.EqualTo("Invalid DataShareRequestStatusType provided"));
    }
    #endregion

    #region CreateDataShareRequestAdminSummarySet() Tests
    [Test]
    public void GivenANullSetOfDataShareRequestAdminSummaries_WhenICreateDataShareRequestAdminSummarySet_ThenAnArgumentNullExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        Assert.That(() => testItems.AcquirerDataShareRequestModelDataFactory.CreateDataShareRequestAdminSummarySet(null!),
            Throws.ArgumentNullException.With.Property("ParamName").EqualTo("dataShareRequestAdminSummaries"));
    }

    [Test]
    public void GivenASetOfDataShareRequestAdminSummaries_WhenICreateDataShareRequestAdminSummarySet_ThenADataShareRequestAdminSummarySetIsCreatedContainingThoseSummaries()
    {
        var testItems = CreateTestItems();

        var testDataShareRequestAdminSummaries = testItems.Fixture.CreateMany<DataShareRequestAdminSummary>().ToList();

        var result = testItems.AcquirerDataShareRequestModelDataFactory.CreateDataShareRequestAdminSummarySet(testDataShareRequestAdminSummaries);

        Assert.That(result.DataShareRequestAdminSummaries, Is.EqualTo(testDataShareRequestAdminSummaries));
    }
    #endregion

    #region CreateDataShareRequestAdminSummary() Tests
    [Test]
    [TestCaseSource(nameof(ValidInputDataShareRequestStatuses))]
    public void GivenParameters_WhenICreateDataShareRequestAdminSummary_ThenADataShareRequestAdminSummaryIsCreatedFromThoseParameters(
        DataShareRequestStatusType testRequestStatus)
    {
        var testItems = CreateTestItems();

        var testDataShareRequestModelData = testItems.Fixture.Build<DataShareRequestModelData>()
            .With(x => x.DataShareRequest_RequestStatus, testRequestStatus)
            .Create();
        var testWhenCreated = testItems.Fixture.Create<DateTime>();
        var testWhenSubmitted = testItems.Fixture.Create<DateTime?>();
        var testCreatedByUserEmailAddress = testItems.Fixture.Create<string>();
        var testWhenNeededBy = testItems.Fixture.Create<DateTime?>();

        var result = testItems.AcquirerDataShareRequestModelDataFactory.CreateDataShareRequestAdminSummary(
            testDataShareRequestModelData,
            testWhenCreated,
            testWhenSubmitted,
            testCreatedByUserEmailAddress,
            testWhenNeededBy);

        Assert.Multiple(() =>
        {
            Assert.That(result.Id, Is.EqualTo(testDataShareRequestModelData.DataShareRequest_Id));
            Assert.That(result.RequestId, Is.EqualTo(testDataShareRequestModelData.DataShareRequest_RequestId));
            Assert.That(result.EsdaName, Is.EqualTo(testDataShareRequestModelData.DataShareRequest_EsdaName));
            Assert.That(result.WhenCreatedUtc, Is.EqualTo(testWhenCreated));
            Assert.That(result.WhenSubmittedUtc, Is.EqualTo(testWhenSubmitted));
            Assert.That(result.CreatedByUserEmailAddress, Is.EqualTo(testCreatedByUserEmailAddress));
            Assert.That(result.WhenNeededByUtc, Is.EqualTo(testWhenNeededBy));
            Assert.That(result.DataShareRequestStatus, Is.EqualTo(GetExpectedDataShareRequestStatus(testDataShareRequestModelData.DataShareRequest_RequestStatus)));
        });
    }
    #endregion

    #region CreateDataShareRequestQuestionsSummary() Tests
    [Test]
    public void GivenANullDataShareRequestQuestionsSummaryModelData_WhenICreateDataShareRequestQuestionsSummary_ThenAnArgumentNullExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        Assert.That(() => testItems.AcquirerDataShareRequestModelDataFactory.CreateDataShareRequestQuestionsSummary(null!),
            Throws.ArgumentNullException.With.Property("ParamName").EqualTo("dataShareRequestQuestionsSummaryModelData"));
    }

    [Test]
    public void GivenADataShareRequestQuestionsSummaryModelData_WhenICreateDataShareRequestQuestionsSummary_ThenADataShareRequestQuestionsSummaryIsCreatedUsingTheModelData(
        [ValueSource(nameof(ValidInputDataShareRequestStatuses))] DataShareRequestStatusType testDataShareRequestStatus,
        [ValueSource(nameof(ValidInputQuestionStatuses))] QuestionStatusType testQuestionStatus)
    {
        var testItems = CreateTestItems();

        var inputQuestionSummaries = testItems.Fixture
            .Build<QuestionSummaryModelData>()
            .With(x => x.Question_QuestionStatus, testQuestionStatus)
            .CreateMany()
            .ToList();

        var inputSectionSummaries = testItems.Fixture
            .Build<QuestionSetSectionSummaryModelData>()
            .With(x => x.QuestionSetSection_QuestionSummaries, inputQuestionSummaries)
            .CreateMany()
            .ToList();

        var inputQuestionSetSummaryModelData = testItems.Fixture
            .Build<QuestionSetSummaryModelData>()
            .With(x => x.QuestionSet_SectionSummaries, inputSectionSummaries)
            .Create();

        var testDataShareRequestQuestionsSummaryModelData = testItems.Fixture
            .Build<DataShareRequestQuestionsSummaryModelData>()
            .With(x => x.DataShareRequest_DataShareRequestStatus, testDataShareRequestStatus)
            .With(x => x.DataShareRequest_QuestionSetSummary, inputQuestionSetSummaryModelData)
            .Create();

        var result = testItems.AcquirerDataShareRequestModelDataFactory.CreateDataShareRequestQuestionsSummary(
            testDataShareRequestQuestionsSummaryModelData);

        Assert.Multiple(() =>
        {
            Assert.That(result.DataShareRequestId, Is.EqualTo(testDataShareRequestQuestionsSummaryModelData.DataShareRequest_Id));
            Assert.That(result.DataShareRequestRequestId, Is.EqualTo(testDataShareRequestQuestionsSummaryModelData.DataShareRequest_RequestId));
            Assert.That(result.EsdaName, Is.EqualTo(testDataShareRequestQuestionsSummaryModelData.DataShareRequest_EsdaName));

            var testDataShareRequestQuestionSetSummary = testDataShareRequestQuestionsSummaryModelData.DataShareRequest_QuestionSetSummary;
            Assert.That(result.QuestionSetSummary.Id, Is.EqualTo(testDataShareRequestQuestionSetSummary.QuestionSet_Id));
            Assert.That(result.QuestionSetSummary.AnswersSectionComplete, Is.EqualTo(testDataShareRequestQuestionSetSummary.QuestionSet_AnswersSectionComplete));
            Assert.That(result.QuestionSetSummary.DataShareRequestStatus, Is.EqualTo(GetExpectedDataShareRequestStatus(testDataShareRequestQuestionsSummaryModelData.DataShareRequest_DataShareRequestStatus)));
            Assert.That(result.QuestionSetSummary.QuestionsRemainThatRequireAResponse, Is.EqualTo(testDataShareRequestQuestionsSummaryModelData.DataShareRequest_QuestionsRemainThatRequireAResponse));
            Assert.That(result.QuestionSetSummary.SupplierOrganisationName, Is.EqualTo(testDataShareRequestQuestionsSummaryModelData.DataShareRequest_SupplierOrganisationName));
            Assert.That(result.QuestionSetSummary.SubmissionResponseFromSupplier, Is.EqualTo(testDataShareRequestQuestionsSummaryModelData.DataShareRequest_SubmissionResponseFromSupplier));
            Assert.That(result.QuestionSetSummary.CancellationReasonsFromAcquirer, Is.EqualTo(testDataShareRequestQuestionsSummaryModelData.DataShareRequest_CancellationReasonsFromAcquirer));

            Assert.That(result.QuestionSetSummary.AcquirerUserDetails.UserId, Is.EqualTo(testDataShareRequestQuestionsSummaryModelData.DataShareRequest_AcquirerUserId));
            Assert.That(result.QuestionSetSummary.AcquirerUserDetails.DomainId, Is.EqualTo(testDataShareRequestQuestionsSummaryModelData.DataShareRequest_AcquirerDomainId));
            Assert.That(result.QuestionSetSummary.AcquirerUserDetails.OrganisationId, Is.EqualTo(testDataShareRequestQuestionsSummaryModelData.DataShareRequest_AcquirerOrganisationId));

            var testSectionSummaries = testDataShareRequestQuestionSetSummary.QuestionSet_SectionSummaries;
            Assert.That(result.QuestionSetSummary.SectionSummaries, Has.Exactly(testSectionSummaries.Count).Items);

            foreach (var testSectionSummary in testSectionSummaries)
            {
                var resultSectionSummary = result.QuestionSetSummary.SectionSummaries.FirstOrDefault(sectionSummary =>
                    sectionSummary.Id == testSectionSummary.QuestionSetSection_Id);

                Assert.That(resultSectionSummary, Is.Not.Null);

                Assert.That(resultSectionSummary!.SectionNumber, Is.EqualTo(testSectionSummary.QuestionSetSection_Number));
                Assert.That(resultSectionSummary.SectionHeader, Is.EqualTo(testSectionSummary.QuestionSetSection_Header));
                Assert.That(resultSectionSummary.SectionIsComplete, Is.EqualTo(testSectionSummary.QuestionSetSection_IsComplete));

                var testQuestionSummaries = testSectionSummary.QuestionSetSection_QuestionSummaries;
                Assert.That(resultSectionSummary.QuestionSummaries, Has.Exactly(testQuestionSummaries.Count).Items);

                foreach (var testQuestionSummary in testSectionSummary.QuestionSetSection_QuestionSummaries)
                {
                    var resultQuestionSummary = resultSectionSummary.QuestionSummaries.FirstOrDefault(questionSummary =>
                        questionSummary.QuestionId == testQuestionSummary.Question_Id);

                    Assert.That(resultQuestionSummary, Is.Not.Null);

                    Assert.That(resultQuestionSummary!.QuestionOrderWithinQuestionSetSection, Is.EqualTo(testQuestionSummary.Question_OrderWithinQuestionSetSection));
                    Assert.That(resultQuestionSummary.QuestionHeader, Is.EqualTo(testQuestionSummary.Question_Header));
                    Assert.That(resultQuestionSummary.QuestionStatus, Is.EqualTo(GetExpectedQuestionStatus(testQuestionSummary.Question_QuestionStatus)));
                    Assert.That(resultQuestionSummary.QuestionCanBeAnswered, Is.EqualTo(testQuestionSummary.Question_QuestionCanBeAnswered));
                }
            }
        });
    }

    [Test]
    public void GivenADataShareRequestQuestionsSummaryModelDataWithAnInvalidQuestionStatus_WhenICreateDataShareRequestQuestionsSummary_ThenAnInconsistentDataExceptionIsThrown(
        [ValueSource(nameof(ValidInputDataShareRequestStatuses))] DataShareRequestStatusType testDataShareRequestStatus,
        [ValueSource(nameof(InvalidQuestionStatuses))] QuestionStatusType testQuestionStatus)
    {
        var testItems = CreateTestItems();

        var inputQuestionSummaries = testItems.Fixture
            .Build<QuestionSummaryModelData>()
            .With(x => x.Question_QuestionStatus, testQuestionStatus)
            .CreateMany()
            .ToList();

        var inputSectionSummaries = testItems.Fixture
            .Build<QuestionSetSectionSummaryModelData>()
            .With(x => x.QuestionSetSection_QuestionSummaries, inputQuestionSummaries)
            .CreateMany()
            .ToList();

        var inputQuestionSetSummaryModelData = testItems.Fixture
            .Build<QuestionSetSummaryModelData>()
            .With(x => x.QuestionSet_SectionSummaries, inputSectionSummaries)
            .Create();

        var testDataShareRequestQuestionsSummaryModelData = testItems.Fixture
            .Build<DataShareRequestQuestionsSummaryModelData>()
            .With(x => x.DataShareRequest_DataShareRequestStatus, testDataShareRequestStatus)
            .With(x => x.DataShareRequest_QuestionSetSummary, inputQuestionSetSummaryModelData)
            .Create();

        Assert.That(() => testItems.AcquirerDataShareRequestModelDataFactory.CreateDataShareRequestQuestionsSummary(testDataShareRequestQuestionsSummaryModelData),
                Throws.TypeOf<InconsistentDataException>().With.Message.EqualTo("QuestionStatusType has not been set"));
    }

    [Test]
    public void GivenADataShareRequestQuestionsSummaryModelDataWithAnUnknownQuestionStatus_WhenICreateDataShareRequestQuestionsSummary_ThenAnInvalidEnumValueExceptionIsThrown(
        [ValueSource(nameof(ValidInputDataShareRequestStatuses))] DataShareRequestStatusType testDataShareRequestStatus)
    {
        var testItems = CreateTestItems();

        var testQuestionStatus = (QuestionStatusType) Enum.GetValues<QuestionStatusType>().Cast<int>().Max() + 1;

        var inputQuestionSummaries = testItems.Fixture
            .Build<QuestionSummaryModelData>()
            .With(x => x.Question_QuestionStatus, testQuestionStatus)
            .CreateMany()
            .ToList();

        var inputSectionSummaries = testItems.Fixture
            .Build<QuestionSetSectionSummaryModelData>()
            .With(x => x.QuestionSetSection_QuestionSummaries, inputQuestionSummaries)
            .CreateMany()
            .ToList();

        var inputQuestionSetSummaryModelData = testItems.Fixture
            .Build<QuestionSetSummaryModelData>()
            .With(x => x.QuestionSet_SectionSummaries, inputSectionSummaries)
            .Create();

        var testDataShareRequestQuestionsSummaryModelData = testItems.Fixture
            .Build<DataShareRequestQuestionsSummaryModelData>()
            .With(x => x.DataShareRequest_DataShareRequestStatus, testDataShareRequestStatus)
            .With(x => x.DataShareRequest_QuestionSetSummary, inputQuestionSetSummaryModelData)
            .Create();

        Assert.That(() => testItems.AcquirerDataShareRequestModelDataFactory.CreateDataShareRequestQuestionsSummary(testDataShareRequestQuestionsSummaryModelData),
            Throws.TypeOf<InvalidEnumValueException>().With.Message.EqualTo("Invalid QuestionStatusType provided"));
    }
    #endregion

    #region CreateDataShareRequestQuestion() Tests
    [Test]
    public void GivenANullDataShareRequestQuestionModelData_WhenICreateDataShareRequestQuestion_ThenAnArgumentNullExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        Assert.That(() => testItems.AcquirerDataShareRequestModelDataFactory.CreateDataShareRequestQuestion(null!),
            Throws.ArgumentNullException.With.Property("ParamName").EqualTo("dataShareRequestQuestionModelData"));
    }

    [Test]
    [TestCaseSource(nameof(CreateDataShareRequestQuestionTestCaseData))]
    public void GivenADataShareRequestQuestionModelData_WhenICreateDataShareRequestQuestion_ThenADataShareRequestQuestionIsCreatedFromTheModelData(
        QuestionPartType testQuestionPartType,
        QuestionPartResponseFormatModelData testResponseFormat)
    {
        var testItems = CreateTestItems();

        var testInputResponses = testItems.Fixture
            .Build<QuestionPartAnswerResponseModelData>()
            .With(x => x.QuestionPartAnswerResponse_InputType, testResponseFormat.InputType)
            .CreateMany()
            .Select((item, index) =>
            {
                item.QuestionPartAnswerResponse_OrderWithinAnswerPart = index + 1;

                var testInputFreeFormResponseItem = testItems.Fixture.Create<QuestionPartAnswerResponseItemFreeFormModelData>();

                var testInputSelectionOptionItems = testItems.Fixture
                    .Build<QuestionPartAnswerItemSelectionOptionItemModelData>()
                    .With(x => x.QuestionPartAnswerItem_SupplementaryQuestionPartAnswer, (QuestionPartAnswerModelData?) null)
                    .CreateMany()
                    .ToList();

                var testInputOptionSelectionResponseItem = testItems.Fixture.Build<QuestionPartAnswerResponseItemOptionSelectionModelData>()
                    .With(x => x.QuestionPartAnswerItem_SelectedOptionItems, testInputSelectionOptionItems)
                    .Create();

                item.QuestionPartAnswerResponse_ResponseItem = testResponseFormat.InputType switch
                {
                    QuestionPartResponseInputType.FreeForm => testInputFreeFormResponseItem,

                    QuestionPartResponseInputType.OptionSelection =>
                        testInputOptionSelectionResponseItem,

                    QuestionPartResponseInputType.None => null,
                    _ => null
                };

                return item;
            })
            .ToList();

        var testQuestionParts = testItems.Fixture
            .Build<DataShareRequestQuestionPartModelData>()
            .CreateMany()
            .Select((item, index) =>
            {
                item.DataShareRequestQuestionPart_Question.QuestionPart_QuestionPartOrderWithinQuestion = index + 1;
                item.DataShareRequestQuestionPart_Question.QuestionPart_QuestionPartType = testQuestionPartType;
                item.DataShareRequestQuestionPart_Question.QuestionPart_ResponseFormat = testResponseFormat;

                item.DataShareRequestQuestionPart_Answer!.QuestionPartAnswer_QuestionPartId = item.DataShareRequestQuestionPart_Question.QuestionPart_Id;
                item.DataShareRequestQuestionPart_Answer.QuestionPartAnswer_AnswerPartResponses = testInputResponses;

                return item;
            })
            .ToList();

        var testInputQuestionFooterItems = testItems.Fixture
            .Build<DataShareRequestQuestionFooterItemModelData>()
            .CreateMany()
            .Select((item, index) =>
            {
                item.DataShareRequestQuestionFooterItem_OrderWithinFooter = index + 1;
                return item;
            })
            .ToList();

        var testInputQuestionFooter = testItems.Fixture
            .Build<DataShareRequestQuestionFooterModelData>()
            .With(x => x.DataShareRequestQuestionFooter_Items, testInputQuestionFooterItems)
            .Create();

        var testInputQuestion = testItems.Fixture
            .Build<DataShareRequestQuestionModelData>()
            .With(x => x.DataShareRequestQuestion_QuestionParts, testQuestionParts)
            .With(x => x.DataShareRequestQuestion_QuestionFooter, testInputQuestionFooter)
            .Create();

        var result = testItems.AcquirerDataShareRequestModelDataFactory.CreateDataShareRequestQuestion(testInputQuestion);

        Assert.Multiple(() =>
        {
            Assert.That(result.DataShareRequestId, Is.EqualTo(testInputQuestion.DataShareRequestQuestion_DataShareRequestId));
            Assert.That(result.DataShareRequestRequestId, Is.EqualTo(testInputQuestion.DataShareRequestQuestion_DataShareRequestRequestId));
            Assert.That(result.QuestionId, Is.EqualTo(testInputQuestion.DataShareRequestQuestion_QuestionId));
            Assert.That(result.IsOptional, Is.EqualTo(testInputQuestion.DataShareRequestQuestion_IsOptional));

            var testQuestionFooter = testInputQuestion.DataShareRequestQuestion_QuestionFooter;
            var resultQuestionFooter = result.QuestionFooter;
            Assert.That(resultQuestionFooter!.FooterHeader, Is.EqualTo(testQuestionFooter!.DataShareRequestQuestionFooter_Header));

            var testQuestionFooterItems = testQuestionFooter.DataShareRequestQuestionFooter_Items;
            var resultQuestionFooterItems = resultQuestionFooter.FooterItems;

            Assert.That(resultQuestionFooterItems, Has.Exactly(testQuestionFooterItems.Count).Items);
            foreach (var testQuestionFooterItem in testQuestionFooterItems)
            {
                var resultQuestionFooterItem = resultQuestionFooterItems.FirstOrDefault(x =>
                    x.OrderWithinFooter == testQuestionFooterItem.DataShareRequestQuestionFooterItem_OrderWithinFooter);

                Assert.That(resultQuestionFooterItem!.Text, Is.EqualTo(testQuestionFooterItem.DataShareRequestQuestionFooterItem_Text));
            }
        });
    }

    private static IEnumerable<TestCaseData> CreateDataShareRequestQuestionTestCaseData()
    {
        foreach (var questionPartType in Enum.GetValues<QuestionPartType>())
        {
            yield return new TestCaseData(questionPartType, new QuestionPartResponseFormatFreeFormCountryModelData());
            yield return new TestCaseData(questionPartType, new QuestionPartResponseFormatFreeFormDateModelData());
            yield return new TestCaseData(questionPartType, new QuestionPartResponseFormatFreeFormDateTimeModelData());
            yield return new TestCaseData(questionPartType, new QuestionPartResponseFormatFreeFormNumericModelData());
            yield return new TestCaseData(questionPartType, new QuestionPartResponseFormatFreeFormTextModelData());
            yield return new TestCaseData(questionPartType, new QuestionPartResponseFormatFreeFormTimeModelData());

            yield return new TestCaseData(questionPartType, new QuestionPartResponseFormatOptionSelectSingleValueModelData
                {
                    ResponseFormatOptionSelectSingleValue_SingleSelectionOptions =
                    [
                        new QuestionPartOptionSelectionItemForSingleSelectionModelData()
                    ]
                }
            );
            yield return new TestCaseData(questionPartType, new QuestionPartResponseFormatOptionSelectMultiValueModelData
            {
                ResponseFormatOptionSelectMultiValue_MultiSelectionOptions =
                [
                    new QuestionPartOptionSelectionItemForMultiSelectionModelData()
                ]
            });

            yield return new TestCaseData(questionPartType, new QuestionPartResponseFormatReadOnlyModelData());
        }
    }

    [Test]
    [TestCaseSource(nameof(CreateDataShareRequestQuestionTestCaseData))]
    public void GivenADataShareRequestQuestionModelDataWithResponsesOfIncorrectFormat_WhenICreateDataShareRequestQuestion_ThenAnInconsistentDataExceptionIsThrown(
        QuestionPartType testQuestionPartType,
        QuestionPartResponseFormatModelData testResponseFormat)
    {
        var testItems = CreateTestItems();

        var testInputResponses = testItems.Fixture
            .Build<QuestionPartAnswerResponseModelData>()
            .With(x => x.QuestionPartAnswerResponse_InputType, testResponseFormat.InputType)
            .CreateMany()
            .Select((item, index) =>
            {
                item.QuestionPartAnswerResponse_OrderWithinAnswerPart = index + 1;

                item.QuestionPartAnswerResponse_ResponseItem = testResponseFormat.InputType switch
                {
                    QuestionPartResponseInputType.FreeForm => testItems.Fixture.Create<QuestionPartAnswerResponseItemFreeFormModelData>(),

                    QuestionPartResponseInputType.OptionSelection =>
                        testItems.Fixture.Build<QuestionPartAnswerResponseItemOptionSelectionModelData>()
                            .With(x => x.QuestionPartAnswerItem_SelectedOptionItems, [])
                            .Create(),

                    QuestionPartResponseInputType.None => null,
                    _ => null
                };

                return item;
            })
            .ToList();

        var testQuestionParts = testItems.Fixture
            .Build<DataShareRequestQuestionPartModelData>()
            .CreateMany()
            .Select((item, index) =>
            {
                item.DataShareRequestQuestionPart_Question.QuestionPart_QuestionPartOrderWithinQuestion = index + 1;
                item.DataShareRequestQuestionPart_Question.QuestionPart_QuestionPartType = testQuestionPartType;

                // Here is where we replace the proper response with one of an unexpected type
                item.DataShareRequestQuestionPart_Question.QuestionPart_ResponseFormat = new TestStubQuestionPartResponseFormatModelData
                {
                    InputType = testResponseFormat.InputType,
                    FormatType = testResponseFormat.FormatType
                };

                item.DataShareRequestQuestionPart_Answer!.QuestionPartAnswer_QuestionPartId = item.DataShareRequestQuestionPart_Question.QuestionPart_Id;
                item.DataShareRequestQuestionPart_Answer.QuestionPartAnswer_AnswerPartResponses = testInputResponses;

                return item;
            })
            .ToList();

        var testInputQuestion = testItems.Fixture
            .Build<DataShareRequestQuestionModelData>()
            .With(x => x.DataShareRequestQuestion_QuestionParts, testQuestionParts)
            .Without(x => x.DataShareRequestQuestion_QuestionFooter)
            .Create();

        Assert.That(() => testItems.AcquirerDataShareRequestModelDataFactory.CreateDataShareRequestQuestion(testInputQuestion),
            Throws.TypeOf<InconsistentDataException>());
    }

    [Test]
    [TestCaseSource(nameof(CreateDataShareRequestQuestionTestCaseData))]
    public void GivenADataShareRequestQuestionModelDataWithAnUnknownInputType_WhenICreateDataShareRequestQuestion_ThenAnInvalidEnumValueExceptionIsThrown(
        QuestionPartType testQuestionPartType,
        QuestionPartResponseFormatModelData testResponseFormat)
    {
        var testItems = CreateTestItems();

        var testInputResponses = testItems.Fixture
            .Build<QuestionPartAnswerResponseModelData>()
            .With(x => x.QuestionPartAnswerResponse_InputType, testResponseFormat.InputType)
            .CreateMany()
            .Select((item, index) =>
            {
                item.QuestionPartAnswerResponse_OrderWithinAnswerPart = index + 1;

                item.QuestionPartAnswerResponse_ResponseItem = testResponseFormat.InputType switch
                {
                    QuestionPartResponseInputType.FreeForm => testItems.Fixture.Create<QuestionPartAnswerResponseItemFreeFormModelData>(),

                    QuestionPartResponseInputType.OptionSelection =>
                        testItems.Fixture.Build<QuestionPartAnswerResponseItemOptionSelectionModelData>()
                            .With(x => x.QuestionPartAnswerItem_SelectedOptionItems, [])
                            .Create(),

                    QuestionPartResponseInputType.None => null,
                    _ => null
                };

                return item;
            })
            .ToList();

        var testQuestionParts = testItems.Fixture
            .Build<DataShareRequestQuestionPartModelData>()
            .CreateMany()
            .Select((item, index) =>
            {
                item.DataShareRequestQuestionPart_Question.QuestionPart_QuestionPartOrderWithinQuestion = index + 1;
                item.DataShareRequestQuestionPart_Question.QuestionPart_QuestionPartType = testQuestionPartType;

                // Here is where we replace the proper response with one of an unexpected type
                item.DataShareRequestQuestionPart_Question.QuestionPart_ResponseFormat = new TestStubQuestionPartResponseFormatModelData
                {
                    InputType = Enum.GetValues<QuestionPartResponseInputType>().Max() + 1,
                    FormatType = testResponseFormat.FormatType
                };

                item.DataShareRequestQuestionPart_Answer!.QuestionPartAnswer_QuestionPartId = item.DataShareRequestQuestionPart_Question.QuestionPart_Id;
                item.DataShareRequestQuestionPart_Answer.QuestionPartAnswer_AnswerPartResponses = testInputResponses;

                return item;
            })
            .ToList();

        var testInputQuestion = testItems.Fixture
            .Build<DataShareRequestQuestionModelData>()
            .With(x => x.DataShareRequestQuestion_QuestionParts, testQuestionParts)
            .Without(x => x.DataShareRequestQuestion_QuestionFooter)
            .Create();

        Assert.That(() => testItems.AcquirerDataShareRequestModelDataFactory.CreateDataShareRequestQuestion(testInputQuestion),
            Throws.TypeOf<InvalidEnumValueException>());
    }


    [Test]
    [TestCaseSource(nameof(CreateDataShareRequestQuestionTestCaseData))]
    public void GivenADataShareRequestQuestionModelDataWithAnUnknownFreeFormFormatType_WhenICreateDataShareRequestQuestion_ThenAnInvalidEnumValueExceptionIsThrown(
        QuestionPartType testQuestionPartType,
        QuestionPartResponseFormatModelData testResponseFormat)
    {
        var testItems = CreateTestItems();

        var testInputResponses = testItems.Fixture
            .Build<QuestionPartAnswerResponseModelData>()
            .With(x => x.QuestionPartAnswerResponse_InputType, testResponseFormat.InputType)
            .CreateMany()
            .Select((item, index) =>
            {
                item.QuestionPartAnswerResponse_OrderWithinAnswerPart = index + 1;

                item.QuestionPartAnswerResponse_ResponseItem = testResponseFormat.InputType switch
                {
                    QuestionPartResponseInputType.FreeForm => testItems.Fixture.Create<QuestionPartAnswerResponseItemFreeFormModelData>(),

                    QuestionPartResponseInputType.OptionSelection =>
                        testItems.Fixture.Build<QuestionPartAnswerResponseItemOptionSelectionModelData>()
                            .With(x => x.QuestionPartAnswerItem_SelectedOptionItems, [])
                            .Create(),

                    QuestionPartResponseInputType.None => null,
                    _ => null
                };

                return item;
            })
            .ToList();

        var testQuestionParts = testItems.Fixture
            .Build<DataShareRequestQuestionPartModelData>()
            .CreateMany()
            .Select((item, index) =>
            {
                item.DataShareRequestQuestionPart_Question.QuestionPart_QuestionPartOrderWithinQuestion = index + 1;
                item.DataShareRequestQuestionPart_Question.QuestionPart_QuestionPartType = testQuestionPartType;

                // Here is where we replace the proper response with one of an unexpected type
                item.DataShareRequestQuestionPart_Question.QuestionPart_ResponseFormat = new TestStubQuestionPartResponseFormatModelData
                {
                    InputType = testResponseFormat.InputType,
                    FormatType = (QuestionPartResponseFormatType) Enum.GetValues<QuestionPartResponseFormatType>().Cast<int>().Max() + 1
                };

                item.DataShareRequestQuestionPart_Answer!.QuestionPartAnswer_QuestionPartId = item.DataShareRequestQuestionPart_Question.QuestionPart_Id;
                item.DataShareRequestQuestionPart_Answer.QuestionPartAnswer_AnswerPartResponses = testInputResponses;

                return item;
            })
            .ToList();

        var testInputQuestion = testItems.Fixture
            .Build<DataShareRequestQuestionModelData>()
            .With(x => x.DataShareRequestQuestion_QuestionParts, testQuestionParts)
            .Without(x => x.DataShareRequestQuestion_QuestionFooter)
            .Create();

        Assert.That(() => testItems.AcquirerDataShareRequestModelDataFactory.CreateDataShareRequestQuestion(testInputQuestion),
            Throws.TypeOf<InvalidEnumValueException>());
    }

    private class TestStubQuestionPartResponseFormatModelData : QuestionPartResponseFormatModelData;
    #endregion

    #region CreateDataShareRequestAnswersSummary() Tests
    [Test]
    public void GivenANullDataShareRequestSubmissionResultModelData_WhenICreateDataShareRequestAnswersSummary_ThenAnArgumentNullExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        Assert.That(() => testItems.AcquirerDataShareRequestModelDataFactory.CreateDataShareRequestAnswersSummary(null!),
            Throws.ArgumentNullException.With.Property("ParamName").EqualTo("dataShareRequestAnswersSummaryModelData"));
    }

    [Test]
    [TestCaseSource(nameof(CreateDataShareRequestAnswersSummaryTestCaseData))]
    public void GivenDataShareRequestAnswersSummaryModelData_WhenICreateDataShareRequestAnswersSummary_ThenADataShareRequestAnswersSummaryIsCreatedFromTheModelData(
        QuestionPartResponseInputType testQuestionResponseInputType,
        QuestionPartResponseFormatType testQuestionResponseFormatType,
        DataShareRequestStatusType testDataShareRequestStatus)
    {
        var testItems = CreateTestItems();

        var responseItem = CreateTestResponseItem(testItems.Fixture, testQuestionResponseInputType);

        var testQuestionPartResponses = testItems.Fixture
            .Build<DataShareRequestAnswersSummaryQuestionPartResponseModelData>()
            .Without(x => x.DataShareRequestAnswersSummaryQuestionPartResponse_OrderWithinQuestionPart)
            .With(x => x.DataShareRequestAnswersSummaryQuestionPart_ResponseItem, responseItem)
            .CreateMany()
            .Select((item, index) =>
            {
                item.DataShareRequestAnswersSummaryQuestionPartResponse_OrderWithinQuestionPart = index + 1;
                return item;
            })
            .ToList();

        var testQuestionParts = testItems.Fixture
            .Build<DataShareRequestAnswersSummaryQuestionPartModelData>()
            .Without(x => x.DataShareRequestAnswersSummaryQuestionPart_OrderWithinQuestion)
            .With(x => x.DataShareRequestAnswersSummaryQuestionPart_ResponseInputType, testQuestionResponseInputType)
            .With(x => x.DataShareRequestAnswersSummaryQuestionPart_ResponseFormatType, testQuestionResponseFormatType)
            .With(x => x.DataShareRequestAnswersSummaryQuestionPart_Responses, testQuestionPartResponses)
            .CreateMany()
            .Select((item, index) =>
            {
                item.DataShareRequestAnswersSummaryQuestionPart_OrderWithinQuestion = index + 1;
                return item;
            })
            .ToList();

        var testSummaryMainQuestion = testItems.Fixture
            .Build<DataShareRequestAnswersSummaryQuestionModelData>()
            .With(x => x.DataShareRequestAnswersSummaryQuestion_QuestionParts, testQuestionParts)
            .Create();

        var testSummaryBackingQuestions = testItems.Fixture
            .Build<DataShareRequestAnswersSummaryQuestionModelData>()
            .With(x => x.DataShareRequestAnswersSummaryQuestion_QuestionParts, testQuestionParts)
            .CreateMany()
            .ToList();

        var testQuestionGroups = testItems.Fixture
            .Build<DataShareRequestAnswersSummaryQuestionGroupModelData>()
            .Without(x => x.DataShareRequestAnswersSummaryQuestionGroup_OrderWithinSection)
            .With(x => x.DataShareRequestAnswersSummaryQuestionGroup_SummaryMainQuestion, testSummaryMainQuestion)
            .With(x => x.DataShareRequestAnswersSummaryQuestionGroup_SummaryBackingQuestions, testSummaryBackingQuestions)
            .CreateMany()
            .Select((item, index) =>
            {
                item.DataShareRequestAnswersSummaryQuestionGroup_OrderWithinSection = index + 1;
                return item;
            })
            .ToList();

        var testSummarySections = testItems.Fixture
            .Build<DataShareRequestAnswersSummarySectionModelData>()
            .Without(x => x.DataShareRequestAnswersSummarySection_OrderWithinSummary)
            .With(x => x.DataShareRequestAnswersSummarySection_QuestionGroups, testQuestionGroups)
            .CreateMany()
            .Select((item, index) =>
            {
                item.DataShareRequestAnswersSummarySection_OrderWithinSummary = index + 1;
                return item;
            })
            .ToList();

        var testDataShareRequestSubmissionResultModelData = testItems.Fixture
            .Build<DataShareRequestAnswersSummaryModelData>()
            .With(x => x.DataShareRequestAnswersSummary_RequestStatus, testDataShareRequestStatus)
            .With(x => x.DataShareRequestAnswersSummary_SummarySections, testSummarySections)
            .Create();

        var result = testItems.AcquirerDataShareRequestModelDataFactory.CreateDataShareRequestAnswersSummary(
            testDataShareRequestSubmissionResultModelData);

        Assert.Multiple(() =>
        {
            Assert.That(result.DataShareRequestId, Is.EqualTo(testDataShareRequestSubmissionResultModelData.DataShareRequestAnswersSummary_DataShareRequestId));
            Assert.That(result.RequestId, Is.EqualTo(testDataShareRequestSubmissionResultModelData.DataShareRequestAnswersSummary_RequestId));
            Assert.That(result.EsdaName, Is.EqualTo(testDataShareRequestSubmissionResultModelData.DataShareRequestAnswersSummary_EsdaName));
            Assert.That(result.DataShareRequestStatus, Is.EqualTo(GetExpectedDataShareRequestStatus(testDataShareRequestSubmissionResultModelData.DataShareRequestAnswersSummary_RequestStatus)));
            Assert.That(result.QuestionsRemainThatRequireAResponse, Is.EqualTo(testDataShareRequestSubmissionResultModelData.DataShareRequestAnswersSummary_QuestionsRemainThatRequireAResponse));
            Assert.That(result.SubmissionResponseFromSupplier, Is.EqualTo(testDataShareRequestSubmissionResultModelData.DataShareRequestAnswersSummary_SubmissionResponseFromSupplier));
            Assert.That(result.CancellationReasonsFromAcquirer, Is.EqualTo(testDataShareRequestSubmissionResultModelData.DataShareRequestAnswersSummary_CancellationReasonsFromAcquirer));

            Assert.That(result.SummarySections, Has.Exactly(testDataShareRequestSubmissionResultModelData.DataShareRequestAnswersSummary_SummarySections.Count).Items);

            foreach (var testSummarySection in testDataShareRequestSubmissionResultModelData.DataShareRequestAnswersSummary_SummarySections)
            {
                var resultSummarySection = result.SummarySections.FirstOrDefault(x =>
                    x.SectionHeader == testSummarySection.DataShareRequestAnswersSummarySection_SectionHeader);

                Assert.That(resultSummarySection, Is.Not.Null);

                Assert.That(resultSummarySection!.OrderWithinSummary, Is.EqualTo(testSummarySection.DataShareRequestAnswersSummarySection_OrderWithinSummary));
                
                Assert.That(resultSummarySection.SummaryQuestionGroups, Has.Exactly(testSummarySection.DataShareRequestAnswersSummarySection_QuestionGroups.Count).Items);

                foreach (var testSummaryQuestionGroup in testSummarySection.DataShareRequestAnswersSummarySection_QuestionGroups)
                {
                    var resultSummaryQuestionGroup = resultSummarySection.SummaryQuestionGroups.FirstOrDefault(x =>
                        x.OrderWithinSection == testSummaryQuestionGroup.DataShareRequestAnswersSummaryQuestionGroup_OrderWithinSection);

                    Assert.That(resultSummaryQuestionGroup, Is.Not.Null);

                    // Main Question
                    var testMainQuestion = testSummaryQuestionGroup.DataShareRequestAnswersSummaryQuestionGroup_SummaryMainQuestion;
                    var resultMainQuestionSummary = resultSummaryQuestionGroup!.MainQuestionSummary;
                    Assert.That(resultMainQuestionSummary.QuestionId, Is.EqualTo(testMainQuestion.DataShareRequestAnswersSummaryQuestion_QuestionId));
                    Assert.That(resultMainQuestionSummary.QuestionHeader, Is.EqualTo(testMainQuestion.DataShareRequestAnswersSummaryQuestion_QuestionHeader));
                    Assert.That(resultMainQuestionSummary.QuestionIsApplicable, Is.EqualTo(testMainQuestion.DataShareRequestAnswersSummaryQuestion_QuestionIsApplicable));

                    var testMainQuestionParts = testMainQuestion.DataShareRequestAnswersSummaryQuestion_QuestionParts;
                    var resultMainQuestionParts = resultMainQuestionSummary.SummaryQuestionParts;

                    Assert.That(resultMainQuestionParts, Has.Exactly(testMainQuestionParts.Count).Items);
                    foreach (var testMainQuestionPart in testMainQuestionParts)
                    {
                        var resultMainQuestionPart = resultMainQuestionParts.FirstOrDefault(x =>
                            x.OrderWithinQuestion == testMainQuestionPart.DataShareRequestAnswersSummaryQuestionPart_OrderWithinQuestion);

                        Assert.That(resultMainQuestionPart, Is.Not.Null);

                        Assert.That(resultMainQuestionPart!.OrderWithinQuestion, Is.EqualTo(testMainQuestionPart.DataShareRequestAnswersSummaryQuestionPart_OrderWithinQuestion));
                        Assert.That(resultMainQuestionPart.QuestionPartText, Is.EqualTo(testMainQuestionPart.DataShareRequestAnswersSummaryQuestionPart_QuestionPartText));
                        Assert.That(resultMainQuestionPart.MultipleResponsesAllowed, Is.EqualTo(testMainQuestionPart.DataShareRequestAnswersSummaryQuestionPart_MultipleResponsesAllowed));
                        Assert.That(resultMainQuestionPart.MultipleResponsesCollectionHeaderIfMultipleResponsesAllowed, Is.EqualTo(testMainQuestionPart.DataShareRequestAnswersSummaryQuestionPart_MultipleResponsesCollectionDescriptionIfMultipleResponsesAllowed));
                        Assert.That(resultMainQuestionPart.ResponseInputType, Is.EqualTo(GetExpectedResponseInputType(testMainQuestionPart.DataShareRequestAnswersSummaryQuestionPart_ResponseInputType)));
                        Assert.That(resultMainQuestionPart.ResponseFormatType, Is.EqualTo(GetExpectedResponseFormatType(testMainQuestionPart.DataShareRequestAnswersSummaryQuestionPart_ResponseFormatType)));

                        var testResponses = testMainQuestionPart.DataShareRequestAnswersSummaryQuestionPart_Responses;
                        var resultResponses = resultMainQuestionPart.Responses;

                        Assert.That(resultResponses, Has.Exactly(testResponses.Count).Items);

                        foreach (var testResponse in testResponses)
                        {
                            var resultResponse = resultResponses.FirstOrDefault(x =>
                                x.OrderWithinQuestionPartAnswer == testResponse.DataShareRequestAnswersSummaryQuestionPartResponse_OrderWithinQuestionPart);

                            Assert.That(resultResponse, Is.Not.Null);

                            var resultResponseItem = resultResponse!.QuestionPartAnswerResponseItem;
                            var testResponseItem = testResponse.DataShareRequestAnswersSummaryQuestionPart_ResponseItem;

                            if (testMainQuestionPart.DataShareRequestAnswersSummaryQuestionPart_ResponseInputType == QuestionPartResponseInputType.None) continue;

                            Assert.That(resultResponseItem!.InputType, Is.EqualTo(GetExpectedResponseInputType(testResponseItem!.DataShareRequestAnswersSummaryQuestionPartAnswerResponseItem_ResponseInputType)));
                            
                            // Now check polymorphic response types ...
                        }
                    }
                }
            }
        });
    }
    
    private static IEnumerable<TestCaseData> CreateDataShareRequestAnswersSummaryTestCaseData()
    {
        foreach (var requestStatus in ValidInputDataShareRequestStatuses)
        {
            yield return new TestCaseData(QuestionPartResponseInputType.FreeForm, QuestionPartResponseFormatType.Text, requestStatus);
            yield return new TestCaseData(QuestionPartResponseInputType.FreeForm, QuestionPartResponseFormatType.Numeric, requestStatus);
            yield return new TestCaseData(QuestionPartResponseInputType.FreeForm, QuestionPartResponseFormatType.Date, requestStatus);
            yield return new TestCaseData(QuestionPartResponseInputType.FreeForm, QuestionPartResponseFormatType.Time, requestStatus);
            yield return new TestCaseData(QuestionPartResponseInputType.FreeForm, QuestionPartResponseFormatType.DateTime, requestStatus);
            yield return new TestCaseData(QuestionPartResponseInputType.FreeForm, QuestionPartResponseFormatType.Country, requestStatus);

            yield return new TestCaseData(QuestionPartResponseInputType.OptionSelection, QuestionPartResponseFormatType.SelectSingle, requestStatus);
            yield return new TestCaseData(QuestionPartResponseInputType.OptionSelection, QuestionPartResponseFormatType.SelectMulti, requestStatus);

            yield return new TestCaseData(QuestionPartResponseInputType.None, QuestionPartResponseFormatType.ReadOnly, requestStatus);
        }
    }



    private static DataShareRequestAnswersSummaryQuestionPartAnswerResponseItemModelData? CreateTestResponseItem(
        ISpecimenBuilder fixture,
        QuestionPartResponseInputType responseInputType)
    {
        return responseInputType switch
        {
            QuestionPartResponseInputType.FreeForm => fixture.Create<DataShareRequestAnswersSummaryQuestionPartAnswerResponseItemFreeFormModelData>(),

            QuestionPartResponseInputType.OptionSelection => fixture.Create<DataShareRequestAnswersSummaryQuestionPartAnswerResponseItemOptionSelectionModelData>(),

            QuestionPartResponseInputType.None => null,
            _ => null
        };
    }

    [Test]
    public void GivenDataShareRequestAnswersSummaryModelDataWithAQuestionPartOfUnknownFormatType_WhenICreateDataShareRequestAnswersSummary_ThenAnInvalidEnumValueExceptionIsThrown()
    {
        var testItems = CreateTestItems();
        
        var invalidResponseFormatType = (QuestionPartResponseFormatType) Enum.GetValues<QuestionPartResponseFormatType>().Cast<int>().Max() + 1;

        var testQuestionParts = testItems.Fixture
            .Build<DataShareRequestAnswersSummaryQuestionPartModelData>()
            .With(x => x.DataShareRequestAnswersSummaryQuestionPart_ResponseFormatType, invalidResponseFormatType)
            .CreateMany()
            .ToList();

        var testSummaryMainQuestion = testItems.Fixture
            .Build<DataShareRequestAnswersSummaryQuestionModelData>()
            .With(x => x.DataShareRequestAnswersSummaryQuestion_QuestionParts, testQuestionParts)
            .Create();

        var testQuestionGroups = testItems.Fixture
            .Build<DataShareRequestAnswersSummaryQuestionGroupModelData>()
            .Without(x => x.DataShareRequestAnswersSummaryQuestionGroup_OrderWithinSection)
            .With(x => x.DataShareRequestAnswersSummaryQuestionGroup_SummaryMainQuestion, testSummaryMainQuestion)
            .With(x => x.DataShareRequestAnswersSummaryQuestionGroup_SummaryBackingQuestions, [])
            .CreateMany()
            .ToList();

        var testSummarySections = testItems.Fixture
            .Build<DataShareRequestAnswersSummarySectionModelData>()
            .Without(x => x.DataShareRequestAnswersSummarySection_OrderWithinSummary)
            .With(x => x.DataShareRequestAnswersSummarySection_QuestionGroups, testQuestionGroups)
            .CreateMany()
            .ToList();

        var testDataShareRequestSubmissionResultModelData = testItems.Fixture
            .Build<DataShareRequestAnswersSummaryModelData>()
            .With(x => x.DataShareRequestAnswersSummary_RequestStatus, ValidInputDataShareRequestStatuses.First())
            .With(x => x.DataShareRequestAnswersSummary_SummarySections, testSummarySections)
            .Create();

        Assert.That(() => testItems.AcquirerDataShareRequestModelDataFactory.CreateDataShareRequestAnswersSummary(testDataShareRequestSubmissionResultModelData),
            Throws.TypeOf<InvalidEnumValueException>().With.Message.EqualTo("Unknown ResponseFormatType"));
    }

    [Test]
    public void GivenDataShareRequestAnswersSummaryModelDataWithAQuestionPartResponseOfNoneInputType_WhenICreateDataShareRequestAnswersSummary_ThenAnInvalidOperationExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        var responseItem = new TestNoResponseClassWithResponse();

        var testQuestionPartResponses = testItems.Fixture
            .Build<DataShareRequestAnswersSummaryQuestionPartResponseModelData>()
            .With(x => x.DataShareRequestAnswersSummaryQuestionPart_ResponseItem, responseItem)
            .CreateMany()
            .ToList();

        var testQuestionParts = testItems.Fixture
            .Build<DataShareRequestAnswersSummaryQuestionPartModelData>()
            .With(x => x.DataShareRequestAnswersSummaryQuestionPart_Responses, testQuestionPartResponses)
            .CreateMany()
            .ToList();

        var testSummaryMainQuestion = testItems.Fixture
            .Build<DataShareRequestAnswersSummaryQuestionModelData>()
            .With(x => x.DataShareRequestAnswersSummaryQuestion_QuestionParts, testQuestionParts)
            .Create();

        var testQuestionGroups = testItems.Fixture
            .Build<DataShareRequestAnswersSummaryQuestionGroupModelData>()
            .Without(x => x.DataShareRequestAnswersSummaryQuestionGroup_OrderWithinSection)
            .With(x => x.DataShareRequestAnswersSummaryQuestionGroup_SummaryMainQuestion, testSummaryMainQuestion)
            .With(x => x.DataShareRequestAnswersSummaryQuestionGroup_SummaryBackingQuestions, [])
            .CreateMany()
            .ToList();

        var testSummarySections = testItems.Fixture
            .Build<DataShareRequestAnswersSummarySectionModelData>()
            .Without(x => x.DataShareRequestAnswersSummarySection_OrderWithinSummary)
            .With(x => x.DataShareRequestAnswersSummarySection_QuestionGroups, testQuestionGroups)
            .CreateMany()
            .ToList();

        var testDataShareRequestSubmissionResultModelData = testItems.Fixture
            .Build<DataShareRequestAnswersSummaryModelData>()
            .With(x => x.DataShareRequestAnswersSummary_RequestStatus, ValidInputDataShareRequestStatuses.First())
            .With(x => x.DataShareRequestAnswersSummary_SummarySections, testSummarySections)
            .Create();

        Assert.That(() => testItems.AcquirerDataShareRequestModelDataFactory.CreateDataShareRequestAnswersSummary(testDataShareRequestSubmissionResultModelData),
            Throws.TypeOf<InvalidOperationException>().With.Message.EqualTo("Answer Response Item has NoInput response type"));
    }

    private class TestNoResponseClassWithResponse : DataShareRequestAnswersSummaryQuestionPartAnswerResponseItemModelData
    {
        public override QuestionPartResponseInputType DataShareRequestAnswersSummaryQuestionPartAnswerResponseItem_ResponseInputType => QuestionPartResponseInputType.None;
    };

    [Test]
    public void GivenDataShareRequestAnswersSummaryModelDataWithAQuestionPartResponseOfInvalidInputType_WhenICreateDataShareRequestAnswersSummary_ThenAnInvalidEnumValueExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        var responseItem = new TestInvalidResponseClass();

        var testQuestionPartResponses = testItems.Fixture
            .Build<DataShareRequestAnswersSummaryQuestionPartResponseModelData>()
            .With(x => x.DataShareRequestAnswersSummaryQuestionPart_ResponseItem, responseItem)
            .CreateMany()
            .ToList();

        var testQuestionParts = testItems.Fixture
            .Build<DataShareRequestAnswersSummaryQuestionPartModelData>()
            .With(x => x.DataShareRequestAnswersSummaryQuestionPart_Responses, testQuestionPartResponses)
            .CreateMany()
            .ToList();

        var testSummaryMainQuestion = testItems.Fixture
            .Build<DataShareRequestAnswersSummaryQuestionModelData>()
            .With(x => x.DataShareRequestAnswersSummaryQuestion_QuestionParts, testQuestionParts)
            .Create();

        var testQuestionGroups = testItems.Fixture
            .Build<DataShareRequestAnswersSummaryQuestionGroupModelData>()
            .Without(x => x.DataShareRequestAnswersSummaryQuestionGroup_OrderWithinSection)
            .With(x => x.DataShareRequestAnswersSummaryQuestionGroup_SummaryMainQuestion, testSummaryMainQuestion)
            .With(x => x.DataShareRequestAnswersSummaryQuestionGroup_SummaryBackingQuestions, [])
            .CreateMany()
            .ToList();

        var testSummarySections = testItems.Fixture
            .Build<DataShareRequestAnswersSummarySectionModelData>()
            .Without(x => x.DataShareRequestAnswersSummarySection_OrderWithinSummary)
            .With(x => x.DataShareRequestAnswersSummarySection_QuestionGroups, testQuestionGroups)
            .CreateMany()
            .ToList();

        var testDataShareRequestSubmissionResultModelData = testItems.Fixture
            .Build<DataShareRequestAnswersSummaryModelData>()
            .With(x => x.DataShareRequestAnswersSummary_RequestStatus, ValidInputDataShareRequestStatuses.First())
            .With(x => x.DataShareRequestAnswersSummary_SummarySections, testSummarySections)
            .Create();

        Assert.That(() => testItems.AcquirerDataShareRequestModelDataFactory.CreateDataShareRequestAnswersSummary(testDataShareRequestSubmissionResultModelData),
            Throws.TypeOf<InvalidEnumValueException>().With.Message.EqualTo("QuestionPartAnswerResponseItem has unknown InputType"));
    }

    private class TestInvalidResponseClass : DataShareRequestAnswersSummaryQuestionPartAnswerResponseItemModelData
    {
        public override QuestionPartResponseInputType DataShareRequestAnswersSummaryQuestionPartAnswerResponseItem_ResponseInputType =>
            (QuestionPartResponseInputType) Enum.GetValues<QuestionPartResponseInputType>().Cast<int>().Max() + 1;
    };


    [Test]
    public void GivenDataShareRequestAnswersSummaryModelDataWithAQuestionPartOfUnknownInputType_WhenICreateDataShareRequestAnswersSummary_ThenAnInvalidEnumValueExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        var invalidResponseInputType = (QuestionPartResponseInputType)Enum.GetValues<QuestionPartResponseInputType>().Cast<int>().Max() + 1;

        var testQuestionParts = testItems.Fixture
            .Build<DataShareRequestAnswersSummaryQuestionPartModelData>()
            .With(x => x.DataShareRequestAnswersSummaryQuestionPart_ResponseInputType, invalidResponseInputType)
            .CreateMany()
            .ToList();

        var testSummaryMainQuestion = testItems.Fixture
            .Build<DataShareRequestAnswersSummaryQuestionModelData>()
            .With(x => x.DataShareRequestAnswersSummaryQuestion_QuestionParts, testQuestionParts)
            .Create();

        var testQuestionGroups = testItems.Fixture
            .Build<DataShareRequestAnswersSummaryQuestionGroupModelData>()
            .Without(x => x.DataShareRequestAnswersSummaryQuestionGroup_OrderWithinSection)
            .With(x => x.DataShareRequestAnswersSummaryQuestionGroup_SummaryMainQuestion, testSummaryMainQuestion)
            .With(x => x.DataShareRequestAnswersSummaryQuestionGroup_SummaryBackingQuestions, [])
            .CreateMany()
            .ToList();

        var testSummarySections = testItems.Fixture
            .Build<DataShareRequestAnswersSummarySectionModelData>()
            .Without(x => x.DataShareRequestAnswersSummarySection_OrderWithinSummary)
            .With(x => x.DataShareRequestAnswersSummarySection_QuestionGroups, testQuestionGroups)
            .CreateMany()
            .ToList();

        var testDataShareRequestSubmissionResultModelData = testItems.Fixture
            .Build<DataShareRequestAnswersSummaryModelData>()
            .With(x => x.DataShareRequestAnswersSummary_RequestStatus, ValidInputDataShareRequestStatuses.First())
            .With(x => x.DataShareRequestAnswersSummary_SummarySections, testSummarySections)
            .Create();

        Assert.That(() => testItems.AcquirerDataShareRequestModelDataFactory.CreateDataShareRequestAnswersSummary(testDataShareRequestSubmissionResultModelData),
            Throws.TypeOf<InvalidEnumValueException>().With.Message.EqualTo("Unsupported response input type"));
    }
    #endregion

    #region CreateDataShareRequestSubmissionResult() Tests
    [Test]
    public void GivenADataShareRequestSubmissionResultModelData_WhenICreateDataShareRequestSubmissionResult_ThenADataShareRequestSubmissionResultIsCreatedUsingTheGivenModelData()
    {
        var testItems = CreateTestItems();

        var testDataShareRequestSubmissionResultModelData = testItems.Fixture.Create<DataShareRequestSubmissionResultModelData>();

        var result = testItems.AcquirerDataShareRequestModelDataFactory.CreateDataShareRequestSubmissionResult(
            testDataShareRequestSubmissionResultModelData,
            It.IsAny<bool>());

        Assert.Multiple(() =>
        {
            Assert.That(result.DataShareRequestId, Is.EqualTo(testDataShareRequestSubmissionResultModelData.DataShareRequest_Id));
            Assert.That(result.DataShareRequestRequestId, Is.EqualTo(testDataShareRequestSubmissionResultModelData.DataShareRequest_RequestId));
        });
    }

    [Theory]
    public void GivenANotificationsSuccess_WhenICreateDataShareRequestSubmissionResult_ThenADataShareRequestSubmissionResultIsCreatedUsingTheGivenNotificationsSuccess(
        bool testNotificationsSuccess)
    {
        var testItems = CreateTestItems();

        var result = testItems.AcquirerDataShareRequestModelDataFactory.CreateDataShareRequestSubmissionResult(
            testItems.Fixture.Create<DataShareRequestSubmissionResultModelData>(),
            testNotificationsSuccess);

        Assert.That(result.NotificationSuccess, Is.EqualTo(GetExpectedNotificationSuccess(testNotificationsSuccess)));
    }
    #endregion

    #region CreateDataShareRequestCancellationResult() Tests
    [Test]
    public void GivenNullReasonsForCancellation_WhenICreateDataShareRequestCancellationResult_ThenAnArgumentNullExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        Assert.That(() => testItems.AcquirerDataShareRequestModelDataFactory.CreateDataShareRequestCancellationResult(
                testItems.Fixture.Create<Guid>(),
                null!,
                It.IsAny<bool>()),
            Throws.ArgumentNullException.With.Property("ParamName").EqualTo("reasonsForCancellation"));
    }
    
    [Test]
    public void GivenInputParameters_WhenICreateDataShareRequestCancellationResult_ThenADataShareRequestCancellationResultIsCreatedUsingTheGivenInputParameters()
    {
        var testItems = CreateTestItems();

        var testDataShareRequestId = testItems.Fixture.Create<Guid>();
        var testReasonsForCancellation = testItems.Fixture.Create<string>();

        var result = testItems.AcquirerDataShareRequestModelDataFactory.CreateDataShareRequestCancellationResult(
            testDataShareRequestId,
            testReasonsForCancellation,
            It.IsAny<bool>());

        Assert.Multiple(() =>
        {
            Assert.That(result.DataShareRequestId, Is.EqualTo(testDataShareRequestId));
            Assert.That(result.ReasonsForCancellation, Is.EqualTo(testReasonsForCancellation));
        });
    }

    [Theory]
    public void GivenANotificationsSuccess_WhenICreateDataShareRequestCancellationResult_ThenADataShareRequestCancellationResultIsCreatedUsingTheGivenNotificationsSuccess(
        bool testNotificationsSuccess)
    {
        var testItems = CreateTestItems();

        var result = testItems.AcquirerDataShareRequestModelDataFactory.CreateDataShareRequestCancellationResult(
            testItems.Fixture.Create<Guid>(),
            testItems.Fixture.Create<string>(),
            testNotificationsSuccess);

        Assert.That(result.NotificationSuccess, Is.EqualTo(GetExpectedNotificationSuccess(testNotificationsSuccess)));
    }
    #endregion

    #region CreateDataShareRequestRaisedForEsdaByAcquirerOrganisationSummary() Tests
    [Test]
    public void GivenInputParameters_WhenICreateDataShareRequestRaisedForEsdaByAcquirerOrganisationSummary_ThenADataShareRequestRaisedForEsdaByAcquirerOrganisationSummaryIsCreatedFromThoseParameters(
        [ValueSource(nameof(ValidInputDataShareRequestStatuses))] DataShareRequestStatusType testDataShareRequestStatus)
    {
        var testItems = CreateTestItems();

        var testDataShareRequestModelData = testItems.Fixture.Build<DataShareRequestModelData>()
            .With(x => x.DataShareRequest_RequestStatus, testDataShareRequestStatus)
            .Create();
        var testAuditLogForCreation = testItems.Fixture.Create<AuditLogDataShareRequestStatusChangeModelData>();
        var testAuditLogForMostRecentSubmission = testItems.Fixture.Create<AuditLogDataShareRequestStatusChangeModelData>();
        var testUserDetails = testItems.Fixture.Create<UserDetails>();

        var result = testItems.AcquirerDataShareRequestModelDataFactory.CreateDataShareRequestRaisedForEsdaByAcquirerOrganisationSummary(
            testDataShareRequestModelData,
            testAuditLogForCreation,
            testAuditLogForMostRecentSubmission,
            testUserDetails);

        Assert.Multiple(() =>
        {
            Assert.That(result.Id, Is.EqualTo(testDataShareRequestModelData.DataShareRequest_Id));
            Assert.That(result.RequestId, Is.EqualTo(testDataShareRequestModelData.DataShareRequest_RequestId));
            Assert.That(result.Status, Is.EqualTo(GetExpectedDataShareRequestStatus(testDataShareRequestModelData.DataShareRequest_RequestStatus)));
            Assert.That(result.DateStarted, Is.EqualTo(testAuditLogForCreation.AuditLogDataShareRequestStatusChange_ChangedAtUtc));
            Assert.That(result.DateSubmitted, Is.EqualTo(testAuditLogForMostRecentSubmission.AuditLogDataShareRequestStatusChange_ChangedAtUtc));

            Assert.That(result.OriginatingAcquirerContactDetails.UserName, Is.EqualTo(testUserDetails.UserContactDetails.UserName));
            Assert.That(result.OriginatingAcquirerContactDetails.EmailAddress, Is.EqualTo(testUserDetails.UserContactDetails.EmailAddress));
        });
    }
    #endregion

    #region CreateQuestionAnswerWriteData() Tests
    [Test]
    public void GivenANullDataShareRequestQuestionAnswer_WhenICreateQuestionAnswerWriteData_ThenAnArgumentNullExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        Assert.That(() => testItems.AcquirerDataShareRequestModelDataFactory.CreateQuestionAnswerWriteData(null!),
            Throws.ArgumentNullException.With.Property("ParamName").EqualTo("questionAnswer"));
    }
    
    [Test]
    public void GivenADataShareRequestQuestionAnswerForFreeFormQuestion_WhenICreateQuestionAnswerWriteData_ThenADataShareRequestQuestionAnswerWriteModelDataIsCreatedUsingTheGivenAnswer()
    {
        var testItems = CreateTestItems();

        var testInputAnswerPartFreeFormResponses = testItems.Fixture
            .Build<DataShareRequestQuestionAnswerPartResponseFreeForm>()
            .With(x => x.InputType, Dto.Models.Questions.QuestionParts.ResponseFormats.QuestionPartResponseInputType.FreeForm)
            .CreateMany()
            .Select((item, index) =>
            {
                item.OrderWithinAnswerPart = index + 1;
                return item;
            })
            .ToList();

        var testInputAnswerPartResponses = new List<DataShareRequestQuestionAnswerPartResponseBase>();
        testInputAnswerPartResponses.AddRange(testInputAnswerPartFreeFormResponses);

        var testInputAnswerParts = testItems.Fixture
            .Build<DataShareRequestQuestionAnswerPart>()
            .With(x => x.AnswerPartResponses, testInputAnswerPartResponses)
            .CreateMany()
            .ToList();

        var testDataShareRequestQuestionAnswer = testItems.Fixture.Build<DataShareRequestQuestionAnswer>()
            .With(x => x.AnswerParts, testInputAnswerParts)
            .Create();

        var result = testItems.AcquirerDataShareRequestModelDataFactory.CreateQuestionAnswerWriteData(testDataShareRequestQuestionAnswer);

        Assert.Multiple(() =>
        {
            Assert.That(result.DataShareRequestId, Is.EqualTo(testDataShareRequestQuestionAnswer.DataShareRequestId));
            Assert.That(result.QuestionId, Is.EqualTo(testDataShareRequestQuestionAnswer.QuestionId));

            Assert.That(result.AnswerParts, Has.Exactly(testDataShareRequestQuestionAnswer.AnswerParts.Count).Items);

            foreach (var testAnswerPart in testDataShareRequestQuestionAnswer.AnswerParts)
            {
                var resultAnswerPart = result.AnswerParts.FirstOrDefault(x =>
                    x.QuestionPartId == testAnswerPart.QuestionPartId);

                Assert.That(resultAnswerPart, Is.Not.Null);

                Assert.That(resultAnswerPart!.AnswerPartResponses, Has.Exactly(testAnswerPart.AnswerPartResponses.Count).Items);

                foreach (var testAnswerPartResponse in testAnswerPart.AnswerPartResponses)
                {
                    var resultAnswerPartResponse = resultAnswerPart.AnswerPartResponses.FirstOrDefault(x => 
                        x.OrderWithinAnswerPart == testAnswerPartResponse.OrderWithinAnswerPart);

                    Assert.That(resultAnswerPartResponse, Is.Not.Null);

                    Assert.That(resultAnswerPartResponse, Is.TypeOf<DataShareRequestQuestionAnswerPartResponseFreeFormWriteModelData>());

                    var testFreeFormAnswerPartResponse = testAnswerPartResponse as DataShareRequestQuestionAnswerPartResponseFreeForm;
                    var resultFreeFormAnswerPartResponse = resultAnswerPartResponse as DataShareRequestQuestionAnswerPartResponseFreeFormWriteModelData;

                    Assert.That(resultFreeFormAnswerPartResponse!.InputType, Is.EqualTo(QuestionPartResponseInputType.FreeForm));
                    Assert.That(resultFreeFormAnswerPartResponse.EnteredValue, Is.EqualTo(testFreeFormAnswerPartResponse!.EnteredValue));
                    Assert.That(resultFreeFormAnswerPartResponse.ValueEntryDeclined, Is.EqualTo(testFreeFormAnswerPartResponse.ValueEntryDeclined));
                }
            }
        });
    }

    [Test]
    public void GivenADataShareRequestQuestionAnswerForSelectionOptionQuestion_WhenICreateQuestionAnswerWriteData_ThenADataShareRequestQuestionAnswerWriteModelDataIsCreatedUsingTheGivenAnswer()
    {
        var testItems = CreateTestItems();

        var testInputSelectedOptionItems = testItems.Fixture
            .Build<DataShareRequestQuestionAnswerPartResponseSelectionOptionItem>()
            .With(x => x.SupplementaryQuestionAnswerPart, (DataShareRequestQuestionAnswerPart?) null)
            .CreateMany()
            .ToList();

        var testInputAnswerPartSelectionOptionResponses = testItems.Fixture
            .Build<DataShareRequestQuestionAnswerPartResponseSelectionOption>()
            .With(x => x.InputType, Dto.Models.Questions.QuestionParts.ResponseFormats.QuestionPartResponseInputType.OptionSelection)
            .With(x => x.SelectedOptionItems, testInputSelectedOptionItems)
            .CreateMany()
            .Select((item, index) =>
            {
                item.OrderWithinAnswerPart = index + 1;
                return item;
            })
            .ToList();

        var testInputAnswerPartResponses = new List<DataShareRequestQuestionAnswerPartResponseBase>();
        testInputAnswerPartResponses.AddRange(testInputAnswerPartSelectionOptionResponses);

        var testInputAnswerParts = testItems.Fixture
            .Build<DataShareRequestQuestionAnswerPart>()
            .With(x => x.AnswerPartResponses, testInputAnswerPartResponses)
            .CreateMany()
            .ToList();

        var testDataShareRequestQuestionAnswer = testItems.Fixture.Build<DataShareRequestQuestionAnswer>()
            .With(x => x.AnswerParts, testInputAnswerParts)
            .Create();

        var result = testItems.AcquirerDataShareRequestModelDataFactory.CreateQuestionAnswerWriteData(testDataShareRequestQuestionAnswer);

        Assert.Multiple(() =>
        {
            Assert.That(result.DataShareRequestId, Is.EqualTo(testDataShareRequestQuestionAnswer.DataShareRequestId));
            Assert.That(result.QuestionId, Is.EqualTo(testDataShareRequestQuestionAnswer.QuestionId));

            Assert.That(result.AnswerParts, Has.Exactly(testDataShareRequestQuestionAnswer.AnswerParts.Count).Items);

            foreach (var testAnswerPart in testDataShareRequestQuestionAnswer.AnswerParts)
            {
                var resultAnswerPart = result.AnswerParts.FirstOrDefault(x =>
                    x.QuestionPartId == testAnswerPart.QuestionPartId);

                Assert.That(resultAnswerPart, Is.Not.Null);

                Assert.That(resultAnswerPart!.AnswerPartResponses, Has.Exactly(testAnswerPart.AnswerPartResponses.Count).Items);

                foreach (var testAnswerPartResponse in testAnswerPart.AnswerPartResponses)
                {
                    var resultAnswerPartResponse = resultAnswerPart.AnswerPartResponses.FirstOrDefault(x =>
                        x.OrderWithinAnswerPart == testAnswerPartResponse.OrderWithinAnswerPart);

                    Assert.That(resultAnswerPartResponse, Is.Not.Null);

                    Assert.That(resultAnswerPartResponse, Is.TypeOf<DataShareRequestQuestionAnswerPartResponseOptionSelectionWriteModelData>());

                    var testSelectionOptionAnswerPartResponse = testAnswerPartResponse as DataShareRequestQuestionAnswerPartResponseSelectionOption;
                    var resultSelectionOptionAnswerPartResponse = resultAnswerPartResponse as DataShareRequestQuestionAnswerPartResponseOptionSelectionWriteModelData;

                    Assert.That(resultSelectionOptionAnswerPartResponse!.InputType, Is.EqualTo(QuestionPartResponseInputType.OptionSelection));

                    Assert.That(resultSelectionOptionAnswerPartResponse.SelectionOptions, Has.Exactly(testSelectionOptionAnswerPartResponse!.SelectedOptionItems.Count).Items);
                }
            }
        });
    }

    [Test]
    public void GivenADataShareRequestQuestionAnswerForNoResponseQuestion_WhenICreateQuestionAnswerWriteData_ThenAnInvalidOperationExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        var testInputAnswerPartFreeFormResponses = testItems.Fixture
            .Build<DataShareRequestQuestionAnswerPartResponseFreeForm>()
            .With(x => x.InputType, Dto.Models.Questions.QuestionParts.ResponseFormats.QuestionPartResponseInputType.None)
            .CreateMany()
            .ToList();

        var testInputAnswerPartResponses = new List<DataShareRequestQuestionAnswerPartResponseBase>();
        testInputAnswerPartResponses.AddRange(testInputAnswerPartFreeFormResponses);

        var testInputAnswerParts = testItems.Fixture
            .Build<DataShareRequestQuestionAnswerPart>()
            .With(x => x.AnswerPartResponses, testInputAnswerPartResponses)
            .CreateMany()
            .ToList();

        var testDataShareRequestQuestionAnswer = testItems.Fixture.Build<DataShareRequestQuestionAnswer>()
            .With(x => x.AnswerParts, testInputAnswerParts)
            .Create();

        Assert.That(() => testItems.AcquirerDataShareRequestModelDataFactory.CreateQuestionAnswerWriteData(testDataShareRequestQuestionAnswer),
            Throws.TypeOf<InvalidOperationException>().With.Message.EqualTo("QuestionAnswerPartResponse received for a NoInput InputType"));
    }

    [Test]
    public void GivenADataShareRequestQuestionAnswerForQuestionOfInvalidInputType_WhenICreateQuestionAnswerWriteData_ThenAnInvalidEnumValueExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        var invalidInputType = (Dto.Models.Questions.QuestionParts.ResponseFormats.QuestionPartResponseInputType)
            Enum.GetValues<Dto.Models.Questions.QuestionParts.ResponseFormats.QuestionPartResponseInputType>().Cast<int>().Max() + 1;

        var testInputAnswerPartFreeFormResponses = testItems.Fixture
            .Build<DataShareRequestQuestionAnswerPartResponseFreeForm>()
            .With(x => x.InputType, invalidInputType)
            .CreateMany()
            .ToList();

        var testInputAnswerPartResponses = new List<DataShareRequestQuestionAnswerPartResponseBase>();
        testInputAnswerPartResponses.AddRange(testInputAnswerPartFreeFormResponses);

        var testInputAnswerParts = testItems.Fixture
            .Build<DataShareRequestQuestionAnswerPart>()
            .With(x => x.AnswerPartResponses, testInputAnswerPartResponses)
            .CreateMany()
            .ToList();

        var testDataShareRequestQuestionAnswer = testItems.Fixture.Build<DataShareRequestQuestionAnswer>()
            .With(x => x.AnswerParts, testInputAnswerParts)
            .Create();

        Assert.That(() => testItems.AcquirerDataShareRequestModelDataFactory.CreateQuestionAnswerWriteData(testDataShareRequestQuestionAnswer),
            Throws.TypeOf<InvalidEnumValueException>().With.Message.EqualTo("QuestionAnswerPartResponse has unknown InputType"));
    }
    #endregion

    #region CreateDataShareRequestQuestionFromAnswer() Tests
    [Test]
    public void GivenANullDataShareRequestQuestion_WhenICreateDataShareRequestQuestionFromAnswer_ThenAnArgumentNullExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        Assert.That(() => testItems.AcquirerDataShareRequestModelDataFactory.CreateDataShareRequestQuestionFromAnswer(
                null!,
                testItems.Fixture.Create<DataShareRequestQuestionAnswer>(),
                testItems.Fixture.CreateMany<SetDataShareRequestQuestionAnswerPartResponseValidationError>()),
            Throws.ArgumentNullException.With.Property("ParamName").EqualTo("dataShareRequestQuestion"));
    }

    [Test]
    public void GivenANullDataShareRequestQuestionAnswer_WhenICreateDataShareRequestQuestionFromAnswer_ThenAnArgumentNullExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        Assert.That(() => testItems.AcquirerDataShareRequestModelDataFactory.CreateDataShareRequestQuestionFromAnswer(
                testItems.Fixture.Create<DataShareRequestQuestion>(),
                null!,
                testItems.Fixture.CreateMany<SetDataShareRequestQuestionAnswerPartResponseValidationError>()),
            Throws.ArgumentNullException.With.Property("ParamName").EqualTo("dataShareRequestQuestionAnswer"));
    }

    [Test]
    public void GivenANullSetOfSetDataShareRequestQuestionAnswerPartResponseValidationErrors_WhenICreateDataShareRequestQuestionFromAnswer_ThenAnArgumentNullExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        Assert.That(() => testItems.AcquirerDataShareRequestModelDataFactory.CreateDataShareRequestQuestionFromAnswer(
                testItems.Fixture.Create<DataShareRequestQuestion>(),
                testItems.Fixture.Create<DataShareRequestQuestionAnswer>(),
                null!),
            Throws.ArgumentNullException.With.Property("ParamName").EqualTo("questionAnswerPartResponseValidationErrors"));
    }

    [Test]
    public void GivenADataShareRequestQuestionForAFreeFormQuestion_WhenICreateDataShareRequestQuestionFromAnswer_ThenADataShareRequestQuestionIsCreatedFromTheGivenAnswer()
    {
        var testItems = CreateTestItems();

        var testInputAnswerPartResponses = testItems.Fixture
            .Build<DataShareRequestQuestionAnswerPartResponseFreeForm>()
            .With(x => x.InputType, Dto.Models.Questions.QuestionParts.ResponseFormats.QuestionPartResponseInputType.FreeForm)
            .CreateMany()
            .Select((item, index) =>
            {
                item.OrderWithinAnswerPart = index + 1;
                return item;
            })
            .ToList();

        var testInputQuestionPartAnswerResponses = testItems.Fixture
            .Build<QuestionPartAnswerResponse>()
            .CreateMany(testInputAnswerPartResponses.Count)
            .Select((item, index) =>
            {
                item.OrderWithinAnswerPart = index + 1;
                item.InputType = Dto.Models.Questions.QuestionParts.ResponseFormats.QuestionPartResponseInputType.FreeForm;

                return item;
            })
            .ToList();

        var testInputQuestionParts = testItems.Fixture
            .Build<DataShareRequestQuestionPart>()
            .CreateMany()
            .Select((item, index) =>
            {
                item.QuestionPartQuestion.QuestionPartOrderWithinQuestion = index + 1;

                item.QuestionPartAnswer!.QuestionPartId = item.QuestionPartQuestion.Id;
                item.QuestionPartAnswer.AnswerPartResponses = testInputQuestionPartAnswerResponses;
                return item;
            })
            .ToList();

        var testInputDataShareRequestQuestion = testItems.Fixture
            .Build<DataShareRequestQuestion>()
            .With(x => x.QuestionParts, testInputQuestionParts)
            .Create();
        
        var testInputAnswerPartBaseResponses = new List<DataShareRequestQuestionAnswerPartResponseBase>();
        testInputAnswerPartBaseResponses.AddRange(testInputAnswerPartResponses);

        var testInputAnswerParts = testItems.Fixture
            .Build<DataShareRequestQuestionAnswerPart>()
            .With(x => x.AnswerPartResponses, testInputAnswerPartBaseResponses)
            .CreateMany(testInputDataShareRequestQuestion.QuestionParts.Count)
            .Select((item, index) =>
            {
                var testQuestionPartQuestion = testInputDataShareRequestQuestion.QuestionParts[index].QuestionPartQuestion;
                item.QuestionPartId = testQuestionPartQuestion.Id;
                return item;
            })
            .ToList();

        var testInputDataShareRequestQuestionAnswer = testItems.Fixture
            .Build<DataShareRequestQuestionAnswer>()
            .With(x => x.AnswerParts, testInputAnswerParts)
            .Create();

        var testInputsSetDataShareRequestQuestionAnswerPartResponseValidationErrors = testItems.Fixture
            .Build<SetDataShareRequestQuestionAnswerPartResponseValidationError>()
            .CreateMany(testInputDataShareRequestQuestion.QuestionParts.Count)
            .Select((item, index) =>
            {
                var testQuestionPartQuestion = testInputDataShareRequestQuestion.QuestionParts[index].QuestionPartQuestion;
                item.QuestionPartId = testQuestionPartQuestion.Id;
                item.ResponseOrderWithinAnswerPart = testQuestionPartQuestion.QuestionPartOrderWithinQuestion;
                return item;
            })
            .ToList();

        var result = testItems.AcquirerDataShareRequestModelDataFactory.CreateDataShareRequestQuestionFromAnswer(
            testInputDataShareRequestQuestion, testInputDataShareRequestQuestionAnswer, testInputsSetDataShareRequestQuestionAnswerPartResponseValidationErrors);

        Assert.Multiple(() =>
        {
            Assert.That(result.DataShareRequestId, Is.EqualTo(testInputDataShareRequestQuestion.DataShareRequestId));
            Assert.That(result.DataShareRequestRequestId, Is.EqualTo(testInputDataShareRequestQuestion.DataShareRequestRequestId));
            Assert.That(result.QuestionId, Is.EqualTo(testInputDataShareRequestQuestion.QuestionId));
            Assert.That(result.IsOptional, Is.EqualTo(testInputDataShareRequestQuestion.IsOptional));
            Assert.That(result.QuestionFooter, Is.EqualTo(testInputDataShareRequestQuestion.QuestionFooter));

            Assert.That(result.QuestionParts, Has.Exactly(testInputDataShareRequestQuestion.QuestionParts.Count).Items);

            foreach (var testQuestionPart in testInputDataShareRequestQuestion.QuestionParts)
            {
                var resultQuestionPart = result.QuestionParts.FirstOrDefault(x =>
                    x.QuestionPartQuestion.Id == testQuestionPart.QuestionPartQuestion.Id);

                Assert.That(resultQuestionPart, Is.Not.Null);

                foreach (var testResponse in testQuestionPart.QuestionPartAnswer!.AnswerPartResponses)
                {
                    var resultResponse = resultQuestionPart!.QuestionPartAnswer!.AnswerPartResponses.FirstOrDefault(x => 
                        x.OrderWithinAnswerPart == testResponse.OrderWithinAnswerPart);

                    Assert.That(resultResponse, Is.Not.Null);
                }
            }
        });
    }

    [Test]
    public void GivenADataShareRequestQuestionForAnOptionSelectionQuestion_WhenICreateDataShareRequestQuestionFromAnswer_ThenADataShareRequestQuestionIsCreatedFromTheGivenAnswer()
    {
        var testItems = CreateTestItems();
        
        var testInputSupplementaryAnswerPartResponse = testItems.Fixture
            .Build<DataShareRequestQuestionAnswerPartResponseFreeForm>()
            .With(x => x.OrderWithinAnswerPart, 1)
            .With(x => x.InputType, Dto.Models.Questions.QuestionParts.ResponseFormats.QuestionPartResponseInputType.FreeForm)
            .Create();

        var testInputSupplementaryAnswerPart = testItems.Fixture
            .Build<DataShareRequestQuestionAnswerPart>()
            .With(x => x.AnswerPartResponses, [testInputSupplementaryAnswerPartResponse])
            .Create();

        var testInputSelectionOptionItems = testItems.Fixture
            .Build<DataShareRequestQuestionAnswerPartResponseSelectionOptionItem>()
            .Without(x => x.SupplementaryQuestionAnswerPart)
            .CreateMany(3)
            .Select((item, index) =>
            {
                if (index == 0)
                {
                    item.SupplementaryQuestionAnswerPart = testInputSupplementaryAnswerPart;
                }

                return item;
            })
            .ToList();

        var testInputAnswerPartResponses = testItems.Fixture
            .Build<DataShareRequestQuestionAnswerPartResponseSelectionOption>()
            .With(x => x.InputType, Dto.Models.Questions.QuestionParts.ResponseFormats.QuestionPartResponseInputType.OptionSelection)
            .CreateMany()
            .Select((item, index) =>
            {
                item.OrderWithinAnswerPart = index + 1;
                item.SelectedOptionItems = testInputSelectionOptionItems;
                return item;
            })
            .ToList();

        var testInputQuestionPartAnswerResponses = testItems.Fixture
            .Build<QuestionPartAnswerResponse>()
            .CreateMany(testInputAnswerPartResponses.Count)
            .Select((item, index) =>
            {
                item.OrderWithinAnswerPart = index + 1;
                item.InputType = Dto.Models.Questions.QuestionParts.ResponseFormats.QuestionPartResponseInputType.OptionSelection;

                return item;
            })
            .ToList();

        var testInputQuestionParts = testItems.Fixture
            .Build<DataShareRequestQuestionPart>()
            .CreateMany()
            .Select((item, index) =>
            {
                item.QuestionPartQuestion.QuestionPartOrderWithinQuestion = index + 1;

                item.QuestionPartAnswer!.QuestionPartId = item.QuestionPartQuestion.Id;
                item.QuestionPartAnswer.AnswerPartResponses = testInputQuestionPartAnswerResponses;
                return item;
            })
            .ToList();

        var testInputDataShareRequestQuestion = testItems.Fixture
            .Build<DataShareRequestQuestion>()
            .With(x => x.QuestionParts, testInputQuestionParts)
            .Create();

        var testInputAnswerPartBaseResponses = new List<DataShareRequestQuestionAnswerPartResponseBase>();
        testInputAnswerPartBaseResponses.AddRange(testInputAnswerPartResponses);

        var testInputAnswerParts = testItems.Fixture
            .Build<DataShareRequestQuestionAnswerPart>()
            .With(x => x.AnswerPartResponses, testInputAnswerPartBaseResponses)
            .CreateMany(testInputDataShareRequestQuestion.QuestionParts.Count)
            .Select((item, index) =>
            {
                var testQuestionPartQuestion = testInputDataShareRequestQuestion.QuestionParts[index].QuestionPartQuestion;
                item.QuestionPartId = testQuestionPartQuestion.Id;
                return item;
            })
            .ToList();

        var testInputDataShareRequestQuestionAnswer = testItems.Fixture
            .Build<DataShareRequestQuestionAnswer>()
            .With(x => x.AnswerParts, testInputAnswerParts)
            .Create();

        var testInputsSetDataShareRequestQuestionAnswerPartResponseValidationErrors = testItems.Fixture
            .Build<SetDataShareRequestQuestionAnswerPartResponseValidationError>()
            .CreateMany(testInputDataShareRequestQuestion.QuestionParts.Count)
            .Select((item, index) =>
            {
                var testQuestionPartQuestion = testInputDataShareRequestQuestion.QuestionParts[index].QuestionPartQuestion;
                item.QuestionPartId = testQuestionPartQuestion.Id;
                item.ResponseOrderWithinAnswerPart = testQuestionPartQuestion.QuestionPartOrderWithinQuestion;
                return item;
            })
            .ToList();

        var result = testItems.AcquirerDataShareRequestModelDataFactory.CreateDataShareRequestQuestionFromAnswer(
            testInputDataShareRequestQuestion, testInputDataShareRequestQuestionAnswer, testInputsSetDataShareRequestQuestionAnswerPartResponseValidationErrors);

        Assert.Multiple(() =>
        {
            Assert.That(result.DataShareRequestId, Is.EqualTo(testInputDataShareRequestQuestion.DataShareRequestId));
            Assert.That(result.DataShareRequestRequestId, Is.EqualTo(testInputDataShareRequestQuestion.DataShareRequestRequestId));
            Assert.That(result.QuestionId, Is.EqualTo(testInputDataShareRequestQuestion.QuestionId));
            Assert.That(result.IsOptional, Is.EqualTo(testInputDataShareRequestQuestion.IsOptional));
            Assert.That(result.QuestionFooter, Is.EqualTo(testInputDataShareRequestQuestion.QuestionFooter));

            Assert.That(result.QuestionParts, Has.Exactly(testInputDataShareRequestQuestion.QuestionParts.Count).Items);

            foreach (var testQuestionPart in testInputDataShareRequestQuestion.QuestionParts)
            {
                var resultQuestionPart = result.QuestionParts.FirstOrDefault(x =>
                    x.QuestionPartQuestion.Id == testQuestionPart.QuestionPartQuestion.Id);

                Assert.That(resultQuestionPart, Is.Not.Null);

                foreach (var testResponse in testQuestionPart.QuestionPartAnswer!.AnswerPartResponses)
                {
                    var resultResponse = resultQuestionPart!.QuestionPartAnswer!.AnswerPartResponses.FirstOrDefault(x =>
                        x.OrderWithinAnswerPart == testResponse.OrderWithinAnswerPart);

                    Assert.That(resultResponse, Is.Not.Null);
                }
            }
        });
    }

    [Test]
    public void GivenADataShareRequestQuestionForAQuestionOfUnknownInputType_WhenICreateDataShareRequestQuestionFromAnswer_ThenAnInvalidEnumValueExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        var invalidInputType = (Dto.Models.Questions.QuestionParts.ResponseFormats.QuestionPartResponseInputType)
            Enum.GetValues<Dto.Models.Questions.QuestionParts.ResponseFormats.QuestionPartResponseInputType>().Cast<int>().Max() + 1;

        var testInputAnswerPartResponses = testItems.Fixture
            .Build<DataShareRequestQuestionAnswerPartResponseSelectionOption>()
            .With(x => x.InputType, invalidInputType)
            .CreateMany()
            .Select((item, index) =>
            {
                item.OrderWithinAnswerPart = index + 1;
                item.SelectedOptionItems = [];
                return item;
            })
            .ToList();
        
        var testInputQuestionPartAnswerResponses = testItems.Fixture
            .Build<QuestionPartAnswerResponse>()
            .CreateMany(testInputAnswerPartResponses.Count)
            .Select((item, index) =>
            {
                item.OrderWithinAnswerPart = index + 1;
                item.InputType = invalidInputType;

                return item;
            })
            .ToList();

        var testInputQuestionParts = testItems.Fixture
            .Build<DataShareRequestQuestionPart>()
            .CreateMany()
            .Select((item, index) =>
            {
                item.QuestionPartQuestion.QuestionPartOrderWithinQuestion = index + 1;

                item.QuestionPartAnswer!.QuestionPartId = item.QuestionPartQuestion.Id;
                item.QuestionPartAnswer.AnswerPartResponses = testInputQuestionPartAnswerResponses;
                return item;
            })
            .ToList();

        var testInputDataShareRequestQuestion = testItems.Fixture
            .Build<DataShareRequestQuestion>()
            .With(x => x.QuestionParts, testInputQuestionParts)
            .Create();

        var testInputAnswerPartBaseResponses = new List<DataShareRequestQuestionAnswerPartResponseBase>();
        testInputAnswerPartBaseResponses.AddRange(testInputAnswerPartResponses);

        var testInputAnswerParts = testItems.Fixture
            .Build<DataShareRequestQuestionAnswerPart>()
            .With(x => x.AnswerPartResponses, testInputAnswerPartBaseResponses)
            .CreateMany(testInputDataShareRequestQuestion.QuestionParts.Count)
            .Select((item, index) =>
            {
                var testQuestionPartQuestion = testInputDataShareRequestQuestion.QuestionParts[index].QuestionPartQuestion;
                item.QuestionPartId = testQuestionPartQuestion.Id;
                return item;
            })
            .ToList();

        var testInputDataShareRequestQuestionAnswer = testItems.Fixture
            .Build<DataShareRequestQuestionAnswer>()
            .With(x => x.AnswerParts, testInputAnswerParts)
            .Create();

        var testInputsSetDataShareRequestQuestionAnswerPartResponseValidationErrors = testItems.Fixture
            .Build<SetDataShareRequestQuestionAnswerPartResponseValidationError>()
            .CreateMany(testInputDataShareRequestQuestion.QuestionParts.Count)
            .Select((item, index) =>
            {
                var testQuestionPartQuestion = testInputDataShareRequestQuestion.QuestionParts[index].QuestionPartQuestion;
                item.QuestionPartId = testQuestionPartQuestion.Id;
                item.ResponseOrderWithinAnswerPart = testQuestionPartQuestion.QuestionPartOrderWithinQuestion;
                return item;
            })
            .ToList();

        Assert.That(() => testItems.AcquirerDataShareRequestModelDataFactory.CreateDataShareRequestQuestionFromAnswer(
            testInputDataShareRequestQuestion, testInputDataShareRequestQuestionAnswer, testInputsSetDataShareRequestQuestionAnswerPartResponseValidationErrors),
                Throws.TypeOf<InvalidEnumValueException>().With.Message.EqualTo("Response received for unexpected input type"));
    }
    #endregion

    #region Helpers
    private static DataShareRequestStatus GetExpectedDataShareRequestStatus(DataShareRequestStatusType inputRequestStatus)
    {
        return inputRequestStatus switch
        {
            DataShareRequestStatusType.Draft => DataShareRequestStatus.Draft,
            DataShareRequestStatusType.Submitted => DataShareRequestStatus.Submitted,
            DataShareRequestStatusType.Accepted => DataShareRequestStatus.Accepted,
            DataShareRequestStatusType.Rejected => DataShareRequestStatus.Rejected,
            DataShareRequestStatusType.Cancelled => DataShareRequestStatus.Cancelled,
            DataShareRequestStatusType.Returned => DataShareRequestStatus.Returned,
            DataShareRequestStatusType.InReview => DataShareRequestStatus.InReview,
            DataShareRequestStatusType.Deleted => DataShareRequestStatus.Deleted,
            _ => throw new Exception()
        };
    }

    private static QuestionStatus GetExpectedQuestionStatus(QuestionStatusType inputQuestionStatus)
    {
        return inputQuestionStatus switch
        {
            QuestionStatusType.NotStarted => QuestionStatus.NotStarted,
            QuestionStatusType.CannotStartYet => QuestionStatus.CannotStartYet,
            QuestionStatusType.Completed => QuestionStatus.Completed,
            QuestionStatusType.NotApplicable => QuestionStatus.NotApplicable,
            QuestionStatusType.NoResponseNeeded => QuestionStatus.NoResponseNeeded,
            _ => throw new Exception()
        };
    }

    private static NotificationSuccess GetExpectedNotificationSuccess(
        bool? notificationsSuccess)
    {
        return notificationsSuccess switch
        {
            true => NotificationSuccess.SentSuccessfully,
            false => NotificationSuccess.FailedToSend,
            null => NotificationSuccess.NotSent
        };
    }

    private static Dto.Models.Questions.QuestionParts.ResponseFormats.QuestionPartResponseInputType GetExpectedResponseInputType(
        QuestionPartResponseInputType responseInputType)
    {
        return responseInputType switch
        {
            QuestionPartResponseInputType.FreeForm => Dto.Models.Questions.QuestionParts.ResponseFormats.QuestionPartResponseInputType.FreeForm,
            QuestionPartResponseInputType.OptionSelection => Dto.Models.Questions.QuestionParts.ResponseFormats.QuestionPartResponseInputType.OptionSelection,
            QuestionPartResponseInputType.None => Dto.Models.Questions.QuestionParts.ResponseFormats.QuestionPartResponseInputType.None,
            _ => throw new Exception()
        };
    }

    private static Dto.Models.Questions.QuestionParts.ResponseFormats.QuestionPartResponseFormatType GetExpectedResponseFormatType(
        QuestionPartResponseFormatType responseFormatType)
    {
        return responseFormatType switch
        {
            QuestionPartResponseFormatType.Text => Dto.Models.Questions.QuestionParts.ResponseFormats.QuestionPartResponseFormatType.Text,
            QuestionPartResponseFormatType.Numeric => Dto.Models.Questions.QuestionParts.ResponseFormats.QuestionPartResponseFormatType.Numeric,
            QuestionPartResponseFormatType.Date => Dto.Models.Questions.QuestionParts.ResponseFormats.QuestionPartResponseFormatType.Date,
            QuestionPartResponseFormatType.Time => Dto.Models.Questions.QuestionParts.ResponseFormats.QuestionPartResponseFormatType.Time,
            QuestionPartResponseFormatType.DateTime => Dto.Models.Questions.QuestionParts.ResponseFormats.QuestionPartResponseFormatType.DateTime,
            QuestionPartResponseFormatType.SelectSingle => Dto.Models.Questions.QuestionParts.ResponseFormats.QuestionPartResponseFormatType.SelectSingle,
            QuestionPartResponseFormatType.SelectMulti => Dto.Models.Questions.QuestionParts.ResponseFormats.QuestionPartResponseFormatType.SelectMulti,
            QuestionPartResponseFormatType.ReadOnly => Dto.Models.Questions.QuestionParts.ResponseFormats.QuestionPartResponseFormatType.ReadOnly,
            QuestionPartResponseFormatType.Country => Dto.Models.Questions.QuestionParts.ResponseFormats.QuestionPartResponseFormatType.Country,
            _ => throw new Exception()
        };
    }
    #endregion

    #region Test Data Creation
    #region Request Statuses
    private static readonly IEnumerable<DataShareRequestStatusType> AllDataShareRequestStatuses = Enum.GetValues<DataShareRequestStatusType>();

    private static readonly IEnumerable<DataShareRequestStatusType> InvalidInputDataShareRequestStatuses = [DataShareRequestStatusType.None];

    private static readonly IEnumerable<DataShareRequestStatusType> ValidInputDataShareRequestStatuses = AllDataShareRequestStatuses.Except(InvalidInputDataShareRequestStatuses);
    #endregion

    #region Question Statuses
    private static readonly IEnumerable<QuestionStatusType> AllQuestionStatuses = Enum.GetValues<QuestionStatusType>();

    private static readonly IEnumerable<QuestionStatusType> InvalidQuestionStatuses = [QuestionStatusType.NotSet];

    private static readonly IEnumerable<QuestionStatusType> ValidInputQuestionStatuses = AllQuestionStatuses.Except(InvalidQuestionStatuses);
    #endregion
    #endregion

    #region Test Item Creation
    private static TestItems CreateTestItems()
    {
        var fixture = new Fixture().Customize(new AutoMoqCustomization());

        var acquirerDataShareRequestModelDataFactory = new AcquirerDataShareRequestModelDataFactory();

        return new TestItems(
            fixture,
            acquirerDataShareRequestModelDataFactory);
    }

    private class TestItems(
        IFixture fixture,
        IAcquirerDataShareRequestModelDataFactory acquirerDataShareRequestModelDataFactory)
    {
        public IFixture Fixture { get; } = fixture;
        public IAcquirerDataShareRequestModelDataFactory AcquirerDataShareRequestModelDataFactory { get; } = acquirerDataShareRequestModelDataFactory;
    }
    #endregion
}
