using LanguageExt.Common;
using PlugwiseControl.Cache;
using PlugwiseControl.Message;
using PlugwiseControl.Message.Requests;
using PlugwiseControl.Message.Responses;

namespace PlugwiseControl.Actions;

internal sealed class On {
    private readonly RequestManager _requestManager;
    private readonly CircleInfoCache _circleInfoCache;

    public On(RequestManager requestManager, CircleInfoCache  circleInfoCache) {
        _requestManager = requestManager;
        _circleInfoCache = circleInfoCache;
    }

    public Result<SwitchOnResponse> Execute(string mac) {
        return _requestManager.Send<SwitchOnResponse>(new OnRequest(mac)).Match(
            response => {
                if (response.Status == Status.Success) {
                    _circleInfoCache.Invalidate(mac);
                }
                return response;
            },
            ex => new Result<SwitchOnResponse>(ex));
    }
}
