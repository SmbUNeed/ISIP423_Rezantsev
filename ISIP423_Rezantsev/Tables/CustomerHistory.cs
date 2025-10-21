using System;
using System.Collections.Generic;

namespace ISIP423_Rezantsev;

public partial class CustomerHistory
{
    public int Id { get; set; }

    public int Day { get; set; }

    public int PartNeeded { get; set; }

    public decimal RepairPrice { get; set; }

    public string Status { get; set; } = null!;

    public decimal Earnings { get; set; }

    public virtual Part PartNeededNavigation { get; set; } = null!;
}
