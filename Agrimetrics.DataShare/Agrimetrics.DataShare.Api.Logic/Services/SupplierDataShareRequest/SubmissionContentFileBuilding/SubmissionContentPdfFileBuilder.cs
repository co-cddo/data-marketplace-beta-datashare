using Agrimetrics.DataShare.Api.Dto.Models.Supplier.DataShareRequests;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using PdfSharp.Fonts;
using PdfSharp.Pdf;
using PdfSharp.Snippets.Font;
using Agrimetrics.DataShare.Api.Dto.Models.Questions.QuestionParts.ResponseFormats;
using MigraDoc.DocumentObjectModel.Tables;
using Document = MigraDoc.DocumentObjectModel.Document;
using System.Globalization;
using Agrimetrics.DataShare.Api.Logic.Exceptions;

namespace Agrimetrics.DataShare.Api.Logic.Services.SupplierDataShareRequest.SubmissionContentFileBuilding;

internal class SubmissionContentPdfFileBuilder : ISubmissionContentPdfFileBuilder
{
    private const string bulletListLevel1StyleName = "BulletListLevel1";
    private const string bulletListLevel2StyleName = "BulletListLevel2";

    private const int titleRequestIdSize = 12;
    private const int titleHeaderSize = 18;
    private const int paragraphHeaderSize = 12;
    private const int paragraphContentSize = 11;
    private const int answerGroupHeaderSize = 14;
    private const int answerHeaderSize = 12;
    private const int answerContentSize = 11;

    private readonly Unit headerPostParagraphSpacing = Unit.FromCentimeter(0.5);
    private readonly Unit answerRowSpacing = Unit.FromCentimeter(0.25);
    private readonly Unit answerGroupHeaderSpacing = Unit.FromCentimeter(0.25);
    private readonly Unit answerGroupPartHeaderSpacing = Unit.FromCentimeter(0.1);
    private readonly Unit backingQuestionPartSpacing = Unit.FromCentimeter(0.1);
    private readonly Unit selectedOptionSpacing = Unit.FromCentimeter(0.25);
    private readonly Unit multiResponseSpacing = Unit.FromCentimeter(0.1);

    async Task<byte[]> ISubmissionContentPdfFileBuilder.BuildAsync(
        SubmissionInformation submissionInformation,
        SubmissionDetails submissionDetails)
    {
        ArgumentNullException.ThrowIfNull(submissionInformation);
        ArgumentNullException.ThrowIfNull(submissionDetails);

        return await Task.Run(() => DoBuild(submissionInformation, submissionDetails));
    }

    private byte[] DoBuild(
        SubmissionInformation submissionInformation,
        SubmissionDetails submissionDetails)
    {
        GlobalFontSettings.FontResolver ??= new FailsafeFontResolver();

        var document = new Document();

        var bulletListLevel1Style = document.AddStyle(bulletListLevel1StyleName, "Normal");
        bulletListLevel1Style.ParagraphFormat.LeftIndent = "0.5cm";
        bulletListLevel1Style.ParagraphFormat.ListInfo = new ListInfo
        {
            ContinuePreviousList = true,
            ListType = ListType.BulletList1
        };

        var bulletListLevel2Style = document.AddStyle(bulletListLevel2StyleName, "Normal");
        bulletListLevel2Style.ParagraphFormat.LeftIndent = "1.0cm";
        bulletListLevel2Style.ParagraphFormat.ListInfo = new ListInfo
        {
            ContinuePreviousList = true,
            ListType = ListType.BulletList2
        };

        DoAddHeaderSection(document, submissionInformation);
        DoAddContentSection(document, submissionDetails);

        var renderer = new PdfDocumentRenderer
        {
            Document = document,
            PdfDocument = new PdfDocument
            {
                PageLayout = PdfPageLayout.SinglePage,
                ViewerPreferences =
                {
                    FitWindow = true
                }
            }
        };

        renderer.RenderDocument();

        var memoryStream = new MemoryStream();
        renderer.PdfDocument.Save(memoryStream);

        return memoryStream.ToArray();
    }

