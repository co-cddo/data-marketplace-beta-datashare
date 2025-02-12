namespace Agrimetrics.DataShare.Api.Logic.Exceptions;

public class ExternalServiceAccessException : Exception
{
    public ExternalServiceAccessException() { }

    public ExternalServiceAccessException(string message)
        : base(message) { }

    public ExternalServiceAccessException(string message, Exception innerException)
        : base(message, innerException) { }
}