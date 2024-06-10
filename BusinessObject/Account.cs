using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace BusinessObject
{
    public partial class Account
    {
        public Account()
        {
            Orders = new HashSet<Order>();
            Places = new HashSet<Place>();
            Services = new HashSet<Service>();
        }

        public Guid Id { get; set; }
        [Required]
        [RegularExpression(@"^[\w-]+(\.[\w-]+)*@([\w-]+\.)+[a-zA-Z]{2,7}$", ErrorMessage = "Invalid email format")]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public string Name { get; set; }
        [Required]
        [RegularExpression(@"^0\d{9}$", ErrorMessage = "Telephone number must be 10 digits and start with 0")]
        public string Phone { get; set; }
        public int? Role { get; set; }
        public int? DeleteFlag { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Place> Places { get; set; }
        public virtual ICollection<Service> Services { get; set; }
    }
}
