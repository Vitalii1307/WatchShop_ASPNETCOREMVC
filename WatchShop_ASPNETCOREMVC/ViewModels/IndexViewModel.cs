using WatchShop_ASPNETCOREMVC.Models.Domain;

namespace WatchShop_ASPNETCOREMVC.ViewModels
{
    public class IndexViewModel
    {
        public IEnumerable<Watch> Watches { get; }
        public PageViewModel PageViewModel { get; }
        public FilterViewModel FilterViewModel { get; }
        public SortViewModel SortViewModel { get; }
        public IndexViewModel(IEnumerable<Watch> watches, PageViewModel pageViewModel,
            FilterViewModel filterViewModel, SortViewModel sortViewModel)
        {
            Watches = watches;
            PageViewModel = pageViewModel;
            FilterViewModel = filterViewModel;
            SortViewModel = sortViewModel;
        }
    }
}
