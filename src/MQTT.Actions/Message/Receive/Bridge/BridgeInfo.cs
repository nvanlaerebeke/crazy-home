using System.Text.Json;
using System.Text.Json.Serialization;

namespace MQTT.Actions.Message.Receive.Bridge;

internal sealed class BridgeInfo {
    [JsonPropertyName("version")]
    public string Version { get; init; } = "";

    [JsonPropertyName("commit")]
    public string Commit { get; init; } = "";

    [JsonPropertyName("coordinator")]
    public CoordinatorInfo Coordinator { get; init; } = new();

    [JsonPropertyName("zigbee_herdsman_converters")]
    public ModuleVersion ZigbeeHerdsmanConverters { get; init; } = new();

    [JsonPropertyName("zigbee_herdsman")]
    public ModuleVersion ZigbeeHerdsman { get; init; } = new();

    [JsonPropertyName("network")]
    public NetworkInfo Network { get; init; } = new();

    [JsonPropertyName("log_level")]
    public string LogLevel { get; init; } = "";

    [JsonPropertyName("permit_join")]
    public bool PermitJoin { get; init; }

    // Epoch seconds; null when permit join is disabled
    [JsonPropertyName("permit_join_end")]
    public long? PermitJoinEnd { get; init; }

    // Raw blobs so you keep everything (except network_key, which Z2M omits)
    [JsonPropertyName("config")]
    public JsonElement Config { get; init; }

    [JsonPropertyName("config_schema")]
    public JsonElement ConfigSchema { get; init; }

    [JsonPropertyName("restart_required")]
    public bool RestartRequired { get; init; }

    [JsonPropertyName("os")]
    public OsInfo Os { get; init; } = new();

    [JsonPropertyName("mqtt")]
    public MqttInfo Mqtt { get; init; } = new();
}
