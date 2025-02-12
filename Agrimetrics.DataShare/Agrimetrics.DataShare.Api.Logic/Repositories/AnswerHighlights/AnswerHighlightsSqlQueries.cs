using System.Diagnostics.CodeAnalysis;

namespace Agrimetrics.DataShare.Api.Logic.Repositories.AnswerHighlights;

[SuppressMessage("ReSharper", "StringLiteralTypo", Justification = "Lots of queries have abbreviations for conciseness")]
[ExcludeFromCodeCoverage] // It makes no sense to write unit tests against SQL statements as they have actual function
internal class AnswerHighlightsSqlQueries : IAnswerHighlightsSqlQueries
{
    string IAnswerHighlightsSqlQueries.GetQuestionSetSelectionOptionQuestionHighlightModelDatas =>
        @"SELECT
	            [qssoqh].[Id] AS QuestionSetSelectionOptionQuestionHighlight_Id,
	            [qssoqh].[QuestionSet] AS QuestionSetSelectionOptionQuestionHighlight_QuestionSetId,
	            [qssoqh].[SelectionOption] AS QuestionSetSelectionOptionQuestionHighlight_SelectionOptionId,
	            [qssoqh].[HighlightCondition] AS QuestionSetSelectionOptionQuestionHighlight_HighlightCondition,
	            [qssoqh].[ReasonHighlighted] AS QuestionSetSelectionOptionQuestionHighlight_ReasonHighlighted
            FROM [dbo].[DataShareRequest] [dsr]
	            JOIN [dbo].[QuestionSet] [qs] ON [qs].[Id] = [dsr].[QuestionSet]
	            JOIN [dbo].[QuestionSetSelectionOptionQuestionHighlight] [qssoqh] ON [qssoqh].[QuestionSet] = [qs].[Id]
            WHERE [dsr].[Id] = @DataShareRequestId";

    string IAnswerHighlightsSqlQueries.GetDataShareRequestSelectedOptionsModelDatas =>
        @"SELECT
	            [dsr].[Id] AS DataShareRequestSelectionOptions_DataShareRequestId,

	            [qp].[Id] AS DataShareRequestSelectedOption_QuestionPartId,
	            [ap].[Id] AS DataShareRequestSelectedOption_AnswerPartId,
	            [apr].[Id] AS DataShareRequestSelectedOption_AnswerPartResponseId,
	            [apri].[Id] AS DataShareRequestSelectedOption_AnswerPartResponseItemId,
	            [apriso].[Id] AS DataShareRequestSelectedOption_AnswerPartResponseItemSelectionOptionId,
	            [apriso].[SelectionOption] AS DataShareRequestSelectedOption_OptionSelectionId

            FROM [dbo].[DataShareRequest] [dsr]
	            JOIN [dbo].[QuestionSet] [qs] ON [qs].[Id] = [dsr].[QuestionSet]
	            JOIN [dbo].[QuestionSetQuestion] [qsq] ON [qsq].[QuestionSet] = [qs].[Id]
	            JOIN [dbo].[Question] [q] ON [q].[Id] = [qsq].[Question]
	            JOIN [dbo].[QuestionPartSet] [qps] ON [qps].[Question] = [q].[Id] AND [qps].[QuestionPartType] = 'MainQuestionPart'
	            JOIN [dbo].[QuestionPart] [qp] ON [qp].[Id] = [qps].[QuestionPart]
	            JOIN [dbo].[ResponseFormatType] [rft] ON [rft].[Id] = [qp].[ResponseFormatType] AND [rft].[InputType] = 'OptionSelection'
	            
	            JOIN [dbo].[AnswerSet] [as] ON [as].[DataShareRequest] = [dsr].[Id]
	            JOIN [dbo].[Answer] [a] ON [a].[AnswerSet] = [as].[Id] AND [a].[Question] = [q].[Id]
	            JOIN [dbo].[AnswerPart] [ap] ON [ap].[Answer] = [a].[Id] AND [ap].[QuestionPart] = [qp].[Id]
	            JOIN [dbo].[AnswerPartResponse] [apr] ON [apr].[AnswerPart] = [ap].[Id]
	            JOIN [dbo].[AnswerPartResponseItem] [apri] ON [apri].[AnswerPartResponse] = [apr].[Id]
	            JOIN [dbo].[AnswerPartResponseItemSelectionOption] [apriso] ON [apriso].[AnswerPartResponseItem] = [apri].[Id]
            WHERE [dsr].[Id] = @DataShareRequestId";
}