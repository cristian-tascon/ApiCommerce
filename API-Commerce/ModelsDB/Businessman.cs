using System;
using System.Collections.Generic;

namespace API_Commerce.ModelsDB;

public partial class Businessman
{
    public int BusId { get; set; }

    public string BusName { get; set; } = null!;

    public string? BusPhoneNumber { get; set; }

    public string? BusEmail { get; set; }

    public DateOnly BusDateRegistration { get; set; }

    public DateOnly? BusDateUpdate { get; set; }

    public string BusStatus { get; set; } = null!;

    public string BusMunicipality { get; set; } = null!;

    public string? BusUserUpdate { get; set; }

    public virtual Municipality BusMunicipalityNavigation { get; set; } = null!;

    public virtual BusinessmanStatus BusStatusNavigation { get; set; } = null!;

    public virtual ICollection<Establishment> Establishments { get; set; } = new List<Establishment>();
}
