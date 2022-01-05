using API.DataAccess.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace API.DataAccess.Models
{
    public class Comment : BaseModel
    {
        [Required]
        [MaxLength(150)]
        public string Text { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public int BookId { get; set; }
        public Book Book { get; set; }
    }
}
