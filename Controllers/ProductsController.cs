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
    public class ProductsController : Controller
    {
        private readonly Context db;
        private readonly IOptions<AppOptions> _options;

        public ProductsController(IOptions<AppOptions> options, Context ctx)
        {
            this.db = ctx;
            _options = options;
        }

        // GET: api/values
        [HttpGet]
        public async Task<IEnumerable<Product>> Get()
        {
            var products = await db.Products.Find(t => true).ToListAsync();
            var categories = await db.ProductCategories.Find(t => true).ToListAsync();

            foreach(var p in products)
            {
                var c = categories.Where(t => p.Categories.Contains(t.Id));
                p.AllCategories = string.Join(", ", c.Select(o => o.Title));
                p.Service = c.Any(t => t.Service == true);
            }
            
            return products;
        }

        // GET api/values/5
        [HttpGet]
        public async Task<IActionResult> Get(string id)
        {
            var obj = await db.Products.Find(t => t.Id == id).FirstOrDefaultAsync();
            if (obj == null)
            {
                return this.NotFound();
            }
            return this.Ok(obj);
        }

        // POST api/values
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Product value)
        {
            await db.Products.InsertOneAsync(value);
            return this.Ok(value);
        }

        // PUT api/values/5
        [HttpPut]
        public async Task<IActionResult> Put(string id, [FromBody]Product value)
        {
            var obj = await db.Products.Find(t => t.Id == id).FirstOrDefaultAsync();
            if (obj == null)
            {
                return this.NotFound();
            }

            var query = Builders<Product>.Filter.Eq(e => e.Id, id);
            await db.Products.ReplaceOneAsync(query, value);
            return this.Ok(value);
        }

        // DELETE api/values/5
        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            var builder = Builders<Repair>.Filter;
            var filter = builder.Eq("Product.Id", id) | builder.Eq("Products.Id", id);
            var result = await db.Repairs.Find(filter).CountAsync();
            if (result > 0)
            {
                return this.BadRequest(_options.Value.Error.ProductHasRepairs);
            }

            await db.Products.FindOneAndDeleteAsync(t => t.Id == id);
            return this.Ok();
        }
    }
}