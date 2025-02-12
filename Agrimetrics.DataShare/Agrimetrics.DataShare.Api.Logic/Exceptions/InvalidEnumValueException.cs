namespace Agrimetrics.DataShare.Api.Logic.Exceptions;

public class InvalidEnumValueException : Exception
{
    public InvalidEnumValueException() { }

    public InvalidEnumValueException(string message)
        : base(message) { }

    public InvalidEnumValueException(string message, Exception innerException)
        : base(message, innerException) { }
}