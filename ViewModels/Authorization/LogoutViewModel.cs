using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace GenVue.ViewModels.Authorization
{
    public class LogoutViewModel
    {
        [BindNever]
        public string RequestId { get; set; }
    }
}
