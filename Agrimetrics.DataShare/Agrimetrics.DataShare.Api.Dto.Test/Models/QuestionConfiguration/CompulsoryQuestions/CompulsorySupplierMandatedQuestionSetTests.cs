using Agrimetrics.DataShare.Api.Dto.Models.QuestionConfiguration.CompulsoryQuestions;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Dto.Test.Models.QuestionConfiguration.CompulsoryQuestions;

[TestFixture]
public class CompulsorySupplierMandatedQuestionSetTests
{
    [Test]
    public void GivenACompulsorySupplierMandatedQuestionSet_WhenISetAnEmptySetOfCompulsorySupplierMandatedQuestions_ThenCompulsorySupplierMandatedQuestionsIsSet()
    {
        var testCompulsorySupplierMandatedQuestionSet = new CompulsorySupplierMandatedQuestionSet();

        var testCompulsorySupplierMandatedQuestions = new List<CompulsorySupplierMandatedQuestion>();

        testCompulsorySupplierMandatedQuestionSet.CompulsorySupplierMandatedQuestions = testCompulsorySupplierMandatedQuestions;

        var result = testCompulsorySupplierMandatedQuestionSet.CompulsorySupplierMandatedQuestions;

        Assert.That(result, Is.EqualTo(testCompulsorySupplierMandatedQuestions));
    }

    [Test]
    public void GivenACompulsorySupplierMandatedQuestionSet_WhenISetCompulsorySupplierMandatedQuestions_ThenCompulsorySupplierMandatedQuestionsIsSet()
    {
        var testCompulsorySupplierMandatedQuestionSet = new CompulsorySupplierMandatedQuestionSet();

        var testCompulsorySupplierMandatedQuestions = new List<CompulsorySupplierMandatedQuestion> {new(), new(), new()};

        testCompulsorySupplierMandatedQuestionSet.CompulsorySupplierMandatedQuestions = testCompulsorySupplierMandatedQuestions;

        var result = testCompulsorySupplierMandatedQuestionSet.CompulsorySupplierMandatedQuestions;

        Assert.That(result, Is.EqualTo(testCompulsorySupplierMandatedQuestions));
    }
}