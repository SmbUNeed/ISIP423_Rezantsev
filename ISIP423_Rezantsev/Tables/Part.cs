using System;
using System.Collections.Generic;

namespace ISIP423_Rezantsev;

public partial class Part
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public decimal Price { get; set; }

    public decimal RepairFee { get; set; }

    public int Quantity { get; set; }

    public virtual ICollection<CustomerHistory> CustomerHistories { get; set; } = new List<CustomerHistory>();

    public virtual ICollection<PurchaseOrder> PurchaseOrders { get; set; } = new List<PurchaseOrder>();
}
