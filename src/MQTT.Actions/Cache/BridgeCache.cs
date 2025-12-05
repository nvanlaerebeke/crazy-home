using MQTT.Actions.Message.Receive.Bridge;

namespace MQTT.Actions.Cache;

internal sealed class BridgeCache {
    private BridgeState? _bridgeState;
    private BridgeInfo? _bridgeInfo;
    
    public void SetState(BridgeState state) {
        _bridgeState = state;
    }

    public void SetInfo(BridgeInfo bridgeInfo) {
        _bridgeInfo = bridgeInfo;
    }
}

