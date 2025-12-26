using Home.AutoPlayer.Dto;
using LanguageExt.Common;

namespace Home.AutoPlayer;

public interface IAudioActions {
    Task<Result<List<Device>>> GetDevicesAsync();
    Task<Result<Device?>> GetDeviceAsync(string name);
    Task<Result<bool>> StartPlayBackAsync(string deviceName);
}
