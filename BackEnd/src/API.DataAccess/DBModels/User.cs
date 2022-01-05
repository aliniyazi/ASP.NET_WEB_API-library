using API.DataAccess.Contracts;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace API.DataAccess.Models
{
    public class User : IdentityUser, IAuditable, IDeletable
    {
        public User()
        {
            this.Books = new HashSet<Book>();
            this.Requests = new HashSet<Request>();
            this.CreatedOn = DateTime.UtcNow;
        }

        [Required]
        [Range(2, 40)]
        public string FirstName { get; set; }
        [Required]
        [Range(2, 40)]
        public string Lastname { get; set; }

        public bool UserApproved { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int AddressId { get; set; }
        public Address Address { get; set; }
        public ICollection<Book> Books { get; set; }
        public ICollection<Request> Requests { get; set; }
    }
}
