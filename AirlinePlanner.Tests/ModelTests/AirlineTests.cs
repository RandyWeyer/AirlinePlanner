using Microsoft.VisualStudio.TestTools.UnitTesting;
using AirlinePlanner.Models;
using System;
using MySql.Data.MySqlClient;

namespace AirlinePlanner.Tests
{
    [TestClass]
    public class ArrivalCityTests
    {

        public ArrivalCityTests()
        {
            DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=airline_planner_test;";
        }

        [TestMethod]
        public void Arrival_City_is_Empty()
        {
            int result = ArrivalCity.GetAll().Count;

            Assert.AreEqual(10, result);
        }
    }

    [TestClass]
    public class DepartureCityTests
    {

        public DepartureCityTests()
        {
            DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=airline_planner_test;";
        }

        [TestMethod]
        public void DepartureCity_returns_Seattle()
        {
            DepartureCity result = DepartureCity.Find(1);
            string finalResult = result.GetDepartureCity();

            Assert.AreEqual("Seattle", finalResult);
        }
    }
}
