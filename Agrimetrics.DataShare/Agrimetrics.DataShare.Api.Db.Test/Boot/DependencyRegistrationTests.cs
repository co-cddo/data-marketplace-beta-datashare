using Agrimetrics.DataShare.Api.Db.Boot;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Db.Test.Boot
{
    [TestFixture]
    public class DependencyRegistrationTests
    {
        #region RegisterDbAccessDependencies() Tests
        [Test]
        public void GivenANullServiceCollection_WhenIRegisterDbAccessDependencies_ThenTheGivenServiceCollectionIsReturned()
        {
            IServiceCollection nullServiceCollection = null!;

            Assert.That(() => nullServiceCollection.RegisterDbAccessDependencies(),
                Throws.ArgumentNullException.With.Property("ParamName").EqualTo("services"));
        }

        [Test]
        public void GivenAServiceCollection_WhenIRegisterDbAccessDependencies_ThenTheGivenServiceCollectionIsReturned()
        {
            var testServiceCollection = Mock.Of<IServiceCollection>();

            var result = testServiceCollection.RegisterDbAccessDependencies();

            Assert.That(result, Is.SameAs(testServiceCollection));
        }
        #endregion
    }
}
