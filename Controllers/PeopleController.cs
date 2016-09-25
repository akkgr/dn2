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
    [Route("api/people")]
    public class PeopleController : Controller
    {
        private readonly Context db;
        private readonly IOptions<AppOptions> _options;

        public PeopleController(IOptions<AppOptions> options, Context ctx)
        {
            this.db = ctx;
            _options = options;
        }

        // GET: api/values
        [HttpGet]
        public async Task<IEnumerable<Person>> Get()
        {
            return await db.People.Find(t => true).ToListAsync();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var obj = await db.People.Find(t => t.Id == id).FirstOrDefaultAsync();
            if (obj == null)
            {
                return this.NotFound();
            }
            return this.Ok(obj);
        }

        // POST api/values
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Person value)
        {
            await db.People.InsertOneAsync(value);
            return this.Ok(value);
        }

        // PUT api/values/5
        [HttpPut]
        public async Task<IActionResult> Put(string id, [FromBody]Person value)
        {
            var obj = await db.People.Find(t => t.Id == id).FirstOrDefaultAsync();
            if (obj == null)
            {
                return this.NotFound();
            }

            var query = Builders<Person>.Filter.Eq(e => e.Id, id);
            await db.People.ReplaceOneAsync(query, value);
            return this.Ok(value);
        }

        // DELETE api/values/5
        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            var filter = Builders<Repair>.Filter.Eq("Customer.Id", id);
            var result = await db.Repairs.Find(filter).CountAsync();
            if (result > 0)
            {
                return this.BadRequest(_options.Value.Error.CustomerHasRepairs);
            }

            await db.People.FindOneAndDeleteAsync(t => t.Id == id);
            return this.Ok();
        }
    }
}