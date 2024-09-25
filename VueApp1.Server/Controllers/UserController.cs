using Microsoft.AspNetCore.Mvc;
using Arekat.Server.Models;
using System.Net.Mail;
using System.Net;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Arekat.Server.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace Arekat.Server.Controllers
{
    [ApiController]
    [Route("user-management")]
    //[Authorize("User")]
    public class UserController(BaseContext context, ILogger<UserController> logger, IConfiguration config, JwtHelper jwtHelper) : ControllerBase
    {
        //DBContext
        private readonly BaseContext _context = context;
        //Logger
        private readonly ILogger<UserController> _logger = logger;
        //Config
        private readonly IConfiguration _config = config;
        //Authentication and Authorization
        private readonly JwtHelper _jwtHelper = jwtHelper;

        //What client want to do?
        private enum VerifyEmailQuestClass
        {
            Regist = 0,
            ChangePassword = 1,
        };

        //User regeistration
        [HttpPost("regist")]
        [AllowAnonymous]
        public async Task<IActionResult> RegistAsync([FromBody] RegeistQuestObject quest)
        {
            if (await _context.Users.AnyAsync(x => x.Email == quest.Email))
            {
                return BadRequest("Email already regeisted.");
            }
            Task<bool> verifyEmailKey = VerifyEmailKeyAsync(quest.Email, quest.EmailKey);
            if (!VerifyRegeistInput(quest.UserName, quest.Password))
            {
                return BadRequest("Invalid input.");
            }
            if (!await verifyEmailKey)
            {
                return BadRequest("Verify code incorrect.");
            }
            int newId = await GetUidFromStoreAsync();
            if (newId == -1)
            {
                return StatusCode(500);
            }
            BaseUser newUser = new BaseUser
            {
                Guid = Guid.NewGuid(),
                Id = newId,
                Name = quest.UserName.Trim(),
                Password = quest.Password,
                Email = quest.Email,
                RegistrationDate = DateTime.Now,
            };
            await _context.Users.AddAsync(newUser);
            await RemoveExistEmailAsync(quest.Email);
            await _context.SaveChangesAsync();
            return Ok();
        }

        //User login
        [HttpPut("login")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginAsync([FromBody] LoginQuestObject quest)
        {
            if (await VerifyLoginQuestAsync(quest.Email, quest.Password))
            {
                BaseUser user;
                try
                {
                    user = await _context.Users.SingleAsync(x => x.Email == quest.Email);
                    user.LoginTime = DateTime.Now;
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
                var newToken = _jwtHelper.CreateToken(user.Id, user.Name, user.ChartDesignerLevel, user.AdminLevel);
                return Ok(new ReturnLoginQuest
                {
                    Token = newToken,
                });
            }
            else
            {
                return BadRequest("Incorrect email or password.");
            }
        }

        //Send a verify email
        [HttpPost("verify-email")]
        [AllowAnonymous]
        public async Task<IActionResult> VerifyEmailAsync([FromBody] VerifyEmailQuestObject quest)
        {
            Task<bool> removeExistEmail = VarifyExistEmailAsync(quest.Email);
            var rand = new Random();
            int verifyCode = rand.Next(100001, 999999);
            string to = quest.Email;
            string from = _config.GetValue<string>("Mail:SenderEmail");
            MailMessage message = new(from, to);
            SmtpClient client = new();
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Host = _config.GetValue<string>("Mail:SmtpServer");
            client.Port = _config.GetValue<int>("Mail:SmtpPort");
            client.EnableSsl = true;
            //client.UseDefaultCredentials = true;
            client.Credentials = new NetworkCredential(_config.GetValue<string>("Mail:SenderEmail"), _config.GetValue<string>("Mail:SenderPassword"));
            message.Subject = "Arekat verifier.";
            message.BodyEncoding = Encoding.UTF8;
            message.IsBodyHtml = false;
            message.Priority = MailPriority.Normal;
            if (!await removeExistEmail)
            {
                return BadRequest("Frequent quest.");
            }
            if (quest.QuestClass == (byte)VerifyEmailQuestClass.Regist)
            {
                message.Body = $"You have a regeistration quest in Arekat, please input the verify code to complete regeistration.\n" +
                    $"-----------------------------------------------------------\n" +
                    $"{verifyCode}\n" +
                    $"-----------------------------------------------------------\n" +
                    $"Please verify in 4 hours, this verify code will be abolished 4 hours after.";
            }
            else if (quest.QuestClass == (byte)VerifyEmailQuestClass.ChangePassword)
            {
                message.Body = $"You have a password change quest in Arekat, please input the verify code to complete regeistration.\n" +
                    $"-----------------------------------------------------------\n" +
                    $"{verifyCode}\n" +
                    $"-----------------------------------------------------------\n" +
                    $"Please verify in 4 hours, this verify code will be abolished 4 hours after.";
            }
            else
                return BadRequest();
            try
            {
                await Task.Run(() =>
                {
                    client.Send(message);
                    _logger.LogInformation("{Message} {Time}", $"Send a email to {quest.Email}", DateTime.UtcNow.ToLongTimeString());
                });
            }
            catch (Exception ex)
            {
                _logger.LogError("{Message} {Time}", ex.ToString(), DateTime.UtcNow.ToLongTimeString());
                return BadRequest(ex.Message);
            }
            EmailVerifyKey newKey = new EmailVerifyKey
            {
                Id = Guid.NewGuid(),
                QuestClass = quest.QuestClass,
                QuestTime = DateTime.Now,
                ReceiverEmail = quest.Email,
                Key = verifyCode.ToString(),
            };
            await _context.EmailVerifyKeys.AddAsync(newKey);
            await _context.SaveChangesAsync();
            return Ok();
        }

        //User change password
        [Authorize]
        [HttpPatch("change-password")]
        public async Task<IActionResult> ChangePasswordAsync(string newPassword)
        {

            return Ok();
        }

        [HttpGet("users/{id}")]
        public async Task<ActionResult> GetUserInfoAsync([FromRoute] int id)
        {
            BaseUser? aimUser = await _context.Users.SingleOrDefaultAsync(u => u.Id == id);
            if (aimUser == null)
                return NotFound();
            var tempQuery = _context.Users.Where(u => u.Id == id);
            var amountChart = await tempQuery.SelectMany(u => u.Charts).ToListAsync();
            var badges = await tempQuery.SelectMany(u => u.Badges).ToListAsync();
            return Ok(new UserInfoObject
            {
                UserId = aimUser.Id,
                UserName = aimUser.Name,
                UserLevel = aimUser.Level,
                DesignerLevel = aimUser.ChartDesignerLevel,
                Badges = badges,
                Intro = aimUser.Intro,
                ChartAmount = amountChart.Count,
            });
        }

        [Authorize]
        [HttpGet("users/self")]
        public async Task<ActionResult> GetSelfInfoAsync()
        {
            string userIdStr = User.Claims.Single(u => u.Type == "UserId").ToString();
            int userId = Convert.ToInt32(userIdStr.Replace("UserId:","").Trim());
            BaseUser? aimUser = await _context.Users.SingleOrDefaultAsync(u => u.Id == userId);
            if (aimUser == null)
                return NotFound();
            var tempQuery = _context.Users.Where(u => u.Id == userId);
            var badges = await tempQuery.SelectMany(u => u.Badges).ToListAsync();
            return Ok(new UserInfoObject
            {
                UserId = aimUser.Id,
                UserName = aimUser.Name,
                UserLevel = aimUser.Level,
                DesignerLevel = aimUser.ChartDesignerLevel,
                Badges = badges,
                Intro = aimUser.Intro,
                ChartAmount = 0,
            });
        }

        [HttpGet("user-search")]
        public async Task<IActionResult> UserSearchAsync([FromQuery] string keywords)
        {
            bool isIntKeywords = false;
            Task task = Task.Run(() =>
            {
                foreach (char c in keywords)
                {
                    if (c > '9' || c < '0')
                    {
                        isIntKeywords = false;
                    }
                }
            });
            await task;
            List<BaseUser> userIdSearchResult = [];
            if(isIntKeywords)
            {
                userIdSearchResult = await _context.Users.Where(u => u.Id.ToString().Contains(keywords)).ToListAsync();
            }
            List<BaseUser> userNameSearchResult = await _context.Users.Where(u=>u.Name.Contains(keywords)).ToListAsync();
            List<UserSearchItem> userSearchItems = new List<UserSearchItem>();
            int amount = await _context.Users.CountAsync();
            foreach (var item in userIdSearchResult)
            {
                userSearchItems.Add(new UserSearchItem
                {
                    Id = item.Id,
                    Name = item.Name,
                    AvatorSrc = "",
                    ChartAmount = await _context.Users.Where(u => u.Id == item.Id).SelectMany(u => u.Charts).CountAsync()
                });
            }
            foreach (var item in userNameSearchResult)
            {
                if(userIdSearchResult.Contains(item))
                    continue;
                userSearchItems.Add(new UserSearchItem
                {
                    Id = item.Id,
                    Name = item.Name,
                    AvatorSrc = "",
                    ChartAmount = await _context.Users.Where(u => u.Id == item.Id).SelectMany(u => u.Charts).CountAsync()
                });
            }
            return Ok(new UserSearchResultObject
            {
                Amount = amount,
                Users = userSearchItems
            });
        }

        private bool VerifyRegeistInput(string? userName, string? password)
        {
            if (userName == null || password == null)
                return false;
            if (userName.Length < 4 || userName.Length > 32)
                return false;
            if (password.Length != 64)
                return false;
            if (!CheckPasswordFormat(password))
                return false;
            return true;
        }
        private bool CheckPasswordFormat(string input)
        {
            if (string.IsNullOrEmpty(input))
                return false;
            foreach (char c in input)
            {
                if ((short)c < 32 || (short)c > 126)
                {
                    return false;
                }
            }
            return true;
        }
        private async Task<bool> VerifyLoginQuestAsync(string email, string password)
        {
            if (!await _context.Users.AnyAsync(x => x.Email == email))
                return false;
            BaseUser user = await _context.Users.SingleAsync(x => x.Email == email);
            if (user.Password == password)
                return true;
            else
                return false;
        }
        private async Task<bool> VerifyEmailKeyAsync(string email, string emailKey)
        {
            if (!await _context.EmailVerifyKeys.AnyAsync(x => x.ReceiverEmail == email))
            {
                return false;
            }
            bool ifSuccess = false;
            var verifier = await _context.EmailVerifyKeys.SingleAsync(x => x.ReceiverEmail == email);
            if (verifier.Key == emailKey)
            {
                _context.EmailVerifyKeys.Remove(verifier);
                ifSuccess = true;
            }
            await _context.SaveChangesAsync();
            return ifSuccess;
        }
        private async Task<int> GetUidFromStoreAsync()
        {
            StoreUid newStoreid = new StoreUid
            {
                Guid = new Guid(),
                Id = -1
            };
            try
            {
                newStoreid = await _context.StoreUids.OrderBy(x => x.Id).FirstAsync();
                _context.StoreUids.Remove(newStoreid);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("{message}", ex.Message);
            }
            return newStoreid.Id;
        }
        //Return false if get email too fast
        private async Task<bool> VarifyExistEmailAsync(string email)
        {
            bool haveEmailVerifyKey = await _context.EmailVerifyKeys.AnyAsync(t => t.ReceiverEmail == email);
            if (haveEmailVerifyKey)
            {
                var lastKey = await _context.EmailVerifyKeys.SingleAsync(x => x.ReceiverEmail == email);
                if (DateTime.Now - lastKey.QuestTime < TimeSpan.FromMinutes(1))
                {
                    return false;
                }
                _context.EmailVerifyKeys.Remove(lastKey);
                _logger.LogInformation("{message}", "Removed a exist email verify code");
                _context.SaveChanges();
                return true;
            }
            return true;
        }
        private async Task RemoveExistEmailAsync(string email)
        {
            bool haveEmailVerifyKey = await _context.EmailVerifyKeys.AnyAsync(t => t.ReceiverEmail == email);
            if (haveEmailVerifyKey)
            {
                var lastKey = await _context.EmailVerifyKeys.SingleAsync(x => x.ReceiverEmail == email);
                _context.EmailVerifyKeys.Remove(lastKey);
            }
        }
    }

    //Receive objects
    public struct UserInfoObject
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int UserLevel { get; set; }
        public int DesignerLevel { get; set; }
        public List<Badge> Badges { get; set; }
        public string? Intro { get; set; }
        public int ChartAmount { get; set; }
    }
    public class UserSearchResultObject
    {
        public int Amount { get; set; }
        public List<UserSearchItem> Users { get; set; }
    }
    public struct LoginQuestObject
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
    public struct RegeistQuestObject
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string EmailKey { get; set; }
    }
    public struct VerifyEmailQuestObject
    {
        public string? Token { get; set; }
        public int QuestClass { get; set; }
        public string Email { get; set; }
    }

    //Send objects
    public struct ReturnLoginQuest
    {
        public int UserId { get; set; }
        public string Token { get; set; }
    }

    //Internal objects
    public class UserSearchItem
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? AvatorSrc { get; set; }
        public int ChartAmount { get; set; }
    }
}