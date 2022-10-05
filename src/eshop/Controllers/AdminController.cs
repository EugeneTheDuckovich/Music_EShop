using eshop.Models;
using eshop.ViewsModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eshop.Controllers;

[Authorize]
public class AdminController : Controller
{
    IWebHostEnvironment _env;

    public AdminController(IWebHostEnvironment env)
    {
        _env = env;
    }

    // GET: AdminController
    public IActionResult Index()
    {
        return View();
    }


    public IActionResult CRUD(string category)
    {
        Instrument[] model;
        if (category == "guitars")
        {
            using (var context = new EShopFullDbContext())
            {
                model = context.Guitars.Include(g => g.Instrument).Select(g => g.Instrument).ToArray();
            }
            return View("GuitarsCRUD", model);
        }
        else if (category == "keyboards")
        {
            using (var context = new EShopFullDbContext())
            {
                model = context.Keyboards.Include(g => g.Instrument).Select(g => g.Instrument).ToArray();
            }
            return View("KeyboardsCRUD", model);
        }
        else return Redirect("'Home/Index");
    }

    // GET: AdminController/Create
    //[Authorize(Roles = "Admin")]
    [HttpGet]
    public IActionResult CreateGuitar()
    {
        return View();
    }

    //[Authorize(Roles = "Admin")]
    [HttpPost]
    public IActionResult CreateGuitar( GuitarCreationViewModel viewmodel)
    {
        using(var context = new EShopFullDbContext())
        {
            context.Instruments.Add(new Instrument
            {
                Producer = viewmodel.Producer,
                Model = viewmodel.Model,
                BriefDescription = viewmodel.BriefDescription,
                ImageSource = "",
                Price = viewmodel.Price
            });
            context.SaveChanges();
            var instrument = context.Instruments.AsEnumerable().MaxBy(i => i.Id);
            context.Guitars.Add(new Guitar
            {
                Instrument = instrument,
                BodyMaterial = viewmodel.BodyMaterial,
                DeckMaterial = viewmodel.DeckMaterial,
                NumberOfStrings = viewmodel.NumberOfStrings
            });
            context.SaveChanges();
            if (HttpContext.Request.Form.Files.Count > 0) SaveImage(instrument.Id);
        }

        return Index();
    }

    // POST: AdminController/Delete/5
    //[Authorize(Roles = "Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteGuitar(int id)
    {
        using (var context = new EShopFullDbContext())
        {
            var instrument = context.Instruments.Find(id);
            if(instrument == null) return Index();
            context.Instruments.Remove(context.Instruments.Find(id));
            context.SaveChanges();
        }
        System.IO.File.Delete($"{_env.WebRootPath}/images/{id}.jpg");

        return CRUD("guitars");
    }

    // GET: AdminController/Create
    //[Authorize(Roles = "Admin")]
    [HttpGet]
    public IActionResult Createkeyboard()
    {
        return View();
    }

    //[Authorize(Roles = "Admin")]
    [HttpPost]
    public IActionResult CreateKeyboard(KeyboardCreationViewModel viewmodel)
    {
        using (var context = new EShopFullDbContext())
        {
            context.Instruments.Add(new Instrument
            {
                Producer = viewmodel.Producer,
                Model = viewmodel.Model,
                BriefDescription = viewmodel.BriefDescription,
                ImageSource = "",
                Price = viewmodel.Price
            });
            context.SaveChanges();
            var instrument = context.Instruments.AsEnumerable().MaxBy(i => i.Id);
            context.Keyboards.Add(new Keyboard
            {
                Instrument = instrument,
                Material = viewmodel.Material,
                NumberOfOctaves = viewmodel.NumberOfOctaves
            });
            context.SaveChanges();
            if (HttpContext.Request.Form.Files.Count > 0) SaveImage(instrument.Id);
        }

        return CRUD("keyboards");
    }

    // POST: AdminController/Delete/5
    //[Authorize(Roles = "Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteKeyboard(int id)
    {
        using (var context = new EShopFullDbContext())
        {
            var instrument = context.Instruments.Find(id);
            if (instrument == null) CRUD("keyboards");
            context.Instruments.Remove(context.Instruments.Find(id));
            context.SaveChanges();

        }
        System.IO.File.Delete($"{_env.WebRootPath}/images/{id}.jpg");
        return CRUD("keyboards");
    }

    private async void SaveImage(int id)
    {
        await using var fs = new FileStream($"{_env.WebRootPath}/images/{id}.jpg",
            FileMode.Create);
        await HttpContext.Request.Form.Files[0].CopyToAsync(fs);
    }
}