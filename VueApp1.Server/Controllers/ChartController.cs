using Microsoft.AspNetCore.Mvc;
using Arekat.Server.Models;
using Microsoft.EntityFrameworkCore;
using System.IO.Compression;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;
using System.Security.Claims;

namespace Arekat.Server.Controllers
{
    [ApiController]
    [Route("chart-management")]
    public class ChartController(BaseContext context, ILogger<ChartController> logger, IConfiguration config, IWebHostEnvironment webHostEnvironment) : ControllerBase
    {
        //DBContext
        private readonly BaseContext _context = context;
        //Logger
        private readonly ILogger<ChartController> _logger = logger;
        //Config
        private readonly IConfiguration _config = config;
        //Environment
        private readonly IWebHostEnvironment _webHostEnvironment = webHostEnvironment;

        public static readonly string[] gameString =
        {
            //GameClass = 0
            "Arcaea"
        };
        public static readonly string[] examineStageString =
        {
            "不限","test","stable"
        };
        public static readonly string[] difficultyString =
        {
            "不限","9以下","9","9+","10","10+","11","11+","12","12+及以上"
        };
        public static readonly List<string> dateString =
        [
            "不限","今天上新","最近7天","最近30天"
        ];
        public static readonly List<string> orderString =
        [
            "时间从新到旧","时间从旧到新","下载量从高到低","下载量从低到高"
        ];
        public static readonly string[] chartDesignerLevel =
        {
            "ChartDesigner lv0",
            "ChartDesigner lv1",
            "ChartDesigner lv2",
            "ChartDesigner lv3",
        };
        public static readonly string[] adminLevel =
        {
            "User",
            "Admin",
            "SuperAdmin",
        };

        public enum GameClass
        {
            Arcaea = 0
        }

        [HttpGet("filter-info")]
        public IActionResult GetFilterInfo()
        {

            return Ok();
        }

