using System.ComponentModel.DataAnnotations;

namespace GenVue.ViewModels.User
{
    public class AddUserToGroupViewModel
    {
        public string ApplicationUserId { get; set; }
        public int GroupId { get; set; }
    }
}
