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
}