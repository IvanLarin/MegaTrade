using TSLab.DataSource;
using TSLab.Script;

namespace MegaTrade.Basic.Trading;

internal class ReturningFromTheFuture(IPositionInfo positionInfo) : IPositionInfo
{
    public int EntryBarNum => Math.Min(Now, positionInfo.EntryBarNum);

    public IDataBar EntryBar => BasicTimeframe.Bars[EntryBarNum];

    public double EntryPrice =>
        positionInfo.EntryBarNum > Now ? BasicTimeframe.Bars[Now].Close : positionInfo.EntryPrice;

    public double AverageEntryPrice => positionInfo.EntryBarNum > Now
        ? BasicTimeframe.Bars[Now].Close
        : positionInfo.AverageEntryPrice;

    public string EntrySignalName => positionInfo.EntrySignalName;

    public string EntryNotes => positionInfo.EntryNotes;

    public string ExitSignalName => positionInfo.ExitSignalName;

    public string ExitNotes => positionInfo.ExitNotes;

    public double EntryCommission => positionInfo.EntryCommission;

    public double ExitPrice => positionInfo.ExitPrice;

    public double ExitCommission => positionInfo.ExitCommission;

    public double Shares => positionInfo.Shares;

    public double SignedShares => positionInfo.SignedShares;

    public double SharesChange => positionInfo.SharesChange;

    public int ExitBarNum => positionInfo.ExitBarNum;

    public IDataBar ExitBar => positionInfo.ExitBar;

    public required INowProvider NowProvider { get; init; }

    public required ISecurity BasicTimeframe { get; init; }

    private int Now => NowProvider.Now;
}