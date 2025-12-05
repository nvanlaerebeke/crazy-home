using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Home.Db.Model;

[Table("theme")]
public sealed class Theme : Entity, IHasLastUpdated{
    [Required(AllowEmptyStrings=false)]
    [Column("name")]
    public required string Name { get; init; } 
    
    [Required(AllowEmptyStrings = false)]
    [Column("primary")]
    [Length(6, 6)]
    public required string Primary { get; set; }
    
    [Required(AllowEmptyStrings = false)]
    [Column("secondary")]
    [Length(6, 6)]
    public required string Secondary { get; set; }
    
    [Required(AllowEmptyStrings = false)]
    [Column("tertiary")]
    [Length(6, 6)]
    public required string Tertiary { get; set; }

    [Column("background")]
    public byte[] Background { get; set; } = [];
    
    [Column("created")]
    public DateTime Created { get; init; } = DateTime.UtcNow;
    
    [Column("last_updated")]
    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
}
