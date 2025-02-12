namespace Agrimetrics.DataShare.Api.Logic.Exceptions;

public class UserIdTokenAccessException : Exception
{
    public UserIdTokenAccessException() { }

    public UserIdTokenAccessException(string message)
        : base(message) { }

    public UserIdTokenAccessException(string message, Exception innerException)
        : base(message, innerException) { }
}