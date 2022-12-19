using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace WatchShop_ASPNETCOREMVC.Models.Domain
{
    public class Watch
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? ImageName { get; set; }
        public string CompanyName { get; set; }
        public int? CompanyId { get; set; }
        public Company? Company { get; set; }

        [NotMapped]
        [DisplayName("Upload File")]
        public IFormFile? ImageFile { get; set; }
        public int Price { get; set; }
    }
}
