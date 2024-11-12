using Microsoft.AspNetCore.Identity;

namespace TemplateNetCoreApi.Core.Models
{
    public class User : IdentityUser<long> // model kế thừa từ IdentityUser
    {
        public string? FirstName { get; set; } // các property thêm khác
        public string? LastName { get; set; }
    }
}
