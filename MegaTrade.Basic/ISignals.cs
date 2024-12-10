using TSLab.Script;

namespace MegaTrade.Basic;

public interface ISignals
{
    bool IsLongEnterSignal { get; }

    bool IsLongExitSignal { get; }

    bool IsShortEnterSignal { get; }

    bool IsShortExitSignal { get; }

    double LongEnterVolume { get; }

    double LongExitVolume { get; }

    double ShortEnterVolume { get; }

    double ShortExitVolume { get; }

    double? GetLongTake(IPositionInfo position);

    double? GetLongStop(IPositionInfo position);

    double? GetShortTake(IPositionInfo position);

    double? GetShortStop(IPositionInfo position);
}