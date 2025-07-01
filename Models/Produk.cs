using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ProjectRempakTani.Models
{
    public class Produk
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Nama Produk")]
        public string NamaProduk { get; set; }

        [Required]
        public decimal Harga { get; set; }

        public int Stok { get; set; }

        public string? Deskripsi { get; set; }

        public int KategoriId { get; set; }
        [ValidateNever] // ✅ Tambahkan ini agar tidak divalidasi saat POST
        public Kategori Kategori { get; set; }

        public ICollection<DetailTransaksi> DetailTransaksis { get; set; } = new List<DetailTransaksi>();

    }
}
