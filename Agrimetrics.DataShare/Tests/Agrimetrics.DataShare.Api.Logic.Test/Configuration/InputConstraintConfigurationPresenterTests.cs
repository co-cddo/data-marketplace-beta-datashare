using Agrimetrics.DataShare.Api.Core.Configuration;
using Agrimetrics.DataShare.Api.Logic.Configuration;
using Moq;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.Configuration;

[TestFixture]
public class InputConstraintConfigurationPresenterTests
{
    #region GetMaximumLengthOfFreeFormTextResponse() Tests
    [Test]
    public void GivenNoValueIsConfigured_WhenIGetMaximumLengthOfFreeFormTextResponse_ThenADefaultValueIsReturned()
    {
        var testItems = CreateTestItems();

        var result = testItems.InputConstraintConfigurationPresenter.GetMaximumLengthOfFreeFormTextResponse();

        Assert.That(result, Is.EqualTo(4000));
    }

    [Test]
    public void GivenGettingTheValueWillThrowAnException_WhenIGetMaximumLengthOfFreeFormTextResponse_ThenADefaultValueIsReturned()
    {
        var testItems = CreateTestItems();

        testItems.MockServiceConfigurationPresenter.Setup(x => x.GetValueInSection(
                "InputConstraints", "MaximumLengthOfFreeFormTextResponse"))
            .Throws(() => new Exception("oh noes!"));

        var result = testItems.InputConstraintConfigurationPresenter.GetMaximumLengthOfFreeFormTextResponse();

        Assert.That(result, Is.EqualTo(4000));
    }

    [Test]
    public void GivenAnInvalidValueIsConfigured_WhenIGetMaximumLengthOfFreeFormTextResponse_ThenADefaultValueIsReturned()
    {
        var testItems = CreateTestItems();

        testItems.MockServiceConfigurationPresenter.Setup(x => x.GetValueInSection(
                "InputConstraints", "MaximumLengthOfFreeFormTextResponse"))
            .Returns("not-a-number");

        var result = testItems.InputConstraintConfigurationPresenter.GetMaximumLengthOfFreeFormTextResponse();

        Assert.That(result, Is.EqualTo(4000));
    }

    [Test]
    public void GivenAValidValueIsConfigured_WhenIGetMaximumLengthOfFreeFormTextResponse_ThenTheConfiguredValueIsReturned()
    {
        var testItems = CreateTestItems();

        testItems.MockServiceConfigurationPresenter.Setup(x => x.GetValueInSection(
                "InputConstraints", "MaximumLengthOfFreeFormTextResponse"))
            .Returns("12345");

        var result = testItems.InputConstraintConfigurationPresenter.GetMaximumLengthOfFreeFormTextResponse();

        Assert.That(result, Is.EqualTo(12345));
    }
    #endregion

    #region GetMaximumLengthOfFreeFormMultiResponseTextResponse() Tests
    [Test]
    public void GivenNoValueIsConfigured_WhenIGetMaximumLengthOfFreeFormMultiResponseTextResponse_ThenADefaultValueIsReturned()
    {
        var testItems = CreateTestItems();

        var result = testItems.InputConstraintConfigurationPresenter.GetMaximumLengthOfFreeFormMultiResponseTextResponse();

        Assert.That(result, Is.EqualTo(250));
    }

    [Test]
    public void GivenGettingTheValueWillThrowAnException_WhenIGetMaximumLengthOfFreeFormMultiResponseTextResponse_ThenADefaultValueIsReturned()
    {
        var testItems = CreateTestItems();

        testItems.MockServiceConfigurationPresenter.Setup(x => x.GetValueInSection(
                "InputConstraints", "MaximumLengthOfFreeFormMultiResponseTextResponse"))
            .Throws(() => new Exception("oh noes!"));

        var result = testItems.InputConstraintConfigurationPresenter.GetMaximumLengthOfFreeFormMultiResponseTextResponse();

        Assert.That(result, Is.EqualTo(250));
    }

