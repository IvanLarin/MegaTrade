namespace MegaTrade.Common.Extensions;

public static class NumberExtensions
{
    public static bool IsEqualTo(this double number, double another) => Math.Abs(number - another) < double.Epsilon;

    public static bool IsMoreThan(this double number, double another) => number - another > double.Epsilon;

    public static bool IsLessOrEqualTo(this double number, double another) =>
        another.IsMoreThan(number) || another.IsEqualTo(number);

    public static bool IsDivisibleBy(this int number, int by) => number % by == 0;
}