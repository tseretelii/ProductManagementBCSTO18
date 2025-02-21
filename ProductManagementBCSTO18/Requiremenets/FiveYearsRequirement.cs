using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ProductManagementBCSTO18.Requiremenets
{
    public class FiveYearsRequirement : IAuthorizationRequirement
    {
        public int MinimumYears { get; set; }

        public FiveYearsRequirement(int minimumYears)
        {
            MinimumYears = minimumYears;
        }
    }
}