        [HttpPut("charts")]
        public async Task<ActionResult> GetChartsAsync([FromBody] ChartFilter chartFilter, [FromQuery] int startIndex, [FromQuery] int getAmount)
        {
            List<ChartItem> chartItems = [];
            DbSet<Chart> chartDb = _context.Charts;
            DateTime minPassDate = DateTime.Parse($@"{chartFilter.MinPassDate[0]}/{chartFilter.MinPassDate[1]}/{chartFilter.MinPassDate[2]}");
            DateTime maxPassDate = DateTime.Parse($@"{chartFilter.MaxPassDate[0]}/{chartFilter.MaxPassDate[1]}/{chartFilter.MaxPassDate[2]}");

            IQueryable<Chart> tempCharts;
            if (!string.IsNullOrEmpty(chartFilter.KeyWords))
            {
                string[] splitedKeywords = chartFilter.KeyWords.Split(" ");
                tempCharts = _context.Songs
                        .Where(s => splitedKeywords.Any(keyword => s.Title.Contains(keyword)))
                        .SelectMany(s => s.Charts);
                tempCharts = _context.Users
                        .Where(u => splitedKeywords.Any(keyword => u.Name.Contains(keyword) || u.Email.Contains(keyword) || u.Id.ToString().Contains(keyword)))
                        .SelectMany(u => u.Charts);
                var artistSongs = await _context.Artists
                        .Where(a => splitedKeywords.Any(keyword => a.Name.Contains(keyword)))
                        .SelectMany(a => a.Songs).ToListAsync();
                foreach (var artist in artistSongs)
                {
                    tempCharts.Union(_context.Songs
                        .Where(s => s.Id == artist.Id)
                        .SelectMany(a => a.Charts));
                }
                tempCharts = tempCharts
                    .Where(c => c.PublishDate > minPassDate)
                    .Where(c => c.PublishDate < maxPassDate);
            }
            else
            {
                tempCharts = chartDb
                    .Where(c => c.PublishDate > minPassDate)
                    .Where(c => c.PublishDate < maxPassDate);
            }
            //选择了其中一个
            if (!(chartFilter.ExamineStage[0] == examineStageString[0] || chartFilter.ExamineStage.Count == 2))
            {
                //传来的“审核阶段”中是不是“stable”
                bool stable = chartFilter.ExamineStage.Contains(examineStageString[2]);
                if (stable)
                {
                    tempCharts = tempCharts.Where(c => c.IsStable == true);
                }
                else
                {
                    tempCharts = tempCharts.Where(c => c.IsStable == false);
                }
            }
            if (!(chartFilter.Difficulty[0] == difficultyString[0]))
            {
                tempCharts = tempCharts.Where(c => chartFilter.Difficulty.Contains(c.Diffifulty));
            }
            switch(orderString.IndexOf(chartFilter.Order))
            {
                case 0:
                    tempCharts = tempCharts.OrderByDescending(c => c.PublishDate);
                    break;
                case 1:
                    tempCharts = tempCharts.OrderBy(c => c.PublishDate);
                    break;
                case 2:
                    tempCharts = tempCharts.OrderByDescending(c => c.DownloadTime);
                    break;
                case 3:
                    tempCharts = tempCharts.OrderBy(c => c.DownloadTime);
                    break;
                default:
                    return BadRequest("No order.");
            }
            var chartsResult = await tempCharts.ToListAsync();

            foreach (var chart in chartsResult.Skip(startIndex).Take(getAmount))
            {
                Song baseSong = await _context.Songs
                    .SingleAsync(s => s.Id == chart.SongId);
                List<Artist> baseArtist = await _context.Songs
                    .Where(s => s.Id == baseSong.Id)
                    .SelectMany(s => s.Artist)
                    .ToListAsync();
                List<BaseUser> baseDesigner = await _context.Charts
                    .Where(c => c.Id == chart.Id)
                    .SelectMany(c => c.Designer)
                    .ToListAsync();
                List<ArtistItem> artistItemList = [];
                List<DesignerItem> userItemList = [];
                foreach (var artist in baseArtist)
                {
                    artistItemList.Add(new ArtistItem
                    {
                        Id = artist.Id,
                        Name = artist.Name,
                    });
                }
                foreach (var designer in baseDesigner)
                {
                    userItemList.Add(new DesignerItem
                    {
                        Id = designer.Id,
                        Name = designer.Name,
                    });
                }

                chartItems.Add(new ChartItem()
                {
                    ChartId = chart.Id,
                    Title = baseSong.Title,
                    SongId = chart.SongId,
                    CoverSrc = "",
                    Artists = artistItemList,
                    ChartDesigners = userItemList,
                    Bpm = baseSong.Bpm,
                    PassDate = chart.PublishDate.ToShortDateString(),
                    RatingClass = chart.RatingClass,
                    Rating = chart.Diffifulty,
                });
            }
            return Ok(new ChartsObject
            {
                ChartAmount = chartsResult.Count,
                Charts = chartItems
            });
        }

        [HttpGet("charts/{id}")]
        public async Task<IActionResult> GetChartInfoAsync([FromRoute] int id)
        {
            Chart aimChart = await _context.Charts
                .SingleAsync(x => x.Id == id);
            Song baseSong = await _context.Songs
                    .SingleAsync(s => s.Id == aimChart.SongId);
            List<Artist> baseArtist = await _context.Songs
                .Where(s => s.Id == baseSong.Id)
                .SelectMany(s => s.Artist)
                .ToListAsync();
            List<BaseUser> baseDesigner = await _context.Charts
                .Where(c => c.Id == id)
                .SelectMany(c => c.Designer)
                .ToListAsync();
            List<ChartHistory> baseUpdate = await _context.ChartsHistory
                .Where(c => c.ChartId == id)
                .ToListAsync();
            List<ArtistItem> artistItemList = new List<ArtistItem>();
            List<DesignerItem> userItemList = new List<DesignerItem>();
            List<UpdateHistoryItem> updateItemList = new List<UpdateHistoryItem>();
            foreach (var artist in baseArtist)
            {
                artistItemList.Add(new ArtistItem
                {
                    Id = artist.Id,
                    Name = artist.Name,
                });
            }
            foreach (var designer in baseDesigner)
            {
                userItemList.Add(new DesignerItem
                {
                    Id = designer.Id,
                    Name = designer.Name,
                });
            }
            foreach (var update in baseUpdate)
            {
                updateItemList.Add(new UpdateHistoryItem
                {
                    Stage = examineStageString[update.Stage + 1],
                    Description = update.Discription,
                    Time = update.UpdateTime.ToLongDateString(),
                });
            }
            return Ok(new ChartInfoObject
            {
                Id = id,
                SongId = baseSong.Id,
                Title = baseSong.Title,
                Artist = artistItemList,
                Designer = userItemList,
                SongCoverSrc = "",
                UpdateHistory = updateItemList,
            });
        }

