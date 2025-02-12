using NUnit.Framework;
using Agrimetrics.DataShare.Api.Logic.Services.AnswerValidation.Validation.ValidationRules;

namespace Agrimetrics.DataShare.Api.Logic.Test.Services.AnswerValidation.Validation;

[TestFixture]
public class ResponseFormatterTests
{
    #region TryFormatNumericResponse() Tests
    [Test]
    [TestCaseSource(nameof(ValidNumericValueTestCaseData))]
    public void GivenAValidNumericValue_WhenITryFormatNumericResponse_ThenTheValueIsParsedSuccessfully(
        string numericValue,
        int expectedParsedNumber)
    {
        var testItems = CreateTestItems();

        var parsedOk = testItems.ResponseFormatter.TryFormatNumericResponse(numericValue, out var parsedNumber);

        Assert.Multiple(() =>
        {
            Assert.That(parsedOk, Is.True);
            Assert.That(parsedNumber, Is.EqualTo(expectedParsedNumber));
        });
    }

    private static IEnumerable<TestCaseData> ValidNumericValueTestCaseData()
    {
        yield return new TestCaseData("0", 0);
        yield return new TestCaseData("123", 123);
        yield return new TestCaseData("-123", -123);
    }

    [Test]
    [TestCaseSource(nameof(InvalidNumericValueTestCaseData))]
    public void GivenAnInvalidNumericValue_WhenITryFormatNumericResponse_ThenTheValueIsNotParsedSuccessfully(
        string dateValue)
    {
        var testItems = CreateTestItems();

        var parsedOk = testItems.ResponseFormatter.TryFormatNumericResponse(dateValue, out var parsedNumber);

        Assert.Multiple(() =>
        {
            Assert.That(parsedOk, Is.False);
            Assert.That(parsedNumber, Is.Null);
        });
    }

    private static IEnumerable<TestCaseData> InvalidNumericValueTestCaseData()
    {
        yield return new TestCaseData("1.111");
        yield return new TestCaseData("1.0");
        yield return new TestCaseData("1,234");
        yield return new TestCaseData("");
        yield return new TestCaseData("a123");
    }
    #endregion

    #region TryFormatDateResponse() Tests
    [Test]
    [TestCaseSource(nameof(ValidDateValueTestCaseData))]
    public void GivenAValidDateValue_WhenITryFormatDateResponse_ThenTheValueIsParsedSuccessfully(
        string dateValue,
        DateTime expectedParsedDate)
    {
        var testItems = CreateTestItems();

        var parsedOk = testItems.ResponseFormatter.TryFormatDateResponse(dateValue, out var parsedDate);

        Assert.Multiple(() =>
        {
            Assert.That(parsedOk, Is.True);
            Assert.That(parsedDate, Is.EqualTo(expectedParsedDate));
        });
    }

    private static IEnumerable<TestCaseData> ValidDateValueTestCaseData()
    {
        yield return new TestCaseData("20241225", new DateTime(2024, 12, 25));
        yield return new TestCaseData("20220101", new DateTime(2022, 1, 1));
    }

    [Test]
    [TestCaseSource(nameof(InvalidDateValueTestCaseData))]
    public void GivenAnInvalidDateValue_WhenITryFormatDateResponse_ThenTheValueIsNotParsedSuccessfully(
        string dateValue)
    {
        var testItems = CreateTestItems();

        var parsedOk = testItems.ResponseFormatter.TryFormatDateResponse(dateValue, out var parsedDate);

        Assert.Multiple(() =>
        {
            Assert.That(parsedOk, Is.False);
            Assert.That(parsedDate, Is.Null);
        });
    }

    private static IEnumerable<TestCaseData> InvalidDateValueTestCaseData()
    {
        yield return new TestCaseData("20241131");
        yield return new TestCaseData("2024-12-25");
        yield return new TestCaseData("24-12-25");
        yield return new TestCaseData("");
        yield return new TestCaseData("a20241225");
    }
    #endregion

