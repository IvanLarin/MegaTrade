using TSLab.Script;

namespace MegaTrade.Common.Painting;

internal class PaintFunctions : IPaintFunctions
{
    public void Function(IList<double> values, string name) =>
        DrawFunction(values, name, NeutralPalette.PopColor(), out var usedColor);

    public void Function(IList<double> values, string name, out Color usedColor) =>
        DrawFunction(values, name, NeutralPalette.PopColor(), out usedColor);

    public void Function(IList<double> values, string name, AnimalColor animalColor) => DrawFunction(values, name,
        ChooseAnimalPalette(animalColor).PopColor(), out var usedColor);

    public void Function(IList<double> values, string name, AnimalColor animalColor, out Color usedColor) =>
        DrawFunction(values, name, ChooseAnimalPalette(animalColor).PopColor(), out usedColor);

    public void Function(IList<double> values, string name, Color color) =>
        DrawFunction(values, name, color, out var usedColor);

    public void Function(IList<double> values, string name, Color color, out Color usedColor) =>
        DrawFunction(values, name, color, out usedColor);

    private void DrawFunction(IList<double> values, string name, Color colorToDraw, out Color usedColor) =>
        Graph.AddList(
            name,
            values,
            ListStyles.LINE,
            usedColor = colorToDraw,
            LineStyles.SOLID,
            PaneSides.RIGHT);

    private IPalette ChooseAnimalPalette(AnimalColor animalColor) =>
        animalColor == AnimalColor.Bull ? BullPalette :
        animalColor == AnimalColor.Bear ? BearPalette : NeutralPalette;

    public required IPalette BullPalette { private get; init; }

    public required IPalette BearPalette { private get; init; }

    public required IPalette NeutralPalette { private get; init; }

    public required IGraphPane Graph { private get; init; }
}