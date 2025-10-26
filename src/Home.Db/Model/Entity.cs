using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Home.Db.Model;

public abstract class Entity {
    [Key]
    [Column("id")]
    //The Guid.NewGuid() can be called without extra overhead as EFCore bypasses this when getting the object from db
    public string Id { get; set; } = Guid.NewGuid().ToString();
}

