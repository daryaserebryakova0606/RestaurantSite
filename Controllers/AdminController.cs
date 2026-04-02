using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantSite.Data;
using RestaurantSite.Models;

namespace RestaurantSite.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        
        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<IActionResult> Index()
        {
            var activeOrders = await _context.Orders
                .Where(o => !o.IsPaid)
                .ToListAsync();
            
            var tables = new List<TableViewModel>();
            for (int i = 1; i <= 5; i++)
            {
                var tableOrder = activeOrders.FirstOrDefault(o => o.TableNumber == i);
                tables.Add(new TableViewModel
                {
                    TableNumber = i,
                    IsOccupied = tableOrder != null,
                    OrderId = tableOrder?.Id
                });
            }
            
            ViewBag.Tables = tables;
            
            var paidOrders = await _context.Orders
                .Where(o => o.OrderTime.Date == DateTime.Today && o.IsPaid)
                .ToListAsync();
            
            var todayRevenue = paidOrders.Sum(o => o.TotalAmount);
            ViewBag.TodayRevenue = todayRevenue;
            
            return View();
        }
        
        public async Task<IActionResult> Menu()
        {
            var items = await _context.MenuItems.ToListAsync();
            return View(items);
        }
        
        [HttpPost]
        public async Task<IActionResult> AddMenuItem(MenuItem item)
        {
            if (ModelState.IsValid)
            {
                _context.MenuItems.Add(item);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Menu");
        }
        
        [HttpPost]
        public async Task<IActionResult> DeleteMenuItem(int id)
        {
            var item = await _context.MenuItems.FindAsync(id);
            if (item != null)
            {
                _context.MenuItems.Remove(item);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Menu");
        }
        
        public async Task<IActionResult> Employees()
        {
            var employees = await _context.Employees.ToListAsync();
            return View(employees);
        }
        
        [HttpPost]
        public async Task<IActionResult> AddEmployee(Employee emp)
        {
            if (ModelState.IsValid)
            {
                _context.Employees.Add(emp);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Employees");
        }
        
        [HttpPost]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var emp = await _context.Employees.FindAsync(id);
            if (emp != null)
            {
                _context.Employees.Remove(emp);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Employees");
        }
        
        public async Task<IActionResult> Shifts()
        {
            var shifts = await _context.Shifts
                .Include(s => s.Employee)
                .OrderByDescending(s => s.StartTime)
                .ToListAsync();
                
            var employees = await _context.Employees.ToListAsync();
            
            ViewBag.Employees = employees;
            return View(shifts);
        }
        
        [HttpPost]
        public async Task<IActionResult> StartShift(int employeeId)
        {
            var employee = await _context.Employees.FindAsync(employeeId);
            
            if (employee != null)
            {
                var shift = new Shift
                {
                    EmployeeId = employee.Id,
                    EmployeeName = employee.Name,
                    StartTime = DateTime.Now
                };
                
                _context.Shifts.Add(shift);
                await _context.SaveChangesAsync();
            }
            
            return RedirectToAction("Shifts");
        }
        
        [HttpPost]
        public async Task<IActionResult> EndShift(int id)
        {
            var shift = await _context.Shifts.FindAsync(id);
            
            if (shift != null && shift.IsActive)
            {
                shift.EndTime = DateTime.Now;
                await _context.SaveChangesAsync();
            }
            
            return RedirectToAction("Shifts");
        }
        
        public async Task<IActionResult> Orders()
        {
            var orders = await _context.Orders
                .Include(o => o.Employee)
                .Include(o => o.OrderItems)
                .OrderByDescending(o => o.OrderTime)
                .ToListAsync();
                
            var employees = await _context.Employees.ToListAsync();
            var menuItems = await _context.MenuItems.ToListAsync();
            
            ViewBag.Employees = employees;
            ViewBag.MenuItems = menuItems;
            ViewBag.Tables = Enumerable.Range(1, 5).ToList();
            
            return View(orders);
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateOrder(int tableNumber, List<int> menuItemIds, int? employeeId)
        {
            if (menuItemIds == null || !menuItemIds.Any())
            {
                return RedirectToAction("Orders");
            }
            
            var selectedItems = await _context.MenuItems
                .Where(m => menuItemIds.Contains(m.Id))
                .ToListAsync();
                
            var total = selectedItems.Sum(m => m.Price);
            
            var employee = employeeId.HasValue 
                ? await _context.Employees.FindAsync(employeeId.Value)
                : null;
            
            var order = new Order
            {
                TableNumber = tableNumber,
                TotalAmount = total,
                OrderTime = DateTime.Now,
                IsPaid = false,
                EmployeeId = employee?.Id,
                EmployeeName = employee?.Name
            };
            
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            
            foreach (var item in selectedItems)
            {
                var orderItem = new OrderItem
                {
                    OrderId = order.Id,
                    MenuItemId = item.Id,
                    MenuItemName = item.Name,
                    Price = item.Price
                };
                _context.OrderItems.Add(orderItem);
            }
            
            await _context.SaveChangesAsync();
            
            return RedirectToAction("Orders");
        }
        
        [HttpPost]
        public async Task<IActionResult> PayOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            
            if (order != null && !order.IsPaid)
            {
                order.IsPaid = true;
                await _context.SaveChangesAsync();
            }
            
            return RedirectToAction("Orders");
        }
    }
}