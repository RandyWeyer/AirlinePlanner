
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
    }
}
