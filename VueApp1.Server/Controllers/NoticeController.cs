using Arekat.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Arekat.Server.Controllers
{
    [ApiController]
    [Route("notice-management")]
    public class NoticeController(BaseContext context, ILogger<NoticeController> logger, IConfiguration config) : ControllerBase
    {
        //DBContext
        private readonly BaseContext _context = context;
        //Logger
        private readonly ILogger<NoticeController> _logger = logger;
        //Config
        private readonly IConfiguration _config = config;

        [HttpGet("notices/{id}")]
        public async Task<ActionResult> GetNoticeInfo([FromRoute]int id)
        {
            if (!await _context.Notices.AnyAsync(x => x.Id == id))
            {
                return NotFound();
            }
            Notice? searchResult = await _context.Notices
                .SingleAsync(x => x.Id == id);
            string arthorName = await _context.Users
                .Where(u => u.Id == searchResult.ArthorId)
                .Select(u => u.Name)
                .SingleAsync();
            return Ok(new NoticeInfoObject
            {
                Title = searchResult.Title,
                ArthorId = searchResult.ArthorId,
                Arthor = arthorName,
                HtmlText = searchResult.HtmlText,
                PublishDate = searchResult.PublishTime.ToShortDateString(),
                ViewTime = searchResult.ReadTime,
            });
        }

        [HttpGet("notices/newest")]
        public async Task<ActionResult> GetNewestNoticeInfo()
        {
            if (await _context.Notices.AnyAsync())
                return Ok("No Notice now.");
            var newestNotice = await _context.Notices.OrderByDescending(n => n.PublishTime).FirstAsync();
            return Ok(new NoticeListItem
            {
                NoticeId = newestNotice.Id,
                Title = newestNotice.Title,
                PublishDate = newestNotice.PublishTime.ToShortDateString()
            });
        }

        [HttpGet("notice-list")]
        public async Task<IActionResult> GetNoticeList([FromQuery] int startIndex, [FromQuery] int amount)
        {
            List<Notice> searchResult = await _context.Notices.OrderByDescending(n => n.PublishTime).ToListAsync();
            List<NoticeListItem> result = new List<NoticeListItem>();
            foreach (Notice notice in searchResult)
            {
                result.Add(new NoticeListItem
                {
                    NoticeId = notice.Id,
                    Title = notice.Title,
                    PublishDate = notice.PublishTime.ToShortDateString()
                });
            }
            return Ok(result);
        }
    }

    public struct NoticeInfoObject
    {
        public string Title { get; set; }
        public int ArthorId { get; set; }
        public string Arthor {  get; set; }
        public string HtmlText { get; set; }
        public string PublishDate { get; set; }
        public int ViewTime {  get; set; }
    }
    public struct NoticeListObject
    {
        public List<NoticeListItem> Notices { get; set; }
    }

    public struct NoticeListItem
    {
        public int NoticeId { get; set; }
        public string Title { get; set; }
        public string PublishDate { get; set; }
    }
}
