namespace MegaTrade.Common.Calculating;

public abstract class Calculator<TResult>
{
    public TResult Result
    {
        get
        {
            CheckInput();
            return Calculate();
        }
    }

    protected virtual void CheckInput()
    {
    }

    protected abstract TResult Calculate();
}