    #region Header Section
    private void DoAddHeaderSection(
        Document document,
        SubmissionInformation submissionInformation)
    {
        var section = DoAddSection(document);
        
        DoAddHeaderRequestName(section, submissionInformation);
        DoAddHeaderPageTitle(section, submissionInformation);
        DoAddHeaderProjectAims(section, submissionInformation);
        DoAddHeaderDataTypes(section, submissionInformation);
        DoAddHeaderReceivedOn(section, submissionInformation);
        DoAddHeaderNeededBy(section, submissionInformation);
        DoAddHeaderContactInformation(section, submissionInformation);
        DoAddHeaderHighlightedAnswers(document, section, submissionInformation);
    }

    private static void DoAddHeaderRequestName(
        Section section,
        SubmissionInformation submissionInformation)
    {
        var paragraph = section.AddParagraph();
        var requestNameText = paragraph.AddFormattedText($"Request ID: {submissionInformation.DataShareRequestRequestId}");
        requestNameText.Size = titleRequestIdSize;
    }

    private void DoAddHeaderPageTitle(
        Section section,
        SubmissionInformation submissionInformation)
    {
        var paragraph = section.AddParagraph();

        var requestTitleText = paragraph.AddFormattedText($"{submissionInformation.AcquirerOrganisationName} is requesting access to {submissionInformation.EsdaName}", TextFormat.Bold);
        requestTitleText.Size = titleHeaderSize;

        paragraph.Format.SpaceAfter = headerPostParagraphSpacing;
    }

    private void DoAddHeaderProjectAims(
        Section section,
        SubmissionInformation submissionInformation)
    {
        var paragraph = DoCreateHeaderContentParagraph(section, "Project Aims");

        var projectAimText = paragraph.AddFormattedText(submissionInformation.ProjectAims);
        projectAimText.Size = paragraphContentSize;
    }

    private void DoAddHeaderDataTypes(
        Section section,
        SubmissionInformation submissionInformation)
    {
        var paragraph = DoCreateHeaderContentParagraph(section, "Data Types");

        foreach (var indexedDataType in submissionInformation.DataTypes.Select((value, index) => new { Value = value, Index = index }))
        {
            if (indexedDataType.Index > 0)
            {
                paragraph.AddLineBreak();
            }

            var dataTypeText = paragraph.AddFormattedText(indexedDataType.Value);
            dataTypeText.Size = paragraphContentSize;
            paragraph.AddLineBreak();
        }
    }

    private void DoAddHeaderReceivedOn(
        Section section,
        SubmissionInformation submissionInformation)
    {
        var paragraph = DoCreateHeaderContentParagraph(section, "Received");

        var receivedOnText = paragraph.AddFormattedText(PrettifyDate(submissionInformation.SubmittedOn));
        receivedOnText.Size = paragraphContentSize;
    }

    private void DoAddHeaderNeededBy(
        Section section,
        SubmissionInformation submissionInformation)
    {
        var paragraph = DoCreateHeaderContentParagraph(section, "Needed By");

        var neededByText = paragraph.AddFormattedText(PrettifyOptionalDate(submissionInformation.WhenNeededBy));
        neededByText.Size = paragraphContentSize;
    }

    private void DoAddHeaderContactInformation(
        Section section,
        SubmissionInformation submissionInformation)
    {
        var paragraph = DoCreateHeaderContentParagraph(section, "Contact");

        var acquirerEmailAddressText = paragraph.AddFormattedText(submissionInformation.AcquirerEmailAddress);
        acquirerEmailAddressText.Size = paragraphContentSize;
    }

    private void DoAddHeaderHighlightedAnswers(
        Document document,
        Section section,
        SubmissionInformation submissionInformation)
    {
        DoCreateHeaderContentParagraph(section, "Highlighted Answers", includePostParagraphSpacing: false);

        foreach (var highlightedAnswer in submissionInformation.AnswerHighlights)
        {
            document.LastSection.AddParagraph(highlightedAnswer, bulletListLevel1StyleName);
        }
    }

