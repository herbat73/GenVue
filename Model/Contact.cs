using System.ComponentModel.DataAnnotations;

namespace GenVue.Model
{
    public class Contact
    {
        public int contactId { get; set; }

        [Required]
        [MinLength(3)]
        [Display(Name = "Last Name")]
        public string lastName { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string firstName { get; set; }

        public string phone { get; set; }

        [DataType(DataType.EmailAddress)]
        [StringLength(30, MinimumLength = 0)]
        public string email { get; set; }
    }
}
