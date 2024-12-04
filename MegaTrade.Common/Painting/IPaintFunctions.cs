using TSLab.Script;

namespace MegaTrade.Common.Painting;

internal interface IPaintFunctions
{
    void Function(IList<double> values, string name, Color? color = null);

    void Function(IList<double> values, string name, out Color usedColor);
}