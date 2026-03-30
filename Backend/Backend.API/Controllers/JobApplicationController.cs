using Backend.Core.Models;
using Backend.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class JobApplicationsController : ControllerBase
{
    private readonly IJobApplicationService _service;

    public JobApplicationsController(IJobApplicationService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? status, [FromQuery] string? company)
    {
        var applications = await _service.GetAllAsync(status, company);
        return Ok(applications);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var application = await _service.GetByIdAsync(id);

        if (application == null)
            return NotFound();

        return Ok(application);
    }

    [HttpPost]
    public async Task<IActionResult> Create(JobApplication application)
    {
        var createdApplication = await _service.CreateAsync(application);
        return Ok(createdApplication);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, JobApplication application)
    {
        if (id != application.Id)
            return BadRequest("Route id and application id must match");
        
        var updatedApplication = await _service.UpdateAsync(application);
        
        if (updatedApplication == null)
            return NotFound();
        return Ok(updatedApplication);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await _service.DeleteAsync(id);
        if (!deleted)
            return NotFound();
        
        return NoContent();
    }
}