using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjectRempakTani.Models;

namespace ProjectRempakTani.Data
{
    public class AppDbContext : IdentityDbContext<Users>
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Kategori> Kategoris { get; set; }
        public DbSet<Produk> Produks { get; set; }
        public DbSet<Transaksi> Transaksis { get; set; }
        public DbSet<DetailTransaksi> DetailTransaksis { get; set; }

    }
}
