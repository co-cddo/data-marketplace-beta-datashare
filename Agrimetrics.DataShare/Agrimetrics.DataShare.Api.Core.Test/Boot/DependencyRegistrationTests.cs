using Agrimetrics.DataShare.Api.Core.Boot;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Core.Test.Boot
{
    [TestFixture]
    public class DependencyRegistrationTests
    {
        #region RegisterCoreDependencies() Tests
        [Test]
        public void GivenANullServiceCollection_WhenIRegisterCoreDependencies_ThenTheGivenServiceCollectionIsReturned()
        {
            IServiceCollection nullServiceCollection = null!;

            Assert.That(() => nullServiceCollection.RegisterCoreDependencies(),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("services"));
        }

        [Test]
        public void GivenAServiceCollection_WhenIRegisterCoreDependencies_ThenTheGivenServiceCollectionIsReturned()
        {
            var testServiceCollection = Mock.Of<IServiceCollection>();

            var result = testServiceCollection.RegisterCoreDependencies();

            Assert.That(result, Is.SameAs(testServiceCollection));
        }
        #endregion
    }
}
