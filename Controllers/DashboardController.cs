using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectRempakTani.Data;
using ProjectRempakTani.ViewModels;

namespace ProjectRempakTani.Controllers
{
    public class DashboardController : Controller
    {
        private readonly AppDbContext _context;

        public DashboardController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var produkFavorit = _context.DetailTransaksis
                .Include(d => d.Produk)
                .GroupBy(d => d.Produk.NamaProduk)
                .Select(g => new ProdukFavoritViewModel
                {
                    NamaProduk = g.Key,
                    TotalJumlahDibeli = g.Sum(x => x.Jumlah)
                })
                .OrderByDescending(p => p.TotalJumlahDibeli)
                .Take(5) // ambil top 5 produk
                .ToList();

            ViewBag.Labels = produkFavorit.Select(p => p.NamaProduk).ToList();
            ViewBag.Data = produkFavorit.Select(p => p.TotalJumlahDibeli).ToList();

            return View();
        }
    }
}
