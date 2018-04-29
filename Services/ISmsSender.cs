using System.Threading.Tasks;

namespace GenVue.Services
{
    public interface ISmsSender
    {
        Task SendSmsAsync(string number, string message);
    }
}
