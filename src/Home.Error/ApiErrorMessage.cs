namespace Home.Error;

public static class ApiErrorMessage {
    private static readonly Dictionary<ApiErrorCode, string> Messages = new() {
        { ApiErrorCode.InvalidValue, "invalid value" },
        { ApiErrorCode.UnknownError, "An unknown error has occurred" },
        { ApiErrorCode.NotFound, "The item was not found" }
    };

    public static string GetMessageForCode(ApiErrorCode code) {
        return Messages.TryGetValue(code, out string? value) ? value : Messages[ApiErrorCode.UnknownError];
    }
}
