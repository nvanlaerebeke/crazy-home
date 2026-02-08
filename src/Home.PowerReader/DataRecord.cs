using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Home.PowerReader;

internal class DataRecord {
    [JsonPropertyName("device_name")]
    public required RecordValue? DeviceName { get; init; }

    [JsonPropertyName("device_id")]
    public required RecordValue? DeviceId { get; init; }

    [JsonPropertyName("device_type")]
    public RecordValue? DeviceType { get; init; }

    [JsonPropertyName("protocol_version")]
    public RecordValue? ProtocolVersion { get; init; }

    [JsonPropertyName("power_breaker_state")]
    public RecordValue? PowerBreakerState { get; init; }

    [JsonPropertyName("power_device_serial_number")]
    public RecordValue? PowerDeviceSerialNumber { get; init; }

    [JsonPropertyName("l1_power_usage")]
    public RecordValue? L1PowerUsage { get; init; }

    [JsonPropertyName("l2_power_usage")]
    public RecordValue? L2PowerUsage { get; init; }

    [JsonPropertyName("l3_power_usage")]
    public RecordValue? L3PowerUsage { get; init; }

    [JsonPropertyName("l1_power_generated")]
    public RecordValue? L1PowerGenerated { get; init; }

    [JsonPropertyName("l2_power_generated")]
    public RecordValue? L2PowerGenerated { get; init; }

    [JsonPropertyName("l3_power_generated")]
    public RecordValue? L3PowerGenerated { get; init; }

    [JsonPropertyName("l1_amperage")]
    public RecordValue? L1Amperage { get; init; }

    [JsonPropertyName("l2_amperage")]
    public RecordValue? L2Amperage { get; init; }

    [JsonPropertyName("l3_amperage")]
    public RecordValue? L3Amperage { get; init; }

    [JsonPropertyName("l1_voltage")]
    public RecordValue? L1Voltage { get; init; }

    [JsonPropertyName("l2_voltage")]
    public RecordValue? L2Voltage { get; init; }

    [JsonPropertyName("l3_voltage")]
    public RecordValue? L3Voltage { get; init; }

    [JsonPropertyName("current_tariff")]
    public RecordValue? CurrentTariff { get; init; }

    [JsonPropertyName("power_generated_tariff_1")]
    public RecordValue? PowerGeneratedTariff1 { get; init; }

    [JsonPropertyName("power_generated_tariff_2")]
    public RecordValue? PowerGeneratedTariff2 { get; init; }

    [JsonPropertyName("power_used_tariff_1")]
    public RecordValue? PowerUsedTariff1 { get; init; }

    [JsonPropertyName("power_used_tariff_2")]
    public RecordValue? PowerUsedTariff2 { get; init; }

    [JsonPropertyName("total_power_used")]
    public RecordValue? TotalPowerUsed { get; init; }

    [JsonPropertyName("total_power_generated")]
    public RecordValue? TotalPowerGenerated { get; init; }

    [JsonPropertyName("max_phase_power")]
    public RecordValue? MaxPhasePower { get; init; }

    [JsonPropertyName("l1_fuse_threshold")]
    public RecordValue? L1FuseThreshold { get; init; }

    [JsonPropertyName("power_failure_count")]
    public RecordValue? PowerFailureCount { get; init; }

    [JsonPropertyName("power_failure_log")]
    public RecordValue? PowerFailureLog { get; init; }

    [JsonPropertyName("long_power_failure_count")]
    public RecordValue? LongPowerFailureCount { get; init; }

    [JsonPropertyName("gas_device_id")]
    public RecordValue? GasDeviceId { get; init; }

    [JsonPropertyName("gas_device_serial_number")]
    public RecordValue? GasDeviceSerialNumber { get; init; }

    [JsonPropertyName("gas_breaker_state")]
    public RecordValue? GasBreakerState { get; init; }

    [JsonPropertyName("gas_used")]
    public RecordValue? GasUsed { get; init; }

    [JsonPropertyName("gas_usage_timestamp")]
    public RecordValue? GasUsageTimestamp { get; init; }

    [JsonPropertyName("devices_on_bus")]
    public RecordValue? DevicesOnBus { get; init; }

    [JsonPropertyName("timestamp")]
    public RecordValue? TimeStamp { get; init; }

    [JsonPropertyName("text_message")]
    public RecordValue? TextMessage{ get; init; }
}
