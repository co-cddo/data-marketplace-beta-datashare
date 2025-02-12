using Agrimetrics.DataShare.Api.Core.Configuration.Model;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Core.Test.Configuration.Model
{
    [TestFixture]
    public class SettingValueTests
    {
        [Test]
        public void GivenAnyDescription_WhenISetDescription_ThenDescriptionIsSet(
            [Values("", "  ", "abc")] string testDescription)
        {
            var testSettingValue = new SettingValue();

            testSettingValue.Description = testDescription;

            var result = testSettingValue.Description;

            Assert.That(result, Is.EqualTo(testDescription));
        }

        [Test]
        public void GivenAnyValue_WhenISetValue_ThenValueIsSet(
            [Values("", "  ", "abc")] string testValue)
        {
            var testSettingValue = new SettingValue();

            testSettingValue.Value = testValue;

            var result = testSettingValue.Value;

            Assert.That(result, Is.EqualTo(testValue));
        }
    }
}
