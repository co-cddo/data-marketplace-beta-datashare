using Agrimetrics.DataShare.Api.Core.Utilities;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Core.Test.Utilities
{
    [TestFixture]
    public class DatabaseTimestampProvisionTests
    {
        [Test]
        public void GivenADateTime_WhenIProvisionApiTimestampToDatabaseTimestamp_ThenTheUniversalTimeIsReturned()
        {
            var testLocalDateTime = TimeZoneInfo.ConvertTime(
                new DateTime(2025, 12, 25, 14, 0, 0, DateTimeKind.Unspecified),
                TimeZoneInfo.FindSystemTimeZoneById("Central Europe Standard Time"));

            var result = testLocalDateTime.ProvisionApiTimestampToDatabaseTimestamp();

            Assert.That(result, Is.EqualTo(testLocalDateTime.ToUniversalTime()));
        }

        [Test]
        public void GivenANullableDateTimeWithAValue_WhenIProvisionApiTimestampToDatabaseTimestamp_ThenTheUniversalTimeIsReturned()
        {
            DateTime? testLocalDateTime = TimeZoneInfo.ConvertTime(
                new DateTime(2025, 12, 25, 14, 0, 0, DateTimeKind.Unspecified),
                TimeZoneInfo.FindSystemTimeZoneById("Central Europe Standard Time"));

            var result = testLocalDateTime.ProvisionApiTimestampToDatabaseTimestamp();

            Assert.That(result, Is.EqualTo(testLocalDateTime.Value.ToUniversalTime()));
        }

        [Test]
        public void GivenANullDateTimeWithAValue_WhenIProvisionApiTimestampToDatabaseTimestamp_ThenNullIsReturned()
        {
            DateTime? testLocalDateTime = null;

            var result = testLocalDateTime.ProvisionApiTimestampToDatabaseTimestamp();

            Assert.That(result, Is.Null);
        }
    }
}
