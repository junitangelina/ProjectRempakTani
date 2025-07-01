namespace ProjectRempakTani.ViewModels
{
    public class TransaksiItemViewModel
    {
        public int ProdukId { get; set; }
        public string? NamaProduk { get; set; }
        public decimal HargaSatuan { get; set; }
        public int Jumlah { get; set; }

        public decimal Subtotal => HargaSatuan * Jumlah;
    }
    public class TransaksiViewModel
    {
        public DateTime TanggalTransaksi { get; set; } = DateTime.Now;
        public string? Keterangan { get; set; }
        public List<TransaksiItemViewModel> Items { get; set; } = new List<TransaksiItemViewModel>();
    }
}
