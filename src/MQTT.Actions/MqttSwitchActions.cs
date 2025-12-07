using Home.Db;
using LanguageExt.Common;
using Microsoft.Extensions.DependencyInjection;
using MQTT.Actions.Actions.Switch;
using MQTT.Actions.Objects;

namespace MQTT.Actions;

internal class MqttSwitchActions : IMqttSwitchActions {
    private readonly IServiceProvider _provider;

    public MqttSwitchActions(IServiceProvider provider) {
        _provider = provider;
    }
    
    public async Task<Result<List<SwitchStatusDto>>> GetAllAsync() {
        try {
            return await _provider.GetRequiredService<GetAll>().ExecuteAsync();
        } catch (Exception ex) {
            return new Result<List<SwitchStatusDto>>(ex);
        }
    }

    public async Task<Result<bool>> SetStateAsync(string id, SwitchState switchState) {
        try {
            return await _provider.GetRequiredService<SetState>().ExecuteAsync(id, switchState);
        } catch (Exception ex) {
            return new Result<bool>(ex);
        }
    }

    public async Task<Result<SwitchStatusDto?>> GetSwitchInfoAsync(string identifier) {
        try {
            return await _provider.GetRequiredService<GetSwitchStatus>().ExecuteAsync(identifier);
        } catch (Exception ex) {
            return new Result<SwitchStatusDto?>(ex);
        }
    }
}
