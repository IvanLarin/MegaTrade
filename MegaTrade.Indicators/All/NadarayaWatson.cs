using ILGPU;
using ILGPU.Runtime;
using MegaTrade.Common;
using MegaTrade.Common.Caching;
using MegaTrade.Common.Calculating;
using MegaTrade.Common.Extensions;

namespace MegaTrade.Indicators.All;

internal class NadarayaWatson : Calculator<IList<double>>
{
    protected override IList<double> Calculate()
    {
        if (Source.Count == 0) return [];

        var (nwe, sae) = GetNadaraya();

        var result = nwe.Zip(sae, (average, error) =>
            average + error * Multiplier * (int)Direction).ToArray();

        return result;
    }

    private (float[] nwe, float[] sae) GetNadaraya()
    {
        using var context = Context.Create(builder => builder.AllAccelerators());

        var bestGpu = context.GetPreferredDevices(false, false)
            .OrderByDescending(x => x.NumMultiprocessors)
            .ThenBy(x => x.MaxNumThreads)
            .First();

        using var accelerator = bestGpu.CreateAccelerator(context);
        using var stream = accelerator.CreateStream();

        var prices = Local.Context.GetOrCreateArray(Source.Select(x => (float)x));
        using MemoryBuffer1D<float, Stride1D.Dense> pricesBuffer = accelerator.Allocate1D(stream, prices);
        using MemoryBuffer1D<float, Stride1D.Dense> gaussBuffer = accelerator.Allocate1D(stream, Gauss);
        using MemoryBuffer1D<float, Stride1D.Dense> nweBuffer = accelerator.Allocate1D<float>(prices.Length);
        using MemoryBuffer1D<float, Stride1D.Dense> saeBuffer = accelerator.Allocate1D<float>(prices.Length);

        var work = new Work(pricesBuffer, gaussBuffer, GaussLength, Range, nweBuffer, saeBuffer);

        Action<AcceleratorStream, Index1D, Work> kernel = accelerator
            .LoadAutoGroupedKernel<Index1D, Work>(Work.Do);

        kernel(stream, prices.Length, work);

        stream.Synchronize();

        var nweResult = nweBuffer.GetAsArray1D();
        var saeResult = saeBuffer.GetAsArray1D();

        return (nweResult, saeResult);
    }

    public struct Work(
        ArrayView1D<float, Stride1D.Dense> prices,
        ArrayView1D<float, Stride1D.Dense> gauss,
        int gaussLength,
        int range,
        ArrayView1D<float, Stride1D.Dense> nwe,
        ArrayView1D<float, Stride1D.Dense> sae)
    {
        public ArrayView1D<float, Stride1D.Dense> Prices = prices;
        public ArrayView1D<float, Stride1D.Dense> Gauss = gauss;
        public int GaussLength = gaussLength;
        public int Range = range;
        public ArrayView1D<float, Stride1D.Dense> Nwe = nwe;
        public ArrayView1D<float, Stride1D.Dense> Sae = sae;

        public static void Do(Index1D i, Work work)
        {
            if (i < 10)
            {
                work.Nwe[i] = 0;
                work.Sae[i] = 0;
                return;
            }

            var gaussShift = work.GaussLength - 1;

            var available = Math.Min(i + 1, work.Range);

            float num = 0;
            float den = 0;

            var to = Math.Min(available, work.GaussLength);

            for (var j = 0; j < to; j++)
            {
                var w = GetGauss(j, work.Gauss, gaussShift);
                num += work.Prices[i - j] * w;
                den += w;
            }

            work.Nwe[i] = num / den;
            work.Sae[i] = GetSae(i, work.Prices, work.Range, work.Gauss, work.GaussLength, gaussShift);
        }

        private static float GetGauss(int x, ArrayView<float> gauss, int gaussShift)
        {
            var index = x + gaussShift;
            return index < 0 || index >= gauss.Length ? 0 : gauss[index];
        }

        private static float GetSae(int c, ArrayView<float> prices, int range, ArrayView<float> gauss,
            int gaussLength, int gaussShift)
        {
            float sae = 0;

            var available = Math.Min(c + 1, range);
            for (var i = 0; i < available; i++)
            {
                float sum = 0;
                float sumw = 0;

                var from = Math.Max(i - gaussLength, 0);
                var to = Math.Min(i + gaussLength, available);
                for (var j = from; j < to; j++)
                {
                    var w = GetGauss(i - j, gauss, gaussShift);
                    sum += prices[c - j] * w;
                    sumw += w;
                }

                var y2 = sum / sumw;

                sae += Math.Abs(prices[c - i] - y2);
            }

            sae /= available;

            return sae;
        }
    }

    private float[] Gauss => Cache.Entry<float[]>("Gauss", CacheKind.Memory, [Bandwidth]).Calculate(CalculateGauss);

    private int GaussLength => (int)Math.Ceiling((float)Gauss.Length / 2);

    private float[] CalculateGauss()
    {
        List<float> forward = new();

        for (var i = 1; ; i++)
        {
            var g = GaussFormula(i);
            if (g == 0) break;
            forward.Add(g);
        }

        List<float> backward = [.. forward];
        backward.Reverse();

        List<float> result = new();
        result.AddRange(backward);
        result.Add(GaussFormula(0));
        result.AddRange(forward);

        return result.ToArray();
    }

    public required int Range { get; init; }

    private float GaussFormula(int x) => (float)Math.Exp(-x * x / (2 * Bandwidth * Bandwidth));

    public required double Bandwidth { get; init; }

    public required double Multiplier { get; init; }

    public required Direction Direction { get; init; }

    public required IList<double> Source { get; init; }
}