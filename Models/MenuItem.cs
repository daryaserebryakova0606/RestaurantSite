using System.ComponentModel.DataAnnotations;

namespace RestaurantSite.Models
{
    public class MenuItem
    {
        [Key]
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Введите название блюда")]
        [Display(Name = "Название")]
        public string Name { get; set; } = "";
        
        [Required(ErrorMessage = "Введите цену")]
        [Range(0.01, 100000, ErrorMessage = "Цена должна быть больше 0")]
        [Display(Name = "Цена")]
        public decimal Price { get; set; }
        
        [Required(ErrorMessage = "Введите категорию")]
        [Display(Name = "Категория")]
        public string Category { get; set; } = "";
    }
}