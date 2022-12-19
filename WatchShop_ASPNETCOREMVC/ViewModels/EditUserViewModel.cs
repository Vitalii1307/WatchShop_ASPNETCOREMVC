using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using WatchShop_ASPNETCOREMVC.Data;

namespace WatchShop_ASPNETCOREMVC.ViewModels
{
    public class EditUserViewModel
    {
        public ApplicationUser User { get; set; }

        public IList<SelectListItem> Roles { get; set; }
    }
}
