using Microsoft.AspNetCore.Mvc.Rendering;
using WatchShop_ASPNETCOREMVC.Models.Domain;

namespace WatchShop_ASPNETCOREMVC.ViewModels
{
    public class FilterViewModel
    {
        public FilterViewModel(List<Company> companies, int company, string name)
        {
            // устанавливаем начальный элемент, который позволит выбрать всех
            companies.Insert(0, new Company { Name = "All", Id = 0 });
            Companies = new SelectList(companies, "Id", "Name", company);
            SelectedCompany = company;
            SelectedName = name;
        }
        public SelectList Companies { get; }
        public int SelectedCompany { get; }
        public string SelectedName { get; }
    }
}
