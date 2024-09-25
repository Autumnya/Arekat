using Microsoft.AspNetCore.Mvc;
using Arekat.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Bogus;
using System.ComponentModel.DataAnnotations;
using Bogus.DataSets;

namespace Arekat.Server.Controllers
{
    [ApiController]
    [Route("server-management")]
    //[Authorize("SuperAdmin")]
    public class ServerController : ControllerBase
    {
        private readonly BaseContext _context;
        private readonly ILogger<ServerController> _logger;
        private readonly IConfiguration _config;

        public enum GameClass
        {
            Arcaea = 0
        }

        //Enable email auto delete
        private bool _EnabledAutoDeleteEmail { get; set; } = false;
        //How long the email can exist
        private int _EmailExistTimeMinute { get; set; } = 240;
        //Delete emails every ? seconds
        private int _AutoDeleteEmailSecond { get; set; } = 120;

        private TimerCallback? _clearEmailTimerCallback;
        private Timer? _clearEmailTimer;

        public ServerController(BaseContext context, ILogger<ServerController> logger, IConfiguration config)
        {
            _context = context;
            _logger = logger;
            _config = config;
        }

        [HttpGet("config")]
        public ServerConfig GetServerConfig()
        {
            return new ServerConfig
            {
                Server = _config.GetValue<string>("MainDatabase:Server"),
                Port = _config.GetValue<string>("MainDatabase:Port"),
                Database = _config.GetValue<string>("MainDatabase:Database"),
                Username = _config.GetValue<string>("MainDatabase:Username"),
                Password = _config.GetValue<string>("MainDatabase:Password"),
            };
        }

        [HttpOptions("enable-auto-delete-email")]
        public IActionResult EnableAutoDeleteEmail(bool enable)
        {
            _EnabledAutoDeleteEmail = enable;
            if (_EnabledAutoDeleteEmail)
            {
                _clearEmailTimerCallback = new TimerCallback(async (Object) =>
                {
                    await RemoveEmailOutOfTimeAsync();
                });
                _clearEmailTimer = new Timer(_clearEmailTimerCallback, null, 0, _AutoDeleteEmailSecond);
            }
            else
            {
                _clearEmailTimerCallback = null;
            }
            return Ok($"EnableAutoDeleteEmail set to {enable.ToString()}");
        }

        [HttpPost("add-random-users")]
        public async Task<IActionResult> AddRandomUsers([FromQuery] int amount)
        {
            if (amount <= 0)
                return BadRequest("No param");
            int currentMaxId = 100000;
            if (await _context.Users.AnyAsync())
            {
                currentMaxId = _context.Users.Max(x => x.Id);
            }
            var newFaker = new Faker<BaseUser>()
                .RuleFor(u => u.Id, f => currentMaxId + f.IndexFaker + 1)
                .RuleFor(u => u.Name, f => f.Internet.DomainName())
                .RuleFor(u => u.Email, f => f.Internet.Email())
                .RuleFor(u => u.Password, f => f.Internet.Password());
            ICollection<BaseUser> newUsers = newFaker.Generate(amount);
            foreach (var user in newUsers)
            {
                user.Guid = Guid.NewGuid();
                user.RegistrationDate = DateTime.Now;
            }
            await _context.BulkInsertAsync(newUsers);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("add-random-artists")]
        public async Task<IActionResult> AddRandomArtists([FromQuery] int amount)
        {
            if (amount <= 0)
                return BadRequest("No param");
            var newFaker = new Faker<Artist>()
                .RuleFor(a => a.Name, f => f.Internet.UserName());
            ICollection<Artist> newArtists = newFaker.Generate(amount);
            await _context.BulkInsertAsync(newArtists);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("add-random-songs")]
        public async Task<IActionResult> AddRandomSongs([FromQuery] int amount)
        {
            if (amount <= 0)
                return BadRequest("No param");
            Random random = new Random();
            var newFaker = new Faker<Song>()
                .RuleFor(a => a.Title, f => f.Music.Genre());
            ICollection<Song> newSongs = newFaker.Generate(amount);
            foreach (var song in newSongs)
            {
                var newArtist = await _context.Artists.OrderBy(x => Guid.NewGuid()).FirstAsync();
                song.Artist.Add(newArtist);
                newArtist.Songs.Add(song);
            }
            //await _context.BulkInsertAsync(newSongs);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("add-random-charts")]
        public async Task<IActionResult> AddRandomCharts([FromQuery] int amount)
        {
            Random random = new Random();
            int currentId = 1;
            if (amount <= 0)
                return BadRequest("No param");
            if (await _context.Charts.AnyAsync())
            {
                currentId = await _context.Charts.MaxAsync(x => x.Id);
            }
            var newFaker = new Faker<Chart>();
            List<Chart> newCharts = newFaker.Generate(amount);
            foreach (var chart in newCharts)
            {
                chart.PublishDate = DateTime.Now;
                chart.Game = (int)GameClass.Arcaea;
                chart.RatingClass = 2;
                chart.Diffifulty = "10+";
                chart.DownloadTime = random.Next(0, 100000);
                Song baseSong = await _context.Songs.OrderBy(order => Guid.NewGuid()).FirstAsync();
                baseSong.Charts.Add(chart);
                BaseUser user = await _context.Users.OrderBy(x => Guid.NewGuid()).FirstAsync();
                user.Charts.Add(chart);
            }
            await _context.SaveChangesAsync();
            var insertedCharts = await _context.Charts.ToListAsync();
            foreach (var chart in insertedCharts)
            {
                chart.IndexChartId = chart.Id;
            }
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("ClearDb")]
        public async Task<IActionResult> ClearDb([FromQuery] string dbName)
        {
            switch (dbName)
            {
                case "Artist":
                    break;
                case "Base_user":
                    break;
                case "Chart":
                    break;
                case "Email_verify_key":
                    break;
                case "Notice":
                    break;
                case "Song":
                    break;
                case "Store_uid":
                    break;
                default:
                    return BadRequest();
            }
            await _context.Database.ExecuteSqlAsync($"TRUNCATE TABLE [{dbName}]");
            await _context.SaveChangesAsync();
            return Ok();
        }

        private async Task RemoveEmailOutOfTimeAsync()
        {
            int deletedAmount = 0;
            try
            {
                var deleted = await _context.EmailVerifyKeys
                    .Where(x => (DateTime.Now - x.QuestTime) > TimeSpan.FromMinutes(_EmailExistTimeMinute)).ToArrayAsync();
                await Task.Run(() =>
                {
                    _context.EmailVerifyKeys.RemoveRange(deleted);
                });
                await _context.SaveChangesAsync();
                deletedAmount = deleted.Count();
            }
            catch (Exception ex)
            {
                _logger.LogError("{message}", ex.Message);
            }
            finally
            {
                _logger.LogInformation("{message}", $"Deleted {deletedAmount} email messages.");
            }
        }
    }

    public struct ServerConfig
    {
        public string? Server { get; set; }
        public string? Port { get; set; }
        public string? Database { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
    }
}
