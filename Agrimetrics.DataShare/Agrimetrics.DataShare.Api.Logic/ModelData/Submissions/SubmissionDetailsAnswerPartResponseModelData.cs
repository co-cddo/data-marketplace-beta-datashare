namespace Agrimetrics.DataShare.Api.Logic.ModelData.Submissions;

public class SubmissionDetailsAnswerPartResponseModelData
{
    public Guid SubmissionDetailsAnswerPartResponse_Id { get; set; }

    public int SubmissionDetailsAnswerPartResponse_OrderWithinAnswerPart { get; set; }

    // Note: There should only ever be exactly one response item per response.  However, in early development there was an issue whereby
    // selection option responses were being reported incorrectly, and if N options were selected then N responses were received, with
    // each containing the full N selected options.
    // Therefore, this remains to cater for some of the bad data that may remain from those early stages.  But if you're reading this
    // and the system has been deployed and working with real data for a while then you can most likely make this a single response item
    public List<SubmissionDetailsAnswerPartResponseItemModelData> SubmissionDetailsAnswerPartResponse_ResponseItems { get; set; } = [];
}