using System.Data;
using Agrimetrics.DataShare.Api.Db.DbAccess;
using Agrimetrics.DataShare.Api.Logic.Exceptions;
using Agrimetrics.DataShare.Api.Logic.ModelData.QuestionConfiguration.CompulsoryQuestions;
using Microsoft.Extensions.Logging;

namespace Agrimetrics.DataShare.Api.Logic.Repositories.QuestionConfiguration;

internal class QuestionConfigurationRepository(
    ILogger<QuestionConfigurationRepository> logger,
    IDatabaseChannelCreation databaseChannelCreation,
    IDatabaseCommandRunner databaseCommandRunner,
    IQuestionConfigurationSqlQueries questionConfigurationSqlQueries) : IQuestionConfigurationRepository
{
    #region Compulsory Questions
    async Task<IEnumerable<CompulsoryQuestionModelData>> IQuestionConfigurationRepository.GetCompulsoryQuestionsAsync()
    {
        await using var databaseChannel = await databaseChannelCreation.CreateAsync();

        try
        {
            return await DoGetCompulsoryQuestionsAsync(
                databaseChannel.Connection,
                databaseChannel.Transaction!);
        }
        catch (Exception ex)
        {
            const string errorMessage = "Failed to GetCompulsoryQuestions from database";

            logger.LogError(ex, errorMessage);

            throw new DatabaseAccessGeneralException(errorMessage, ex);
        }
        finally
        {
            await databaseChannel.DisposeAsync().ConfigureAwait(false);
        }
    }

    async Task IQuestionConfigurationRepository.SetCompulsoryQuestionAsync(
        Guid questionId)
    {
        await using var databaseChannel = await databaseChannelCreation.CreateAsync();

        try
        {
            var existingCompulsoryQuestions =  await DoGetCompulsoryQuestionsAsync(
                databaseChannel.Connection,
                databaseChannel.Transaction!);

            if (existingCompulsoryQuestions.Any(x => x.CompulsoryQuestion_QuestionId == questionId))
                throw new InvalidOperationException("Question is already compulsory");

            await databaseCommandRunner.DbExecuteScalarAsync(
                databaseChannel.Connection,
                databaseChannel.Transaction!,
                questionConfigurationSqlQueries.SetCompulsoryQuestion,
                new
                {
                    QuestionId = questionId
                }).ConfigureAwait(false);

            await databaseChannel.CommitTransactionAsync().ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            await databaseChannel.RollbackTransactionAsync();

            const string errorMessage = "Failed to SetCompulsoryQuestion in database";

            logger.LogError(ex, errorMessage);

            throw new DatabaseAccessGeneralException(errorMessage, ex);
        }
        finally
        {
            await databaseChannel.DisposeAsync().ConfigureAwait(false);
        }
    }

    async Task IQuestionConfigurationRepository.ClearCompulsoryQuestionAsync(
        Guid questionId)
    {
        await using var databaseChannel = await databaseChannelCreation.CreateAsync();

        try
        {
            var existingCompulsoryQuestions = await DoGetCompulsoryQuestionsAsync(
                databaseChannel.Connection,
                databaseChannel.Transaction!);

            if (existingCompulsoryQuestions.All(x => x.CompulsoryQuestion_QuestionId != questionId))
                throw new InvalidOperationException("Question is not compulsory");

            await databaseCommandRunner.DbExecuteScalarAsync(
                databaseChannel.Connection,
                databaseChannel.Transaction!,
                questionConfigurationSqlQueries.ClearCompulsoryQuestion,
                new
                {
                    QuestionId = questionId
                }).ConfigureAwait(false);

            await databaseChannel.CommitTransactionAsync().ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            await databaseChannel.RollbackTransactionAsync();

            const string errorMessage = "Failed to ClearCompulsoryQuestion in database";

            logger.LogError(ex, errorMessage);

            throw new DatabaseAccessGeneralException(errorMessage, ex);
        }
        finally
        {
            await databaseChannel.DisposeAsync().ConfigureAwait(false);
        }
    }

    private async Task<IEnumerable<CompulsoryQuestionModelData>> DoGetCompulsoryQuestionsAsync(
        IDbConnection dbConnection,
        IDbTransaction dbTransaction)
    {
        return await databaseCommandRunner.DbQueryAsync<CompulsoryQuestionModelData>(
            dbConnection,
            dbTransaction,
            questionConfigurationSqlQueries.GetCompulsoryQuestions,
            new
            {
                // no params
            }).ConfigureAwait(false);
    }
    #endregion

    #region Compulsory Supplier-Mandated Questions
    async Task<IEnumerable<CompulsorySupplierMandatedQuestionModelData>> IQuestionConfigurationRepository.GetCompulsorySupplierMandatedQuestionsAsync(
        int supplierOrganisationId)
    {
        await using var databaseChannel = await databaseChannelCreation.CreateAsync();

        try
        {
            return await DoGetCompulsorySupplierMandatedQuestionsAsync(
                databaseChannel.Connection,
                databaseChannel.Transaction!,
                supplierOrganisationId);
        }
        catch (Exception ex)
        {
            const string errorMessage = "Failed to GetCompulsorySupplierMandatedQuestions from database";

            logger.LogError(ex, errorMessage);

            throw new DatabaseAccessGeneralException(errorMessage, ex);
        }
        finally
        {
            await databaseChannel.DisposeAsync().ConfigureAwait(false);
        }
    }

    async Task IQuestionConfigurationRepository.SetCompulsorySupplierMandatedQuestionAsync(
        int supplierOrganisationId,
        Guid questionId)
    {
        await using var databaseChannel = await databaseChannelCreation.CreateAsync();

        try
        {
            var existingCompulsorySupplierMandatedQuestions =
                await DoGetCompulsorySupplierMandatedQuestionsAsync(
                    databaseChannel.Connection,
                    databaseChannel.Transaction!,
                    supplierOrganisationId);

            if (existingCompulsorySupplierMandatedQuestions.Any(x => x.CompulsorySupplierMandatedQuestion_QuestionId == questionId))
                throw new InvalidOperationException("Question is already supplier-mandated compulsory");

            await databaseCommandRunner.DbExecuteScalarAsync(
                databaseChannel.Connection,
                databaseChannel.Transaction!,
                questionConfigurationSqlQueries.SetCompulsorySupplierMandatedQuestion,
                new
                {
                    SupplierOrganisationId = supplierOrganisationId,
                    QuestionId = questionId
                }).ConfigureAwait(false);

            await databaseChannel.CommitTransactionAsync().ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            await databaseChannel.RollbackTransactionAsync();

            const string errorMessage = "Failed to SetCompulsorySupplierMandatedQuestion in database";

            logger.LogError(ex, errorMessage);

            throw new DatabaseAccessGeneralException(errorMessage, ex);
        }
        finally
        {
            await databaseChannel.DisposeAsync().ConfigureAwait(false);
        }
    }

    async Task IQuestionConfigurationRepository.ClearCompulsorySupplierMandatedQuestionAsync(
        int supplierOrganisationId,
        Guid questionId)
    {
        await using var databaseChannel = await databaseChannelCreation.CreateAsync();

        try
        {
            var existingCompulsorySupplierMandatedQuestions =
                await DoGetCompulsorySupplierMandatedQuestionsAsync(
                    databaseChannel.Connection,
                    databaseChannel.Transaction!,
                    supplierOrganisationId);

            if (existingCompulsorySupplierMandatedQuestions.All(x => x.CompulsorySupplierMandatedQuestion_QuestionId != questionId))
                throw new InvalidOperationException("Question is not supplier-mandated compulsory");

            await databaseCommandRunner.DbExecuteScalarAsync(
                databaseChannel.Connection,
                databaseChannel.Transaction!,
                questionConfigurationSqlQueries.ClearCompulsorySupplierMandatedQuestion,
                new
                {
                    SupplierOrganisationId = supplierOrganisationId,
                    QuestionId = questionId
                }).ConfigureAwait(false);

            await databaseChannel.CommitTransactionAsync().ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            await databaseChannel.RollbackTransactionAsync();

            const string errorMessage = "Failed to ClearCompulsorySupplierMandatedQuestion in database";

            logger.LogError(ex, errorMessage);

            throw new DatabaseAccessGeneralException(errorMessage, ex);
        }
        finally
        {
            await databaseChannel.DisposeAsync().ConfigureAwait(false);
        }
    }

    private async Task<IEnumerable<CompulsorySupplierMandatedQuestionModelData>> DoGetCompulsorySupplierMandatedQuestionsAsync(
        IDbConnection dbConnection,
        IDbTransaction dbTransaction,
        int supplierOrganisationId)
    {
        return await databaseCommandRunner.DbQueryAsync<CompulsorySupplierMandatedQuestionModelData>(
            dbConnection,
            dbTransaction,
            questionConfigurationSqlQueries.GetCompulsorySupplierMandatedQuestions,
            new
            {
                SupplierOrganisationId = supplierOrganisationId
            }).ConfigureAwait(false);
    }
    #endregion
}