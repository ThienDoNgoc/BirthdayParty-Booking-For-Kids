using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace BusinessObject
{
    public partial class Order
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public Guid Id { get; set; }
        public Guid? GuestId { get; set; }
        public Guid? HostId { get; set; }
        public int? TotalPrice { get; set; }
        public string Note { get; set; }
        public Guid? PlaceId { get; set; }
        public int? DeleteFlag { get; set; }
        [Required]
        public DateTime? Date { get; set; }
        public int? Status { get; set; }
        [Required]
        public DateTime? OrderDate { get; set; }

        public virtual Account Guest { get; set; }
        public virtual Account Host { get; set; }
        public virtual Place Place { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
