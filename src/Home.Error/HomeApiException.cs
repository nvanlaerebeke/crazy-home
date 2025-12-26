using System.Net;

namespace Home.Error;

public class HomeApiException : ApiException<ApiErrorCode> {
    private HomeApiException(IApiError<ApiErrorCode> error) : base(error) { }

    public static HomeApiException from(ApiErrorCode error,
        HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError) {
        return from(error, ApiErrorMessage.GetMessageForCode(error), httpStatusCode);
    }

    public static HomeApiException from(ApiErrorCode error, string message) {
        return from(error, message, HttpStatusCode.InternalServerError);
    }

    public static HomeApiException from(ApiErrorCode error, string message, HttpStatusCode httpStatusCode) {
        return new HomeApiException(new ApiError(error, httpStatusCode) { Message = message });
    }

    public static HomeApiException from(Exception ex) {
        if (ex is HomeApiException homeApiException) {
            return homeApiException;
        }
        return from(ApiErrorCode.UnknownError, ex.Message, HttpStatusCode.InternalServerError);
    }
}
