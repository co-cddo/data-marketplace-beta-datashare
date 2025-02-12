using System.ComponentModel;
using Agrimetrics.DataShare.Api.Db.DbAccess;
using Agrimetrics.DataShare.Api.Dto.Models.Questions.QuestionParts.ResponseFormats;
using Agrimetrics.DataShare.Api.Logic.Exceptions;
using Agrimetrics.DataShare.Api.Logic.ModelData.Questions.KeyQuestionParts;
using Microsoft.Extensions.Logging;

namespace Agrimetrics.DataShare.Api.Logic.Repositories.KeyQuestionPartAnswers;

internal class KeyQuestionPartAnswersRepository(
    ILogger<KeyQuestionPartAnswersRepository> logger,
    IDatabaseChannelCreation databaseChannelCreation,
    IDatabaseCommandRunner databaseCommandRunner,
    IKeyQuestionPartAnswersSqlQueries keyQuestionPartAnswersSqlQueries) : IKeyQuestionPartAnswersRepository
{
    async Task<IEnumerable<string>> IKeyQuestionPartAnswersRepository.GetKeyQuestionPartResponsesAsync(
        Guid dataShareRequestId,
        QuestionPartKeyType questionPartKey)
    {
        if (!Enum.IsDefined(typeof(QuestionPartKeyType), questionPartKey))
            throw new InvalidEnumArgumentException(nameof(questionPartKey), (int) questionPartKey, typeof(QuestionPartKeyType));

        await using var databaseChannel = await databaseChannelCreation.CreateAsync();

        try
        {
            var keyQuestionPartAnswerModelDataFlattened = (await databaseCommandRunner.DbQueryAsync<
                KeyQuestionPartAnswerModelData,
                KeyQuestionPartAnswerResponseModelData?,
                KeyQuestionPartAnswerModelData>(
                databaseChannel.Connection,
                databaseChannel.Transaction!,
                keyQuestionPartAnswersSqlQueries.GetKeyQuestionPartModelData,
                (answer, response) =>
                {
                    if (response != null)
                    {
                        answer.KeyQuestionPartAnswer_AnswerPartResponses.Add(response);
                    }

                    return answer;
                },
                splitOn: nameof(KeyQuestionPartAnswerResponseModelData.KeyQuestionPart_AnswerPartResponseId),
                new
                {
                    DataShareRequestId = dataShareRequestId,
                    QuestionPartKey = questionPartKey.ToString()
                }).ConfigureAwait(false)).ToList();

            var keyQuestionPartAnswerModelData = BuildGroupedKeyQuestionPartAnswerModelData(keyQuestionPartAnswerModelDataFlattened);

            if (!keyQuestionPartAnswerModelData.KeyQuestionPartAnswer_AnswerPartResponses.Any())
            {
                logger.LogError("No Response available for key question part");
                return Enumerable.Empty<string>();
            }

            if (keyQuestionPartAnswerModelData.KeyQuestionPartAnswer_ResponseFormatType ==
                QuestionPartResponseInputType.OptionSelection)
            {
                keyQuestionPartAnswerModelData.KeyQuestionPartAnswer_AnswerPartResponses = keyQuestionPartAnswerModelData.KeyQuestionPartAnswer_AnswerPartResponses.Take(1).ToList();
            }

            foreach (var answerPartResponse in keyQuestionPartAnswerModelData.KeyQuestionPartAnswer_AnswerPartResponses)
            {
                answerPartResponse.KeyQuestionPartAnswerResponse_ResponseItem = keyQuestionPartAnswerModelData.KeyQuestionPartAnswer_ResponseFormatType switch
                {
                    QuestionPartResponseInputType.FreeForm => await DoGetResponseItemForFreeFormResponseAsync(databaseChannel, answerPartResponse.KeyQuestionPart_AnswerPartResponseId),
                    QuestionPartResponseInputType.OptionSelection => await DoGetResponseItemForOptionSelectionResponseAsync(databaseChannel, answerPartResponse.KeyQuestionPart_AnswerPartResponseId),
                    _ => throw new InvalidEnumValueException("Cannot report an answer response for a no input question")
                };
            }

            var allResponseStrings =
                keyQuestionPartAnswerModelData.KeyQuestionPartAnswer_ResponseFormatType == QuestionPartResponseInputType.FreeForm
                    ? BuildFreeFormResponseStrings(keyQuestionPartAnswerModelData.KeyQuestionPartAnswer_AnswerPartResponses)
                    : BuildOptionSelectionResponseStrings(keyQuestionPartAnswerModelData.KeyQuestionPartAnswer_AnswerPartResponses);

            return allResponseStrings;
        }
        catch (Exception ex)
        {
            await databaseChannel.RollbackTransactionAsync();

            const string errorMessage = "Failed to GetKeyQuestionPartAnswer from database";

            logger.LogError(ex, errorMessage);

            throw new DatabaseAccessGeneralException(errorMessage, ex);
        }
        finally
        {
            await databaseChannel.DisposeAsync().ConfigureAwait(false);
        }
    }

    private static KeyQuestionPartAnswerModelData BuildGroupedKeyQuestionPartAnswerModelData(
        IReadOnlyList<KeyQuestionPartAnswerModelData> keyQuestionPartAnswerModelDataFlattened)
    {
        var firstEntryInGroup = keyQuestionPartAnswerModelDataFlattened[0];

        firstEntryInGroup.KeyQuestionPartAnswer_AnswerPartResponses = keyQuestionPartAnswerModelDataFlattened
            .SelectMany(x => x.KeyQuestionPartAnswer_AnswerPartResponses).ToList();

        return firstEntryInGroup;
    }

    private async Task<KeyQuestionPartAnswerResponseItemFreeFormModelData> DoGetResponseItemForFreeFormResponseAsync(
        IDatabaseChannel databaseChannel,
        Guid answerPartResponseId)
    {
        return await databaseCommandRunner.DbQuerySingleAsync<KeyQuestionPartAnswerResponseItemFreeFormModelData>(
                databaseChannel.Connection,
                databaseChannel.Transaction!,
                keyQuestionPartAnswersSqlQueries.GetKeyQuestionPartFreeFormResponseItemModelDatas,
                new
                {
                    AnswerPartResponseId = answerPartResponseId
                })
            .ConfigureAwait(false);
    }

    private async Task<KeyQuestionPartAnswerResponseItemOptionSelectionModelData> DoGetResponseItemForOptionSelectionResponseAsync(
        IDatabaseChannel databaseChannel,
        Guid answerPartResponseId)
    {
        var responseItemOptionSelectionModelDatasFlattened =
            (await databaseCommandRunner.DbQueryAsync<
                    KeyQuestionPartAnswerResponseItemOptionSelectionModelData,
                    KeyQuestionPartAnswerResponseItemSelectedOptionModelData,
                    KeyQuestionPartAnswerResponseItemOptionSelectionModelData>(
                    databaseChannel.Connection,
                    databaseChannel.Transaction!,
                    keyQuestionPartAnswersSqlQueries.GetKeyQuestionPartOptionSelectionResponseItemModelDatas,
                    (responseItem, selectedOption) =>
                    {
                        responseItem.KeyQuestionPartAnswerResponseItemOptionSelection_SelectedOptions.Add(selectedOption);
                        return responseItem;
                    },
                    nameof(KeyQuestionPartAnswerResponseItemSelectedOptionModelData.KeyQuestionPartAnswerResponseItemSelectedOption_SelectedOptionValue),
                    new
                    {
                        AnswerPartResponseId = answerPartResponseId
                    })
                .ConfigureAwait(false));

        return BuildGroupedData();

        KeyQuestionPartAnswerResponseItemOptionSelectionModelData BuildGroupedData()
        {
            // There is an issue with the shape of responses for option selection questions going to the DB, so we end up with multiple
            // responses, each containing all the selected options.  So we just work with the first one here
            var responseItemOptionSelectionModelDatasFlattenedGroupedByResponseItemId = responseItemOptionSelectionModelDatasFlattened
                .GroupBy(x => x.KeyQuestionPartAnswerResponseItemOptionSelection_ResponseItemId).ToList();

            var responseItemOptionSelectionModelDatas =
                responseItemOptionSelectionModelDatasFlattenedGroupedByResponseItemId.Select(x => x.First())
                    .ToList();

            var firstEntryInGroup = responseItemOptionSelectionModelDatas.First();

            firstEntryInGroup.KeyQuestionPartAnswerResponseItemOptionSelection_SelectedOptions =
                responseItemOptionSelectionModelDatas.SelectMany(x => x.KeyQuestionPartAnswerResponseItemOptionSelection_SelectedOptions).ToList();

            return firstEntryInGroup;
        }
    }

    private static IEnumerable<string> BuildFreeFormResponseStrings(
        IEnumerable<KeyQuestionPartAnswerResponseModelData> keyQuestionPartAnswerAnswerPartResponses)
    {
        var freeFormResponseItems = keyQuestionPartAnswerAnswerPartResponses
            .Select(responseItem => responseItem.KeyQuestionPartAnswerResponse_ResponseItem)
            .Cast<KeyQuestionPartAnswerResponseItemFreeFormModelData>();

        return freeFormResponseItems.Select(x => x.KeyQuestionPartAnswerResponseItemFreeForm_EnteredValue);
    }

    private static IEnumerable<string> BuildOptionSelectionResponseStrings(
        IEnumerable<KeyQuestionPartAnswerResponseModelData> keyQuestionPartAnswerAnswerPartResponses)
    {
        var optionSelectionResponseItems = keyQuestionPartAnswerAnswerPartResponses
            .Select(x => x.KeyQuestionPartAnswerResponse_ResponseItem)
            .Cast<KeyQuestionPartAnswerResponseItemOptionSelectionModelData>();

        var allSelectedOptions = optionSelectionResponseItems.SelectMany(responseItem =>
            responseItem.KeyQuestionPartAnswerResponseItemOptionSelection_SelectedOptions.Select(selectedOption =>
                selectedOption.KeyQuestionPartAnswerResponseItemSelectedOption_SelectedOptionValue));

        return allSelectedOptions;
    }
}