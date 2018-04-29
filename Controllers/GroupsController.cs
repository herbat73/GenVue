using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using GenVue.Model;
using AspNet.Security.OAuth.Validation;
using Microsoft.AspNetCore.Identity;

namespace GenVue.Controllers
{
    [Authorize(AuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    public class GroupsController : Controller
    {
        private readonly DefaultDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public GroupsController(UserManager<ApplicationUser> userManager,
                                DefaultDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        // GET api/Groups
        [HttpGet]
        public async Task<IEnumerable<Group>> GetAsync()
        {

            // check user rights to enumerate group list

            bool hasPermission = await userHasRole("Admin") || await userHasRole("Manager");

            //return _context.Groups.

            // if (hasPermission)
            return _context.Groups.OrderBy((o) => o.Name);
        }

        // GET api/Groups/5
        [HttpGet("{id}", Name = "GetGroup")]
        public Group Get(int id)
        {
            return _context.Groups.Find(id);
        }

        // POST api/Groups
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Group model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Groups.Add(model);
            await _context.SaveChangesAsync();
            return Ok();
        }

        // PUT api/Groups/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody]Group model)
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

        // DELETE api/Groups/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            Group Group = new Group() { Id = id };
            _context.Entry(Group).State = EntityState.Deleted;

            await _context.SaveChangesAsync();
            return Ok();
        }

        private async Task<bool> userHasRole(string role)
        {
            ApplicationUser user = await _userManager.GetUserAsync(User);
            IEnumerable<string> roles = await _userManager.GetRolesAsync(user);

            return roles.Contains(role);
        }
    }
}
