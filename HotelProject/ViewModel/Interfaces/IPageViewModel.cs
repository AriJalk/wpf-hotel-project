namespace HotelProject.ViewModel
{
    public interface IPageViewModel
    {
        string Name { get; }

        bool ShowButton { get; set; }

        void Refresh();
        void Dispose();
    }
}
