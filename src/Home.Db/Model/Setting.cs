using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Home.Db.Model;

[Table("setting")]
public sealed class Setting : Entity{
    [Required(AllowEmptyStrings=false)]
    [Column("key")]
    public required string Key { get; set; }
    
    [Required(AllowEmptyStrings=true)]
    [Column("value")]
    public required string Value { get; set; }
}

