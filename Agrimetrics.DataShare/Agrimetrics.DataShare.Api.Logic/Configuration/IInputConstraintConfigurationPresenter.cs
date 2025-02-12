using Agrimetrics.DataShare.Api.Core.Configuration.Model;

namespace Agrimetrics.DataShare.Api.Logic.Configuration;

public interface IInputConstraintConfigurationPresenter
{
    int GetMaximumLengthOfFreeFormTextResponse();

    int GetMaximumLengthOfFreeFormMultiResponseTextResponse();

    int GetMaximumLengthOfSupplementaryTextResponse();

    IEnumerable<SettingValue> GetAllSettingValues();
    
}