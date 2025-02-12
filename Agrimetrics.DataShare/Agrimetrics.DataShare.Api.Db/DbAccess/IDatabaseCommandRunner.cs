using System.Data;

namespace Agrimetrics.DataShare.Api.Db.DbAccess;

public interface IDatabaseCommandRunner
{
    Task<IEnumerable<TReturn>> DbQueryAsync<TReturn>(
        IDbConnection dbConnection,
        IDbTransaction dbTransaction,
        string sqlQuery,
        object? parameters);

    Task<IEnumerable<TReturn>> DbQueryAsync<TFirst, TSecond, TReturn>(
        IDbConnection dbConnection,
        IDbTransaction dbTransaction,
        string sqlQuery,
        Func<TFirst, TSecond, TReturn> mappingFunc,
        string splitOn,
        object? parameters = null);

    Task<IEnumerable<TReturn>> DbQueryAsync<TFirst, TSecond, TThird, TReturn>(
        IDbConnection dbConnection,
        IDbTransaction dbTransaction,
        string sqlQuery,
        Func<TFirst, TSecond, TThird, TReturn> mappingFunc,
        string splitOn,
        object? parameters = null);

    Task<IEnumerable<TReturn>> DbQueryAsync<TFirst, TSecond, TThird, TFourth, TReturn>(
        IDbConnection dbConnection,
        IDbTransaction dbTransaction,
        string sqlQuery,
        Func<TFirst, TSecond, TThird, TFourth, TReturn> mappingFunc,
        string splitOn,
        object? parameters = null);

    Task<IEnumerable<TReturn>> DbQueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(
        IDbConnection dbConnection,
        IDbTransaction dbTransaction,
        string sqlQuery,
        Func<TFirst, TSecond, TThird, TFourth, TFifth, TReturn> mappingFunc,
        string splitOn,
        object? parameters = null);

    Task<IEnumerable<TReturn>> DbQueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn>(
        IDbConnection dbConnection,
        IDbTransaction dbTransaction,
        string sqlQuery,
        Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn> mappingFunc,
        string splitOn,
        object? parameters = null);

    Task<IEnumerable<TReturn>> DbQueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn>(
        IDbConnection dbConnection,
        IDbTransaction dbTransaction,
        string sqlQuery,
        Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn> mappingFunc,
        string splitOn,
        object? parameters = null);

    Task<TReturn> DbQuerySingleAsync<TReturn>(
        IDbConnection dbConnection,
        IDbTransaction dbTransaction,
        string sqlQuery,
        object? parameters = null);

    Task<TReturn?> DbQuerySingleOrDefaultAsync<TReturn>(
        IDbConnection dbConnection,
        IDbTransaction dbTransaction,
        string sqlQuery,
        object? parameters = null);

    Task DbExecuteAsync(
        IDbConnection dbConnection,
        IDbTransaction dbTransaction,
        string sqlCommand,
        object? parameters);

    Task DbExecuteScalarAsync(
        IDbConnection dbConnection,
        IDbTransaction dbTransaction,
        string sqlCommand,
        object? parameters = null);

    Task<TReturn> DbExecuteScalarAsync<TReturn>(
        IDbConnection dbConnection,
        IDbTransaction dbTransaction,
        string sqlCommand,
        object? parameters = null);
}