using iWorkDemo.dbo;
using iWorkDemo.model;
using Microsoft.AspNetCore.Mvc;

namespace iWorkDemo.controller
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Route("/v1/register")]
        public JsonResult register(IFormCollection formData)
        {
            Employee emp = new Employee();
            emp.email = formData["email"];
            emp.firstName = formData["firstName"];
            emp.lastName = formData["lastName"];
            emp.gender = formData["gender"];
            emp.password = util.Commons.getSha256Hash(formData["password"]);
            EmployeeDBO dbo = new EmployeeDBO();
            if (dbo.create(emp))
            {
                return Json(new {status = "success", data=emp });
            }
            return Json(new {status= "error", message="User not created!"});
        }

        [Route("/v1/login")]
        public IActionResult login(IFormCollection formData)
        {
            string email = formData["email"];
            string password = util.Commons.getSha256Hash(formData["password"]);

            Employee? emp = new EmployeeDBO().getByEmail(email);
            if (emp != null)
            {
                Console.WriteLine("Comparing hashes: " + emp.password + ":" + password);
                if(emp.password == password)
                {
                    return Json(new {status="success", data=emp});
                }
            }
            return Json(new {status="error", mesage="Login failed!"});
        }

        [Route("/v1/list")]
        public JsonResult listEmployees()
        {
            List<Employee> employees = new EmployeeDBO().getAll();
            if (employees.Count > 0)
            {
                return Json(new { status = "success", data = employees });
            }
            return Json(new { status = "error", message = "No Employees found!" });
        }

        [Route("/v1/get")]
        public JsonResult getEmployee(int id)
        {
            Employee? emp = new EmployeeDBO().getById((long)id);
            if(emp!= null)
            {
                return Json(new {status="success", data=emp});
            }
            return Json(new { status = "error", message = "Employee not found with id: " + id });
        }

        [Route("/v1/update")]
        public JsonResult update(IFormCollection form)
        {
            long id = long.Parse(form["id"]);
            var dao = new EmployeeDBO();
            Employee? emp = dao.getById((long)id);
            if (emp != null)
            {
                emp.email= form["email"];
                emp.firstName = form["firstName"];
                emp.lastName = form["lastName"];
                emp.gender = form["gender"];
                dao.update(emp);
                return Json(new { status = "success", data = emp });
            }
            return Json(new { status = "error", message = "Employee not found with id: " + id });
        }

        [Route("/v1/remove")]
        public JsonResult remove(int id)
        {
            var dao = new EmployeeDBO();
            Employee? emp = dao.getById((long)id);
            if (emp != null)
            {
                dao.remove(emp);
                return Json(new { status = "success", message="deleted"});
            }
            return Json(new { status = "error", message = "Employee not found with id: " + id });
        }

    }
}
