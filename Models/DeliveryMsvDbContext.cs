using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;


namespace Delivery.Msv.Models
{

    public partial class DeliveryMsvDbContext : DbContext
    {
        public DeliveryMsvDbContext()
        {
        }

        public DeliveryMsvDbContext(DbContextOptions<DeliveryMsvDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TrxDelivery> TrxDeliveries { get; set; }    

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql("Name=ConnectionStrings:DeliveryMsvDBConnection");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TrxDelivery>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("trx_delivery_pkey");

                entity.ToTable("trx_delivery");

                entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
                entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
                entity.Property(e => e.CustomerName)
                .HasMaxLength(255)
                .HasColumnName("customer_name");
                entity.Property(e => e.Orderid).HasColumnName("orderid");
                entity.Property(e => e.ShippingAddress).HasColumnName("shipping_address");
                entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValueSql("'Pending'::character varying")
                .HasColumnName("status");
                entity.Property(e => e.TrackingNumber)
                .HasMaxLength(100)
                .HasColumnName("tracking_number");
            });

            this.OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
