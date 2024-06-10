using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace BusinessObject
{
    public partial class Service
    {
        public Service()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Invalid price")]
        [Range(1, int.MaxValue, ErrorMessage = "Invalid price")]
        public int? Price { get; set; }
        public Guid? ServiceTypeId { get; set; }
        public Guid? HostId { get; set; }
        public int? DeleteFlag { get; set; }

        public virtual Account Host { get; set; }
        public virtual ServiceType ServiceType { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
