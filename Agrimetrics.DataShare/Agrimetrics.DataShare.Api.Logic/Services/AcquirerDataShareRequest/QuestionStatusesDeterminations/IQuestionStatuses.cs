﻿using Agrimetrics.DataShare.Api.Logic.ModelData.DataShareRequests.QuestionStatusDeterminations;

namespace Agrimetrics.DataShare.Api.Logic.Services.AcquirerDataShareRequest.DataShareRequestQuestionStatusesDeterminations;

public interface IDataShareRequestQuestionStatusesDetermination
{
    IDataShareRequestQuestionStatusesDeterminationResult DetermineQuestionStatuses(
        DataShareRequestQuestionStatusInformationSetModelData dataShareRequestQuestionStatusInformationSetModelData);
}