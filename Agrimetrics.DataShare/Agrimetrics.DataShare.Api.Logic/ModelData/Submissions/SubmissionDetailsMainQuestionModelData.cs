namespace Agrimetrics.DataShare.Api.Logic.ModelData.Submissions;

public class SubmissionDetailsMainQuestionModelData
{
    public Guid SubmissionDetailsMainQuestion_Id { get; set; }

    public int SubmissionDetailsMainQuestion_OrderWithinSection { get; set; }

    public string SubmissionDetailsMainQuestion_QuestionHeader { get; set; } = string.Empty;

    public List<SubmissionDetailsAnswerPartModelData> SubmissionDetailsMainQuestion_AnswerParts { get; set; } = [];

    public List<SubmissionDetailsBackingQuestionModelData> SubmissionDetailsMainQuestion_BackingQuestions { get; set; } = [];
}