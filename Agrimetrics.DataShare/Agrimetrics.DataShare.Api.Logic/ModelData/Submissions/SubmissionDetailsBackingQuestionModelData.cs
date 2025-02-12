namespace Agrimetrics.DataShare.Api.Logic.ModelData.Submissions;

public class SubmissionDetailsBackingQuestionModelData
{
    public Guid SubmissionDetailsBackingQuestion_Id { get; set; }

    public List<SubmissionDetailsAnswerPartModelData> SubmissionDetailsBackingQuestion_AnswerParts { get; set; } = [];
}