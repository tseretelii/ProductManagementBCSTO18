using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProductManagementBCSTO18.Interfaces;
using ProductManagementBCSTO18.Models.VM.Account;
using ProductManagementBCSTO18.Models.VM.Admin;

namespace ProductManagementBCSTO18.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;
        private readonly RoleManager<IdentityRole> _roleManager;


        public AdminController(IAdminService adminService, RoleManager<IdentityRole> roleManager)
        {
            _adminService = adminService;
            _roleManager = roleManager;
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
        [HttpGet]
        public IActionResult CreateUser()
        {
            RegisterViewModel model = new RegisterViewModel()
            {
                RoleList = _roleManager.Roles.Select(x => x.Name).Select(x =>
                new SelectListItem
                {
                    Text = x,
                    Value = x
                })
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> CreateUser(RegisterViewModel model)
        {
            await _adminService.CreateUser(model);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> EditUser(string Id)
        {
            var result = await _adminService.EditUser(Id);
            return View(result);
        }
        [HttpPost]
        public async Task<IActionResult> EditUser(string Id, RegisterViewModel model)
        {
            await _adminService.EditUser(model, Id);
            return RedirectToAction("Index");
        }
    }
}
