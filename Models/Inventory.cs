using System.ComponentModel.DataAnnotations;

namespace ProjectRempakTani.Models
{
    public class Inventory
    {
        public int Id { get; set; }

        [Required]
        public string NamaBarang { get; set; }

        [Required]
        public string Kategori { get; set; }

        [Required]
        public int Jumlah { get; set; }

        [Required]
        public string Lokasi { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

    }
}
