using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GameWebApi.Controllers
{
    [ApiController]
    [Route("api/players/{playerid}/items")]
    public class ItemsController : ControllerBase
    {
        private readonly ILogger<ItemsController> _logger;
        private readonly IRepository _repository;

        public void ConfigureServices()
        {

        }

        public ItemsController(ILogger<ItemsController> logger, IRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [HttpGet]
        [Route("getall")]
        public Task<Item[]> GetAll(int minlevel)
        {
            return Task.FromResult(new Item[] { new Item() { Level = minlevel },
                                    new Item() { Level = (minlevel + 1) } });
        }

        [HttpGet]
        [Route("{PlayerId}")]
        public Task<Item> GetItem(Guid id, Guid ItemId)
        {
            return _repository.GetItem(id, ItemId);
        }

        [HttpPost]
        [Route("create")]
        public async Task<Item> CreateItem([FromBody] NewItem Item)
        {
            DateTime localDate = DateTime.UtcNow;

            Item new_Item = new Item();
            new_Item.Type = 0;
            new_Item.Level = 0;
            if (new_Item.Level < 1 || new_Item.Level > 99)
            {
                return null;
            }

            new_Item.CreationTime = localDate;

            //await _repository.CreateItem();
            return new_Item;
        }

        [HttpPost]
        [Route("{PlayerId}/modify/{id:Guid}")]
        public async Task<Item> Modify(Guid id, [FromBody] ModifiedItem Item)
        {
            // await _repository.Modify(id, Item);
            return null;
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<Item> Delete(Guid id)
        {
            await _repository.Delete(id);
            return null;
        }
    }
}