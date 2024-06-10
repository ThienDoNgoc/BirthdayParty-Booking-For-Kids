using System;
using System.Collections.Generic;

#nullable disable

namespace BusinessObject
{
    public partial class OrderDetail
    {
        public Guid Id { get; set; }
        public int? Number { get; set; }
        public int? Price { get; set; }
        public Guid? ServiceId { get; set; }
        public Guid? OrderId { get; set; }

        public virtual Order Order { get; set; }
        public virtual Service Service { get; set; }
    }
}
