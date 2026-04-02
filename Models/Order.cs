using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantSite.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [Display(Name = "Номер стола")]
        public int TableNumber { get; set; }
        
        [Display(Name = "Сумма")]
        public decimal TotalAmount { get; set; }
        
        [Display(Name = "Время заказа")]
        public DateTime OrderTime { get; set; }
        
        [Display(Name = "Статус")]
        public bool IsPaid { get; set; }
        
        public int? EmployeeId { get; set; }
        
        [Display(Name = "Официант")]
        public string? EmployeeName { get; set; }
        
        // Связи
        public Employee? Employee { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}