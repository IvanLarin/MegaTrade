using TSLab.Script;

namespace MegaTrade.Common.Painting;

internal class PaintHistograms : PaintBase, IPaintHistograms
{
    public void Histogram(IList<double> values, string name) =>
        DrawHistogram(values, name, NeutralPalette.PopColor(), out var usedColor);

    public void Histogram(IList<double> values, string name, out Color usedColor) =>
        DrawHistogram(values, name, NeutralPalette.PopColor(), out usedColor);

    public void Histogram(IList<double> values, string name, AnimalColor animalColor) =>
        DrawHistogram(values, name, ChooseAnimalPalette(animalColor).PopColor(), out var usedColor);

    public void Histogram(IList<double> values, string name, AnimalColor animalColor, out Color usedColor) =>
        DrawHistogram(values, name, ChooseAnimalPalette(animalColor).PopColor(), out usedColor);

    public void Histogram(IList<double> values, string name, Color color) =>
        DrawHistogram(values, name, color, out var usedColor);

    public void Histogram(IList<double> values, string name, Color color, out Color usedColor) =>
        DrawHistogram(values, name, color, out usedColor);

    private void DrawHistogram(IList<double> values, string name, Color colorToDraw, out Color usedColor)
    {
        var chart = Graph.AddList(
            name,
            values,
            ListStyles.HISTOHRAM,
            usedColor = colorToDraw,
            LineStyles.SOLID,
            PaneSides.LEFT);
        chart.Autoscaling = true;
    }
}