using System;
using System.Collections.Generic;

namespace ISIP423_Rezantsev;

public partial class Storage
{
    public int PartId { get; set; }

    public int Quantity { get; set; }

    public virtual Part Part { get; set; } = null!;
}
