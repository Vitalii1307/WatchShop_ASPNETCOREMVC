using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WatchShop_ASPNETCOREMVC.ViewModels
{
    public class AddWatchViewModel
    {
        [Required(ErrorMessage = "Name isn't specified")]
        public string Name { get; set; }
        public string ImageName { get; set; }
        public IFormFile ImageFile { get; set; }
        [Remote(action: "CheckPrice", controller: "Shop", ErrorMessage = "Price must be more than 0!")]
        public int Price { get; set; }
        [Required(ErrorMessage = "Company isn't specified")]
        public string CompanyName { get; set; }
    }
}