        [HttpGet("recommand-lists")]
        public async Task<IActionResult> GetRecommandListsAsync()
        {

            return Ok();
        }

        [HttpGet("chartlist")]
        public async Task<ActionResult> GetChartListsAsync([FromQuery] string member, [FromQuery] int id, [FromQuery] int startIndex, [FromQuery] int getAmount)
        {
            List<Chart> __charts = [];
            List<ChartListItem> chartListItems = [];
            List<DesignerItem> designerItems = [];
            switch (member)
            {
                case "user":
                    __charts = await _context.Users
                        .Where(u => u.Id == id)
                        .SelectMany(u => u.Charts)
                        .Skip(startIndex)
                        .Take(getAmount)
                        .ToListAsync();
                    break;
                case "song":
                    __charts = await _context.Songs
                        .Where(s => s.Id == id)
                        .SelectMany(s => s.Charts)
                        .Skip(startIndex)
                        .Take(getAmount)
                        .ToListAsync();
                    break;
                default:
                    return BadRequest();
            };
            foreach (var chart in __charts)
            {
                designerItems = [];
                var designers = await _context.Charts
                    .Where(c => c.Id == chart.Id)
                    .SelectMany(c => c.Designer)
                    .ToListAsync();
                foreach (BaseUser user in designers)
                {
                    designerItems.Add(new DesignerItem
                    {
                        Id = user.Id,
                        Name = user.Name,
                    });
                }
                chartListItems.Add(new ChartListItem
                {
                    Id = chart.Id,
                    Game = gameString[chart.Game],
                    Difficulty = chart.Diffifulty,
                    RatingClass = chart.RatingClass,
                    Designer = designerItems,
                    DownloadCount = chart.DownloadTime,
                    UploadDate = chart.PublishDate.ToString(),
                });
            }
            return Ok(new ChartListObject
            {
                Charts = chartListItems
            });
        }

        [Authorize]
        [HttpGet("download")]
        public async Task<ActionResult> GetChartFilesAsync([FromQuery] int id)
        {
            string fileFolderPath = $@"{_webHostEnvironment.ContentRootPath}{_config.GetValue<string>("FilePath:ChartFileDir")}/{id}";
            /*
            Response.Headers.ContentDisposition = "attachment; filename = ChartTest.zip";
            using (var archive = new ZipArchive(Response.BodyWriter.AsStream(), ZipArchiveMode.Create))
            {
                archive.CreateEntryFromFile(fileFolderPath, @$"Chart{id}.zip");
            }
            */
            string zipPath = Path.GetTempFileName() + ".zip"; //临时保存路径
            await Task.Run(() =>
            {
                using var zipArchive = ZipFile.Open(zipPath, ZipArchiveMode.Update);
                Parallel.ForEach(Directory.GetFiles(fileFolderPath), (file) =>
                {
                    zipArchive.CreateEntryFromFile(file, Path.GetFileName(file));
                });
            });
            var zipBytes = await System.IO.File.ReadAllBytesAsync(zipPath);
            //删除临时文件
            System.IO.File.Delete(zipPath);

            var aimChart = await _context.Charts.SingleAsync(c => c.Id == id);
            aimChart.DownloadTime += 1;
            await _context.SaveChangesAsync();
            return File(zipBytes, "application/zip", @$"Chart{id}.zip");
        }

        #region ChartUpload
        //客户端上传文件说明：
        //1.客户端每次在进入谱面上传页时会发起一个上传请求
        //2.api同意后在临时谱面目录下生成一个名为用户ID的文件夹用于存放临时文件，并发送一个guid给客户端，该guid也存在于临时文件夹内的temp文件用于验证
        //3.由于用户同一时间只能上传一张谱面，新生成的guid将会覆盖掉原先的guid，同时会删除原先的临时文件
        //4.客户端每次提交上传请求时都应该验证guid是否正确，并通知客户端
        //5.系统应定时清楚无用的临时文件

