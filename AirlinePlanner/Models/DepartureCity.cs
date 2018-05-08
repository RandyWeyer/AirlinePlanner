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

        public void Save()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO departure_cities (departure_city) VALUES (@departure_city);";

            MySqlParameter departureCity = new MySqlParameter();
            departureCity.ParameterName = "@departure_city";
            departureCity.Value = this._departureCity;
            cmd.Parameters.Add(departureCity);

            cmd.ExecuteNonQuery();
            _id = (int) cmd.LastInsertedId;
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
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
//read input to join-table
        public List<ArrivalCity> GetArrivals()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT arrival_cities.* FROM departure_cities
                JOIN flights ON (departure_cities.id = flights.departure_city_id)
                JOIN arrival_cities ON (flights.arrival_city_id = arrival_cities.id)
                WHERE departure_cities.id = @DepartureCityId;";

            MySqlParameter departureCityIdParameter = new MySqlParameter();
            departureCityIdParameter.ParameterName = "@DepartureCityId";
            departureCityIdParameter.Value = _id;
            cmd.Parameters.Add(departureCityIdParameter);

            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
            List<ArrivalCity> arrivalCities = new List<ArrivalCity>{};

            while(rdr.Read())
            {
              int arrivalCityId = rdr.GetInt32(0);
              string arrivalCityName = rdr.GetString(1);
              ArrivalCity newArrivalCity = new ArrivalCity(arrivalCityName, arrivalCityId);
              arrivalCities.Add(newArrivalCity);
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return arrivalCities;
        }
//saves input to join table
        public void SetArrivals(ArrivalCity newArrivalCity)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO flights (arrival_city_id, departure_city_id) VALUES (@ArrivalCityId, @DepartureCityId);";

            MySqlParameter arrival_city_id = new MySqlParameter();
            arrival_city_id.ParameterName = "@ArrivalCityId";
            arrival_city_id.Value = newArrivalCity.GetId();
            cmd.Parameters.Add(arrival_city_id);

            MySqlParameter departure_city_id = new MySqlParameter();
            departure_city_id.ParameterName = "@DepartureCityId";
            departure_city_id.Value = _id;
            cmd.Parameters.Add(departure_city_id);

            cmd.ExecuteNonQuery();
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
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
