using Agrimetrics.DataShare.Api.Dto.Models.Questions.QuestionParts.FreeFormItems;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Dto.Test.Models.Questions.QuestionParts.FreeFormItems;

[TestFixture]
public class QuestionPartFreeFormOptionsTests
{
    [Test]
    public void GivenAQuestionPartFreeFormOptions_WhenISetId_ThenIdIsSet()
    {
        var testQuestionPartFreeFormOptions = new QuestionPartFreeFormOptions();

        var testId = new Guid("8A130159-DC03-407C-8DF6-35B0B28373C7");

        testQuestionPartFreeFormOptions.Id = testId;

        var result = testQuestionPartFreeFormOptions.Id;

        Assert.That(result, Is.EqualTo(testId));
    }

    [Theory]
    public void GivenAQuestionPartFreeFormOptions_WhenISetValueEntryMayBeDeclined_ThenValueEntryMayBeDeclinedIsSet(
        bool testValueEntryMayBeDeclined)
    {
        var testQuestionPartFreeFormOptions = new QuestionPartFreeFormOptions();

        testQuestionPartFreeFormOptions.ValueEntryMayBeDeclined = testValueEntryMayBeDeclined;

        var result = testQuestionPartFreeFormOptions.ValueEntryMayBeDeclined;

        Assert.That(result, Is.EqualTo(testValueEntryMayBeDeclined));
    }
}

