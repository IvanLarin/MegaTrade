using TSLab.Script;

namespace MegaTrade.Basic.Trading;

internal class RealTrade : TradeBase
{
    protected override IPosition? LongPosition =>
        BasicTimeframe.Positions.GetLastLongPositionActive(Now);

    protected override IPosition? ShortPosition =>
        BasicTimeframe.Positions.GetLastShortPositionActive(Now);

    protected override IPosition? NextLongPosition =>
        BasicTimeframe.Positions.GetLastLongPositionActive(OnTheNextCandle);

    protected override IPosition? NextShortPosition =>
        BasicTimeframe.Positions.GetLastShortPositionActive(OnTheNextCandle);

    protected override void UpdateNow()
    {
    }

    protected override void UpdateNext()
    {
    }
}