using eshop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eshop.Controllers
{
    public class DetailsController : Controller
    {
        public IActionResult Index(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Instrument? instrument;
            using (var context = new EShopFullDbContext())
            {
                instrument = context.Instruments.Include(i => i.Guitar)
                    .Include(i => i.Keyboard)
                    .FirstOrDefault(m => m.Id == id);
            }

            if (instrument == null)
            {
                return NotFound();
            }

            return View("Details",instrument);
        }
    }
}
