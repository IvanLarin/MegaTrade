using TSLab.Script;

namespace MegaTrade.Draw.Painters;

internal interface IPaintLevels
{
    void Level(double value, string name, Color color);

    void Level(double value, string name, Color color, out Color usedColor);

    void Level(double value, string name, AnimalColor animalColor);

    void Level(double value, string name, AnimalColor animalColor, out Color usedColor);
}