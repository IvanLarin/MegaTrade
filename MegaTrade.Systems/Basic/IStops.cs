namespace MegaTrade.Systems.Basic;

internal interface IStops
{
    double? LongTake { get; }

    double? LongStop { get; }

    double? ShortTake { get; }

    double? ShortStop { get; }
}