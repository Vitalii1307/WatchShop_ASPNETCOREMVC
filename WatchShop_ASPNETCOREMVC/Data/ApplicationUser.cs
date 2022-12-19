using Microsoft.AspNetCore.Identity;

namespace WatchShop_ASPNETCOREMVC.Data
{
    public class ApplicationUser: IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}

public class ApplicationRole : IdentityRole
{
    
}
