using Agrimetrics.DataShare.Api.Dto.Models.Acquirer;
using Agrimetrics.DataShare.Api.Dto.Models.Acquirer.DataShareRequests;
using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests;
using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests.Answers.Answers;
using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests.Answers.DataShareRequestAnswerSummaries;
using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests.Answers.DataShareRequestQuestionAnswers;
using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests.Questions;
using Agrimetrics.DataShare.Api.Dto.Models.Questions;
using Agrimetrics.DataShare.Api.Dto.Models.Questions.QuestionParts;
using Agrimetrics.DataShare.Api.Dto.Models.Questions.QuestionParts.FreeFormItems;
using Agrimetrics.DataShare.Api.Dto.Models.Questions.QuestionParts.OptionSelectionItems;
using Agrimetrics.DataShare.Api.Dto.Models.Questions.QuestionParts.ResponseFormats;
using Agrimetrics.DataShare.Api.Dto.Models.Questions.QuestionSets;
using Agrimetrics.DataShare.Api.Dto.Responses.Notifications;
using Agrimetrics.DataShare.Api.Logic.Exceptions;
using Agrimetrics.DataShare.Api.Logic.ModelData.AuditLogs;
using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests;
using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests.Answers.DataShareRequestAnswerSummaries;
using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests.Answers.DataShareRequestQuestionAnswerResponses;
using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests.Answers.DataShareRequestQuestionAnswers;
using Agrimetrics.DataShare.Api.Logic.ModelData.Questions;
using Agrimetrics.DataShare.Api.Logic.ModelData.Questions.QuestionParts;
using Agrimetrics.DataShare.Api.Logic.ModelData.Questions.QuestionParts.FreeFormItems;
using Agrimetrics.DataShare.Api.Logic.ModelData.Questions.QuestionParts.OptionSelectionItems;
using Agrimetrics.DataShare.Api.Logic.ModelData.Questions.QuestionParts.ResponseFormats;
using Agrimetrics.DataShare.Api.Logic.ModelData.Questions.QuestionSets;
using Agrimetrics.DataShare.Api.Logic.Services.Users.Model;
using QuestionPartResponseFormatType = Agrimetrics.DataShare.Api.Logic.ModelData.Questions.QuestionParts.ResponseFormats.QuestionPartResponseFormatType;
using QuestionPartResponseInputType = Agrimetrics.DataShare.Api.Logic.ModelData.Questions.QuestionParts.ResponseFormats.QuestionPartResponseInputType;

namespace Agrimetrics.DataShare.Api.Logic.ModelData;

internal class AcquirerDataShareRequestModelDataFactory : IAcquirerDataShareRequestModelDataFactory
{
    #region Model Data To Dto Data#
    #region Create Question Set Outline
    QuestionSetOutline IAcquirerDataShareRequestModelDataFactory.CreateQuestionSetOutline(
        QuestionSetOutlineModelData questionSetOutlineModelData)
    {
        return new QuestionSetOutline
        {
            Sections = questionSetOutlineModelData.Sections.Select(DoConvertQuestionSetSectionOutline).ToList()
        };
    }

    private static QuestionSetSectionOutline DoConvertQuestionSetSectionOutline(
        QuestionSetSectionOutlineModelData section)
    {
        return new QuestionSetSectionOutline
        {
            OrderWithinQuestionSetOutline = section.QuestionSetSectionOutline_OrderWithinQuestionSetOutline,
            Header = section.QuestionSetSectionOutline_SectionHeader,
            Questions = section.Questions.Select(DoConvertQuestionSetQuestionOutline).ToList()
        };
    }

    private static QuestionSetQuestionOutline DoConvertQuestionSetQuestionOutline(
        QuestionSetQuestionOutlineModelData question)
    {
        return new QuestionSetQuestionOutline
        {
            OrderWithinQuestionSetSection = question.QuestionSetQuestionOutline_OrderWithinSection,
            QuestionText = question.QuestionSetQuestionOutline_QuestionText
        };
    }
    #endregion

    #region Create Data Share Request Summary Set
    DataShareRequestSummarySet IAcquirerDataShareRequestModelDataFactory.CreateDataShareRequestSummarySet(
        IEnumerable<DataShareRequestModelData> dataShareRequestModelDatas)
    {
        ArgumentNullException.ThrowIfNull(dataShareRequestModelDatas);

        return new DataShareRequestSummarySet
        {
            DataShareRequestSummaries = dataShareRequestModelDatas.Select(DoCreateDataShareRequestSummary).ToList()
        };
    }

    private static DataShareRequestSummary DoCreateDataShareRequestSummary(DataShareRequestModelData dataShareRequestModelData)
    {
        return new DataShareRequestSummary
        {
            Id = dataShareRequestModelData.DataShareRequest_Id,
            RequestId = dataShareRequestModelData.DataShareRequest_RequestId,
            EsdaName = dataShareRequestModelData.DataShareRequest_EsdaName,
            DataShareRequestStatus = DoConvertDataShareRequestStatus(dataShareRequestModelData.DataShareRequest_RequestStatus)
        };
    }
    #endregion

    #region Create Data Share Request Admin Summary Set
    DataShareRequestAdminSummarySet IAcquirerDataShareRequestModelDataFactory.CreateDataShareRequestAdminSummarySet(
        IEnumerable<DataShareRequestAdminSummary> dataShareRequestAdminSummaries)
    {
        ArgumentNullException.ThrowIfNull(dataShareRequestAdminSummaries);

        return new DataShareRequestAdminSummarySet
        {
            DataShareRequestAdminSummaries = dataShareRequestAdminSummaries.ToList()
        };
    }
    #endregion

    #region Create Data Share Request Admin Summary
    DataShareRequestAdminSummary IAcquirerDataShareRequestModelDataFactory.CreateDataShareRequestAdminSummary(
        DataShareRequestModelData dataShareRequestModelData,
        DateTime whenCreated,
        DateTime? whenSubmitted,
        string createdByUserEmailAddresses,
        DateTime? whenNeededBy)
    {
        return new DataShareRequestAdminSummary
        {
            Id = dataShareRequestModelData.DataShareRequest_Id,
            RequestId = dataShareRequestModelData.DataShareRequest_RequestId,
            EsdaName = dataShareRequestModelData.DataShareRequest_EsdaName,
            WhenCreatedUtc = whenCreated,
            WhenSubmittedUtc = whenSubmitted,
            CreatedByUserEmailAddress = createdByUserEmailAddresses,
            WhenNeededByUtc = whenNeededBy,
            DataShareRequestStatus = DoConvertDataShareRequestStatus(dataShareRequestModelData.DataShareRequest_RequestStatus)
        };
    }
    #endregion

    #region Create Data Share Request Questions Summary
    DataShareRequestQuestionsSummary IAcquirerDataShareRequestModelDataFactory.CreateDataShareRequestQuestionsSummary(
        DataShareRequestQuestionsSummaryModelData dataShareRequestQuestionsSummaryModelData)
    {
        ArgumentNullException.ThrowIfNull(dataShareRequestQuestionsSummaryModelData);

        return new DataShareRequestQuestionsSummary
        {
            DataShareRequestId = dataShareRequestQuestionsSummaryModelData.DataShareRequest_Id,
            DataShareRequestRequestId = dataShareRequestQuestionsSummaryModelData.DataShareRequest_RequestId,
            EsdaName = dataShareRequestQuestionsSummaryModelData.DataShareRequest_EsdaName,
            QuestionSetSummary = DoConvertQuestionSetSummary(dataShareRequestQuestionsSummaryModelData)
        };
    }

