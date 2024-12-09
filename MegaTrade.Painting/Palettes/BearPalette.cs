using TSLab.Script;

namespace MegaTrade.Draw.Palettes;

public class BearPalette : PaletteBase
{
    private IList<Color>? _colors;

    protected override IList<Color> Colors => _colors ??=
    [
        ScriptColors.Crimson,
        ScriptColors.Firebrick,
        ScriptColors.DarkRed,
        ScriptColors.Red,
        ScriptColors.IndianRed,
        ScriptColors.Maroon,
        ScriptColors.Brown,
        ScriptColors.Tomato,
        ScriptColors.OrangeRed,
        ScriptColors.Coral,
        ScriptColors.Salmon,
        ScriptColors.LightCoral,
        ScriptColors.PaleVioletRed,
        ScriptColors.DeepPink,
        ScriptColors.HotPink,
        ScriptColors.Pink,
        ScriptColors.LightPink,
    ];
}