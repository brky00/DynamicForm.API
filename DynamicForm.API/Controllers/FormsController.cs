
using DynamicForm.API.Dto;
using DynamicForm.API.Models;
using DynamicForm.API.Models.SubmissionFolder;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
    //With that meta atribute only logged-in users can create a form (JWT)
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
  

    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpGet]
    public async Task<IActionResult> GetForms(int pageNumber = 1, int pageSize = 5)
    {
        var queryForm = _context.Forms
            .Include(f => f.Fields);
         
        var totalCount = await queryForm.CountAsync();

        var forms = await queryForm
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var result = new PagedResult<Form>(forms, totalCount, pageSize, pageNumber);

        return Ok(result); // veya return Ok(forms);
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


//[HttpGet]
//public async Task<IActionResult> GetSubmissions(int formId, int pageNumber = 1, int pageSize = 5)
//{
//    var query = _context.Submissions
//        .Where(s => s.FormId == formId)
//        .Include(s => s.Answers)
//        .OrderByDescending(s => s.SubmittedAt);

//    var totalCount = await query.CountAsync();
//    var submissions = await query
//        .Skip((pageNumber - 1) * pageSize)
//        .Take(pageSize)
//        .ToListAsync();
//    var result = new PagedResult<Submission>(submissions, totalCount, pageSize, pageNumber); // I created a central pagination/paged result file/module which we can use multiple times in other Controllers too(for ex FormController)


//    return Ok(result);
//}