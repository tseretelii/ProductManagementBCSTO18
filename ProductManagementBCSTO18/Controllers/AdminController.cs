using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductManagementBCSTO18.Interfaces;
using ProductManagementBCSTO18.Models.VM.Admin;

namespace ProductManagementBCSTO18.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _adminService.Index();
            return View(users);
        }
        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateRole(RoleCreateViewModel model)
        {
            await _adminService.CreateRole(model);

            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> AllRoles()
        {
            var roles = await _adminService.AllRoles();
            return View(roles);
        }
        [HttpGet]
        public async Task<IActionResult> EditRole(string Id)
        {
            var role = await _adminService.EditRole(Id);
            return View(role);
        }
        [HttpPost]
        public async Task<IActionResult> EditRole(RolesViewModel model, string Id)
        {
            await _adminService.EditRole(model,Id);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> DeleteRole(string Id)
        {
            await _adminService.DeleteRole(Id);
            return RedirectToAction("Index");
        }
    }
}
