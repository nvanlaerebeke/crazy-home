using Home.AutoPlayer.Actions;
using Home.AutoPlayer.Dto;
using LanguageExt.Common;
using SpotifyAPI.Web;
using Device = Home.AutoPlayer.Dto.Device;

namespace Home.AutoPlayer;

internal sealed class AudioActions : IAudioActions {
    private readonly GetDevice _getDevice;
    private readonly GetDevices _getDevices;
    private readonly StartPlayback _startPlayback;
    private readonly SetPlayList _setPlayList;
    private readonly GetPlayList _getPlayList;
    private readonly GetCover _getCover;
    private readonly GetUserPlaylists _getUserPlaylists;
    private readonly GetLastPlayedPlaylist _getLastPlayedPlaylist;
    private readonly PlayLikedSongs _playLikedSongs;

    public AudioActions(
        GetDevice getDevice, 
        GetDevices getDevices,
        StartPlayback startPlayback,
        SetPlayList setPlayList,
        GetPlayList getPlayList,
        GetCover getCover,
        GetUserPlaylists getUserPlaylists,
        GetLastPlayedPlaylist getLastPlayedPlaylist,
        PlayLikedSongs playLikedSongs
    ) {
        _getDevice = getDevice;
        _getDevices = getDevices;
        _startPlayback = startPlayback;
        _setPlayList = setPlayList;
        _getPlayList = getPlayList;
        _getCover = getCover;
        _getUserPlaylists = getUserPlaylists;
        _getLastPlayedPlaylist = getLastPlayedPlaylist;
        _playLikedSongs = playLikedSongs;
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

    public async Task<Result<Uri>> GetCoverUrlAsync(string id) {
        try {
            return await _getCover.ExecuteAsync(id);
        } catch (Exception ex) {
            return new Result<Uri>(ex);
        }
    }

    public async Task<Result<List<PlayList>>> GetCurrentUserPlaylistsAsync() {
        try {
            return await _getUserPlaylists.GetAsync();
        } catch (Exception ex) {
            return new Result<List<PlayList>>(ex);
        }
    }
    
    public async Task<Result<FullPlaylist?>> GetLastPlayedPlaylistsAsync() {
        try {
            return await _getLastPlayedPlaylist.GetAsync();
        } catch (Exception ex) {
            return new Result<FullPlaylist?>(ex);
        }
    }
    
    public async Task<Result<bool>> PlayLikedSongsAsync() {
        try {
            return await _playLikedSongs.PlayAsync();
        } catch (Exception ex) {
            return new Result<bool>(ex);
        }
    }
}
