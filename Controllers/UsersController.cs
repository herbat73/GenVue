using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using GenVue.Model;
using GenVue.Services;
using GenVue.ViewModels.User;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using AspNet.Security.OAuth.Validation;
using System.Net;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System;

namespace GenVue.Controllers
{
    [Authorize(AuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme)]
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly DefaultDbContext _context;

        public UsersController(
                UserManager<ApplicationUser> userManager,
                SignInManager<ApplicationUser> signInManager,
                DefaultDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        [HttpPost("~/api/register"), Produces("application/json")]
        public async Task<IActionResult> Register(RegistrationRequest model)
        {
            bool hasPermission = await userHasRole("Admin");
            if (!hasPermission) return BadRequest(); ;

            //var user = new ApplicationUser { FirstName = model.Firstname, LastName = model.Lastname, UserName = model.Username, Email = model.Username };
            //var result = await _userManager.CreateAsync(user, model.Password);

            bool created =  AddUserToRole(model.Username, model.Password, model.Role, model.Firstname, model.Lastname);

            if (created)
            {
                return Ok();
            }

            // If we got this far, something failed, send back badrequest
            return BadRequest();
        }

        // GET: users
        [HttpGet("~/api/users")]
        public async Task<IEnumerable<ApplicationUser>> GetAsync()
        {
            bool hasPermission = await userHasRole("Admin");
            if  (!hasPermission) return null;
                
            return await _context.ApplicationUsers.Select(c => new ApplicationUser
                                {
                                    Id = c.Id,
                                    Email = c.Email,
                                    FirstName = c.FirstName,
                                    LastName = c.LastName
                                }).ToListAsync();

        }

        private async Task<bool> userHasRole(string role)
        {
            ApplicationUser user = await _userManager.GetUserAsync(User);
            IEnumerable<string> roles = await _userManager.GetRolesAsync(user);

            return roles.Contains(role);
        }

        /// <summary>
        /// Add user to a role if the user exists, otherwise, create the user and adds him to the role.
        /// </summary>
        /// <param name="userEmail">User Email</param>
        /// <param name="userPwd">User Password. Used to create the user if not exists.</param>
        /// <param name="roleName">Role Name</param>
        /// <param name="firstName">First Name</param>
        /// <param name="lastName">Last Name</param>
        private bool AddUserToRole(string userEmail, string userPwd, string roleName, string firstName, string lastName)
        {

            Task<ApplicationUser> checkAppUser = _userManager.FindByEmailAsync(userEmail);
            checkAppUser.Wait();

            ApplicationUser appUser = checkAppUser.Result;

            if (checkAppUser.Result == null)
            {
                ApplicationUser newAppUser = new ApplicationUser
                {
                    UserName = userEmail,
                    NormalizedUserName = userEmail,
                    Email = userEmail,
                    NormalizedEmail = userEmail,
                    FirstName = firstName,
                    LastName = lastName,
                    EmailConfirmed = true,
                    LockoutEnabled = false,
                    SecurityStamp = Guid.NewGuid().ToString()
                };

                Task<IdentityResult> taskCreateAppUser = _userManager.CreateAsync(newAppUser, userPwd);
                taskCreateAppUser.Wait();

                if (taskCreateAppUser.Result.Succeeded)
                {
                    appUser = newAppUser;

                    // add role if not user
                    if (!String.IsNullOrEmpty(roleName) && roleName.ToLower() != "user")
                    {
                        Task<IdentityResult> newUserRole = _userManager.AddToRoleAsync(appUser, roleName);
                        newUserRole.Wait();
                    }

                    return true;
                }

            }

            return false;

        }

        // // GET book/5
        // [HttpGet("{id}", Name = "GetBook")]
        // public IActionResult Get(int id)
        // {
        //     var book = _bookRepository.GetById(id);
        //     if (book == null)
        //     {
        //         return NotFound();
        //     }

        //     return Ok(book);
        // }

    }
}
