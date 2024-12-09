using TSLab.Script;

namespace MegaTrade.Draw.Palettes;

public abstract class PaletteBase : IPalette
{
    protected abstract IList<Color> Colors { get; }

    public Color PopColor()
    {
        if (Colors.Count == 0)
            throw new Exception("В палитре кончились цвета");

        var color = Colors[0];

        Colors.RemoveAt(0);

        return color;
    }
}