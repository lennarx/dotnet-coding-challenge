namespace dotnet.challenge.api
{
    public sealed class Error
    {
        public readonly string Message;
        public readonly int Code;
        public Error(string message, int code)
        {
            Message = message;
            Code = code;
        }
    }
}
