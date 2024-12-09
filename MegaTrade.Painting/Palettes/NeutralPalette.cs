using TSLab.Script;

namespace MegaTrade.Draw.Palettes;

public class NeutralPalette : PaletteBase
{
    private IList<Color>? _colors;

    protected override IList<Color> Colors => _colors ??=
    [
        ScriptColors.Gold,
        ScriptColors.Aqua,
        ScriptColors.OrangeRed,
        ScriptColors.SteelBlue,
        ScriptColors.Lime,
        ScriptColors.HotPink,
        ScriptColors.Red,
        ScriptColors.Violet,
        ScriptColors.Green,
        ScriptColors.Brown,
        ScriptColors.YellowGreen
    ];
}