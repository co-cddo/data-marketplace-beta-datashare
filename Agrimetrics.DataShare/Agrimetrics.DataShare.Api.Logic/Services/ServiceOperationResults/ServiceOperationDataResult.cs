using System.Net;

namespace Agrimetrics.DataShare.Api.Logic.Services.ServiceOperationResults;

internal class ServiceOperationDataResult<T>(bool success, string? error, T? data, HttpStatusCode? statusCode)
    : IServiceOperationDataResult<T>
{
    public bool Success { get; } = success;

    public string? Error { get; } = error;

    public HttpStatusCode? StatusCode { get; } = statusCode;

    public T? Data { get; } = data;
}