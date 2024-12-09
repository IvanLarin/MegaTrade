namespace MegaTrade.Basic.Gapping;

internal interface IAntiGap
{
    bool IsLastCandleOfSession(int barNumber);

    bool IsJustBeforeLastCandleOfSession(int barNumber);
}