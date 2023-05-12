using Blog.Data;
using Blog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Api.Controllers
{
    [ApiController]
    [Route("categories")]
    public class CategoryController : ControllerBase
    {
        private readonly BlogDataContext _context;
        public CategoryController(BlogDataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            try
            {
                var categories = await _context.Categories.ToListAsync();
                return Ok(categories);
            }
            catch (Exception)
            {
                return StatusCode(500, "Falha interna no servidor"); ;
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
        {
            try
            {
                var category = await _context
                .Categories
                .FirstOrDefaultAsync(x => x.Id == id);

                if (category == null)
                    return NotFound();

                return Ok(category);
            }
            catch (Exception)
            {
                return StatusCode(500, "Falha interna no servidor"); ;
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] Category model)
        {
            try
            {
                await _context.Categories.AddAsync(model);
                await _context.SaveChangesAsync();

                return Created($"categories/{model.Id}", model);
            }
            catch(DbUpdateException ex)
            {
                return StatusCode(500, "Não foi possível incluir a categoria");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Falha interna no servidor");
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutAsync([FromRoute] int id, [FromBody] Category model)
        {
            try
            {
                var category = await _context
                    .Categories
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (category == null)
                    return NotFound();

                category.Name = model.Name;
                category.Slug = model.Slug;

                _context.Categories.Update(category);
                await _context.SaveChangesAsync();

                return Ok(category);
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, "Não foi possível alterar a categoria");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Falha interna no servidor");
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id)
        {
            try
            {
                var category = await _context
                .Categories
                .FirstOrDefaultAsync(x => x.Id == id);

                if (category == null)
                    return NotFound();

                _context.Remove(category);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, "Não foi possível excluir a categoria");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Falha interna no servidor");
            }
        }
    }
}
