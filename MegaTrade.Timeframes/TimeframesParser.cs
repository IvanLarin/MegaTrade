using System.Text.RegularExpressions;

namespace MegaTrade.Timeframes;

internal class TimeframesParser
{
    public int[][] Parse(string expression)
    {
        var sanitized = Sanitize(expression);

        if (!FineSyntax(sanitized))
            throw new Exception("Неправильный синтаксис у Мегасжатия");

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

    private string Sanitize(string expression)
    {
        string sanitizedExpression = expression.Replace(" ", "");

        if (sanitizedExpression.StartsWith("{") && sanitizedExpression.EndsWith("}"))
        {
            string innerContent = sanitizedExpression[1..^1];

            int openBraces = 0;
            bool isBalanced = true;

            foreach (char c in innerContent)
            {
                if (c == '{')
                {
                    openBraces++;
                }
                else if (c == '}')
                {
                    openBraces--;
                    if (openBraces < 0)
                    {
                        isBalanced = false;
                        break;
                    }
                }
            }

            if (isBalanced && openBraces == 0)
            {
                return innerContent;
            }
        }

        return sanitizedExpression;
    }

    private bool FineSyntax(string expression) =>
        Regex.IsMatch(expression, @"^(\d+|(\{\d+(,\d+)*\}))*(,(\d+|(\{\d+(,\d+)*\})))*$");

    private int[][] ParseExpression(string expression)
    {
        List<int[]> result = new List<int[]>();

        MatchCollection matches = Regex.Matches(expression, @"(\{(\d+(,\d+)*)\}|(\d+))");

        foreach (Match match in matches)
        {
            if (match.Groups[0].Value.StartsWith("{"))
            {
                string innerGroup = match.Groups[2].Value;
                int[] numbers = Array.ConvertAll(innerGroup.Split(','), int.Parse);
                result.Add(numbers);
            }
            else
            {
                int number = int.Parse(match.Groups[0].Value);
                result.Add([number]);
            }
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