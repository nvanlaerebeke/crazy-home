using LanguageExt.Common;
using Microsoft.Extensions.DependencyInjection;
using MQTT.Actions.Actions.Plug;
using MQTT.Actions.Objects;

namespace MQTT.Actions;

internal class MqttPlugActions : IMqttPlugActions {
    private readonly IServiceProvider _provider;

    public MqttPlugActions(IServiceProvider provider) {
        _provider = provider;
    }
    
    public async Task<Result<List<PlugStatusDto>>> GetAllAsync() {
        try {
            return await _provider.GetRequiredService<GetAll>().ExecuteAsync();
        } catch (Exception ex) {
            return new Result<List<PlugStatusDto>>(ex);
        }
    }

    public async Task<Result<bool>> SetOnAsync(string id) {
        try {
            return await _provider.GetRequiredService<On>().ExecuteAsync(id);
        } catch (Exception ex) {
            return new Result<bool>(ex);
        }
    }

    public async Task<Result<bool>> SetOffAsync(string id) {
        try {
            return await _provider.GetRequiredService<Off>().ExecuteAsync();
        } catch (Exception ex) {
            return new Result<bool>(ex);
        }
    }

    public async Task<Result<PlugStatusDto?>> GetPlugInfoAsync(string identifier) {
        try {
            return await _provider.GetRequiredService<GetPlugStatus>().ExecuteAsync(identifier);
        } catch (Exception ex) {
            return new Result<PlugStatusDto?>(ex);
        }
    }
}
