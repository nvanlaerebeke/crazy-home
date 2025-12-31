using Home.AutoPlayer.Actions.ZeroConf;
using Home.AutoPlayer.Auth;
using Home.AutoPlayer.Dto;
using Home.Error;
using SpotifyAPI.Web;
using DeviceDto = Home.AutoPlayer.Dto.Device;

namespace Home.AutoPlayer.Actions;

internal sealed class GetDevice {
    private readonly SpotifyTokenStore _tokenStore;
    private readonly GetDevices _getDevices;

    public GetDevice(SpotifyTokenStore tokenStore, GetDevices getDevices) {
        _tokenStore = tokenStore;
        _getDevices = getDevices;
    }

    public async Task<DeviceDto?> ExecuteAsync(string name) {
        var device = (await GetRegisteredDevicesAsync())
            .FirstOrDefault(d => d.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));

        return device ?? (await GetLanDevicesAsync())
            .FirstOrDefault(d => d.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
    }

    private async Task<List<DeviceDto>> GetRegisteredDevicesAsync() {
        var token = await _tokenStore.GetValidAccessToken();
        if (string.IsNullOrEmpty(token)) {
            throw HomeApiException.from(ApiErrorCode.NotFound);
        }

        var client = new SpotifyClient(token);
        var allDevices = await client.Player.GetAvailableDevices();
        return allDevices.Devices.Select(x => x.ToDto()).ToList();
    }

    private static async Task<List<DeviceDto>> GetLanDevicesAsync() {
        return [];
        var lanDevices = await SpotifyConnectLanDiscovery.DiscoverAsync(TimeSpan.FromSeconds(10));
        return lanDevices.Select(d => d.ToDto()).ToList();
    }
}
