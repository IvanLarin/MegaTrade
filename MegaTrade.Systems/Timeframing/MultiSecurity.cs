using TSLab.DataSource;
using TSLab.Script;
using TSLab.Script.Handlers;
using TSLab.Utils;

namespace MegaTrade.Systems.Timeframing;

public class MultiSecurity : IMultiSecurity
{
    public MultiSecurity(ISecurity[] securities)
    {
        All = securities;
        _security = All[0];
    }

    private readonly ISecurity _security;

    public ISecurity[] All { get; }

    public void Dispose()
    {
        All.ForEach(x => x.Dispose());
    }

    public void Attach()
    {
        _security.Attach();
    }

    public IDisposable Lock() => _security.Lock();

    public DisposeState DisposeState => _security.DisposeState;

    public bool IsDisposed => _security.IsDisposed;

    public bool IsDisposedOrDisposing => _security.IsDisposedOrDisposing;

    public IReadOnlyList<IQueueData> GetBuyQueue(int barNum) => _security.GetBuyQueue(barNum);

    public IReadOnlyList<IQueueData> GetSellQueue(int barNum) => _security.GetSellQueue(barNum);

    public IReadOnlyList<ITrade> GetTrades(int barNum) => _security.GetTrades(barNum);

    public IReadOnlyList<ITrade> GetTrades(int firstBarIndex, int lastBarIndex) =>
        _security.GetTrades(firstBarIndex, lastBarIndex);

    public int GetTradesCount(int firstBarIndex, int lastBarIndex) =>
        _security.GetTradesCount(firstBarIndex, lastBarIndex);

    public IReadOnlyList<IReadOnlyList<ITrade>> GetTradesPerBar(int firstBarIndex, int lastBarIndex) =>
        _security.GetTradesPerBar(firstBarIndex, lastBarIndex);

    public ISecurity CompressTo(int interval) => _security.CompressTo(interval);

    public ISecurity CompressTo(Interval interval) => _security.CompressTo(interval);

    public ISecurity CompressTo(Interval interval, int shift) => _security.CompressTo(interval, shift);

    public ISecurity CompressTo(Interval interval, int shift, int adjustment, int adjShift) =>
        _security.CompressTo(interval, shift, adjustment, adjShift);

    public ISecurity CompressToVolume(Interval interval) => _security.CompressToVolume(interval);

    public ISecurity CompressToPriceRange(Interval interval) => _security.CompressToPriceRange(interval);

    public IList<double> Decompress(IList<double> candles) => _security.Decompress(candles);

    public IList<TK> Decompress<TK>(IList<TK> candles, DecompressMethodWithDef method) where TK : struct =>
        _security.Decompress(candles, method);

    public void ConnectSecurityList(IGraphListBase iList)
    {
        _security.ConnectSecurityList(iList);
    }

    public void ConnectDoubleList(IGraphListBase list, IDoubleHandlerWithUpdate handler)
    {
        _security.ConnectDoubleList(list, handler);
    }

    public double RoundPrice(double price) => _security.RoundPrice(price);

    public double RoundShares(double shares) => _security.RoundShares(shares);

    public ISecurity CloneAndReplaceBars(IEnumerable<IDataBar> newCandles) => _security.CloneAndReplaceBars(newCandles);

    public ISecurity CloneAndReplaceBars(IReadOnlyList<IDataBar> newCandles) =>
        _security.CloneAndReplaceBars(newCandles);

    public void UpdateQueueData()
    {
        _security.UpdateQueueData();
    }

    public string Symbol => _security.Symbol;

    public IDataSourceSecurity SecurityDescription => _security.SecurityDescription;

    public FinInfo FinInfo => _security.FinInfo;

    public IReadOnlyList<IDataBar> Bars => _security.Bars;

    public bool IsBarsLoaded => _security.IsBarsLoaded;

    public IList<double> OpenPrices => _security.OpenPrices;

    public IList<double> ClosePrices => _security.ClosePrices;

    public IList<double> HighPrices => _security.HighPrices;

    public IList<double> LowPrices => _security.LowPrices;

    public IList<double> Volumes => _security.Volumes;

    public Interval IntervalInstance => _security.IntervalInstance;

    public int Interval => _security.Interval;

    public DataIntervals IntervalBase => _security.IntervalBase;

    public DateTime SessionBegin => _security.SessionBegin;

    public DateTime SessionEnd => _security.SessionEnd;

    public double LotSize => _security.LotSize;

    public double LotTick => _security.LotTick;

    public double Margin => _security.Margin;

    public double Tick => _security.Tick;

    public int Decimals => _security.Decimals;

    public IPositionsList Positions => _security.Positions;

    public CommissionDelegate Commission
    {
        get => _security.Commission;
        set => _security.Commission = value;
    }

    public string CacheName => _security.CacheName;

    public double InitDeposit
    {
        get => _security.InitDeposit;
        set => _security.InitDeposit = value;
    }

    public bool IsRealtime => _security.IsRealtime;

    public bool IsPortfolioReady => _security.IsPortfolioReady;

    public bool SimulatePositionOrdering => _security.SimulatePositionOrdering;

    public bool IsAligned => _security.IsAligned;
}