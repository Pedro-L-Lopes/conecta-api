using System.ComponentModel.DataAnnotations;

namespace conecta_api.DTOs.AddressDTOs;
public class AddressDTO
{
    [MaxLength(255)]
    public string? Street { get; set; } = string.Empty;

    [MaxLength(25)]
    public string? Number { get; set; } = string.Empty;

    [Required, MaxLength(100)]
    public string Neighborhood { get; set; } = string.Empty;

    [Required, MaxLength(100)]
    public string City { get; set; } = string.Empty;

    [Required, MaxLength(2)]
    public string State { get; set; } = string.Empty;

    [MaxLength(15)]
    public string? ZipCode { get; set; } = string.Empty;

    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
}
