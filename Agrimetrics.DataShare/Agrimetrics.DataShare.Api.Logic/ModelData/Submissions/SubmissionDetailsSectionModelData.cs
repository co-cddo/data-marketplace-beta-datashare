namespace Agrimetrics.DataShare.Api.Logic.ModelData.Submissions;

public class SubmissionDetailsSectionModelData
{
    public Guid SubmissionDetailsSection_SectionId { get; set; }

    public int SubmissionDetailsSection_SectionNumber { get; set; }

    public string SubmissionDetailsSection_SectionHeader { get; set; } = string.Empty;

    public List<SubmissionDetailsMainQuestionModelData> SubmissionDetailsSection_Questions { get; set; } = [];
}