using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Home.Db.Model;

[Table("device")]
public sealed class Device : Entity{
    [Required(AllowEmptyStrings=false)]
    [Column("device_type")]
    public required DeviceType DeviceType { get; set; } 
    
    [Required(AllowEmptyStrings = false)]
    [Column("ieee_address")]
    [MaxLength(32)]
    public string IeeeAddress { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = false)]
    [Column("friendly_name")]
    [MaxLength(30)]
    public string FriendlyName { get; set; } = string.Empty;
    
    [Required(AllowEmptyStrings = false)]
    [Column("power_on_behavior")]
    [MaxLength(3)]
    public SwitchState? PowerOnBehavior { get; set; } = SwitchState.On;

    [Required]
    [Column("allow_switch_state")]
    public bool AllowStateChange { get; set; } = true;
}
