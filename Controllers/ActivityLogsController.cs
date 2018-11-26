using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using GenVue.Model;
using AspNet.Security.OAuth.Validation;

namespace GenVue.Controllers
{
    [Authorize(AuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    public class ActivityLogsController
    {
        DefaultDbContext _context;

        public ActivityLogsController(DefaultDbContext context)
        {
            _context = context;
        }

        // GET api/ActivityLogs
        [HttpGet]
        public IEnumerable<ActivityLog> Get()
        {
            return  _context.ActivityLogs
                                    .OrderByDescending(i => i.ActivityDate)
                                    .Take(200);
        }

        // GET api/ActivityLogs/5
        [HttpGet("{id}", Name = "GetActivityLogs")]
        public ActivityLog Get(int id)
        {
            return _context.ActivityLogs.Find(id);
        }

        // GET api/ActivityLogs/?=
        [HttpGet("search")]
        public IEnumerable<ActivityLog> Search(string q)
        {
            return _context.ActivityLogs
                .Where((c) => c.User.Contains(q) || c.Action.Contains(q))
                .OrderByDescending(i => i.ActivityDate);
        }
    }

}
