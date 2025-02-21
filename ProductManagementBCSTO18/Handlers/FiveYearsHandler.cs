using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using ProductManagementBCSTO18.Models;
using ProductManagementBCSTO18.Requiremenets;
using System.Security.Claims;

namespace ProductManagementBCSTO18.Handlers
{
    public class FiveYearsHandler : AuthorizationHandler<FiveYearsRequirement>
    {
        private readonly ApplicationDbContext _context;

        public FiveYearsHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        protected override async Task<Task> HandleRequirementAsync(AuthorizationHandlerContext context, FiveYearsRequirement requirement)
        {
            var yearsWorkedClaim = context.User.FindFirst("FiveYearsWorked");

            var daysWorked = 0;

            var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier) ??
                context.User.FindFirst("UserId");
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userIdClaim.Value.ToString());

            if(user != null) 
                daysWorked = (DateTime.Now - user.CreateDate).Days;

            if (yearsWorkedClaim != null)
            {
                if(daysWorked >= requirement.MinimumYears * 365)
                {
                    context.Succeed(requirement);
                }
            } 

            return Task.CompletedTask;
        }
    }
}
