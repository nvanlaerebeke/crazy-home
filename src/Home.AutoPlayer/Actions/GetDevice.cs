using Home.AutoPlayer.Auth;
using Home.AutoPlayer.Dto;
using Home.Error;
using SpotifyAPI.Web;
using Device = Home.AutoPlayer.Dto.Device;

namespace Home.AutoPlayer.Actions;

internal sealed class GetDevice {
    private readonly SpotifyTokenStore _tokenStore;

    public GetDevice(SpotifyTokenStore tokenStore) {
        _tokenStore = tokenStore;
    }

    public async Task<Device?> ExecuteAsync(string name) {
        var token = await _tokenStore.GetValidAccessToken();
        if (string.IsNullOrEmpty(token)) {
            throw HomeApiException.from(ApiErrorCode.NotFound);
        }

        var client = new SpotifyClient(token);
        return (await client.Player.GetAvailableDevices()).Devices.FirstOrDefault(x => x.Name == name)?.ToDto();
    }
}
