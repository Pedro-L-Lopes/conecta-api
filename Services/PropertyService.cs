using conecta_api.context;
using conecta_api.DTOs.PropertyDTOs;
using conecta_api.Models;
using conecta_api.Pagination;
using conecta_api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace conecta_api.Services;
public class PropertyService : IPropertyService
{
    private readonly AppDbContext _context;
    private readonly IUnityOfWork _uof;

    public PropertyService(AppDbContext context, IUnityOfWork uof)
    {
        _context = context;
        _uof = uof;
    }

    public async Task<Property> AddProperty(AddPropertyDTO addPropertyDTO)
    {
        var property = new Property
        {
            Title = addPropertyDTO.Title,
            Description = addPropertyDTO.Description,
            Price = addPropertyDTO.Price,
            CondoFee = addPropertyDTO.CondoFee,
            Area = addPropertyDTO.Area,
            AreaUnit = addPropertyDTO.AreaUnit,
            Bedrooms = addPropertyDTO.Bedrooms,
            Suites = addPropertyDTO.Suites,
            Bathrooms = addPropertyDTO.Bathrooms,
            ParkingSpaces = addPropertyDTO.ParkingSpaces,
            Type = addPropertyDTO.Type,
            Purpose = addPropertyDTO.Purpose,
            Address = new Address
            {
                Street = addPropertyDTO.Address.Street,
                Number = addPropertyDTO.Address.Number,
                Neighborhood = addPropertyDTO.Address.Neighborhood,
                City = addPropertyDTO.Address.City,
                State = addPropertyDTO.Address.State,
                ZipCode = addPropertyDTO.Address.ZipCode,
                Latitude = addPropertyDTO.Address.Latitude,
                Longitude = addPropertyDTO.Address.Longitude
            },
            Photos = addPropertyDTO.Photos,
            Status = addPropertyDTO.Status,
            AdvertiserId = addPropertyDTO.AdvertiserId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Properties.Add(property);

        await _context.SaveChangesAsync();

        return property;
    }

    public async Task<PagedList<Property>> GetAvailableProperties(PropertyParameters parameters)
    {
        // Base query: imóveis com status "Disponível"
        var query = _context.Properties
            .Include(p => p.Address) // Inclui o endereço
            .Where(p => p.Status == "Disponível")
            .AsQueryable();

        // Filtros
        if (!string.IsNullOrEmpty(parameters.City))
        {
            query = query.Where(p => p.Address.City == parameters.City);
        }

        if (!string.IsNullOrEmpty(parameters.Neighborhood))
        {
            query = query.Where(p => p.Address.Neighborhood == parameters.Neighborhood);
        }

        if (parameters.Bedrooms.HasValue)
        {
            query = query.Where(p => p.Bedrooms == parameters.Bedrooms);
        }

        if (parameters.ParkingSpaces.HasValue)
        {
            query = query.Where(p => p.ParkingSpaces == parameters.ParkingSpaces);
        }

        if (parameters.MinPrice.HasValue)
        {
            query = query.Where(p => p.Price >= parameters.MinPrice);
        }

        if (parameters.MaxPrice.HasValue)
        {
            query = query.Where(p => p.Price <= parameters.MaxPrice);
        }

        if (parameters.MinArea.HasValue)
        {
            query = query.Where(p => p.Area >= parameters.MinArea);
        }

        if (parameters.MaxArea.HasValue)
        {
            query = query.Where(p => p.Area <= parameters.MaxArea);
        }

        if (!string.IsNullOrEmpty(parameters.Type))
        {
            query = query.Where(p => p.Type == parameters.Type);
        }
        
        if (!string.IsNullOrEmpty(parameters.Purpose))
        {
            query = query.Where(p => p.Purpose == parameters.Purpose);
        }

        // Aplicar ordenação
        if (!string.IsNullOrEmpty(parameters.OrderBy))
        {
            switch (parameters.OrderBy.ToLower())
            {
                case "price":
                    query = parameters.SortDirection == "asc"
                        ? query.OrderBy(p => p.Price)
                        : query.OrderByDescending(p => p.Price);
                    break;
                case "createdat":
                    query = parameters.SortDirection == "asc"
                        ? query.OrderBy(p => p.CreatedAt)
                        : query.OrderByDescending(p => p.CreatedAt);
                    break;
                case "area":
                    query = parameters.SortDirection == "asc"
                        ? query.OrderBy(p => p.CreatedAt)
                        : query.OrderByDescending(p => p.CreatedAt);
                    break;
                default:
                    query = query.OrderBy(p => p.CreatedAt);
                    break;
            }
        }

        // Paginação
        var totalCount = await query.CountAsync();
        var items = await query
            .Skip((parameters.PageNumber - 1) * parameters.PageSize)
            .Take(parameters.PageSize)
            .ToListAsync();

        return PagedList<Property>.Create(items, totalCount, parameters.PageNumber, parameters.PageSize);
    }
}
