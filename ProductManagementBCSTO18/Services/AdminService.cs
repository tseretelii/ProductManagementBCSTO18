using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProductManagementBCSTO18.Interfaces;
using ProductManagementBCSTO18.Models;
using ProductManagementBCSTO18.Models.Entities;
using ProductManagementBCSTO18.Models.VM.Admin;

namespace ProductManagementBCSTO18.Services
{
    public class AdminService : IAdminService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        public AdminService(ApplicationDbContext context, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
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
    }
}
