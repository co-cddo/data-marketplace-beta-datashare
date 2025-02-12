using Agrimetrics.DataShare.Api.Db.Configuration;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Db.Test.Configuration
{
    [TestFixture]
    public class DatabaseConnectionsConfigurationPresenterTest
    {
        #region GetSqlConnectionString()
        [Test]
        public void GivenADatabaseConnectionStringCanBeReadFromConfiguration_WhenIGetSqlConnectionString_ThenTheConfiguredConnectionStringIsReturned()
        {
            var testItems = CreateTestItems();

            testItems.MockConfiguration.Setup(x => x.GetSection("ConnectionStrings")["sql_connection_string"])
                .Returns("test sql connection string");

            var result = testItems.DatabaseConnectionsConfigurationPresenter.GetSqlConnectionString();

            Assert.That(result, Is.EqualTo("test sql connection string"));
        }

        [Test]
        public void GivenANullDatabaseConnectionStringCannotBeReadFromConfiguration_WhenIGetSqlConnectionString_ThenAnArgumentExceptionIsThrown()
        {
            var testItems = CreateTestItems();

            testItems.MockConfiguration.Setup(x => x.GetSection("ConnectionStrings")["sql_connection_string"])
                .Returns((string?)null);

            Assert.That(() => testItems.DatabaseConnectionsConfigurationPresenter.GetSqlConnectionString(),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("sqlConnectionString"));
        }

        [Test]
        public void GivenAnEmptyDatabaseConnectionStringCannotBeReadFromConfiguration_WhenIGetSqlConnectionString_ThenAnArgumentExceptionIsThrown(
            [Values("", "   ")] string? testConnectionString)
        {
            var testItems = CreateTestItems();

            testItems.MockConfiguration.Setup(x => x.GetSection("ConnectionStrings")["sql_connection_string"])
                .Returns(testConnectionString);

            Assert.That(() => testItems.DatabaseConnectionsConfigurationPresenter.GetSqlConnectionString(),
                Throws.ArgumentException.With.Property("ParamName").EqualTo("sqlConnectionString"));
        }
        #endregion

        #region GetAllSettingValues() Tests
        [Test]
        public void GivenADatabaseConnectionsConfigurationPresenter_WhenIGetAllSettingValues_ThenAllSettingsValuesAreRetrieved()
        {
            var testItems = CreateTestItems();

            testItems.MockConfiguration.Setup(x => x.GetSection("ConnectionStrings")["sql_connection_string"])
                .Returns("test sql connection string");

            var result = testItems.DatabaseConnectionsConfigurationPresenter.GetAllSettingValues().ToList();

            Assert.Multiple(() =>
            {
                Assert.That(result, Has.Exactly(1).Items);

                Assert.That(result.Any(x => x is {Description: "Database Connection String", Value: "test sql connection string"}));
            });
        }

        [Test]
        public void GivenGettingASettingValueFails_WhenIGetAllSettingValues_ThenAnErrorMessageIsReturned()
        {
            var testItems = CreateTestItems();

            var testException = new Exception("oh noes!");

            testItems.MockConfiguration.Setup(x => x.GetSection("ConnectionStrings")["sql_connection_string"])
                .Throws(testException);

            var result = testItems.DatabaseConnectionsConfigurationPresenter.GetAllSettingValues().ToList();

            Assert.Multiple(() =>
            {
                Assert.That(result, Has.Exactly(1).Items);

                Assert.That(result.Any(x => x is { Description: "Database Connection String", Value: "ERROR: oh noes!" }));
            });
        }
        #endregion

        #region Test Item Creation
        private static TestItems CreateTestItems()
        {
            var mockConfiguration = new Mock<IConfiguration>();

            var databaseConnectionsConfigurationPresenter = new DatabaseConnectionsConfigurationPresenter(
                mockConfiguration.Object);

            return new TestItems(
                databaseConnectionsConfigurationPresenter,
                mockConfiguration);
        }

        private class TestItems(
            IDatabaseConnectionsConfigurationPresenter databaseConnectionsConfigurationPresenter,
            Mock<IConfiguration> mockConfiguration)
        {
            public IDatabaseConnectionsConfigurationPresenter DatabaseConnectionsConfigurationPresenter { get; } = databaseConnectionsConfigurationPresenter;
            public Mock<IConfiguration> MockConfiguration { get; } = mockConfiguration;
        }
        #endregion
    }
}
