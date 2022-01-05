using API.DataAccess.Abstractions;
using System;
using System.ComponentModel.DataAnnotations;

namespace API.DataAccess.Models
{
    public class Request : BaseModel
    {
        public string UserId { get; set; }
        public User User { get; set; }
        public int BookId { get; set; }
        public Book Book { get; set; }
        public DateTime? DateToReturnBook { get; set; }
        [Required]
        public bool? RequestApproved { get; set; }
    }
}
