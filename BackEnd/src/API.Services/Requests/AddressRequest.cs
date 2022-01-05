using System.ComponentModel.DataAnnotations;

namespace API.Services.Requests
{
    public class AddressRequest
    {
        [Required]
        [MaxLength(60)]
        [MinLength(2)]
        public string Country { get; set; }
        [Required]
        [MaxLength(60)]
        [MinLength(2)]
        public string City { get; set; }
        [Required]
        [MaxLength(60)]
        [MinLength(2)]
        public string StreetName { get; set; }
        [Required]
        [MaxLength(60)]
        [MinLength(1)]
        public string StreetNumber { get; set; }
        [MaxLength(20)]
        public string BuildingNumber { get; set; }
        [MaxLength(20)]
        public string AppartmentNumber { get; set; }
        [MaxLength(200)]
        public string AdditionalInfo { get; set; }
    }
}
