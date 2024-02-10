namespace Juggor.Core.Process;

public class ProcessVoidResult<TError>
{
    private readonly TError? errorValue;
    private readonly bool isSucceeded;

    private ProcessVoidResult()
    {
        this.isSucceeded = true;
    }

    private ProcessVoidResult(TError? errorValue)
    {
        this.errorValue = errorValue;
    }

    public TError ErrorValue
    {
        get
        {
            if (!IsError)
            {
                throw new InvalidOperationException("Can't get error value because result is succeeded.");
            }

            return errorValue!;
        }
    }

    public bool IsSucceeded => isSucceeded;

    public bool IsError => !IsSucceeded;

    public static ProcessVoidResult<TError> Success()
    {
        return new ProcessVoidResult<TError>();
    }

    public static ProcessVoidResult<TError> Error(TError errorValue)
    {
        return new ProcessVoidResult<TError>(errorValue);
    }
}
