using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.ModelData.DataShareRequests;

[TestFixture]
public class QuestionSetOutlineModelDataTests
{
    [Test]
    public void GivenAQuestionSetOutlineModelData_WhenISetId_ThenIdIsSet()
    {
        var testQuestionSetOutlineModelData = new QuestionSetOutlineModelData();

        var testId = new Guid("39A40D9D-37D4-4AAA-A1CE-4BBA1C9FDD32");

        testQuestionSetOutlineModelData.QuestionSetOutline_Id = testId;

        var result = testQuestionSetOutlineModelData.QuestionSetOutline_Id;

        Assert.That(result, Is.EqualTo(testId));
    }

    [Test]
    public void GivenAQuestionSetOutlineModelData_WhenISetEsdaId_ThenEsdaIdIsSet()
    {
        var testQuestionSetOutlineModelData = new QuestionSetOutlineModelData();

        var testEsdaId = new Guid("39A40D9D-37D4-4AAA-A1CE-4BBA1C9FDD32");

        testQuestionSetOutlineModelData.QuestionSetOutline_EsdaId = testEsdaId;

        var result = testQuestionSetOutlineModelData.QuestionSetOutline_EsdaId;

        Assert.That(result, Is.EqualTo(testEsdaId));
    }

    [Test]
    public void GivenAQuestionSetOutlineModelData_WhenISetSupplierDomain_ThenSupplierDomainIsSet(
        [Values(-1, 0, 999)] int testSupplierDomain)
    {
        var testQuestionSetOutlineModelData = new QuestionSetOutlineModelData();

        testQuestionSetOutlineModelData.QuestionSetOutline_SupplierDomain = testSupplierDomain;

        var result = testQuestionSetOutlineModelData.QuestionSetOutline_SupplierDomain;

        Assert.That(result, Is.EqualTo(testSupplierDomain));
    }

    [Test]
    public void GivenAQuestionSetOutlineModelData_WhenISetSupplierOrganisation_ThenSupplierOrganisationIsSet(
        [Values(-1, 0, 999)] int testSupplierOrganisation)
    {
        var testQuestionSetOutlineModelData = new QuestionSetOutlineModelData();

        testQuestionSetOutlineModelData.QuestionSetOutline_SupplierOrganisation = testSupplierOrganisation;

        var result = testQuestionSetOutlineModelData.QuestionSetOutline_SupplierOrganisation;

        Assert.That(result, Is.EqualTo(testSupplierOrganisation));
    }

    [Test]
    public void GivenAQuestionSetOutlineModelData_WhenISetAnEmptySetOfSections_ThenSectionsIsSet()
    {
        var testQuestionSetOutlineModelData = new QuestionSetOutlineModelData();

        var testSections = new List<QuestionSetSectionOutlineModelData>();

        testQuestionSetOutlineModelData.Sections = testSections;

        var result = testQuestionSetOutlineModelData.Sections;

        Assert.That(result, Is.EqualTo(testSections));
    }

    [Test]
    public void GivenAQuestionSetOutlineModelData_WhenISetSections_ThenSectionsIsSet()
    {
        var testQuestionSetOutlineModelData = new QuestionSetOutlineModelData();

        var testSections = new List<QuestionSetSectionOutlineModelData> {new(), new(), new()};

        testQuestionSetOutlineModelData.Sections = testSections;

        var result = testQuestionSetOutlineModelData.Sections;

        Assert.That(result, Is.EqualTo(testSections));
    }
}