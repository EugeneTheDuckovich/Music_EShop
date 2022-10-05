﻿using System.Diagnostics;
using eshop.Data;
using eshop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eshop.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        Instrument[] instruments;
        using(var context = new EShopFullDbContext())
        {
            instruments = context.Instruments.ToArray();
        }
        return View("HomePage", instruments);
    }

    [Authorize]
    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult Payment()
    {
        return View();
    }

    //public IActionResult Charge(string stripeEmail,string stripeToken)
    //{
    //    var customers = new CustomerService();
    //    var charges = new ChargeService();

    //    var customer = customers.Create(new CustomerCreateOptions
    //    {
    //        Email = stripeEmail,
    //        Source = stripeToken
    //    });

    //    var charge = charges.Create(new ChargeCreateOptions
    //    {
    //        Amount = 500,
    //        Description = "Test Payment",
    //        Currency = "inr",
    //        Customer = customer.Id,
    //        ReceiptEmail=stripeEmail,
    //        Metadata=new Dictionary<string, string>() 
    //        {
    //            {"OrderId","111" },
    //            { "PostalCode","387810"}
    //        }
    //    });

    //    if (charge.Status == "succeeded")
    //    {
    //        string BalanceTransactionId = charge.BalanceTransactionId;
    //        return View();
    //    }
    //    else
    //    {

    //    }
    //    return View();
    //}

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}