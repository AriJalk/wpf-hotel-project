namespace HotelProject.Model.Interfaces
{
    /// <summary>
    /// Implement: use static int as global object count parameter
    /// </summary>
    interface IIncremented
    {
        /// <summary>
        /// Return a static counter of number of objects exist of class
        /// </summary>
        int IdCount { get; set; }
    }
}
