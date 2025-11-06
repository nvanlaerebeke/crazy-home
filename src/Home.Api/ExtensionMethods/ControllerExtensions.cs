using LanguageExt.Common;
using Microsoft.AspNetCore.Mvc;
using Home.Error;

namespace Home.Api.ExtensionMethods;

public static class ControllerExtensions {
    public static IActionResult ToOk<TResult, TContract>(
        this Result<TResult> result, Func<TResult, TContract> mapper
    ) {
        return result.Match<IActionResult>(
            obj => new OkObjectResult(mapper(obj)),
            ToError
        );
    }

    public static IActionResult ToError(Exception exception) {
        if (exception is not HomeApiException) {
            if (exception is UnauthorizedAccessException) {
                return new UnauthorizedResult();
            }
            throw exception;
        }

        var error = (exception as HomeApiException)!.GetError();
        switch (error.Code) {
            case ApiErrorCode.InvalidValue:
                return new BadRequestObjectResult(exception);
            case ApiErrorCode.NotFound:
                return new NotFoundResult();
            case ApiErrorCode.Exists:
                return new ConflictResult();
            default:
                return new JsonResult(error) { StatusCode = (int)error.HttpStatusCode };
        }
    }
}
