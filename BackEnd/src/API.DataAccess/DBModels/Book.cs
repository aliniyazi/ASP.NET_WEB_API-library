using API.DataAccess.Abstractions;
using API.DataAccess.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace API.DataAccess.Models
{
    public class Book : BaseModel
    {
        public Book()
        {
            this.Requests = new HashSet<Request>();
        }

        [Required]
        [Range(2, 40)]
        public string Title { get; set; }
        [Required]
        [MaxLength(300)]
        public string Description { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public Genres Genre { get; set; }
        [Required]
        public Status Status { get; set; }
        public string ImageName { get; set; }
        public DateTime? DateTaken { get; set; }
        public DateTime? DateReturned { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public int AuthorId { get; set; }
        public Author Author { get; set; }
        public ICollection<Request> Requests { get; set;}
    }
}
