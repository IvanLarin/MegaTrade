using TSLab.Script;
using TSLab.Script.Handlers;

namespace MegaTrade.Timeframes;

[HandlerCategory("Мегатрон")]
[HandlerName("Мегатаймфрейм")]
[InputsCount(1)]
[Input(0, TemplateTypes.SECURITY, Name = "Инструмент")]
[OutputsCount(1)]
[OutputType(TemplateTypes.SECURITY)]
public class MegaTimeframe : ICompressHandler, IContextUses, INeedVariableVisual
{
    public ISecurity Execute(ISecurity source)
    {
        var security = GetSecurity(source);

        if (Context != null)
            Context.ScriptResults[VariableVisual] = security.Interval;

        return security;
    }

    private ISecurity GetSecurity(ISecurity source)
    {
        if (source is not IMultiSecurity multiSecurity) return source;

        if (TimeframeNumber > multiSecurity.All.Length)
            throw new Exception(
                $"Некорректный номер таймфрейма у Мегатаймфрейма. Доступный интервал [1, {multiSecurity.All.Length}].");

        return multiSecurity.All[TimeframeNumber - 1];
    }

    [HandlerParameter(Name = "Номер таймфрейма", IsShown = true, Default = "1", Min = "1", NotOptimized = true)]
    public int TimeframeNumber { get; set; }

    public string VariableVisual { get; set; } = "";

    public IContext? Context { get; set; }
}