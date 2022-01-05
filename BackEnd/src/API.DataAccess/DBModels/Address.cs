using API.DataAccess.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace API.DataAccess.Models
{
    public class Address : BaseModel
    {
        [Required]
        public string Country { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string StreetName { get; set; }
        [Required]
        public string StreetNumber { get; set; }
        [MaxLength(20)]
        public string BuildingNumber { get; set; }
        [MaxLength(20)]
        public string AppartmentNumber { get; set; }
        [MaxLength(200)]
        public string AdditionalInfo { get; set; }
        public ICollection<User> Users { get; set; } = new HashSet<User>();
    }
}