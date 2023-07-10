using Microsoft.AspNetCore.Mvc;
using Planera.Data.Dto;
using Planera.Extensions;
using Planera.Models;
using Planera.Services;

namespace Planera.Controllers;

[ApiController]
[Route("projects/{username}")]
public class ProjectController : ControllerBase
{
    private readonly ProjectService _projectService;

    public ProjectController(ProjectService projectService)
    {
        _projectService = projectService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(ICollection<ProjectDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(string username)
    {
        // TODO: Allow viewing public projects when that's a thing
        if (username != User.Identity?.Name)
            return Unauthorized();

        var result = await _projectService.GetAllAsync(username);

        return result.ToActionResult();
    }

    [HttpGet("{slug}")]
    [ProducesResponseType(typeof(ProjectDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get(string username, string slug)
    {
        // TODO: Allow viewing public projects when that's a thing
        if (username != User.Identity?.Name)
            return Unauthorized();

        var result = await _projectService.GetAsync(username, slug);

        return result.ToActionResult();
    }

    [HttpPost]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    public async Task<IActionResult> Create(string username, [FromBody] CreateProjectModel model)
    {
        if (username != User.Identity?.Name)
            return Unauthorized();

        var result = await _projectService.AddAsync(
            User.FindFirst("Id")!.Value,
            model.Slug,
            model.Name,
            model.Description
        );

        return result.ToActionResult();
    }

    [HttpPut("{slug}")]
    public async Task<IActionResult> Edit(string username, string slug, [FromBody] EditProjectModel model)
    {
        if (username != User.Identity?.Name)
            return Unauthorized();

        var result = await _projectService.EditAsync(
            username,
            slug,
            model.Name,
            model.Description
        );

        return result.ToActionResult();
    }

    [HttpDelete("{slug}")]
    public async Task<IActionResult> Remove(string username, string slug)
    {
        if (username != User.Identity?.Name)
            return Unauthorized();

        var result = await _projectService.RemoveAsync(username, slug);

        return result.ToActionResult();
    }

    [HttpGet("{slug}/tickets")]
    [ProducesResponseType(typeof(ICollection<TicketDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTickets(string username, string slug)
    {
        if (username != User.Identity?.Name)
            return Unauthorized();

        var result = await _projectService.GetTicketsAsync(username, slug);

        return result.ToActionResult();
    }

    [HttpPut("{slug}/addParticipant")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> AddParticipant(string username, string slug, string participantName)
    {
        if (username != User.Identity?.Name)
            return Unauthorized();

        var result = await _projectService.AddParticipantAsync(username, slug, participantName);

        return result.ToActionResult();
    }

    [HttpDelete("{slug}/removeParticipant")]
    public async Task<IActionResult> RemoveParticipant(string username, string slug, string participantName)
    {
        if (username != User.Identity?.Name)
            return Unauthorized();

        var result = await _projectService.RemoveParticipantAsync(username, slug, participantName);

        return result.ToActionResult();
    }
}