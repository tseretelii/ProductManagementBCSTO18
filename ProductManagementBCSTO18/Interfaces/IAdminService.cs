using ProductManagementBCSTO18.Models.Entities;
using ProductManagementBCSTO18.Models.VM.Admin;

namespace ProductManagementBCSTO18.Interfaces
{
    public interface IAdminService
    {
        Task<List<GetAllUsersViewModel>> Index();
        Task CreateRole(RoleCreateViewModel model);
        Task<List<RolesViewModel>> AllRoles();
        Task<RolesViewModel> EditRole(string Id);
        Task EditRole(RolesViewModel model, string Id);
        Task DeleteRole(string Id);
    }
}
