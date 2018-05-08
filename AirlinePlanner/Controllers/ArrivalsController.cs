
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using AirlinePlanner.Models;
using System;

namespace AirlinePlanner.Controllers
{
    public class ArrivalsController : Controller
    {
        [HttpGet("/arrivals")]
        public ActionResult Index()
        {
            List<ArrivalCity> allArrivalCities = ArrivalCity.GetAll();
            return View(allArrivalCities);
        }
        [HttpGet("/arrivals/new")]
        public ActionResult CreateForm()
        {
            return View();
        }
        [HttpPost("/create-arrival-city")]
        public ActionResult Create()
        {
            ArrivalCity newArrivalCity = new ArrivalCity(Request.Form["arrival-city"]);
            newArrivalCity.Save();
            return RedirectToAction("Success", "Home");
        }
    }
}
