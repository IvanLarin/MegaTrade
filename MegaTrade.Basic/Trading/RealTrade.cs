using MegaTrade.Basic.Trading.Position;

namespace MegaTrade.Basic.Trading;

internal class RealTrade : TradeBase
{
    protected override IMarketPosition LongPosition
    {
        get
        {
            var position = BasicTimeframe.Positions.GetLastLongPositionActive(Now);

            if (position != null)
                return new MarketPosition(position)
                {
                    BasicTimeframe = BasicTimeframe,
                    NowProvider = NowProvider
                };

            return NullPosition;
        }
    }

    protected override IMarketPosition ShortPosition
    {
        get
        {
            var position = BasicTimeframe.Positions.GetLastShortPositionActive(Now);

            if (position != null)
                return new MarketPosition(position)
                {
                    BasicTimeframe = BasicTimeframe,
                    NowProvider = NowProvider
                };

            return NullPosition;
        }
    }

    private IMarketPosition? _nullPosition;

    private IMarketPosition NullPosition => _nullPosition ??= new NullMarketPosition
    {
        BasicTimeframe = BasicTimeframe
    };

    protected override void UpdateLongPosition()
    {
    }

    protected override void UpdateShortPosition()
    {
    }
}