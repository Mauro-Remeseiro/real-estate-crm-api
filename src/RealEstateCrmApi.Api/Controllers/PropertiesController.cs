using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateCrmApi.Application.Common.Exceptions;
using RealEstateCrmApi.Application.Properties;

namespace RealEstateCrmApi.Api.Controllers;

[ApiController]
[Route("api/properties")]
public class PropertiesController : ControllerBase
{
    private readonly IPropertyService _propertyService;

    public PropertiesController(IPropertyService propertyService)
    {
        _propertyService = propertyService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<PropertyDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<PropertyDto>>> GetAll(CancellationToken cancellationToken)
    {
        var properties = await _propertyService.GetAllAsync(cancellationToken);
        return Ok(properties);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(PropertyDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PropertyDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var property = await _propertyService.GetByIdAsync(id, cancellationToken);

        if (property is null)
        {
            return NotFound();
        }

        return Ok(property);
    }

    [Authorize]
    [HttpPost]
    [ProducesResponseType(typeof(PropertyDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<PropertyDto>> Create(
        [FromBody] CreatePropertyRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var created = await _propertyService.CreateAsync(request, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
        catch (ValidationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (UnauthorizedException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
    }
}