        //发起一个新的上传请求，即打开上传页面
        [Authorize]
        [HttpGet("upload")]
        public ActionResult<string> ChartUploadRequestAsync()
        {
            Guid guid = Guid.NewGuid();
            string userId = User.Claims.Single(c => c.Type == "UserId").Value;
            string folderName = $"{userId}";
            string directory = $"{_webHostEnvironment.ContentRootPath}{_config.GetValue<string>("FilePath:ChartFileDir")}/Temp/{folderName}";
            if (Directory.Exists(directory))
            {
                Directory.Delete(directory, true);
            }
            Directory.CreateDirectory(directory);
            System.IO.File.WriteAllText($"{directory}/temp", $"{guid}");
            return Ok(guid);
        }

        //Upload a Arcaea chart
        [Authorize]
        [HttpPost("upload-confirm")]
        public async Task<IActionResult> ChartUploadConfirmAsync([FromBody] ChartUploadObject songListInfo, [FromQuery] string guid)
        {
            //Validate database
            if (!await _context.Songs.AnyAsync(s => s.Id == songListInfo.SongId))
            {
                return NotFound("Song not exist.");
            }
            foreach (int designerId in songListInfo.DesignerId)
            {
                if (!await _context.Users.AnyAsync(u => u.Id == designerId))
                {
                    return NotFound("User not exist.");
                }
            }
            //Validate Files
            string userId = User.Claims.Single(c => c.Type == "UserId").Value;
            string directory = $"{_webHostEnvironment.ContentRootPath}{_config.GetValue<string>("FilePath:ChartFileDir")}/Temp/{userId}";
            if (!ValidateFileTempGuid(directory, guid, userId))
            {
                return BadRequest("Request expired");
            }
            if (!System.IO.File.Exists($"{directory}/base.ogg"))
                return BadRequest("base.ogg not exist");
            if (!System.IO.File.Exists($"{directory}/base.jpg"))
                return BadRequest("base.jpg not exist");
            if (!System.IO.File.Exists($"{directory}/base_256.jpg"))
                return BadRequest("base_256.jpg not exist");
            foreach (var difficulty in songListInfo.Difficulties)
            {
                if (!System.IO.File.Exists($"{directory}/{difficulty.RatingClass}.aff"))
                {
                    return BadRequest($"{difficulty.RatingClass}.aff not exist.");
                }
            }
            //Create entity, write to database
            List<Chart> newCharts = [];
            foreach (var difficulty in songListInfo.Difficulties)
            {
                Chart newChart = new Chart
                {
                    IndexChartId = -1,
                    Game = (int)GameClass.Arcaea,
                    RatingClass = difficulty.RatingClass,
                    Diffifulty = $"{difficulty.Rating}{(difficulty.RatingPlus ? "+" : "")}",
                    PublishDate = DateTime.Now,
                    DownloadTime = 0
                };
                foreach (int designerId in songListInfo.DesignerId)
                {
                    var designerItem = await _context.Users.SingleAsync(u => u.Id == designerId);
                    designerItem.Charts.Add(newChart);
                    newChart.Designer.Add(designerItem);
                }
                newCharts.Add(newChart);
            }
            var song = await _context.Songs.SingleAsync(s => s.Id == songListInfo.SongId);
            song.Charts.AddRange(newCharts);
            await _context.Charts.AddRangeAsync(newCharts);
            await _context.SaveChangesAsync();
            var insertedCharts =  await _context.Charts
                .Where(c=>c.IndexChartId == -1)
                .ToListAsync();
            int indexId = insertedCharts.OrderByDescending(c=>c.Id).First().Id;
            foreach (var insertedChart in insertedCharts)
            {
                insertedChart.IndexChartId = indexId;
            }
            await _context.SaveChangesAsync();
            //Generate songlist item
            SongListItemObject newSonglistObject;
            var aimSong = _context.Songs.Where(s => s.Id == songListInfo.SongId);
            List<Artist> artists = await aimSong
                .SelectMany(s => s.Artist).ToListAsync();
            var songQuest = await aimSong.FirstAsync();
            string songTitle = songQuest.Title;
            string artistStr = "";
            foreach (Artist artist in artists)
            {
                if (artists.IndexOf(artist) == 0)
                {
                    artistStr = artist.Name;
                }
                else
                {
                    artistStr += $"{artist.Name}";
                }
            }
            SongInfoTitleLocalizedItem songInfoTitleLocalizedItem = new SongInfoTitleLocalizedItem();
            songInfoTitleLocalizedItem.En = songTitle;
            DateTimeOffset dateTimeOffset;
            dateTimeOffset = newCharts[0].PublishDate;
            newSonglistObject = new SongListItemObject
            {
                Id = newCharts[0].Id.ToString(),
                TitleLocalized = songInfoTitleLocalizedItem,
                Artist = artistStr,
                Bpm = songListInfo.Bpm,
                BpmBase = songListInfo.BpmBase,
                Set = "base",
                AudioPreview = songListInfo.AudioPreview,
                AudioPreviewEnd = songListInfo.AudioPreviewEnd,
                Side = songListInfo.Side,
                Bg = songListInfo.Bg,
                Date = dateTimeOffset.ToUnixTimeSeconds(),
                Version = songListInfo.Version,
                Difficulties = songListInfo.Difficulties,
            };
            string checkingChartsDirectory = $"{_webHostEnvironment.ContentRootPath}{_config.GetValue<string>("FilePath:ChartFileDir")}/Checking/{newCharts[0].Id}";
            Directory.CreateDirectory(checkingChartsDirectory);
            string songlist = JsonSerializer.Serialize(newSonglistObject);
            System.IO.File.WriteAllText($"{checkingChartsDirectory}/songlist", songlist);
            //Move files
            string[] files = Directory.GetFiles(directory);
            foreach (string file in files)
            {
                string targetFileName = Path.GetFileName(file);
                System.IO.File.Move(file, $"{checkingChartsDirectory}/{targetFileName}");
            }
            Directory.Delete(directory);
            return Ok("Upload success");
        }

