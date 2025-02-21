using Microsoft.AspNetCore.Identity;

namespace ProductManagementBCSTO18.Models.Entities
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
    }
}
