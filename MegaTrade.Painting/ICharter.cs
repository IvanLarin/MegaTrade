namespace MegaTrade.Draw;

public interface ICharter
{
    IPaint Chart(string name);

    IPaint _______________Chart_______________(string name) => Chart(name);
}