using TSLab.Script;

namespace MegaTrade.Common.Painting;

public class BullPalette : PaletteBase
{
    private IList<Color>? _colors;

    protected override IList<Color> Colors => _colors ??=
    [
        ScriptColors.Green,
        ScriptColors.LimeGreen,
        ScriptColors.ForestGreen,
        ScriptColors.DarkGreen,
        ScriptColors.SeaGreen,
        ScriptColors.MediumSeaGreen,
        ScriptColors.SpringGreen,
        ScriptColors.MediumSpringGreen,
        ScriptColors.LightGreen,
        ScriptColors.PaleGreen,
        ScriptColors.DarkSeaGreen,
        ScriptColors.MediumAquamarine,
        ScriptColors.YellowGreen,
        ScriptColors.LawnGreen,
        ScriptColors.Chartreuse,
        ScriptColors.GreenYellow,
        ScriptColors.OliveDrab,
        ScriptColors.DarkOliveGreen,
        ScriptColors.Olive
    ];
}