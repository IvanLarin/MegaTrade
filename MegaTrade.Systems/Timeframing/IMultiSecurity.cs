using TSLab.Script;

namespace MegaTrade.Systems.Timeframing;

public interface IMultiSecurity : ISecurity
{
    ISecurity[] All { get; }
}