namespace Agrimetrics.DataShare.Api.Dto.Models.Supplier.DataShareRequests;

public class SubmissionDetailsSection
{
    public int SectionNumber { get; set; }

    public string SectionHeader { get; set; } = string.Empty;

    public List<SubmissionDetailsAnswerGroup> AnswerGroups { get; set; } = [];
}