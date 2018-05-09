
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
            return View("Success", "Home");
        }

        [HttpGet("/arrivals/{id}")]
        public ActionResult ViewArrivals(int id)
        {
            Dictionary<string, object> model = new Dictionary<string, object>();
            ArrivalCity selectedArrivalCity = ArrivalCity.Find(id);
            List<Flight> flights = selectedArrivalCity.GetInboundFlights();
            List<DepartureCity> allDepartures = DepartureCity.GetAll();
            model.Add("selectedArrivalCity", selectedArrivalCity);
            model.Add("flights", flights);
            model.Add("allDepartures", allDepartures);
            return View(model);
        }
        [HttpPost("/arrivals/{departureId}/departures/new")]
        public ActionResult AddDepartureToArrival(int departureId)
        {
            ArrivalCity arrival = ArrivalCity.Find(departureId);
            DepartureCity departure = DepartureCity.Find(Int32.Parse(Request.Form["departure-id"]));
            arrival.SetDepartures(departure); //Want to run the join table method
            return RedirectToAction("ViewArrivals",  new { id = departureId });
        }

        [HttpGet("/arrivals/{id}/delete")]
        public ActionResult Delete(int id)
        {
            Dictionary<string, object> model = new Dictionary<string, object>();
            Flight selectedFlight = Flight.Find(id);
            int arrivalCityId = selectedFlight.GetArrivalCityId();
            selectedFlight.DeleteFlight();
            return RedirectToAction("ViewArrivals",  new { id = arrivalCityId });


        }
    }
}
