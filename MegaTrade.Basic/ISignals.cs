namespace MegaTrade.Basic;

public interface ISignals : ITradeSignals
{
    double LongEnterVolume { get; }

    double LongExitVolume { get; }

    double ShortEnterVolume { get; }

    double ShortExitVolume { get; }

    double? LongTakeProfit { get; }

    double? LongStopLoss { get; }

    double? ShortTakeProfit { get; }

    double? ShortStopLoss { get; }
}