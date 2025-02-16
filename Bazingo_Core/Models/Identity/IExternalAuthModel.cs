namespace Bazingo_Core.Models.Identity
{
    public interface IExternalAuthModel
    {
        string Provider { get; set; }
        string AccessToken { get; set; }
        string Token { get; set; }
        string Email { get; set; }
    }
}
