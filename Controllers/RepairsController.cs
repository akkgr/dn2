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
    public class RepairsController : Controller
    {
        private readonly Context db;
        private readonly IOptions<AppOptions> _options;

        public RepairsController(IOptions<AppOptions> options, Context ctx)
        {
            this.db = ctx;
            _options = options;
        }

        // GET: api/values
        [HttpGet]
        public async Task<IEnumerable<Repair>> Get()
        {
            return await db.Repairs.Find(t => true).SortByDescending(x=>x.History[0].Occurred).ToListAsync();
        }

        // GET api/values/5
        [HttpGet]
        public async Task<IActionResult> Get(string id)
        {
            if (id == "new")
            {
                var obj = new Repair();
                obj.History.Add(new RepairHistory() { Occurred = DateTime.Now, Status = RepairStatus.Accepted });
                
                long res = 0;
                do
                {
                    obj.IdCode = Helper.GetBase36(6);
                    var query = Builders<Repair>.Filter.Eq(e => e.IdCode, obj.IdCode);
                    res = await db.Repairs.CountAsync(query);
                }
                while (res > 0);

                return this.Ok(obj);
            }
            else
            {
                var obj = await db.Repairs.Find(t => t.Id == id).FirstOrDefaultAsync();
                if (obj == null)
                {
                    return this.NotFound();
                }
                return this.Ok(obj);
            }
        }

        // POST api/values
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Repair value)
        {
            await db.Repairs.InsertOneAsync(value);
            return this.Ok(value);
        }

        // PUT api/values/5
        [HttpPut]
        public async Task<IActionResult> Put(string id, [FromBody]Repair value)
        {
            var obj = await db.Repairs.Find(t => t.Id == id).FirstOrDefaultAsync();
            if (obj == null)
            {
                return this.NotFound();
            }

            var query = Builders<Repair>.Filter.Eq(e => e.Id, id);
            await db.Repairs.ReplaceOneAsync(query, value);
            return this.Ok(value);
        }

        // DELETE api/values/5
        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            var repair = await db.Repairs.Find(t => t.Id == id).FirstOrDefaultAsync();

            if (repair == null)
            {
                return NotFound();
            }

            if (repair.History.Count > 1)
            {
                return this.BadRequest(_options.Value.Error.RepairDelete);
            }

            await db.Repairs.FindOneAndDeleteAsync(t => t.Id == id);
            return this.Ok();
        }
    }
}