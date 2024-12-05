using MegaTrade.Common.Painting.Palettes;
using TSLab.Script;

namespace MegaTrade.Common.Painting.Painters;

internal abstract class PaintBase
{
    protected IPalette ChooseAnimalPalette(AnimalColor animalColor) =>
        animalColor == AnimalColor.Bull ? BullPalette :
        animalColor == AnimalColor.Bear ? BearPalette : NeutralPalette;

    public required IPalette BullPalette { protected get; init; }

    public required IPalette BearPalette { protected get; init; }

    public required IPalette NeutralPalette { protected get; init; }

    public required IGraphPane Graph { protected get; init; }
}