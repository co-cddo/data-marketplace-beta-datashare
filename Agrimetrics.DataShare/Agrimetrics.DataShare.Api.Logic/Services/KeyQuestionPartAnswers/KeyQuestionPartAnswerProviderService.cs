using Agrimetrics.DataShare.Api.Logic.ModelData.Questions.KeyQuestionParts;
using Agrimetrics.DataShare.Api.Logic.Repositories.KeyQuestionPartAnswers;
using Microsoft.Extensions.Logging;

namespace Agrimetrics.DataShare.Api.Logic.Services.KeyQuestionPartAnswers;

internal class KeyQuestionPartAnswerProviderService(
    ILogger<KeyQuestionPartAnswerProviderService> logger,
    IKeyQuestionPartAnswersRepository keyQuestionPartAnswersRepository) : IKeyQuestionPartAnswerProviderService
{
    async Task<DateTime?> IKeyQuestionPartAnswerProviderService.GetDateRequiredQuestionPartAnswerAsync(
        Guid dataShareRequestId)
    {
        var answers = (await DoGetKeyQuestionPartAnswerAsync(dataShareRequestId, QuestionPartKeyType.DateRequired)).ToList();

        if (answers.Count > 1)
        {
            logger.LogError("KeyQuestionPart 'Date Required' has multiple answers");
            return null;
        }

        var answer = answers.FirstOrDefault();
        if (answer == null) return null;

        if (answer.Length != 8)
        {
            logger.LogError("KeyQuestionPart 'Date Required' has unexpected value");
            return null;
        }

        var yearPart = answer[..4];
        var monthPart = answer.Substring(4, 2);
        var dayPart = answer.Substring(6, 2);

        var yearPartIsNumeric = int.TryParse(yearPart, out var yearPartNumeric);
        var monthPartIsNumeric = int.TryParse(monthPart, out var monthPartNumeric);
        var dayPartIsNumeric = int.TryParse(dayPart, out var dayPartNumeric);

        if (!yearPartIsNumeric || !monthPartIsNumeric || !dayPartIsNumeric)
        {
            logger.LogError("KeyQuestionPart 'Date Required' has badly formed value");
            return null;
        }

        try
        {
            return new DateTime(yearPartNumeric, monthPartNumeric, dayPartNumeric, 0, 0, 0, DateTimeKind.Utc);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "KeyQuestionPart 'Date Required' has an invalid date");
            return null;
        }
    }

    async Task<string?> IKeyQuestionPartAnswerProviderService.GetProjectAimsQuestionPartAnswerAsync(
        Guid dataShareRequestId)
    {
        var answers = (await DoGetKeyQuestionPartAnswerAsync(dataShareRequestId, QuestionPartKeyType.ProjectAims)).ToList();

        if (answers.Count > 1)
        {
            logger.LogError("KeyQuestionPart 'Project Aims' has multiple answers");
            return null;
        }

        return answers.FirstOrDefault();
    }

    async Task<IEnumerable<string>> IKeyQuestionPartAnswerProviderService.GetDataTypesQuestionPartAnswerAsync(
        Guid dataShareRequestId)
    {
        return await DoGetKeyQuestionPartAnswerAsync(dataShareRequestId, QuestionPartKeyType.DataTypes);
    }

    private async Task<IEnumerable<string>> DoGetKeyQuestionPartAnswerAsync(
        Guid dataShareRequestId,
        QuestionPartKeyType questionPartKey)
    {
        return await keyQuestionPartAnswersRepository.GetKeyQuestionPartResponsesAsync(
            dataShareRequestId, questionPartKey);
    }
}