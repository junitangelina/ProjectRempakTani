using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectRempakTani.Data;
using ProjectRempakTani.Models;
using ProjectRempakTani.ViewModels;

namespace ProjectRempakTani.Controllers
{
    public class TransaksiController : Controller
    {
        private readonly AppDbContext _context;

        public TransaksiController(AppDbContext context)
        {
            _context = context;
        }



        // GET: Transaksi/Create
        public IActionResult Create()
        {
            var viewModel = new TransaksiViewModel
            {
                ProdukList = _context.Produks.ToList(),
                Items = new List<TransaksiItemViewModel>
    {
        new TransaksiItemViewModel(),
    }
            };


            return View(viewModel);
        }

        // POST: Transaksi/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TransaksiViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var transaksi = new Transaksi
            {
                TanggalTransaksi = model.TanggalTransaksi,
                Keterangan = model.Keterangan
            };

            _context.Transaksis.Add(transaksi);
            _context.SaveChanges();

            foreach (var item in model.Items.Where(i => i.Jumlah > 0))
            {
                var detail = new DetailTransaksi
                {
                    TransaksiId = transaksi.Id,
                    ProdukId = item.ProdukId,
                    Jumlah = item.Jumlah,
                    HargaSatuan = item.HargaSatuan
                };

                _context.DetailTransaksis.Add(detail);

                // update stok produk
                var produk = _context.Produks.Find(item.ProdukId);
                if (produk != null)
                    produk.Stok -= item.Jumlah;
            }

            _context.SaveChanges();
            return RedirectToAction("Index", "Transaksi");
        }

        // GET: Transaksi/Index
        public IActionResult Index(string search)
        {
            var query = _context.Transaksis
                .Include(t => t.DetailTransaksis)
                .ThenInclude(d => d.Produk)
                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(t => t.Keterangan.Contains(search));
            }

            var data = query
                .OrderByDescending(t => t.TanggalTransaksi)
                .ToList();

            return View(data);
        }


        // GET: Transaksi/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null) return NotFound();

            var transaksi = _context.Transaksis
                .Include(t => t.DetailTransaksis)
                .ThenInclude(d => d.Produk)
                .FirstOrDefault(t => t.Id == id);

            if (transaksi == null) return NotFound();

            var viewModel = new TransaksiViewModel
            {
                TanggalTransaksi = transaksi.TanggalTransaksi,
                Keterangan = transaksi.Keterangan,
                Items = transaksi.DetailTransaksis.Select(dt => new TransaksiItemViewModel
                {
                    ProdukId = dt.ProdukId,
                    NamaProduk = dt.Produk.NamaProduk,
                    HargaSatuan = dt.HargaSatuan,
                    Jumlah = dt.Jumlah
                }).ToList()
            };

            ViewBag.AllProduks = _context.Produks.ToList();
            ViewBag.TransaksiId = transaksi.Id;
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, TransaksiViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var transaksi = _context.Transaksis
                .Include(t => t.DetailTransaksis)
                .FirstOrDefault(t => t.Id == id);

            if (transaksi == null) return NotFound();

            // Kembalikan stok
            foreach (var detail in transaksi.DetailTransaksis)
            {
                var produk = _context.Produks.Find(detail.ProdukId);
                if (produk != null)
                    produk.Stok += detail.Jumlah;
            }

            _context.DetailTransaksis.RemoveRange(transaksi.DetailTransaksis);
            transaksi.DetailTransaksis.Clear();

            transaksi.TanggalTransaksi = model.TanggalTransaksi;
            transaksi.Keterangan = model.Keterangan;

            foreach (var item in model.Items.Where(i => i.Jumlah > 0))
            {
                var produk = _context.Produks.Find(item.ProdukId);
                if (produk == null || produk.Stok < item.Jumlah)
                {
                    ModelState.AddModelError("", $"Stok tidak cukup untuk produk {item.NamaProduk}.");
                    return View(model);
                }

                produk.Stok -= item.Jumlah;

                transaksi.DetailTransaksis.Add(new DetailTransaksi
                {
                    ProdukId = item.ProdukId,
                    Jumlah = item.Jumlah,
                    HargaSatuan = produk.Harga
                });
            }

            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }


        // GET: Transaksi/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null) return NotFound();

            var transaksi = _context.Transaksis
                .Include(t => t.DetailTransaksis)
                .ThenInclude(d => d.Produk)
                .FirstOrDefault(t => t.Id == id);

            if (transaksi == null) return NotFound();

            return View(transaksi);
        }


        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var transaksi = _context.Transaksis
                .Include(t => t.DetailTransaksis)
                .FirstOrDefault(t => t.Id == id);

            if (transaksi != null)
            {
                // Kembalikan stok
                foreach (var detail in transaksi.DetailTransaksis)
                {
                    var produk = _context.Produks.Find(detail.ProdukId);
                    if (produk != null)
                        produk.Stok += detail.Jumlah;
                }

                _context.DetailTransaksis.RemoveRange(transaksi.DetailTransaksis);
                _context.Transaksis.Remove(transaksi);
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Transaksi/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null) return NotFound();

            var transaksi = _context.Transaksis
                .Include(t => t.DetailTransaksis)
                .ThenInclude(d => d.Produk)
                .FirstOrDefault(t => t.Id == id);

            if (transaksi == null) return NotFound();

            return View(transaksi);
        }

    }
}
