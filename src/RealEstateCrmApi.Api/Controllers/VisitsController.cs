using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateCrmApi.Application.Common.Exceptions;
using RealEstateCrmApi.Application.Visits;

namespace RealEstateCrmApi.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/visits")]
public class VisitsController : ControllerBase
{
    private readonly IVisitService _visitService;

    public VisitsController(IVisitService visitService)
    {
        _visitService = visitService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<VisitDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<VisitDto>>> GetAll(CancellationToken cancellationToken)
    {
        var visits = await _visitService.GetAllAsync(cancellationToken);
        return Ok(visits);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(VisitDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<VisitDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var visit = await _visitService.GetByIdAsync(id, cancellationToken);

        if (visit is null)
        {
            return NotFound();
        }

        return Ok(visit);
    }

    [HttpPost]
    [ProducesResponseType(typeof(VisitDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<VisitDto>> Create(
        [FromBody] CreateVisitRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var created = await _visitService.CreateAsync(request, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
        catch (ValidationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(VisitDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<VisitDto>> Update(
        Guid id,
        [FromBody] UpdateVisitRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var updated = await _visitService.UpdateAsync(id, request, cancellationToken);

            if (updated is null)
            {
                return NotFound();
            }

            return Ok(updated);
        }
        catch (ValidationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var deleted = await _visitService.DeleteAsync(id, cancellationToken);

        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}
