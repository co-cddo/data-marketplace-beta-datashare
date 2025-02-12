using Agrimetrics.DataShare.Api.Dto.Models.Supplier.DataShareRequests;

namespace Agrimetrics.DataShare.Api.Logic.Services.SupplierDataShareRequest.SubmissionContentFileBuilding
{
    public interface ISubmissionContentPdfFileBuilder
    {
        Task<byte[]> BuildAsync(
            SubmissionInformation submissionInformation,
            SubmissionDetails submissionDetails);
    }
}
