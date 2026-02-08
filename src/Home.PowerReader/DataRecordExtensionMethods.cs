using System.ComponentModel;
using MQTT.Actions.Objects.Power;

namespace Home.PowerReader;

internal static class DataRecordExtensionMethods {
    public static PowerReaderRecordDto ToDto(this DataRecord dataRecord) {
        return new PowerReaderRecordDto {
            DeviceName = dataRecord.DeviceName.ToDto(),
            DeviceId = dataRecord.DeviceId.ToDto(),
            DeviceType = dataRecord.DeviceType.ToDto(),
            ProtocolVersion = dataRecord.ProtocolVersion.ToDto(),
            PowerBreakerState = dataRecord.PowerBreakerState.ToDto(),
            PowerDeviceSerialNumber = dataRecord.PowerDeviceSerialNumber.ToDto(),
            L1PowerUsage = dataRecord.L1PowerUsage.ToDto(),
            L2PowerUsage = dataRecord.L2PowerUsage.ToDto(),
            L3PowerUsage = dataRecord.L3PowerUsage.ToDto(),
            L1PowerGenerated = dataRecord.L1PowerGenerated.ToDto(),
            L2PowerGenerated = dataRecord.L2PowerGenerated.ToDto(),
            L3PowerGenerated = dataRecord.L3PowerGenerated.ToDto(),
            L1Amperage = dataRecord.L1Amperage.ToDto(),
            L2Amperage = dataRecord.L2Amperage.ToDto(),
            L3Amperage = dataRecord.L3PowerUsage.ToDto(),
            L1Voltage = dataRecord.L1Voltage.ToDto(),
            L2Voltage = dataRecord.L2Voltage.ToDto(),
            L3Voltage = dataRecord.L3Voltage.ToDto(),
            CurrentTariff = dataRecord.CurrentTariff.ToDto(),
            PowerGeneratedTariff1 = dataRecord.PowerGeneratedTariff1.ToDto(),
            PowerGeneratedTariff2 = dataRecord.PowerGeneratedTariff2.ToDto(),
            PowerUsedTariff1 = dataRecord.PowerUsedTariff1.ToDto(),
            PowerUsedTariff2 = dataRecord.PowerUsedTariff2.ToDto(),
            TotalPowerUsed = dataRecord.TotalPowerUsed.ToDto(),
            TotalPowerGenerated = dataRecord.TotalPowerGenerated.ToDto(),
            MaxPhasePower = dataRecord.MaxPhasePower.ToDto(),
            L1FuseThreshold = dataRecord.L1FuseThreshold.ToDto(),
            PowerFailureCount = dataRecord.PowerFailureCount.ToDto(),
            PowerFailureLog = dataRecord.PowerFailureLog.ToDto(),
            LongPowerFailureCount = dataRecord.LongPowerFailureCount.ToDto(),
            GasDeviceId = dataRecord.GasDeviceId.ToDto(),
            GasDeviceSerialNumber = dataRecord.GasDeviceSerialNumber.ToDto(),
            GasBreakerState = dataRecord.GasBreakerState.ToDto(),
            GasUsed = dataRecord.GasUsed.ToDto(),
            GasUsageTimestamp = dataRecord.GasUsageTimestamp.ToDto(),
            DevicesOnBus = dataRecord.DevicesOnBus.ToDto(),
            TimeStamp = dataRecord.TimeStamp.ToDto(),
            TextMessage = dataRecord.TextMessage.ToDto()
        };
    }

    private static RecordValueDto? ToDto(this RecordValue? record) {
        if (record is null) {
            return null;
        }
        return new() {
            Unit = record.Unit.ToString(),
            Value = record.Value
        };
    }
}
