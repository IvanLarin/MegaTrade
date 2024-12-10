using TSLab.Script;

namespace MegaTrade.Basic.Trading;

internal class HistoryTrade : TradeBase
{
    protected override void UpdateNow()
    {
        _longPosition = NextLongPosition is { IsActive: true } ? NextLongPosition : null;

        _shortPosition = NextShortPosition is { IsActive: true } ? NextShortPosition : null;
    }

    protected override void UpdateNext()
    {
        if (IsLongEnter || IsLongExit || NextLongPosition != null)
            _nextLongPosition = BasicTimeframe.Positions.GetLastLongPositionActive(OnTheNextCandle);

        if (IsShortEnter || IsShortExit || NextShortPosition != null)
            _nextShortPosition = BasicTimeframe.Positions.GetLastLongPositionActive(OnTheNextCandle);
    }

    private IPosition? _longPosition;

    private IPosition? _shortPosition;

    protected override IPosition? LongPosition => _longPosition;

    protected override IPosition? ShortPosition => _shortPosition;

    private IPosition? _nextLongPosition;

    private IPosition? _nextShortPosition;

    protected override IPosition? NextLongPosition => _nextLongPosition;

    protected override IPosition? NextShortPosition => _nextShortPosition;
}