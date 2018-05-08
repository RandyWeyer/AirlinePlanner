using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using AirlinePlanner.Models;
using System;

namespace AirlinePlanner.Controllers
{
    public class DeparturesController : Controller
    {
        [HttpGet("/departures")]
        public ActionResult Index()
        {
            List<DepartureCity> allDepartureCities = DepartureCity.GetAll();
            return View(allDepartureCities);
        }
    }
}
