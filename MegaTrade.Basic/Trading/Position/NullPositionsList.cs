using System.Collections;
using TSLab.Script;

namespace MegaTrade.Basic.Trading.Position;

internal class NullPositionsList : IPositionsList
{
    public required ISecurity BasicTimeframe { get; init; }

    public IEnumerator<IPosition> GetEnumerator() => Enumerable.Empty<IPosition>().GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    private IPosition? _nullPosition;

    private IPosition NullPosition => _nullPosition ??= new NullMarketPosition
    {
        BasicTimeframe = BasicTimeframe
    };

    public IPosition GetLastPosition(int barNum) => NullPosition;

    public IPosition GetLastPositionActive(int barNum) => NullPosition;

    public IPosition GetLastLongPositionActive(int barNum) => NullPosition;

    public IPosition GetLastShortPositionActive(int barNum) => NullPosition;

    public IPosition GetLastPositionClosed(int barNum) => NullPosition;

    public IPosition GetLastLongPositionClosed(int barNum) => NullPosition;

    public IPosition GetLastShortPositionClosed(int barNum) => NullPosition;

    public IPosition GetLastForSignal(string signalName, int barNum) => NullPosition;

    public IPosition GetLastActiveForSignal(string signalName) => NullPosition;

    public IPosition GetLastClosedForSignal(string signalName, int barNum) => NullPosition;

    public IPosition GetLastActiveForSignal(string signalName, int barNum) => NullPosition;

    public IPosition GetLastForCloseSignal(string signalName, int barNum) => NullPosition;

    public IEnumerable<IPosition> GetActiveForBar(int barNum) => [];

    public IEnumerable<IPosition> GetClosedForBar(int barNum) => [];

    public IEnumerable<IPosition> GetClosedOrActiveForBar(int barNum) => [];

    public void BuyAtMarket(int barNum, double shares, string signalName, string? notes = null)
    {
    }

    public void BuyAtPrice(int barNum, double shares, double price, string signalName, string? notes = null)
    {
    }

    public void BuyIfLess(int barNum, double shares, double price, string signalName, string? notes = null)
    {
    }

    public void BuyIfLess(int barNum, double shares, double price, double? slippage, string signalName,
        string? notes = null)
    {
    }

    public void BuyIfGreater(int barNum, double shares, double price, string signalName, string? notes = null)
    {
    }

    public void BuyIfGreater(int barNum, double shares, double price, double? slippage, string signalName,
        string? notes = null)
    {
    }

    public void BuyAtMarket(int barNum, double shares, string signalName, string notes, PositionExecution execution)
    {
    }

    public void BuyAtPrice(int barNum, double shares, double price, string signalName, string notes,
        PositionExecution execution)
    {
    }

    public void BuyIfLess(int barNum, double shares, double price, string signalName, string notes,
        PositionExecution execution)
    {
    }

    public void BuyIfLess(int barNum, double shares, double price, double? slippage, string signalName, string notes,
        PositionExecution execution)
    {
    }

    public void BuyIfGreater(int barNum, double shares, double price, string signalName, string notes,
        PositionExecution execution)
    {
    }

    public void BuyIfGreater(int barNum, double shares, double price, double? slippage, string signalName, string notes,
        PositionExecution execution)
    {
    }

    public void SellAtMarket(int barNum, double shares, string signalName, string? notes = null)
    {
    }

    public void SellAtPrice(int barNum, double shares, double price, string signalName, string? notes = null)
    {
    }

    public void SellIfGreater(int barNum, double shares, double price, string signalName, string? notes = null)
    {
    }

    public void SellIfGreater(int barNum, double shares, double price, double? slippage, string signalName,
        string? notes = null)
    {
    }

    public void SellIfLess(int barNum, double shares, double price, string signalName, string? notes = null)
    {
    }

    public void SellIfLess(int barNum, double shares, double price, double? slippage, string signalName,
        string? notes = null)
    {
    }

    public void SellAtMarket(int barNum, double shares, string signalName, string notes, PositionExecution execution)
    {
    }

    public void SellAtPrice(int barNum, double shares, double price, string signalName, string notes,
        PositionExecution execution)
    {
    }

    public void SellIfGreater(int barNum, double shares, double price, string signalName, string notes,
        PositionExecution execution)
    {
    }

    public void SellIfGreater(int barNum, double shares, double price, double? slippage, string signalName,
        string notes,
        PositionExecution execution)
    {
    }

    public void SellIfLess(int barNum, double shares, double price, string signalName, string notes,
        PositionExecution execution)
    {
    }

    public void SellIfLess(int barNum, double shares, double price, double? slippage, string signalName, string notes,
        PositionExecution execution)
    {
    }

    public void OpenAtMarket(bool isLong, int barNum, double shares, string signalName, string? notes = null)
    {
    }

    public void OpenAtPrice(bool isLong, int barNum, double shares, double price, string signalName,
        string? notes = null)
    {
    }

    public void OpenIfLess(bool isLong, int barNum, double shares, double price, string signalName,
        string? notes = null)
    {
    }

    public void OpenIfLess(bool isLong, int barNum, double shares, double price, double? slippage, string signalName,
        string? notes = null)
    {
    }

    public void OpenIfGreater(bool isLong, int barNum, double shares, double price, string signalName,
        string? notes = null)
    {
    }

    public void OpenIfGreater(bool isLong, int barNum, double shares, double price, double? slippage, string signalName,
        string? notes = null)
    {
    }

    public void OpenAtMarket(bool isLong, int barNum, double shares, string signalName, string notes,
        PositionExecution execution)
    {
    }

    public void OpenAtPrice(bool isLong, int barNum, double shares, double price, string signalName, string notes,
        PositionExecution execution)
    {
    }

    public void OpenIfLess(bool isLong, int barNum, double shares, double price, string signalName, string notes,
        PositionExecution execution)
    {
    }

    public void OpenIfLess(bool isLong, int barNum, double shares, double price, double? slippage, string signalName,
        string notes,
        PositionExecution execution)
    {
    }

    public void OpenIfGreater(bool isLong, int barNum, double shares, double price, string signalName, string notes,
        PositionExecution execution)
    {
    }

    public void OpenIfGreater(bool isLong, int barNum, double shares, double price, double? slippage, string signalName,
        string notes, PositionExecution execution)
    {
    }

    public IPosition MakeVirtualPosition(int barNum, double shares, double price, string signalName,
        string? notes = null) => NullPosition;

    public bool IsRealtime => BasicTimeframe.IsRealtime;

    public ISecurity Security => BasicTimeframe;

    public int BarsCount => BasicTimeframe.Bars.Count;

    public bool HavePositions => false;

    public int ActivePositionCount => 0;

    public int PositionCount => 0;
}