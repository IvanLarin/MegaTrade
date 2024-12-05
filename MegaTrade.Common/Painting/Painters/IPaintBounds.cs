namespace MegaTrade.Common.Painting.Painters;

internal interface IPaintBounds
{
    void Bound(params double[] bounds);

    void BoundOfMin(params IList<double>[] bounds);

    void BoundOfMax(params IList<double>[] bounds);

    void BoundOfMajorMax(params IList<double>[] bounds);

    void BoundOfMajorMin(params IList<double>[] bounds);

    void BoundOfMajorMax(double majorityPercent, IList<double>[] bounds);

    void BoundOfMajorMin(double majorityPercent, IList<double>[] bounds);
}