using System.Data;
using System.Diagnostics.CodeAnalysis;
using Dapper;

namespace Agrimetrics.DataShare.Api.Db.DbAccess;

[ExcludeFromCodeCoverage] // This class provides a proxy to a third party Dapper utilities, which cannot be reliably unit tested
internal class DatabaseCommandRunner : IDatabaseCommandRunner
{
    async Task<IEnumerable<TReturn>> IDatabaseCommandRunner.DbQueryAsync<TReturn>(
        IDbConnection dbConnection,
        IDbTransaction dbTransaction,
        string sqlQuery,
        object? parameters)
    {
        return await dbConnection.QueryAsync<TReturn>(
                sql: sqlQuery,
                param: parameters,
                transaction: dbTransaction)
            .ConfigureAwait(false);
    }

    async Task<IEnumerable<TReturn>> IDatabaseCommandRunner.DbQueryAsync<TFirst, TSecond, TReturn>(
        IDbConnection dbConnection,
        IDbTransaction dbTransaction,
        string sqlQuery,
        Func<TFirst, TSecond, TReturn> mappingFunc,
        string splitOn,
        object? parameters)
    {
        return await dbConnection.QueryAsync(
                sql: sqlQuery,
                map: mappingFunc,
                param: parameters,
                transaction: dbTransaction,
                splitOn: splitOn)
            .ConfigureAwait(false);
    }

    async Task<IEnumerable<TReturn>> IDatabaseCommandRunner.DbQueryAsync<TFirst, TSecond, TThird, TReturn>(
        IDbConnection dbConnection,
        IDbTransaction dbTransaction,
        string sqlQuery,
        Func<TFirst, TSecond, TThird, TReturn> mappingFunc,
        string splitOn,
        object? parameters)
    {
        return await dbConnection.QueryAsync(
                sql: sqlQuery,
                map: mappingFunc,
                param: parameters,
                transaction: dbTransaction,
                splitOn: splitOn)
            .ConfigureAwait(false);
    }

    async Task<IEnumerable<TReturn>> IDatabaseCommandRunner.DbQueryAsync<TFirst, TSecond, TThird, TFourth, TReturn>(
        IDbConnection dbConnection,
        IDbTransaction dbTransaction,
        string sqlQuery,
        Func<TFirst, TSecond, TThird, TFourth, TReturn> mappingFunc,
        string splitOn,
        object? parameters)
    {
        return await dbConnection.QueryAsync(
                sql: sqlQuery,
                map: mappingFunc,
                param: parameters,
                transaction: dbTransaction,
                splitOn: splitOn)
            .ConfigureAwait(false);
    }

    async Task<IEnumerable<TReturn>> IDatabaseCommandRunner.DbQueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(
        IDbConnection dbConnection,
        IDbTransaction dbTransaction,
        string sqlQuery,
        Func<TFirst, TSecond, TThird, TFourth, TFifth, TReturn> mappingFunc,
        string splitOn,
        object? parameters)
    {
        return await dbConnection.QueryAsync(
                sql: sqlQuery,
                map: mappingFunc,
                param: parameters,
                transaction: dbTransaction,
                splitOn: splitOn)
            .ConfigureAwait(false);
    }

    async Task<IEnumerable<TReturn>> IDatabaseCommandRunner.DbQueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn>(
        IDbConnection dbConnection,
        IDbTransaction dbTransaction,
        string sqlQuery,
        Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn> mappingFunc,
        string splitOn,
        object? parameters)
    {
        return await dbConnection.QueryAsync(
                sql: sqlQuery,
                map: mappingFunc,
                param: parameters,
                transaction: dbTransaction,
                splitOn: splitOn)
            .ConfigureAwait(false);
    }

    async Task<IEnumerable<TReturn>> IDatabaseCommandRunner.DbQueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn>(
        IDbConnection dbConnection,
        IDbTransaction dbTransaction,
        string sqlQuery,
        Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn> mappingFunc,
        string splitOn,
        object? parameters)
    {
        return await dbConnection.QueryAsync(
                sql: sqlQuery,
                map: mappingFunc,
                param: parameters,
                transaction: dbTransaction,
                splitOn: splitOn)
            .ConfigureAwait(false);
    }

    async Task<TReturn> IDatabaseCommandRunner.DbQuerySingleAsync<TReturn>(
        IDbConnection dbConnection,
        IDbTransaction dbTransaction,
        string sqlQuery,
        object? parameters)
    {
        return await dbConnection.QuerySingleAsync<TReturn>(
                sql: sqlQuery,
                param: parameters,
                transaction: dbTransaction)
            .ConfigureAwait(false);
    }

    async Task<TReturn?> IDatabaseCommandRunner.DbQuerySingleOrDefaultAsync<TReturn>(
        IDbConnection dbConnection,
        IDbTransaction dbTransaction,
        string sqlQuery,
        object? parameters) where TReturn : default
    {
        var result = await dbConnection.QuerySingleOrDefaultAsync<TReturn>(
                sql: sqlQuery,
                param: parameters,
                transaction: dbTransaction)
            .ConfigureAwait(false);

        return result ?? default;
    }

    async Task IDatabaseCommandRunner.DbExecuteAsync(
        IDbConnection dbConnection,
        IDbTransaction dbTransaction,
        string sqlCommand,
        object? parameters)
    {
        await dbConnection.ExecuteAsync(
            sql: sqlCommand,
            param: parameters,
            transaction: dbTransaction);
    }

    async Task IDatabaseCommandRunner.DbExecuteScalarAsync(
        IDbConnection dbConnection,
        IDbTransaction dbTransaction,
        string sqlCommand,
        object? parameters)
    {
        await dbConnection.ExecuteScalarAsync(
            sql: sqlCommand,
            param: parameters,
            transaction: dbTransaction);
    }

    async Task<TReturn> IDatabaseCommandRunner.DbExecuteScalarAsync<TReturn>(
        IDbConnection dbConnection,
        IDbTransaction dbTransaction,
        string sqlCommand,
        object? parameters)
    {
        return await dbConnection.ExecuteScalarAsync<TReturn>(
            sql: sqlCommand,
            param: parameters,
            transaction: dbTransaction)
            .ConfigureAwait(false);
    }
}