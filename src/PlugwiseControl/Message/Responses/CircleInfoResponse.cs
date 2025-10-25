using System;
using Home.Shared;

namespace PlugwiseControl.Message.Responses;

public class CircleInfoResponse : Response {
    public override string Code => Responses[2][..4];
    public override string Sequence => Responses[2][4..8];
    public override string Crc16 => Responses[2][..^4];

    public string CirclePlusMac => Responses[2][8..24];

    public DateTime Date => new DateTime(
            Convert.ToInt32(Responses[2][24..26], 16) + 2000,
            Convert.ToInt32(Responses[2][26..28], 16),
            1)
        .AddMinutes(
            Convert.ToInt32(Responses[2][28..32], 16)
        );

    public string CurrentLog => Responses.Count >= 40 ? Responses[2][32..40] : string.Empty;
    public SwitchState State => Responses[2][40..42] == "00" ? SwitchState.Off : SwitchState.On;
    public string Hertz => Responses.Count >= 44 ? Responses[2][42..44] : string.Empty;

    public string HW1 => Responses.Count >= 48 ? Responses[2][44..48] : String.Empty;
    public string HW2 => Responses.Count >= 52 ? Responses[2][48..52] : string.Empty;
    public string HW3 => Responses.Count >= 56 ? Responses[2][52..56] : string.Empty;

    // => d/m/y
    public string Firmware => Responses.Count >= 64 ? Responses[2][56..64] : string.Empty;

    public string Type {
        get {
            return Responses.Count >= 66 ? Responses[3][64..66] : string.Empty;
        }
    }

    public override bool IsComplete() {
        return
            Responses.Count.Equals(3) &&
            Responses[2].Length.Equals(70) &&
            Responses[2].StartsWith("0024");
    }

    public override string ToString() {
        try {
            return $"Code: {Code}, Sequence: {Sequence}, State: {State}";
        } catch (Exception) {
            return string.Empty;
        }
    }
}
