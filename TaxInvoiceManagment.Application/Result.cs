namespace TaxInvoiceManagment.Application
{
    public class Result<T>
    {
        public bool IsSuccess { get; }
        public T Value { get; }
        public List<string> Errors { get; }


        private Result(T value, bool isSuccess, List<string> errors)
        {
            Value = value;
            IsSuccess = isSuccess;
            Errors = errors;
        }


        public static Result<T> Success(T value) => new Result<T>(value, true, new List<string>());

        public static Result<T> Failure(List<string> errors) => new Result<T>(default!, false, errors);
    }
}
