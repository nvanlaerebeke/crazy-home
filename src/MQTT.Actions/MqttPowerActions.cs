using System;
using System.Threading.Tasks;
using LanguageExt.Common;
using Microsoft.Extensions.Logging;
using MQTT.Actions.Objects.Power;

namespace MQTT.Actions;

internal class MqttPowerActions : IMqttPowerActions {
    private readonly MqttClient _client;
    private readonly ILogger<MqttPowerActions> _logger;

    public MqttPowerActions(
        MqttClient client,
        ILogger<MqttPowerActions> logger
    ) {
        _client = client;
        _logger = logger;
    }

    public async Task<Result<bool>> PublishPowerUpdateAsync(PowerReaderRecordDto data) {
        try {
            _logger.LogInformation("Sending power update to MQTT");
            // Base topics that DSMR Reader uses in "Split topic" mode.
            // If you configured another prefix in DSMR Reader, change it here.
            const string baseTopic = "dsmr/reading";
            
            await PublishIfPresent($"{baseTopic}/device_name", data.DeviceName?.Value);

            // 1) kWh totals (import/export, tariff 1/2)
            await PublishIfPresent($"{baseTopic}/electricity_delivered_1", data.PowerUsedTariff1.AsKwh());
            await PublishIfPresent($"{baseTopic}/electricity_delivered_2", data.PowerUsedTariff2.AsKwh());
            await PublishIfPresent($"{baseTopic}/electricity_returned_1", data.PowerGeneratedTariff1.AsKwh());
            await PublishIfPresent($"{baseTopic}/electricity_returned_2", data.PowerGeneratedTariff2.AsKwh());

            // 2) instantaneous power (kW)
            // These map to 1-0:1.7.0 and 1-0:2.7.0 equivalents
            await PublishIfPresent($"{baseTopic}/electricity_currently_delivered", data.TotalPowerUsed.AsKwFromWattOrKw());
            await PublishIfPresent($"{baseTopic}/electricity_currently_returned", data.TotalPowerGenerated.AsKwFromWattOrKw());

            // 3) per-phase instantaneous (kW) – publish only if present
            await PublishIfPresent($"{baseTopic}/phase_currently_delivered_l1", data.L1PowerUsage.AsKwFromWattOrKw());
            await PublishIfPresent($"{baseTopic}/phase_currently_delivered_l2", data.L2PowerUsage.AsKwFromWattOrKw());
            await PublishIfPresent($"{baseTopic}/phase_currently_delivered_l3", data.L3PowerUsage.AsKwFromWattOrKw());

            await PublishIfPresent($"{baseTopic}/phase_currently_returned_l1", data.L1PowerGenerated.AsKwFromWattOrKw());
            await PublishIfPresent($"{baseTopic}/phase_currently_returned_l2", data.L2PowerGenerated.AsKwFromWattOrKw());
            await PublishIfPresent($"{baseTopic}/phase_currently_returned_l3", data.L3PowerGenerated.AsKwFromWattOrKw());

            // 4) timestamp
            await PublishIfPresent($"{baseTopic}/timestamp", data.TimeStamp.AsTimestamp());

            // 5) gas (if you want DSMR Reader "extra device" style topics)
            // DSMR Reader typically uses extra_device_* topics under the same base.
            await PublishIfPresent("dsmr/consumption_gas_delivered", data.GasUsed.AsCubicMeters());

            // 6) “nice to have” stats (not required for Energy)
            // Tariff indicator, power failure counts, breaker, etc.
            await PublishIfPresent($"{baseTopic}/electricity_tariff", data.CurrentTariff?.Value);
            await PublishIfPresent($"{baseTopic}/power_failure_count", data.PowerFailureCount?.Value);
            await PublishIfPresent($"{baseTopic}/long_power_failure_count", data.LongPowerFailureCount?.Value);
            await PublishIfPresent($"{baseTopic}/electricity_switch_position", data.PowerBreakerState?.Value);

            return new Result<bool>(true);
        } catch (Exception ex) {
            return new Result<bool>(ex);
        }
    }

    private async Task PublishIfPresent(string topic, string? payload) {
        if (string.IsNullOrWhiteSpace(payload)) {
            return;
        }

        // DSMR Reader split topic payload is just a string numeric/timestamp/etc
        _logger.LogInformation("Sending to topic {Topic}: {Payload}", topic, payload);
        await _client.SendAsync(new DsmrMqttMessage(topic, payload), true);
    }
}
