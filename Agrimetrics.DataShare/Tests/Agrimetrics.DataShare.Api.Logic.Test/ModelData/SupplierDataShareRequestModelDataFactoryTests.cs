using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests;
using Agrimetrics.DataShare.Api.Dto.Models.Supplier.DataShareRequests;
using Agrimetrics.DataShare.Api.Dto.Models.Supplier.DataShareRequests.Decisions;
using Agrimetrics.DataShare.Api.Dto.Responses.Notifications;
using Agrimetrics.DataShare.Api.Logic.Exceptions;
using Agrimetrics.DataShare.Api.Logic.ModelData;
using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests;
using Agrimetrics.DataShare.Api.Logic.ModelData.Questions.QuestionParts.ResponseFormats;
using Agrimetrics.DataShare.Api.Logic.ModelData.Submissions;
using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.ModelData;

[TestFixture]
public class SupplierDataShareRequestModelDataFactoryTests
{
    #region CreateSubmissionSummarySet() Tests
    [Test]
    public void GivenANullSetOfPendingSubmissionSummaryModelDatas_WhenICreateSubmissionSummarySet_ThenAnArgumentNullExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        Assert.That(() => testItems.SupplierDataShareRequestModelDataFactory.CreateSubmissionSummarySet(
                null!,
                testItems.Fixture.CreateMany<CompletedSubmissionSummaryModelData>()),
            Throws.ArgumentNullException.With.Property("ParamName").EqualTo("pendingSubmissionSummaryModelDatas"));
    }

    [Test]
    public void GivenANullSetOfCompletedSubmissionSummaryModelDatas_WhenICreateSubmissionSummarySet_ThenAnArgumentNullExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        Assert.That(() => testItems.SupplierDataShareRequestModelDataFactory.CreateSubmissionSummarySet(
                testItems.Fixture.CreateMany<PendingSubmissionSummaryModelData>(),
                null!),
            Throws.ArgumentNullException.With.Property("ParamName").EqualTo("completedSubmissionSummaryModelDatas"));
    }

    [Test]
    public void GivenPendingSubmissionSummaries_WhenICreateSubmissionSummarySet_ThenASubmissionSummariesSetIsCreatedFromTheGivenData(
        [ValueSource(nameof(ValidInputDataShareRequestStatuses))] DataShareRequestStatusType testDataShareRequestStatus)
    {
        var testItems = CreateTestItems();

        var testPendingSubmissionSummaries = testItems.Fixture
            .Build<PendingSubmissionSummaryModelData>()
            .With(x => x.PendingSubmissionSummary_RequestStatus, testDataShareRequestStatus)
            .CreateMany()
            .ToList();

        var result = testItems.SupplierDataShareRequestModelDataFactory.CreateSubmissionSummarySet(
            testPendingSubmissionSummaries, []);

        Assert.Multiple(() =>
        {
            var resultPendingSubmissionSummaries = result.PendingSubmissionSummaries;
            Assert.That(result.PendingSubmissionSummaries, Has.Exactly(testPendingSubmissionSummaries.Count).Items);

            foreach (var testPendingSubmissionSummary in testPendingSubmissionSummaries)
            {
                var resultPendingSubmissionSummary = resultPendingSubmissionSummaries.FirstOrDefault(x =>
                    x.DataShareRequestId == testPendingSubmissionSummary.PendingSubmissionSummary_DataShareRequestId);

                Assert.That(resultPendingSubmissionSummary!.DataShareRequestRequestId, Is.EqualTo(testPendingSubmissionSummary.PendingSubmissionSummary_DataShareRequestRequestId));
                Assert.That(resultPendingSubmissionSummary.AcquirerOrganisationName, Is.EqualTo(testPendingSubmissionSummary.PendingSubmissionSummary_AcquirerOrganisationName));
                Assert.That(resultPendingSubmissionSummary.EsdaName, Is.EqualTo(testPendingSubmissionSummary.PendingSubmissionSummary_EsdaName));
                Assert.That(resultPendingSubmissionSummary.SubmittedOn, Is.EqualTo(testPendingSubmissionSummary.PendingSubmissionSummary_SubmittedOn));
                Assert.That(resultPendingSubmissionSummary.WhenNeededBy, Is.EqualTo(testPendingSubmissionSummary.PendingSubmissionSummary_WhenNeededBy));
                Assert.That(resultPendingSubmissionSummary.RequestStatus, Is.EqualTo(GetExpectedDataShareRequestStatus(testPendingSubmissionSummary.PendingSubmissionSummary_RequestStatus)));
            }
        });
    }

    [Test]
    public void GivenCompletedSubmissionSummaries_WhenICreateSubmissionSummarySet_ThenASubmissionSummariesSetIsCreatedFromTheGivenData(
        [ValueSource(nameof(ValidSubmissionDecisions))] SubmissionDecisionType testSubmissionDecision)
    {
        var testItems = CreateTestItems();

        var testCompletedSubmissionSummaries = testItems.Fixture
            .Build<CompletedSubmissionSummaryModelData>()
            .With(x => x.CompletedSubmissionSummary_Decision, testSubmissionDecision)
            .CreateMany()
            .ToList();

        var result = testItems.SupplierDataShareRequestModelDataFactory.CreateSubmissionSummarySet(
            [], testCompletedSubmissionSummaries);

        Assert.Multiple(() =>
        {
            var resultCompletedSubmissionSummaries = result.CompletedSubmissionSummaries;
            Assert.That(result.CompletedSubmissionSummaries, Has.Exactly(testCompletedSubmissionSummaries.Count).Items);

            foreach (var testCompletedSubmissionSummary in testCompletedSubmissionSummaries)
            {
                var resultPendingSubmissionSummary = resultCompletedSubmissionSummaries.FirstOrDefault(x =>
                    x.DataShareRequestId == testCompletedSubmissionSummary.CompletedSubmissionSummary_DataShareRequestId);

                Assert.That(resultPendingSubmissionSummary!.DataShareRequestRequestId, Is.EqualTo(testCompletedSubmissionSummary.CompletedSubmissionSummary_DataShareRequestRequestId));
                Assert.That(resultPendingSubmissionSummary.AcquirerOrganisationName, Is.EqualTo(testCompletedSubmissionSummary.CompletedSubmissionSummary_AcquirerOrganisationName));
                Assert.That(resultPendingSubmissionSummary.EsdaName, Is.EqualTo(testCompletedSubmissionSummary.CompletedSubmissionSummary_EsdaName));
                Assert.That(resultPendingSubmissionSummary.SubmittedOn, Is.EqualTo(testCompletedSubmissionSummary.CompletedSubmissionSummary_SubmittedOn));
                Assert.That(resultPendingSubmissionSummary.CompletedOn, Is.EqualTo(testCompletedSubmissionSummary.CompletedSubmissionSummary_CompletedOn));
                Assert.That(resultPendingSubmissionSummary.Decision, Is.EqualTo(GetExpectedSubmissionDecision(testCompletedSubmissionSummary.CompletedSubmissionSummary_Decision)));
            }
        });
    }

    [Test]
    public void GivenCompletedSubmissionSummariesWithInvalidSubmissionDecision_WhenICreateSubmissionSummarySet_ThenAnInvalidEnumValueExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        var invalidSubmissionDecision = (SubmissionDecisionType) Enum.GetValues<SubmissionDecisionType>().Cast<int>().Max() + 1;

        var testCompletedSubmissionSummaries = testItems.Fixture
            .Build<CompletedSubmissionSummaryModelData>()
            .With(x => x.CompletedSubmissionSummary_Decision, invalidSubmissionDecision)
            .CreateMany()
            .ToList();

        Assert.That(() => testItems.SupplierDataShareRequestModelDataFactory.CreateSubmissionSummarySet([], testCompletedSubmissionSummaries),
                Throws.TypeOf<InvalidEnumValueException>().With.Message.EqualTo("Submission has unknown Decision"));
    }
    #endregion

    #region CreateSubmissionInformation() Tests
    [Test]
    public void GivenSubmissionInformationModelData_WhenICreateSubmissionInformation_ThenSubmissionInformationIsCreatedFromTheGivenData(
        [ValueSource(nameof(ValidInputDataShareRequestStatuses))] DataShareRequestStatusType testDataShareRequestStatus)
    {
        var testItems = CreateTestItems();

        var testSubmissionInformation = testItems.Fixture
            .Build<SubmissionInformationModelData>()
            .With(x => x.SubmissionInformation_RequestStatus, testDataShareRequestStatus)
            .Create();

        var result = testItems.SupplierDataShareRequestModelDataFactory.CreateSubmissionInformation(
            testSubmissionInformation);

        Assert.Multiple(() =>
        {
            Assert.That(result.DataShareRequestId, Is.EqualTo(testSubmissionInformation.SubmissionInformation_DataShareRequestId));
            Assert.That(result.DataShareRequestRequestId, Is.EqualTo(testSubmissionInformation.SubmissionInformation_DataShareRequestRequestId));
            Assert.That(result.RequestStatus, Is.EqualTo(GetExpectedDataShareRequestStatus(testSubmissionInformation.SubmissionInformation_RequestStatus)));
            Assert.That(result.EsdaName, Is.EqualTo(testSubmissionInformation.SubmissionInformation_EsdaName));
            Assert.That(result.AcquirerOrganisationName, Is.EqualTo(testSubmissionInformation.SubmissionInformation_AcquirerOrganisationName));
            Assert.That(result.DataTypes, Is.EqualTo(testSubmissionInformation.SubmissionInformation_DataTypes));
            Assert.That(result.ProjectAims, Is.EqualTo(testSubmissionInformation.SubmissionInformation_ProjectAims));
            Assert.That(result.WhenNeededBy, Is.EqualTo(testSubmissionInformation.SubmissionInformation_WhenNeededBy));
            Assert.That(result.SubmittedOn, Is.EqualTo(testSubmissionInformation.SubmissionInformation_SubmittedOn));
            Assert.That(result.AcquirerEmailAddress, Is.EqualTo(testSubmissionInformation.SubmissionInformation_AcquirerEmailAddress));
            Assert.That(result.AnswerHighlights, Is.EqualTo(testSubmissionInformation.SubmissionInformation_AnswerHighlights));
        });
    }
    #endregion

    #region CreateSubmissionDetails() Tests
    [Test]
    public void GivenANullSubmissionDetailsModelData_WhenICreateSubmissionDetails_ThenAnArgumentNullExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        Assert.That(() => testItems.SupplierDataShareRequestModelDataFactory.CreateSubmissionDetails(null!),
            Throws.ArgumentNullException.With.Property("ParamName").EqualTo("submissionDetailsModelData"));
    }

    [Test]
    [TestCaseSource(nameof(CreateSubmissionDetailsTestCaseData))]
    public void GivenSubmissionDetailsModelData_WhenICreateSubmissionDetails_ThenASubmissionDetailsIsCreatedUsingTheGivenModelData(
        DataShareRequestStatusType testDataShareRequestStatus,
        QuestionPartResponseInputType testResponseInputType,
        QuestionPartResponseFormatType testResponseFormatType)
    {
        var testItems = CreateTestItems();

        var testInputQuestionResponseItems = testItems.Fixture
            .Build<SubmissionDetailsAnswerPartResponseItemModelData>()
            .CreateMany(1) // It is a list, but should contain only one item - see source for details
            .ToList();

        var testInputQuestionResponses = testItems.Fixture
            .Build<SubmissionDetailsAnswerPartResponseModelData>()
            .With(x => x.SubmissionDetailsAnswerPartResponse_ResponseItems, testInputQuestionResponseItems)
            .CreateMany(1)
            .ToList();

        var testInputQuestionAnswerParts = testItems.Fixture
            .Build<SubmissionDetailsAnswerPartModelData>()
            .With(x => x.SubmissionDetailsAnswerPart_InputType, testResponseInputType)
            .With(x => x.SubmissionDetailsAnswerPart_FormatType, testResponseFormatType)
            .With(x => x.SubmissionDetailsAnswerPart_Responses, testInputQuestionResponses)
            .CreateMany()
            .ToList();

        var testInputBackingQuestions = testItems.Fixture
            .Build<SubmissionDetailsBackingQuestionModelData>()
            .With(x => x.SubmissionDetailsBackingQuestion_AnswerParts, testInputQuestionAnswerParts)
            .CreateMany()
            .ToList();

        var testInputMainQuestions = testItems.Fixture
            .Build<SubmissionDetailsMainQuestionModelData>()
            .With(x => x.SubmissionDetailsMainQuestion_AnswerParts, testInputQuestionAnswerParts)
            .With(x => x.SubmissionDetailsMainQuestion_BackingQuestions, testInputBackingQuestions)
            .CreateMany()
            .ToList();

        var testInputSubmissionDetailsSections = testItems.Fixture
            .Build<SubmissionDetailsSectionModelData>()
            .With(x => x.SubmissionDetailsSection_Questions, testInputMainQuestions)
            .CreateMany()
            .ToList();

        var testSubmissionInformation = testItems.Fixture
            .Build<SubmissionDetailsModelData>()
            .With(x => x.SubmissionDetails_RequestStatus, testDataShareRequestStatus)
            .With(x => x.SubmissionDetails_Sections, testInputSubmissionDetailsSections)
            .Create();

        var result = testItems.SupplierDataShareRequestModelDataFactory.CreateSubmissionDetails(
            testSubmissionInformation);

        Assert.Multiple(() =>
        {
            Assert.That(result.DataShareRequestId, Is.EqualTo(testSubmissionInformation.SubmissionDetails_DataShareRequestId));
            Assert.That(result.DataShareRequestRequestId, Is.EqualTo(testSubmissionInformation.SubmissionDetails_DataShareRequestRequestId));
            Assert.That(result.RequestStatus, Is.EqualTo(GetExpectedDataShareRequestStatus(testSubmissionInformation.SubmissionDetails_RequestStatus)));
            Assert.That(result.EsdaName, Is.EqualTo(testSubmissionInformation.SubmissionDetails_EsdaName));
            Assert.That(result.AcquirerOrganisationName, Is.EqualTo(testSubmissionInformation.SubmissionDetails_AcquirerOrganisationName));

            var resultSubmissionReturns = result.SubmissionReturnDetailsSet.SubmissionReturns;
            var testReturnComments = testSubmissionInformation.SubmissionDetails_SubmissionReturnComments;
            
            Assert.That(resultSubmissionReturns, Has.Exactly(testReturnComments.Count).Items);
            foreach (var testReturnComment in testReturnComments)
            {
                var resultSubmissionReturn = resultSubmissionReturns.FirstOrDefault(
                    x => x.ReturnComments == testReturnComment.Comments);

                Assert.That(resultSubmissionReturn!.ReturnedOnUtc, Is.EqualTo(testReturnComment.ReturnedOnUtc));
            }

            var resultSections = result.Sections;
            var testSections = testSubmissionInformation.SubmissionDetails_Sections;
            Assert.That(resultSections, Has.Exactly(testSections.Count).Items);

            foreach (var testSection in testSections)
            {
                var resultSection = resultSections.FirstOrDefault(
                    x => x.SectionNumber == testSection.SubmissionDetailsSection_SectionNumber);

                Assert.That(resultSection!.SectionHeader, Is.EqualTo(testSection.SubmissionDetailsSection_SectionHeader));

                var testQuestions = testSection.SubmissionDetailsSection_Questions;
                var resultAnswerGroups = resultSection.AnswerGroups;

                Assert.That(resultAnswerGroups, Has.Exactly(testQuestions.Count).Items);
                foreach (var testQuestion in testQuestions)
                {
                    var resultAnswerGroup = resultAnswerGroups.FirstOrDefault(x =>
                        x.MainQuestionHeader == testQuestion.SubmissionDetailsMainQuestion_QuestionHeader);

                    Assert.That(resultAnswerGroup!.OrderWithinSubmission, Is.EqualTo(testQuestion.SubmissionDetailsMainQuestion_OrderWithinSection));

                    var testMainQuestionAnswerParts = testQuestion.SubmissionDetailsMainQuestion_AnswerParts;
                    var resultMainQuestionAnswerParts = resultAnswerGroup.MainEntry.EntryParts;

                    Assert.That(resultMainQuestionAnswerParts, Has.Exactly(testMainQuestionAnswerParts.Count).Items);
                    foreach (var testMainQuestionAnswerPart in testMainQuestionAnswerParts)
                    {
                        var resultMainQuestionAnswerPart = resultMainQuestionAnswerParts.FirstOrDefault(x =>
                            x.QuestionPartText == testMainQuestionAnswerPart.SubmissionDetailsAnswerPart_QuestionPartText);

                        Assert.That(resultMainQuestionAnswerPart!.OrderWithinGroupEntry, Is.EqualTo(testMainQuestionAnswerPart.SubmissionDetailsAnswerPart_OrderWithinAnswer));
                        Assert.That(resultMainQuestionAnswerPart.ResponseInputType, Is.EqualTo(GetExpectedResponseInputType(testMainQuestionAnswerPart.SubmissionDetailsAnswerPart_InputType)));
                        Assert.That(resultMainQuestionAnswerPart.ResponseFormatType, Is.EqualTo(GetExpectedResponseFormatType(testMainQuestionAnswerPart.SubmissionDetailsAnswerPart_FormatType)));
                        Assert.That(resultMainQuestionAnswerPart.MultipleResponsesAllowed, Is.EqualTo(testMainQuestionAnswerPart.SubmissionDetailsAnswerPart_MultipleResponsesAllowed));
                        Assert.That(resultMainQuestionAnswerPart.CollectionDescriptionIfMultipleResponsesAllowed, Is.EqualTo(testMainQuestionAnswerPart.SubmissionDetailsAnswerPart_CollectionDescriptionIfMultipleResponsesAllowed));

                        var testResponses = testMainQuestionAnswerPart.SubmissionDetailsAnswerPart_Responses;
                        var resultResponses = resultMainQuestionAnswerPart.Responses;
                        Assert.That(resultResponses, Has.Exactly(testResponses.Count).Items);

                        foreach (var testResponse in testResponses)
                        {
                            var resultResponse = resultResponses.FirstOrDefault(x => 
                                x.OrderWithinAnswer == testResponse.SubmissionDetailsAnswerPartResponse_OrderWithinAnswerPart);

                            var testResponseItem = testResponse.SubmissionDetailsAnswerPartResponse_ResponseItems.Single();
                            var resultResponseItem = resultResponse!.ResponseItem;

                            if (resultMainQuestionAnswerPart.ResponseInputType == Dto.Models.Questions.QuestionParts.ResponseFormats.QuestionPartResponseInputType.FreeForm)
                            {
                                var testFreeFormResponseItem = testResponseItem.SubmissionDetailsAnswerResponseItem_FreeFormData!;
                                var resultFreeFormResultResponseItem = (SubmissionDetailsAnswerResponseItemFreeForm) resultResponseItem;
                                Assert.That(resultFreeFormResultResponseItem.AnswerValue, Is.EqualTo(testFreeFormResponseItem.SubmissionDetailsAnswerPartResponseItemFreeForm_AnswerValue));
                                Assert.That(resultFreeFormResultResponseItem.ValueEntryDeclined, Is.EqualTo(testFreeFormResponseItem.SubmissionDetailsAnswerPartResponseItemFreeForm_ValueEntryDeclined));
                            }
                            else
                            {
                                var testOptionSelectionResponseItem = testResponseItem.SubmissionDetailsAnswerResponseItem_SelectionOptionData!;
                                var resultOptionSelectionResultResponseItem = (SubmissionDetailsAnswerResponseItemOptionSelection) resultResponseItem;

                                var testSelectedOptions = testOptionSelectionResponseItem.SubmissionDetailsAnswerPartResponseItemSelectionOption_SelectedOptions;
                                var resultSelectedOptions = resultOptionSelectionResultResponseItem.SelectedOptions;

                                Assert.That(resultSelectedOptions, Has.Exactly(testSelectedOptions.Count).Items);
                                foreach (var testSelectedOption in testSelectedOptions)
                                {
                                    var resultSelectedOption = resultSelectedOptions.FirstOrDefault(x =>
                                        x.SelectionOptionText == testSelectedOption.SubmissionDetailsAnswerPartResponseItemSelectionOptionSelectedOptionModelData_OptionValueText);

                                    Assert.That(resultSelectedOption!.OrderWithinSelectedOptions, Is.EqualTo(testSelectedOption.SubmissionDetailsAnswerPartResponseItemSelectionOptionSelectedOptionModelData_OrderWithinSelectedOptions));
                                    Assert.That(resultSelectedOption.SupplementaryAnswerText, Is.EqualTo(testSelectedOption.SubmissionDetailsAnswerPartResponseItemSelectionOptionSelectedOptionModelData_SupplementaryAnswerText));
                                }
                            }
                        }
                    }
                }

            }
        });
    }

    private static IEnumerable<TestCaseData> CreateSubmissionDetailsTestCaseData()
    {
        foreach (var dataShareRequestStatus in ValidInputDataShareRequestStatuses)
        {
            yield return new TestCaseData(dataShareRequestStatus, QuestionPartResponseInputType.FreeForm, QuestionPartResponseFormatType.Text);
            yield return new TestCaseData(dataShareRequestStatus, QuestionPartResponseInputType.FreeForm, QuestionPartResponseFormatType.Numeric);
            yield return new TestCaseData(dataShareRequestStatus, QuestionPartResponseInputType.FreeForm, QuestionPartResponseFormatType.Date);
            yield return new TestCaseData(dataShareRequestStatus, QuestionPartResponseInputType.FreeForm, QuestionPartResponseFormatType.Time);
            yield return new TestCaseData(dataShareRequestStatus, QuestionPartResponseInputType.FreeForm, QuestionPartResponseFormatType.DateTime);
            yield return new TestCaseData(dataShareRequestStatus, QuestionPartResponseInputType.FreeForm, QuestionPartResponseFormatType.Country);

            yield return new TestCaseData(dataShareRequestStatus, QuestionPartResponseInputType.OptionSelection, QuestionPartResponseFormatType.SelectSingle);
            yield return new TestCaseData(dataShareRequestStatus, QuestionPartResponseInputType.OptionSelection, QuestionPartResponseFormatType.SelectMulti);
        }
    }

    [Test]
    public void GivenAResponseItemWithInvalidInputType_WhenICreateSubmissionDetails_ThenAnInvalidEnumValueExceptionIsThrown(
        [ValueSource(nameof(InvalidInputDataShareRequestStatuses))] DataShareRequestStatusType dataShareRequestStatusType)
    {
        var testItems = CreateTestItems();
        
        var testSubmissionInformation = testItems.Fixture
            .Build<SubmissionDetailsModelData>()
            .With(x => x.SubmissionDetails_RequestStatus, dataShareRequestStatusType)
            .With(x => x.SubmissionDetails_Sections, [])
            .Create();

        Assert.That(() => testItems.SupplierDataShareRequestModelDataFactory.CreateSubmissionDetails(testSubmissionInformation),
                Throws.TypeOf<InvalidEnumValueException>().With.Message.EqualTo("Invalid DataShareRequestStatusType provided"));
    }

    [Test]
    public void GivenSubmissionDetailsWithInvalidResponseInputType_WhenICreateSubmissionDetails_ThenAnInvalidEnumValueExceptionIsThrown()
    {
        var testItems = CreateTestItems();
        
        var invalidResponseInputType = (QuestionPartResponseInputType) Enum.GetValues<QuestionPartResponseInputType>().Cast<int>().Max() + 1;

        var testInputQuestionAnswerParts = testItems.Fixture
            .Build<SubmissionDetailsAnswerPartModelData>()
            .With(x => x.SubmissionDetailsAnswerPart_InputType, invalidResponseInputType)
            .With(x => x.SubmissionDetailsAnswerPart_FormatType, QuestionPartResponseFormatType.Text)
            .With(x => x.SubmissionDetailsAnswerPart_Responses, [])
            .CreateMany()
            .ToList();

        var testInputBackingQuestions = testItems.Fixture
            .Build<SubmissionDetailsBackingQuestionModelData>()
            .With(x => x.SubmissionDetailsBackingQuestion_AnswerParts, testInputQuestionAnswerParts)
            .CreateMany()
            .ToList();

        var testInputMainQuestions = testItems.Fixture
            .Build<SubmissionDetailsMainQuestionModelData>()
            .With(x => x.SubmissionDetailsMainQuestion_AnswerParts, testInputQuestionAnswerParts)
            .With(x => x.SubmissionDetailsMainQuestion_BackingQuestions, testInputBackingQuestions)
            .CreateMany()
            .ToList();

        var testInputSubmissionDetailsSections = testItems.Fixture
            .Build<SubmissionDetailsSectionModelData>()
            .With(x => x.SubmissionDetailsSection_Questions, testInputMainQuestions)
            .CreateMany()
            .ToList();

        var testSubmissionInformation = testItems.Fixture
            .Build<SubmissionDetailsModelData>()
            .With(x => x.SubmissionDetails_RequestStatus, DataShareRequestStatusType.Draft)
            .With(x => x.SubmissionDetails_Sections, testInputSubmissionDetailsSections)
            .Create();

        Assert.That(() => testItems.SupplierDataShareRequestModelDataFactory.CreateSubmissionDetails(testSubmissionInformation), 
            Throws.TypeOf<InvalidEnumValueException>().With.Message.EqualTo("Unknown ResponseInputType"));
    }

    [Test]
    public void GivenSubmissionDetailsWithInvalidResponseFormatType_WhenICreateSubmissionDetails_ThenAnInvalidEnumValueExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        var invalidResponseFormatType = (QuestionPartResponseFormatType)Enum.GetValues<QuestionPartResponseFormatType>().Cast<int>().Max() + 1;

        var testInputQuestionAnswerParts = testItems.Fixture
            .Build<SubmissionDetailsAnswerPartModelData>()
            .With(x => x.SubmissionDetailsAnswerPart_InputType, QuestionPartResponseInputType.FreeForm)
            .With(x => x.SubmissionDetailsAnswerPart_FormatType, invalidResponseFormatType)
            .With(x => x.SubmissionDetailsAnswerPart_Responses, [])
            .CreateMany()
            .ToList();

        var testInputBackingQuestions = testItems.Fixture
            .Build<SubmissionDetailsBackingQuestionModelData>()
            .With(x => x.SubmissionDetailsBackingQuestion_AnswerParts, testInputQuestionAnswerParts)
            .CreateMany()
            .ToList();

        var testInputMainQuestions = testItems.Fixture
            .Build<SubmissionDetailsMainQuestionModelData>()
            .With(x => x.SubmissionDetailsMainQuestion_AnswerParts, testInputQuestionAnswerParts)
            .With(x => x.SubmissionDetailsMainQuestion_BackingQuestions, testInputBackingQuestions)
            .CreateMany()
            .ToList();

        var testInputSubmissionDetailsSections = testItems.Fixture
            .Build<SubmissionDetailsSectionModelData>()
            .With(x => x.SubmissionDetailsSection_Questions, testInputMainQuestions)
            .CreateMany()
            .ToList();

        var testSubmissionInformation = testItems.Fixture
            .Build<SubmissionDetailsModelData>()
            .With(x => x.SubmissionDetails_RequestStatus, DataShareRequestStatusType.Draft)
            .With(x => x.SubmissionDetails_Sections, testInputSubmissionDetailsSections)
            .Create();

        Assert.That(() => testItems.SupplierDataShareRequestModelDataFactory.CreateSubmissionDetails(testSubmissionInformation),
            Throws.TypeOf<InvalidEnumValueException>().With.Message.EqualTo("Unknown ResponseFormatType"));
    }
    #endregion

    #region CreateSubmissionReviewInformation() Tests
    [Test]
    public void GivenNullSubmissionReviewInformationModelData_WhenICreateSubmissionReviewInformation_ThenAnArgumentNullExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        Assert.That(() => testItems.SupplierDataShareRequestModelDataFactory.CreateSubmissionReviewInformation(null!),
            Throws.ArgumentNullException.With.Property("ParamName").EqualTo("submissionReviewInformationModelData"));
    }

    [Test]
    public void GivenSubmissionReviewInformationModelData_WhenICreateSubmissionReviewInformation_ThenSubmissionReviewInformationIsCreatedFromTheGivenModelData(
        [ValueSource(nameof(ValidInputDataShareRequestStatuses))] DataShareRequestStatusType testDataShareRequestStatusType)
    {
        var testItems = CreateTestItems();

        var testSubmissionDetailsModelData = testItems.Fixture
            .Build<SubmissionDetailsModelData>()
            .With(x => x.SubmissionDetails_RequestStatus, testDataShareRequestStatusType)
            .Create();

        var testSubmissionReviewInformationModelData = testItems.Fixture
            .Build<SubmissionReviewInformationModelData>()
            .With(x => x.SubmissionReviewInformation_SubmissionDetails, testSubmissionDetailsModelData)
            .Create();

        var result = testItems.SupplierDataShareRequestModelDataFactory.CreateSubmissionReviewInformation(
            testSubmissionReviewInformationModelData);

        Assert.Multiple(() =>
        {
            Assert.That(result.SupplierNotes, Is.EqualTo(testSubmissionReviewInformationModelData.SubmissionReviewInformation_SupplierNotes));

            var testSubmissionDetails = testSubmissionReviewInformationModelData.SubmissionReviewInformation_SubmissionDetails;
            var resultSubmissionDetails = result.SubmissionDetails;

            Assert.That(resultSubmissionDetails.DataShareRequestId, Is.EqualTo(testSubmissionDetails.SubmissionDetails_DataShareRequestId));
            Assert.That(resultSubmissionDetails.DataShareRequestRequestId, Is.EqualTo(testSubmissionDetails.SubmissionDetails_DataShareRequestRequestId));
            Assert.That(resultSubmissionDetails.RequestStatus, Is.EqualTo(GetExpectedDataShareRequestStatus(testSubmissionDetails.SubmissionDetails_RequestStatus)));
            Assert.That(resultSubmissionDetails.AcquirerOrganisationName, Is.EqualTo(testSubmissionDetails.SubmissionDetails_AcquirerOrganisationName));
            Assert.That(resultSubmissionDetails.EsdaName, Is.EqualTo(testSubmissionDetails.SubmissionDetails_EsdaName));

            var testSubmissionSections = testSubmissionDetails.SubmissionDetails_Sections;
            var resultSubmissionSections = resultSubmissionDetails.Sections;
            Assert.That(resultSubmissionSections, Has.Exactly(testSubmissionSections.Count).Items);

            foreach (var testSubmissionSection in testSubmissionSections)
            {
                var resultSubmissionSection = resultSubmissionSections.FirstOrDefault(x =>
                    x.SectionHeader == testSubmissionSection.SubmissionDetailsSection_SectionHeader);

                Assert.That(resultSubmissionSection!.SectionNumber, Is.EqualTo(testSubmissionSection.SubmissionDetailsSection_SectionNumber));
            }

            var testSubmissionComments = testSubmissionDetails.SubmissionDetails_SubmissionReturnComments;
            var resultSubmissionReturns = resultSubmissionDetails.SubmissionReturnDetailsSet.SubmissionReturns;
            Assert.That(resultSubmissionReturns, Has.Exactly(testSubmissionComments.Count).Items);
            foreach (var testSubmissionComment in testSubmissionComments)
            {
                var resultSubmissionReturn = resultSubmissionReturns.FirstOrDefault(x =>
                    x.ReturnComments == testSubmissionComment.Comments);

                Assert.That(resultSubmissionReturn!.ReturnedOnUtc, Is.EqualTo(testSubmissionComment.ReturnedOnUtc));
            }
        });
    }
    #endregion

    #region CreateReturnedSubmissionInformation() Tests
    [Test]
    public void GivenANullReturnedSubmissionInformationModelData_WhenICreateReturnedSubmissionInformation_ThenAnArgumentNullExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        Assert.That(() => testItems.SupplierDataShareRequestModelDataFactory.CreateReturnedSubmissionInformation(null!),
            Throws.ArgumentNullException.With.Property("ParamName").EqualTo("returnedSubmissionInformationModelData"));
    }

    [Test]
    public void GivenReturnedSubmissionInformationModelData_WhenICreateReturnedSubmissionInformation_ThenAReturnedSubmissionInformationIsCreatedFromTheModelData(
        [ValueSource(nameof(ValidInputDataShareRequestStatuses))] DataShareRequestStatusType testDataShareRequestStatusType)
    {
        var testItems = CreateTestItems();

        var testReturnedSubmissionInformationModelData = testItems.Fixture
            .Build<ReturnedSubmissionInformationModelData>()
            .With(x => x.ReturnedSubmission_RequestStatus, testDataShareRequestStatusType)
            .Create();

        var result = testItems.SupplierDataShareRequestModelDataFactory.CreateReturnedSubmissionInformation(
            testReturnedSubmissionInformationModelData);

        Assert.Multiple(() =>
        {
            Assert.That(result.DataShareRequestId, Is.EqualTo(testReturnedSubmissionInformationModelData.ReturnedSubmission_DataShareRequestId));
            Assert.That(result.DataShareRequestRequestId, Is.EqualTo(testReturnedSubmissionInformationModelData.ReturnedSubmission_DataShareRequestRequestId));
            Assert.That(result.RequestStatus, Is.EqualTo(GetExpectedDataShareRequestStatus(testReturnedSubmissionInformationModelData.ReturnedSubmission_RequestStatus)));
            Assert.That(result.AcquirerOrganisationName, Is.EqualTo(testReturnedSubmissionInformationModelData.ReturnedSubmission_AcquirerOrganisationName));
            Assert.That(result.EsdaName, Is.EqualTo(testReturnedSubmissionInformationModelData.ReturnedSubmission_EsdaName));
            Assert.That(result.SubmittedOn, Is.EqualTo(testReturnedSubmissionInformationModelData.ReturnedSubmission_SubmittedOn));
            Assert.That(result.ReturnedOn, Is.EqualTo(testReturnedSubmissionInformationModelData.ReturnedSubmission_ReturnedOn));
            Assert.That(result.WhenNeededBy, Is.EqualTo(testReturnedSubmissionInformationModelData.ReturnedSubmission_WhenNeededBy));
            Assert.That(result.SupplierNotes, Is.EqualTo(testReturnedSubmissionInformationModelData.ReturnedSubmission_SupplierNotes));
            Assert.That(result.FeedbackProvided, Is.EqualTo(testReturnedSubmissionInformationModelData.ReturnedSubmission_FeedbackProvided));
        });
    }
    #endregion

    #region CreateCompletedSubmissionInformation() Tests
    [Test]
    public void GivenANullCompletedSubmissionInformationModelData_WhenICreateCompletedSubmissionInformation_ThenAnArgumentNullExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        Assert.That(() => testItems.SupplierDataShareRequestModelDataFactory.CreateCompletedSubmissionInformation(null!),
            Throws.ArgumentNullException.With.Property("ParamName").EqualTo("completedSubmissionInformationModelData"));
    }

    [Test]
    public void GivenCompletedSubmissionInformationModelData_WhenICreateCompletedSubmissionInformation_ThenACompletedSubmissionInformationIsCreatedFromTheModelData(
        [ValueSource(nameof(ValidInputDataShareRequestStatuses))] DataShareRequestStatusType testDataShareRequestStatusType,
        [ValueSource(nameof(ValidSubmissionDecisions))] SubmissionDecisionType testSubmissionDecisionType)
    {
        var testItems = CreateTestItems();

        var testCompletedSubmissionInformationModelData = testItems.Fixture
            .Build<CompletedSubmissionInformationModelData>()
            .With(x => x.CompletedSubmission_DataShareRequestStatus, testDataShareRequestStatusType)
            .With(x => x.CompletedSubmission_Decision, testSubmissionDecisionType)
            .Create();

        var result = testItems.SupplierDataShareRequestModelDataFactory.CreateCompletedSubmissionInformation(
            testCompletedSubmissionInformationModelData);

        Assert.Multiple(() =>
        {
            Assert.That(result.DataShareRequestId, Is.EqualTo(testCompletedSubmissionInformationModelData.CompletedSubmission_DataShareRequestId));
            Assert.That(result.DataShareRequestRequestId, Is.EqualTo(testCompletedSubmissionInformationModelData.CompletedSubmission_DataShareRequestRequestId));
            Assert.That(result.RequestStatus, Is.EqualTo(GetExpectedDataShareRequestStatus(testCompletedSubmissionInformationModelData.CompletedSubmission_DataShareRequestStatus)));
            Assert.That(result.Decision, Is.EqualTo(GetExpectedSubmissionDecision(testCompletedSubmissionInformationModelData.CompletedSubmission_Decision)));
            Assert.That(result.AcquirerUserEmail, Is.EqualTo(testCompletedSubmissionInformationModelData.CompletedSubmission_AcquirerUserEmailAddress));
            Assert.That(result.AcquirerOrganisationName, Is.EqualTo(testCompletedSubmissionInformationModelData.CompletedSubmission_AcquirerOrganisationName));
            Assert.That(result.EsdaName, Is.EqualTo(testCompletedSubmissionInformationModelData.CompletedSubmission_EsdaName));
            Assert.That(result.SubmittedOn, Is.EqualTo(testCompletedSubmissionInformationModelData.CompletedSubmission_SubmittedOn));
            Assert.That(result.CompletedOn, Is.EqualTo(testCompletedSubmissionInformationModelData.CompletedSubmission_CompletedOn));
            Assert.That(result.WhenNeededBy, Is.EqualTo(testCompletedSubmissionInformationModelData.CompletedSubmission_WhenNeededBy));
            Assert.That(result.SupplierNotes, Is.EqualTo(testCompletedSubmissionInformationModelData.CompletedSubmission_SupplierNotes));
            Assert.That(result.FeedbackProvided, Is.EqualTo(testCompletedSubmissionInformationModelData.CompletedSubmission_FeedbackProvided));
        });
    }
    #endregion

    #region CreateDataShareRequestAcceptanceResult() Tests
    [Test]
    public void GivenANullAcceptedDecisionSummaryModelData_WhenICreateDataShareRequestAcceptanceResult_ThenAnArgumentNullExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        Assert.That(() => testItems.SupplierDataShareRequestModelDataFactory.CreateDataShareRequestAcceptanceResult(null!, It.IsAny<bool?>()),
            Throws.ArgumentNullException.With.Property("ParamName").EqualTo("acceptedDecisionSummaryModelData"));
    }

    [Test]
    public void GivenAcceptedDecisionSummaryModelData_WhenICreateDataShareRequestAcceptanceResult_ThenADataShareRequestAcceptanceResultIsCreatedUsingTheModelData(
        [ValueSource(nameof(ValidInputDataShareRequestStatuses))] DataShareRequestStatusType testDataShareRequestStatus,
        [Values(true, false, null)] bool? testNotificationSuccess)
    {
        var testItems = CreateTestItems();

        var testAcceptedDecisionSummaryModelData = testItems.Fixture
            .Build<AcceptedDecisionSummaryModelData>()
            .With(x => x.AcceptedDecisionSummary_RequestStatus, testDataShareRequestStatus)
            .Create();

        var result = testItems.SupplierDataShareRequestModelDataFactory.CreateDataShareRequestAcceptanceResult(
            testAcceptedDecisionSummaryModelData,
            testNotificationSuccess);

        Assert.Multiple(() =>
        {
            Assert.That(result.DataShareRequestId, Is.EqualTo(testAcceptedDecisionSummaryModelData.AcceptedDecisionSummary_DataShareRequestId));

            Assert.That(result.NotificationSuccess, Is.EqualTo(GetExpectedNotificationSuccess(testNotificationSuccess)));

            var resultSummary = result.AcceptedDecisionSummary;
            Assert.That(resultSummary.DataShareRequestId, Is.EqualTo(testAcceptedDecisionSummaryModelData.AcceptedDecisionSummary_DataShareRequestId));
            Assert.That(resultSummary.DataShareRequestRequestId, Is.EqualTo(testAcceptedDecisionSummaryModelData.AcceptedDecisionSummary_DataShareRequestRequestId));
            Assert.That(resultSummary.RequestStatus, Is.EqualTo(GetExpectedDataShareRequestStatus(testAcceptedDecisionSummaryModelData.AcceptedDecisionSummary_RequestStatus)));
            Assert.That(resultSummary.AcquirerUserEmailAddress, Is.EqualTo(testAcceptedDecisionSummaryModelData.AcceptedDecisionSummary_AcquirerUserEmailAddress));
            Assert.That(resultSummary.AcquirerOrganisationName, Is.EqualTo(testAcceptedDecisionSummaryModelData.AcceptedDecisionSummary_AcquirerOrganisationName));
        });
    }
    #endregion

    #region CreateDataShareRequestRejectionResult() Tests
    [Test]
    public void GivenANullRejectedDecisionSummaryModelData_WhenICreateDataShareRequestRejectionResult_ThenAnArgumentNullExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        Assert.That(() => testItems.SupplierDataShareRequestModelDataFactory.CreateDataShareRequestRejectionResult(null!, It.IsAny<bool?>()),
            Throws.ArgumentNullException.With.Property("ParamName").EqualTo("rejectedDecisionSummaryModelData"));
    }

    [Test]
    public void GivenRejectedDecisionSummaryModelData_WhenICreateDataShareRequestRejectionResult_ThenADataShareRequestRejectionResultIsCreatedUsingTheModelData(
        [ValueSource(nameof(ValidInputDataShareRequestStatuses))] DataShareRequestStatusType testDataShareRequestStatus,
        [Values(true, false, null)] bool? testNotificationSuccess)
    {
        var testItems = CreateTestItems();

        var testRejectedDecisionSummaryModelData = testItems.Fixture
            .Build<RejectedDecisionSummaryModelData>()
            .With(x => x.RejectedDecisionSummary_RequestStatus, testDataShareRequestStatus)
            .Create();

        var result = testItems.SupplierDataShareRequestModelDataFactory.CreateDataShareRequestRejectionResult(
            testRejectedDecisionSummaryModelData,
            testNotificationSuccess);

        Assert.Multiple(() =>
        {
            Assert.That(result.DataShareRequestId, Is.EqualTo(testRejectedDecisionSummaryModelData.RejectedDecisionSummary_DataShareRequestId));

            Assert.That(result.NotificationSuccess, Is.EqualTo(GetExpectedNotificationSuccess(testNotificationSuccess)));

            var resultSummary = result.RejectedDecisionSummary;
            Assert.That(resultSummary.DataShareRequestId, Is.EqualTo(testRejectedDecisionSummaryModelData.RejectedDecisionSummary_DataShareRequestId));
            Assert.That(resultSummary.DataShareRequestRequestId, Is.EqualTo(testRejectedDecisionSummaryModelData.RejectedDecisionSummary_DataShareRequestRequestId));
            Assert.That(resultSummary.RequestStatus, Is.EqualTo(GetExpectedDataShareRequestStatus(testRejectedDecisionSummaryModelData.RejectedDecisionSummary_RequestStatus)));
            Assert.That(resultSummary.AcquirerOrganisationName, Is.EqualTo(testRejectedDecisionSummaryModelData.RejectedDecisionSummary_AcquirerOrganisationName));
        });
    }
    #endregion

    #region CreateDataShareRequestReturnResult() Tests
    [Test]
    public void GivenANullReturnedDecisionSummaryModelData_WhenICreateDataShareRequestReturnResult_ThenAnArgumentNullExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        Assert.That(() => testItems.SupplierDataShareRequestModelDataFactory.CreateDataShareRequestReturnResult(null!, It.IsAny<bool?>()),
            Throws.ArgumentNullException.With.Property("ParamName").EqualTo("returnedDecisionSummaryModelData"));
    }

    [Test]
    public void GivenReturnedDecisionSummaryModelData_WhenICreateDataShareRequestReturnResult_ThenADataShareRequestReturnResultIsCreatedUsingTheModelData(
        [ValueSource(nameof(ValidInputDataShareRequestStatuses))] DataShareRequestStatusType testDataShareRequestStatus,
        [Values(true, false, null)] bool? testNotificationSuccess)
    {
        var testItems = CreateTestItems();

        var testReturnedDecisionSummaryModelData = testItems.Fixture
            .Build<ReturnedDecisionSummaryModelData>()
            .With(x => x.ReturnedDecisionSummary_RequestStatus, testDataShareRequestStatus)
            .Create();

        var result = testItems.SupplierDataShareRequestModelDataFactory.CreateDataShareRequestReturnResult(
            testReturnedDecisionSummaryModelData,
            testNotificationSuccess);

        Assert.Multiple(() =>
        {
            Assert.That(result.DataShareRequestId, Is.EqualTo(testReturnedDecisionSummaryModelData.ReturnedDecisionSummary_DataShareRequestId));

            Assert.That(result.NotificationSuccess, Is.EqualTo(GetExpectedNotificationSuccess(testNotificationSuccess)));

            var resultSummary = result.ReturnedDecisionSummary;
            Assert.That(resultSummary.DataShareRequestId, Is.EqualTo(testReturnedDecisionSummaryModelData.ReturnedDecisionSummary_DataShareRequestId));
            Assert.That(resultSummary.DataShareRequestRequestId, Is.EqualTo(testReturnedDecisionSummaryModelData.ReturnedDecisionSummary_DataShareRequestRequestId));
            Assert.That(resultSummary.RequestStatus, Is.EqualTo(GetExpectedDataShareRequestStatus(testReturnedDecisionSummaryModelData.ReturnedDecisionSummary_RequestStatus)));
            Assert.That(resultSummary.AcquirerOrganisationName, Is.EqualTo(testReturnedDecisionSummaryModelData.ReturnedDecisionSummary_AcquirerOrganisationName));
        });
    }
    #endregion

    #region Test Data Creation
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

    private static SubmissionDecision GetExpectedSubmissionDecision(
        SubmissionDecisionType submissionDecision)
    {
        return submissionDecision switch
        {
            SubmissionDecisionType.Accepted => SubmissionDecision.Accepted,
            SubmissionDecisionType.Rejected => SubmissionDecision.Rejected,
            _ => throw new Exception()
        };
    }

    private static NotificationSuccess GetExpectedNotificationSuccess(
        bool? notificationSuccess)
    {
        return notificationSuccess switch
        {
            true => NotificationSuccess.SentSuccessfully,
            false => NotificationSuccess.FailedToSend,
            null => NotificationSuccess.NotSent
        };
    }

    #region Request Statuses
    private static readonly IEnumerable<DataShareRequestStatusType> AllDataShareRequestStatuses = Enum.GetValues<DataShareRequestStatusType>();

    private static readonly IEnumerable<DataShareRequestStatusType> InvalidInputDataShareRequestStatuses = [DataShareRequestStatusType.None];

    private static readonly IEnumerable<DataShareRequestStatusType> ValidInputDataShareRequestStatuses = AllDataShareRequestStatuses.Except(InvalidInputDataShareRequestStatuses);
    #endregion

    #region Decisions
    private static readonly IEnumerable<SubmissionDecisionType> AllSubmissionDecisions = Enum.GetValues<SubmissionDecisionType>();

    private static readonly IEnumerable<SubmissionDecisionType> InvalidSubmissionDecisions = [SubmissionDecisionType.None];

    private static readonly IEnumerable<SubmissionDecisionType> ValidSubmissionDecisions = AllSubmissionDecisions.Except(InvalidSubmissionDecisions);
    #endregion
    #endregion

    #region Test Item Creation
    private static TestItems CreateTestItems()
    {
        var fixture = new Fixture().Customize(new AutoMoqCustomization());

        var supplierDataShareRequestModelDataFactory = new SupplierDataShareRequestModelDataFactory();

        return new TestItems(
            fixture,
            supplierDataShareRequestModelDataFactory);
    }

    private class TestItems(
        IFixture fixture,
        ISupplierDataShareRequestModelDataFactory supplierDataShareRequestModelDataFactory)
    {
        public IFixture Fixture { get; } = fixture;
        public ISupplierDataShareRequestModelDataFactory SupplierDataShareRequestModelDataFactory { get; } = supplierDataShareRequestModelDataFactory;
    }
    #endregion
}