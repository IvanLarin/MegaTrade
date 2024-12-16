using MegaTrade.Common;
using MegaTrade.Common.Calculating;
using MegaTrade.Common.Extensions;

namespace MegaTrade.Indicators.All;

internal class NadarayaWatsonBand : Calculator<IList<double>>
{
    protected override IList<double> Calculate() =>
        Local.Context.GetOrCreateArray<double>(Source.nwe.Length)
            .FillFrom(Source.nwe.Zip(Source.sae, (average, error) =>
                average + error * Multiplier * (int)Band));

    public required Band Band { private get; init; }

    public required double Multiplier { private get; init; }

    public required (double[] nwe, double[] sae) Source { get; init; }
}