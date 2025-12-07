using Home.Db;

namespace MQTT.Actions.Objects;

public sealed class PlugStatusDto {
    public required string Id { get; init; }
    public required string Name { get; init; }

    public required SwitchState SwitchState { get; init; }

    public required double Usage { get; init; }

    public required double Current { get; init; }
    public required double Voltage { get; init; }
    public required bool AllowStateChange { get; init; }
    public required SwitchState PowerOnBehavior { get; init; }

    public double PowerFactor {
        get {
            if (Current == 0 || Voltage == 0) {
                return 1;
            }

            return Math.Round(Usage / (Current * Voltage), 2);
        }
    }
}
