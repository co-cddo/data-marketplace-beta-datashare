using Agrimetrics.DataShare.Api.Dto.Models.Questions.QuestionSets;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Dto.Test.Models.Questions.QuestionSets;

[TestFixture]
public class QuestionSetOutlineTests
{
    [Test]
    public void GivenAQuestionSetOutline_WhenISetAnEmptySetOfSections_ThenSectionsIsSet()
    {
        var testQuestionSetOutline = new QuestionSetOutline();

        var testSections = new List<QuestionSetSectionOutline>();

        testQuestionSetOutline.Sections = testSections;

        var result = testQuestionSetOutline.Sections;

        Assert.That(result, Is.EqualTo(testSections));
    }

    [Test]
    public void GivenAQuestionSetOutline_WhenISetSections_ThenSectionsIsSet()
    {
        var testQuestionSetOutline = new QuestionSetOutline();

        var testSections = new List<QuestionSetSectionOutline> {new(), new(), new()};

        testQuestionSetOutline.Sections = testSections;

        var result = testQuestionSetOutline.Sections;

        Assert.That(result, Is.EqualTo(testSections));
    }
}