    private static QuestionSetSummary DoConvertQuestionSetSummary(
        DataShareRequestQuestionsSummaryModelData dataShareRequestQuestionsSummaryModelData)
    {
        var questionSetSummaryModelData = dataShareRequestQuestionsSummaryModelData.DataShareRequest_QuestionSetSummary;

        return new QuestionSetSummary
        {
            Id = questionSetSummaryModelData.QuestionSet_Id,
            AnswersSectionComplete = questionSetSummaryModelData.QuestionSet_AnswersSectionComplete,
            SectionSummaries = questionSetSummaryModelData.QuestionSet_SectionSummaries.Select(DoConvertQuestionSetSectionSummary).ToList(),
            DataShareRequestStatus = DoConvertDataShareRequestStatus(dataShareRequestQuestionsSummaryModelData.DataShareRequest_DataShareRequestStatus),
            QuestionsRemainThatRequireAResponse = dataShareRequestQuestionsSummaryModelData.DataShareRequest_QuestionsRemainThatRequireAResponse,
            SupplierOrganisationName = dataShareRequestQuestionsSummaryModelData.DataShareRequest_SupplierOrganisationName,
            SubmissionResponseFromSupplier = dataShareRequestQuestionsSummaryModelData.DataShareRequest_SubmissionResponseFromSupplier,
            CancellationReasonsFromAcquirer = dataShareRequestQuestionsSummaryModelData.DataShareRequest_CancellationReasonsFromAcquirer,
            AcquirerUserDetails = DoBuildAcquirerUserDetails(dataShareRequestQuestionsSummaryModelData)
        };
    }

    private static AcquirerUserDetails DoBuildAcquirerUserDetails(
        DataShareRequestQuestionsSummaryModelData dataShareRequestQuestionsSummaryModelData)
    {
        return new AcquirerUserDetails
        {
            OrganisationId = dataShareRequestQuestionsSummaryModelData.DataShareRequest_AcquirerOrganisationId,
            DomainId = dataShareRequestQuestionsSummaryModelData.DataShareRequest_AcquirerDomainId,
            UserId = dataShareRequestQuestionsSummaryModelData.DataShareRequest_AcquirerUserId
        };
    }

    private static QuestionSetSectionSummary DoConvertQuestionSetSectionSummary(
        QuestionSetSectionSummaryModelData questionSetSectionSummaryModelData)
    {
        return new QuestionSetSectionSummary
        {
            Id = questionSetSectionSummaryModelData.QuestionSetSection_Id,
            SectionNumber = questionSetSectionSummaryModelData.QuestionSetSection_Number,
            SectionHeader = questionSetSectionSummaryModelData.QuestionSetSection_Header,
            SectionIsComplete = questionSetSectionSummaryModelData.QuestionSetSection_IsComplete,
            QuestionSummaries = questionSetSectionSummaryModelData.QuestionSetSection_QuestionSummaries.Select(DoConvertQuestionSummary).ToList()
        };
    }

    private static QuestionSummary DoConvertQuestionSummary(QuestionSummaryModelData questionSummaryModelData)
    {
        return new QuestionSummary
        {
            QuestionId = questionSummaryModelData.Question_Id,
            QuestionOrderWithinQuestionSetSection = questionSummaryModelData.Question_OrderWithinQuestionSetSection,
            QuestionHeader = questionSummaryModelData.Question_Header,
            QuestionStatus = DoConvertQuestionState(questionSummaryModelData.Question_QuestionStatus),
            QuestionCanBeAnswered = questionSummaryModelData.Question_QuestionCanBeAnswered
        };
    }

    private static QuestionStatus DoConvertQuestionState(QuestionStatusType questionStatus)
    {
        return questionStatus switch
        {
            QuestionStatusType.NotStarted => QuestionStatus.NotStarted,
            QuestionStatusType.CannotStartYet => QuestionStatus.CannotStartYet,
            QuestionStatusType.Completed => QuestionStatus.Completed,
            QuestionStatusType.NotApplicable => QuestionStatus.NotApplicable,
            QuestionStatusType.NoResponseNeeded => QuestionStatus.NoResponseNeeded,

            QuestionStatusType.NotSet => throw new InconsistentDataException("QuestionStatusType has not been set"),
            _ => throw new InvalidEnumValueException("Invalid QuestionStatusType provided")
        };
    }
    #endregion

    #region Create Data Share Request Question
    DataShareRequestQuestion IAcquirerDataShareRequestModelDataFactory.CreateDataShareRequestQuestion(
        DataShareRequestQuestionModelData dataShareRequestQuestionModelData)
    {
        ArgumentNullException.ThrowIfNull(dataShareRequestQuestionModelData);

        return new DataShareRequestQuestion
        {
            DataShareRequestId = dataShareRequestQuestionModelData.DataShareRequestQuestion_DataShareRequestId,
            DataShareRequestRequestId = dataShareRequestQuestionModelData.DataShareRequestQuestion_DataShareRequestRequestId,
            QuestionId = dataShareRequestQuestionModelData.DataShareRequestQuestion_QuestionId,
            IsOptional = dataShareRequestQuestionModelData.DataShareRequestQuestion_IsOptional,
            QuestionParts = dataShareRequestQuestionModelData.DataShareRequestQuestion_QuestionParts.Select(DoConvertDataShareRequestQuestionPart).ToList(),
            QuestionFooter = DoConvertQuestionFooter(dataShareRequestQuestionModelData.DataShareRequestQuestion_QuestionFooter)
        };
    }

    private static DataShareRequestQuestionFooter? DoConvertQuestionFooter(
        DataShareRequestQuestionFooterModelData? questionFooter)
    {
        return questionFooter == null
            ? null
            : new DataShareRequestQuestionFooter
            {
                FooterHeader = questionFooter.DataShareRequestQuestionFooter_Header,
                FooterItems = questionFooter.DataShareRequestQuestionFooter_Items.Select(DoConvertQuestionFooterItem).ToList()
            };
    }

    private static DataShareRequestQuestionFooterItem DoConvertQuestionFooterItem(
        DataShareRequestQuestionFooterItemModelData questionFooterItem)
    {
        return new DataShareRequestQuestionFooterItem
        {
            Text = questionFooterItem.DataShareRequestQuestionFooterItem_Text,
            OrderWithinFooter = questionFooterItem.DataShareRequestQuestionFooterItem_OrderWithinFooter
        };
    }

    private static DataShareRequestQuestionPart DoConvertDataShareRequestQuestionPart(
        DataShareRequestQuestionPartModelData dataShareRequestQuestionPartModelData)
    {
        return new DataShareRequestQuestionPart
        {
            QuestionPartQuestion = DoConvertQuestionPart(dataShareRequestQuestionPartModelData.DataShareRequestQuestionPart_Question),
            QuestionPartAnswer = DoConvertQuestionPartAnswer(dataShareRequestQuestionPartModelData.DataShareRequestQuestionPart_Answer)
        };
    }

