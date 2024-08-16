using Arekat.Server.Entities;
using Microsoft.EntityFrameworkCore;

namespace Arekat.Server
{
    public class BaseContext(DbContextOptions options):DbContext(options)
    {
        public DbSet<BaseMemberUser> Users { get; set; }

        public DbSet<ChartInfo> Charts { get; set; }
        public DbSet<StoreUid> Uids { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer($@"Server=(localdb)\");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
