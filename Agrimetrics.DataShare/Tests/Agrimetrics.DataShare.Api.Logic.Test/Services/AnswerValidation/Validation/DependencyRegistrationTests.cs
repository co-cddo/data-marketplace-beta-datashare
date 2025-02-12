using AutoFixture.AutoMoq;
using AutoFixture;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Agrimetrics.DataShare.Api.Logic.Services.AnswerValidation.Validation;

namespace Agrimetrics.DataShare.Api.Logic.Test.Services.AnswerValidation.Validation;

[TestFixture]
public class DependencyRegistrationTests
{
    #region RegisterValidationRules() Tests
    [Test]
    public void GivenANullServiceCollection_WhenIRegisterValidationRules_ThenAnArgumentNullExceptionIsThrown()
    {
        IServiceCollection serviceCollection = null!;

        Assert.That(() => serviceCollection.RegisterValidationRules(),
            Throws.ArgumentNullException.With.Property("ParamName").EqualTo("services"));
    }

    [Test]
    public void GivenAServiceCollection_WhenIRegisterValidationRules_ThenTheGivenServiceCollectionIsReturned()
    {
        var testItems = CreateTestItems();

        var serviceCollection = testItems.Fixture.Create<IServiceCollection>();

        var result = serviceCollection.RegisterValidationRules();

        Assert.That(result, Is.EqualTo(serviceCollection));
    }
    #endregion

    #region Test Item Creation
    private static TestItems CreateTestItems()
    {
        var fixture = new Fixture().Customize(new AutoMoqCustomization());

        return new TestItems(
            fixture);
    }

    private class TestItems(
        IFixture fixture)
    {
        public IFixture Fixture { get; } = fixture;
    }
    #endregion
}