    [Test]
    public void GivenAnInvalidValueIsConfigured_WhenIGetMaximumLengthOfFreeFormMultiResponseTextResponse_ThenADefaultValueIsReturned()
    {
        var testItems = CreateTestItems();

        testItems.MockServiceConfigurationPresenter.Setup(x => x.GetValueInSection(
                "InputConstraints", "MaximumLengthOfFreeFormMultiResponseTextResponse"))
            .Returns("not-a-number");

        var result = testItems.InputConstraintConfigurationPresenter.GetMaximumLengthOfFreeFormMultiResponseTextResponse();

        Assert.That(result, Is.EqualTo(250));
    }

    [Test]
    public void GivenAValidValueIsConfigured_WhenIGetMaximumLengthOfFreeFormMultiResponseTextResponse_ThenTheConfiguredValueIsReturned()
    {
        var testItems = CreateTestItems();

        testItems.MockServiceConfigurationPresenter.Setup(x => x.GetValueInSection(
                "InputConstraints", "MaximumLengthOfFreeFormMultiResponseTextResponse"))
            .Returns("12345");

        var result = testItems.InputConstraintConfigurationPresenter.GetMaximumLengthOfFreeFormMultiResponseTextResponse();

        Assert.That(result, Is.EqualTo(12345));
    }
    #endregion

    #region GetMaximumLengthOfSupplementaryTextResponse() Tests
    [Test]
    public void GivenNoValueIsConfigured_WhenIGetMaximumLengthOfSupplementaryTextResponse_ThenADefaultValueIsReturned()
    {
        var testItems = CreateTestItems();

        var result = testItems.InputConstraintConfigurationPresenter.GetMaximumLengthOfSupplementaryTextResponse();

        Assert.That(result, Is.EqualTo(250));
    }

    [Test]
    public void GivenGettingTheValueWillThrowAnException_WhenIGetMaximumLengthOfSupplementaryTextResponse_ThenADefaultValueIsReturned()
    {
        var testItems = CreateTestItems();

        testItems.MockServiceConfigurationPresenter.Setup(x => x.GetValueInSection(
                "InputConstraints", "MaximumLengthOfSupplementaryTextResponse"))
            .Throws(() => new Exception("oh noes!"));

        var result = testItems.InputConstraintConfigurationPresenter.GetMaximumLengthOfSupplementaryTextResponse();

        Assert.That(result, Is.EqualTo(250));
    }

    [Test]
    public void GivenAnInvalidValueIsConfigured_WhenIGetMaximumLengthOfSupplementaryTextResponse_ThenADefaultValueIsReturned()
    {
        var testItems = CreateTestItems();

        testItems.MockServiceConfigurationPresenter.Setup(x => x.GetValueInSection(
                "InputConstraints", "MaximumLengthOfSupplementaryTextResponse"))
            .Returns("not-a-number");

        var result = testItems.InputConstraintConfigurationPresenter.GetMaximumLengthOfSupplementaryTextResponse();

        Assert.That(result, Is.EqualTo(250));
    }

    [Test]
    public void GivenAValidValueIsConfigured_WhenIGetMaximumLengthOfSupplementaryTextResponse_ThenTheConfiguredValueIsReturned()
    {
        var testItems = CreateTestItems();

        testItems.MockServiceConfigurationPresenter.Setup(x => x.GetValueInSection(
                "InputConstraints", "MaximumLengthOfSupplementaryTextResponse"))
            .Returns("12345");

        var result = testItems.InputConstraintConfigurationPresenter.GetMaximumLengthOfSupplementaryTextResponse();

        Assert.That(result, Is.EqualTo(12345));
    }
    #endregion

