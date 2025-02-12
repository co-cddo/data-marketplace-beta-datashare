namespace Agrimetrics.DataShare.Api.Logic.Exceptions;

public class DatabaseAccessGeneralException : Exception
{
    public DatabaseAccessGeneralException() { }

    public DatabaseAccessGeneralException(string message)
        : base(message) { }

    public DatabaseAccessGeneralException(string message, Exception innerException)
        : base(message, innerException) { }
}