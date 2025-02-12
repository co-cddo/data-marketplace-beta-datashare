using System.ComponentModel.DataAnnotations;
using Agrimetrics.DataShare.Api.Dto.Models.Reporting;

namespace Agrimetrics.DataShare.Api.Dto.Requests.Reporting;

public class QueryDataShareRequestCountsRequest
{
    [Required]
    public List<DataShareRequestCountQuery> DataShareRequestCountQueries { get; set; } = [];
}