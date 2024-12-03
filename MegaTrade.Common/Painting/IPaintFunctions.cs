using TSLab.Script;

namespace MegaTrade.Common.Painting;

public interface IPaintFunctions
{
    public void Function(IList<double> values, string name, Color? color = null);
}