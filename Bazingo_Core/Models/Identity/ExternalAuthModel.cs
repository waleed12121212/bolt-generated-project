namespace Bazingo_Core.Models.Identity
{
    public class ExternalAuthModel
    {
        public string Provider { get; set; }
        public string AccessToken { get; set; }
        public string Email { get; set; }
    }
}
