using Microsoft.AspNetCore.Identity;
using WatchShop_ASPNETCOREMVC.Core.Repositories;
using WatchShop_ASPNETCOREMVC.Data;

namespace WatchShop_ASPNETCOREMVC.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly MVCWatchesDbContext _context;

        public RoleRepository(MVCWatchesDbContext context)
        {
            _context = context;
        }
        public ICollection<IdentityRole> GetRoles()
        {
            return _context.Roles.ToList();
        }
    }
}
