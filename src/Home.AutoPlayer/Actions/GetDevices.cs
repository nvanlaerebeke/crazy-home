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
        var token = await _tokenStore.GetValidAccessToken();
        return string.IsNullOrEmpty(token) 
            ? throw HomeApiException.from(ApiErrorCode.NotFound) 
            : (await new SpotifyClient(token).Player.GetAvailableDevices()).Devices.Select(x => x.ToDto()).ToList();
    }
}
