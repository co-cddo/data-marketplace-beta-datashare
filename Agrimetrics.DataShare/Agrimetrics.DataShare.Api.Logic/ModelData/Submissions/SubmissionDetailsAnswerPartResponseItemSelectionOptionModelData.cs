namespace Agrimetrics.DataShare.Api.Logic.ModelData.Submissions;

public class SubmissionDetailsAnswerPartResponseItemSelectionOptionModelData
{
    public Guid SubmissionDetailsAnswerPartResponseItemSelectionOption_Id { get; set; }

    public List<SubmissionDetailsAnswerPartResponseItemSelectionOptionSelectedOptionModelData> SubmissionDetailsAnswerPartResponseItemSelectionOption_SelectedOptions { get; set; } = [];
}