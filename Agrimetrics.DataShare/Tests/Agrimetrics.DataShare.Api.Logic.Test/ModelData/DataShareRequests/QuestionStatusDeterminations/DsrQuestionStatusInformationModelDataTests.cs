using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests.QuestionStatusDeterminations;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.ModelData.DataShareRequests.QuestionStatusDeterminations;

[TestFixture]
public class DataShareRequestQuestionStatusInformationModelDataTests
{
    [Test]
    public void GivenADataShareRequestQuestionStatusInformationModelData_WhenISetQuestionId_ThenQuestionIdIsSet()
    {
        var testDataShareRequestQuestionStatusInformationModelData = new DataShareRequestQuestionStatusInformationModelData();

        var testQuestionId = new Guid("F591005B-FF83-4804-93DD-95A0D6185C39");

        testDataShareRequestQuestionStatusInformationModelData.DataShareRequestQuestionStatus_QuestionId = testQuestionId;

        var result = testDataShareRequestQuestionStatusInformationModelData.DataShareRequestQuestionStatus_QuestionId;

        Assert.That(result, Is.EqualTo(testQuestionId));
    }

    [Test]
    public void GivenADataShareRequestQuestionStatusInformationModelData_WhenISetQuestionSetQuestionInformation_ThenQuestionSetQuestionInformationIsSet()
    {
        var testDataShareRequestQuestionStatusInformationModelData = new DataShareRequestQuestionStatusInformationModelData();

        var testQuestionSetQuestionInformation = new QuestionSetQuestionInformationModelData();

        testDataShareRequestQuestionStatusInformationModelData.QuestionSetQuestionInformation = testQuestionSetQuestionInformation;

        var result = testDataShareRequestQuestionStatusInformationModelData.QuestionSetQuestionInformation;

        Assert.That(result, Is.EqualTo(testQuestionSetQuestionInformation));
    }

    [Test]
    public void GivenADataShareRequestQuestionStatusInformationModelData_WhenISetQuestionResponseInformation_ThenQuestionResponseInformationIsSet()
    {
        var testDataShareRequestQuestionStatusInformationModelData = new DataShareRequestQuestionStatusInformationModelData();

        var testQuestionResponseInformation = new QuestionResponseInformationDataModel();

        testDataShareRequestQuestionStatusInformationModelData.QuestionResponseInformation = testQuestionResponseInformation;

        var result = testDataShareRequestQuestionStatusInformationModelData.QuestionResponseInformation;

        Assert.That(result, Is.EqualTo(testQuestionResponseInformation));
    }

    [Test]
    public void GivenADataShareRequestQuestionStatusInformationModelData_WhenISetAnEmptySetOfQuestionPreRequisites_ThenQuestionPreRequisitesIsSet()
    {
        var testDataShareRequestQuestionStatusInformationModelData = new DataShareRequestQuestionStatusInformationModelData();

        var testQuestionPreRequisites = new List<QuestionPreRequisiteDataModel>();

        testDataShareRequestQuestionStatusInformationModelData.QuestionPreRequisites = testQuestionPreRequisites;

        var result = testDataShareRequestQuestionStatusInformationModelData.QuestionPreRequisites;

        Assert.That(result, Is.EqualTo(testQuestionPreRequisites));
    }

    [Test]
    public void GivenADataShareRequestQuestionStatusInformationModelData_WhenISetQuestionPreRequisites_ThenQuestionPreRequisitesIsSet()
    {
        var testDataShareRequestQuestionStatusInformationModelData = new DataShareRequestQuestionStatusInformationModelData();

        var testQuestionPreRequisites = new List<QuestionPreRequisiteDataModel> {new(), new(), new()};

        testDataShareRequestQuestionStatusInformationModelData.QuestionPreRequisites = testQuestionPreRequisites;

        var result = testDataShareRequestQuestionStatusInformationModelData.QuestionPreRequisites;

        Assert.That(result, Is.EqualTo(testQuestionPreRequisites));
    }

    [Test]
    public void GivenADataShareRequestQuestionStatusInformationModelData_WhenISetAnEmptySetOfSelectionOptionQuestionSetQuestionApplicabilityOverrides_ThenSelectionOptionQuestionSetQuestionApplicabilityOverridesIsSet()
    {
        var testDataShareRequestQuestionStatusInformationModelData = new DataShareRequestQuestionStatusInformationModelData();

        var testSelectionOptionQuestionSetQuestionApplicabilityOverrides = new List<QuestionSetQuestionApplicabilityOverride>();

        testDataShareRequestQuestionStatusInformationModelData.SelectionOptionQuestionSetQuestionApplicabilityOverrides = testSelectionOptionQuestionSetQuestionApplicabilityOverrides;

        var result = testDataShareRequestQuestionStatusInformationModelData.SelectionOptionQuestionSetQuestionApplicabilityOverrides;

        Assert.That(result, Is.EqualTo(testSelectionOptionQuestionSetQuestionApplicabilityOverrides));
    }

    [Test]
    public void GivenADataShareRequestQuestionStatusInformationModelData_WhenISetSelectionOptionQuestionSetQuestionApplicabilityOverrides_ThenSelectionOptionQuestionSetQuestionApplicabilityOverridesIsSet()
    {
        var testDataShareRequestQuestionStatusInformationModelData = new DataShareRequestQuestionStatusInformationModelData();

        var testSelectionOptionQuestionSetQuestionApplicabilityOverrides = new List<QuestionSetQuestionApplicabilityOverride> {new(), new(), new()};

        testDataShareRequestQuestionStatusInformationModelData.SelectionOptionQuestionSetQuestionApplicabilityOverrides = testSelectionOptionQuestionSetQuestionApplicabilityOverrides;

        var result = testDataShareRequestQuestionStatusInformationModelData.SelectionOptionQuestionSetQuestionApplicabilityOverrides;

        Assert.That(result, Is.EqualTo(testSelectionOptionQuestionSetQuestionApplicabilityOverrides));
    }
}