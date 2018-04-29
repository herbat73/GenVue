using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GenVue.Model
{
    public class ApplicationUser : IdentityUser
    {

        // FirstName and LastName will be stored in the same table as Users
        [StringLength(50)]
        [Display(Name = "Firt Name")]
        public string FirstName { get; set; }

        [StringLength(50)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Full Name")]
        public string FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }

        public ICollection<StoredFile> StoredFiles { get; set; }

        public ICollection<GroupAccess> GroupAccess { get; set; }
    }
}
