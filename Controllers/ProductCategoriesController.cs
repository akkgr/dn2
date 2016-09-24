using cinnamon.api.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using cinnamon.api;

namespace cinnamon.api.Controllers
{
    public class ProductCategoriesController : Controller
    {
        private readonly Context db;
        private readonly IOptions<AppOptions> _options;

        public ProductCategoriesController(IOptions<AppOptions> options, Context ctx)
        {
            this.db = ctx;
            _options = options;
        }

        // GET: api/values
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var list = await db.ProductCategories.Find(t => true).ToListAsync();
            return Ok(list);
        }
        
        // GET api/values/5
        [HttpGet]
        public async Task<IActionResult> Get(string id)
        {
            var obj = await db.ProductCategories.Find(t => t.Id == id).FirstOrDefaultAsync();
            if (obj == null)
            {
                return this.NotFound();
            }
            return this.Ok(obj);
        }

        // POST api/values
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]ProductCategory value)
        {
            await db.ProductCategories.InsertOneAsync(value);
            return this.Ok(value);
        }

        // PUT api/values/5
        [HttpPut]
        public async Task<IActionResult> Put(string id, [FromBody]ProductCategory value)
        {
            var obj = await db.ProductCategories.Find(t => t.Id == id).FirstOrDefaultAsync();
            if (obj == null)
            {
                return this.NotFound();
            }

            var query = Builders<ProductCategory>.Filter.Eq(e => e.Id, id);
            await db.ProductCategories.ReplaceOneAsync(query, value);
            return this.Ok(value);
        }

        // DELETE api/values/5
        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            {
                var builder = Builders<ProductCategory>.Filter;
                var filter = builder.Eq("ParentId", id);
                var result = await db.ProductCategories.Find(filter).CountAsync();
                if (result > 0)
                {
                    return this.BadRequest(_options.Value.Error.ProductCategoryHasDocuments);
                }
            }

            {
                var builder = Builders<Product>.Filter;
                var filter = builder.Eq("Categories.Id", id);
                var result = await db.Products.Find(filter).CountAsync();
                if (result > 0)
                {
                    return this.BadRequest(_options.Value.Error.ProductCategoryHasDocuments);
                }
            }

            await db.ProductCategories.FindOneAndDeleteAsync(t => t.Id == id);
            return this.Ok();
        }
    }
}