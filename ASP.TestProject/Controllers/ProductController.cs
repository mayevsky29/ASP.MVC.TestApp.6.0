using ASP.TestProject.Data;
using ASP.TestProject.Models;
using Microsoft.AspNetCore.Mvc;

namespace ASP.TestProject.Controllers;

public class ProductController : Controller
{
    private readonly ApplicationDbContext _db;
    public ProductController(ApplicationDbContext db)
    {
        _db = db;
    }

    [Route("api/product")]
    public IActionResult Index()
    {
        IEnumerable<Product> objProductList = _db.Products;
        return View(objProductList);
    }

    // GET
    public IActionResult Create()
    {
        return View();
    }

    //POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Product obj)
    {
        if (obj.Name == obj.Description.ToString())
        {
            ModelState.AddModelError("name", "The DisplayOrder cannot exactly match the Name.");
        }
        if (ModelState.IsValid)
        {
            _db.Products.Add(obj);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
        return View(obj);
    }

    [HttpGet]
    public IActionResult GetPage([FromQuery] SearchViewModel model)
    {
        int count = 2;
        int skipped = model.Page != 0 ? (model.Page - 1) * count : 0;
        string search = model.Search != null ? model.Search : "";
        var returnProducts = _db.Products.Where(x =>
     x.Name.Contains(search) ||
     x.Description.Contains(search));
        return Ok(new
        {
            data = new
            {
                data = returnProducts.Select(x => x).Skip(skipped)
            .Take(count).ToList(),
                current_page = model.Page,
                last_page = (Math.Ceiling((decimal)returnProducts.Count() / count)),
                total = returnProducts.Count(),
                per_page = count
            },
            search = model.Search
        });
    }

}

