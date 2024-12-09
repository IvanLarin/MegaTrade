namespace MegaTrade.Basic.Trading;

internal interface IStops
{
    double? LongTake { get; }

    double? LongStop { get; }

    double? ShortTake { get; }

    double? ShortStop { get; }
}