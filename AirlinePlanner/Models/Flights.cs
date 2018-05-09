using System.Collections.Generic;
using MySql.Data.MySqlClient;
using AirlinePlanner;
using System;
using Microsoft.AspNetCore.Mvc;

namespace AirlinePlanner.Models
{
    public class Flight
    {
        private int _departureCityId;
        private int _arrivalCityId;
        private int _id;

        public Flight(int departureCityId, int arrivalCityId, int id=0)
        {
            _departureCityId = departureCityId;
            _arrivalCityId = arrivalCityId;
            _id = id;
        }

        public int GetFlightId()
        {
            return _id;
        }
        public int GetDepartureCityId()
        {
            return _departureCityId;
        }
        public int GetArrivalCityId()
        {
            return _arrivalCityId;
        }

        public override bool Equals(System.Object otherFlight)
        {
          if (!(otherFlight is Flight))
          {
            return false;
          }
          else
          {
             Flight newFlight = (Flight) otherFlight;
             bool idEquality = this.GetFlightId() == newFlight.GetFlightId();
             bool idDepartureEquality = this.GetDepartureCityId() == newFlight.GetDepartureCityId();
             bool idArrivalEquality = this.GetArrivalCityId() == newFlight.GetArrivalCityId();
             return (idEquality && idDepartureEquality && idArrivalEquality);
           }
        }
        public override int GetHashCode()
        {
             return this.GetFlightId().GetHashCode();
        }

        //deletes from join table
        public void DeleteFlight()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM flights WHERE flight_id = @FlightID;";

            MySqlParameter flightIdParameter = new MySqlParameter();
            flightIdParameter.ParameterName = "@FlightId";
            flightIdParameter.Value = _id;
            cmd.Parameters.Add(flightIdParameter);

            cmd.ExecuteNonQuery();
            if (conn != null)
            {
              conn.Close();
            }

            conn.Dispose();
        }

        public static Flight Find(int flightId)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM flights WHERE flight_id = (@searchId);";

            MySqlParameter searchId = new MySqlParameter();
            searchId.ParameterName = "@searchId";
            searchId.Value = flightId;
            cmd.Parameters.Add(searchId);

            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            int id = 0;
            int arrivalCityId = 0;
            int departureCityId = 0;

            while(rdr.Read())
            {
              id = rdr.GetInt32(0);
              arrivalCityId = rdr.GetInt32(1);
              departureCityId = rdr.GetInt32(2);
            }
            Flight newFlight= new Flight(arrivalCityId, departureCityId, id);
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return newFlight;
        }
    }
}
