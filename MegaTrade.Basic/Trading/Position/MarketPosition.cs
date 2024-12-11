using TSLab.DataSource;
using TSLab.Script;

namespace MegaTrade.Basic.Trading.Position;

internal class MarketPosition(IPosition position) : IMarketPosition
{
    public required INowProvider NowProvider { get; init; }

    public required ISecurity BasicTimeframe { get; init; }

    private int Now => NowProvider.Now;

    public bool IsOpen => position.EntryBarNum <= Now && Now <= position.ExitBarNum;

    public int EntryBarNum => Math.Min(Now, position.EntryBarNum);

    public IDataBar EntryBar => BasicTimeframe.Bars[EntryBarNum];

    public double AverageEntryPrice => position.EntryBarNum > Now
        ? BasicTimeframe.Bars[Now].Close
        : position.AverageEntryPrice;

    public double Shares => IsOpen ? position.Shares : 0;

    public string EntrySignalName => position.EntrySignalName;

    public string EntryNotes => position.EntryNotes;

    public string ExitSignalName => position.ExitSignalName;

    public string ExitNotes => position.ExitNotes;

    public double EntryPrice => position.EntryPrice;

    public double EntryCommission => position.EntryCommission;

    public double ExitPrice => position.ExitPrice;

    public double ExitCommission => position.ExitCommission;

    public double SignedShares => position.SignedShares;

    public double SharesChange => position.SharesChange;

    public int ExitBarNum => position.ExitBarNum;

    public IDataBar ExitBar => position.ExitBar;

    public double GetAccumulatedProfit(int bar) => position.GetAccumulatedProfit(bar);

    public double GetMaxAccumulatedProfit(out int bar) => position.GetMaxAccumulatedProfit(out bar);

    public double Profit() => position.Profit();

    public double ProfitPct() => position.ProfitPct();

    public double CurrentProfit(int bar) => position.CurrentProfit(bar);

    public double CurrentProfitByOpenPrice(int bar) => position.CurrentProfitByOpenPrice(bar);

    public double CurrentProfitMin(int bar) => position.CurrentProfitMin(bar);

    public double CurrentProfitPct(int bar) => position.CurrentProfitPct(bar);

    public double OpenProfit(int bar) => position.OpenProfit(bar);

    public double OpenProfitPct(int bar) => position.OpenProfitPct(bar);

    public double OpenMFEPct(int bar) => position.OpenMFEPct(bar);

    public double OpenMAEPct(int bar) => position.OpenMAEPct(bar);

    public double MFEPct() => position.MFEPct();

    public double MAEPct() => position.MAEPct();

    public double OpenMFE(int bar) => position.OpenMFE(bar);

    public double OpenMAE(int bar) => position.OpenMAE(bar);

    public double MFE() => position.MFE();

    public double MAE() => position.MAE();

    public double ReducedMAE() => position.ReducedMAE();

    public DateTime MAEDate() => position.MAEDate();

    public int MAEBar() => position.MAEBar();

    public int FindHighBar(int bar) => position.FindHighBar(bar);

    public int FindLowBar(int bar) => position.FindLowBar(bar);

    public bool IsActiveForBar(int bar) => position.IsActiveForBar(bar);

    public double GetBalancePrice(int bar) => position.GetBalancePrice(bar);

    public double GetShares(int bar) => position.GetShares(bar);

    public double GetStop(int bar) => position.GetStop(bar);

    public IEnumerable<double> GetStops(int firstIndex, int lastIndex) => position.GetStops(firstIndex, lastIndex);

    public double GetTakeProfit(int bar) => position.GetTakeProfit(bar);

    public IEnumerable<double> GetTakeProfits(int firstIndex, int lastIndex) =>
        position.GetTakeProfits(firstIndex, lastIndex);

    public void ChangeAtMarket(int barNum, double newShares, string signalName, string? notes = null) =>
        position.ChangeAtMarket(barNum, newShares, signalName, notes);

