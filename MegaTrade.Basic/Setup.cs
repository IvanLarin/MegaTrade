using TSLab.Script;

namespace MegaTrade.Basic;

public class Setup
{
    public required ISecurity BasicTimeframe { get; init; }

    public int[] MinBarNumberLimits { get; init; } = [];
}