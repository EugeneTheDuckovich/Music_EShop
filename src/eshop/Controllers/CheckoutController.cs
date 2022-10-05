using eshop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;

namespace eshop.Controllers
{
    public class CheckoutController : Controller
    {
        [Authorize]
        public IActionResult Index()
        {
            // Fetch all products from the cart
            // Pass the products into the ViewData

            //if(HttpContext.Session.Get("cart") == null)
            //{
            //    // Error! the cart is empty.
            //    return View("Checkout", null);
            //}

            //JsonSerializerOptions options = new()
            //{
            //    ReferenceHandler = ReferenceHandler.Preserve,
            //    WriteIndented = true
            //};
            //instruments = JsonSerializer.Deserialize<List<Instrument>>(HttpContext.Session.GetString("cart"), options);

            PurchaseProduct[] instruments;

            using (var context = new EShopFullDbContext())
            {
                var user = context.Users
                    .Include(u => u.Purchases)
                    .ThenInclude(p => p.PurchaseProducts)
                    .ThenInclude(p => p.Instrument)
                    .First(u => u.Email == User.Identity.Name);
                if(user.Purchases.FirstOrDefault(p => p.Status != "completed") == null)
                {
                    return View("Checkout", null);
                }
                var currentPurchase = user.Purchases.FirstOrDefault(p => p.Status != "completed");

                instruments = currentPurchase.PurchaseProducts.ToArray();
            }

            //ViewData["cart"] = instruments.ToArray();

            return View("Checkout", instruments);
        }

        [HttpPost]
        public IActionResult Confirm()
        {
            using (var context = new EShopFullDbContext())
            {
                var user = context.Users
                    .Include(u => u.Purchases)
                    .ThenInclude(p => p.PurchaseProducts)
                    .ThenInclude(p => p.Instrument)
                    .First(u => u.Email == User.Identity.Name);
                var currentPurchase = user.Purchases.First(p => p.Status != "completed");
                currentPurchase.Status = "completed";
                context.SaveChanges();
            }

            return Redirect("/Home/Index");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            PurchaseProduct[] model;

            using (var context = new EShopFullDbContext())
            {
                var user = context.Users
                    .Include(u => u.Purchases)
                    .ThenInclude(p => p.PurchaseProducts)
                    .ThenInclude(p => p.Instrument)
                    .First(u => u.Email == User.Identity.Name);
                var currentPurchase = user.Purchases.FirstOrDefault(p => p.Status != "completed");

                currentPurchase.PurchaseProducts.Remove(context.PurchaseProducts.Find(id));
                context.SaveChanges();

                model = currentPurchase.PurchaseProducts.ToArray();
            }
            return Index();
        }
    }
}