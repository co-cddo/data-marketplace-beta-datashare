using Agrimetrics.DataShare.Api.Logic.ModelData.QuestionConfiguration.CompulsoryQuestions;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.ModelData.QuestionConfiguration.CompulsoryQuestions;

[TestFixture]
public class CompulsorySupplierMandatedQuestionModelDataTests
{
    [Test]
    public void GivenACompulsorySupplierMandatedQuestionModelData_WhenISetSupplierOrganisationId_ThenSupplierOrganisationIdIsSet(
        [Values(-1, 0, 999)] int testSupplierOrganisationId)
    {
        var testCompulsorySupplierMandatedQuestionModelData = new CompulsorySupplierMandatedQuestionModelData();

        testCompulsorySupplierMandatedQuestionModelData.CompulsorySupplierMandatedQuestion_SupplierOrganisationId = testSupplierOrganisationId;

        var result = testCompulsorySupplierMandatedQuestionModelData.CompulsorySupplierMandatedQuestion_SupplierOrganisationId;

        Assert.That(result, Is.EqualTo(testSupplierOrganisationId));
    }

    [Test]
    public void GivenACompulsorySupplierMandatedQuestionModelData_WhenISetQuestionId_ThenQuestionIdIsSet()
    {
        var testCompulsorySupplierMandatedQuestionModelData = new CompulsorySupplierMandatedQuestionModelData();

        var testQuestionId = new Guid("1F0A6743-EC9F-4EEF-8FCC-15B73B7A1576");

        testCompulsorySupplierMandatedQuestionModelData.CompulsorySupplierMandatedQuestion_QuestionId = testQuestionId;

        var result = testCompulsorySupplierMandatedQuestionModelData.CompulsorySupplierMandatedQuestion_QuestionId;

        Assert.That(result, Is.EqualTo(testQuestionId));
    }
}