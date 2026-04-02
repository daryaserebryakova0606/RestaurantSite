namespace RestaurantSite.Models
{
    public class TableViewModel
    {
        public int TableNumber { get; set; }
        public bool IsOccupied { get; set; }
        public int? OrderId { get; set; }
    }
}