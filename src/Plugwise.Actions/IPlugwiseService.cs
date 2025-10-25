using LanguageExt.Common;
using PlugwiseControl.Message.Responses;

namespace Plugwise.Actions;

public interface IPlugwiseService {
    Result<StickStatusResponse> Initialize();
    Result<bool> On(string mac);
    Result<bool> Off(string mac);
    Result<CalibrationResponse> Calibrate(string mac);
    Result<double> Usage(string mac);
    Result<CircleInfoResponse> CircleInfo(string mac);
    Result<ResultResponse> SetDateTime(string mac, long unixDStamp);
}
