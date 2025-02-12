namespace Agrimetrics.DataShare.Api.Dto.Models.Supplier.DataShareRequests;

public class SubmissionDetailsAnswerResponseItemSelectedOption
{
    public int OrderWithinSelectedOptions { get; set; }

    public string SelectionOptionText { get; set; } = string.Empty;

    public string? SupplementaryAnswerText { get; set; }
}