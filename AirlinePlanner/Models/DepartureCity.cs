using System.Collections.Generic;
using MySql.Data.MySqlClient;
using AirlinePlanner;
using System;
using Microsoft.AspNetCore.Mvc;

namespace AirlinePlanner.Models
{
    public class DepartureCity
    {
        private string _departureCity;
        private int _id;

        public DepartureCity(string DepartureCity, int Id = 0)
        {
            _departureCity = DepartureCity;
            _id = Id;
        }

        public int GetId()
        {
          return _id;
        }

        public string GetDepartureCity()
        {
          return _departureCity;
        }

        public static List<DepartureCity> GetAll()
        {
            List<DepartureCity> allDepartureCity = new List<DepartureCity> {};
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM departure_cities;";
            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            while(rdr.Read())
            {
              int departureCityId = rdr.GetInt32(0);
              string departureCity = rdr.GetString(1);
              DepartureCity newDepartureCity = new DepartureCity(departureCity, departureCityId);
              allDepartureCity.Add(newDepartureCity);
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return allDepartureCity;
        }

        public static List<ArrivalCity> GetFlights()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT arrival_cities.* FROM departure_cities
                JOIN flights ON (departure_cities.id = flights.departure_city_id)
                JOIN items ON (flights.arrival_city_id = arrival_cities.id)
                WHERE departure_cities.id = @DepartureCityId;";

            MySqlParameter categoryIdParameter = new MySqlParameter();
            categoryIdParameter.ParameterName = "@DepartureCityId";
            categoryIdParameter.Value = _id;
            cmd.Parameters.Add(departureCityIdParameter);

            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
            List<ArrivalCity> arrivalCities = new List<ArrivalCity>{};

            while(rdr.Read())
            {
              int arrivalCityId = rdr.GetInt32(0);
              string arrivalCityName = rdr.GetString(1);
              ArrivalCity newArrivalCity = new Arrival(arrivalCityName, arrivalCityId);
              items.Add(newItem);
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return arivalCities;
        }

        public static DepartureCity Find(int id)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM departure_cities WHERE id = (@searchId);";

            MySqlParameter searchId = new MySqlParameter();
            searchId.ParameterName = "@searchId";
            searchId.Value = id;
            cmd.Parameters.Add(searchId);

            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            int departureCityId = 0;
            string departureCity = "";

            while(rdr.Read())
            {
              departureCityId = rdr.GetInt32(0);
              departureCity = rdr.GetString(1);
            }
            DepartureCity newDepartureCity = new DepartureCity(departureCity, departureCityId);
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return newDepartureCity;
        }

        public static void DeleteAll()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM departure_cities;";
            cmd.ExecuteNonQuery();
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }
    }
}
