using TSLab.Script;

namespace MegaTrade.Draw.Painters;

internal class PaintFunctions : PaintBase, IPaintFunctions
{
    public void Function(IList<double> values, string name) =>
        DrawFunction(values, name, NeutralPalette.PopColor(), ListStyles.LINE, out var usedColor);

    public void Function(IList<double> values, string name, out Color usedColor) =>
        DrawFunction(values, name, NeutralPalette.PopColor(), ListStyles.LINE, out usedColor);

    public void Function(IList<double> values, string name, AnimalColor animalColor) => DrawFunction(values, name,
        ChooseAnimalPalette(animalColor).PopColor(), ListStyles.LINE, out var usedColor);

    public void Function(IList<double> values, string name, AnimalColor animalColor, out Color usedColor) =>
        DrawFunction(values, name, ChooseAnimalPalette(animalColor).PopColor(), ListStyles.LINE, out usedColor);

    public void Function(IList<double> values, string name, Color color) =>
        DrawFunction(values, name, color, ListStyles.LINE, out var usedColor);

    public void Function(IList<double> values, string name, Color color, out Color usedColor) =>
        DrawFunction(values, name, color, ListStyles.LINE, out usedColor);

    public void FunctionWithoutZeroes(IList<double> values, string name) =>
        DrawFunction(values, name, NeutralPalette.PopColor(), ListStyles.LINE_WO_ZERO, out var usedColor);

    public void FunctionWithoutZeroes(IList<double> values, string name, out Color usedColor) =>
        DrawFunction(values, name, NeutralPalette.PopColor(), ListStyles.LINE_WO_ZERO, out usedColor);

    public void FunctionWithoutZeroes(IList<double> values, string name, AnimalColor animalColor) => DrawFunction(
        values, name,
        ChooseAnimalPalette(animalColor).PopColor(), ListStyles.LINE_WO_ZERO, out var usedColor);

    public void FunctionWithoutZeroes(IList<double> values, string name, AnimalColor animalColor,
        out Color usedColor) =>
        DrawFunction(values, name, ChooseAnimalPalette(animalColor).PopColor(), ListStyles.LINE_WO_ZERO, out usedColor);

    public void FunctionWithoutZeroes(IList<double> values, string name, Color color) =>
        DrawFunction(values, name, color, ListStyles.LINE_WO_ZERO, out var usedColor);

    public void FunctionWithoutZeroes(IList<double> values, string name, Color color, out Color usedColor) =>
        DrawFunction(values, name, color, ListStyles.LINE_WO_ZERO, out usedColor);

    private void DrawFunction(IList<double> values, string name, Color colorToDraw, ListStyles listStyle,
        out Color usedColor) =>
        Graph.AddList(
            name,
            values,
            listStyle,
            usedColor = colorToDraw,
            LineStyles.SOLID,
            PaneSides.RIGHT);
}