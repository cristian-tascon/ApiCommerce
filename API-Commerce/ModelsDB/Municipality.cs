using System;
using System.Collections.Generic;

namespace API_Commerce.ModelsDB;

public partial class Municipality
{
    public string MunId { get; set; } = null!;

    public string MunNombre { get; set; } = null!;

    public virtual ICollection<Businessman> Businessmen { get; set; } = new List<Businessman>();
}