        [Authorize]
        [HttpPatch("check-pass")]
        public async Task<IActionResult> CheckPassChartAsync([FromQuery] int chartId)
        {
            string role = User.Claims.Single(c => c.Type == ClaimTypes.Role).Value;
            if (role == adminLevel[0])
            {
                return Forbid();
            }
            var charts = await _context.Charts.Where(c => c.IndexChartId == chartId).ToListAsync();
            foreach (var chart in charts)
            {
                chart.IsChecked = true;
            }
            //Move files
            string basePath = $"{_webHostEnvironment.ContentRootPath}/{_config.GetValue<string>("FilePath:ChartFileDir")}";
            string originPath = $"{basePath}/Checking/{chartId}";
            Directory.CreateDirectory($"{basePath}/{chartId}");
            string[] files = Directory.GetFiles(originPath);
            System.IO.File.Delete($"{originPath}/temp");
            foreach (string file in files)
            {
                string fileName = Path.GetFileName(file);
                System.IO.File.Move(file, $"{basePath}/{chartId}/{fileName}");
            }
            Directory.Delete(originPath);
            return Ok();
        }

        [Authorize]
        [HttpPost("upload/base.ogg")]
        public async Task<IActionResult> UploadChartFileMusicAsync(IFormFile chartFileMusic, [FromQuery] string guid)
        {
            string userId = User.Claims.Single(c => c.Type == "UserId").Value;
            //Max size: 32MB
            string fileVerifyCode = ValidateFile(chartFileMusic, 33554432, "base.ogg");
            if (fileVerifyCode != "Ok.")
            {
                return BadRequest(fileVerifyCode);
            }
            string directory = $"{_webHostEnvironment.ContentRootPath}{_config.GetValue<string>("FilePath:ChartFileDir")}/Temp/{userId}";
            if (!ValidateFileTempGuid(directory, guid, userId))
            {
                return BadRequest("Request expired.");
            }
            //Save file
            if (await SaveTempChartFileAsync(chartFileMusic, directory))
            {
                return Ok("Upload success.");
            }
            else
            {
                return BadRequest("Error request.");
            }
        }
        [Authorize]
        [HttpPost("upload/base.jpg")]
        public async Task<IActionResult> UploadChartFilePicAsync(IFormFile chartFileJpg, [FromQuery] string guid)
        {
            string userId = User.Claims.Single(c => c.Type == "UserId").Value;
            //Max size: 4MB
            string fileVerifyCode = ValidateFile(chartFileJpg, 4194304, "base.jpg");
            if (fileVerifyCode != "Ok.")
            {
                return BadRequest(fileVerifyCode);
            }
            string directory = $"{_webHostEnvironment.ContentRootPath}{_config.GetValue<string>("FilePath:ChartFileDir")}/Temp/{userId}";
            if (!ValidateFileTempGuid(directory, guid, userId))
            {
                return BadRequest("Request expired.");
            }
            //Save file
            if (await SaveTempChartFileAsync(chartFileJpg, directory))
            {
                return Ok("Upload success.");
            }
            else
            {
                return BadRequest("Error request.");
            }
        }
        [Authorize]
        [HttpPost("upload/base_256.jpg")]
        public async Task<IActionResult> UploadChartFilePic256Async(IFormFile chartFileJpg256, [FromQuery] string guid)
        {
            string userId = User.Claims.Single(c => c.Type == "UserId").Value;
            //Max size: 4MB
            string fileVerifyCode = ValidateFile(chartFileJpg256, 4194304, "base_256.jpg");
            if (fileVerifyCode != "Ok.")
            {
                return BadRequest(fileVerifyCode);
            }
            string directory = $"{_webHostEnvironment.ContentRootPath}{_config.GetValue<string>("FilePath:ChartFileDir")}/Temp/{userId}";
            if (!ValidateFileTempGuid(directory, guid, userId))
            {
                return BadRequest("Request expired.");
            }
            //Save file
            if (await SaveTempChartFileAsync(chartFileJpg256, directory))
            {
                return Ok("Upload success.");
            }
            else
            {
                return BadRequest("Error request.");
            }
        }
        [Authorize]
        [HttpPost("upload/0.aff")]
        public async Task<IActionResult> UploadChartFileAff0Async(IFormFile chartFileAff0, [FromQuery] string guid)
        {
            string userId = User.Claims.Single(c => c.Type == "UserId").Value;
            //Max size: 4MB
            string fileVerifyCode = ValidateFile(chartFileAff0, 4194304, "0.aff");
            if (fileVerifyCode != "Ok.")
            {
                return BadRequest(fileVerifyCode);
            }
            string directory = $"{_webHostEnvironment.ContentRootPath}{_config.GetValue<string>("FilePath:ChartFileDir")}/Temp/{userId}";
            if (!ValidateFileTempGuid(directory, guid, userId))
            {
                return BadRequest("Request expired.");
            }
            //Save file
            if (await SaveTempChartFileAsync(chartFileAff0, directory))
            {
                return Ok("Upload success.");
            }
            else
            {
                return BadRequest("Error request.");
            }
        }
        [Authorize]
        [HttpPost("upload/1.aff")]
        public async Task<IActionResult> UploadChartFileAff1Async(IFormFile chartFileAff1, [FromQuery] string guid)
        {
            string userId = User.Claims.Single(c => c.Type == "UserId").Value;
            //Max size: 4MB
            string fileVerifyCode = ValidateFile(chartFileAff1, 4194304, "1.aff");
            if (fileVerifyCode != "Ok.")
            {
                return BadRequest(fileVerifyCode);
            }
            string directory = $"{_webHostEnvironment.ContentRootPath}{_config.GetValue<string>("FilePath:ChartFileDir")}/Temp/{userId}";
            if (!ValidateFileTempGuid(directory, guid, userId))
            {
                return BadRequest("Request expired.");
            }
            //Save file
            if (await SaveTempChartFileAsync(chartFileAff1, directory))
            {
                return Ok("Upload success.");
            }
            else
            {
                return BadRequest("Error request.");
            }
        }
        [Authorize]
        [HttpPost("upload/2.aff")]
        public async Task<IActionResult> UploadChartFileAff2Async(IFormFile chartFileAff2, [FromQuery] string guid)
        {
            string userId = User.Claims.Single(c => c.Type == "UserId").Value;
            //Max size: 4MB
            string fileVerifyCode = ValidateFile(chartFileAff2, 4194304, "2.aff");
            if (fileVerifyCode != "Ok.")
            {
                return BadRequest(fileVerifyCode);
            }
            string directory = $"{_webHostEnvironment.ContentRootPath}{_config.GetValue<string>("FilePath:ChartFileDir")}/Temp/{userId}";
            if (!ValidateFileTempGuid(directory, guid, userId))
            {
                return BadRequest("Request expired.");
            }
            //Save file
            if (await SaveTempChartFileAsync(chartFileAff2, directory))
            {
                return Ok("Upload success.");
            }
            else
            {
                return BadRequest("Error request.");
            }
        }
        [Authorize]
        [HttpPost("upload/3.aff")]
        public async Task<IActionResult> UploadChartFileAff3Async(IFormFile chartFileAff3, [FromQuery] string guid)
        {
            string userId = User.Claims.Single(c => c.Type == "UserId").Value;
            //Max size: 4MB
            string fileVerifyCode = ValidateFile(chartFileAff3, 4194304, "3.aff");
            if (fileVerifyCode != "Ok.")
            {
                return BadRequest(fileVerifyCode);
            }
            string directory = $"{_webHostEnvironment.ContentRootPath}{_config.GetValue<string>("FilePath:ChartFileDir")}/Temp/{userId}";
            if (!ValidateFileTempGuid(directory, guid, userId))
            {
                return BadRequest("Request expired.");
            }
            //Save file
            if (await SaveTempChartFileAsync(chartFileAff3, directory))
            {
                return Ok("Upload success.");
            }
            else
            {
                return BadRequest("Error request.");
            }
        }
        #endregion

