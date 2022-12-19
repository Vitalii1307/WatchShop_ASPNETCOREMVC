using WatchShop_ASPNETCOREMVC.Core.Repositories;
using WatchShop_ASPNETCOREMVC.Data;
using Microsoft.EntityFrameworkCore;

namespace WatchShop_ASPNETCOREMVC.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly MVCWatchesDbContext _context;
        public UserRepository(MVCWatchesDbContext context)
        {
            _context = context;
        }

        

        public ICollection<ApplicationUser> GetUsers()
        {
            return _context.Users.ToList();
        }

        public ApplicationUser GetUser(string id)
        {
            return _context.Users.FirstOrDefault(u => u.Id == id);
        }

        public ApplicationUser UpdateUser(ApplicationUser user)
        {
            _context.Update(user);
            _context.SaveChanges();

            return user;
        }
    }
}
