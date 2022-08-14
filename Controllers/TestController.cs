using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("v1")]
public class TestController : ControllerBase
{
    [HttpGet]
    [Route("tests")]
    public async Task<IActionResult> GetAsync(
        [FromServices] AppDbContext context)
    {
        var tests = await context
            .Tests
            .AsNoTracking()
            .ToListAsync();
        return Ok(tests);
    }

    [HttpGet]
    [Route("tests/{id}")]
    public async Task<IActionResult> GetByIdAsync(
        [FromServices] AppDbContext context,
        [FromRoute] int id)
    {
        var test = await context
            .Tests
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);
        return test == null
            ? NotFound()
            : Ok(test);
    }

    [HttpPost("test")]
    public async Task<IActionResult> PostAsync(
        [FromServices] AppDbContext context,
        [FromBody] CreateTestViewModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var test = new Test
        {
            Date = DateTime.Now,
            Done = false,
            Title = model.Title
        };

        try
        {
            await context.Tests.AddAsync(test);
            await context.SaveChangesAsync();
            return Created($"v1/tests/{test.Id}", test);
        }
        catch (Exception e)
        {
            return BadRequest();
        }
    }

    [HttpPut("tests/{id}")]
    public async Task<IActionResult> PutAsync(
        [FromServices] AppDbContext context,
        [FromBody] CreateTestViewModel model,
        [FromRoute] int id)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var test = await context.Tests
            .FirstOrDefaultAsync(x => x.Id == id);

        if (test == null)
            return NotFound();

        try
        {
            test.Title = model.Title;

            context.Tests.Update(test);
            await context.SaveChangesAsync();
            return Ok(test);
        }
        catch (Exception e)
        {
            return BadRequest();
        }
    }

    [HttpDelete("tests/{id}")]
    public async Task<IActionResult> DeleteAsync(
        [FromServices] AppDbContext context,
        [FromRoute] int id)
    {
        var test = await context.Tests
            .FirstOrDefaultAsync(x => x.Id == id);

        try
        {
            context.Tests.Remove(test);
            await context.SaveChangesAsync();
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest();
        }
    }
}