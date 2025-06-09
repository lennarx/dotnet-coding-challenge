using System;

namespace dotnet.challenge.api
{
    public class Result<TValue, TError> where TValue : class where TError : class
    {
        public readonly TValue Value;
        public readonly TError Error;

        private bool _isSuccess;

        private Result(TValue value)
        {
            _isSuccess = true;
            Value = value;
            Error = default(TError);
        }

        private Result(TError error)
        {
            _isSuccess = false;
            Value = default(TValue);
            Error = error;
        }

        public static implicit operator Result<TValue, TError>(TValue value) => new Result<TValue, TError>(value);

        public static implicit operator Result<TValue, TError>(TError error) => new Result<TValue, TError>(error);
        public static Result<TValue, TError> Success(TValue value) => new Result<TValue, TError>(value);
        public static Result<TValue, TError> Failure(TError value) => new Result<TValue, TError>(value);

        public Result<TValue, TError> Match(Func<TValue, Result<TValue, TError>> success, Func<TError, Result<TValue, TError>> failure)
        {
            if (_isSuccess)
            {
                if (Value == null) throw new InvalidOperationException("Value is null on success.");
                return success(Value);
            }
            if (Error == null) throw new InvalidOperationException("Error is null on failure.");
            return failure(Error);
        }
    }
}