    private static QuestionPart DoConvertQuestionPart(
        QuestionPartModelData questionPartModelData)
    {
        return new QuestionPart
        {
            Id = questionPartModelData.QuestionPart_Id,
            QuestionPartOrderWithinQuestion = questionPartModelData.QuestionPart_QuestionPartOrderWithinQuestion,
            Prompts = DoConvertQuestionPartPrompts(questionPartModelData.QuestionPart_Prompts),
            MultipleAnswerItemControl = DoConvertMultipleAnswerItemControl(questionPartModelData.QuestionPart_MultipleAnswerItemControl),
            ResponseFormat = DoConvertResponseFormat(questionPartModelData.QuestionPart_ResponseFormat)
        };
    }

    private static QuestionPartPrompts DoConvertQuestionPartPrompts(
        QuestionPartPromptsModelData questionPartPrompts)
    {
        return new QuestionPartPrompts
        {
            QuestionText = questionPartPrompts.QuestionPartPrompt_QuestionText,
            HintText = questionPartPrompts.QuestionPartPrompt_HintText,
        };
    }

    private static QuestionPartMultipleAnswerItemControl DoConvertMultipleAnswerItemControl(
        QuestionPartMultipleAnswerItemControlModelData questionPartMultipleAnswerItemControl)
    {
        return new QuestionPartMultipleAnswerItemControl
        {
            MultipleAnswerItemsAreAllowed = questionPartMultipleAnswerItemControl.QuestionPartMultipleAnswerItemControl_MultipleAnswerItemsAreAllowed,
            ItemDescription = questionPartMultipleAnswerItemControl.QuestionPartMultipleAnswerItemControl_ItemDescription,
            CollectionDescription = questionPartMultipleAnswerItemControl.QuestionPartMultipleAnswerItemControl_CollectionDescription
        };
    }

    #region Do Convert Response Format
    private static QuestionPartResponseFormatBase DoConvertResponseFormat(QuestionPartResponseFormatModelData questionPartResponseFormat)
    {
        return questionPartResponseFormat.InputType switch
        {
            QuestionPartResponseInputType.FreeForm => DoConvertFreeFormResponseFormat(questionPartResponseFormat),
            QuestionPartResponseInputType.OptionSelection => DoConvertOptionSelectionResponseFormat(questionPartResponseFormat),
            QuestionPartResponseInputType.None => DoConvertNoInputResponseFormat(questionPartResponseFormat),
            _ => throw new InvalidEnumValueException("Question Part has unknown Input Type")
        };
    }

    private static QuestionPartResponseFormatFreeForm DoConvertFreeFormResponseFormat(
        QuestionPartResponseFormatModelData questionPartResponseFormat)
    {
        return questionPartResponseFormat.FormatType switch
        {
            QuestionPartResponseFormatType.Text => DoConvertFreeFormTextResponseFormat(questionPartResponseFormat),
            QuestionPartResponseFormatType.Numeric => DoConvertFreeFormNumericResponseFormat(questionPartResponseFormat),
            QuestionPartResponseFormatType.Date => DoConvertFreeFormDateResponseFormat(questionPartResponseFormat),
            QuestionPartResponseFormatType.Time => DoConvertFreeFormTimeResponseFormat(questionPartResponseFormat),
            QuestionPartResponseFormatType.DateTime => DoConvertFreeFormDateTimeResponseFormat(questionPartResponseFormat),
            QuestionPartResponseFormatType.Country => DoConvertFreeFormCountryResponseFormat(questionPartResponseFormat),
            _ => throw new InvalidEnumValueException("Question Part with FreeForm Input type has unexpected Response type")
        };
    }

    private static QuestionPartResponseFormatOptionSelect DoConvertOptionSelectionResponseFormat(
        QuestionPartResponseFormatModelData questionPartResponseFormat)
    {
        return questionPartResponseFormat.FormatType switch
        {
            QuestionPartResponseFormatType.SelectSingle => DoConvertOptionSelectionSingleValueResponseFormat(questionPartResponseFormat),
            QuestionPartResponseFormatType.SelectMulti => DoConvertOptionSelectionMultiValueResponseFormat(questionPartResponseFormat),
            _ => throw new InvalidEnumValueException("Question Part with OptionSelect Input type has unexpected Response type")
        };
    }

    private static QuestionPartResponseFormatNone DoConvertNoInputResponseFormat(
        QuestionPartResponseFormatModelData questionPartResponseFormat)
    {
        return questionPartResponseFormat.FormatType switch
        {
            QuestionPartResponseFormatType.ReadOnly => DoConvertNoInputReadOnlyResponseFormat(questionPartResponseFormat),
            _ => throw new InvalidEnumValueException("Question Part with None Input type has unexpected Response type")
        };
    }

    private static QuestionPartResponseFormatFreeFormText DoConvertFreeFormTextResponseFormat(
        QuestionPartResponseFormatModelData questionPartResponseFormat)
    {
        if (questionPartResponseFormat is not QuestionPartResponseFormatFreeFormTextModelData freeFormTextModelData)
            throw new InconsistentDataException("Question Part is not of expected FreeForm Text type");

        return new QuestionPartResponseFormatFreeFormText
        {
            MaximumResponseLength = freeFormTextModelData.QuestionPartResponseFormatFreeFormText_MaximumResponseLength,
            FreeFormOptions = DoConvertFreeFormOptions(freeFormTextModelData.FreeFormOptions)
        };
    }

    private static QuestionPartResponseFormatFreeFormNumeric DoConvertFreeFormNumericResponseFormat(
        QuestionPartResponseFormatModelData questionPartResponseFormat)
    {
        if (questionPartResponseFormat is not QuestionPartResponseFormatFreeFormNumericModelData freeFormNumericModelData)
            throw new InconsistentDataException("Question Part is not of expected FreeForm Numeric type");

        return new QuestionPartResponseFormatFreeFormNumeric
        {
            FreeFormOptions = DoConvertFreeFormOptions(freeFormNumericModelData.FreeFormOptions)
        };
    }

    private static QuestionPartResponseFormatFreeFormCountry DoConvertFreeFormCountryResponseFormat(
        QuestionPartResponseFormatModelData questionPartResponseFormat)
    {
        if (questionPartResponseFormat is not QuestionPartResponseFormatFreeFormCountryModelData freeFormCountryModelData)
            throw new InconsistentDataException("Question Part is not of expected FreeForm Country type");

        return new QuestionPartResponseFormatFreeFormCountry
        {
            FreeFormOptions = DoConvertFreeFormOptions(freeFormCountryModelData.FreeFormOptions)
        };
    }

    private static QuestionPartResponseFormatFreeFormDate DoConvertFreeFormDateResponseFormat(
        QuestionPartResponseFormatModelData questionPartResponseFormat)
    {
        if (questionPartResponseFormat is not QuestionPartResponseFormatFreeFormDateModelData freeFormDateModelData)
            throw new InconsistentDataException("Question Part is not of expected FreeForm Date type");

        return new QuestionPartResponseFormatFreeFormDate
        {
            FreeFormOptions = DoConvertFreeFormOptions(freeFormDateModelData.FreeFormOptions)
        };
    }

