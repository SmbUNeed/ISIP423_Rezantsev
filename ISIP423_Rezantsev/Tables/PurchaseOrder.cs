using System;
using System.Collections.Generic;

namespace ISIP423_Rezantsev;

public partial class PurchaseOrder
{
    public int Id { get; set; }

    public int PartId { get; set; }

    public int Quantity { get; set; }

    public decimal Cost { get; set; }

    public int DeliveryIn { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Part Part { get; set; } = null!;
}
