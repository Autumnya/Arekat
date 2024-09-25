using Microsoft.AspNetCore.Mvc;
using Arekat.Server.Models;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;

namespace Arekat.Server.Controllers
{
    [ApiController]
    [Route("artist-management")]
    public class ArtistController(BaseContext context, ILogger<ArtistController> logger, IConfiguration config) : ControllerBase
    {
        //DBContext
        private readonly BaseContext _context = context;
        //Logger
        private readonly ILogger<ArtistController> _logger = logger;
        //Config
        private readonly IConfiguration _config = config;

        [HttpGet("artists/{id}")]
        public async Task<ActionResult> GetArtistInfo([FromRoute]int id)
        {
            if(!await _context.Artists.AnyAsync(x => x.Id == id))
            {
                return NotFound();
            }
            Artist aimArtist = await _context.Artists.SingleAsync(x => x.Id == id);
            int songCount = await _context.Artists.Where(_ => _.Id == id).SelectMany(x=>x.Songs).CountAsync();
            ArtistInfoObject artistInfoObject = new ArtistInfoObject
            {
                ArtistName = aimArtist.Name,
                ArtistPicSrc = "",
                SongAmount = songCount,
            };
            return Ok(artistInfoObject);
        }
    }

    public struct ArtistInfoObject
    {
        public string ArtistName { get; set; }
        public string ArtistPicSrc { get; set; }
        public int SongAmount { get; set; }
    }
    
}
