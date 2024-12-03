using TSLab.Script;

namespace MegaTrade.Common.Painting;

public class Palette : IPalette
{
    private readonly List<Color> _colors =
    [
        ScriptColors.Gold,
        ScriptColors.Aqua,
        ScriptColors.OrangeRed,
        ScriptColors.SteelBlue,
        ScriptColors.Lime,
        ScriptColors.HotPink,
    ];

    public Color PopColor()
    {
        if (_colors.Count == 0)
            throw new Exception("В палитре кончились цвета");

        var color = _colors[0];

        _colors.RemoveAt(0);

        return color;
    }
}