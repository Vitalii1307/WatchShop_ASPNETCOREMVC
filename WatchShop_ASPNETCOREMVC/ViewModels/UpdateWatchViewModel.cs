using System.ComponentModel.DataAnnotations;

namespace WatchShop_ASPNETCOREMVC.ViewModels
{
    public class UpdateWatchViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name isn't specified")]
        public string Name { get; set; }
        public string ImageName { get; set; }
        public IFormFile ImageFile { get; set; }


        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Price must be more than 0")]
        public int Price { get; set; }
    }
}
