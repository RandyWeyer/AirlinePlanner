using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using AirlinePlanner.Models;
using System;

namespace AirlinePlanner.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet("/")]
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet("/success")]
        public ActionResult Success()
        {
            return View("Success");
        }
    }
}
