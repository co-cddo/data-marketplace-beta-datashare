using Agrimetrics.DataShare.Api.Logic.ModelData.Questions.QuestionParts.ResponseFormats;

namespace Agrimetrics.DataShare.Api.Logic.ModelData.Questions.QuestionParts;

public class QuestionPartResponseTypeInformationModelData
{
    public Guid QuestionPartResponseTypeInformation_QuestionPartId { get; set; }

    public QuestionPartResponseInputType QuestionPartResponseTypeInformation_InputType { get; set; }

    public QuestionPartResponseFormatType QuestionPartResponseTypeInformation_FormatType { get; set; }
}