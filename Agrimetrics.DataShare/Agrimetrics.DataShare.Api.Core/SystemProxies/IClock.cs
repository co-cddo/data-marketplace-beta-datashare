namespace Agrimetrics.DataShare.Api.Core.SystemProxies;

public interface IClock
{
    DateTime LocalNow { get; }
    DateTime UtcNow { get; }
}