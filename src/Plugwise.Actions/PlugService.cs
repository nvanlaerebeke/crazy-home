using LanguageExt.Common;
using PlugwiseControl;
using PlugwiseControl.Message;
using PlugwiseControl.Message.Responses;

namespace Plugwise.Actions;

internal class PlugService : IPlugService {
    private readonly IPlugControl _plugControl;

    public PlugService(IPlugControl plugControl) {
        _plugControl = plugControl;
    }

    public Result<bool> On(string mac) {
        try {
            return _plugControl.On(mac).Match(
                r => r.Status == Status.Success,
                ex => new Result<bool>(ex));
        } catch (Exception ex) {
            return new Result<bool>(ex);
        }
    }

    public Result<bool> Off(string mac) {
        try {
            return _plugControl.Off(mac).Match(
                r => r.Status == Status.Success,
                ex => new Result<bool>(ex)
            );
        } catch (Exception ex) {
            return new Result<bool>(ex);
        }
    }

    public Result<CalibrationResponse> Calibrate(string mac) {
        try {
            return _plugControl.Calibrate(mac);
        } catch (Exception ex) {
            return new Result<CalibrationResponse>(ex);
        }
    }

    public Result<double> Usage(string mac) {
        try {
            return _plugControl.GetUsage(mac);
        } catch (Exception ex) {
            return new Result<double>(ex);
        }
    }

    public Result<StickStatusResponse> Initialize() {
        try {
            return _plugControl.Initialize();
        } catch (Exception ex) {
            return new Result<StickStatusResponse>(ex);
        }
    }

    public Result<CircleInfoResponse> CircleInfo(string mac) {
        try {
            return _plugControl.CircleInfo(mac);
        } catch (Exception ex) {
            return new Result<CircleInfoResponse>(ex);
        }
    }

    public Result<ResultResponse> SetDateTime(string mac, long unixDStamp) {
        try {
            return _plugControl.SetDateTime(mac, unixDStamp);
        } catch (Exception ex) {
            return new Result<ResultResponse>(ex);
        }
    }
}
