using System.ComponentModel.DataAnnotations;

namespace RestaurantSite.Models
{
    public class Shift
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int EmployeeId { get; set; }
        
        [Display(Name = "Сотрудник")]
        public string EmployeeName { get; set; } = "";
        
        [Display(Name = "Начало смены")]
        public DateTime StartTime { get; set; }
        
        [Display(Name = "Конец смены")]
        public DateTime? EndTime { get; set; }
        
        public bool IsActive => EndTime == null;
        
        // Связь с сотрудником
        public Employee? Employee { get; set; }
    }
}