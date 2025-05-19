using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using AtirAPI.Models;
using AtirAPI.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using ECommerceAPI.Data;

namespace ECommerceAPI.Controllers
{
   [Route("api/[controller]")]
   [ApiController]
   [AllowAnonymous] // Allow anonymous access to all actions
   public class CategoriesController : ControllerBase
   {
       private readonly ECommerceDbContext _context;
       private readonly IMapper _mapper;

       public CategoriesController(ECommerceDbContext context, IMapper mapper)
       {
           _context = context;
           _mapper = mapper;
       }

       // GET: api/Categories
       [HttpGet]
       public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetCategories()
       {
           var categories = await _context.Categories.ToListAsync();
           return Ok(_mapper.Map<IEnumerable<CategoryDTO>>(categories));
       }

       // GET: api/Categories/5
       [HttpGet("{id}")]
       public async Task<ActionResult<CategoryDTO>> GetCategory(int id)
       {
           var category = await _context.Categories.FindAsync(id);
           if (category == null)
           {
               return NotFound();
           }
           return _mapper.Map<CategoryDTO>(category);
       }

       // POST: api/Categories
       [HttpPost]
       public async Task<ActionResult<CategoryDTO>> PostCategory(CategoryCreateDTO categoryDto)
       {
           if (!ModelState.IsValid)
           {
               return BadRequest(ModelState);
           }

           var category = _mapper.Map<Category>(categoryDto);
           _context.Categories.Add(category);
           await _context.SaveChangesAsync();

           return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, _mapper.Map<CategoryDTO>(category));
       }

       // PUT: api/Categories/5
       [HttpPut("{id}")]
       public async Task<IActionResult> PutCategory(int id, CategoryUpdateDTO categoryDto)
       {
           var category = await _context.Categories.FindAsync(id);
           if (category == null)
           {
               return NotFound();
           }

           _mapper.Map(categoryDto, category);

           try
           {
               await _context.SaveChangesAsync();
           }
           catch (DbUpdateConcurrencyException)
           {
               if (!CategoryExists(id))
               {
                   return NotFound();
               }
               else
               {
                   throw;
               }
           }

           return NoContent();
       }

       // DELETE: api/Categories/5
       [HttpDelete("{id}")]
       public async Task<IActionResult> DeleteCategory(int id)
       {
           var category = await _context.Categories.FindAsync(id);
           if (category == null)
           {
               return NotFound();
           }

           _context.Categories.Remove(category);
           await _context.SaveChangesAsync();

           return NoContent();
       }

       private bool CategoryExists(int id)
       {
           return _context.Categories.Any(e => e.Id == id);
       }
   }
}