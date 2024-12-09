namespace MegaTrade.Basic.Gapping;

internal interface IAntiGap
{
    bool IsLastCandleOfSession { get; }

    bool IsJustBeforeLastCandleOfSession { get; }
}