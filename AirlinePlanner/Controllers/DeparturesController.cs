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
        [HttpGet("/departures/new")]
        public ActionResult CreateForm()
        {
            return View();
        }
        [HttpPost("/create-departure-city")]
        public ActionResult Create()
        {
            DepartureCity newDepartureCity = new DepartureCity(Request.Form["departure-city"]);
            newDepartureCity.Save();
            return View("Success", "Home");
        }
        [HttpGet("/departures/{id}")]
        public ActionResult ViewDepartures(int id)
        {
            Dictionary<string, object> model = new Dictionary<string, object>();
            DepartureCity selectedDepartureCity = DepartureCity.Find(id);
            List<ArrivalCity> flights = selectedDepartureCity.GetArrivals();
            List<ArrivalCity> allArrivals = ArrivalCity.GetAll();
            model.Add("selectedDepartureCity", selectedDepartureCity);
            model.Add("flights", flights);
            model.Add("allArrivals", allArrivals);
            return View(model);
        }
        [HttpPost("/departures/{arrivalId}/arrivals/new")]
        public ActionResult AddArrivalToDeparture(int arrivalId)
        {
            DepartureCity departure = DepartureCity.Find(arrivalId);
            ArrivalCity arrival = ArrivalCity.Find(Int32.Parse(Request.Form["arrival-id"]));
            departure.SetArrivals(arrival); //Want to run the join table method
            return RedirectToAction("ViewDepartures",  new { id = arrivalId });
        }
    }
}
