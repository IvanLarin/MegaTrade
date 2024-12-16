using TSLab.Script;

namespace MegaTrade.Timeframes;

internal interface IMultiSecurity : ISecurity
{
    ISecurity[] All { get; }
}