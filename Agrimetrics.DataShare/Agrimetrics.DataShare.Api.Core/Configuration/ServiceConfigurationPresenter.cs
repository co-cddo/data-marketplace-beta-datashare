using Microsoft.Extensions.Configuration;

namespace Agrimetrics.DataShare.Api.Core.Configuration;

internal class ServiceConfigurationPresenter(
    IConfiguration configuration): IServiceConfigurationPresenter
{
    string IServiceConfigurationPresenter.GetValue(string valueKey)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(valueKey);

        return configuration[valueKey] ?? throw new InvalidOperationException($"Configuration key not found: Key='{valueKey}'");
    }

    string IServiceConfigurationPresenter.GetValueInSection(string sectionName, string valueKey)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(sectionName);
        ArgumentException.ThrowIfNullOrWhiteSpace(valueKey);

        var section = DoFindSection(sectionName);

        return section[valueKey] ?? throw new InvalidOperationException($"Configuration key not found in section: Key='{valueKey}', Section='{section.Path}'");
    }

    string IServiceConfigurationPresenter.GetValueInMultiLevelSection(IEnumerable<string> sectionNames, string valueKey)
    {
        var sectionNamesList = sectionNames.ToList();

        if (!sectionNamesList.Any()) throw new ArgumentException("Given value is empty", nameof(sectionNames));

        ArgumentException.ThrowIfNullOrWhiteSpace(valueKey);

        var section = DoFindMultiLevelSection(sectionNamesList);

        return section[valueKey] ?? throw new InvalidOperationException($"Configuration key not found in section: Key='{valueKey}', Section='{section.Path}'");
    }

    IEnumerable<string> IServiceConfigurationPresenter.GetValuesInSection(string sectionName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(sectionName);

        var section = DoFindSection(sectionName);

        return section.Get<List<string>>()!;
    }

    IEnumerable<string> IServiceConfigurationPresenter.GetValuesInMultiLevelSection(IEnumerable<string> sectionNames)
    {
        var sectionNamesList = sectionNames.ToList();

        if (!sectionNamesList.Any()) throw new ArgumentException("Given value is empty", nameof(sectionNames));

        var section = DoFindMultiLevelSection(sectionNamesList);
        
        return section.Get<List<string>>()!;
    }

    #region Section Fetching
    private IConfigurationSection DoFindSection(string sectionName)
    {
        return configuration.GetSection(sectionName) ?? throw new InvalidOperationException($"Configuration section not found: Section='{sectionName}'");
    }

    private IConfigurationSection DoFindMultiLevelSection(IEnumerable<string> sectionNames)
    {
        var sectionNamesList = sectionNames.ToList();

        return FindSection();

        IConfigurationSection FindSection()
        {
            return sectionNamesList.Aggregate((IConfigurationSection?)null, FindNextSection)!;

            IConfigurationSection FindNextSection(IConfigurationSection? currentConfigurationSection, string sectionName)
            {
                if (currentConfigurationSection == null)
                {
                    return configuration.GetSection(sectionName)
                           ?? throw new InvalidOperationException($"Configuration section not found at root of configuration: Section='{sectionName}'");
                }

                return currentConfigurationSection.GetSection(sectionName)
                       ?? throw new InvalidOperationException($"Configuration section not found within parent configuration section: Section='{sectionName}', Parent Section='{currentConfigurationSection.Path}'");
            }
        }
    }
    #endregion
}