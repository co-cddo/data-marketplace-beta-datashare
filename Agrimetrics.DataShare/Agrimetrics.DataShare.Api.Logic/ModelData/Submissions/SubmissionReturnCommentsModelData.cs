namespace Agrimetrics.DataShare.Api.Logic.ModelData.Submissions;

public class SubmissionReturnCommentsModelData
{
    public DateTime ReturnedOnUtc { get; set; }

    public string Comments { get; set; } = string.Empty;
}