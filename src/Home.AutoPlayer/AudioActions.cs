using Home.AutoPlayer.Actions;
using Home.AutoPlayer.Dto;
using LanguageExt.Common;

namespace Home.AutoPlayer;

internal sealed class AudioActions : IAudioActions {
    private readonly GetDevice _getDevice;
    private readonly GetDevices _getDevices;
    private readonly StartPlayback _startPlayback;

    public AudioActions(
        GetDevice getDevice, 
        GetDevices getDevices,
        StartPlayback startPlayback
    ) {
        _getDevice = getDevice;
        _getDevices = getDevices;
        _startPlayback = startPlayback;
    }
    public async Task<Result<List<Device>>> GetDevicesAsync() {
        try {
            return await _getDevices.ExecuteAsync();
        } catch (Exception ex) {
            return new Result<List<Device>>(ex);
        }
    }

    public async Task<Result<Device?>> GetDeviceAsync(string name) {
        try {
            return await _getDevice.ExecuteAsync(name);
        } catch (Exception ex) {
            return new Result<Device?>(ex);
        }
    }

    public async Task<Result<bool>> StartPlayBackAsync(string deviceName) {
        try {
            return await _startPlayback.ExecuteAsync(deviceName);
        } catch (Exception ex) {
            return new Result<bool>(ex);
        }
    }
}

