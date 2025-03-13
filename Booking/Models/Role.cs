using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

public class Role
{
    [Key]
    public int Id { get; set; }

    [Required, MaxLength(50)]
    public string Name { get; set; }

    public List<User> Users { get; set; } = new List<User>();
}
