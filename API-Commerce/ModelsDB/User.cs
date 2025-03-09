using System;
using System.Collections.Generic;

namespace API_Commerce.ModelsDB;

public partial class User
{
    public int UseId { get; set; }

    public string UseFirstName { get; set; } = null!;

    public string? UseMiddleName { get; set; }

    public string UseFirstLastName { get; set; } = null!;

    public string? UseMiddleLastName { get; set; }

    public string UseEmail { get; set; } = null!;

    public string UsePassword { get; set; } = null!;

    public int UseRol { get; set; }

    public virtual Rol UseRolNavigation { get; set; } = null!;
}
