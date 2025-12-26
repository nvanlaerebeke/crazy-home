using Home.AutoPlayer.Actions;
using Home.AutoPlayer.Dto;
using LanguageExt.Common;

namespace Home.AutoPlayer;

internal sealed class AudioActions : IAudioActions {
    private readonly GetDevice _getDevice;
    private readonly GetDevices _getDevices;
    private readonly StartPlayback _startPlayback;
    private readonly SetPlayList _setPlayList;
    private readonly GetPlayList _getPlayList;

    public AudioActions(
        GetDevice getDevice, 
        GetDevices getDevices,
        StartPlayback startPlayback,
        SetPlayList setPlayList,
        GetPlayList getPlayList
    ) {
        _getDevice = getDevice;
        _getDevices = getDevices;
        _startPlayback = startPlayback;
        _setPlayList = setPlayList;
        _getPlayList = getPlayList;
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

    public async Task<Result<bool>> SetPlayListAsync(string name) {
        try {
            await _setPlayList.ExecuteAsync(name);
            return true;
        } catch (Exception ex) {
            return new Result<bool>(ex);
        }
    }
    
    public async Task<Result<PlayList?>> GetPlayListAsync(string? name = null) {
        try {
            return await _getPlayList.ExecuteAsync(name);
        } catch (Exception ex) {
            return new Result<PlayList?>(ex);
        }
    }
}
