namespace dotnet.challenge.api.Errors
{
    public class UserErrors
    {
        public static readonly Error UserEmailIsAlreadyRegistered = new Error("User Email is already registered", 409);
        public static readonly Error UserNotFound = new Error("User does not exist", 404);
        public static readonly Error InvalidId = new Error("The provided Guid is not valid", 400);
        public static readonly Error InvalidPagination = new Error("Invalid pagination parameters", 400);
    }
}
