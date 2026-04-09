using Backend.Core.DTOs;
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
    public async Task<ActionResult<PagedResult<JobApplicationResponseDto>>> GetAll(
        [FromQuery] string? status,
        [FromQuery] string? company,
        [FromQuery] PaginationParams pagination)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _service.GetAllAsync(status, company, pagination);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var application = await _service.GetByIdAsync(id);

        if (application == null)
            return NotFound();

        var response = new JobApplicationResponseDto()
        {
            Id = application.Id,
            Company = application.Company,
            Role = application.Role,
            Status = application.Status,
            DateApplied = application.DateApplied,
            Notes = application.Notes,
            SalaryRange = application.SalaryRange,
            Source = application.Source
        };

        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> Create(JobApplication dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var application = new JobApplication
        {
            Company = dto.Company,
            Role = dto.Role,
            Status = dto.Status,
            DateApplied = dto.DateApplied,
            Notes = dto.Notes,
            SalaryRange = dto.SalaryRange,
            Source = dto.Source
        };

        var createdApplication = await _service.CreateAsync(application);

        var response = new JobApplicationResponseDto
        {
            Id = createdApplication.Id,
            Company = createdApplication.Company,
            Role = createdApplication.Role,
            Status = createdApplication.Status,
            DateApplied = createdApplication.DateApplied,
            Notes = createdApplication.Notes,
            SalaryRange = createdApplication.SalaryRange,
            Source = createdApplication.Source
        };

        return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
    }

    [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateJobApplicationDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != dto.Id)
                return BadRequest("Route id and body id must match.");

            var application = new JobApplication
            {
                Id = dto.Id,
                Company = dto.Company,
                Role = dto.Role,
                Status = dto.Status,
                DateApplied = dto.DateApplied,
                Notes = dto.Notes,
                SalaryRange = dto.SalaryRange,
                Source = dto.Source
            };

            var updatedApplication = await _service.UpdateAsync(application);

            if (updatedApplication == null)
                return NotFound();

            var response = new JobApplicationResponseDto
            {
                Id = updatedApplication.Id,
                Company = updatedApplication.Company,
                Role = updatedApplication.Role,
                Status = updatedApplication.Status,
                DateApplied = updatedApplication.DateApplied,
                Notes = updatedApplication.Notes,
                SalaryRange = updatedApplication.SalaryRange,
                Source = updatedApplication.Source
            };

            return Ok(response);
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
