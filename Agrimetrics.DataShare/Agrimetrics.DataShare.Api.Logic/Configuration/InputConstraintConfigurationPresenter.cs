using Agrimetrics.DataShare.Api.Core.Configuration;
using Agrimetrics.DataShare.Api.Core.Configuration.Model;

namespace Agrimetrics.DataShare.Api.Logic.Configuration;

internal class InputConstraintConfigurationPresenter(
    IServiceConfigurationPresenter serviceConfigurationPresenter) : IInputConstraintConfigurationPresenter
{
    private const string inputConstraintsSectionName = "InputConstraints";

    private const int defaultMaximumLengthOfFreeFormTextResponse = 4000;
    private const int defaultMaximumLengthOfFreeFormMultiResponseTextResponse = 250;
    private const int defaultMaximumLengthOfSupplementaryTextResponse = 250;

    int IInputConstraintConfigurationPresenter.GetMaximumLengthOfFreeFormTextResponse()
    {
        try
        {
            var maximumLengthOfFreeFormTextResponseString = DoGetMaximumLengthOfFreeFormTextResponse();

            return int.TryParse(maximumLengthOfFreeFormTextResponseString, out var maximumLengthOfFreeFormTextResponse)
                ? maximumLengthOfFreeFormTextResponse
                : defaultMaximumLengthOfFreeFormTextResponse;
        }
        catch
        {
            return defaultMaximumLengthOfFreeFormTextResponse;
        }
    }

    int IInputConstraintConfigurationPresenter.GetMaximumLengthOfFreeFormMultiResponseTextResponse()
    {
        try
        {
            var maximumLengthOfFreeFormMultiResponseTextResponseString = DoGetMaximumLengthOfFreeFormMultiResponseTextResponse();

            return int.TryParse(maximumLengthOfFreeFormMultiResponseTextResponseString, out var maximumLengthOfFreeFormMultiResponseTextResponse)
                ? maximumLengthOfFreeFormMultiResponseTextResponse
                : defaultMaximumLengthOfFreeFormMultiResponseTextResponse;
        }
        catch
        {
            return defaultMaximumLengthOfFreeFormMultiResponseTextResponse;
        }
    }

    int IInputConstraintConfigurationPresenter.GetMaximumLengthOfSupplementaryTextResponse()
    {
        try
        {
            var maximumLengthOfSupplementaryTextResponseString = DoGetMaximumLengthOfSupplementaryTextResponse();

            return int.TryParse(maximumLengthOfSupplementaryTextResponseString, out var maximumLengthOfSupplementaryTextResponse)
                ? maximumLengthOfSupplementaryTextResponse
                : defaultMaximumLengthOfSupplementaryTextResponse;
        }
        catch
        {
            return defaultMaximumLengthOfSupplementaryTextResponse;
        }
    }

    IEnumerable<SettingValue> IInputConstraintConfigurationPresenter.GetAllSettingValues()
    {
        return new List<SettingValue>
        {
            DoGetSettingValue("Maximum Length Of FreeForm Text Response", DoGetMaximumLengthOfFreeFormTextResponse),
            DoGetSettingValue("Maximum Length Of FreeForm Multi-Response Text Response", DoGetMaximumLengthOfFreeFormMultiResponseTextResponse),
            DoGetSettingValue("Maximum Length Of Supplementary Text Response", DoGetMaximumLengthOfSupplementaryTextResponse),
        };

        static SettingValue DoGetSettingValue(string description, Func<string> getSettingValueFunc)
        {
            return new SettingValue
            {
                Description = description,
                Value = GetSettingValue()
            };

            string GetSettingValue()
            {
                try
                {
                    return getSettingValueFunc();
                }
                catch (Exception ex)
                {
                    return $"ERROR: {ex.Message}";
                }
            }
        }
    }

    private string DoGetMaximumLengthOfFreeFormTextResponse()
    {
        return serviceConfigurationPresenter.GetValueInSection(
            inputConstraintsSectionName, "MaximumLengthOfFreeFormTextResponse");
    }

    private string DoGetMaximumLengthOfFreeFormMultiResponseTextResponse()
    {
        return serviceConfigurationPresenter.GetValueInSection(
            inputConstraintsSectionName, "MaximumLengthOfFreeFormMultiResponseTextResponse");
    }

    private string DoGetMaximumLengthOfSupplementaryTextResponse()
    {
        return serviceConfigurationPresenter.GetValueInSection(
            inputConstraintsSectionName, "MaximumLengthOfSupplementaryTextResponse");
    }
}