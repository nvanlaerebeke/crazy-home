namespace MQTT.Actions.Objects.Power;

public class PowerReaderRecordDto {
    public required RecordValueDto? DeviceName { get; init; }
    public required RecordValueDto? DeviceId { get; init; }
    public RecordValueDto? DeviceType { get; init; }
    public RecordValueDto? ProtocolVersion { get; init; }
    public RecordValueDto? PowerBreakerState { get; init; }
    public RecordValueDto? PowerDeviceSerialNumber { get; init; }
    public RecordValueDto? L1PowerUsage { get; init; }
    public RecordValueDto? L2PowerUsage { get; init; }
    public RecordValueDto? L3PowerUsage { get; init; }
    public RecordValueDto? L1PowerGenerated { get; init; }
    public RecordValueDto? L2PowerGenerated { get; init; }
    public RecordValueDto? L3PowerGenerated { get; init; }
    public RecordValueDto? L1Amperage { get; init; }
    public RecordValueDto? L2Amperage { get; init; }
    public RecordValueDto? L3Amperage { get; init; }
    public RecordValueDto? L1Voltage { get; init; }
    public RecordValueDto? L2Voltage { get; init; }
    public RecordValueDto? L3Voltage { get; init; }
    public RecordValueDto? CurrentTariff { get; init; }
    public RecordValueDto? PowerGeneratedTariff1 { get; init; }
    public RecordValueDto? PowerGeneratedTariff2 { get; init; }
    public RecordValueDto? PowerUsedTariff1 { get; init; }
    public RecordValueDto? PowerUsedTariff2 { get; init; }
    public RecordValueDto? TotalPowerUsed { get; init; }
    public RecordValueDto? TotalPowerGenerated { get; init; }
    public RecordValueDto? MaxPhasePower { get; init; }
    public RecordValueDto? L1FuseThreshold { get; init; }
    public RecordValueDto? PowerFailureCount { get; init; }
    public RecordValueDto? PowerFailureLog { get; init; }
    public RecordValueDto? LongPowerFailureCount { get; init; }
    public RecordValueDto? GasDeviceId { get; init; }
    public RecordValueDto? GasDeviceSerialNumber { get; init; }
    public RecordValueDto? GasBreakerState { get; init; }
    public RecordValueDto? GasUsed { get; init; }
    public RecordValueDto? GasUsageTimestamp { get; init; }
    public RecordValueDto? DevicesOnBus { get; init; }
    public RecordValueDto? TimeStamp { get; init; }
    public RecordValueDto? TextMessage{ get; init; }
}
