using API.DataAccess.Contracts;
using System;
using System.ComponentModel.DataAnnotations;

namespace API.DataAccess.Abstractions
{
    public abstract class BaseModel : IAuditable, IDeletable
    {
        [Required]
        public int Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }
    }
}
