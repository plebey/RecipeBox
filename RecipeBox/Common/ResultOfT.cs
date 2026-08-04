namespace RecipeBox.Common
{
    public class Result<T>
    {
        public bool IsSuccess { get; }
        public T? Value { get; }
        public ErrorType? ErrorType { get; }
        public string? ErrorMsg { get; }


        private Result(bool isSuccess, T? value, ErrorType? errorType, string? errorMsg)
        {
            IsSuccess = isSuccess;
            Value = value;
            ErrorType = errorType;
            ErrorMsg = errorMsg;
        }

        public static Result<T> Success(T value)
        {
            return new Result<T>(
                        isSuccess: true,
                        value: value,
                        errorType: null,
                        errorMsg: null);
        }

        public static Result<T> Failure(ErrorType errorType, string errorMsg)
        {
            return new Result<T>(
                        isSuccess: false,
                        value: default,
                        errorType: errorType,
                        errorMsg: errorMsg);
        }
    }
}
