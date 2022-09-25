using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace iWorkDemo.Pages
{
    public class EmployeeEditModel : PageModel
    {
        public int Id { get; set; }
        public void OnGet(int id)
        {
            Console.WriteLine("Incoming ID: " + id);
            this.Id = id;
        }
    }
}
