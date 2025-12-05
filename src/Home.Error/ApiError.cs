using System.Net;
using System.Text.Json.Serialization;

namespace Home.Error;

public class ApiError : IApiError<ApiErrorCode> {
    public ApiError() : this(ApiErrorCode.UnknownError) { }

    public ApiError(ApiErrorCode error, HttpStatusCode httpStatus = HttpStatusCode.InternalServerError) {
        HttpStatusCode = httpStatus;
        Code = error;
        Message = ApiErrorMessage.GetMessageForCode(error);
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public HttpStatusCode HttpStatusCode { get; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ApiErrorCode Code { get; set; }

    public string Message { get; set; }
}
