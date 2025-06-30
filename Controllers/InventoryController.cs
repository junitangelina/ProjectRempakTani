using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ProjectRempakTani.Models;
using System.Collections.Generic;
using System.Linq;

namespace ProjectRempakTani.Controllers
{
    [Authorize] // ⬅️ Ini memastikan hanya user login yang bisa akses
    public class InventoryController : Controller
    {
        private static List<Inventory> _inventories = new List<Inventory>
        {
            new Inventory { Id = 1, NamaBarang = "Cangkul", Kategori = "Alat Tani", Jumlah = 10, Lokasi = "Gudang 1" },
            new Inventory { Id = 2, NamaBarang = "Benih Padi", Kategori = "Bibit", Jumlah = 25, Lokasi = "Gudang 2" }
        };

        public IActionResult Index()
        {
            return View(_inventories);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Inventory model)
        {
            if (ModelState.IsValid)
            {
                model.Id = _inventories.Any() ? _inventories.Max(x => x.Id) + 1 : 1;
                _inventories.Add(model);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var item = _inventories.FirstOrDefault(x => x.Id == id);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost]
        public IActionResult Edit(Inventory model)
        {
            var item = _inventories.FirstOrDefault(x => x.Id == model.Id);
            if (item == null) return NotFound();

            if (ModelState.IsValid)
            {
                item.NamaBarang = model.NamaBarang;
                item.Kategori = model.Kategori;
                item.Jumlah = model.Jumlah;
                item.Lokasi = model.Lokasi;

                return RedirectToAction("Index");
            }

            return View(model);
        }

        public IActionResult Delete(int id)
        {
            var item = _inventories.FirstOrDefault(x => x.Id == id);
            if (item != null)
            {
                _inventories.Remove(item);
            }
            return RedirectToAction("Index");
        }
    }
}
