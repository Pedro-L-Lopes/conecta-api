namespace conecta_api.DTOs.AddressDTOs;
public class AddressSummaryDTO
{
    public string Neighborhood { get; set; } = string.Empty;

    public string City { get; set; } = string.Empty;

    public string State { get; set; } = string.Empty;

    public string? ZipCode { get; set; } = string.Empty;
}
