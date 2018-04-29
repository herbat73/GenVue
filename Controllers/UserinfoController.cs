using System.Threading.Tasks;
using AspNet.Security.OAuth.Validation;
using AspNet.Security.OpenIdConnect.Primitives;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using GenVue.Model;
using Newtonsoft.Json.Linq;
using OpenIddict.Core;
using System.Collections.Generic;
using System.Linq;

namespace GenVue.Controllers
{
    [Authorize(AuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme)]
    [Route("api")]
    public class UserinfoController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserinfoController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        // GET: /api/userinfo
        [Authorize(AuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme)]
        [HttpGet("userinfo"), Produces("application/json")]
        public async Task<IActionResult> Userinfo()
        {
            ApplicationUser user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return BadRequest(new OpenIdConnectResponse
                {
                    Error = OpenIdConnectConstants.Errors.InvalidGrant,
                    ErrorDescription = "The user profile is no longer available."
                });
            }

            JObject o = new JObject
            {
                { "quota", 1000000 },
                { "usage", 0 }
            };
  
            return Json(o);
        }

        // GET: /api/userroles
        [HttpGet("userroles"), Produces("application/json")]
        public async Task<IActionResult> Userroles()
        {
            ApplicationUser user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return BadRequest(new OpenIdConnectResponse
                {
                    Error = OpenIdConnectConstants.Errors.InvalidGrant,
                    ErrorDescription = "The user profile is no longer available."
                });
            }

            IEnumerable<string> roles = await _userManager.GetRolesAsync(user);

            bool IsAdmin = roles.Contains("Admin");
            bool IsManager = roles.Contains("Manager");

            JObject o = new JObject
            {
                { "IsAdmin", IsAdmin },
                { "IsManager", IsManager }
            };

            return Json(o);
        }
    }
}
