namespace Agrimetrics.DataShare.Api.Logic.ModelData.Submissions;

public class SubmissionDetailsAnswerPartResponseItemFreeFormModelData
{
    public Guid SubmissionDetailsAnswerPartResponseItemFreeForm_Id { get; set; }

    public string SubmissionDetailsAnswerPartResponseItemFreeForm_AnswerValue { get; set; } = string.Empty;

    public bool SubmissionDetailsAnswerPartResponseItemFreeForm_ValueEntryDeclined { get; set; }
}