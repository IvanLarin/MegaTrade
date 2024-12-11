using MegaTrade.Basic.Trading.Position;

namespace MegaTrade.Basic.Trading;

internal class HistoryTrade : TradeBase
{
    protected override void UpdateLongPosition()
    {
        var position = BasicTimeframe.Positions.GetLastLongPositionActive(OnTheNextCandle);

        _longPosition = new MarketPosition(position)
        {
            BasicTimeframe = BasicTimeframe,
            NowProvider = NowProvider
        };
    }

    protected override void UpdateShortPosition()
    {
        var position = BasicTimeframe.Positions.GetLastShortPositionActive(OnTheNextCandle);

        _longPosition = new MarketPosition(position)
        {
            BasicTimeframe = BasicTimeframe,
            NowProvider = NowProvider
        };
    }

    private IMarketPosition? _longPosition;

    private IMarketPosition? _shortPosition;

    protected override IMarketPosition LongPosition => _longPosition ??= new NullMarketPosition
    {
        BasicTimeframe = BasicTimeframe
    };

    protected override IMarketPosition ShortPosition => _shortPosition ??= new NullMarketPosition
    {
        BasicTimeframe = BasicTimeframe
    };
}