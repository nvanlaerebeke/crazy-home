using Home.Db;
using LanguageExt.Common;
using Microsoft.Extensions.DependencyInjection;
using MQTT.Actions.Actions;
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

    public async Task<Result<bool>> SetStateAsync(string id, SwitchState switchState) {
        try {
            return await _provider.GetRequiredService<SetState>().ExecuteAsync(id, switchState);
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

    public async Task<Result<bool>> SetPowerOnBehavior(string id, SwitchState switchState) {
        try {
            return await _provider.GetRequiredService<SetPowerOnBehavior>().ExecuteAsync(id, switchState);
        } catch (Exception ex) {
            return new Result<bool>(ex);
        }
    }
    
    public async Task<Result<bool>> SetAllowStateChange(string id, bool allowStateChange) {
        try {
            return await _provider.GetRequiredService<SetAllowChangeState>().ExecuteAsync(id, allowStateChange);
        } catch (Exception ex) {
            return new Result<bool>(ex);
        }
    }
}
