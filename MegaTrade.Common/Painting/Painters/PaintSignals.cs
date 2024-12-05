using TSLab.Script;

namespace MegaTrade.Common.Painting.Painters;

internal class PaintSignals : PaintBase, IPaintSignals
{
    private const int SignalOpacity = 150;

    public void Signal(IList<bool> values, string name) =>
        DrawHistogram(values, name, NeutralPalette.PopColor(), out var usedColor);

    public void Signal(IList<bool> values, string name, out Color usedColor) =>
        DrawHistogram(values, name, NeutralPalette.PopColor(), out usedColor);

    public void Signal(IList<bool> values, string name, AnimalColor animalColor) =>
        DrawHistogram(values, name, ChooseAnimalPalette(animalColor).PopColor(), out var usedColor);

    public void Signal(IList<bool> values, string name, AnimalColor animalColor, out Color usedColor) =>
        DrawHistogram(values, name, ChooseAnimalPalette(animalColor).PopColor(), out usedColor);

    public void Signal(IList<bool> values, string name, Color color) =>
        DrawHistogram(values, name, color, out var usedColor);

    public void Signal(IList<bool> values, string name, Color color, out Color usedColor) =>
        DrawHistogram(values, name, color, out usedColor);

    private void DrawHistogram(IList<bool> values, string name, Color colorToDraw, out Color usedColor)
    {
        var chart = Graph.AddList(
            name,
            values,
            ListStyles.HISTOHRAM,
            usedColor = colorToDraw,
            LineStyles.SOLID,
            PaneSides.LEFT);

        chart.Opacity = SignalOpacity;
        chart.Autoscaling = true;
    }
}