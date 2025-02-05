using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace conecta_api.Models;
public class Property
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required, MaxLength(255)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(2000)]
    public string? Description { get; set; } = string.Empty;

    [Required]
    public decimal Price { get; set; }

    public decimal? CondoFee { get; set; }

    public double? Area { get; set; }

    [MaxLength(10)]
    public string? AreaUnit { get; set; } = "m²"; // Default: square meters

    public int? Bedrooms { get; set; }
    public int? Suites { get; set; }
    public int? Bathrooms { get; set; }
    public int? ParkingSpaces { get; set; }

    [Required, MaxLength(50)]
    public string Type { get; set; } = string.Empty; // Ex: "house", "apartment", "land"

    [Required, MaxLength(20)]
    public string Purpose { get; set; } = string.Empty; // Ex: "sale", "rent"

    [Required]
    public Address Address { get; set; } = new();

    [Column(TypeName = "json")]
    public string? Photos { get; set; } = "[]";

    [Required, MaxLength(20)]
    public string Status { get; set; } = "Available"; // Ex: "available", "sold", "rented"

    [Required]
    public Guid AdvertiserId { get; set; } // FK for user/advertiser table

    public int Views { get; set; } = 0;
    public int favorites { get; set; } = 0;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; }
}
