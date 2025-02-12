using Agrimetrics.DataShare.Api.Dto.Models.QuestionConfiguration.CompulsoryQuestions;
using Agrimetrics.DataShare.Api.Logic.ModelData.QuestionConfiguration.CompulsoryQuestions;

namespace Agrimetrics.DataShare.Api.Logic.ModelData.QuestionConfiguration;

internal class QuestionConfigurationModelDataFactory : IQuestionConfigurationModelDataFactory
{
    CompulsoryQuestionSet IQuestionConfigurationModelDataFactory.CreateCompulsoryQuestionSet(
        IEnumerable<CompulsoryQuestionModelData> compulsoryQuestionModelDatas)
    {
        ArgumentNullException.ThrowIfNull(compulsoryQuestionModelDatas);

        return new CompulsoryQuestionSet
        {
            CompulsoryQuestions = compulsoryQuestionModelDatas.Select(ConvertCompulsoryQuestionModelData).ToList()
        };

        CompulsoryQuestion ConvertCompulsoryQuestionModelData(
            CompulsoryQuestionModelData compulsoryQuestionModelData)
        {
            ArgumentNullException.ThrowIfNull(compulsoryQuestionModelData);

            return new CompulsoryQuestion
            {
                QuestionId = compulsoryQuestionModelData.CompulsoryQuestion_QuestionId
            };
        }
    }

    CompulsorySupplierMandatedQuestionSet IQuestionConfigurationModelDataFactory.CreateCompulsorySupplierMandatedQuestionSet(
        IEnumerable<CompulsorySupplierMandatedQuestionModelData> compulsorySupplierMandatedQuestionModelDatas)
    {
        ArgumentNullException.ThrowIfNull(compulsorySupplierMandatedQuestionModelDatas);

        return new CompulsorySupplierMandatedQuestionSet
        {
            CompulsorySupplierMandatedQuestions = compulsorySupplierMandatedQuestionModelDatas.Select(ConvertCompulsorySupplierMandatedQuestionModelData).ToList()
        };

        CompulsorySupplierMandatedQuestion ConvertCompulsorySupplierMandatedQuestionModelData(
            CompulsorySupplierMandatedQuestionModelData compulsorySupplierMandatedQuestionModelData)
        {
            ArgumentNullException.ThrowIfNull(compulsorySupplierMandatedQuestionModelData);

            return new CompulsorySupplierMandatedQuestion
            {
                SupplierOrganisationId = compulsorySupplierMandatedQuestionModelData.CompulsorySupplierMandatedQuestion_SupplierOrganisationId,
                QuestionId = compulsorySupplierMandatedQuestionModelData.CompulsorySupplierMandatedQuestion_QuestionId
            };
        }
    }
}