    private Paragraph DoCreateHeaderContentParagraph(
        Section section,
        string headerText,
        bool includePostParagraphSpacing = true)
    {
        var paragraph = section.AddParagraph();
        var headerTextText = paragraph.AddFormattedText(headerText, TextFormat.Bold);
        headerTextText.Size = paragraphHeaderSize;
        paragraph.AddLineBreak();

        if (includePostParagraphSpacing)
        {
            paragraph.Format.SpaceAfter = headerPostParagraphSpacing;
        }

        return paragraph;
    }
    #endregion

    #region Content Section
    private void DoAddContentSection(
        Document document,
        SubmissionDetails submissionDetails)
    {
        var section = DoAddSection(document);

        DoAddContentRequestName(section, submissionDetails);
        DoAddContentPageTitle(section, submissionDetails);
        DoAddContentAnswers(section, submissionDetails);
    }

    private static void DoAddContentRequestName(
        Section section,
        SubmissionDetails submissionDetails)
    {
        var paragraph = section.AddParagraph();
        var requestNameText = paragraph.AddFormattedText($"Request ID: {submissionDetails.DataShareRequestRequestId}");
        requestNameText.Size = titleRequestIdSize;
    }

    private void DoAddContentPageTitle(
        Section section,
        SubmissionDetails submissionDetails)
    {
        var paragraph = section.AddParagraph();

        var requestTitleText = paragraph.AddFormattedText($"{submissionDetails.AcquirerOrganisationName} is requesting access to {submissionDetails.EsdaName}", TextFormat.Bold);
        requestTitleText.Size = titleHeaderSize;

        paragraph.Format.SpaceAfter = answerRowSpacing;
    }

    private void DoAddContentAnswers(
        Section section,
        SubmissionDetails submissionDetails)
    {
        var totalWidth = section.PageSetup.PageWidth - section.PageSetup.LeftMargin - section.PageSetup.RightMargin;
        var answersTable = section.AddTable();
        answersTable.KeepTogether = false;
        answersTable.Borders.Bottom.Width = 0.25;

        var column = answersTable.AddColumn();
        column.Width = totalWidth;

        var questionSetSectionsOrderedBySectionNumber = submissionDetails.Sections.OrderBy(x => x.SectionNumber);

        var orderedAnswerGroups = questionSetSectionsOrderedBySectionNumber
            .SelectMany(orderedSection => orderedSection.AnswerGroups.OrderBy(answerGroup => answerGroup.OrderWithinSubmission));

        DoAddContentAnswerGroups(answersTable, orderedAnswerGroups);
    }

    private void DoAddContentAnswerGroups(
        Table answersTable,
        IEnumerable<SubmissionDetailsAnswerGroup> answerGroups)
    {
        foreach (var answerGroup in answerGroups)
        {
            DoAddContentAnswerGroup(answersTable, answerGroup);
        }
    }

    private void DoAddContentAnswerGroup(
        Table answersTable,
        SubmissionDetailsAnswerGroup answerGroup)
    {
        var row = answersTable.AddRow();

        row.TopPadding = answerRowSpacing;
        row.BottomPadding = answerRowSpacing;

        var cell = row.Cells[0];

        var answerGroupHeaderParagraph = cell.AddParagraph();
        var answerGroupHeaderText = answerGroupHeaderParagraph.AddFormattedText(answerGroup.MainQuestionHeader, TextFormat.Bold);
        answerGroupHeaderText.Size = answerGroupHeaderSize;
        answerGroupHeaderParagraph.Format.SpaceAfter = answerGroupHeaderSpacing;

        DoAddContentMainEntryParts(cell, answerGroup.MainEntry);

        DoAddContentBackingEntryParts(cell, answerGroup);
    }

