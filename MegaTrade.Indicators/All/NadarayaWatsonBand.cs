using MegaTrade.Common.Calculating;

namespace MegaTrade.Indicators.All;

internal class NadarayaWatsonBand : Calculator<IList<double>>
{
    protected override IList<double> Calculate() =>
        Input.nwe.Zip(Input.sae, (average, error) =>
            average + error * Multiplier * (int)Band).ToArray();

    public required (double[] nwe, double[] sae) Input { private get; init; }

    public required Band Band { private get; init; }

    public required double Multiplier { private get; init; }
}