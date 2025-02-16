namespace Bazingo_Core.Models.Identity
{
    public interface ILoginModel
    {
        string Username { get; set; }
        string Email { get; set; }
        string Password { get; set; }
        bool RememberMe { get; set; }
    }
}