    private void DoAddContentMainEntryParts(
        Cell cell,
        SubmissionAnswerGroupEntry mainEntry)
    {
        var orderedMainEntryParts = mainEntry.EntryParts.OrderBy(entryPart => entryPart.OrderWithinGroupEntry);

        foreach (var mainEntryPart in orderedMainEntryParts)
        {
            DoAddContentMainEntryPart(cell, mainEntryPart);
        }
    }

    private void DoAddContentMainEntryPart(
        Cell cell,
        SubmissionAnswerGroupEntryPart mainEntryPart)
    {
        var mainEntryPartQuestionParagraph = cell.AddParagraph();
        var answerGroupQuestionText = mainEntryPartQuestionParagraph.AddFormattedText(mainEntryPart.QuestionPartText, TextFormat.Bold);
        answerGroupQuestionText.Size = answerHeaderSize;
        if (mainEntryPart.OrderWithinGroupEntry > 0)
        {
            mainEntryPartQuestionParagraph.Format.SpaceBefore = answerGroupPartHeaderSpacing;
        }
        mainEntryPartQuestionParagraph.Format.SpaceAfter = answerGroupPartHeaderSpacing;

        foreach (var response in mainEntryPart.Responses)
        {
            DoAddContentMainEntryPartResponse(cell, mainEntryPart.ResponseInputType, mainEntryPart.ResponseFormatType, response);
        }
    }

    private void DoAddContentMainEntryPartResponse(
        Cell cell,
        QuestionPartResponseInputType responseInputType,
        QuestionPartResponseFormatType responseFormatType,
        SubmissionDetailsAnswerResponse mainEntryPartResponse)
    {
        switch (responseInputType)
        {
            case QuestionPartResponseInputType.FreeForm:
            {
                DoAddContentFreeFormResponseItem(cell, (SubmissionDetailsAnswerResponseItemFreeForm)mainEntryPartResponse.ResponseItem, responseFormatType);
            } break;

            case QuestionPartResponseInputType.OptionSelection:
            {
                DoAddContentOptionSelectionResponseItem(cell, (SubmissionDetailsAnswerResponseItemOptionSelection)mainEntryPartResponse.ResponseItem);
            } break;

            default: throw new InvalidEnumValueException("Unhandled response type for answer");
        }
    }

    private static void DoAddContentFreeFormResponseItem(
        Cell cell,
        SubmissionDetailsAnswerResponseItemFreeForm freeFormResponseItem,
        QuestionPartResponseFormatType responseFormatType)
    {
        var responseText = DoFormatContentFreeFormResponseText(freeFormResponseItem, responseFormatType);

        var freeFormParagraph = cell.AddParagraph();

        var responseTextText = freeFormParagraph.AddFormattedText(responseText);
        responseTextText.Size = answerContentSize;
    }

    private static string DoFormatContentFreeFormResponseText(
        SubmissionDetailsAnswerResponseItemFreeForm freeFormResponseItem,
        QuestionPartResponseFormatType responseFormatType)
    {
        if (responseFormatType == QuestionPartResponseFormatType.Date)
        {
            var dateString = freeFormResponseItem.AnswerValue;

            if (string.IsNullOrWhiteSpace(dateString)) return "N/A";

            var date = DateTime.ParseExact(dateString, "yyyyMMdd", CultureInfo.InvariantCulture);

            return PrettifyDate(date);
        }

        return freeFormResponseItem.AnswerValue;
    }

    private void DoAddContentOptionSelectionResponseItem(
        Cell cell,
        SubmissionDetailsAnswerResponseItemOptionSelection optionSelectionResponseItem)
    {
        var indexedSelectedOptions = optionSelectionResponseItem.SelectedOptions.OrderBy(x => x.OrderWithinSelectedOptions)
            .Select((value, index) => new { Value = value, Index = index });

        foreach (var indexedSelectedOption in indexedSelectedOptions)
        {
            var selectedOptionParagraph = cell.AddParagraph();
            if (indexedSelectedOption.Index > 0)
            {
                selectedOptionParagraph.Format.SpaceBefore = selectedOptionSpacing;
            }

            var selectedOption = indexedSelectedOption.Value;
            var selectedOptionText = selectedOptionParagraph.AddFormattedText(selectedOption.SelectionOptionText);
            selectedOptionText.Size = answerContentSize;

            if (!string.IsNullOrWhiteSpace(selectedOption.SupplementaryAnswerText))
            {
                var supplementaryAnswerParagraph = cell.AddParagraph(selectedOption.SupplementaryAnswerText!);
                supplementaryAnswerParagraph.Style = bulletListLevel1StyleName;
            }
        }
    }

