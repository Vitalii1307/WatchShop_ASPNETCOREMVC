namespace WatchShop_ASPNETCOREMVC.Models.Domain
{
    public class Company
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public List<Watch> Watches { get; set; } = null;
    }
}
