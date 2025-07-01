using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectRempakTani.Models
{
    public class DetailTransaksi
    {
        public int Id { get; set; }

        public int TransaksiId { get; set; }
        public Transaksi Transaksi { get; set; }

        public int ProdukId { get; set; }
        public Produk Produk { get; set; }

        public int Jumlah { get; set; }

        public decimal HargaSatuan { get; set; }

        [NotMapped]
        public decimal Subtotal => Jumlah * HargaSatuan;
    }
}
