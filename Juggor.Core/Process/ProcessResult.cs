namespace Juggor.Core.Process;

public class ProcessResult<TSuccess, TError>
{
    private TSuccess? successValue;
    private TError? errorValue;
    private bool isSucceeded;

    public ProcessResult(TSuccess? successValue)
    {
        this.successValue = successValue;
        this.isSucceeded = true;
    }

    public ProcessResult(TError? errorValue)
    {
        this.errorValue = errorValue;
    }

    public TSuccess SuccessValue
    {
        get
        {
            if (!IsSucceeded)
            {
                throw new InvalidOperationException("Can't get success value because result is error.");
            }

            return successValue!;
        }
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

    public static ProcessResult<TSuccess, TError> Success(TSuccess successValue)
    {
        return new ProcessResult<TSuccess, TError>(successValue);
    }

    public static ProcessResult<TSuccess, TError> Error(TError errorValue)
    {
        return new ProcessResult<TSuccess, TError>(errorValue);
    }
}
