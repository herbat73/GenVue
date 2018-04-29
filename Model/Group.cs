using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GenVue.Model
{
    public class Group
    {

        public int Id { get; set; }

        [Required]
        [MinLength(3)]
        [Display(Name = "Group name")]
        public string Name { get; set; }

        public ICollection<StoredFile> StoredFiles { get; set; }

        public ICollection<GroupAccess> GroupAccess { get; set; }

    }
}
