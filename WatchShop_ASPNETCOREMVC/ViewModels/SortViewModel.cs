using WatchShop_ASPNETCOREMVC.Models;

namespace WatchShop_ASPNETCOREMVC.ViewModels
{
    public class SortViewModel
    {
        public SortState NameSort { get; }
        public SortState PriceSort { get; }
        public SortState CompanyNameSort { get; }
        public SortState Current { get; }

        public SortViewModel(SortState sortOrder)
        {
            NameSort = sortOrder == SortState.NameAsc ? SortState.NameDesc : SortState.NameAsc;
            PriceSort = sortOrder == SortState.PriceAsc ? SortState.PriceDesc : SortState.PriceAsc;
            CompanyNameSort = sortOrder == SortState.CompanyNameAsc ? SortState.CompanyNameDesc : SortState.CompanyNameAsc;
            Current = sortOrder;
        }
    }
}

