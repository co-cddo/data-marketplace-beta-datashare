using System.Diagnostics.CodeAnalysis;

namespace Agrimetrics.DataShare.Api.Core.SystemProxies;

[ExcludeFromCodeCoverage] // Justification - Proxy class to third party entity that cannot be reliably unit tested
internal class Clock : IClock
{
    DateTime IClock.LocalNow => DateTime.Now;

    DateTime IClock.UtcNow => DateTime.UtcNow;
}