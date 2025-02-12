using System.ComponentModel.DataAnnotations;
using Agrimetrics.DataShare.Api.Dto.Models.Supplier.DataShareRequests;

namespace Agrimetrics.DataShare.Api.Dto.Requests.Supplier;

public class GetSubmissionAsFileRequest
{
    [Required]
    public Guid DataShareRequestId { get; set; }

    [Required]
    public DataShareRequestFileFormat FileFormat { get; set; }
}