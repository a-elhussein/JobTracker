using Backend.Core.DTOs;
using Backend.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class JobApplicationsController : ControllerBase
{
    private readonly IJobApplicationService _service;

    public JobApplicationsController(IJobApplicationService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<JobApplicationResponseDto>>> GetAll(
        [FromQuery] string? status,
        [FromQuery] string? company,
        [FromQuery] PaginationParams pagination)
    {
        var result = await _service.GetAllAsync(status, company, pagination);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<JobApplicationResponseDto>> GetById(Guid id)
    {
        var result = await _service.GetByIdAsync(id);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<JobApplicationResponseDto>> Create(CreateJobApplicationDto dto)
    {
        var result = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<JobApplicationResponseDto>> Update(
        Guid id,
        [FromBody] UpdateJobApplicationDto dto)
    {
        if (id != dto.Id)
            return BadRequest("Route id and body id must match.");

        var result = await _service.UpdateAsync(id, dto);
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _service.DeleteAsync(id);
        return NoContent();
    }
}