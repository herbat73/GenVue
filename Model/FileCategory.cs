using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GenVue.Model
{
    public class FileCategory
    {
        public int Id { get; set; }

        [Required]
        [MinLength(3)]
        [Display(Name = "Category name")]
        public string Name { get; set; }

        public ICollection<StoredFile> StoredFiles { get; set; }
    }
}
