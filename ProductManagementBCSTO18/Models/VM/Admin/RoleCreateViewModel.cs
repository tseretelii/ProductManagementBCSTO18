using System.ComponentModel.DataAnnotations;

namespace ProductManagementBCSTO18.Models.VM.Admin
{
    public class RoleCreateViewModel
    {
        [Required(ErrorMessage = "Requiered")]
        public required string RoleName { get; set; }
    }
}
