
using DynamicForm.API.Dto;
using DynamicForm.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DynamicForm.API.Controllers;

// Note:
//I would actually use the CQRS pattern with a mediator or service layer for better structure,
//but since I have so little time, I'm doing both the service and repository logic at the controller level.
[ApiController]
[Route("forms")]
public class FormsController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly UserManager<User> _userManager;

    public FormsController(AppDbContext context, UserManager<User> userManager)
    {
        _context = context;
        _userManager = userManager;
    }
    //endpoint like you wrote in the tasks
    // POST /forms
    [HttpPost]
    //[Authorize]
    public async Task<IActionResult> CreateForm([FromBody] FormCreateDto dto)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        // I could actually use automappers(with configration of Profiles) to map the dto to the entity
        // but because of the time i made it easy...
        var form = new Form
        {
            Title = dto.Title,
            UserId = userId,
            Fields = dto.Fields.Select(f => new Field
            {
                Type = f.Type,
                Label = f.Label,
                ExtraValue = f.ExtraValue,
                Required = f.Required,
                MinLength = f.MinLength,
                MaxLength = f.MaxLength,

                Options = f.Options
            }).ToList()
        };


        _context.Forms.Add(form);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetFormById), new { id = form.Id }, form);
    }

    // GET /forms
    [HttpGet]
    public async Task<IActionResult> GetForms()
    {
        var forms = await _context.Forms
            .Include(f => f.Fields) //Includind the fields of the form
            .ToListAsync();

        return Ok(forms);
    }

    // GET /forms/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetFormById(int id)
    {
        var form = await _context.Forms
            .Include(f => f.Fields)
            .FirstOrDefaultAsync(f => f.Id == id);

        if (form == null)
            return NotFound();

        return Ok(form);
    }
}
