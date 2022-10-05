using eshop.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eshop.Controllers
{
    public class CategoriesController : Controller
    {
        // GET: CategoriesController
        public IActionResult Index(string category)
        {
            Instrument[] instruments = Enumerable.Empty<Instrument>().ToArray();
            using(var context = new EShopFullDbContext())
            {
                switch (category)
                {
                    case "guitars":
                        if(context.Guitars.Count() >0)
                            instruments = context.Guitars.Include(g => g.Instrument).Select(g => g.Instrument).ToArray();
                        break;
                    case "keyboards":
                        if (context.Keyboards.Count() > 0)
                            instruments = context.Keyboards.Include(g => g.Instrument).Select(g => g.Instrument).ToArray();
                        break;
                    default:
                        instruments = Enumerable.Empty<Instrument>().ToArray();
                        break;
                }
            }
            return View("Categories",instruments);
        }
    }
}
