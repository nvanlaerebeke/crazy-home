using Home.AutoPlayer.Actions.ZeroConf;
using Home.AutoPlayer.Auth;
using Home.AutoPlayer.Dto;
using Home.Error;
using SpotifyAPI.Web;
using DeviceDto = Home.AutoPlayer.Dto.Device;

namespace Home.AutoPlayer.Actions;

internal sealed class GetDevices {
    private readonly SpotifyTokenStore _tokenStore;

    public GetDevices(SpotifyTokenStore tokenStore) {
        _tokenStore = tokenStore;
    }

    public async Task<List<DeviceDto>> ExecuteAsync() {
        var deviceDtoList = new List<DeviceDto>();
        await GetRegisteredDevicesAsync(deviceDtoList);
        //await GetLanDevicesAsync(deviceDtoList);
        return deviceDtoList;
    }

    private async Task GetRegisteredDevicesAsync(List<DeviceDto> deviceDtoList) {
        var token = await _tokenStore.GetValidAccessToken();
        if (string.IsNullOrEmpty(token)) {
            throw HomeApiException.from(ApiErrorCode.NotFound);
        }

        var client = new SpotifyClient(token);
        var allDevices = await client.Player.GetAvailableDevices();
        if (allDevices.Devices.Count > 0) {
            deviceDtoList.AddRange(allDevices.Devices.Select(d => d.ToDto()));
        }
    }

    private static async Task GetLanDevicesAsync(List<DeviceDto> deviceDtoList) {
        var lanDevices = await SpotifyConnectLanDiscovery.DiscoverAsync(TimeSpan.FromSeconds(10));
        if (lanDevices.Count > 0) {
            //Only add the device if it's not yet in the known device list
            //This means that spotify does not yet know that the LAN device exists
            foreach (var lanDevice in lanDevices) {
                if (deviceDtoList.Any(d =>
                        d.Id.Equals(lanDevice.DeviceId, StringComparison.InvariantCultureIgnoreCase)
                    )) {
                    continue;
                }

                deviceDtoList.Add(lanDevice.ToDto());
            }
        }
    }
}
