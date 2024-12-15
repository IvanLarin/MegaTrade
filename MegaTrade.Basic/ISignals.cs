namespace MegaTrade.Basic;

public interface ISignals : ITradeSignals
{
    double LongEnterVolume { get; }

    double LongExitVolume { get; }

    double ShortEnterVolume { get; }

    double ShortExitVolume { get; }

    double? LongTake { get; }

    double? LongStop { get; }

    double? ShortTake { get; }

    double? ShortStop { get; }
}