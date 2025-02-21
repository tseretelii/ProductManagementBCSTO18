using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProductManagementBCSTO18.Interfaces;
using ProductManagementBCSTO18.Models;
using ProductManagementBCSTO18.Models.VM.Account;

namespace ProductManagementBCSTO18.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccauntService _accauntService;
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(IAccauntService accauntService, ApplicationDbContext context, RoleManager<IdentityRole> roleManager)
        {
            _accauntService = accauntService;
            _context = context;
            _roleManager = roleManager;
        }

        [HttpGet] 
        public async Task<IActionResult> Register()
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
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            var result = new IdentityResult();
            if (ModelState.IsValid)
            {
                result = await _accauntService.Register(model);
                if (result.Succeeded)
                    return RedirectToAction("Index", "Home");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(model);

            // შემდეგ ლექციაზე ავხსნათ აიდენთითის იუზერის ველები დეტალურად(SecurityStamp)
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _accauntService.Logout();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _accauntService.Login(model);
                if (result.Succeeded)
                    return RedirectToAction("Index", "Home");
                if (result.IsLockedOut)
                {
                    var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == model.Email);
                    ModelState.AddModelError(string.Empty, $"You are locked out untill {user.LockoutEnd.Value.AddHours(4)}");
                }
            }
            ModelState.AddModelError(string.Empty, "Invalid login attempt");

            return View(model);
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
