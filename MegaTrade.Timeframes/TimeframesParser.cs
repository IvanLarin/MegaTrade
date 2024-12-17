using System.Text.RegularExpressions;

namespace MegaTrade.Timeframes;

internal class TimeframesParser
{
    public int[][] Parse(string expression)
    {
        var sanitized = Sanitize(expression);

        if (!FineSyntax(sanitized))
            throw new Exception("Неправильный синтаксис у Мегасжатия. Вот нормальный пример 3-х таймфреймов: {{1, 5, 15}, {15, 60}, {120, 240, 1440}}");

        var input = ParseExpression(sanitized);

        if (!MultipleOfDays(input))
            throw new Exception("Не кратно дню у Мегасжатия");

        if (!FineSizes(input))
            throw new Exception("Перехлёст тайфреймов у Мегасжатия");

        var combinations = MakeCombinations(input);

        return combinations;
    }

    private bool MultipleOfDays(int[][] input)
    {
        foreach (var array in input)
        {
            foreach (var number in array)
            {
                if (number == 0 || 1440 % number != 0 && number % 1440 != 0)
                    return false;
            }
        }

        return true;
    }

    private string Sanitize(string expression) => expression.Replace(" ", "");

    private bool FineSyntax(string expression) =>
        Regex.IsMatch(expression, @"^\{((\{\d+(,\d+)*\}))*(,(\d+|(\{\d+(,\d+)*\})))*\}$");

    private int[][] ParseExpression(string expression)
    {
        List<int[]> result = [];

        MatchCollection matches = Regex.Matches(expression, @"\{(\d+(,\d+)*)\}");

        foreach (Match match in matches)
        {
            int[] numbers = Array.ConvertAll(match.Groups[1].Value.Split(','), int.Parse);
            result.Add(numbers);
        }

        return result.ToArray();
    }

    private bool FineSizes(int[][] input)
    {
        if (input.Length < 2) return true;

        for (int i = 1; i < input.Length; i++)
        {
            var prev = input[i - 1];
            var current = input[i];
            if (prev.Min() >= current.Min()) return false;
        }

        return true;
    }

    private int[][] MakeCombinations(int[][] input)
    {
        List<int[]> result = new List<int[]>();
        GenerateCombinations(input, 0, new List<int>(), result);
        return result.ToArray();
    }

    private void GenerateCombinations(int[][] arrays, int depth, List<int> current, List<int[]> result)
    {
        if (depth == arrays.Length)
        {
            if (current.Any() && IsStrictlyIncreasing(current))
                result.Add(current.ToArray());
            return;
        }

        foreach (var number in arrays[depth])
        {
            current.Add(number);
            GenerateCombinations(arrays, depth + 1, current, result);
            current.RemoveAt(current.Count - 1);
        }
    }

    private bool IsStrictlyIncreasing(List<int> combination)
    {
        if (combination.Count < 2) return true;

        for (int i = 1; i < combination.Count; i++)
        {
            if (combination[i - 1] >= combination[i])
            {
                return false;
            }
        }

        return true;
    }
}