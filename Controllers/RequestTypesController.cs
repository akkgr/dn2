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
    public class RequestTypesController : Controller
    {
        private readonly Context db;
        private readonly IOptions<AppOptions> _options;

        public RequestTypesController(IOptions<AppOptions> options, Context ctx)
        {
            this.db = ctx;
            _options = options;
        }

        // GET: api/values
        [HttpGet]
        public async Task<IEnumerable<RequestType>> Get()
        {
            return await db.RequestTypes.Find(t => true).ToListAsync();
        }

        // GET api/values/5
        [HttpGet]
        public async Task<IActionResult> Get(string id)
        {
            var obj = await db.RequestTypes.Find(t => t.Id == id).FirstOrDefaultAsync();
            if (obj == null)
            {
                return this.NotFound();
            }
            return this.Ok(obj);
        }

        // POST api/values
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]RequestType value)
        {
            await db.RequestTypes.InsertOneAsync(value);
            return this.Ok(value);
        }

        // PUT api/values/5
        [HttpPut]
        public async Task<IActionResult> Put(string id, [FromBody]RequestType value)
        {
            var obj = await db.RequestTypes.Find(t => t.Id == id).FirstOrDefaultAsync();
            if (obj == null)
            {
                return this.NotFound();
            }

            var query = Builders<RequestType>.Filter.Eq(e => e.Id, id);
            await db.RequestTypes.ReplaceOneAsync(query, value);
            return this.Ok(value);
        }

        // DELETE api/values/5
        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            var filter = Builders<Request>.Filter.Eq("RequestType.Id", id);
            var result = await db.Requests.Find(filter).CountAsync();
            if (result > 0)
            {
                return this.BadRequest(_options.Value.Error.TypeHasRequests);
            }

            await db.RequestTypes.FindOneAndDeleteAsync(t => t.Id == id);
            return this.Ok();
        }
    }
}