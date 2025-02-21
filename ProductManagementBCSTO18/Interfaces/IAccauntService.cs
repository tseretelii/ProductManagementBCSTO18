using Microsoft.AspNetCore.Identity;
using ProductManagementBCSTO18.Models.VM.Account;

namespace ProductManagementBCSTO18.Interfaces
{
    public interface IAccauntService
    {
        Task<IdentityResult> Register(RegisterViewModel model);
        Task<SignInResult> Login(LoginViewModel model);
        Task Logout();
    }
}
