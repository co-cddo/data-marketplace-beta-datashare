namespace Agrimetrics.DataShare.Api.Logic.ModelData.Submissions;

public class SubmissionDetailsAnswerPartResponseItemModelData
{
    public Guid SubmissionDetailsAnswerResponseItem_Id { get; set; }

    public SubmissionDetailsAnswerPartResponseItemFreeFormModelData? SubmissionDetailsAnswerResponseItem_FreeFormData { get; set; }

    public SubmissionDetailsAnswerPartResponseItemSelectionOptionModelData? SubmissionDetailsAnswerResponseItem_SelectionOptionData { get; set; }
}