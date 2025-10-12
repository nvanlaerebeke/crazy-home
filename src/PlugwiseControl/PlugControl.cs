using LanguageExt.Common;
using PlugwiseControl.Actions;
using PlugwiseControl.Cache;
using PlugwiseControl.Message.Requests;
using PlugwiseControl.Message.Responses;

namespace PlugwiseControl;

internal class PlugControl : IPlugControl {
    private readonly PlugwiseActions _actions;
    private readonly RequestManager _requestManager;
    private readonly UsageCache _usageCache;
    private readonly CircleInfoCache _circleInfoCache;

    public PlugControl(
        PlugwiseActions  actions,
        RequestManager requestManager, 
        UsageCache usageCache, 
        CircleInfoCache circleInfoCache
    ) {
        _actions = actions;
        _requestManager = requestManager;
        _usageCache = usageCache;
        _circleInfoCache = circleInfoCache;
    }

    public Result<StickStatusResponse> Initialize() {
        return _requestManager.Send<StickStatusResponse>(new InitializeRequest());
    }

    public Result<SwitchOnResponse> On(string mac) {
        return _actions.On(mac);
    }

    public Result<SwitchOffResponse> Off(string mac) {
        return _actions.Off(mac);
    }

    public Result<CalibrationResponse> Calibrate(string mac) {
        return _requestManager.Send<CalibrationResponse>(new CalibrationRequest(mac));
    }

    public Result<double> GetUsage(string mac) {
        return _usageCache.Get(mac);
    }

    public Result<CircleInfoResponse> CircleInfo(string mac) {
        return _circleInfoCache.Get(mac);
    }

    public Result<ResultResponse> SetDateTime(string mac, long unixDStamp) {
        return _requestManager.Send<ResultResponse>(new SetDateTimeRequest(mac, unixDStamp));
    }
}
