using Agrimetrics.DataShare.Api.Dto.Models.Questions.QuestionParts.ResponseFormats;
using Agrimetrics.DataShare.Api.Dto.Models.Supplier.DataShareRequests;
using AutoFixture.AutoMoq;
using AutoFixture;
using NUnit.Framework;
using Agrimetrics.DataShare.Api.Logic.Services.SupplierDataShareRequest.SubmissionContentFileBuilding;
using Agrimetrics.DataShare.Api.Logic.Exceptions;

namespace Agrimetrics.DataShare.Api.Logic.Test.Services.SupplierDataShareRequest.SubmissionContentFileBuilding;

[TestFixture]
public class SubmissionContentPdfFileBuilderTests
{
    #region BuildAsync() Tests
    [Test]
    public void GivenANullSubmissionInformation_WhenIBuildAsync_ThenAnArgumentNullExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        Assert.That(() => testItems.SubmissionContentPdfFileBuilder.BuildAsync(
            null!,
            testItems.Fixture.Create<SubmissionDetails>()),
            Throws.ArgumentNullException.With.Property("ParamName").EqualTo("submissionInformation"));
    }

    [Test]
    public void GivenANullSubmissionDetails_WhenIBuildAsync_ThenAnArgumentNullExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        Assert.That(() => testItems.SubmissionContentPdfFileBuilder.BuildAsync(
                testItems.Fixture.Create<SubmissionInformation>(),
                null!),
            Throws.ArgumentNullException.With.Property("ParamName").EqualTo("submissionDetails"));
    }

    [Test]
    public async Task GivenSubmissionDetailsAndInformation_WhenIBuildAsync_ThenPdfContentIsBuild()
    {
        var testItems = CreateTestItems();

        var testAnswerGroups = testItems.Fixture
            .Build<SubmissionDetailsAnswerGroup>()
            .Without(x => x.MainEntry)
            .Without(x => x.BackingEntries)
            .CreateMany(2)
            .Select((item, index) =>
            {
                item.MainEntry = index == 0
                    ? CreateTestMainEntryFreeFormAnswerGroupEntry()
                    : CreateTestMainEntryOptionSelectionAnswerGroupEntry();

                item.BackingEntries = index == 0
                    ? CreateTestBackingFreeFormAnswerGroupEntries().ToList()
                    : CreateTestBackingOptionSelectionAnswerGroupEntries().ToList();

                return item;
            })
            .ToList();

        var testSection = testItems.Fixture
            .Build<SubmissionDetailsSection>()
            .With(x => x.AnswerGroups, testAnswerGroups)
            .Create();

        var testSubmissionDetails = testItems.Fixture
            .Build<SubmissionDetails>()
            .With(x => x.Sections, [testSection])
            .Create();

        var result = await testItems.SubmissionContentPdfFileBuilder.BuildAsync(
            testItems.Fixture.Create<SubmissionInformation>(),
            testSubmissionDetails);

        Assert.That(result, Is.Not.Null);

        SubmissionAnswerGroupEntry CreateTestMainEntryFreeFormAnswerGroupEntry()
        {
            var freeFormTextMainEntryResponseItem = testItems.Fixture.Create<SubmissionDetailsAnswerResponseItemFreeForm>();

            var freeFormTextMainEntryResponse = testItems.Fixture
                .Build<SubmissionDetailsAnswerResponse>()
                .With(x => x.ResponseItem, freeFormTextMainEntryResponseItem)
                .Create();

            var freeFormTextMainEntryPart = testItems.Fixture
                .Build<SubmissionAnswerGroupEntryPart>()
                .With(x => x.OrderWithinGroupEntry, 1)
                .With(x => x.ResponseInputType, QuestionPartResponseInputType.FreeForm)
                .With(x => x.ResponseFormatType, QuestionPartResponseFormatType.Text)
                .With(x => x.Responses, [freeFormTextMainEntryResponse])
                .Create();


            var freeFormDateMainEntryResponses = new List<SubmissionDetailsAnswerResponse>();

            foreach (var dayOfTheMonth in Enumerable.Range(1, 31))
            {
                var freeFormDateMainEntryResponseItem = testItems.Fixture
                    .Build<SubmissionDetailsAnswerResponseItemFreeForm>()
                    .With(x => x.AnswerValue, $"202512{dayOfTheMonth:D2}")
                    .Create();

                var freeFormDateMainEntryResponse = testItems.Fixture
                    .Build<SubmissionDetailsAnswerResponse>()
                    .With(x => x.ResponseItem, freeFormDateMainEntryResponseItem)
                    .Create();

                freeFormDateMainEntryResponses.Add(freeFormDateMainEntryResponse);
            }
            
            var freeFormDateEmptyMainEntryResponseItem = testItems.Fixture
                .Build<SubmissionDetailsAnswerResponseItemFreeForm>()
                .With(x => x.AnswerValue, string.Empty)
                .Create();

            var freeFormDateEmptyMainEntryResponse = testItems.Fixture
                .Build<SubmissionDetailsAnswerResponse>()
                .With(x => x.ResponseItem, freeFormDateEmptyMainEntryResponseItem)
                .Create();

            freeFormDateMainEntryResponses.Add(freeFormDateEmptyMainEntryResponse);

            var freeFormDateMainEntryPart = testItems.Fixture
                .Build<SubmissionAnswerGroupEntryPart>()
                .With(x => x.OrderWithinGroupEntry, 1)
                .With(x => x.ResponseInputType, QuestionPartResponseInputType.FreeForm)
                .With(x => x.ResponseFormatType, QuestionPartResponseFormatType.Date)
                .With(x => x.Responses, freeFormDateMainEntryResponses)
                .Create();

            var freeFormMainAnswerGroupEntry = testItems.Fixture
                .Build<SubmissionAnswerGroupEntry>()
                .With(x => x.EntryParts, [freeFormTextMainEntryPart, freeFormDateMainEntryPart])
                .Create();

            return freeFormMainAnswerGroupEntry;
        }

        IEnumerable<SubmissionAnswerGroupEntry> CreateTestBackingFreeFormAnswerGroupEntries()
        {
            var freeFormBackingEntryResponseItem = testItems.Fixture.Create<SubmissionDetailsAnswerResponseItemFreeForm>();

            var freeFormBackingEntryResponse = testItems.Fixture
                .Build<SubmissionDetailsAnswerResponse>()
                .With(x => x.ResponseItem, freeFormBackingEntryResponseItem)
                .Create();

            var freeFormBackingEntryPart = testItems.Fixture
                .Build<SubmissionAnswerGroupEntryPart>()
                .With(x => x.OrderWithinGroupEntry, 1)
                .With(x => x.ResponseInputType, QuestionPartResponseInputType.FreeForm)
                .With(x => x.ResponseFormatType, QuestionPartResponseFormatType.Text)
                .With(x => x.Responses, [freeFormBackingEntryResponse])
                .With(x => x.MultipleResponsesAllowed, true)
                .With(x => x.CollectionDescriptionIfMultipleResponsesAllowed, "some-test-description")
                .Create();

            var freeFormBackingAnswerGroupEntry = testItems.Fixture
                .Build<SubmissionAnswerGroupEntry>()
                .With(x => x.EntryParts, [freeFormBackingEntryPart])
                .Create();

            return [freeFormBackingAnswerGroupEntry];
        }

        SubmissionAnswerGroupEntry CreateTestMainEntryOptionSelectionAnswerGroupEntry()
        {
            var optionSelectionMainEntryResponseItem = testItems.Fixture.Create<SubmissionDetailsAnswerResponseItemOptionSelection>();

            var optionSelectionMainEntryResponse = testItems.Fixture
                .Build<SubmissionDetailsAnswerResponse>()
                .With(x => x.ResponseItem, optionSelectionMainEntryResponseItem)
                .Create();

            var optionSelectionMainEntryPart = testItems.Fixture
                .Build<SubmissionAnswerGroupEntryPart>()
                .With(x => x.OrderWithinGroupEntry, 1)
                .With(x => x.ResponseInputType, QuestionPartResponseInputType.OptionSelection)
                .With(x => x.ResponseFormatType, QuestionPartResponseFormatType.SelectSingle)
                .With(x => x.Responses, [optionSelectionMainEntryResponse])
                .Create();

            var optionSelectionMainAnswerGroupEntry = testItems.Fixture
                .Build<SubmissionAnswerGroupEntry>()
                .With(x => x.EntryParts, [optionSelectionMainEntryPart])
                .Create();

            return optionSelectionMainAnswerGroupEntry;
        }

        IEnumerable<SubmissionAnswerGroupEntry> CreateTestBackingOptionSelectionAnswerGroupEntries()
        {
            var optionSelectionBackingEntryResponseItem = testItems.Fixture.Create<SubmissionDetailsAnswerResponseItemOptionSelection>();

            var optionSelectionBackingEntryResponse = testItems.Fixture
                .Build<SubmissionDetailsAnswerResponse>()
                .With(x => x.ResponseItem, optionSelectionBackingEntryResponseItem)
                .Create();

            var optionSelectionBackingEntryPart = testItems.Fixture
                .Build<SubmissionAnswerGroupEntryPart>()
                .With(x => x.OrderWithinGroupEntry, 1)
                .With(x => x.ResponseInputType, QuestionPartResponseInputType.OptionSelection)
                .With(x => x.ResponseFormatType, QuestionPartResponseFormatType.SelectSingle)
                .With(x => x.Responses, [optionSelectionBackingEntryResponse])
                .With(x => x.MultipleResponsesAllowed, false)
                .Create();

            var optionSelectionBackingAnswerGroupEntry = testItems.Fixture
                .Build<SubmissionAnswerGroupEntry>()
                .With(x => x.EntryParts, [optionSelectionBackingEntryPart])
                .Create();

            return [optionSelectionBackingAnswerGroupEntry];
        }
    }

    [Test]
    public void GivenSubmissionDetailsWithInvalidResponseInputType_WhenIBuildAsync_ThenAnInvalidEnumValueExceptionIsThrown()
    {
        var testItems = CreateTestItems();

        var invalidResponseInputType = (QuestionPartResponseInputType) Enum.GetValues<QuestionPartResponseInputType>().Cast<int>().Max() + 1;

        var testEntryPart = testItems.Fixture
            .Build<SubmissionAnswerGroupEntryPart>()
            .With(x => x.ResponseInputType, invalidResponseInputType)
            .Create();

        var testAnswerGroupEntry = testItems.Fixture
            .Build<SubmissionAnswerGroupEntry>()
            .With(x => x.EntryParts, [testEntryPart])
            .Create();

        var testAnswerGroups = testItems.Fixture
            .Build<SubmissionDetailsAnswerGroup>()
            .With(x => x.MainEntry, testAnswerGroupEntry)
            .With(x => x.BackingEntries, [])
            .CreateMany(1)
            .ToList();

        var testSection = testItems.Fixture
            .Build<SubmissionDetailsSection>()
            .With(x => x.AnswerGroups, testAnswerGroups)
            .Create();

        var testSubmissionDetails = testItems.Fixture
            .Build<SubmissionDetails>()
            .With(x => x.Sections, [testSection])
            .Create();

        Assert.That(async () => await testItems.SubmissionContentPdfFileBuilder.BuildAsync(
                testItems.Fixture.Create<SubmissionInformation>(),
                testSubmissionDetails),
            Throws.TypeOf<InvalidEnumValueException>().With.Message.EqualTo("Unhandled response type for answer"));
    }
    #endregion

    #region Test Item Creation
    private static TestItems CreateTestItems()
    {
        var fixture = new Fixture().Customize(new AutoMoqCustomization());

        var submissionContentPdfFileBuilder = new SubmissionContentPdfFileBuilder();

        return new TestItems(
            fixture,
            submissionContentPdfFileBuilder);
    }

    private class TestItems(
        IFixture fixture,
        ISubmissionContentPdfFileBuilder submissionContentPdfFileBuilder)
    {
        public IFixture Fixture { get; } = fixture;
        public ISubmissionContentPdfFileBuilder SubmissionContentPdfFileBuilder { get; } = submissionContentPdfFileBuilder;
    }
    #endregion
}