    private static QuestionPartResponseFormatFreeFormTime DoConvertFreeFormTimeResponseFormat(
        QuestionPartResponseFormatModelData questionPartResponseFormat)
    {
        if (questionPartResponseFormat is not QuestionPartResponseFormatFreeFormTimeModelData freeFormTimeModelData)
            throw new InconsistentDataException("Question Part is not of expected FreeForm Time type");

        return new QuestionPartResponseFormatFreeFormTime
        {
            FreeFormOptions = DoConvertFreeFormOptions(freeFormTimeModelData.FreeFormOptions)
        };
    }

    private static QuestionPartResponseFormatFreeFormDateTime DoConvertFreeFormDateTimeResponseFormat(
        QuestionPartResponseFormatModelData questionPartResponseFormat)
    {
        if (questionPartResponseFormat is not QuestionPartResponseFormatFreeFormDateTimeModelData freeFormDateTimeModelData)
            throw new InconsistentDataException("Question Part is not of expected FreeForm DateTime type");

        return new QuestionPartResponseFormatFreeFormDateTime
        {
            FreeFormOptions = DoConvertFreeFormOptions(freeFormDateTimeModelData.FreeFormOptions)
        };
    }

    private static QuestionPartFreeFormOptions? DoConvertFreeFormOptions(
        QuestionPartResponseFormatFreeFormOptionsModelData? freeFormOptions)
    {
        return freeFormOptions == null
            ? null
            : new QuestionPartFreeFormOptions
            {
                Id = freeFormOptions.QuestionPartResponseFormatFreeFormOptions_Id,
                ValueEntryMayBeDeclined = freeFormOptions.QuestionPartResponseFormatFreeFormOptions_ValueEntryMayBeDeclined
            };
    }

    private static QuestionPartResponseFormatOptionSelectSingleValue DoConvertOptionSelectionSingleValueResponseFormat(
        QuestionPartResponseFormatModelData questionPartResponseFormat)
    {
        if (questionPartResponseFormat is not QuestionPartResponseFormatOptionSelectSingleValueModelData optionSelectSingleValueModelData)
            throw new InconsistentDataException("Question Part is not of expected OptionSelect SingleValue type");

        return new QuestionPartResponseFormatOptionSelectSingleValue
        {
            SingleSelectionOptions = optionSelectSingleValueModelData.ResponseFormatOptionSelectSingleValue_SingleSelectionOptions.Select(DoConvertOptionSelectSingleSelectionOption).ToList()
        };
    }

    private static QuestionPartOptionSelectionItemForSingleSelection DoConvertOptionSelectSingleSelectionOption(
        QuestionPartOptionSelectionItemForSingleSelectionModelData optionSelectSingleSelectionItemModelData)
    {
        var optionSelectionItemForSingleSelection = DoConvertOptionSelectionItem<QuestionPartOptionSelectionItemForSingleSelection>(optionSelectSingleSelectionItemModelData);
        optionSelectionItemForSingleSelection.IsAlternativeAnswer = optionSelectSingleSelectionItemModelData.SingleSelectionOption_IsAlternativeAnswer;

        return optionSelectionItemForSingleSelection;
    }

    private static QuestionPartResponseFormatOptionSelectMultiValue DoConvertOptionSelectionMultiValueResponseFormat(
        QuestionPartResponseFormatModelData questionPartResponseFormat)
    {
        if (questionPartResponseFormat is not QuestionPartResponseFormatOptionSelectMultiValueModelData optionSelectMultiValueModelData)
            throw new InconsistentDataException("Question Part is not of expected OptionSelect MultiValue type");

        return new QuestionPartResponseFormatOptionSelectMultiValue
        {
            MultiSelectionOptions = optionSelectMultiValueModelData.ResponseFormatOptionSelectMultiValue_MultiSelectionOptions.Select(DoConvertOptionSelectMultiSelectionOption).ToList()
        };
    }

    private static QuestionPartOptionSelectionItemForMultiSelection DoConvertOptionSelectMultiSelectionOption(
        QuestionPartOptionSelectionItemForMultiSelectionModelData optionSelectMultiSelectionItemModelData)
    {
        var optionSelectionItemForMultiSelection = DoConvertOptionSelectionItem<QuestionPartOptionSelectionItemForMultiSelection>(optionSelectMultiSelectionItemModelData);
        optionSelectionItemForMultiSelection.IsMaster = optionSelectMultiSelectionItemModelData.MultiSelectOption_IsMaster;
        return optionSelectionItemForMultiSelection;
    }

    private static T DoConvertOptionSelectionItem<T>(QuestionPartOptionSelectionItemModelData optionSelectionItemModelData)
        where T : QuestionPartOptionSelectionItemBase, new()
    {
        return new T
        {
            Id = optionSelectionItemModelData.OptionSelectionItem_Id,
            ValueText = optionSelectionItemModelData.OptionSelectionItem_ValueText,
            HintText = optionSelectionItemModelData.OptionSelectionItem_HintText,
            OptionOrderWithinSelection = optionSelectionItemModelData.OptionSelectionItem_OptionOrderWithinSelection,
            SupplementaryQuestion = optionSelectionItemModelData.OptionSelectionItem_SupplementaryQuestionPart == null
                ? null
                : DoConvertQuestionPart(optionSelectionItemModelData.OptionSelectionItem_SupplementaryQuestionPart)
        };
    }

    private static QuestionPartResponseFormatNoneReadOnly DoConvertNoInputReadOnlyResponseFormat(
        QuestionPartResponseFormatModelData questionPartResponseFormat)
    {
        if (questionPartResponseFormat is not QuestionPartResponseFormatReadOnlyModelData)
            throw new InconsistentDataException("Question Part is not of expected ReadOnly type");

        return new QuestionPartResponseFormatNoneReadOnly();
    }
    #endregion

    private static QuestionPartAnswer? DoConvertQuestionPartAnswer(
        QuestionPartAnswerModelData? questionPartAnswerModelData)
    {
        return questionPartAnswerModelData == null
            ? null
            : new QuestionPartAnswer
            {
                QuestionPartId = questionPartAnswerModelData.QuestionPartAnswer_QuestionPartId,
                AnswerPartResponses = questionPartAnswerModelData.QuestionPartAnswer_AnswerPartResponses.Select(DoConvertQuestionPartAnswerResponse).ToList()
            };
    }

    private static QuestionPartAnswerResponse DoConvertQuestionPartAnswerResponse(
        QuestionPartAnswerResponseModelData questionPartAnswerResponseModelData)
    {
        return new QuestionPartAnswerResponse
        {
            InputType = DoConvertResponseInputType(questionPartAnswerResponseModelData.QuestionPartAnswerResponse_InputType),
            OrderWithinAnswerPart = questionPartAnswerResponseModelData.QuestionPartAnswerResponse_OrderWithinAnswerPart,
            ResponseItem = questionPartAnswerResponseModelData.QuestionPartAnswerResponse_ResponseItem != null
                ? DoConvertResponseItem(questionPartAnswerResponseModelData, questionPartAnswerResponseModelData.QuestionPartAnswerResponse_ResponseItem)
                : null
        };
    }

