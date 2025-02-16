namespace Bazingo_Core.Models.Identity
{
    public interface IRegisterModel
    {
        string Username { get; set; }
        string Email { get; set; }
        string Password { get; set; }
        string ConfirmPassword { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string PhoneNumber { get; set; }
    }
}
