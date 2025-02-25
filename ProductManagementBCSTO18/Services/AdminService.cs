using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProductManagementBCSTO18.Interfaces;
using ProductManagementBCSTO18.Models;
using ProductManagementBCSTO18.Models.Entities;
using ProductManagementBCSTO18.Models.VM.Account;
using ProductManagementBCSTO18.Models.VM.Admin;

namespace ProductManagementBCSTO18.Services
{
    public class AdminService : IAdminService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AdminService(ApplicationDbContext context, UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public async Task<List<GetAllUsersViewModel>> Index()
        {
            var userList = await _context.Users.ToListAsync();

            var userViewList = new List<GetAllUsersViewModel>();

            //userList.ForEach
            //    (
            //    async x => userViewList.Add
            //        (
            //            new GetAllUsersViewModel()
            //            {
            //                FirstName = x.FirstName,
            //                LastName = x.LastName,
            //                CreateDate = x.CreateDate,
            //                Roles = (List<string>)await _userManager.GetRolesAsync(x)
            //            }
            //        )

            //    );

            foreach (var user in userList)
            {
                var userView = new GetAllUsersViewModel()
                {
                    Id = user.Id,
                    FirstName = user.FirstName, 
                    LastName = user.LastName,
                    CreateDate = user.CreateDate,
                    Roles = (List<string>)await _userManager.GetRolesAsync(user),
                };
                userViewList.Add(userView);
            }
            return userViewList;
        }

        public async Task CreateRole(RoleCreateViewModel model)
        {
            IdentityRole identityRole = new IdentityRole(model.RoleName);
            identityRole.NormalizedName = model.RoleName.ToUpper();
            await _context.Roles.AddAsync(identityRole);
            await _context.SaveChangesAsync();
        }
        public async Task<List<RolesViewModel>> AllRoles()
        {
            var roles = await _context.Roles.ToListAsync();

            var allRoles = new List<RolesViewModel>();

            foreach (var role in roles)
            {
                allRoles.Add(new RolesViewModel() { Name = role.Name, Id = role.Id});
            }

            return allRoles;
        }
        public async Task<RolesViewModel> EditRole(string Id)
        {
            var role = await _context.Roles.FirstOrDefaultAsync(x => x.Id == Id);

            return new RolesViewModel() { Name = role.Name, Id = role.Id };
        }
        public async Task EditRole(RolesViewModel model, string Id)
        {
            var role = await _context.Roles.FirstOrDefaultAsync(x => x.Id == Id);

            role.Name = model.Name;
            role.NormalizedName = model.Name.ToUpper();

            _context.Roles.Update(role);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteRole(string Id)
        {
            var role = _context.Roles.FirstOrDefault(x => x.Id == Id);

            _context.Roles.Remove(role);

            await _context.SaveChangesAsync();
        }

        public async Task CreateUser(RegisterViewModel model)
        {
            var user = new User()
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                if (model.RoleSelected != null)
                    await _userManager.AddToRoleAsync(user, model.RoleSelected);
                else
                    await _userManager.AddToRoleAsync(user, "User");
                
            }
        }

        public async Task<RegisterViewModel> EditUser(string Id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == Id);

            var userRoles = await _userManager.GetRolesAsync(user);

            var roles = await _roleManager.Roles.ToListAsync();

            var model = new RegisterViewModel()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                RoleList = _roleManager.Roles.Select(x => x.Name).Select(x =>
                new SelectListItem
                {
                    Text = x,
                    Value = x
                })
            };
            return model;
        }

        //public async Task EditUser(string Id, RegisterViewModel model)
        //{
            
        //}

        public async Task EditUser(RegisterViewModel model, string Id)
        {
            var oldUser = await _context.Users.FirstOrDefaultAsync(x => x.Id == Id);

            await _userManager.DeleteAsync(oldUser);

            var user = new User()
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                if (model.RoleSelected != null)
                    await _userManager.AddToRoleAsync(user, model.RoleSelected);
                else
                    await _userManager.AddToRoleAsync(user, "User");

            }
        }

        //public Task EditUser(RegisterViewModel model, string Id)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
