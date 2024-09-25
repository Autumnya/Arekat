using Microsoft.AspNetCore.Mvc;
using Arekat.Server.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Arekat.Server.Controllers
{
    [ApiController]
    [Route("song-management")]
    public class SongController(BaseContext context, ILogger<SongController> logger, IConfiguration config) : ControllerBase
    {
        //DBContext
        private readonly BaseContext _context = context;
        //Logger
        private readonly ILogger<SongController> _logger = logger;
        //Config
        private readonly IConfiguration _config = config;

        [HttpGet("songs/{id}")]
        public async Task<ActionResult> GetSongInfo([FromRoute] int id)
        {
            if (!await _context.Songs.AnyAsync(x => x.Id == id))
            {
                return NotFound();
            }
            Song aimSong = await _context.Songs.SingleAsync(x => x.Id == id);
            int chartCount = await _context.Songs.Where(x => x.Id == id).SelectMany(x=>x.Charts).CountAsync();
            List<Artist> artists = await _context.Songs.Where(x => x.Id == id).SelectMany(s => s.Artist).ToListAsync();
            List<ArtistObjectItem> artistList = [];
            foreach (Artist artist in artists)
            {
                artistList.Add(new ArtistObjectItem
                {
                    ArtistId = artist.Id,
                    ArtistName = artist.Name,
                });
            }
            var result = new SongInfoObject
            {
                Id = aimSong.Id,
                Title = aimSong.Title,
                Artist = artistList,
                SongCoverSrc = "",
                ChartAmount = chartCount,
            };
            return Ok(result);
        }

        [HttpGet("songlist")]
        public async Task<ActionResult<ArtistSongsObject>> GetSongList([FromQuery] int artistId, [FromQuery] int startIndex, [FromQuery] int getAmount)
        {
            if (!await _context.Artists.AnyAsync(x => x.Id == artistId))
            {
                return NotFound();
            }
            Artist artist = await _context.Artists.SingleAsync(x => x.Id == artistId);
            IEnumerable<Song> searchResult = await _context.Songs
                .Where(x => x.Artist.Contains(artist))
                .OrderByDescending(x => x.Charts.Count())
                .Skip(startIndex)
                .Take(getAmount)
                .ToListAsync();
            List<SongObjectItem> songlist = new List<SongObjectItem>();
            foreach (Song song in searchResult)
            {
                songlist.Add(new SongObjectItem
                {
                    SongId = song.Id.ToString(),
                    Title = song.Title,
                    SongCoverSrc = "",
                    ChartAmount = await _context.Charts.Where(c => c.SongId == song.Id).CountAsync(),
                });
            }
            return Ok(new ArtistSongsObject
            {
                songs = songlist
            });
        }
    }

    public struct SongInfoObject
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<ArtistObjectItem> Artist { get; set; }
        public string SongCoverSrc { get; set; }
        public int ChartAmount { get; set; }
    }
    public struct ArtistSongsObject
    {
        public IEnumerable<SongObjectItem> songs { get; set; }
    }

    public struct ArtistObjectItem
    {
        public int ArtistId { get; set; }
        public string ArtistName { get; set; }
    }
    public struct SongObjectItem
    {
        public string SongId { get; set; }
        public string? Title { get; set; }
        public string? SongCoverSrc { get; set; }
        public int ChartAmount { get; set; }
    }
}
