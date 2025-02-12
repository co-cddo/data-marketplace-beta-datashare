using Agrimetrics.DataShare.Api.Logic.ModelData.Submissions;
using NUnit.Framework;

namespace Agrimetrics.DataShare.Api.Logic.Test.ModelData.Submissions;

[TestFixture]
public class SubmissionDetailsBackingQuestionModelDataTests
{
    [Test]
    public void GivenASubmissionDetailsBackingQuestionModelData_WhenISetId_ThenIdIsSet()
    {
        var testSubmissionDetailsBackingQuestionModelData = new SubmissionDetailsBackingQuestionModelData();

        var testId = new Guid("1762BB7C-5D7E-4F6F-8702-F7D2FAE5AB47");

        testSubmissionDetailsBackingQuestionModelData.SubmissionDetailsBackingQuestion_Id = testId;

        var result = testSubmissionDetailsBackingQuestionModelData.SubmissionDetailsBackingQuestion_Id;

        Assert.That(result, Is.EqualTo(testId));
    }

    [Test]
    public void GivenASubmissionDetailsBackingQuestionModelData_WhenISetAnEmptySetOfAnswerParts_ThenAnswerPartsIsSet()
    {
        var testSubmissionDetailsBackingQuestionModelData = new SubmissionDetailsBackingQuestionModelData();

        var testAnswerParts = new List<SubmissionDetailsAnswerPartModelData>();

        testSubmissionDetailsBackingQuestionModelData.SubmissionDetailsBackingQuestion_AnswerParts = testAnswerParts;

        var result = testSubmissionDetailsBackingQuestionModelData.SubmissionDetailsBackingQuestion_AnswerParts;

        Assert.That(result, Is.EqualTo(testAnswerParts));
    }

    [Test]
    public void GivenASubmissionDetailsBackingQuestionModelData_WhenISetAnswerParts_ThenAnswerPartsIsSet()
    {
        var testSubmissionDetailsBackingQuestionModelData = new SubmissionDetailsBackingQuestionModelData();

        var testAnswerParts = new List<SubmissionDetailsAnswerPartModelData> {new(), new(), new()};

        testSubmissionDetailsBackingQuestionModelData.SubmissionDetailsBackingQuestion_AnswerParts = testAnswerParts;

        var result = testSubmissionDetailsBackingQuestionModelData.SubmissionDetailsBackingQuestion_AnswerParts;

        Assert.That(result, Is.EqualTo(testAnswerParts));
    }
}