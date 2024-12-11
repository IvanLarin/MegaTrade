using TSLab.DataSource;
using TSLab.Script;

namespace MegaTrade.Basic.Trading.Position;

internal class NullMarketPosition : IMarketPosition
{
    public required ISecurity BasicTimeframe { get; init; }

    public string EntrySignalName => "";

    public string EntryNotes => "";

    public string ExitSignalName => "";

    public string ExitNotes => "";

    public double EntryPrice => 0;

    public double EntryCommission => 0;

    public double AverageEntryPrice => 0;

    public double ExitPrice => 0;

    public double ExitCommission => 0;

    public double Shares => 0;

    public double SignedShares => 0;

    public double SharesChange => 0;

    public int EntryBarNum => 0;

    public int ExitBarNum => 0;

    public IDataBar EntryBar => BasicTimeframe.Bars[0];

    public IDataBar ExitBar => BasicTimeframe.Bars[0];

    public double GetAccumulatedProfit(int bar) => 0;

    public double GetMaxAccumulatedProfit(out int bar)
    {
        bar = 0;
        return 0;
    }

    public double Profit() => 0;

    public double ProfitPct() => 0;

    public double CurrentProfit(int bar) => 0;

    public double CurrentProfitByOpenPrice(int bar) => 0;

    public double CurrentProfitMin(int bar) => 0;

    public double CurrentProfitPct(int bar) => 0;

    public double OpenProfit(int bar) => 0;

    public double OpenProfitPct(int bar) => 0;

    public double OpenMFEPct(int bar) => 0;

    public double OpenMAEPct(int bar) => 0;

    public double MFEPct() => 0;

    public double MAEPct() => 0;

    public double OpenMFE(int bar) => 0;

    public double OpenMAE(int bar) => 0;

    public double MFE() => 0;

    public double MAE() => 0;

    public double ReducedMAE() => 0;

    public DateTime MAEDate() => new(0);

    public int MAEBar() => 0;

    public int FindHighBar(int bar) => 0;

    public int FindLowBar(int bar) => 0;

    public bool IsActiveForBar(int bar) => false;

    public double GetBalancePrice(int bar) => 0;

    public double GetShares(int bar) => 0;

    public double GetStop(int bar) => 0;

    public IEnumerable<double> GetStops(int firstIndex, int lastIndex) => [];

    public double GetTakeProfit(int bar) => 0;

    public IEnumerable<double> GetTakeProfits(int firstIndex, int lastIndex) => [];

    public void ChangeAtMarket(int barNum, double newShares, string signalName, string? notes = null)
    {
    }

    public void ChangeAtPrice(int barNum, double price, double newShares, string signalName, string? notes = null)
    {
    }

    public void ChangeAtProfit(int barNum, double price, double newShares, string signalName, string? notes = null)
    {
    }

    public void ChangeAtProfit(int barNum, double price, double? slippage, double newShares, string signalName,
        string? notes = null)
    {
    }

    public void ChangeAtStop(int barNum, double price, double newShares, string signalName, string? notes = null)
    {
    }

    public void ChangeAtStop(int barNum, double price, double? slippage, double newShares, string signalName,
        string? notes = null)
    {
    }

    public void VirtualChange(int barNum, double price, double newShares, string signalName, string? notes = null)
    {
    }

    public void CloseAtMarket(int barNum, string signalName, string? notes = null)
    {
    }

    public void CloseAtStop(int barNum, double price, string signalName, string? notes = null)
    {
    }

    public void CloseAtStop(int barNum, double price, double? slippage, string signalName, string? notes = null)
    {
    }

    public void CloseAtProfit(int barNum, double price, string signalName, string? notes = null)
    {
    }

    public void CloseAtProfit(int barNum, double price, double? slippage, string signalName, string? notes = null)
    {
    }

    public void CloseAtPrice(int barNum, double price, string signalName, string? notes = null)
    {
    }

    public void CloseAtMarket(int barNum, string signalName, string notes, PositionExecution execution)
    {
    }

    public void CloseAtStop(int barNum, double price, string signalName, string notes, PositionExecution execution)
    {
    }

    public void CloseAtStop(int barNum, double price, double? slippage, string signalName, string notes,
        PositionExecution execution)
    {
    }

    public void CloseAtProfit(int barNum, double price, string signalName, string notes, PositionExecution execution)
    {
    }

    public void CloseAtProfit(int barNum, double price, double? slippage, string signalName, string notes,
        PositionExecution execution)
    {
    }

    public void CloseAtPrice(int barNum, double price, string signalName, string notes, PositionExecution execution)
    {
    }

    private IPositionsList? _positionsList;

    public IPositionsList ParentList => _positionsList ??= new NullPositionsList
    {
        BasicTimeframe = BasicTimeframe
    };

    public ISecurity Security => BasicTimeframe;

    public IReadOnlyList<IPositionInfo> ChangeInfos => [];

    public bool IsVirtual => false;

    public bool IsVirtualClosed => false;

    public PositionState PositionState => PositionState.NoError;

    public double FullEntryCommission => 0;

    public double FullExitCommission => 0;

    public int BarsHeld => 0;

    public double? ProfitPerTrade => null;

    public bool IsLong => false;

    public bool IsShort => false;

    public bool IsActive => false;

    public double SharesOrigin => 0;

    public double MaxShares => 0;

    public PositionExecution EntryExecution => PositionExecution.Normal;

    public PositionExecution? ExitExecution => PositionExecution.Normal;

    public bool IsOpen => false;
}