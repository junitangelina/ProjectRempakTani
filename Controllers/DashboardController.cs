using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization; // ← tambahin ini di bagian atas

namespace ProjectRempakTani.Controllers
{
    [Authorize] // ← tambahkan di atas class
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
