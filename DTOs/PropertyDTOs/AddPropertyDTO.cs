using conecta_api.DTOs.AddressDTOs;
using System.ComponentModel.DataAnnotations;

namespace conecta_api.DTOs.PropertyDTOs
{
    public class AddPropertyDTO
    {
        [Required, MaxLength(255)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(2000)]
        public string? Description { get; set; } = string.Empty;

        [Required]
        public decimal Price { get; set; }

        public decimal? CondoFee { get; set; }

        public double? Area { get; set; }

        [MaxLength(10)]
        public string? AreaUnit { get; set; } = "m²";

        public int? Bedrooms { get; set; }
        public int? Suites { get; set; }
        public int? Bathrooms { get; set; }
        public int? ParkingSpaces { get; set; }

        [Required, MaxLength(50)]
        public string Type { get; set; } = string.Empty;

        [Required, MaxLength(20)]
        public string Purpose { get; set; } = string.Empty;

        [Required]
        public AddressDTO Address { get; set; } = new();

        public string? Photos { get; set; } = "[]";

        [Required, MaxLength(20)]
        public string Status { get; set; } = "Available";

        [Required]
        public Guid AdvertiserId { get; set; }
    }
}
