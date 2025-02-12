namespace Agrimetrics.DataShare.Api.Logic.Exceptions;

public class InconsistentDataException : Exception
{
    public InconsistentDataException() { }

    public InconsistentDataException(string message)
        : base(message) { }

    public InconsistentDataException(string message, Exception innerException)
        : base(message, innerException) { }
}