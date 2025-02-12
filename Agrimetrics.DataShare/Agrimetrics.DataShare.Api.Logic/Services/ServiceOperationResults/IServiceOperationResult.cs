using System.Net;

namespace Agrimetrics.DataShare.Api.Logic.Services.ServiceOperationResults;

public interface IServiceOperationResult
{
    bool Success { get; }

    string? Error { get; }

    HttpStatusCode? StatusCode { get; }
}