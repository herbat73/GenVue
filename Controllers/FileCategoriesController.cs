using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using GenVue.Model;
using AspNet.Security.OAuth.Validation;

namespace GenVue.Controllers
{
    [Authorize(AuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    public class FileCategoriesController : Controller
    {
        DefaultDbContext _context;

        public FileCategoriesController(DefaultDbContext context)
        {
            _context = context;
        }

        // GET api/FileCategories
        [HttpGet]
        public IEnumerable<FileCategory> Get()
        {
            return _context.FileCategories.OrderBy((o) => o.Name);
        }

        // GET api/FileCategories/5
        [HttpGet("{id}", Name = "GetFileCategory")]
        public FileCategory Get(int id)
        {
            return _context.FileCategories.Find(id);
        }

        // POST api/FileCategories
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]FileCategory model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.FileCategories.Add(model);
            await _context.SaveChangesAsync();
            return Ok();
        }

        // PUT api/FileCategories/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody]FileCategory model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            model.Id = id;
            _context.Update(model);
            await _context.SaveChangesAsync();
            return Ok();
        }

        // DELETE api/FileCategories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            FileCategory Group = new FileCategory() { Id = id };
            _context.Entry(Group).State = EntityState.Deleted;

            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
