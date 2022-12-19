using Microsoft.AspNetCore.Identity;

namespace WatchShop_ASPNETCOREMVC.Core.Repositories
{
    public interface IRoleRepository
    {
        ICollection<IdentityRole> GetRoles();
    }
}
