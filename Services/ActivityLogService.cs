using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GenVue.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GenVue.Services
{
    public class ActivityLogService : IActivityLogService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly DefaultDbContext _context;

        public ActivityLogService(UserManager<ApplicationUser> userManager,
                    DefaultDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task AddActivityAsync(string User, string Action, int Level, int? FileId)
        {

            var activityLog = new ActivityLog(User, Action, Level, FileId);

            await _context.ActivityLogs.AddAsync (activityLog);
            await _context.SaveChangesAsync();
        }

    }
}
