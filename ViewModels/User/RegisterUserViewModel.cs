using System.ComponentModel.DataAnnotations;

namespace GenVue.ViewModels.User
{
    public class RegistrationRequest
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
