using Arekat.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace Arekat.Server
{
    public class BaseContext(DbContextOptions<BaseContext> options,IConfiguration config) :DbContext(options)
    {
        private readonly IConfiguration _config = config;

        public DbSet<BaseUser> Users { get; set; }
        public DbSet<Song> Songs { get; set; }
        public DbSet<Artist> Artists { get; set; }
        public DbSet<Chart> Charts { get; set; }
        public DbSet<ChartHistory> ChartsHistory { get; set; }
        public DbSet<Notice> Notices { get; set; }
        public DbSet<StoreUid> StoreUids { get; set; }
        public DbSet<EmailVerifyKey> EmailVerifyKeys { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@$"
                Data Source={_config.GetValue<string>("MainDatabase:Server")},{_config.GetValue<int>("MainDatabase:Port")};
                User ID={_config.GetValue<string>("MainDatabase:UserId")};
                Password={_config.GetValue<string>("MainDatabase:Password")};
                Database={_config.GetValue<string>("MainDatabase:Database")};
                Connect Timeout=30;
                Encrypt=True;   
                Trust Server Certificate=True;
                Application Intent=ReadWrite;
                Multi Subnet Failover=False");
        }
    }
}
