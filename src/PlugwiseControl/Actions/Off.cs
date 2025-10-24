using LanguageExt.Common;
using PlugwiseControl.Cache;
using PlugwiseControl.Message;
using PlugwiseControl.Message.Requests;
using PlugwiseControl.Message.Responses;

namespace PlugwiseControl.Actions;

internal sealed class Off {
    private readonly IRequestManager _requestManager;
    private readonly CircleInfoCache _circleInfoCache;

    public Off(IRequestManager requestManager, CircleInfoCache circleInfoCache) {
        _requestManager = requestManager;
        _circleInfoCache = circleInfoCache;
    }

    public Result<SwitchOffResponse> Execute(string mac) {
        return _requestManager.Send<SwitchOffResponse>(new OffRequest(mac)).Match(
            response => {
                if (response.Status == Status.Success) {
                    _circleInfoCache.Invalidate(mac);
                }

                return response;
            },
            ex => new Result<SwitchOffResponse>(ex));
    }
}