    private static QuestionPartAnswerResponseItemBase DoConvertResponseItem(
        QuestionPartAnswerResponseModelData questionPartAnswerResponseModelData,
        QuestionPartAnswerResponseItemModelData responseItem)
    {
        // At this stage the input type has been verified and can only be freeform or option selection
        return questionPartAnswerResponseModelData.QuestionPartAnswerResponse_InputType == QuestionPartResponseInputType.FreeForm
            ? DoConvertFreeFormQuestionPartAnswerResponseItem(responseItem)
            : DoConvertOptionSelectionQuestionPartAnswerResponseItem(responseItem);
    }

    private static QuestionPartAnswerResponseItemFreeForm DoConvertFreeFormQuestionPartAnswerResponseItem(
        QuestionPartAnswerResponseItemModelData questionPartAnswerResponseItem)
    {
        var questionPartResponseAnswerItemFreeFormModelData = (QuestionPartAnswerResponseItemFreeFormModelData) questionPartAnswerResponseItem;

        var questionPartAnswerItemFreeForm = DoConvertOptionSelectionQuestion<QuestionPartAnswerResponseItemFreeForm>();
        questionPartAnswerItemFreeForm.EnteredValue = questionPartResponseAnswerItemFreeFormModelData.QuestionPartAnswerItemFreeForm_EnteredValue;
        questionPartAnswerItemFreeForm.ValueEntryDeclined = questionPartResponseAnswerItemFreeFormModelData.QuestionPartAnswerItemFreeForm_ValueEntryDeclined;

        return questionPartAnswerItemFreeForm;
    }

    private static QuestionPartAnswerResponseItemSelectionOption DoConvertOptionSelectionQuestionPartAnswerResponseItem(
        QuestionPartAnswerResponseItemModelData questionPartAnswerResponseItem)
    {
        var questionPartAnswerItemOptionSelectionModelData = (QuestionPartAnswerResponseItemOptionSelectionModelData) questionPartAnswerResponseItem;

        var questionPartAnswerItemSelectionOption = DoConvertOptionSelectionQuestion<QuestionPartAnswerResponseItemSelectionOption>();
        questionPartAnswerItemSelectionOption.SelectedOptions = questionPartAnswerItemOptionSelectionModelData.QuestionPartAnswerItem_SelectedOptionItems
            .Select(DoConvertQuestionPartAnswerItemSelectionOptionItemModelData).ToList();

        return questionPartAnswerItemSelectionOption;
    }

    private static QuestionPartAnswerItemSelectionOptionItem DoConvertQuestionPartAnswerItemSelectionOptionItemModelData(
        QuestionPartAnswerItemSelectionOptionItemModelData questionPartAnswerItemSelectionOptionItemModelData)
    {
        return new QuestionPartAnswerItemSelectionOptionItem
        {
            OptionSelectionItemId = questionPartAnswerItemSelectionOptionItemModelData.QuestionPartAnswerItem_OptionSelectionItemId,
            SupplementaryQuestionPartAnswer = DoConvertQuestionPartAnswer(questionPartAnswerItemSelectionOptionItemModelData.QuestionPartAnswerItem_SupplementaryQuestionPartAnswer)
        };
    }

    private static T DoConvertOptionSelectionQuestion<T>()
        where T : QuestionPartAnswerResponseItemBase, new()
    {
        return new T();
    }
    #endregion

    #region Create Data Share Request Answers Summary
    DataShareRequestAnswersSummary IAcquirerDataShareRequestModelDataFactory.CreateDataShareRequestAnswersSummary(
        DataShareRequestAnswersSummaryModelData dataShareRequestAnswersSummaryModelData)
    {
        ArgumentNullException.ThrowIfNull(dataShareRequestAnswersSummaryModelData);

        return new DataShareRequestAnswersSummary
        {
            DataShareRequestId = dataShareRequestAnswersSummaryModelData.DataShareRequestAnswersSummary_DataShareRequestId,
            RequestId = dataShareRequestAnswersSummaryModelData.DataShareRequestAnswersSummary_RequestId,
            EsdaName = dataShareRequestAnswersSummaryModelData.DataShareRequestAnswersSummary_EsdaName,
            DataShareRequestStatus = DoConvertDataShareRequestStatus(dataShareRequestAnswersSummaryModelData.DataShareRequestAnswersSummary_RequestStatus),
            QuestionsRemainThatRequireAResponse = dataShareRequestAnswersSummaryModelData.DataShareRequestAnswersSummary_QuestionsRemainThatRequireAResponse,
            SummarySections = dataShareRequestAnswersSummaryModelData.DataShareRequestAnswersSummary_SummarySections.Select(DoConvertSummarySection).ToList(),
            SubmissionResponseFromSupplier = dataShareRequestAnswersSummaryModelData.DataShareRequestAnswersSummary_SubmissionResponseFromSupplier,
            CancellationReasonsFromAcquirer = dataShareRequestAnswersSummaryModelData.DataShareRequestAnswersSummary_CancellationReasonsFromAcquirer
        };
    }

    private static DataShareRequestAnswersSummarySection DoConvertSummarySection(
        DataShareRequestAnswersSummarySectionModelData summarySection)
    {
        return new DataShareRequestAnswersSummarySection
        {
            OrderWithinSummary = summarySection.DataShareRequestAnswersSummarySection_OrderWithinSummary,
            SectionHeader = summarySection.DataShareRequestAnswersSummarySection_SectionHeader,
            SummaryQuestionGroups = summarySection.DataShareRequestAnswersSummarySection_QuestionGroups.Select(DoConvertQuestionGroup).ToList()
        };
    }

    private static DataShareRequestAnswersSummaryQuestionGroup DoConvertQuestionGroup(
        DataShareRequestAnswersSummaryQuestionGroupModelData questionGroup)
    {
        return new DataShareRequestAnswersSummaryQuestionGroup
        {
            OrderWithinSection = questionGroup.DataShareRequestAnswersSummaryQuestionGroup_OrderWithinSection,
            MainQuestionSummary = DoConvertQuestion(questionGroup.DataShareRequestAnswersSummaryQuestionGroup_SummaryMainQuestion),
            BackingQuestionSummaries = questionGroup.DataShareRequestAnswersSummaryQuestionGroup_SummaryBackingQuestions.Select(DoConvertQuestion).ToList()
        };
    }

    private static DataShareRequestAnswersSummaryQuestion DoConvertQuestion(
        DataShareRequestAnswersSummaryQuestionModelData question)
    {
        return new DataShareRequestAnswersSummaryQuestion
        {
            QuestionId = question.DataShareRequestAnswersSummaryQuestion_QuestionId,
            QuestionHeader = question.DataShareRequestAnswersSummaryQuestion_QuestionHeader,
            QuestionIsApplicable = question.DataShareRequestAnswersSummaryQuestion_QuestionIsApplicable,
            SummaryQuestionParts = question.DataShareRequestAnswersSummaryQuestion_QuestionParts.Select(DoConvertAnswersSummaryQuestionPart).ToList()
        };
    }

