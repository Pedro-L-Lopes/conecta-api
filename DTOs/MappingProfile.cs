using AutoMapper;
using conecta_api.DTOs.AddressDTOs;
using conecta_api.DTOs.PropertyDTOs;
using conecta_api.Models;

namespace conecta_api.DTOs;
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Property
        CreateMap<Property, AddPropertyDTO>().ReverseMap();
        CreateMap<Property, PropertySummaryDTO>();
        CreateMap<Property, PropertyDTO>();

        // Address
        CreateMap<Address, AddressDTO>();
        CreateMap<Address, AddressSummaryDTO>();
    }
}
