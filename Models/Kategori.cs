using System.ComponentModel.DataAnnotations;

namespace ProjectRempakTani.Models
{
    public class Kategori
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Nama Kategori")]
        public string NamaKategori { get; set; }

        public ICollection<Produk> Produks { get; set; } = new List<Produk>();
    }
}
