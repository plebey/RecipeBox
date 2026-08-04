namespace RecipeBox.Common
{
    public class Result
    {
        public bool IsSuccess {  get; }
        public ErrorType? ErrorType { get; }
        public string? ErrorMsg { get; }

        private Result (bool isSuccess, ErrorType? errorType, string? errorMsg)
        {
            IsSuccess = isSuccess;
            ErrorType = errorType;
            ErrorMsg = errorMsg;
        }

        public static Result Success()
        {
            return new Result(
                        isSuccess: true,
                        errorType: null,
                        errorMsg: null);
        }

        public static Result Failure(ErrorType errorType, string errorMsg)
        {
            return new Result(
                        isSuccess: false,
                        errorType: errorType,
                        errorMsg:  errorMsg);
        }
    }
}
