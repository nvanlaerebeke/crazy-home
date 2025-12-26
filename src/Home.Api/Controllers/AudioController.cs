using Home.Api.Auth;
using Home.Api.ExtensionMethods;
using Home.Api.Objects.Audio;
using Home.AutoPlayer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Home.Api.Objects.Audio.ExtensionMethods;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace Home.Api.Controllers;

[ApiController]
[Route("[controller]")]
public sealed class AudioController : ControllerBase {
    private readonly ISpotifyActions _spotifyActions;
    private readonly IAudioActions _audioActions;

    public AudioController(ISpotifyActions spotifyActions, IAudioActions audioActions) {
        _spotifyActions = spotifyActions;
        _audioActions = audioActions;
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpGet("spotify/login")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Uri))]
    public IActionResult Login() {
        var uri = _spotifyActions.GetLoginUri();
        return uri.ToOk(x => x);
    }

    [HttpGet("spotify/login/callback")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> LoginCallback() {
        var result = await _spotifyActions.HandleLoginCallbackAsync(Request.Query["code"].ToString());
        return result.ToOk(_ => new EmptyResult());
    }

    [HttpGet("spotify/device")]
    [ApiKeyAuthorize]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<AudioDevice>))]
    public async Task<IActionResult> GetAll() {
        var result = await _audioActions.GetDevicesAsync();
        return result.ToOk(x => x.Select(y => y.ToApiObject()));
    }

    [HttpGet("spotify/device/{name}")]
    [ApiKeyAuthorize]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AudioDevice))]
    public async Task<IActionResult> Get(string name) {
        var result = await _audioActions.GetDeviceAsync(name);
        if (result.IsFaulted) {
            return result.ToOk(x => x.ToApiObject());
        }

        return result.Match(x => x, _ => null) is null
            ? NotFound()
            : result.ToOk(x => x.ToApiObject());
    }

    [HttpPut("spotify/device/{name}")]
    [ApiKeyAuthorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> SetDevice(string name) {
        var result = await _audioActions.StartPlayBackAsync(name);
        return result.ToOk(_ => new EmptyResult());
    }

    [HttpGet("spotify/playlist")]
    [ApiKeyAuthorize]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PlayList))]
    public async Task<IActionResult> GetPlayList() {
        var result = await _audioActions.GetPlayListAsync();
        if (result.IsFaulted) {
            return result.ToOk(_ => new EmptyResult());
        }
        
        return result.Match(x => x, _ => null) is null 
            ? NotFound() 
            : result.ToOk(x => x.ToApiObject());
    }
    
    [HttpGet("spotify/playlist/{name}")]
    [ApiKeyAuthorize]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PlayList))]
    public async Task<IActionResult> GetPlayListByName(string name) {
        var result = await _audioActions.GetPlayListAsync(name);
        if (result.IsFaulted) {
            return result.ToOk(_ => new EmptyResult());
        }
        
        return result.Match(x => x, _ => null) is null 
            ? NotFound() 
            : result.ToOk(x => x.ToApiObject());
    }
    
    [HttpPut("spotify/playlist/{name}")]
    [ApiKeyAuthorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> SetPlayList(string name) {
        var result = await _audioActions.SetPlayListAsync(name);
        return result.ToOk(_ => new EmptyResult());
    }
}
