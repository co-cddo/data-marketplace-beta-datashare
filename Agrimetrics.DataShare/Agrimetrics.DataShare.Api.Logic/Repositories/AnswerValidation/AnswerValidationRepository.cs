using Agrimetrics.DataShare.Api.Db.DbAccess;
using Agrimetrics.DataShare.Api.Logic.Exceptions;
using Agrimetrics.DataShare.Api.Logic.ModelData.AnswerValidation;
using Microsoft.Extensions.Logging;

namespace Agrimetrics.DataShare.Api.Logic.Repositories.AnswerValidation;

internal class AnswerValidationRepository(
    ILogger<AnswerValidationRepository> logger,
    IDatabaseChannelCreation databaseChannelCreation,
    IDatabaseCommandRunner databaseCommandRunner,
    IAnswerValidationSqlQueries answerValidationSqlQueries) : IAnswerValidationRepository
{
    async Task<QuestionPartAnswerValidationRuleSetModelData> IAnswerValidationRepository.GetQuestionPartAnswerValidationRulesAsync(
        Guid dataShareRequestId,
        Guid questionPartId)
    {
        await using var databaseChannel = await databaseChannelCreation.CreateAsync();

        try
        {
            var questionPartAnswerValidationRuleSetModelDatasFlattened =
                (await databaseCommandRunner.DbQueryAsync<
                    QuestionPartAnswerValidationRuleSetModelData,
                    QuestionPartAnswerValidationRuleModelData?,
                    QuestionPartAnswerValidationRuleSetModelData>(
                    databaseChannel.Connection,
                    databaseChannel.Transaction!,
                    answerValidationSqlQueries.GetQuestionPartAnswerValidationRuleSet,
                    (ruleSet, rule) =>
                    {
                        if (rule != null)
                        {
                            ruleSet.QuestionPartAnswerValidationRuleSet_ValidationRules.Add(rule);
                        }

                        return ruleSet;
                    },
                    nameof(QuestionPartAnswerValidationRuleModelData.QuestionPartAnswerValidationRule_RuleId),
                    new
                    {
                        DataShareRequestId = dataShareRequestId,
                        QuestionPartId = questionPartId
                    }).ConfigureAwait(false)).ToList();

            return BuildGroupedData();

            QuestionPartAnswerValidationRuleSetModelData BuildGroupedData()
            {
                var firstRecordInGroup = questionPartAnswerValidationRuleSetModelDatasFlattened[0];

                firstRecordInGroup.QuestionPartAnswerValidationRuleSet_ValidationRules = questionPartAnswerValidationRuleSetModelDatasFlattened
                        .SelectMany(x => x.QuestionPartAnswerValidationRuleSet_ValidationRules).ToList();

                return firstRecordInGroup;
            }
        }
        catch (Exception ex)
        {
            await databaseChannel.RollbackTransactionAsync();

            const string errorMessage = "Failed to GetQuestionPartAnswerValidationRules from database";

            logger.LogError(ex, errorMessage);

            throw new DatabaseAccessGeneralException(errorMessage, ex);
        }
        finally
        {
            await databaseChannel.DisposeAsync().ConfigureAwait(false);
        }
    }
}