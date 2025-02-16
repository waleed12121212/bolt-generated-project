using Bazingo_Core.Models.Identity;

namespace Bazingo_Core.Interfaces.Auth
{
    public interface ILoginModel
    {
        string Email { get; set; }
        string Password { get; set; }
        bool RememberMe { get; set; }
    }

    public interface IRegisterModel
    {
        string Email { get; set; }
        string Password { get; set; }
        string ConfirmPassword { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
    }

    public interface IExternalAuthModel
    {
        string Provider { get; set; }
        string Token { get; set; }
        string Email { get; set; }
    }
}
