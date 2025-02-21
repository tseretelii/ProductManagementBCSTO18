namespace ProductManagementBCSTO18.Models.VM.Admin
{
    public class GetAllUsersViewModel
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public List<string> Roles { get; set; } = new List<string>();
    }
}
