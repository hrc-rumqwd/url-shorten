namespace UrlShorten.Models.Commons
{
    public class Result
    {
        public bool IsSuccess { get; set; }
        public string Error { get; set; }
    }

    public class Result<T> : Result
    {
        public T Data { get; set; }

        public static Result<T> Success(T data)
            => new Result<T>
            {
                IsSuccess = true,
                Data = data
            };

        public static Result<T> Failure(string error)
            => new Result<T>
            {
                IsSuccess = false,
                Error = error
            };
    }
}
