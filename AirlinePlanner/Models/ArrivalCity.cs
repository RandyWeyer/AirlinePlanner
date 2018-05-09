using System.Collections.Generic;
using MySql.Data.MySqlClient;
using AirlinePlanner;
using System;
using Microsoft.AspNetCore.Mvc;

namespace AirlinePlanner.Models
{
    public class ArrivalCity
    {
        private string _arrivalCityName;
        private int _id;

        public ArrivalCity(string arrivalCityName, int id=0)
        {
            _arrivalCityName = arrivalCityName;
            _id = id;
        }

        public string GetArrivalCityName()
        {
            return _arrivalCityName;
        }
        public void SetArrivalCityName(string ArrivalCityName)
        {
            _arrivalCityName = ArrivalCityName;
        }

        public int GetId()
        {
            return _id;
        }


        public override bool Equals(System.Object otherArrivalCity)
        {
          if (!(otherArrivalCity is ArrivalCity))
          {
            return false;
          }
          else
          {
             ArrivalCity newArrivalCity = (ArrivalCity) otherArrivalCity;
             bool idEquality = this.GetId() == newArrivalCity.GetId();
             bool nameEquality = this.GetArrivalCityName() == newArrivalCity.GetArrivalCityName();
             return (idEquality && nameEquality);
           }
        }
        public override int GetHashCode()
        {
             return this.GetArrivalCityName().GetHashCode();
        }

        public void Save()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO arrival_cities (arrival_city) VALUES (@arrival_city);";

            MySqlParameter arrivalCity = new MySqlParameter();
            arrivalCity.ParameterName = "@arrival_city";
            arrivalCity.Value = this._arrivalCityName;
            cmd.Parameters.Add(arrivalCity);

            cmd.ExecuteNonQuery();
            _id = (int) cmd.LastInsertedId;
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public static List<ArrivalCity> GetAll()
        {
            List<ArrivalCity> allArrivalCities = new List<ArrivalCity> {};
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM arrival_cities;";
            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            while(rdr.Read())
            {
              int id = rdr.GetInt32(0);
              string arrivalCityName = rdr.GetString(1);
              ArrivalCity newArrivalCity = new ArrivalCity(arrivalCityName, id);
              allArrivalCities.Add(newArrivalCity);
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return allArrivalCities;
        }

        public List<Flight> GetInboundFlights()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            // cmd.CommandText = @"SELECT departure_cities.* FROM arrival_cities
            //     JOIN flights ON (arrival_cities.id = flights.arrival_city_id)
            //     JOIN departure_cities ON (flights.departure_city_id = departure_cities.id)
            //     WHERE arrival_cities.id = @ArrivalCityId;";

            cmd.CommandText = @"SELECT * FROM flights WHERE arrival_city_id = @ThisCityId;";


            MySqlParameter arrivalCityIdParameter = new MySqlParameter();
            arrivalCityIdParameter.ParameterName = "@ThisCityId";
            arrivalCityIdParameter.Value = _id;
            cmd.Parameters.Add(arrivalCityIdParameter);

            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
            List<Flight> inboundFlights = new List<Flight>{};

            while(rdr.Read())
            {
              int flightId = rdr.GetInt32(0);
              int departureCityId = rdr.GetInt32(1);
              int arrivalCityId = rdr.GetInt32(2);
              Flight newInboundFlight = new Flight(departureCityId, arrivalCityId, flightId);
              inboundFlights.Add(newInboundFlight);
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return inboundFlights;
        }


        public void SetDepartures(DepartureCity newDepartureCity)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO flights (arrival_city_id, departure_city_id) VALUES (@ArrivalCityId, @DepartureCityId);";

            MySqlParameter departure_city_id = new MySqlParameter();
            departure_city_id.ParameterName = "@DepartureCityId";
            departure_city_id.Value = newDepartureCity.GetId();
            cmd.Parameters.Add(departure_city_id);

            MySqlParameter arrival_city_id = new MySqlParameter();
            arrival_city_id.ParameterName = "@ArrivalCityId";
            arrival_city_id.Value = _id;
            cmd.Parameters.Add(arrival_city_id);

            cmd.ExecuteNonQuery();
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public static ArrivalCity Find(int arrivalId)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM arrival_cities WHERE id = (@searchId);";

            MySqlParameter searchId = new MySqlParameter();
            searchId.ParameterName = "@searchId";
            searchId.Value = arrivalId;
            cmd.Parameters.Add(searchId);

            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            int id = 0;
            string arrivalCityName = "";

            while(rdr.Read())
            {
              id = rdr.GetInt32(0);
              arrivalCityName = rdr.GetString(1);
            }
            ArrivalCity newArrivalCity = new ArrivalCity(arrivalCityName, id);
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return newArrivalCity;
        }

        public static void DeleteAll()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM arrival_cities;";
            cmd.ExecuteNonQuery();
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }
    }
}