    private void DoAddContentBackingEntryParts(
        Cell cell,
        SubmissionDetailsAnswerGroup answerGroup)
    {
        var orderedBackingEntryParts = answerGroup.BackingEntries
            .SelectMany(backingEntry => backingEntry.EntryParts.OrderBy(entryPart => entryPart.OrderWithinGroupEntry));

        foreach (var backingEntryPart in orderedBackingEntryParts)
        {
            DoAddContentBackingEntryPartResponses(cell, backingEntryPart);
        }
    }

    private void DoAddContentBackingEntryPartResponses(
        Cell cell,
        SubmissionAnswerGroupEntryPart backingEntryPart)
    {
        if (!backingEntryPart.MultipleResponsesAllowed || backingEntryPart.ResponseFormatType != QuestionPartResponseFormatType.Text) return;

        var backingEntryPartParagraph = cell.AddParagraph();
        backingEntryPartParagraph.Format.SpaceBefore = backingQuestionPartSpacing;

        var collectionDescriptionText = backingEntryPartParagraph.AddFormattedText($"{PrettifyString(backingEntryPart.CollectionDescriptionIfMultipleResponsesAllowed!)}:");
        collectionDescriptionText.Size = answerContentSize;
        backingEntryPartParagraph.AddLineBreak();

        var orderedBackingEntryPartResponses = backingEntryPart.Responses.OrderBy(x => x.OrderWithinAnswer);

        foreach (var response in orderedBackingEntryPartResponses)
        {
            DoAddContentBackingEntryPartResponse(cell, response);
        }
    }

    private void DoAddContentBackingEntryPartResponse(
        Cell cell,
        SubmissionDetailsAnswerResponse response)
    {
        var backingEntryResponseItem = (SubmissionDetailsAnswerResponseItemFreeForm)response.ResponseItem;

        var backingEntryResponseParagraph = cell.AddParagraph(backingEntryResponseItem.AnswerValue);
        backingEntryResponseParagraph.Style = bulletListLevel1StyleName;
        if (response.OrderWithinAnswer > 0)
        {
            backingEntryResponseParagraph.Format.SpaceBefore = multiResponseSpacing;
        }
    }
    #endregion

    private static Section DoAddSection(Document document)
    {
        var section = document.AddSection();

        section.PageSetup = document.DefaultPageSetup.Clone();
        section.PageSetup.PageFormat = PageFormat.A4;

        return section;
    }

    private static string PrettifyOptionalDate(DateTime? date)
    {
        return date == null
            ? "N/A"
            : PrettifyDate(date.Value);
    }

    private static string PrettifyDate(DateTime date)
    {
        var dayOfTheMonth = date.Day % 100;

        var daySuffix = GetDayOfTheMonthSuffix();

        var formattedMonthAndYear = date.ToString("MMMM yyyy");

        return $"{dayOfTheMonth}{daySuffix} {formattedMonthAndYear}";

        string GetDayOfTheMonthSuffix()
        {
            // 10th to 20th are a special case as they all end in 'th'
            if (dayOfTheMonth is > 10 and < 20) return "th";

            var lastDigit = dayOfTheMonth % 10;

            return lastDigit switch
            {
                1 => "st",
                2 => "nd",
                3 => "rd",
                _ => "th"
            };
        }
    }

    private static string PrettifyString(string input)
    {
        return !string.IsNullOrWhiteSpace(input)
            ? string.Join(" ", input.Split('-').Select(token => char.ToUpper(token.First()) + token[1..]))
            : string.Empty;
    }
}