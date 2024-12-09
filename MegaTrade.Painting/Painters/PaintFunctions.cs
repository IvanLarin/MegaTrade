using TSLab.Script;

namespace MegaTrade.Draw.Painters;

internal class PaintFunctions : PaintBase, IPaintFunctions
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
}