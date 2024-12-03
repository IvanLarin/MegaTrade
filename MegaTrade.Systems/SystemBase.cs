using MegaTrade.Common;
using MegaTrade.Common.Extensions;
using MegaTrade.Common.Painting;
using System.ComponentModel;
using TSLab.Script;
using TSLab.Script.Handlers;

namespace MegaTrade.Systems;

[HandlerCategory("МегаСистемы")]
public abstract class SystemBase : IHandler, IContextUses
{
    protected void Setup(ISecurity security) => Security = security;

    protected void Run()
    {
        for (var i = Context.TradeFromBar; i < Context.BarsCount; i++)
        {
            Now = i;

            if (ShouldEnterLong)
                EnterLongAtMarket();

            if (ShouldEnterShort)
                EnterShortAtMarket();

            if (ShouldExitLong)
                ExitLongAtMarket();

            if (ShouldExitShort)
                ExitShortAtMarket();
        }

        if (!Context.IsOptimization)
            DoDraw();
    }

    protected int Now { get; private set; }

    protected virtual bool IsLongEnterSignal => false;

    protected virtual bool IsLongExitSignal => false;

    protected virtual bool IsShortEnterSignal => false;

    protected virtual bool IsShortExitSignal => false;

    protected virtual double GetLongEnterVolume =>
        Security.LotSize; //TODO каким количеством торговать, если счёт слился?

    protected virtual double GetLongExitVolume =>
        Security.LotSize; //TODO каким количеством торговать, если счёт слился?

    protected virtual double GetShortEnterVolume =>
        Security.LotSize; //TODO каким количеством торговать, если счёт слился?

    protected virtual double GetShortExitVolume =>
        Security.LotSize; //TODO каким количеством торговать, если счёт слился?

    private IPaint? _paint;

    protected IPaint Paint => _paint ??= new Paint(Context, Security.Symbol, new Palette(), true);

    private readonly IPalette _palette = new Palette();

    protected IPaint AddPaint(string name) => new Paint(Context, name, _palette);

    protected virtual bool IsBasicTimeframeDraw => true;

    private void DoDraw()
    {
        if (IsBasicTimeframeDraw)
            Paint.Candles(Security, Security.Symbol);

        Draw();

        Paint.Trades(Security);
    }

    protected virtual void Draw()
    {
    }

    private bool ShouldEnterLong =>
        IsLongTrade && LotsToLongEnter.IsMoreThan(0) && IsLongEnterSignal;

    private bool ShouldEnterShort =>
        IsShortTrade && LotsToShortEnter.IsMoreThan(0) && IsShortEnterSignal;

    private bool ShouldExitLong =>
        IsLongTrade && LotsToLongExit.IsMoreThan(0) && IsLongExitSignal;

    protected bool ShouldExitShort =>
        IsShortTrade && LotsToShortExit.IsMoreThan(0) && IsShortExitSignal;

    private void EnterLongAtMarket()
    {
        var lotsToBuy = LotsToLongEnter;
        var lotsInPosition = LotsInLongPosition;

        if (lotsInPosition.IsEqualTo(0))
            Security.Positions.BuyAtMarket(Now + 1, lotsToBuy, LongEnterName);
        else
            LongPosition?.ChangeAtMarket(Now + 1, lotsInPosition + lotsToBuy, LongIncreaseName);
    }

    private void ExitLongAtMarket()
    {
        var lotsToSell = LotsToLongExit;
        var lotsInPosition = LotsInLongPosition;

        if (lotsInPosition.IsEqualTo(lotsToSell))
            LongPosition?.CloseAtMarket(Now + 1, LongExitName);
        else
            LongPosition?.ChangeAtMarket(Now + 1, lotsInPosition - lotsToSell, LongDecreaseName);
    }

    private void EnterShortAtMarket()
    {
        var lotsToSell = LotsToShortEnter;
        var lotsInPosition = LotsInShortPosition;

        if (lotsInPosition.IsEqualTo(0))
            Security.Positions.SellAtMarket(Now + 1, lotsToSell, ShortEnterName);
        else
            ShortPosition?.ChangeAtMarket(Now + 1, lotsInPosition + lotsToSell, ShortIncreaseName);
    }

    private void ExitShortAtMarket()
    {
        var lotsToBuy = LotsToShortExit;
        var lotsInPosition = LotsInShortPosition;

        if (lotsInPosition.IsEqualTo(lotsToBuy))
            ShortPosition?.CloseAtMarket(Now + 1, ShortExitName);
        else
            ShortPosition?.ChangeAtMarket(Now + 1, lotsInPosition - lotsToBuy, ShortDecreaseName);
    }

    private double LotsToLongEnter => IsLongTrade ? ToLotsCount(GetLongEnterVolume) : 0;

    private double LotsToLongExit =>
        IsLongTrade ? Math.Min(ToLotsCount(GetLongExitVolume), LotsInLongPosition) : 0;

    private double LotsToShortEnter => IsShortTrade ? ToLotsCount(GetShortEnterVolume) : 0;

    private double LotsToShortExit =>
        IsShortTrade ? Math.Min(ToLotsCount(GetShortExitVolume), LotsInShortPosition) : 0;

    private double ToLotsCount(double volume) => Security.RoundShares(volume / Security.LotSize);

    protected bool NoLongPosition => NoActivePosition(LongEnterName);

    protected bool NoShortPosition => NoActivePosition(ShortEnterName);

    private bool NoActivePosition(string signalName) =>
        Security.Positions.GetLastActiveForSignal(signalName, Now) == null;

    private double LotsInLongPosition => IsLongTrade ? LongPosition?.Shares ?? 0 : 0;

    private double LotsInShortPosition => IsShortTrade ? ShortPosition?.Shares ?? 0 : 0;

    private IPosition? LongPosition =>
        Security.Positions.GetLastActiveForSignal(LongEnterName, Now);

    private IPosition? ShortPosition =>
        Security.Positions.GetLastActiveForSignal(ShortEnterName, Now);

    private IndicatorFactory? _indicatorFactory;

    protected IndicatorFactory IndicatorFactory => _indicatorFactory ??= new TheIndicatorFactory(Context);

    private IContext? _context;

    public IContext Context
    {
        get => _context!;
        set => _context = value;
    }

    private ISecurity? _security;

    protected ISecurity Security
    {
        get => _security ?? throw new Exception("Забыли вызвать метод Setup");
        private set => _security = value;
    }

    private const string LongEnterName = "LE";

    private const string ShortEnterName = "SE";

    private const string LongExitName = "LX";

    private const string ShortExitName = "SX";

    private const string LongIncreaseName = "L+";

    private const string LongDecreaseName = "L-";

    private const string ShortIncreaseName = "S+";

    private const string ShortDecreaseName = "S-";

    [Description("Открывать ли длинные позиции")]
    [HandlerParameter(true, "true")]
    public bool IsLongTrade { get; set; }

    [Description("Открывать ли короткие позиции")]
    [HandlerParameter(true, "true")]
    public bool IsShortTrade { get; set; }
}