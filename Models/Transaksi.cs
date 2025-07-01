using System.ComponentModel.DataAnnotations;

namespace ProjectRempakTani.Models
{
    public class Transaksi
    {
        public int Id { get; set; }

        [Required]
        public DateTime TanggalTransaksi { get; set; }

        public string? Keterangan { get; set; }

        public ICollection<DetailTransaksi> DetailTransaksis { get; set; }
    }
}
