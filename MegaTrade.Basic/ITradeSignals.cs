namespace MegaTrade.Basic;

public interface ITradeSignals
{
    bool IsLongEnter { get; }

    bool IsLongExit { get; }

    bool IsShortEnter { get; }

    bool IsShortExit { get; }
}