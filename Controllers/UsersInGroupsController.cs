using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using GenVue.Model;
using AspNet.Security.OAuth.Validation;
using Microsoft.AspNetCore.Identity;
using GenVue.ViewModels.User;

namespace GenVue.Controllers
{
    [Authorize(AuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    public class UsersInGroupsController : Controller
    {
        private readonly DefaultDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UsersInGroupsController(UserManager<ApplicationUser> userManager, DefaultDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        // GET api/usersingroup/5
        [HttpGet("/api/usersingroup/{id}", Name = "usersingroup")]
        public async Task<IEnumerable<GroupAccess>> UsersInGroup(int id)
        {

            bool hasPermission = await userHasRole("Admin");
            if (!hasPermission) return null; 

            return await _context.GroupAccess
                    .Where(g => g.GroupId == id)
                    .Include(u => u.ApplicationUser)
                    .ToListAsync();
        }


        [HttpPost("/api/addusertogroup")]
        public async Task<IActionResult> AddUserToGroup(AddUserToGroupViewModel model)
        {
            bool hasPermission = await userHasRole("Admin");
            if (!hasPermission) return BadRequest();

            GroupAccess groupAccess = new GroupAccess
            {
                GroupId = model.GroupId,
                ApplicationUserId = model.ApplicationUserId
            };

            _context.GroupAccess.Add(groupAccess);
            _context.SaveChanges();

            return Ok();
        }

        [HttpDelete("/api/usersingroup/{id}", Name = "usersingroup")]
        public async Task<IActionResult> Delete(int id)
        {
            bool hasPermission = await userHasRole("Admin");
            if (!hasPermission) return BadRequest(); 

            GroupAccess groupAccess = new GroupAccess() { Id = id };
            _context.Entry(groupAccess).State = EntityState.Deleted;

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
