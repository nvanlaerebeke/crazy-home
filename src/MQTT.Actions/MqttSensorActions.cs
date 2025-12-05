using LanguageExt.Common;
using Microsoft.Extensions.DependencyInjection;
using MQTT.Actions.Actions;
using MQTT.Actions.Actions.Sensor;
using MQTT.Actions.Objects;

namespace MQTT.Actions;

internal class MqttSensorActions : IMqttSensorActions {
    private readonly IServiceProvider _provider;

    public MqttSensorActions(IServiceProvider provider) {
        _provider = provider;
    }

    public async Task<Result<List<SensorDto>>> GetAllAsync() {
        try {
            return await _provider.GetRequiredService<GetAll>().ExecuteAsync();
        } catch (Exception ex) {
            return new Result<List<SensorDto>>(ex);
        }
    }

    public async Task<Result<SensorDto?>> GetSensorStatusAsync(string id) {
        try {
            return await _provider.GetRequiredService<GetSensorStatus>().ExecuteAsync(id);
        } catch (Exception ex) {
            return new Result<SensorDto?>(ex);
        }
    }

    public async Task<Result<double>> PermitJoiningAsync() {
        try {
            return await _provider.GetRequiredService<PermitJoining>().ExecuteAsync();
        } catch (Exception ex) {
            return new Result<double>(ex);
        }
    }

    public async Task<Result<bool>> DisableJoiningAsync() {
        try {
            await _provider.GetRequiredService<PermitJoining>().ExecuteAsync(TimeSpan.FromSeconds(0));
            return true;
        } catch (Exception ex) {
            return new Result<bool>(ex);
        }
    }
}
