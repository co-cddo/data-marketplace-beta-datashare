using Agrimetrics.DataShare.Api.Core.Configuration;
using AutoFixture;
using AutoFixture.AutoMoq;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Core.Test.Configuration
{
    [TestFixture]
    public class ServiceConfigurationPresenterTests
    {
        #region GetValue() Tests
        [Test]
        public void GivenAnEmptyValueKey_WhenIGetValue_ThenAnArgumentExceptionIsThrown(
            [Values("", "  ")] string valueKey)
        {
            var testItems = CreateTestItems();

            Assert.That(() => testItems.ServiceConfigurationPresenter.GetValue(valueKey),
                Throws.ArgumentException.With.Property("ParamName").EqualTo("valueKey"));
        }

        [Test]
        public void GivenAUnknownValueKey_WhenIGetValue_ThenAnInvalidOperationExceptionIsThrown()
        {
            var testItems = CreateTestItems();

            Assert.That(() => testItems.ServiceConfigurationPresenter.GetValue("some value key"),
                Throws.InvalidOperationException.With.Message.EqualTo("Configuration key not found: Key='some value key'"));
        }

        [Test]
        public void GivenAKnownValueKey_WhenIGetValue_ThenTheReferencedValueIsReturned()
        {
            var testItems = CreateTestItems();

            testItems.MockConfiguration.SetupGet(x => x["test value key"]).Returns("test value");

            var result = testItems.ServiceConfigurationPresenter.GetValue("test value key");

            Assert.That(result, Is.EqualTo("test value"));
        }
        #endregion

        #region GetValueInSection() Tests
        [Test]
        public void GivenAnEmptySectionName_WhenIGetValueInSection_ThenAnArgumentExceptionIsThrown(
            [Values("", "  ")] string sectionName)
        {
            var testItems = CreateTestItems();

            Assert.That(() => testItems.ServiceConfigurationPresenter.GetValueInSection(sectionName, "_"),
                Throws.ArgumentException.With.Property("ParamName").EqualTo("sectionName"));
        }

        [Test]
        public void GivenAnEmptyValueKey_WhenIGetValueInSection_ThenAnArgumentExceptionIsThrown(
            [Values("", "  ")] string valueKey)
        {
            var testItems = CreateTestItems();

            Assert.That(() => testItems.ServiceConfigurationPresenter.GetValueInSection("_", valueKey),
                Throws.ArgumentException.With.Property("ParamName").EqualTo("valueKey"));
        }

        [Test]
        public void GivenAnUnknownSectionName_WhenIGetValueInSection_ThenAnInvalidOperationExceptionIsThrown()
        {
            var testItems = CreateTestItems();

            Assert.That(() => testItems.ServiceConfigurationPresenter.GetValueInSection("some section name", "_"),
                Throws.InvalidOperationException.With.Message.EqualTo("Configuration section not found: Section='some section name'"));
        }

        [Test]
        public void GivenAUnknownValueKey_WhenIGetValueInSection_ThenAnInvalidOperationExceptionIsThrown()
        {
            var testItems = CreateTestItems();

            var mockSection = new Mock<IConfigurationSection>();
            mockSection.SetupGet(x => x.Path).Returns("test section path");

            testItems.MockConfiguration.Setup(x => x.GetSection("test section name"))
                .Returns(mockSection.Object);

            Assert.That(() => testItems.ServiceConfigurationPresenter.GetValueInSection("test section name", "some value key"),
                Throws.InvalidOperationException.With.Message.EqualTo("Configuration key not found in section: Key='some value key', Section='test section path'"));
        }

        [Test]
        public void GivenAKnownValueKey_WhenIGetValueInSection_ThenTheReferencedValueIsReturned()
        {
            var testItems = CreateTestItems();

            var mockSection = Mock.Get(testItems.Fixture.Create<IConfigurationSection>());
            mockSection.SetupGet(x => x["test value key"]).Returns("test value");

            testItems.MockConfiguration.Setup(x => x.GetSection("some section name"))
                .Returns(mockSection.Object);

            var result = testItems.ServiceConfigurationPresenter.GetValueInSection("some section name", "test value key");

            Assert.That(result, Is.EqualTo("test value"));
        }
        #endregion

        #region GetValueInMultiLevelSection() Tests
        [Test]
        public void GivenAnEmptySetOfSectionNames_WhenIGetValueInMultiLevelSection_ThenAnArgumentExceptionIsThrown()
        {
            var testItems = CreateTestItems();

            Assert.That(() => testItems.ServiceConfigurationPresenter.GetValueInMultiLevelSection(Enumerable.Empty<string>(), "_"),
                Throws.ArgumentException.With.Property("ParamName").EqualTo("sectionNames")
                    .And.With.Message.StartWith("Given value is empty"));
        }

        [Test]
        public void GivenAnEmptyValueKey_WhenIGetValueInMultiLevelSection_ThenAnArgumentExceptionIsThrown(
            [Values("", "  ")] string valueKey)
        {
            var testItems = CreateTestItems();

            Assert.That(() => testItems.ServiceConfigurationPresenter.GetValueInMultiLevelSection(["_"], valueKey),
                Throws.ArgumentException.With.Property("ParamName").EqualTo("valueKey"));
        }

        [Test]
        public void GivenAnUnknownRootSectionName_WhenIGetValueInMultiLevelSection_ThenAnInvalidOperationExceptionIsThrown()
        {
            var testItems = CreateTestItems();

            Assert.That(() => testItems.ServiceConfigurationPresenter.GetValueInMultiLevelSection(["some section name"], "_"),
                Throws.InvalidOperationException.With.Message.EqualTo("Configuration section not found at root of configuration: Section='some section name'"));
        }

        [Test]
        public void GivenAnUnknownChildSectionName_WhenIGetValueInMultiLevelSection_ThenAnInvalidOperationExceptionIsThrown()
        {
            var testItems = CreateTestItems();

            var mockRootSection = new Mock<IConfigurationSection>();
            mockRootSection.SetupGet(x => x.Path).Returns("test root section path");

            testItems.MockConfiguration.Setup(x => x.GetSection("test root section name"))
                .Returns(mockRootSection.Object);

            Assert.That(() => testItems.ServiceConfigurationPresenter.GetValueInMultiLevelSection(["test root section name", "some child section name"], "_"),
                Throws.InvalidOperationException.With.Message.EqualTo(
                    "Configuration section not found within parent configuration section: Section='some child section name', Parent Section='test root section path'"));
        }

        [Test]
        public void GivenAnUnknownValueKey_WhenIGetValueInMultiLevelSection_ThenAnInvalidOperationExceptionIsThrown()
        {
            var testItems = CreateTestItems();

            var mockChildSection = new Mock<IConfigurationSection>();
            mockChildSection.SetupGet(x => x.Path).Returns("test child section path");

            var mockMiddleSection = new Mock<IConfigurationSection>();
            mockMiddleSection.SetupGet(x => x.Path).Returns("test middle section path");
            mockMiddleSection.Setup(x => x.GetSection("test child section name")).Returns(mockChildSection.Object);

            var mockRootSection = new Mock<IConfigurationSection>();
            mockRootSection.SetupGet(x => x.Path).Returns("test root section path");
            mockRootSection.Setup(x => x.GetSection("test middle section name")).Returns(mockMiddleSection.Object);

            testItems.MockConfiguration.Setup(x => x.GetSection("test root section name")).Returns(mockRootSection.Object);

            Assert.That(() => testItems.ServiceConfigurationPresenter.GetValueInMultiLevelSection(
                    ["test root section name", "test middle section name", "test child section name"], "some value key"),
                Throws.InvalidOperationException.With.Message.EqualTo("Configuration key not found in section: Key='some value key', Section='test child section path'"));
        }

        [Test]
        public void GivenAKnownValueKey_WhenIGetValueInMultiLevelSection_ThenTheReferencedValueIsReturned()
        {
            var testItems = CreateTestItems();

            var mockChildSection = new Mock<IConfigurationSection>();
            mockChildSection.SetupGet(x => x.Path).Returns("test child section path");
            mockChildSection.SetupGet(x => x["test value key"]).Returns("test value");

            var mockMiddleSection = new Mock<IConfigurationSection>();
            mockMiddleSection.SetupGet(x => x.Path).Returns("test middle section path");
            mockMiddleSection.Setup(x => x.GetSection("test child section name")).Returns(mockChildSection.Object);

            var mockRootSection = new Mock<IConfigurationSection>();
            mockRootSection.SetupGet(x => x.Path).Returns("test root section path");
            mockRootSection.Setup(x => x.GetSection("test middle section name")).Returns(mockMiddleSection.Object);

            testItems.MockConfiguration.Setup(x => x.GetSection("test root section name")).Returns(mockRootSection.Object);

            var result = testItems.ServiceConfigurationPresenter.GetValueInMultiLevelSection(
                    ["test root section name", "test middle section name", "test child section name"],
                    "test value key");

            Assert.That(result, Is.EqualTo("test value"));
        }
        #endregion

        #region GetValuesInSection() Tests
        [Test]
        public void GivenAnEmptySectionName_WhenIGetValuesInSection_ThenAnArgumentExceptionIsThrown(
            [Values("", "  ")] string sectionName)
        {
            var testItems = CreateTestItems();

            Assert.That(() => testItems.ServiceConfigurationPresenter.GetValuesInSection(sectionName),
                Throws.ArgumentException.With.Property("ParamName").EqualTo("sectionName"));
        }

        [Test]
        public void GivenAnUnknownSectionName_WhenIGetValuesInSection_ThenAnInvalidOperationExceptionIsThrown()
        {
            var testItems = CreateTestItems();

            Assert.That(() => testItems.ServiceConfigurationPresenter.GetValuesInSection("some section name"),
                Throws.InvalidOperationException.With.Message.EqualTo("Configuration section not found: Section='some section name'"));
        }

        [Test]
        public void GivenTheNameOfASectionContainingASingleItem_WhenIGetValuesInSection_ThenACollectionContainingThatSingleItemIsReturned()
        {
            var testItems = CreateTestItems();

            var mockChildSection = new Mock<IConfigurationSection>();
            mockChildSection.Setup(x => x.Value).Returns("test value");

            var mockSection = Mock.Get(testItems.Fixture.Create<IConfigurationSection>());
            mockSection
                .Setup(x => x.GetChildren())
                .Returns([mockChildSection.Object]);

            testItems.MockConfiguration.Setup(x => x.GetSection("some section name"))
                .Returns(mockSection.Object);

            var result = testItems.ServiceConfigurationPresenter.GetValuesInSection("some section name").ToList();

            Assert.Multiple(() =>
            {
                Assert.That(result, Has.One.Items);

                Assert.That(result, Has.Member("test value"));
            });
        }

        [Test]
        public void GivenTheNameOfASectionContainingMultipleItems_WhenIGetValuesInSection_ThenACollectionContainingEachChildValueIsReturned()
        {
            var testItems = CreateTestItems();

            var childSections = Enumerable.Range(1, 3)
                .Select(number =>
                {
                    var mockChildSection = new Mock<IConfigurationSection>();
                    mockChildSection.Setup(x => x.Value).Returns($"test value {number}");

                    return mockChildSection.Object;
                });

            var mockSection = Mock.Get(testItems.Fixture.Create<IConfigurationSection>());
            mockSection.Setup(x => x.GetChildren())
                .Returns(childSections);

            testItems.MockConfiguration.Setup(x => x.GetSection("some section name"))
                .Returns(mockSection.Object);

            var result = testItems.ServiceConfigurationPresenter.GetValuesInSection("some section name").ToList();

            Assert.Multiple(() =>
            {
                Assert.That(result, Has.Exactly(3).Items);

                Assert.That(result, Has.Member("test value 1"));
                Assert.That(result, Has.Member("test value 2"));
                Assert.That(result, Has.Member("test value 3"));
            });
        }
        #endregion


        #region GetValuesInMultiLevelSection() Tests
        [Test]
        public void GivenAnCollectionListOfSectionNames_WhenIGetValuesInMultiLevelSection_ThenAnArgumentExceptionIsThrown()
        {
            var testItems = CreateTestItems();

            Assert.That(() => testItems.ServiceConfigurationPresenter.GetValuesInMultiLevelSection([]),
                Throws.ArgumentException.With.Property("ParamName").EqualTo("sectionNames"));
        }

        [Test]
        public void GivenAnUnknownRootSectionInAMultiLevelSectionName_WhenIGetValuesInMultiLevelSection_ThenAnInvalidOperationExceptionIsThrown()
        {
            var testItems = CreateTestItems();

            Assert.That(() => testItems.ServiceConfigurationPresenter.GetValuesInMultiLevelSection(["root section", "some section name"]),
                Throws.InvalidOperationException.With.Message.EqualTo("Configuration section not found at root of configuration: Section='root section'"));
        }

        [Test]
        public void GivenAnUnknownChildSectionInAMultiLevelSectionName_WhenIGetValuesInMultiLevelSection_ThenAnInvalidOperationExceptionIsThrown()
        {
            var testItems = CreateTestItems();

            var mockRootSection = new Mock<IConfigurationSection>();
            mockRootSection.SetupGet(x => x.Path).Returns("test root section path");

            testItems.MockConfiguration.Setup(x => x.GetSection("test root section name"))
                .Returns(mockRootSection.Object);

            Assert.That(() => testItems.ServiceConfigurationPresenter.GetValuesInMultiLevelSection(["test root section name", "some section name"]),
                Throws.InvalidOperationException.With.Message.EqualTo("Configuration section not found within parent configuration section: Section='some section name', Parent Section='test root section path'"));
        }

        [Test]
        public void GivenTheNameOfAMultiLevelSectionContainingASingleItem_WhenIGetValuesInMultiLevelSection_ThenACollectionContainingThatSingleItemIsReturned()
        {
            var testItems = CreateTestItems();

            var mockRootSection = new Mock<IConfigurationSection>();
            mockRootSection.SetupGet(x => x.Path).Returns("test root section path");

            testItems.MockConfiguration.Setup(x => x.GetSection("test root section name"))
                .Returns(mockRootSection.Object);
            
            var mockChildSection = new Mock<IConfigurationSection>();
            mockChildSection.Setup(x => x.Value).Returns("test value");

            var mockSubSection = Mock.Get(testItems.Fixture.Create<IConfigurationSection>());
            mockSubSection
                .Setup(x => x.GetChildren())
                .Returns([mockChildSection.Object]);

            mockRootSection.Setup(x => x.GetChildren())
                .Returns([mockSubSection.Object]);

            mockRootSection.Setup(x => x.GetSection("some section name"))
                .Returns(mockSubSection.Object);

            var result = testItems.ServiceConfigurationPresenter.GetValuesInMultiLevelSection(["test root section name", "some section name"]).ToList();

            Assert.Multiple(() =>
            {
                Assert.That(result, Has.One.Items);

                Assert.That(result, Has.Member("test value"));
            });
        }

        [Test]
        public void GivenTheNameOfAMultiLevelSectionContainingMultipleItems_WhenIGetValuesInMultiLevelSection_ThenACollectionContainingEachChildValueIsReturned()
        {
            var testItems = CreateTestItems();

            var mockRootSection = new Mock<IConfigurationSection>();
            mockRootSection.SetupGet(x => x.Path).Returns("test root section path");

            testItems.MockConfiguration.Setup(x => x.GetSection("test root section name"))
                .Returns(mockRootSection.Object);

            var childSections = Enumerable.Range(1, 3)
                .Select(number =>
                {
                    var mockChildSection = new Mock<IConfigurationSection>();
                    mockChildSection.Setup(x => x.Value).Returns($"test value {number}");

                    return mockChildSection.Object;
                });

            var mockSubSection = Mock.Get(testItems.Fixture.Create<IConfigurationSection>());
            mockSubSection
                .Setup(x => x.GetChildren())
                .Returns(childSections);

            mockRootSection.Setup(x => x.GetChildren())
                .Returns([mockSubSection.Object]);

            mockRootSection.Setup(x => x.GetSection("some section name"))
                .Returns(mockSubSection.Object);

            var result = testItems.ServiceConfigurationPresenter.GetValuesInMultiLevelSection(["test root section name", "some section name"]).ToList();

            Assert.Multiple(() =>
            {
                Assert.That(result, Has.Exactly(3).Items);

                Assert.That(result, Has.Member("test value 1"));
                Assert.That(result, Has.Member("test value 2"));
                Assert.That(result, Has.Member("test value 3"));
            });
        }
        #endregion

        #region Test Item Creation
        private static TestItems CreateTestItems()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());

            var mockConfiguration = new Mock<IConfiguration>();

            var serviceConfigurationPresenter = new ServiceConfigurationPresenter(
                mockConfiguration.Object);

            return new TestItems(
                fixture,
                serviceConfigurationPresenter,
                mockConfiguration);
        }

        private class TestItems(
            IFixture fixture,
            IServiceConfigurationPresenter serviceConfigurationPresenter,
            Mock<IConfiguration> mockConfiguration)
        {
            public IFixture Fixture { get; } = fixture;
            public IServiceConfigurationPresenter ServiceConfigurationPresenter { get; } = serviceConfigurationPresenter;
            public Mock<IConfiguration> MockConfiguration { get; } = mockConfiguration;
        }
        #endregion
    }
}
