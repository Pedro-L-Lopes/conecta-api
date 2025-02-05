using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace conecta_api.Models;
public class Address
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [MaxLength(255)]
    public string? Street { get; set; } = string.Empty;

    [MaxLength(25)]
    public string? Number { get; set; } = string.Empty; // Number and complement

    [Required, MaxLength(100)]
    public string Neighborhood { get; set; } = string.Empty;

    [Required, MaxLength(100)]
    public string City { get; set; } = string.Empty;

    [Required, MaxLength(2)]
    public string State { get; set; } = string.Empty; // Ex: "NY", "CA"

    [MaxLength(15)]
    public string? ZipCode { get; set; } = string.Empty;

    [Column(TypeName = "decimal(9,6)")]
    public decimal? Latitude { get; set; }

    [Column(TypeName = "decimal(9,6)")]
    public decimal? Longitude { get; set; }
}
