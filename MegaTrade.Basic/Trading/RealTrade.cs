using TSLab.Script;

namespace MegaTrade.Basic.Trading;

internal class RealTrade : TradeBase
{
    protected override IPosition? LongPosition =>
        BasicTimeframe.Positions.GetLastLongPositionActive(Now);

    protected override IPosition? ShortPosition =>
        BasicTimeframe.Positions.GetLastShortPositionActive(Now);

    protected override void UpdateLongPosition()
    {
    }

    protected override void UpdateShortPosition()
    {
    }
}