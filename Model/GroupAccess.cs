using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GenVue.Model
{
    public class GroupAccess
    {
        public int Id { get; set; }

        public string ApplicationUserId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

        public int GroupId { get; set; }
    }
}
