namespace Agrimetrics.DataShare.Api.Core.Configuration;

public interface IServiceConfigurationPresenter
{
    string GetValue(string valueKey);

    string GetValueInSection(string sectionName, string valueKey);

    string GetValueInMultiLevelSection(IEnumerable<string> sectionNames, string valueKey);

    IEnumerable<string> GetValuesInSection(string sectionName);

    IEnumerable<string> GetValuesInMultiLevelSection(IEnumerable<string> sectionNames);
}