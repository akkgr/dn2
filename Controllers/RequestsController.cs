using cinnamon.api.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;

namespace cinnamon.api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class RequestsController : Controller
    {
        private readonly Context db;
        private readonly IOptions<AppOptions> _options;

        public RequestsController(IOptions<AppOptions> options, Context ctx)
        {
            this.db = ctx;
            _options = options;
        }

        // GET: api/values
        [HttpGet]
        public async Task<IEnumerable<Request>> Get()
        {
            var list = await db.Requests.Find(t => true).SortBy(x => x.Inserted).ToListAsync();
            var rt = await db.RequestTypes.Find(t => true).ToListAsync();

            foreach (var item in list)
            {
                var c = rt.FirstOrDefault(t=>t.Id == item.RequestTypeId);
                item.RequestType = c.Title ;
            }
            return list;
        }

        // GET api/values/5
        [HttpGet]
        public async Task<IActionResult> Get(string id)
        {
            if (id == "new")
            {
                var obj = new Request();
                obj.Inserted = DateTime.Now;
                obj.UserName = User.Identity.Name;
                obj.History.Add(new RequestHistory() { Occurred = DateTime.Now, Status = RequestStatus.Open, UserName = User.Identity.Name });
                
                long res = 0;
                do
                {
                    obj.IdCode = Helper.GetBase36(6);
                    var query = Builders<Request>.Filter.Eq(e => e.IdCode, obj.IdCode);
                    res = await db.Requests.CountAsync(query);
                }
                while (res > 0);

                return this.Ok(obj);
            }
            else
            {
                var obj = await db.Requests.Find(t => t.Id == id).FirstOrDefaultAsync();
                if (obj == null)
                {
                    return this.NotFound();
                }
                return this.Ok(obj);
            }
        }

        // POST api/values
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Request value)
        {
            await db.Requests.InsertOneAsync(value);
            return this.Ok(value);
        }

        // PUT api/values/5
        [HttpPut]
        public async Task<IActionResult> Put(string id, [FromBody]Request value)
        {
            var obj = await db.Requests.Find(t => t.Id == id).FirstOrDefaultAsync();
            if (obj == null)
            {
                return this.NotFound();
            }

            var query = Builders<Request>.Filter.Eq(e => e.Id, id);
            await db.Requests.ReplaceOneAsync(query, value);
            return this.Ok(value);
        }

        // DELETE api/values/5
        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            var request = await db.Requests.Find(t => t.Id == id).FirstOrDefaultAsync();

            if (request == null)
            {
                return NotFound();
            }

            if (request.History.Count > 1)
            {
                return this.BadRequest(_options.Value.Error.RepairDelete);
            }

            await db.Requests.FindOneAndDeleteAsync(t => t.Id == id);
            return this.Ok();
        }
    }
}