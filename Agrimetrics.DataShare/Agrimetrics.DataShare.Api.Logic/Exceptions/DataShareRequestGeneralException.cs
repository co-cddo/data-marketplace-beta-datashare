namespace Agrimetrics.DataShare.Api.Logic.Exceptions;

public class DataShareRequestGeneralException : Exception
{
    public DataShareRequestGeneralException() { }

    public DataShareRequestGeneralException(string message)
        : base(message) { }

    public DataShareRequestGeneralException(string message, Exception innerException)
        : base(message, innerException) { }
}