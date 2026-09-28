using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Delivery.Msv.Models
{
    public partial class DeliveryMsvDbContext:DbContext
    {
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
        {
            modelBuilder.AddTransactionalOutboxEntities();
        }
    }
} 