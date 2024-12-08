﻿using TSLab.Script;

namespace MegaTrade.Basic.Trading;

internal class HistoryTrade : TradeBase
{
    protected override void UpdateLongPosition() =>
        _longPosition = BasicTimeframe.Positions.GetLastLongPositionActive(Now);

    protected override void UpdateShortPosition() =>
        _shortPosition = BasicTimeframe.Positions.GetLastShortPositionActive(Now);

    private IPosition? _longPosition;

    private IPosition? _shortPosition;

    protected override IPosition? LongPosition => _longPosition;

    protected override IPosition? ShortPosition => _shortPosition;
}