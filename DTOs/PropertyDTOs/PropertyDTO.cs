using conecta_api.DTOs.AddressDTOs;
using System.ComponentModel.DataAnnotations;

namespace conecta_api.DTOs.PropertyDTOs;
public class PropertyDTO
{
    public Guid Id { get; set; }
    public string Title { get; set; }

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public decimal? CondoFee { get; set; }

    public double? Area { get; set; }

    public string? AreaUnit { get; set; }

    public int? Bedrooms { get; set; }
    public int? Suites { get; set; }
    public int? Bathrooms { get; set; }
    public int? ParkingSpaces { get; set; }

    public string Type { get; set; }

    public string Purpose { get; set; }

    public AddressSummaryDTO Address { get; set; }

    public string? Photos { get; set; }

    public string Status { get; set; }
}
