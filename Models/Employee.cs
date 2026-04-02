using System.ComponentModel.DataAnnotations;

namespace RestaurantSite.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Введите имя сотрудника")]
        [Display(Name = "Имя")]
        public string Name { get; set; } = "";
        
        [Required(ErrorMessage = "Введите должность")]
        [Display(Name = "Должность")]
        public string Position { get; set; } = "";
    }
}