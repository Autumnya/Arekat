using Arekat.Server.Models;
using Azure.Core;
using Microsoft.AspNetCore.Mvc;

namespace Arekat.Server.Controllers
{
    [ApiController]
    [Route("storeuid-management")]
    public class StoreUidController(BaseContext context, ILogger<StoreUidController> logger, IConfiguration config) : ControllerBase
    {
        private readonly BaseContext _context = context;
        private readonly ILogger<StoreUidController> _logger = logger;
        private readonly IConfiguration _config = config;

        [HttpPost("fill-uid-factor")]
        public async Task<IActionResult> FillStoreAsync([FromQuery] float factor)
        {
            int currentMaxUid = _context.StoreUids.Max(x => x.Id);
            Task<List<StoreUid>> newIdList = GenerateStoreUidAsync(currentMaxUid, factor);

            await newIdList;
            _context.BulkInsert(newIdList.Result);
            return Ok($"Filled {newIdList.Result.Count} new userId");
        }

        [HttpPost("fill-uid-amount")]
        public async Task<IActionResult> FillStoreAsync([FromQuery] int amount)
        {
            int countId = _context.StoreUids.Count();
            int currentMaxUid = 100000;
            if (countId > 0)
                currentMaxUid = _context.StoreUids.Max(x => x.Id);
            Task<List<StoreUid>> newIdList = GenerateStoreUidAsync(currentMaxUid, amount);

            await newIdList;
            _context.BulkInsert(newIdList.Result);
            return Ok($"Filled {newIdList.Result.Count} new userId");
        }



        private async Task<List<StoreUid>> GenerateStoreUidAsync(int currentMaxId, float expandFactor)
        {
            List<StoreUid> uidList = new List<StoreUid>();
            var task = Task.Run(() =>
            {
                for (int newId = currentMaxId + 1; newId <= currentMaxId * expandFactor; newId++)
                {
                    uidList.Add(new StoreUid { Guid = Guid.NewGuid(), Id = newId });
                }
            });
            await task;
            return uidList;
        }
        private async Task<List<StoreUid>> GenerateStoreUidAsync(int currentMaxId, int amount)
        {
            List<StoreUid> uidList = new List<StoreUid>();
            var task = Task.Run(() =>
            {
                for (int newId = currentMaxId + 1; newId <= currentMaxId + amount; newId++)
                {
                    uidList.Add(new StoreUid { Guid = Guid.NewGuid(), Id = newId });
                    _logger.LogInformation("{message1} {message2}", newId, amount);
                }
            });
            await task;
            return uidList;
        }
        private bool AddOneId(int newId)
        {
            try
            {
                _context.StoreUids.Add(new StoreUid
                {
                    Guid = Guid.NewGuid(),
                    Id = newId,
                });
            }
            catch (Exception ex)
            {
                _logger.LogError("{message}", ex.Message);
                return false;
            }
            return true;
        }
    }

}
