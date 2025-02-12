using Agrimetrics.DataShare.Api.HealthChecks;
using AutoFixture.AutoMoq;
using AutoFixture;
using Moq;
using NUnit.Framework;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Agrimetrics.DataShare.Api.Test.HealthChecks
{
    [TestFixture]
    public class CustomSqlHealthCheckTests
    {
        #region Construction Tests
        [Test]
        public void GivenANullCustomSqlHealthCheckSqlCommandRunner_WhenIConstructAnInstanceOfCustomSqlHealthCheck_ThenAnArgumentNullExceptionIsThrown()
        {
            Assert.That(() => new CustomSqlHealthCheck(null!, "_"),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("customSqlHealthCheckSqlCommandRunner"));
        }

        [Test]
        public void GivenANullConnectionString_WhenIConstructAnInstanceOfCustomSqlHealthCheck_ThenAnArgumentNullExceptionIsThrown()
        {

            Assert.That(() => new CustomSqlHealthCheck(
                    Mock.Of<ICustomSqlHealthCheckSqlCommandRunner>(),
                    null!),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("connectionString"));
        }
        #endregion

        #region CheckHealthAsync() Tests
        [Test]
        public async Task GivenAConnectionString_WhenICheckHealthAsync_ThenACommandIsRunOnThatConnection()
        {
            var testItems = CreateTestItems("test connection string");

            await testItems.CustomSqlHealthCheck.CheckHealthAsync(It.IsAny<HealthCheckContext>());

            testItems.MockCustomSqlHealthCheckSqlCommandRunner.Verify(x => x.RunCommandAsync(
                    "test connection string",
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Test]
        public async Task GivenACancellationToken_WhenICheckHealthAsync_ThenACommandIsRunUsingThatCancellationToken()
        {
            var testItems = CreateTestItems();

            var testCancellationToken = new CancellationToken();

            await testItems.CustomSqlHealthCheck.CheckHealthAsync(It.IsAny<HealthCheckContext>(), testCancellationToken);
            
            testItems.MockCustomSqlHealthCheckSqlCommandRunner.Verify(x => x.RunCommandAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    testCancellationToken),
                Times.Once);
        }

        [Test]
        public async Task GivenTheHealthCheckWillSucceed_WhenICheckHealthAsync_ThenAHealthyHealthCheckResultISReturned()
        {
            var testItems = CreateTestItems();

            var result = await testItems.CustomSqlHealthCheck.CheckHealthAsync(It.IsAny<HealthCheckContext>());

            Assert.Multiple(() =>
            {
                Assert.That(result.Status, Is.EqualTo(HealthStatus.Healthy));
                Assert.That(result.Description, Is.EqualTo("SQL Database is up and running."));
            });
        }

        [Test]
        public async Task GivenTheHealthCheckWillFail_WhenICheckHealthAsync_ThenAnUnHealthyHealthCheckResultISReturned()
        {
            var testItems = CreateTestItems();

            var testException = new Exception("oh noes!");

            testItems.MockCustomSqlHealthCheckSqlCommandRunner.Setup(x => x.RunCommandAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(testException);

            var result = await testItems.CustomSqlHealthCheck.CheckHealthAsync(It.IsAny<HealthCheckContext>());

            Assert.Multiple(() =>
            {
                Assert.That(result.Status, Is.EqualTo(HealthStatus.Unhealthy));
                Assert.That(result.Description, Is.EqualTo("SQL Database is not responding."));
                Assert.That(result.Exception, Is.EqualTo(testException));
            });
        }
        #endregion


        #region Test Item Creation
        private static TestItems CreateTestItems(
            string connectionString = "abc")
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());

            var mockCustomSqlHealthCheckSqlCommandRunner = Mock.Get(fixture.Freeze<ICustomSqlHealthCheckSqlCommandRunner>());

            var customSqlHealthCheck = new CustomSqlHealthCheck(
                mockCustomSqlHealthCheckSqlCommandRunner.Object,
                connectionString);

            return new TestItems(
                customSqlHealthCheck,
                mockCustomSqlHealthCheckSqlCommandRunner);
        }

        private class TestItems(
            CustomSqlHealthCheck customSqlHealthCheck,
            Mock<ICustomSqlHealthCheckSqlCommandRunner> mockCustomSqlHealthCheckSqlCommandRunner)
        {
            public CustomSqlHealthCheck CustomSqlHealthCheck { get; } = customSqlHealthCheck;
            public Mock<ICustomSqlHealthCheckSqlCommandRunner> MockCustomSqlHealthCheckSqlCommandRunner { get; } = mockCustomSqlHealthCheckSqlCommandRunner;
        }
        #endregion
    }
}
