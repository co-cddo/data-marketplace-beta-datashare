using Agrimetrics.DataShare.Api.Boot;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Test
{
    [TestFixture]
    public class DependencyRegistrationTests
    {
        #region RegisterServiceDependencies() Tests
        [Test]
        public void GivenANullServiceCollection_WhenIRegisterServiceDependencies_ThenAnArgumentNullExceptionIsThrown()
        {
            var testItems = CreateTestItems();

            Assert.That(() => testItems.DependencyRegistration.RegisterServiceDependencies(null!),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("services"));
        }

        [Test]
        public void GivenAServiceCollection_WhenIRegisterServiceDependencies_ThenTheServiceCollectionIsReturned()
        {
            var testItems = CreateTestItems();

            var mockServiceCollection = new Mock<IServiceCollection>();

            var result = testItems.DependencyRegistration.RegisterServiceDependencies(mockServiceCollection.Object);

            Assert.That(result, Is.SameAs(mockServiceCollection.Object));
        }
        #endregion

        #region Test Item Creation
        private static TestItems CreateTestItems()
        {
            var dependencyRegistration = new DependencyRegistration();

            return new TestItems(dependencyRegistration);
        }

        private class TestItems(
            IDependencyRegistration dependencyRegistration)
        {
            public IDependencyRegistration DependencyRegistration { get; } = dependencyRegistration;
        }
        #endregion
    }
}