    #region GetAllSettingValues()
    [Test]
    public void GivenAnInputConstraintConfigurationPresenter_WhenIGetAllSettingValues_ThenADescriptionOfAllSettingsIsReturned()
    {
        var testItems = CreateTestItems();

        const string testMaximumLengthOfFreeFormTextResponse = "100";
        const string testMaximumLengthOfFreeFormMultiResponseTextResponse = "200";
        const string testMaximumLengthOfSupplementaryTextResponse = "300";

        testItems.MockServiceConfigurationPresenter.Setup(x => x.GetValueInSection(
                "InputConstraints", "MaximumLengthOfFreeFormTextResponse"))
            .Returns(() => testMaximumLengthOfFreeFormTextResponse);

        testItems.MockServiceConfigurationPresenter.Setup(x => x.GetValueInSection(
                "InputConstraints", "MaximumLengthOfFreeFormMultiResponseTextResponse"))
            .Returns(() => testMaximumLengthOfFreeFormMultiResponseTextResponse);

        testItems.MockServiceConfigurationPresenter.Setup(x => x.GetValueInSection(
                "InputConstraints", "MaximumLengthOfSupplementaryTextResponse"))
            .Returns(() => testMaximumLengthOfSupplementaryTextResponse);

        var result = testItems.InputConstraintConfigurationPresenter.GetAllSettingValues().ToList();

        Assert.Multiple(() =>
        {
            Assert.That(result, Has.Exactly(3).Items);

            Assert.That(result.Any(x =>
                    x is {Description: "Maximum Length Of FreeForm Text Response", Value: testMaximumLengthOfFreeFormTextResponse}),
                Is.True);

            Assert.That(result.Any(x =>
                    x is {Description: "Maximum Length Of FreeForm Multi-Response Text Response", Value: testMaximumLengthOfFreeFormMultiResponseTextResponse }),
                Is.True);

            Assert.That(result.Any(x =>
                    x is {Description: "Maximum Length Of Supplementary Text Response", Value: testMaximumLengthOfSupplementaryTextResponse }),
                Is.True);
        });

    }

    [Test]
    public void GivenGettingAConfigurationValueWithThrowAnException_WhenIGetAllSettingValues_ThenAnEmptyStringIsReturnedForThatValue()
    {
        var testItems = CreateTestItems();

        testItems.MockServiceConfigurationPresenter.Setup(x => x.GetValueInSection(
                "InputConstraints", "MaximumLengthOfFreeFormTextResponse"))
            .Throws(new Exception("error 1"));

        testItems.MockServiceConfigurationPresenter.Setup(x => x.GetValueInSection(
                "InputConstraints", "MaximumLengthOfFreeFormMultiResponseTextResponse"))
            .Returns("123");

        testItems.MockServiceConfigurationPresenter.Setup(x => x.GetValueInSection(
                "InputConstraints", "MaximumLengthOfSupplementaryTextResponse"))
            .Throws(new Exception("error 2"));

        var result = testItems.InputConstraintConfigurationPresenter.GetAllSettingValues().ToList();

        Assert.Multiple(() =>
        {
            var valuesNotFound = result.Where(x =>
                x.Value.StartsWith("ERROR")).ToList();

            Assert.That(valuesNotFound, Has.Exactly(2).Items);

            Assert.That(result.FirstOrDefault(x =>
                x is { Description: "Maximum Length Of FreeForm Text Response", Value: "ERROR: error 1" }), Is.Not.Null);

            Assert.That(result.FirstOrDefault(x =>
                x is { Description: "Maximum Length Of Supplementary Text Response", Value: "ERROR: error 2" }), Is.Not.Null);
        });
    }
    #endregion

    #region Test Item Creation
    private static TestItems CreateTestItems()
    {
        var mockServiceConfigurationPresenter = new Mock<IServiceConfigurationPresenter>();

        var inputConstraintConfigurationPresenter = new InputConstraintConfigurationPresenter(
            mockServiceConfigurationPresenter.Object);

        return new TestItems(
            inputConstraintConfigurationPresenter,
            mockServiceConfigurationPresenter);
    }

    private class TestItems(
        IInputConstraintConfigurationPresenter inputConstraintConfigurationPresenter,
        Mock<IServiceConfigurationPresenter> mockServiceConfigurationPresenter)
    {
        public IInputConstraintConfigurationPresenter InputConstraintConfigurationPresenter { get; } = inputConstraintConfigurationPresenter;

        public Mock<IServiceConfigurationPresenter> MockServiceConfigurationPresenter { get; } = mockServiceConfigurationPresenter;
    }
    #endregion
}