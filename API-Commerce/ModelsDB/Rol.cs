using System;
using System.Collections.Generic;

namespace API_Commerce.ModelsDB;

public partial class Rol
{
    public int RolId { get; set; }

    public string RolDescription { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
