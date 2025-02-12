using Agrimetrics.DataShare.Api.Dto.Models.QuestionConfiguration.CompulsoryQuestions;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Dto.Test.Models.QuestionConfiguration.CompulsoryQuestions;

[TestFixture]
public class CompulsorySupplierMandatedQuestionTests
{
    [Test]
    public void GivenACompulsorySupplierMandatedQuestion_WhenISetSupplierOrganisationId_ThenSupplierOrganisationIdIsSet(
        [Values(-1, 0, 999)] int testSupplierOrganisationId)
    {
        var testCompulsorySupplierMandatedQuestion = new CompulsorySupplierMandatedQuestion();

        testCompulsorySupplierMandatedQuestion.SupplierOrganisationId = testSupplierOrganisationId;

        var result = testCompulsorySupplierMandatedQuestion.SupplierOrganisationId;

        Assert.That(result, Is.EqualTo(testSupplierOrganisationId));
    }

    [Test]
    public void GivenACompulsorySupplierMandatedQuestion_WhenISetQuestionId_ThenQuestionIdIsSet()
    {
        var testCompulsorySupplierMandatedQuestion = new CompulsorySupplierMandatedQuestion();

        var testQuestionId = new Guid("666FCD37-B397-45AE-ADFF-A6AD1E48A683");

        testCompulsorySupplierMandatedQuestion.QuestionId = testQuestionId;

        var result = testCompulsorySupplierMandatedQuestion.QuestionId;

        Assert.That(result, Is.EqualTo(testQuestionId));
    }
}