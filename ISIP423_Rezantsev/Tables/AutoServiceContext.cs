using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ISIP423_Rezantsev;

public partial class AutoServiceContext : DbContext
{
    public AutoServiceContext()
    {
    }

    public AutoServiceContext(DbContextOptions<AutoServiceContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CustomerHistory> CustomerHistories { get; set; }

    public virtual DbSet<Part> Parts { get; set; }

    public virtual DbSet<PurchaseOrder> PurchaseOrders { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=LocalHost;Initial Catalog=AutoService;Integrated Security=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CustomerHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__customer__3213E83F9EC27D06");

            entity.ToTable("Customer_history");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Day).HasColumnName("day");
            entity.Property(e => e.Earnings)
                .HasColumnType("decimal(8, 2)")
                .HasColumnName("earnings");
            entity.Property(e => e.PartNeeded).HasColumnName("part_needed");
            entity.Property(e => e.RepairPrice)
                .HasColumnType("decimal(8, 2)")
                .HasColumnName("repair_price");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("status");

            entity.HasOne(d => d.PartNeededNavigation).WithMany(p => p.CustomerHistories)
                .HasForeignKey(d => d.PartNeeded)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__customer___part___52593CB8");
        });

        modelBuilder.Entity<Part>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__parts__3213E83F802C7FF6");

            entity.HasIndex(e => e.Name, "UQ__parts__72E12F1BBF68F8C3").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(8, 2)")
                .HasColumnName("price");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.RepairFee)
                .HasColumnType("decimal(8, 2)")
                .HasColumnName("repair_fee");
        });

        modelBuilder.Entity<PurchaseOrder>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__purchase__3213E83F7DC5119E");

            entity.ToTable("Purchase_orders");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Cost)
                .HasColumnType("decimal(8, 2)")
                .HasColumnName("cost");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DeliveryIn)
                .HasDefaultValue(2)
                .HasColumnName("delivery_in");
            entity.Property(e => e.PartId).HasColumnName("part_id");
            entity.Property(e => e.Quantity).HasColumnName("quantity");

            entity.HasOne(d => d.Part).WithMany(p => p.PurchaseOrders)
                .HasForeignKey(d => d.PartId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__purchase___part___4F7CD00D");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