    private static DataShareRequestAnswersSummaryQuestionPart DoConvertAnswersSummaryQuestionPart(
        DataShareRequestAnswersSummaryQuestionPartModelData questionPart)
    {
        return new DataShareRequestAnswersSummaryQuestionPart
        {
            OrderWithinQuestion = questionPart.DataShareRequestAnswersSummaryQuestionPart_OrderWithinQuestion,
            QuestionPartText = questionPart.DataShareRequestAnswersSummaryQuestionPart_QuestionPartText,
            MultipleResponsesAllowed = questionPart.DataShareRequestAnswersSummaryQuestionPart_MultipleResponsesAllowed,
            MultipleResponsesCollectionHeaderIfMultipleResponsesAllowed = questionPart.DataShareRequestAnswersSummaryQuestionPart_MultipleResponsesCollectionDescriptionIfMultipleResponsesAllowed,
            ResponseInputType = DoConvertResponseInputType(questionPart.DataShareRequestAnswersSummaryQuestionPart_ResponseInputType),
            ResponseFormatType = DoConvertResponseFormatType(questionPart.DataShareRequestAnswersSummaryQuestionPart_ResponseFormatType),
            Responses = questionPart.DataShareRequestAnswersSummaryQuestionPart_Responses.Select(DoConvertAnswersSummaryQuestionPartAnswerResponse).ToList()
        };
    }

