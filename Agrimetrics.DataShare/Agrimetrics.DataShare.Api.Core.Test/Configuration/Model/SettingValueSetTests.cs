using Agrimetrics.DataShare.Api.Core.Configuration.Model;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Core.Test.Configuration.Model;

[TestFixture]
public class SettingValueSetTests
{
    [Test]
    public void GivenAListOfDatabaseConnectionSettingValues_WhenISetDatabaseConnectionSettingValues_ThenDatabaseConnectionSettingValuesIsSet(
        [Values(0, 1, 3)] int numberOfValuesInList)
    {
        var testSettingValueSet = new SettingValueSet();

        var testDatabaseConnectionSettingValues = CreateTestSettingValueCollection(numberOfValuesInList);

        testSettingValueSet.DatabaseConnectionSettingValues = testDatabaseConnectionSettingValues;

        var result = testSettingValueSet.DatabaseConnectionSettingValues;

        Assert.That(result, Is.EqualTo(testDatabaseConnectionSettingValues));
    }

    [Test]
    public void GivenAListOfNotificationsSettingValues_WhenISetNotificationsSettingValues_ThenNotificationsSettingValuesIsSet(
        [Values(0, 1, 3)] int numberOfValuesInList)
    {
        var testSettingValueSet = new SettingValueSet();

        var testDatabaseConnectionSettingValues = CreateTestSettingValueCollection(numberOfValuesInList);

        testSettingValueSet.NotificationsSettingValues = testDatabaseConnectionSettingValues;

        var result = testSettingValueSet.NotificationsSettingValues;

        Assert.That(result, Is.EqualTo(testDatabaseConnectionSettingValues));
    }

    [Test]
    public void GivenAListOfUserServiceSettingValues_WhenISetUserServiceSettingValues_ThenUserServiceSettingValuesIsSet(
        [Values(0, 1, 3)] int numberOfValuesInList)
    {
        var testSettingValueSet = new SettingValueSet();

        var testDatabaseConnectionSettingValues = CreateTestSettingValueCollection(numberOfValuesInList);

        testSettingValueSet.UserServiceSettingValues = testDatabaseConnectionSettingValues;

        var result = testSettingValueSet.UserServiceSettingValues;

        Assert.That(result, Is.EqualTo(testDatabaseConnectionSettingValues));
    }

    [Test]
    public void GivenAListOfDatasetInformationSettingValues_WhenISetDatasetInformationSettingValues_ThenDatasetInformationSettingValuesIsSet(
        [Values(0, 1, 3)] int numberOfValuesInList)
    {
        var testSettingValueSet = new SettingValueSet();

        var testDatabaseConnectionSettingValues = CreateTestSettingValueCollection(numberOfValuesInList);

        testSettingValueSet.DatasetInformationSettingValues = testDatabaseConnectionSettingValues;

        var result = testSettingValueSet.DatasetInformationSettingValues;

        Assert.That(result, Is.EqualTo(testDatabaseConnectionSettingValues));
    }

    [Test]
    public void GivenAListOfPageLinksSettingValues_WhenISetPageLinksSettingValues_ThenPageLinksSettingValuesIsSet(
        [Values(0, 1, 3)] int numberOfValuesInList)
    {
        var testSettingValueSet = new SettingValueSet();

        var testDatabaseConnectionSettingValues = CreateTestSettingValueCollection(numberOfValuesInList);

        testSettingValueSet.PageLinksSettingValues = testDatabaseConnectionSettingValues;

        var result = testSettingValueSet.PageLinksSettingValues;

        Assert.That(result, Is.EqualTo(testDatabaseConnectionSettingValues));
    }

    #region Test Data Creation
    private static List<SettingValue> CreateTestSettingValueCollection(
        int numberOfItems)
    {
        return Enumerable.Range(1, numberOfItems)
            .Select(number => new SettingValue
            {
                Description = $"test setting value {number} description",
                Value = $"test setting value {number} value",
            }).ToList();
    }
    #endregion
}