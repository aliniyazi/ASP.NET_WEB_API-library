using API.DataAccess.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace API.DataAccess.Models
{
    public class Author : BaseModel
    {
        public string FirstName { get; set; }
        [Required]
        [Range(2, 40)]
        public string LastName { get; set; }
        public ICollection<Book> Books { get; set; } = new HashSet<Book>();
    }
}