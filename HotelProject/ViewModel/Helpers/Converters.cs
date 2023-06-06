namespace HotelProject.ViewModel.Helpers
{
    static class Converters
    {
        public static string BoolToTable(bool source)
        {
            if (source == true)
                return "-1";
            else return "0";
        }
    }
}