    private static Dto.Models.Questions.QuestionParts.ResponseFormats.QuestionPartResponseFormatType DoConvertResponseFormatType(
        QuestionPartResponseFormatType questionPartResponseFormatType)
    {
        return questionPartResponseFormatType switch
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
            _ => throw new InvalidEnumValueException("Unknown ResponseFormatType")
        };
    }

    private static DataShareRequestAnswersSummaryQuestionPartAnswerResponse DoConvertAnswersSummaryQuestionPartAnswerResponse(
        DataShareRequestAnswersSummaryQuestionPartResponseModelData questionPartAnswerResponseModelData)
    {
        return new DataShareRequestAnswersSummaryQuestionPartAnswerResponse
        {
            OrderWithinQuestionPartAnswer = questionPartAnswerResponseModelData.DataShareRequestAnswersSummaryQuestionPartResponse_OrderWithinQuestionPart,
            QuestionPartAnswerResponseItem = DoConvertQuestionPartAnswerResponseItem(questionPartAnswerResponseModelData.DataShareRequestAnswersSummaryQuestionPart_ResponseItem)
        };
    }

    private static DataShareRequestAnswersSummaryQuestionPartAnswerResponseItemBase? DoConvertQuestionPartAnswerResponseItem(
        DataShareRequestAnswersSummaryQuestionPartAnswerResponseItemModelData? questionPartAnswerResponseItemModelData)
    {
        if (questionPartAnswerResponseItemModelData == null) return null;

        return questionPartAnswerResponseItemModelData.DataShareRequestAnswersSummaryQuestionPartAnswerResponseItem_ResponseInputType switch
        {
            QuestionPartResponseInputType.FreeForm => DoConvertQuestionPartAnswerResponseItemFreeForm((DataShareRequestAnswersSummaryQuestionPartAnswerResponseItemFreeFormModelData)questionPartAnswerResponseItemModelData),
            QuestionPartResponseInputType.OptionSelection => DoConvertQuestionPartAnswerResponseItemOptionSelection((DataShareRequestAnswersSummaryQuestionPartAnswerResponseItemOptionSelectionModelData)questionPartAnswerResponseItemModelData),
            QuestionPartResponseInputType.None => throw new InvalidOperationException("Answer Response Item has NoInput response type"),
            _ => throw new InvalidEnumValueException("QuestionPartAnswerResponseItem has unknown InputType")
        };
    }

    private static DataShareRequestAnswersSummaryQuestionPartAnswerResponseItemFreeForm DoConvertQuestionPartAnswerResponseItemFreeForm(
        DataShareRequestAnswersSummaryQuestionPartAnswerResponseItemFreeFormModelData questionPartAnswerResponseItemFreeFormModelData)
    {
        return new DataShareRequestAnswersSummaryQuestionPartAnswerResponseItemFreeForm
        {
            AnswerValue = questionPartAnswerResponseItemFreeFormModelData.DataShareRequestAnswersSummaryQuestionPartAnswerResponseItemFreeForm_AnswerValue,
            ValueEntryDeclined = questionPartAnswerResponseItemFreeFormModelData.DataShareRequestAnswersSummaryQuestionPartAnswerResponseItemFreeForm_ValueEntryDeclined
        };
    }

    private static DataShareRequestAnswersSummaryQuestionPartAnswerResponseOptionSelection DoConvertQuestionPartAnswerResponseItemOptionSelection(
        DataShareRequestAnswersSummaryQuestionPartAnswerResponseItemOptionSelectionModelData questionPartAnswerResponseItemOptionSelectionModelData)
    {
        return new DataShareRequestAnswersSummaryQuestionPartAnswerResponseOptionSelection
        {
            SelectedOptions = questionPartAnswerResponseItemOptionSelectionModelData.DataShareRequestAnswersSummaryQuestionPartAnswerResponseItemOptionSelection_SelectedOptions
                .Select(DoConvertOptionSelectionAnswer).ToList()
        };
    }

    private static DataShareRequestAnswersSummaryQuestionPartAnswerResponseOptionSelectionSelectedOption DoConvertOptionSelectionAnswer(
        DataShareRequestAnswersSummaryQuestionPartAnswerResponseOptionSelectionSelectedOptionModelData optionSelectionItemModelData)
    {
        return new DataShareRequestAnswersSummaryQuestionPartAnswerResponseOptionSelectionSelectedOption
        {
            OrderWithinAnswerPart = optionSelectionItemModelData.DataShareRequestAnswersSummaryQuestionPartAnswerItemOptionSelectionItem_OrderWithinAnswerPartResponse,
            SelectionOptionText = optionSelectionItemModelData.DataShareRequestAnswersSummaryQuestionPartAnswerResponseOptionSelectionSelectedOption_SelectionOptionText,
            SupplementaryAnswerText = optionSelectionItemModelData.DataShareRequestAnswersSummaryQuestionPartAnswerResponseOptionSelectionSelectedOption_SupplementaryAnswerText
        };
    }
    #endregion

    #region Create Data Share Request Submission Result
    DataShareRequestSubmissionResult IAcquirerDataShareRequestModelDataFactory.CreateDataShareRequestSubmissionResult(
        DataShareRequestSubmissionResultModelData submissionResultModelData,
        bool notificationsSuccess)
    {
        ArgumentNullException.ThrowIfNull(submissionResultModelData);

        return new DataShareRequestSubmissionResult
        {
            DataShareRequestId = submissionResultModelData.DataShareRequest_Id,
            DataShareRequestRequestId = submissionResultModelData.DataShareRequest_RequestId,
            NotificationSuccess = DoConvertNotificationSuccess(notificationsSuccess)
        };
    }
    #endregion

    #region Create Data Share Request Cancellation Result
    DataShareRequestCancellationResult IAcquirerDataShareRequestModelDataFactory.CreateDataShareRequestCancellationResult(
        Guid dataShareRequestId,
        string reasonsForCancellation,
        bool notificationsSuccess)
    {
        ArgumentNullException.ThrowIfNull(reasonsForCancellation);

        return new DataShareRequestCancellationResult
        {
            DataShareRequestId = dataShareRequestId,
            ReasonsForCancellation = reasonsForCancellation,
            NotificationSuccess = DoConvertNotificationSuccess(notificationsSuccess)
        };
    }
    #endregion

    #region Create Data Share Request Raised For Esda By Acquirer Organisation Summary
    DataShareRequestRaisedForEsdaByAcquirerOrganisationSummary IAcquirerDataShareRequestModelDataFactory.CreateDataShareRequestRaisedForEsdaByAcquirerOrganisationSummary(
        DataShareRequestModelData dataShareRequestModelData,
        AuditLogDataShareRequestStatusChangeModelData auditLogForCreation,
        AuditLogDataShareRequestStatusChangeModelData? auditLogForMostRecentSubmission,
        IUserDetails dataShareRequestAcquirerUserDetails)
    {
        return new DataShareRequestRaisedForEsdaByAcquirerOrganisationSummary
        {
            Id = dataShareRequestModelData.DataShareRequest_Id,
            RequestId = dataShareRequestModelData.DataShareRequest_RequestId,
            Status = DoConvertDataShareRequestStatus(dataShareRequestModelData.DataShareRequest_RequestStatus),
            DateStarted = auditLogForCreation.AuditLogDataShareRequestStatusChange_ChangedAtUtc,
            DateSubmitted = auditLogForMostRecentSubmission?.AuditLogDataShareRequestStatusChange_ChangedAtUtc,
            OriginatingAcquirerContactDetails = new AcquirerContactDetails
            {
                UserName = dataShareRequestAcquirerUserDetails.UserContactDetails.UserName,
                EmailAddress = dataShareRequestAcquirerUserDetails.UserContactDetails.EmailAddress
            }
        };
    }
    #endregion

    private static DataShareRequestStatus DoConvertDataShareRequestStatus(
        DataShareRequestStatusType dataShareRequestStatusType)
    {
        return dataShareRequestStatusType switch
        {
            DataShareRequestStatusType.Draft => DataShareRequestStatus.Draft,
            DataShareRequestStatusType.Submitted => DataShareRequestStatus.Submitted,
            DataShareRequestStatusType.Accepted => DataShareRequestStatus.Accepted,
            DataShareRequestStatusType.Rejected => DataShareRequestStatus.Rejected,
            DataShareRequestStatusType.Cancelled => DataShareRequestStatus.Cancelled,
            DataShareRequestStatusType.Returned => DataShareRequestStatus.Returned,
            DataShareRequestStatusType.InReview => DataShareRequestStatus.InReview,
            DataShareRequestStatusType.Deleted => DataShareRequestStatus.Deleted,
            _ => throw new InvalidEnumValueException("Invalid DataShareRequestStatusType provided")
        };
    }

    private static NotificationSuccess DoConvertNotificationSuccess(
        bool notificationSuccess)
    {
        return notificationSuccess
            ? NotificationSuccess.SentSuccessfully
            : NotificationSuccess.FailedToSend;
    }

    private static Dto.Models.Questions.QuestionParts.ResponseFormats.QuestionPartResponseInputType DoConvertResponseInputType(
        QuestionPartResponseInputType responseInputType)
    {
        return responseInputType switch
        {
            QuestionPartResponseInputType.FreeForm => Dto.Models.Questions.QuestionParts.ResponseFormats.QuestionPartResponseInputType.FreeForm,
            QuestionPartResponseInputType.OptionSelection => Dto.Models.Questions.QuestionParts.ResponseFormats.QuestionPartResponseInputType.OptionSelection,
            QuestionPartResponseInputType.None => Dto.Models.Questions.QuestionParts.ResponseFormats.QuestionPartResponseInputType.None,
            _ => throw new InvalidEnumValueException("Unsupported response input type")
        };
    }
    #endregion

    #region Dto Data To Model Data
    DataShareRequestQuestionAnswerWriteModelData IAcquirerDataShareRequestModelDataFactory.CreateQuestionAnswerWriteData(
        DataShareRequestQuestionAnswer questionAnswer)
    {
        ArgumentNullException.ThrowIfNull(questionAnswer);

        return new DataShareRequestQuestionAnswerWriteModelData
        {
            DataShareRequestId = questionAnswer.DataShareRequestId,
            QuestionId = questionAnswer.QuestionId,
            AnswerParts = questionAnswer.AnswerParts.Select(CreateQuestionAnswerPartWriteData).ToList()
        };

        DataShareRequestQuestionAnswerPartWriteModelData CreateQuestionAnswerPartWriteData(
            DataShareRequestQuestionAnswerPart questionAnswerPart)
        {
            return new DataShareRequestQuestionAnswerPartWriteModelData
            {
                QuestionPartId = questionAnswerPart.QuestionPartId,
                AnswerPartResponses = questionAnswerPart.AnswerPartResponses.Select(CreateQuestionAnswerPartResponseWriteData).ToList()
            };
        }

        DataShareRequestQuestionAnswerPartResponseWriteModelData CreateQuestionAnswerPartResponseWriteData(
            DataShareRequestQuestionAnswerPartResponseBase questionAnswerPartResponse)
        {
            return questionAnswerPartResponse.InputType switch
            {
                Dto.Models.Questions.QuestionParts.ResponseFormats.QuestionPartResponseInputType.FreeForm =>
                    CreateQuestionAnswerPartResponseFreeFormWriteData((DataShareRequestQuestionAnswerPartResponseFreeForm)questionAnswerPartResponse),

                Dto.Models.Questions.QuestionParts.ResponseFormats.QuestionPartResponseInputType.OptionSelection =>
                    CreateQuestionAnswerPartResponseOptionSelectionWriteData((DataShareRequestQuestionAnswerPartResponseSelectionOption)questionAnswerPartResponse),

                Dto.Models.Questions.QuestionParts.ResponseFormats.QuestionPartResponseInputType.None =>
                    throw new InvalidOperationException("QuestionAnswerPartResponse received for a NoInput InputType"),

                _ => throw new InvalidEnumValueException("QuestionAnswerPartResponse has unknown InputType")
            };
        }

        DataShareRequestQuestionAnswerPartResponseFreeFormWriteModelData CreateQuestionAnswerPartResponseFreeFormWriteData(
            DataShareRequestQuestionAnswerPartResponseFreeForm questionAnswerPartResponseFreeForm)
        {
            return new DataShareRequestQuestionAnswerPartResponseFreeFormWriteModelData
            {
                OrderWithinAnswerPart = questionAnswerPartResponseFreeForm.OrderWithinAnswerPart,
                EnteredValue = questionAnswerPartResponseFreeForm.EnteredValue,
                ValueEntryDeclined = questionAnswerPartResponseFreeForm.ValueEntryDeclined
            };
        }

        DataShareRequestQuestionAnswerPartResponseOptionSelectionWriteModelData CreateQuestionAnswerPartResponseOptionSelectionWriteData(
                DataShareRequestQuestionAnswerPartResponseSelectionOption questionAnswerPartResponseSelectionOption)
        {
            return new DataShareRequestQuestionAnswerPartResponseOptionSelectionWriteModelData
            {
                OrderWithinAnswerPart = questionAnswerPartResponseSelectionOption.OrderWithinAnswerPart,
                SelectionOptions = questionAnswerPartResponseSelectionOption.SelectedOptionItems.Select(ConvertSelectionOptionWriteData).ToList()
            };
        }

        DataShareRequestQuestionAnswerPartResponseSelectionOptionWriteModelData ConvertSelectionOptionWriteData(
            DataShareRequestQuestionAnswerPartResponseSelectionOptionItem selectionOptionItem)
        {
            return new DataShareRequestQuestionAnswerPartResponseSelectionOptionWriteModelData
            {
                OptionSelectionId = selectionOptionItem.OptionSelectionItemId,
                SupplementaryQuestionAnswerPart = selectionOptionItem.SupplementaryQuestionAnswerPart == null
                    ? null
                    : CreateQuestionAnswerPartWriteData(selectionOptionItem.SupplementaryQuestionAnswerPart)
            };
        }
    }
    #endregion

    #region General
    DataShareRequestQuestion IAcquirerDataShareRequestModelDataFactory.CreateDataShareRequestQuestionFromAnswer(
        DataShareRequestQuestion dataShareRequestQuestion,
        DataShareRequestQuestionAnswer dataShareRequestQuestionAnswer,
        IEnumerable<SetDataShareRequestQuestionAnswerPartResponseValidationError> questionAnswerPartResponseValidationErrors)
    {
        ArgumentNullException.ThrowIfNull(dataShareRequestQuestion);
        ArgumentNullException.ThrowIfNull(dataShareRequestQuestionAnswer);
        ArgumentNullException.ThrowIfNull(questionAnswerPartResponseValidationErrors);

        var validationErrorsList = questionAnswerPartResponseValidationErrors.ToList();

        return new DataShareRequestQuestion
        {
            DataShareRequestId = dataShareRequestQuestion.DataShareRequestId,
            DataShareRequestRequestId = dataShareRequestQuestion.DataShareRequestRequestId,
            QuestionId = dataShareRequestQuestion.QuestionId,
            IsOptional = dataShareRequestQuestion.IsOptional,
            QuestionParts = dataShareRequestQuestion.QuestionParts.Select(BuildQuestionPart).ToList(),
            QuestionFooter = dataShareRequestQuestion.QuestionFooter
        };

        DataShareRequestQuestionPart BuildQuestionPart(DataShareRequestQuestionPart sourceDataShareRequestQuestionPart)
        {
            var receivedAnswerPart = dataShareRequestQuestionAnswer.AnswerParts.Single(x => 
                x.QuestionPartId == sourceDataShareRequestQuestionPart.QuestionPartQuestion.Id);

            var questionPartAnswer = BuildQuestionPartAnswer(receivedAnswerPart);

            return new DataShareRequestQuestionPart
            {
                QuestionPartQuestion = sourceDataShareRequestQuestionPart.QuestionPartQuestion,
                QuestionPartAnswer = questionPartAnswer
            };
        }

        QuestionPartAnswer BuildQuestionPartAnswer(
            DataShareRequestQuestionAnswerPart receivedAnswerPart)
        {
            return new QuestionPartAnswer
            {
                QuestionPartId = receivedAnswerPart.QuestionPartId,
                AnswerPartResponses = receivedAnswerPart.AnswerPartResponses.Select(response =>
                {
                    var validationErrorsForThisResponse = validationErrorsList.Where(x =>
                        x.QuestionPartId == receivedAnswerPart.QuestionPartId &&
                        x.ResponseOrderWithinAnswerPart == response.OrderWithinAnswerPart);

                    var validationErrors = validationErrorsForThisResponse.SelectMany(x =>
                        x.ValidationErrors);

                    return BuildAnswerPartResponse(response, validationErrors);
                }).ToList()
            };
        }
    }

    private static QuestionPartAnswerResponse BuildAnswerPartResponse(
        DataShareRequestQuestionAnswerPartResponseBase receivedResponse,
        IEnumerable<string> validationErrors)
    {
        return receivedResponse.InputType switch
        {
            Dto.Models.Questions.QuestionParts.ResponseFormats.QuestionPartResponseInputType.FreeForm => BuildFreeFormAnswerPartResponse(),
            Dto.Models.Questions.QuestionParts.ResponseFormats.QuestionPartResponseInputType.OptionSelection => BuildOptionSelectionAnswerPartResponse(),
            _ => throw new InvalidEnumValueException("Response received for unexpected input type")
        };

        QuestionPartAnswerResponse BuildFreeFormAnswerPartResponse()
        {
            var receivedFreeFormResponse = (DataShareRequestQuestionAnswerPartResponseFreeForm) receivedResponse;

            return new QuestionPartAnswerResponse
            {
                InputType = receivedFreeFormResponse.InputType,
                OrderWithinAnswerPart = receivedFreeFormResponse.OrderWithinAnswerPart,
                ResponseItem = new QuestionPartAnswerResponseItemFreeForm
                {
                    EnteredValue = receivedFreeFormResponse.EnteredValue,
                    ValueEntryDeclined = receivedFreeFormResponse.ValueEntryDeclined
                },
                ValidationErrors = validationErrors.ToList()
            };
        }

        QuestionPartAnswerResponse BuildOptionSelectionAnswerPartResponse()
        {
            var receivedOptionSelectionResponse = (DataShareRequestQuestionAnswerPartResponseSelectionOption) receivedResponse;

            return new QuestionPartAnswerResponse
            {
                InputType = receivedOptionSelectionResponse.InputType,
                OrderWithinAnswerPart = receivedOptionSelectionResponse.OrderWithinAnswerPart,
                ResponseItem = new QuestionPartAnswerResponseItemSelectionOption
                {
                    SelectedOptions = receivedOptionSelectionResponse.SelectedOptionItems.Select(BuildSelectionOption).ToList()
                },
                ValidationErrors = validationErrors.ToList()
            };
        }

        QuestionPartAnswerItemSelectionOptionItem BuildSelectionOption(
            DataShareRequestQuestionAnswerPartResponseSelectionOptionItem receivedSelectionOption)
        {
            return new QuestionPartAnswerItemSelectionOptionItem
            {
                OptionSelectionItemId = receivedSelectionOption.OptionSelectionItemId,
                SupplementaryQuestionPartAnswer = BuildSelectionOptionSupplementaryAnswerPart(receivedSelectionOption.SupplementaryQuestionAnswerPart)
            };
        }

        QuestionPartAnswer? BuildSelectionOptionSupplementaryAnswerPart(
            DataShareRequestQuestionAnswerPart? receivedSupplementaryAnswerPart)
        {
            if (receivedSupplementaryAnswerPart == null) return null;

            return new QuestionPartAnswer
            {
                QuestionPartId = receivedSupplementaryAnswerPart.QuestionPartId,
                AnswerPartResponses = receivedSupplementaryAnswerPart.AnswerPartResponses.Select(response =>
                    BuildAnswerPartResponse(response, [])).ToList()
            };
        }
    }
    #endregion
}