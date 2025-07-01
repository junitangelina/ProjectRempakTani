using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectRempakTani.Data;
using ProjectRempakTani.Models;

public class ProdukController : Controller
{
    private readonly AppDbContext _context;

    public ProdukController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var data = _context.Produks.Include(p => p.Kategori).ToList();
        return View(data);
    }

    public IActionResult Create()
    {
        ViewBag.KategoriId = new SelectList(_context.Kategoris, "Id", "NamaKategori");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Produk produk)
    {
        Console.WriteLine("== POST Produk ==");
        Console.WriteLine($"NamaProduk: {produk.NamaProduk}");
        Console.WriteLine($"Harga: {produk.Harga}");
        Console.WriteLine($"Stok: {produk.Stok}");
        Console.WriteLine($"Deskripsi: {produk.Deskripsi}");
        Console.WriteLine($"KategoriId: {produk.KategoriId}");

        if (!ModelState.IsValid)
        {
            Console.WriteLine("ModelState INVALID");
            foreach (var key in ModelState.Keys)
            {
                var state = ModelState[key];
                foreach (var error in state.Errors)
                {
                    Console.WriteLine($"KEY: {key} - ERROR: {error.ErrorMessage}");
                }
            }

            ViewBag.KategoriId = new SelectList(_context.Kategoris, "Id", "NamaKategori");
            return View(produk);
        }
        Console.WriteLine("ModelState VALID");
        _context.Produks.Add(produk);
        _context.SaveChanges();
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Edit(int? id)
    {
        if (id == null) return NotFound();

        var produk = _context.Produks.Find(id);
        if (produk == null) return NotFound();

        ViewBag.KategoriId = new SelectList(_context.Kategoris, "Id", "NamaKategori", produk.KategoriId);
        return View(produk);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, Produk produk)
    {
        if (id != produk.Id) return NotFound();

        if (!ModelState.IsValid)
        {
            ViewBag.KategoriId = new SelectList(_context.Kategoris, "Id", "NamaKategori", produk.KategoriId);
            return View(produk);
        }

        _context.Update(produk);
        _context.SaveChanges();
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Delete(int? id)
    {
        if (id == null) return NotFound();

        var produk = _context.Produks.Include(p => p.Kategori).FirstOrDefault(p => p.Id == id);
        if (produk == null) return NotFound();

        return View(produk);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        var produk = _context.Produks.Find(id);
        if (produk != null)
        {
            _context.Produks.Remove(produk);
            _context.SaveChanges();
        }

        return RedirectToAction(nameof(Index));
    }

    public IActionResult Details(int? id)
    {
        if (id == null) return NotFound();

        var produk = _context.Produks.Include(p => p.Kategori).FirstOrDefault(p => p.Id == id);
        if (produk == null) return NotFound();

        return View(produk);
    }
}
