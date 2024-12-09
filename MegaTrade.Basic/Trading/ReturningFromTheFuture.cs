using TSLab.DataSource;
using TSLab.Script;

namespace MegaTrade.Basic.Trading;

internal class ReturningFromTheFuture : IPositionInfo
{
    public int EntryBarNum => Math.Min(Now, PositionInfo.EntryBarNum);

    public IDataBar EntryBar => BasicTimeframe.Bars[EntryBarNum];

    public double EntryPrice =>
        PositionInfo.EntryBarNum > Now ? BasicTimeframe.Bars[Now].Close : PositionInfo.EntryPrice;

    public double AverageEntryPrice => PositionInfo.EntryBarNum > Now
        ? BasicTimeframe.Bars[Now].Close
        : PositionInfo.AverageEntryPrice;

    public string EntrySignalName => PositionInfo.EntrySignalName;

    public string EntryNotes => PositionInfo.EntryNotes;

    public string ExitSignalName => PositionInfo.ExitSignalName;

    public string ExitNotes => PositionInfo.ExitNotes;

    public double EntryCommission => PositionInfo.EntryCommission;

    public double ExitPrice => PositionInfo.ExitPrice;

    public double ExitCommission => PositionInfo.ExitCommission;

    public double Shares => PositionInfo.Shares;

    public double SignedShares => PositionInfo.SignedShares;

    public double SharesChange => PositionInfo.SharesChange;

    public int ExitBarNum => PositionInfo.ExitBarNum;

    public IDataBar ExitBar => PositionInfo.ExitBar;

    public required IPositionInfo PositionInfo { get; init; }

    public required INowProvider NowProvider { get; init; }

    public required ISecurity BasicTimeframe { get; init; }

    private int Now => NowProvider.Now;
}