    public void ChangeAtPrice(int barNum, double price, double newShares, string signalName, string? notes = null) =>
        position.ChangeAtPrice(barNum, price, newShares, signalName, notes);

    public void ChangeAtProfit(int barNum, double price, double newShares, string signalName, string? notes = null) =>
        position.ChangeAtProfit(barNum, price, newShares, signalName, notes);

    public void ChangeAtProfit(int barNum, double price, double? slippage, double newShares, string signalName,
        string? notes = null) => position.ChangeAtProfit(barNum, price, slippage, newShares, signalName, notes);

    public void ChangeAtStop(int barNum, double price, double newShares, string signalName, string? notes = null) =>
        position.ChangeAtStop(barNum, price, newShares, signalName, notes);

    public void ChangeAtStop(int barNum, double price, double? slippage, double newShares, string signalName,
        string? notes = null) => position.ChangeAtStop(barNum, price, slippage, newShares, signalName, notes);

    public void VirtualChange(int barNum, double price, double newShares, string signalName, string? notes = null) =>
        position.VirtualChange(barNum, price, newShares, signalName, notes);

    public void CloseAtMarket(int barNum, string signalName, string? notes = null) =>
        position.CloseAtMarket(barNum, signalName, notes);

    public void CloseAtStop(int barNum, double price, string signalName, string? notes = null) =>
        position.CloseAtStop(barNum, price, signalName, notes);

    public void CloseAtStop(int barNum, double price, double? slippage, string signalName, string? notes = null) =>
        position.CloseAtStop(barNum, price, slippage, signalName, notes);

    public void CloseAtProfit(int barNum, double price, string signalName, string? notes = null) =>
        position.CloseAtProfit(barNum, price, signalName, notes);

    public void CloseAtProfit(int barNum, double price, double? slippage, string signalName, string? notes = null) =>
        position.CloseAtProfit(barNum, price, slippage, signalName, notes);

    public void CloseAtPrice(int barNum, double price, string signalName, string? notes = null) =>
        position.CloseAtPrice(barNum, price, signalName, notes);

    public void CloseAtMarket(int barNum, string signalName, string notes, PositionExecution execution) =>
        position.CloseAtMarket(barNum, signalName, notes, execution);

    public void CloseAtStop(int barNum, double price, string signalName, string notes, PositionExecution execution) =>
        position.CloseAtStop(barNum, price, signalName, notes, execution);

    public void CloseAtStop(int barNum, double price, double? slippage, string signalName, string notes,
        PositionExecution execution) => position.CloseAtStop(barNum, price, slippage, signalName, notes, execution);

    public void CloseAtProfit(int barNum, double price, string signalName, string notes, PositionExecution execution) =>
        position.CloseAtProfit(barNum, price, signalName, notes, execution);

    public void CloseAtProfit(int barNum, double price, double? slippage, string signalName, string notes,
        PositionExecution execution) => position.CloseAtProfit(barNum, price, slippage, signalName, notes, execution);

    public void CloseAtPrice(int barNum, double price, string signalName, string notes, PositionExecution execution) =>
        position.CloseAtPrice(barNum, price, signalName, notes, execution);

    public IPositionsList ParentList => position.ParentList;

    public ISecurity Security => position.Security;

    public IReadOnlyList<IPositionInfo> ChangeInfos => position.ChangeInfos;

    public bool IsVirtual => position.IsVirtual;

    public bool IsVirtualClosed => position.IsVirtualClosed;

    public PositionState PositionState => position.PositionState;

    public double FullEntryCommission => position.FullEntryCommission;

    public double FullExitCommission => position.FullExitCommission;

    public int BarsHeld => position.BarsHeld;

    public double? ProfitPerTrade => position.ProfitPerTrade;

    public bool IsLong => position.IsLong;

    public bool IsShort => position.IsShort;

    public bool IsActive => position.IsActive;

    public double SharesOrigin => position.SharesOrigin;

    public double MaxShares => position.MaxShares;

    public PositionExecution EntryExecution => position.EntryExecution;

    public PositionExecution? ExitExecution => position.ExitExecution;
}