    #region TryFormatTimeResponse() Tests
    [Test]
    [TestCaseSource(nameof(ValidTimeValueTestCaseData))]
    public void GivenAValidTimeValue_WhenITryFormatTimeResponse_ThenTheValueIsParsedSuccessfully(
        string timeValue,
        TimeSpan expectedParsedTime)
    {
        var testItems = CreateTestItems();

        var parsedOk = testItems.ResponseFormatter.TryFormatTimeResponse(timeValue, out var parsedTime);

        Assert.Multiple(() =>
        {
            Assert.That(parsedOk, Is.True);
            Assert.That(parsedTime, Is.EqualTo(expectedParsedTime));
        });
    }

    private static IEnumerable<TestCaseData> ValidTimeValueTestCaseData()
    {
        yield return new TestCaseData("01:01:01", new TimeSpan(1, 1, 1));
        yield return new TestCaseData("23:59:59", new TimeSpan(23, 59, 59));
    }

    [Test]
    [TestCaseSource(nameof(InvalidTimeValueTestCaseData))]
    public void GivenAnInvalidTimeValue_WhenITryFormatTimeResponse_ThenTheValueIsNotParsedSuccessfully(
        string timeValue)
    {
        var testItems = CreateTestItems();

        var parsedOk = testItems.ResponseFormatter.TryFormatTimeResponse(timeValue, out var parsedTime);

        Assert.Multiple(() =>
        {
            Assert.That(parsedOk, Is.False);
            Assert.That(parsedTime, Is.Null);
        });
    }

    private static IEnumerable<TestCaseData> InvalidTimeValueTestCaseData()
    {
        yield return new TestCaseData("24:00:00");
        yield return new TestCaseData("24:00:01");
        yield return new TestCaseData("01:60:01");
        yield return new TestCaseData("01:01:60");
        yield return new TestCaseData("01:01:01.987");
        yield return new TestCaseData("");
        yield return new TestCaseData("a01:01:01");
    }
    #endregion

    #region TryFormatDateTimeResponse() Tests
    [Test]
    [TestCaseSource(nameof(ValidDateTimeValueTestCaseData))]
    public void GivenAValidDateTimeValue_WhenITryFormatDateTimeResponse_ThenTheValueIsParsedSuccessfully(
        string dateTimeValue,
        DateTime expectedParsedDateTime)
    {
        var testItems = CreateTestItems();

        var parsedOk = testItems.ResponseFormatter.TryFormatDateTimeResponse(dateTimeValue, out var parsedDateTime);

        Assert.Multiple(() =>
        {
            Assert.That(parsedOk, Is.True);
            Assert.That(parsedDateTime, Is.EqualTo(expectedParsedDateTime));
        });
    }

    private static IEnumerable<TestCaseData> ValidDateTimeValueTestCaseData()
    {
        yield return new TestCaseData("20241225 01:01:01", new DateTime(2024, 12, 25, 1, 1, 1));
        yield return new TestCaseData("20220101 23:59:59", new DateTime(2022, 1, 1, 23, 59, 59));
    }

    [Test]
    [TestCaseSource(nameof(InvalidDateTimeValueTestCaseData))]
    public void GivenAnInvalidDateTimeValue_WhenITryFormatDateTimeResponse_ThenTheValueIsNotParsedSuccessfully(
        string dateTimeValue)
    {
        var testItems = CreateTestItems();

        var parsedOk = testItems.ResponseFormatter.TryFormatDateTimeResponse(dateTimeValue, out var parsedDateTime);

        Assert.Multiple(() =>
        {
            Assert.That(parsedOk, Is.False);
            Assert.That(parsedDateTime, Is.Null);
        });
    }

    private static IEnumerable<TestCaseData> InvalidDateTimeValueTestCaseData()
    {
        yield return new TestCaseData("20240101 01:01:01.987");
        yield return new TestCaseData("20241131 01:01:01");
        yield return new TestCaseData("20241231 24:01:01");
        yield return new TestCaseData("");
        yield return new TestCaseData("01:01:01");
        yield return new TestCaseData("20241231");
        yield return new TestCaseData("120220101 23:59:59");
    }
    #endregion

    #region Test Item Creation
    private static TestItems CreateTestItems()
    {
        var responseFormatter = new ResponseFormatter();

        return new TestItems(
            responseFormatter);
    }

    private class TestItems(
        IResponseFormatter responseFormatter)
    {
        public IResponseFormatter ResponseFormatter { get; } = responseFormatter;
    }
    #endregion
}