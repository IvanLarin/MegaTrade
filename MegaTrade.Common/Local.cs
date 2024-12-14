using TSLab.Script.Handlers;

namespace MegaTrade.Common;

public static class Local
{
    public static IContext? Context
    {
        get => LocalContext.Value;
        set => LocalContext.Value = value;
    }

    private static readonly AsyncLocal<IContext?> LocalContext = new();

    //TODO сделать для IContext null object
}