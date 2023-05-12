using Blog.Api.Extensions;
using Blog.Api.ViewModels;
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
                return Ok(new ResultViewModel<IEnumerable<Category>>(categories));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<IEnumerable<Category>>("Falha interna no servidor"));
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
                    return NotFound(new ResultViewModel<Category>("Conteúdo não encontrado"));

                return Ok(new ResultViewModel<Category>(category));
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultViewModel<Category>("Falha interna no servidor"));
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] EditorCategoryViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new ResultViewModel<Category>(ModelState.GetErros()));

                var category = new Category
                { 
                    Id = 0,
                    Name = model.Name,
                    Slug = model.Slug,
                };

                await _context.Categories.AddAsync(category);
                await _context.SaveChangesAsync();

                return Created($"categories/{category.Id}", new ResultViewModel<Category>(category));
            }
            catch(DbUpdateException ex)
            {
                return StatusCode(500, new ResultViewModel<Category>("Não foi possível incluir a categoria"));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<Category>("Falha interna no servidor"));
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutAsync([FromRoute] int id, [FromBody] EditorCategoryViewModel model)
        {
            try
            {
                var category = await _context
                    .Categories
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (category == null)
                    return NotFound(new ResultViewModel<Category>("Conteúdo não encontrado"));

                if (!ModelState.IsValid)
                    return BadRequest(new ResultViewModel<Category>(ModelState.GetErros()));

                category.Name = model.Name;
                category.Slug = model.Slug;

                _context.Categories.Update(category);
                await _context.SaveChangesAsync();

                return Ok(new ResultViewModel<Category>(category));
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new ResultViewModel<Category>("Não foi possível alterar a categoria"));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<Category>("Falha interna no servidor"));
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
                    return NotFound(new ResultViewModel<Category>("Conteúdo não encontrado"));

                _context.Remove(category);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new ResultViewModel<Category>("Não foi possível excluir a categoria"));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<Category>("Falha interna no servidor"));
            }
        }
    }
}