        private bool ValidateJwt(IHeaderDictionary header)
        {
            if (!Response.Headers.TryGetValue("Authorization", out var authHeader))
            {
                return false;
            }
            string token = authHeader.ToString().Replace("Bearer ", "");
            var signKey = _config.GetValue<string>("Jwt:SecretKey");
            if (string.IsNullOrEmpty(signKey))
            {
                throw new Exception("No jwt sign key");
            }
            var tokenValidationParameters = new TokenValidationParameters()
            {
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signKey)),
                ValidIssuer = _config.GetValue<string>("Jwt:Issuer"),
                ValidAudience = _config.GetValue<string>("Jwt:Audience"),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                RequireExpirationTime = true,
                ValidateIssuerSigningKey = true,
            };
            try
            {
                var handler = new JwtSecurityTokenHandler();
                handler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);

                var jwtToken = validatedToken as JwtSecurityToken;
                var claims = jwtToken.Claims;
                var username = claims.FirstOrDefault(c => c.Type == "name")?.Value;

                return true;
            }
            catch (SecurityTokenException ex)
            {
                return false;
            }
        }
        private string ValidateFile(IFormFile chartFile, int maxFileByte, string fileName)
        {
            if (chartFile == null)
            {
                return "No file.";
            }
            if (chartFile.FileName != fileName)
            {
                return "Error file name.";
            }
            if (chartFile.Length > maxFileByte)
            {
                return "File out of size.";
            }
            return "Ok.";
        }
        private bool ValidateFileTempGuid(string directory, string questGuid, string userId)
        {
            string guid = System.IO.File.ReadAllText($"{directory}/temp");
            if (questGuid == guid)
            {
                return true;
            }
            return false;
        }
        private async Task<bool> SaveTempChartFileAsync(IFormFile chartFile, string directory)
        {
            string path = $@"{directory}/{chartFile.FileName}";
            using (var fileStream = System.IO.File.Create(path))
            {
                await chartFile.OpenReadStream().CopyToAsync(fileStream);
            }
            _logger.LogInformation("{message}", @$"Saved file at {path}");
            return true;
        }
    }

    public struct ReclistObject
    {
        public string MainTitle { get; set; }
        public string SubTitle { get; set; }
        public ChartListObject ChartList { get; set; }
    }
    public struct ChartInfoObject
    {
        public int Id { get; set; }
        public int SongId { get; set; }
        public string Title { get; set; }
        public List<ArtistItem> Artist { get; set; }
        public List<DesignerItem> Designer { get; set; }
        public string SongCoverSrc { get; set; }
        public List<UpdateHistoryItem> UpdateHistory { get; set; }
    }
    public struct ChartsObject
    {
        public int ChartAmount { get; set; }
        public List<ChartItem> Charts { get; set; }
    }
    public struct ChartListObject
    {
        public ChartFilter ChartFilter { get; set; }
        public List<ChartListItem> Charts { get; set; }
    }
    public struct FilterItem
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public IEnumerable<string>? Item { get; set; }
        public bool AllowMultiChoose { get; set; }
    }
    public struct FilterItemCollention
    {
        public FilterItem? Games { get; set; }
        public FilterItem? ExamineStage { get; set; }
        public FilterItem? Difficulty { get; set; }
        public FilterItem? Date { get; set; }
        public FilterItem? MinPassDate { get; set; }
        public FilterItem? MaxPassDate { get; set; }
        public FilterItem? Order { get; set; }
    }
    public struct ChartFilter()
    {
        public List<string> Games { get; set; } = ["Arcaea"];
        public List<string> ExamineStage { get; set; }
        public List<string> Difficulty { get; set; }
        public string? Date { get; set; }
        public List<string> MinPassDate { get; set; }
        public List<string> MaxPassDate { get; set; }
        public string Order { get; set; }
        public string KeyWords { get; set; }
    }
    public struct SongListItemObject()
    {
        public string Id { get; set; }
        public SongInfoTitleLocalizedItem TitleLocalized { get; set; }
        public string Artist { get; set; }
        public string Bpm { get; set; }
        public int BpmBase { get; set; }
        public string Set { get; set; } = "base";
        public string Purchase { get; set; } = "";
        public int AudioPreview { get; set; }
        public int AudioPreviewEnd { get; set; }
        public int Side { get; set; }
        public string Bg { get; set; }
        public long Date { get; set; }
        public string Version { get; set; }
        public List<SongInfoDifficultiyItem> Difficulties { get; set; }
    }
    public struct ChartUploadObject
    {
        public int SongId { get; set; }
        public List<int> DesignerId { get; set; }
        public string Bpm { get; set; }
        public int BpmBase { get; set; }
        public int AudioPreview { get; set; }
        public int AudioPreviewEnd { get; set; }
        public int Side { get; set; }
        public string Bg { get; set; }
        public string Version { get; set; }
        public List<SongInfoDifficultiyItem> Difficulties { get; set; }
    }

    public struct ChartItem
    {
        public int ChartId { get; set; }
        public string Title { get; set; }
        public int SongId { get; set; }
        public string CoverSrc { get; set; }
        public List<ArtistItem> Artists { get; set; }
        public List<DesignerItem> ChartDesigners { get; set; }
        public string Bpm { get; set; }
        public string PassDate { get; set; }
        public int RatingClass { get; set; }
        public string Rating { get; set; }
    }
    public struct ChartListItem
    {
        public int Id { get; set; }
        public string Game { get; set; }
        public string Difficulty { get; set; }
        public int RatingClass { get; set; }
        public List<DesignerItem> Designer { get; set; }
        public int DownloadCount { get; set; }
        public string UploadDate { get; set; }
    }
    public struct ArtistItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public struct DesignerItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public struct UpdateHistoryItem
    {
        public string Stage { get; set; }
        public string Description { get; set; }
        public string Time { get; set; }
    }
    public struct SongInfoTitleLocalizedItem
    {
        public string En { get; set; }
        public string? Jp { get; set; }
        public string? Cn { get; set; }
    }
    public struct SongInfoDifficultiyItem()
    {
        public int RatingClass { get; set; }
        public string ChartDesigner { get; set; }
        public string JacketDesigner { get; set; }
        public int Rating { get; set; }
        public bool RatingPlus { get; set; } = false;
    }
}
