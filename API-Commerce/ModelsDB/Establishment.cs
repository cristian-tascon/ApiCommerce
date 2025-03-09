using System;
using System.Collections.Generic;

namespace API_Commerce.ModelsDB;

public partial class Establishment
{
    public int EstId { get; set; }

    public string EstNombre { get; set; } = null!;

    public decimal EstMoneyIncome { get; set; }

    public int EstEmployees { get; set; }

    public int EstBusinessman { get; set; }

    public DateOnly? EstDateUpdate { get; set; }

    public string? EstUserUpdate { get; set; }

    public virtual Businessman EstBusinessmanNavigation { get; set; } = null!;
}
