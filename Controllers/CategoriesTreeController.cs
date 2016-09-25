using cinnamon.api.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace cinnamon.api.Controllers
{
    [Authorize]
    public class CategoriesTreeController : Controller
    {
        private readonly Context db;

        public CategoriesTreeController(Context ctx)
        {
            this.db = ctx;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var list = await db.ProductCategories.Find(t => true).ToListAsync();
            list.ForEach(t => t.Categories.AddRange(list.Where(c => c.ParentId == t.Id).OrderBy(o => o.Title)));
            var pc = new ProductCategory();
            pc.Title = "Categories";
            pc.Categories.AddRange(list.Where(t => t.ParentId == null).OrderBy(o => o.Title));
            var rtn = new List<ProductCategory>();
            rtn.Add(pc);
            return Ok(rtn);
        }
    }
}
