using System;
using System.Collections.Generic;

namespace API_Commerce.ModelsDB;

public partial class BusinessmanStatus
{
    public string BstId { get; set; } = null!;

    public string BstDescription { get; set; } = null!;

    public virtual ICollection<Businessman> Businessmen { get; set; } = new List<Businessman>();
}
