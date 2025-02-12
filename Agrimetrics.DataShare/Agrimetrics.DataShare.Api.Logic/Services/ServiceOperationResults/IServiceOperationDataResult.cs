using System.Net;

namespace Agrimetrics.DataShare.Api.Logic.Services.ServiceOperationResults;

public interface IServiceOperationDataResult<out T>
{
    bool Success { get; }

    string? Error { get; }

    HttpStatusCode? StatusCode { get; }

    T? Data { get; }
}