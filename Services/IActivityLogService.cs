using GenVue.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GenVue.Services
{
    public interface IActivityLogService
    {
        Task AddActivityAsync(string User, string Action, int Level, int? FileId);
    }
}
