using AppCore.Dtos;
using AppCore.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controller;

[ApiController]
[Route("/api/[controller]")]
public class GatesController(IParkingGateService service): ControllerBase
{
    public async Task<IActionResult> GetAllGates(int page = 1, int size = 100)
    {
        return Ok(await service.GetAllPaged(page, size));
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateGate(CreateGateDto dto)
    {
        var result = await service.Create(dto);
        return CreatedAtAction(nameof(GetGate), new { id = result.Id }, result);
    }
    
    [HttpGet("{id:guid}")]
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
    public async Task<IActionResult> UpdateGate(Guid id, UpdateGateDto gateDto)
    {
        var dto = await service.Update(id, gateDto);
        if (dto == null)
        {
            return NotFound();
        }
        
        return Ok(dto);
    }
}