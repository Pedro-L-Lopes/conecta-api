using conecta_api.DTOs.PropertyDTOs;
using conecta_api.Models;
using conecta_api.Pagination;

namespace conecta_api.Services.Interfaces;
public interface IPropertyService
{
    Task<Property> AddProperty(AddPropertyDTO addPropertyDTO);
    Task<PagedList<Property>> GetAvailableProperties(PropertyParameters parameters);
}
