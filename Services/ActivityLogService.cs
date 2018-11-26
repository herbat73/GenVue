using System.Threading.Tasks;
using GenVue.Model;
using Microsoft.AspNetCore.Identity;

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
