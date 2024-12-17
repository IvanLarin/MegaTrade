using MegaTrade.Common.Extensions;
using TSLab.DataSource;
using TSLab.Script;
using TSLab.Script.Handlers;
using TSLab.Script.Handlers.Options;

namespace MegaTrade.Timeframes;

[HandlerCategory("��������")]
[HandlerName("����������")]
[InputsCount(1)]
[Input(0, TemplateTypes.SECURITY, Name = "����������")]
[OutputsCount(1)]
[OutputType(TemplateTypes.SECURITY)]
public class MegaCompress : ICompressHandler, IContextUses
{
    public ISecurity Execute(ISecurity source)
    {
        var allTimeframes = new TimeframesParser().Parse(Timeframes);

        if (allTimeframes.Length < Number)
            throw new Exception($"������� ������� Number � ����������. Max=={allTimeframes}, � Number=={Number}");

        var timeframes = allTimeframes[Number - 1];

        var securities = timeframes
            .Select(interval =>
            {
                switch (source.IntervalBase)
                {
                    case DataIntervals.VOLUME:
                        return source.CompressToVolume(new Interval(interval, source.IntervalBase));
                    case DataIntervals.PRICERANGE:
                        return source.CompressToPriceRange(new Interval(interval, source.IntervalBase));
                    default:
                        return source.DailyCompressTo(interval);
                }
            }).ToArray();

        if (Context != null)
            Context.ScriptResults["����� ��������� �� ����������"] = allTimeframes.Length;

        return new MultiSecurity(securities);
    }

    [HelperName("Timeframes", Constants.En)]
    [HelperName("����������", Constants.Ru)]
    [HandlerParameter(true, "{5,15}", NotOptimized = true)]
    public string Timeframes { get; set; } = "";

    [HandlerParameter(Name = "����� ���������", IsShown = true, Default = "1", Min = "1", Step = "1", EditorMin = "1")]
    public int Number { get; set; }

    public IContext? Context { get; set; }
}
