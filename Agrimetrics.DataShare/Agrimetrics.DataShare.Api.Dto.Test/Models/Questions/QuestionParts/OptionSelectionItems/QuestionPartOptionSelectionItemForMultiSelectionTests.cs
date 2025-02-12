using Agrimetrics.DataShare.Api.Dto.Models.Questions.QuestionParts.OptionSelectionItems;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Dto.Test.Models.Questions.QuestionParts.OptionSelectionItems;

[TestFixture]
public class QuestionPartOptionSelectionItemForMultiSelectionTests
{
    [Theory]
    public void GivenAQuestionPartOptionSelectionItemForMultiSelection_WhenISetIsMaster_ThenIsMasterIsSet(
        bool testIsMaster)
    {
        var testQuestionPartOptionSelectionItemForMultiSelection = new QuestionPartOptionSelectionItemForMultiSelection();

        testQuestionPartOptionSelectionItemForMultiSelection.IsMaster = testIsMaster;

        var result = testQuestionPartOptionSelectionItemForMultiSelection.IsMaster;

        Assert.That(result, Is.EqualTo(testIsMaster));
    }
}