using Agrimetrics.DataShare.Api.Dto.Models.QuestionConfiguration.CompulsoryQuestions;
using Agrimetrics.DataShare.Api.Logic.ModelData.QuestionConfiguration.CompulsoryQuestions;

namespace Agrimetrics.DataShare.Api.Logic.ModelData.QuestionConfiguration;

public interface IQuestionConfigurationModelDataFactory
{
    #region Model Data To Dto Data
    CompulsoryQuestionSet CreateCompulsoryQuestionSet(
        IEnumerable<CompulsoryQuestionModelData> compulsoryQuestionModelDatas);

    CompulsorySupplierMandatedQuestionSet CreateCompulsorySupplierMandatedQuestionSet(
        IEnumerable<CompulsorySupplierMandatedQuestionModelData> compulsorySupplierMandatedQuestionModelDatas);
    #endregion
}