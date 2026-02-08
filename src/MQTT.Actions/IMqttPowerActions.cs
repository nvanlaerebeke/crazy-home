using LanguageExt.Common;
using MQTT.Actions.Objects.Power;

namespace MQTT.Actions;

public interface IMqttPowerActions {
    Task<Result<bool>> PublishPowerUpdateAsync(PowerReaderRecordDto data);
}
