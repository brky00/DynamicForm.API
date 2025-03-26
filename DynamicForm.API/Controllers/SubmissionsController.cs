
using Azure.Core;
using DynamicForm.API.Dto;
using DynamicForm.API.Models.SubmissionFolder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DynamicForm.API.Controllers;

[ApiController]
[Route("forms/{formId}/submissions")]
public class SubmissionsController : ControllerBase
{
    private readonly AppDbContext _context;

    public SubmissionsController(AppDbContext context)
    {
        _context = context;
    }

    // POST /forms/{formId}/submissions
    [HttpPost]
    public async Task<IActionResult> SubmitForm(int formId, [FromBody] SubmissionCreateDto dto)
    {
        //to check if the form exists
        var form = await _context.Forms.FindAsync(formId);
        if (form == null) return NotFound();

        // I could actually use automappers(with configration of Profiles) to map the dto to the entity
        // but because of the time i made it easy...

        var submission = new Submission
        {
            FormId = formId,
            SubmittedAt = DateTime.UtcNow,
            Answers = dto.Answers.Select(a => new SubmissionAnswer
            {
                FieldId = a.FieldId,
                Value = a.Value
            }).ToList()
        };

        _context.Submissions.Add(submission);
        await _context.SaveChangesAsync();

        return Ok(submission);
    }

    // GET forms/{formId}/submissions?page=1&pageSize=5 , this is just a example to url.
    [HttpGet]
    public async Task<IActionResult> GetSubmissions(int formId, int pageNumber = 1, int pageSize = 5)
    {
        var query = _context.Submissions
            .Where(s => s.FormId == formId)
            .Include(s => s.Answers)
            .OrderByDescending(s => s.SubmittedAt);

        var totalCount = await query.CountAsync();
        var submissions = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        var result = new PagedResult<Submission>(submissions, totalCount, pageSize, pageNumber); // I created a central pagination/paged result file/module which we can use multiple times in other Controllers too(for ex FormController)


        return Ok(result);
    }
}
