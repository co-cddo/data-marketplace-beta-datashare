using Agrimetrics.DataShare.Api.Db.DbAccess;
using Agrimetrics.DataShare.Api.Logic.Exceptions;
using Agrimetrics.DataShare.Api.Logic.ModelData.AnswerHighlights;
using Microsoft.Extensions.Logging;

namespace Agrimetrics.DataShare.Api.Logic.Repositories.AnswerHighlights;

internal class AnswerHighlightsRepository(
    ILogger<AnswerHighlightsRepository> logger,
    IDatabaseChannelCreation databaseChannelCreation,
    IDatabaseCommandRunner databaseCommandRunner,
    IAnswerHighlightsSqlQueries answerHighlightsSqlQueries) : IAnswerHighlightsRepository
{
    async Task<IEnumerable<QuestionSetSelectionOptionQuestionHighlightModelData>> IAnswerHighlightsRepository.GetQuestionSetSelectionOptionQuestionHighlightModelDataAsync(
        Guid dataShareRequestId)
    {
        await using var databaseChannel = await databaseChannelCreation.CreateAsync();

        try
        {
            return await databaseCommandRunner.DbQueryAsync<QuestionSetSelectionOptionQuestionHighlightModelData>(
                    databaseChannel.Connection,
                    databaseChannel.Transaction!,
                    answerHighlightsSqlQueries.GetQuestionSetSelectionOptionQuestionHighlightModelDatas,
                    new
                    {
                        DataShareRequestId = dataShareRequestId
                    })
                .ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            await databaseChannel.RollbackTransactionAsync();

            const string errorMessage = "Failed to GetQuestionSetSelectionOptionQuestionHighlightModelData from database";

            logger.LogError(ex, errorMessage);

            throw new DatabaseAccessGeneralException(errorMessage, ex);
        }
        finally
        {
            await databaseChannel.DisposeAsync().ConfigureAwait(false);
        }
    }

    async Task<DataShareRequestSelectionOptionsModelData> IAnswerHighlightsRepository.GetDataShareRequestSelectionOptionsModelDataAsync(
        Guid dataShareRequestId)
    {
        await using var databaseChannel = await databaseChannelCreation.CreateAsync();

        try
        {
            var dataShareRequestSelectionOptionsModelDataFlattened =
                (await databaseCommandRunner.DbQueryAsync<
                    DataShareRequestSelectionOptionsModelData,
                    DataShareRequestSelectedOptionModelData,
                    DataShareRequestSelectionOptionsModelData>(
                    databaseChannel.Connection,
                    databaseChannel.Transaction!,
                    answerHighlightsSqlQueries.GetDataShareRequestSelectedOptionsModelDatas,
                    (selectionOptions, selectedOption) =>
                    {
                        selectionOptions.DataShareRequestSelectionOptions_SelectedOptions.Add(selectedOption);
                        return selectionOptions;
                    },
                    nameof(DataShareRequestSelectedOptionModelData.DataShareRequestSelectedOption_OptionSelectionId),
                    new
                    {
                        DataShareRequestId = dataShareRequestId
                    })
                .ConfigureAwait(false)).ToList();

            return BuildGroupedData();

            DataShareRequestSelectionOptionsModelData BuildGroupedData()
            {
                var firstRecordInGroup = dataShareRequestSelectionOptionsModelDataFlattened[0];

                firstRecordInGroup.DataShareRequestSelectionOptions_SelectedOptions = dataShareRequestSelectionOptionsModelDataFlattened
                    .SelectMany(x => x.DataShareRequestSelectionOptions_SelectedOptions).ToList();

                return firstRecordInGroup;
            }
        }
        catch (Exception ex)
        {
            await databaseChannel.RollbackTransactionAsync();

            const string errorMessage = "Failed to GetDataShareRequestSelectionOptionsModelData from database";

            logger.LogError(ex, errorMessage);

            throw new DatabaseAccessGeneralException(errorMessage, ex);
        }
        finally
        {
            await databaseChannel.DisposeAsync().ConfigureAwait(false);
        }
    }
}