namespace MegaTrade.Common.Calculating;

public abstract class Calculator<T>
{
    public T Result
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

    protected abstract T Calculate();
}