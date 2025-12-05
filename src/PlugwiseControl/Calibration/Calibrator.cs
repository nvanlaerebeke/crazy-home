using System;
using System.Collections.Concurrent;
using PlugwiseControl.Message;
using PlugwiseControl.Message.Requests;
using PlugwiseControl.Message.Responses;

namespace PlugwiseControl.Calibration;

internal class Calibrator {
    private readonly ConcurrentDictionary<string, CalibrationResponse> _cache = new();
    private readonly IRequestManager _requestManager;

    public Calibrator(IRequestManager requestManager) {
        _requestManager = requestManager;
    }

    public double GetCorrected(int pulses, string mac) {
        var calibration = GetCalibration(mac);
        var corrected = 1 * (
            (Math.Pow(pulses + calibration.OffsetNoise, 2) * calibration.GainB) +
            ((pulses + calibration.OffsetNoise) * calibration.GainA) + calibration.OffsetTotal
        );
        return 1000 * ConversionClass.PulsesToKws(corrected);
    }

    private CalibrationResponse GetCalibration(string mac) {
        if (_cache.TryGetValue(mac, out var calibration)) {
            return calibration;
        }

        calibration = _requestManager.Send<CalibrationResponse>(new CalibrationRequest(mac))
            .Match(r => r, ex => throw ex);
        if (calibration.Status != Status.Success) {
            throw new Exception(calibration.Status.ToString());
        }

        _cache.TryAdd(mac, calibration);
        return calibration;
    }
}
