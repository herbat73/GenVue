using System;
using System.ComponentModel.DataAnnotations;

namespace GenVue.Model
{
    public class ActivityLog
    {

        public ActivityLog()
        {
        }

        public ActivityLog(string user, string action, int level, int? fileId)
        {
            this.User = user;
            this.Action = action;
            this.Level = level;
            this.FileId = fileId;
        }

        public int Id { get; set; }

        public string User { get; set; }

        public string Action { get; set; }
    
        public int Level { get; set; }

        public int? FileId { get; set; }

        public DateTime ActivityDate { get; set; } = DateTime.Now;


    }
}
