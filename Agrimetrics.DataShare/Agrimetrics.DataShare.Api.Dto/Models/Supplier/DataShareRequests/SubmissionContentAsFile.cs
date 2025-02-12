namespace Agrimetrics.DataShare.Api.Dto.Models.Supplier.DataShareRequests;

public class SubmissionContentAsFile
{
    public required byte[] Content { get; init; }

    public required string ContentType { get; init; }

    public required string FileName { get; init; }
}