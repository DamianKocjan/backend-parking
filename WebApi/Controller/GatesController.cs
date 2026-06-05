using AppCore.Authorization;
using AppCore.Dtos;
using AppCore.Services;
using AppCore.ValueObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controller;

[ApiController]
[Route("/api/[controller]")]
public class GatesController(IParkingGateService service): ControllerBase
{
    [HttpGet]
    [Authorize(Policy = nameof(AppPolicies.AdminOnly))]
    [ProducesResponseType(typeof(PagedResult<PagedResult<ParkingGateDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetAllGates(int page = 1, int size = 100)
    {
        return Ok(await service.GetAllPaged(page, size));
    }
    
    [HttpPost]
    [Authorize(Policy = nameof(AppPolicies.ActiveUser))]
    [ProducesResponseType(typeof(PagedResult<ParkingGateDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CreateGate(CreateGateDto dto)
    {
        var result = await service.Create(dto);
        return CreatedAtAction(nameof(GetGate), new { id = result.Id }, result);
    }
    
    [HttpGet("{id:guid}")]
    [Authorize(Policy = nameof(AppPolicies.AdminOnly))]
    [ProducesResponseType(typeof(ParkingGateDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetGate(Guid id)
    {
        var dto = await service.GetById(id);
        if (dto == null)
        {
            return NotFound();
        }
        
        return Ok(dto);
    }
    
    [HttpPut("{id:guid}")]
    [Authorize(Policy = nameof(AppPolicies.AdminOnly))]
    [ProducesResponseType(typeof(PagedResult<ParkingGateDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> UpdateGate(Guid id, UpdateGateDto gateDto)
    {
        var dto = await service.Update(id, gateDto);
        if (dto == null)
        {
            return NotFound();
        }
        
        return Ok(dto);
    }
    
    [HttpPost("{id:guid}/captures")]
    [Authorize(Policy = nameof(AppPolicies.ActiveUser))]
    [ProducesResponseType(typeof(CameraCaptureDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddCameraCapture([FromRoute] Guid id, [FromBody] CreateCameraCaptureDto dto)
    {
        var capture = await service.AddCapture(id, dto);
        return CreatedAtAction(
            nameof(GetCaptures),
            new { id },
            capture
        );
    }

    [HttpGet("{id:guid}/captures")]
    [Authorize(Policy = nameof(AppPolicies.ActiveUser))]
    [ProducesResponseType(typeof(PagedResult<CameraCaptureDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCaptures([FromRoute] Guid id, int page = 1, int size = 100)
    {
        var gate = await service.GetById(id);
        if (gate == null)
        {
            return NotFound();
        }
        
        var captures = await service.GetCameraCaptures(id, page, size);
        return Ok(captures);
    }
    
    [HttpDelete("{id:guid}/captures/{captureId:guid}")]
    [Authorize(Policy = nameof(AppPolicies.AdminOnly))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteCapture([FromRoute] Guid id, [FromRoute] Guid captureId)
    {
        await service.DeleteCapture(id, captureId);
        
        return NoContent();
    }
}