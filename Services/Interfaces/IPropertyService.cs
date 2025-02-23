using conecta_api.DTOs.PropertyDTOs;
using conecta_api.Models;
using conecta_api.Pagination;

namespace conecta_api.Services.Interfaces;
public interface IPropertyService
{
    Task<Property> AddProperty(AddPropertyDTO addPropertyDTO);
    Task<List<PhotoDTO>> AddPhoto(string propertyId, List<string> img);
    Task<PagedList<PropertySummaryDTO>> GetAvailableProperties(PropertyParameters parameters);
    Task<PropertyDTO> GetPropertyById(string Id);
    Task<PagedList<PropertySummaryDTO>> GetFavoriteProperties(PropertyParameters parameters, List<Guid> propertiesIds);
}
