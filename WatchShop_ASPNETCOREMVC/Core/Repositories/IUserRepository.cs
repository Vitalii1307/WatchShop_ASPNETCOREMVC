using WatchShop_ASPNETCOREMVC.Data;

namespace WatchShop_ASPNETCOREMVC.Core.Repositories
{
    public interface IUserRepository
    {
        ICollection<ApplicationUser> GetUsers();
        ApplicationUser GetUser(string id);

        ApplicationUser UpdateUser(ApplicationUser user);
    }
}
