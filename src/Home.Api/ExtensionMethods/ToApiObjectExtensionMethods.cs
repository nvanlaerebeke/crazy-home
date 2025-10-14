using Home.Api.Objects.Plugwise;
using Home.Config;
using PlugwiseControl.Message.Responses;

namespace Home.Api.ExtensionMethods;

internal static class ToApiObjectExtensionMethods {
    public static StickStatus ToApiObject(this StickStatusResponse stickStatusResponse) {
        return new StickStatus {
            StickMac = stickStatusResponse.StickMac,
            IsCirclePlus = stickStatusResponse.IsCirclePlus
        };
    }
    
    public static CircleInfo ToApiObject(this CircleInfoResponse circleInfoResponse, Plug plug, double usage) {
        return new CircleInfo {
            CirclePlusMac = circleInfoResponse.CirclePlusMac,
            Date = circleInfoResponse.Date,
            Year = circleInfoResponse.Date.Year,
            Month = circleInfoResponse.Date.Month,
            Min = string.Empty,
            CurrentLog = circleInfoResponse.CurrentLog,
            State = circleInfoResponse.State,
            Hertz = circleInfoResponse.Hertz,
            HW1 = circleInfoResponse.HW1,
            HW2 = circleInfoResponse.HW2,
            HW3 = circleInfoResponse.HW3,
            Firmware = circleInfoResponse.Firmware,
            Type = circleInfoResponse.Type,
            Usage = usage,
            Name = plug.Name,
            Mac = plug.Mac,
            AllowStateUpdate = plug.PowerControl
        };
    }

    public static Calibration ToApiObject(this CalibrationResponse calibrationResponse) {
        return new Calibration {
            Mac = calibrationResponse.Mac,
            GainA = calibrationResponse.GainA,
            GainB = calibrationResponse.GainB,
            OffsetTotal = calibrationResponse.OffsetTotal,
            OffsetNoise = calibrationResponse.OffsetNoise
        };
    }
}
