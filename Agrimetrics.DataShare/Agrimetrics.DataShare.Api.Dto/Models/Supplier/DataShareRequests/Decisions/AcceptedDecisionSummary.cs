﻿using Agrimetrics.DataShare.Api.Dto.Models.DataShareRequests;

namespace Agrimetrics.DataShare.Api.Dto.Models.Supplier.DataShareRequests.Decisions;

public class AcceptedDecisionSummary
{
    public Guid DataShareRequestId { get; set; }

    public string DataShareRequestRequestId { get; set; } = string.Empty;

    public DataShareRequestStatus RequestStatus { get; set; }

    public string AcquirerUserEmailAddress { get; set; } = string.Empty;

    public string AcquirerOrganisationName { get; set; } = string.Empty;
}