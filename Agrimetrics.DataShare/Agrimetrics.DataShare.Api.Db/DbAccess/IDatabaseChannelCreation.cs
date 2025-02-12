namespace Agrimetrics.DataShare.Api.Db.DbAccess;

public interface IDatabaseChannelCreation
{
    Task<IDatabaseChannel> CreateAsync(bool beginTransaction = true);
}