using System;
using System.Collections.Generic;


namespace Delivery.Msv.Models
{

    public partial class TrxDelivery
    {
        public Guid Id { get; set; }

        public Guid Orderid { get; set; }

        public string? CustomerName { get; set; }

        public string ShippingAddress { get; set; } = null!;

        public string? TrackingNumber { get; set; }

        public string Status { get; set; } = null!;

        public DateTime CreatedAt { get; set; }
    }
}