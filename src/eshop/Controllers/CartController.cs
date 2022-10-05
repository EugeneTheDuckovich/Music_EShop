using eshop.Data;
using eshop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text;
using eshop.Models;
using Microsoft.EntityFrameworkCore;

namespace eshop.Controllers
{
    public class CartController : Controller
    {
        // Todo: convert this to API controller?
        // Todo: after adding the instrument to the cart redirect to the same page where the user was initially on

        [Authorize]
        public IActionResult AddItem(int id)
        {
            List<Instrument> instruments = new List<Instrument>();

            //if(HttpContext.Session.GetString("cart") != null)
            //{
            //    JsonSerializerOptions serializerOptions = new()
            //    {
            //        ReferenceHandler = ReferenceHandler.Preserve,
            //        WriteIndented = true
            //    };
            //    instruments = JsonSerializer.Deserialize<List<Instrument>>(HttpContext.Session.GetString("cart"), serializerOptions);
            //}

            using (var context = new EShopFullDbContext())
            {
                //var instrument = context.Instruments.Find(id);
                //if (instrument != null)
                //{
                //    instruments.Add(instrument);
                //}

                var user = context.Users
                    .Include(u => u.Purchases)
                    .ThenInclude(p => p.PurchaseProducts)
                    .ThenInclude(p => p.Instrument)
                    .First(u => u.Email == User.Identity.Name);

                if (user.Purchases.FirstOrDefault(p => p.Status != "completed") == null)
                {
                    user.Purchases.Add(new Purchase
                    {
                        User = user,
                        Status = "ongoing"
                    });
                    context.SaveChanges();
                }

                var currentPurchase = user.Purchases.First(p => p.Status == "ongoing");
                if (currentPurchase.PurchaseProducts.FirstOrDefault(p => p.Instrument.Id == id) == null) 
                {
                    currentPurchase.PurchaseProducts.Add(new PurchaseProduct
                    {
                        Purchase = currentPurchase,
                        Instrument = context.Instruments.Find(id),
                        Amount = 1
                    });
                    context.SaveChanges();
                }
                else
                {
                    ++currentPurchase.PurchaseProducts.FirstOrDefault(p => p.Instrument.Id == id).Amount;
                    context.SaveChanges();
                }
            }
            
            // Avoid cycles.
            //JsonSerializerOptions options = new()
            //{
            //    ReferenceHandler = ReferenceHandler.Preserve,
            //    WriteIndented = true
            //};
            //HttpContext.Session.SetString("cart", JsonSerializer.Serialize<List<Instrument>>(instruments, options));

            //Instrument[] model;
            //using (var context = new EShopFullDbContext())
            //{
            //    model =  context.Instruments.ToArray();
            //}

            return Redirect($"/Details/Index/{id}");
        }
    }
}