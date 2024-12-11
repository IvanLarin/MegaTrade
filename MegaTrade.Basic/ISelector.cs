namespace MegaTrade.Basic;

internal interface ISelector
{
    IList<T> Select<T>(Func<T> func) where T : struct;
}