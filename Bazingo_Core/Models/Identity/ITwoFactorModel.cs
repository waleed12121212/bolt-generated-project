namespace Bazingo_Core.Models.Identity
{
    public interface ITwoFactorModel
    {
        string UserId { get; set; }
        string Code { get; set; }
        bool RememberDevice { get; set; }
    }
}
