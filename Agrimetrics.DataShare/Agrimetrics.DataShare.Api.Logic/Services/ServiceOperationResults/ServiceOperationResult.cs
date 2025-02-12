using System.Net;

namespace Agrimetrics.DataShare.Api.Logic.Services.ServiceOperationResults;

internal class ServiceOperationResult(bool success, string? error, HttpStatusCode? statusCode)
    : IServiceOperationResult
{
    public bool Success { get; } = success;

    public string? Error { get; } = error;

    public HttpStatusCode? StatusCode { get; } = statusCode;
}