namespace Agrimetrics.DataShare.Api.Core.Utilities
{
    public static class DatabaseTimestampProvision
    {
        public static DateTime ProvisionApiTimestampToDatabaseTimestamp(this DateTime apiTimestamp) => apiTimestamp.ToUniversalTime();

        public static DateTime? ProvisionApiTimestampToDatabaseTimestamp(this DateTime? apiTimestamp) => apiTimestamp?.ToUniversalTime();
    }
}
