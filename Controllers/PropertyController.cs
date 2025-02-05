using conecta_api.DTOs.PropertyDTOs;
using conecta_api.Pagination;
using conecta_api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace conecta_api.Controllers;

[Route("/[controller]")]
[ApiController]
[Produces("application/json")]
public class PropertyController : ControllerBase
{
    private readonly IPropertyService _propertyService;

    public PropertyController(IPropertyService propertyService)
    {
        _propertyService = propertyService;
    }

    /// <summary>
    /// Adicionar um novo imóvel
    /// </summary>
    /// <param name="addPropertyDTO">Informações a serem adicionadas</param>
    /// <returns>Retorna um status 201 (Created) com o imóvel criado.</returns>
    [HttpPost]
    public async Task<IActionResult> AddProperty([FromBody] AddPropertyDTO addPropertyDTO)
    {
        var property = await _propertyService.AddProperty(addPropertyDTO);
        return CreatedAtAction(nameof(AddProperty), new {id = property.Id}, property);
    }

    /// <summary>
    /// Obtém uma lista de imóveis disponíveis com base nos parâmetros fornecidos.
    /// </summary>
    /// <param name="parameters">Parâmetros de consulta, como filtros e paginação.</param>
    /// <returns>Retorna uma lista paginada de imóveis disponíveis.</returns>
    /// <remarks>
    /// <para><strong>Parâmetros disponíveis:</strong></para>
    /// <list type="bullet">
    ///   <item><description><c>City</c>: Nome da cidade onde o imóvel está localizado.</description></item>
    ///   <item><description><c>Neighborhood</c>: Nome do bairro onde o imóvel está localizado.</description></item>
    ///   <item><description><c>Bedrooms</c>: Número de quartos do imóvel.</description></item>
    ///   <item><description><c>ParkingSpaces</c>: Número de vagas de garagem.</description></item>
    ///   <item><description><c>MinPrice</c>: Preço mínimo do imóvel.</description></item>
    ///   <item><description><c>MaxPrice</c>: Preço máximo do imóvel.</description></item>
    ///   <item><description><c>MinArea</c>: Área mínima do imóvel em m².</description></item>
    ///   <item><description><c>MaxArea</c>: Área máxima do imóvel em m².</description></item>
    ///   <item><description><c>Type</c>: Tipo do imóvel. Valores possíveis: <c>Casa</c>, <c>Apartamento</c>, <c>Terreno</c>, <c>Sobrado</c>, etc.</description></item>
    ///   <item><description><c>Purpose</c>: Finalidade do imóvel. Valores possíveis: <c>Venda</c>, <c>Aluguel</c>.</description></item>
    ///   <item><description><c>OrderBy</c>: Campo para ordenação. Valores possíveis: <c>price</c>, <c>createdAt</c>, <c>area</c>.</description></item>
    ///   <item><description><c>SortDirection</c>: Direção da ordenação. Valores possíveis: <c>asc</c> (crescente), <c>desc</c> (decrescente).</description></item>
    /// </list>
    /// </remarks>
    [HttpGet("available")]
    public async Task<IActionResult> GetAvailableProperties([FromQuery] PropertyParameters parameters)
    {
        var properties = await _propertyService.GetAvailableProperties(parameters);
        return Ok(properties);
    }

}
