using AutoMapper;
using conecta_api.context;
using conecta_api.DTOs.PropertyDTOs;
using conecta_api.Models;
using conecta_api.Pagination;
using conecta_api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace conecta_api.Services;
public class PropertyService : IPropertyService
{
    private readonly AppDbContext _context;
    private readonly IUnityOfWork _uof;
    private readonly IMapper _mapper;

    private readonly IPhotoService _photoService;

    public PropertyService(AppDbContext context, IUnityOfWork uof, IMapper mapper, IPhotoService photoService)
    {
        _context = context;
        _uof = uof;
        _mapper = mapper;
        _photoService = photoService;
    }

    public async Task<Property> AddProperty(AddPropertyDTO addPropertyDTO)
    {
        try
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
            await _uof.Commit();

            return property;
        }
        catch (DbUpdateException ex)
        {
            throw new Exception("Erro ao salvar o imóvel.", ex);
        }
        catch (Exception ex)
        {
            throw new Exception("Ocorreu um erro ao adicionar o imóvel.", ex);
        }
    }

    public async Task<List<PhotoDTO>> AddPhoto(string propertyId, List<string> img)
    {
        // Faz upload das imagens e obtém as URLs correspondentes
        var photoUrls = await _photoService.UploadPhotos(img);

        // Busca o imóvel pelo ID
        var property = await _context.Properties.FindAsync(Guid.Parse(propertyId));
        if (property == null)
            throw new Exception("Imóvel não encontrado.");

        // Desserializa a lista de fotos existente ou inicializa uma nova lista
        var photosList = JsonConvert.DeserializeObject<List<string>>(property.Photos ?? "[]")
                         ?? new List<string>();

        // Adiciona as novas URLs à lista
        photosList.AddRange(photoUrls);

        // Atualiza a propriedade com a lista atualizada de fotos
        property.Photos = JsonConvert.SerializeObject(photosList);
        _context.Properties.Update(property);
        await _context.SaveChangesAsync();

        // Retorna um DTO para cada URL adicionada
        return photoUrls.Select(url => new PhotoDTO { Url = url }).ToList();
    }



    public async Task<PagedList<PropertySummaryDTO>> GetAvailableProperties(PropertyParameters parameters)
    {
        var query = _context.Properties
            .Include(p => p.Address)
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

        var dtos = _mapper.Map<IEnumerable<PropertySummaryDTO>>(items);

        return PagedList<PropertySummaryDTO>.Create(dtos, totalCount, parameters.PageNumber, parameters.PageSize);
    }

    public async Task<PropertyDTO> GetPropertyById(string id)
    {
        try
        {
            Guid propertyId = Guid.Parse(id);

            var property = await _context.Properties
                .Include(p => p.Address)
                .Where(p => p.Status == "Disponível")
                .FirstOrDefaultAsync(p => p.Id == propertyId);

            if (property == null)
            {
                throw new KeyNotFoundException($"Imóvel não encontrado.");
            }

            var propertyDTO = _mapper.Map<PropertyDTO>(property);

            return propertyDTO;
        }
        catch (FormatException ex)
        {
            throw new ArgumentException("ID do imóvel está em formato inválido.", ex);
        }
        catch (Exception ex)
        {
            throw new Exception("Ocorreu um erro ao buscar o imóvel.", ex);
        }
    }

    public async Task<PagedList<PropertySummaryDTO>> GetFavoriteProperties(PropertyParameters parameters, List<Guid> propertiesIds)
    {
        var query = _context.Properties
            .Include(p => p.Address)
            .Where(p => propertiesIds.Contains(p.Id) && p.Status == "Disponível")
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

        var dtos = _mapper.Map<IEnumerable<PropertySummaryDTO>>(items);

        return PagedList<PropertySummaryDTO>.Create(dtos, totalCount, parameters.PageNumber, parameters.PageSize);
    }
}
