using System.ComponentModel.DataAnnotations;

namespace RestaurantSite.Models
{
    public class OrderItem
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int OrderId { get; set; }
        
        [Required]
        public int MenuItemId { get; set; }
        
        [Display(Name = "Название")]
        public string MenuItemName { get; set; } = "";
        
        [Display(Name = "Цена")]
        public decimal Price { get; set; }
        
        // Связи
        public Order? Order { get; set; }
        public MenuItem? MenuItem { get; set; }
    }
}